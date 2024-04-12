using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Utility;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using WxPayAPI;
using System.IO;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.TenPayLibV3;
using System.Text;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class PayController : NeedLoginController
    {
        public UnitOfWork work = new UnitOfWork();
        private static TenPayV3Info _tenPayV3Info;
        public static TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[WebSiteConfig.WeixinMCHID];
                }
                return _tenPayV3Info;
            }
        }
        JsonResult json = new JsonResult
        {
            Data = new { }
        };
        public ActionResult Index(int ID = 0)
        {
            if (ID > 0)
            {
                var rst = work.OrderRepository.Get(m => m.UserID == LoginedUserModel.ID && m.ID == ID);

                ViewBag.Orders = rst.ToList();
                if (rst.Count() < 1)
                {
                    Response.Redirect("/Mobile/");
                }
                return View(rst.FirstOrDefault<Order>());
            }
            Response.Redirect("/Mobile/");
            return View();
        }

        #region 支付专用发票 税点金额

        /// <summary>
        /// 支付专用发票 税点金额
        /// </summary>
        /// <returns></returns>
        public ActionResult Invoice(int ID = 0)
        {
            if (ID > 0)
            {
                InvoiceLog existModel = work.InvoiceLogRepository.Get(m => m.UserID == LoginedUserModel.ID && m.ID == ID).FirstOrDefault();
                return View(existModel);
            }
            else
            {
                Response.Redirect("/Mobile/Member/Invoice");
                Response.End();
                return null;
            }
        }
        #endregion

        #region 支付
        /// <summary>
        /// 支付
        /// </summary>
        /// <returns></returns>
        public ActionResult Pay(decimal pay_amount = 0, string payway = "", string order_no = "")
        {
            if (pay_amount > 0)
            {
                if (string.IsNullOrEmpty(order_no))
                {
                    order_no = OrderService.GetOrderNo(0, LoginedUserModel.ID, "02");
                }

                if (payway == "alipay")//支付宝
                {
                    Response.Redirect(string.Format("/alipay_utf8/pay.aspx?trade_no={0}&total_fee={1}", order_no, pay_amount));
                }
                else if (payway == "wxpay")//微信
                {
                    if (UtilityClass.IsMobile())
                    {
                        string userAgent = Request.UserAgent;
                        if (userAgent.ToLower().Contains("micromessenger"))
                        {
                            //JsApiPay必须回主站支付 -2018-05-10
                            //return RedirectToAction("JsApiPay", new { trade_no = order_no, total_fee = pay_amount });
                            //return RedirectToAction("QRCodePay", new { trade_no = order_no, total_fee = pay_amount });
                            Response.Redirect(string.Format("{0}/Mobile/Pay/JsApiPay?trade_no={1}&total_fee={2}&host={3}", WebSiteConfig.WebSiteDomain, order_no, pay_amount, Request.Url.Host.ToLower()));
                            Response.End();
                        }
                        else
                        {
                            return RedirectToAction("H5ApiPay", new { trade_no = order_no, total_fee = pay_amount });
                        }
                    }
                    else
                    {
                        //Response.Redirect(string.Format("/wxpay/JsApiPayPage.aspx?trade_no={0}&total_fee={1}", order_no, pay_amount));
                        return RedirectToAction("QRCodePay", new { trade_no = order_no, total_fee = pay_amount });
                    }
                }
                else if (payway == "yue")
                {
                    User user = UserService.GetModel(LoginedUserModel.ID);
                    if (user.U_Amount < pay_amount)
                    {
                        return RedirectToAction("Error", new { msg = HttpUtility.UrlEncode("账户余额不足！<a href='/Mobile/Member/Recharge' class='btn btn-danger btn-sm'>充值</a>") });
                    }
                    else
                    {
                        bool do_success = OrderService.PaySuccessToDo(order_no, order_no, pay_amount, 3);
                        if (do_success)
                        {
                            return RedirectToAction("Success", new { trade_no = order_no, total_fee = pay_amount });
                        }
                        else
                        {
                            return RedirectToAction("Error", new { msg = HttpUtility.UrlEncode("账户余额不足！<a href='/Mobile/Member/Recharge' class='btn btn-danger btn-sm'>充值</a>") });
                        }
                    }

                }
            }
            else
            {
                Response.Redirect("/Mobile/");
            }
            return View();
        }

        #endregion

        #region  JsApiPay

        /// <summary>
        /// JsApiPay
        /// </summary>
        /// <returns></returns>
        public ActionResult JsApiPay(string trade_no = "", decimal total_fee = 0, string host = "")
        {
            ViewBag.trade_no = trade_no;
            ViewBag.total_fee = total_fee;
            ViewBag.wxJsApiParam = "";
            //实际操作站点主机名
            ViewBag.host = host;

            string wxJsApiParam = "";
            string openid = CookieHelper.GetValue("openid");// Request.QueryString["openid"];
            string out_trade_no = trade_no;
            decimal pay_total_fee = 0;


            if (string.IsNullOrEmpty(openid))
            {
                string state = string.Format("{0}/Mobile/Pay/JsApiPay?trade_no={1}&total_fee={2}&host={3}", WebSiteConfig.WebSiteDomain, trade_no, total_fee, host);
                //Response.Redirect("");
                return RedirectToAction("GotoOauth", "WeiXin", new { state = Server.UrlEncode(state) });
            }

            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(openid) || total_fee == 0 || string.IsNullOrEmpty(out_trade_no))
            {
                ViewBag.MessageInfo = "<span style='color:#FF0000;'>" + "页面传参出错,请返回重试" + "</span>";
                WxPayAPI.Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
            }
            string order_type = OrderService.GetOrderTypeByOrderNo(trade_no);
            if (order_type == "01")//在线支付
            {
                //在线支付总金额=订单总金额-现金券
                Order orderModel = OrderService.GetModelByOrderNo(trade_no);

                if (orderModel != null)
                {
                    pay_total_fee = orderModel.O_PayAmount;
                    if (pay_total_fee < 0)
                    {
                        ViewBag.MessageInfo = "<span style='color:#FF0000;'>" + "订单支付失败！(UpdateShQuanAndReturnTotalFee)" + "</span>";
                        WxPayAPI.Log.Error(this.GetType().ToString(), "订单支付失败！(UpdateShQuanAndReturnTotalFee:" + out_trade_no + ")");
                    }
                }
            }
            else
            {
                //充值，重新生成订单号--20171101改成统一支付后面追加4位数
                //out_trade_no = OrderService.GetOrderNo(0, LoginedUserModel.ID, "02");

                pay_total_fee = Convert.ToDecimal(total_fee);
                //if (pay_total_fee < 200)
                //{
                //    Response.Write("<span style='color:#FF0000;'>" + "充值失败，充值金额需不少于200元！" + "</span>");
                //    Log.Error(this.GetType().ToString(), "充值失败，充值金额需不少于200元");
                //    submit.Visible = false;
                //    return;
                //}
            }
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(new System.Web.UI.Page());
            jsApiPay.openid = openid;
            jsApiPay.total_fee = Convert.ToInt32(pay_total_fee * 100);
            jsApiPay.out_trade_no = out_trade_no;
            jsApiPay.body = "微信支付" + out_trade_no;
            jsApiPay.attach = out_trade_no;


            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                WxPayAPI.Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                ViewBag.wxJsApiParam = wxJsApiParam;

            }
            catch (Exception ex)
            {
                ViewBag.MessageInfo = "<span style='color:#FF0000;'>" + "下单失败，请返回重试" + ex.Message + "</span>";
            }
            return View();
        }

        /// <summary>
        /// 获得js支付API参数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJsApiPayParam(string trade_no = "", decimal total_fee = 0)
        {
            string wxJsApiParam = "";
            string openid = CookieHelper.GetValue("openid");
            string out_trade_no = trade_no;
            decimal pay_total_fee = Convert.ToDecimal(total_fee);

            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(out_trade_no))
            {
                out_trade_no = OrderService.GetOrderNo(0, LoginedUserModel.ID, "02");
            }

            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(openid) || pay_total_fee == 0)
            {
                WxPayAPI.Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
                json.Data = new { status = "-1", msg = "页面传参出错,请返回重试" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(new System.Web.UI.Page());
            jsApiPay.openid = openid;
            jsApiPay.total_fee = Convert.ToInt32(pay_total_fee * 100);
            jsApiPay.out_trade_no = out_trade_no;
            jsApiPay.body = "微信支付" + out_trade_no;
            jsApiPay.attach = out_trade_no;

            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                WxPayAPI.Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);

                json.Data = new { status = "0", msg = "请求支付成功", data = wxJsApiParam };
            }
            catch (Exception ex)
            {
                json.Data = new { status = "-1", msg = "请求支付失败,请返回重试" };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  H5ApiPay

        /// <summary>
        /// H5ApiPay
        /// </summary>
        /// <returns></returns>
        public ActionResult H5ApiPay(string trade_no = "", decimal total_fee = 0)
        {
            ViewBag.trade_no = trade_no;
            ViewBag.total_fee = total_fee;
            ViewBag.wxJsApiParam = "";
            ViewBag.mwebUrl = "";

            string openid = "";// H5浏览器过来没有openid
            string out_trade_no = trade_no;
            decimal pay_total_fee = 0;
            //检测是否给当前页面传递了相关参数
            if (total_fee == 0 || string.IsNullOrEmpty(out_trade_no))
            {
                ViewBag.MessageInfo = "<span style='color:#FF0000;'>" + "页面传参出错,请返回重试" + "</span>";
                WxPayAPI.Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
            }
            string order_type = OrderService.GetOrderTypeByOrderNo(trade_no);
            if (order_type == "01")//在线支付
            {
                //在线支付总金额=订单总金额-现金券
                Order orderModel = OrderService.GetModelByOrderNo(trade_no);

                if (orderModel != null)
                {
                    pay_total_fee = orderModel.O_PayAmount;
                    if (pay_total_fee < 0)
                    {
                        ViewBag.MessageInfo = "<span style='color:#FF0000;'>" + "订单支付失败！(UpdateShQuanAndReturnTotalFee)" + "</span>";
                        WxPayAPI.Log.Error(this.GetType().ToString(), "订单支付失败！(UpdateShQuanAndReturnTotalFee:" + out_trade_no + ")");
                    }
                }
            }
            else
            {
                //充值，重新生成订单号-20171101改成统一支付后面追加4位数
                //out_trade_no = OrderService.GetOrderNo(0, LoginedUserModel.ID, "02");

                pay_total_fee = Convert.ToDecimal(total_fee);
                //if (pay_total_fee < 200)
                //{
                //    Response.Write("<span style='color:#FF0000;'>" + "充值失败，充值金额需不少于200元！" + "</span>");
                //    Log.Error(this.GetType().ToString(), "充值失败，充值金额需不少于200元");
                //    submit.Visible = false;
                //    return;
                //}
            }
            ////若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            //JsApiPay jsApiPay = new JsApiPay(new System.Web.UI.Page());
            //jsApiPay.openid = openid;
            //jsApiPay.total_fee = Convert.ToInt32(pay_total_fee * 100);
            //jsApiPay.out_trade_no = out_trade_no;
            //jsApiPay.body = "微信支付" + out_trade_no;
            //jsApiPay.attach = out_trade_no;


            ////H5支付(MWEB)预处理
            //try
            //{
            //    var returnUrl = string.Format("http://www.yinxiamall.com/Pay/Success?trade_no={0}&total_fee={1}", out_trade_no, pay_total_fee);
            //    ViewBag.mwebUrl = jsApiPay.GetUnifiedOrderResultH5() + "&redirect_url=" + Server.UrlEncode(returnUrl);
            //    WxPayAPI.Log.Debug(this.GetType().ToString(), "mwebUrl : " + ViewBag.mwebUrl);
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.MessageInfo = "<span style='color:#FF0000;'>" + "下单失败，请返回重试" + ex.Message + "</span>";
            //}

            try
            {
                string openId = null;//此时在外部浏览器，无法或得到OpenId

                string sp_billno = out_trade_no + DateTime.Now.ToString("mmss");// 微信订单号增加随机位数4位尾数
                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();
                var body = "在线支付";
                var price = Convert.ToInt32(pay_total_fee * 100);
                var xmlDataInfo = new TenPayV3UnifiedorderRequestData(TenPayV3Info.AppId, TenPayV3Info.MchId, body, sp_billno, price, Request.UserHostAddress, TenPayV3Info.TenPayV3Notify, TenPayV3Type.MWEB/*此处无论传什么，方法内部都会强制变为MWEB*/, openId, TenPayV3Info.Key, nonceStr);
                var urlFormat = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                xmlDataInfo.TradeType = TenPayV3Type.MWEB;

                var data = xmlDataInfo.PackageRequestHandler.ParseXML();//获取XML
                //throw new Exception(data.HtmlEncode());
                MemoryStream ms = new MemoryStream();
                var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
                ms.Write(formDataBytes, 0, formDataBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置

                var resultXml = RequestUtility.HttpPost(urlFormat, null, ms, timeOut: 10000);
                var result = new UnifiedorderResult(resultXml);
                ViewBag.MessageInfo = resultXml;
                //var result = TenPayV3.Html5Order(xmlDataInfo);//调用统一订单接口

                //JsSdkUiPackage jsPackage = new JsSdkUiPackage(TenPayV3Info.AppId, timeStamp, nonceStr,);

                /*
                 * result:{"device_info":"","trade_type":"MWEB","prepay_id":"wx20170810143223420ae5b0dd0537136306","code_url":"","mweb_url":"https://wx.tenpay.com/cgi-bin/mmpayweb-bin/checkmweb?prepay_id=wx20170810143223420ae5b0dd0537136306\u0026package=1505175207","appid":"wx669ef95216eef885","mch_id":"1241385402","sub_appid":"","sub_mch_id":"","nonce_str":"juTchIZyhXvZ2Rfy","sign":"5A37D55A897C854F64CCCC4C94CDAFE3","result_code":"SUCCESS","err_code":"","err_code_des":"","return_code":"SUCCESS","return_msg":null}
                 */
                //return Json(result, JsonRequestBehavior.AllowGet);
                //var package = string.Format("prepay_id={0}", result.prepay_id);
                //ViewData["paySign"] = TenPayV3.GetJsPaySign(TenPayV3Info.AppId, timeStamp, nonceStr, package, TenPayV3Info.Key);

                //设置成功页面（也可以不设置，支付成功后默认返回来源地址）
                var returnUrl = string.Format("http://www.yinxiamall.com/Pay/Success?trade_no={0}&total_fee={1}", out_trade_no, pay_total_fee);

                ViewBag.mwebUrl = result.mweb_url + string.Format("&redirect_url={0}", returnUrl.AsUrlData());

            }
            catch (Exception ex)
            {
                ViewBag.MessageInfo = "<span>" + "下单失败，请返回重试" + ex.Message + "</span>";
            }
            return View();
        }

        #endregion

        #region 二维码支付

        /// <summary>
        /// 二维码支付
        /// </summary>
        /// <returns></returns>
        public ActionResult QRCodePay(string trade_no = "", decimal total_fee = 0)
        {
            ViewBag.trade_no = trade_no;
            ViewBag.total_fee = total_fee;
            ViewBag.QRCodeImage = "";

            WxPayAPI.Log.Info(this.GetType().ToString(), "Native pay mode 2 url is producing...");

            if (!string.IsNullOrEmpty(trade_no))
            {
                trade_no = trade_no.ToLower();
            }
            else
            {
                trade_no = OrderService.GetOrderNo(0, LoginedUserModel.ID, "02");
                ViewBag.trade_no = trade_no;
            }

            WxPayData data = new WxPayData();

            string order_type = OrderService.GetOrderTypeByOrderNo(trade_no);
            if (order_type == "01")//在线支付
            {
                Order orderModel = OrderService.GetModelByOrderNo(trade_no);

                if (orderModel != null)
                {
                    data.SetValue("body", "在线支付");//商品描述
                    data.SetValue("attach", "微信");//附加数据
                    data.SetValue("out_trade_no", trade_no);//随机字符串//WxPayApi.GenerateOutTradeNo()
                    data.SetValue("total_fee", Convert.ToInt32(total_fee * 100));//总金额
                    data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
                    data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
                    data.SetValue("goods_tag", orderModel.ID);//商品标记
                    data.SetValue("trade_type", "NATIVE");//交易类型
                    data.SetValue("product_id", orderModel.ID);//商品ID

                }
            }
            else//充值
            {

                data.SetValue("body", "充值");//商品描述
                data.SetValue("attach", "微信");//附加数据
                data.SetValue("out_trade_no", trade_no);//随机字符串//WxPayApi.GenerateOutTradeNo()
                data.SetValue("total_fee", Convert.ToInt32(total_fee * 100));//总金额
                data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
                data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
                data.SetValue("goods_tag", "chongzhi");//商品标记
                data.SetValue("trade_type", "NATIVE");//交易类型
                data.SetValue("product_id", "100");//商品ID
            }
            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            string pay_url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接

            WxPayAPI.Log.Info(this.GetType().ToString(), "Get native pay mode 2 url : " + pay_url);

            ViewBag.QRCodeImage = pay_url;
            return View();
        }

        #endregion

        #region 支付成功 + 失败

        /// <summary>
        /// 支付成功
        /// </summary>
        /// <returns></returns>
        public ActionResult Success(string trade_no = "", decimal total_fee = 0)
        {
            ViewBag.trade_no = trade_no;
            ViewBag.total_fee = total_fee;

            var orderList = work.Context.Orders
               .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
               .Where(m => m.o.O_OrderNo == trade_no && m.od.OD_IsHasDesignFile == 1)
               .Select(m => m.od).ToList();

            ViewBag.orderList = orderList;

            return View();
        }

        /// <summary>
        /// 支付失败
        /// </summary>
        /// <returns></returns>
        public ActionResult Error(string msg)
        {
            ViewBag.MessageInfo = msg;
            return View();
        }

        #endregion

    }
}