using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Threading;
using LitJson;
using System.Web.Security;

using WxPayAPI;

public partial class JsApiPayPage : System.Web.UI.Page
{
    public static string wxJsApiParam { get; set; } //H5调起JS API参数
    public decimal pay_total_fee = 0;
    public string out_trade_no;
    protected void Page_Load(object sender, EventArgs e)
    {
        Log.Info(this.GetType().ToString(), "page load");
        if (!IsPostBack)
        {
            ////if ("test123" == Visitor.UserLoginId || "wdd354" == Visitor.UserLoginId)
            ////{
            ////    submit.Visible = true;
            ////}
            ////else
            ////{
            ////    submit.Visible = false;
            ////}
            //string openid = Visitor.OpenId;// Request.QueryString["openid"];
            //string total_fee = Request.QueryString["total_fee"];
            //out_trade_no = Request.QueryString["trade_no"];
            ////检测是否给当前页面传递了相关参数
            //if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(total_fee) || string.IsNullOrEmpty(out_trade_no))
            //{
            //    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
            //    Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
            //    submit.Visible = false;
            //    return;
            //}
            //pay_total_fee = 0;
            //if (!out_trade_no.ToLower().Contains("cz"))//如果不是充值
            //{
            //    //在线支付总金额=订单总金额-现金券
            //    pay_total_fee = WangZhicn.Control.ShopHistory.UpdateShQuanAndReturnTotalFee(out_trade_no, Visitor);
            //    if (pay_total_fee == -1)
            //    {
            //        Response.Write("<span style='color:#FF0000;font-size:20px'>" + "订单支付失败！(UpdateShQuanAndReturnTotalFee)" + "</span>");
            //        Log.Error(this.GetType().ToString(), "订单支付失败！(UpdateShQuanAndReturnTotalFee:" + out_trade_no + ")");
            //        submit.Visible = false;
            //        return;
            //    }
            //}
            //else
            //{
            //    //充值，重新生成订单号
            //    out_trade_no = WangZhicn.Control.UserInfo.GenerateOutTradeNo("CZ");

            //    pay_total_fee = Convert.ToDecimal(total_fee);
            //    //if (pay_total_fee < 200)
            //    //{
            //    //    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "充值失败，充值金额需不少于200元！" + "</span>");
            //    //    Log.Error(this.GetType().ToString(), "充值失败，充值金额需不少于200元");
            //    //    submit.Visible = false;
            //    //    return;
            //    //}
            //}
            ////若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            //JsApiPay jsApiPay = new JsApiPay(this);
            //jsApiPay.openid = openid;
            //jsApiPay.total_fee = Convert.ToInt32(pay_total_fee * 100);
            //jsApiPay.out_trade_no = WangZhicn.Control.ShopHistory.ConvertOrderNoToTradeNo(out_trade_no);
            //jsApiPay.body = "微信支付" + out_trade_no;
            //jsApiPay.attach = out_trade_no;


            ////JSAPI支付预处理
            //try
            //{
            //    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
            //    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
            //    Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
            //    //在页面上显示订单信息
            //    //Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
            //    //Response.Write("<span style='color:#00CD00;font-size:20px'>" + unifiedOrderResult.ToPrintStr() + "</span>");

            //}
            //catch (Exception ex)
            //{
            //    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + ex.Message + "</span>");
            //    submit.Visible = false;
            //}
        }
    }

}