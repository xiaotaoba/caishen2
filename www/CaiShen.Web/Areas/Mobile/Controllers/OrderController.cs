using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Utility;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using System.Text;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class OrderController : NeedLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        //private UserShop cuurentUserShopModel = UserShopService.GetCurrentShop();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        #region 进入+编辑地址

        public ActionResult Index(string cart = "", string at = "", int aid = 0, string info = "")
        {
            if (cart == "")
            {
                cart = CookieHelper.GetValue("orderCartIds");
                if (string.IsNullOrEmpty(cart))
                {
                    Response.Redirect("/Mobile/");
                    Response.End();
                }
            }
            else
            {
                CookieHelper.SetValue("orderCartIds", cart, 3600);
            }
            //InitData(null, cart, at, aid);
            ViewBag.cart = cart;
            ViewBag.at = at;
            ViewBag.aid = aid;

            if (!string.IsNullOrEmpty(info))
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("error", info); ;
            }

            #region 下单商品列表

            Int32[] cartids = CartService.GetCartIds(cart);
            ViewBag.IsZiti = OrderService.IsZiti(cartids, CurrentShopModel);

            List<CartVModel> cartList = CartService.GetCartVList(cartids);
            ViewBag.CartList = cartList;

            //未选择购买商品
            if (cartList.Count < 1)
            {
                Response.Redirect("/Mobile/");
            }

            #endregion

            #region 收货地址列表

            List<UserAddress> userAddressList = work.UserAddressRepository.Get(m => m.UserID == LoginedUserModel.ID).OrderByDescending(m => m.Is_Default).ThenByDescending(m => m.Time).ToList();
            ViewBag.Address = userAddressList;
            if (userAddressList != null && userAddressList.Count() > 0)
            {
                if (userAddressList.Where(m => m.Is_Default == 1).Count() > 0)//存在默认
                {
                    ViewBag.defaultAddress = userAddressList.Where(m => m.Is_Default == 1).FirstOrDefault();
                }
                else
                {
                    //默认第一个
                    ViewBag.defaultAddress = userAddressList.FirstOrDefault();
                }
            }

            #endregion

            #region 默认显示运费

            if (ViewBag.defaultAddress != null)
            {
                UserAddress defaultAddressModel = ViewBag.defaultAddress;
                ViewBag.post_fee = OrderService.GetShippingFee(defaultAddressModel.ID, cartids, CurrentShopModel);
            }
            else
            {
                ViewBag.post_fee = 0;
            }

            #endregion

            #region 编辑地址

            if (at != "")
            {
                ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1).ToList();
            }

            UserAddress userAddress = null;
            if (aid != 0)
            {
                userAddress = work.UserAddressRepository.GetByID(aid);
            }
            if (userAddress != null)
            {
                List<Area> listArea = work.AreaRepository.Get(m => m.Area_ParentID == userAddress.Province || m.Area_ParentID == userAddress.City).ToList();
                ViewBag.AddressModel = userAddress;
                ViewBag.Citys = listArea.Where(m => m.Area_ParentID == userAddress.Province).ToList();
                ViewBag.Regions = listArea.Where(m => m.Area_ParentID == userAddress.City).ToList();
            }

            #endregion

            //优惠券列表
            ViewBag.CouponList = CouponService.GetListOfOrder(CurrentShopModel.ID, LoginedUserModel.ID, CartService.GetGoodsList(cartids), CartService.GetTotalGoodsAmount(cartList));
            //红包列表
            ViewBag.UserHongBaoList = UserHongBaoService.GetListOfOrder(LoginedUserModel.ID, CurrentShopModel.ID);

            return View();
        }

        #endregion

        #region 保存收货人信息

        [HttpPost]
        public ActionResult SaveAdd(UserAddress userAddress, string cart = "", string at = "", int aid = 0, string AddressPrefix = "")
        {
            string errorInfo = "";

            #region 验证表单

            if (string.IsNullOrEmpty(userAddress.UserName))
            {
                errorInfo = "请输入收货人";
                return RedirectToAction("Index", new { cart, at, aid, info = errorInfo });
            }
            if (userAddress.Province == 0 || userAddress.City == 0)//|| userAddress.Region == 0
            {
                errorInfo = "请选择完整地区";
                return RedirectToAction("Index", new { cart, at, aid, info = errorInfo });
            }
            if (string.IsNullOrEmpty(userAddress.Address))
            {
                errorInfo = "请输入详细地址";
                return RedirectToAction("Index", new { cart, at, aid, info = errorInfo });
            }
            if (string.IsNullOrEmpty(userAddress.Tel) && string.IsNullOrEmpty(userAddress.Mobile))
            {
                errorInfo = "手机号码和电话至少填写一项";
                return RedirectToAction("Index", new { cart, at, aid, info = errorInfo });
            }
            //if (string.IsNullOrEmpty(userAddress.Post_Code))
            //{
            //    errorInfo = "请输入邮编";
            //}

            //StringBuilder errinfo = new StringBuilder();
            //foreach (var s in ModelState.Values)
            //{
            //    foreach (var p in s.Errors)
            //    {
            //        errinfo.AppendFormat("{0}\\n", p.ErrorMessage);
            //    }
            //}

            #endregion

            userAddress.UserID = LoginedUserModel.ID;
            if (!string.IsNullOrEmpty(userAddress.Address))
            {
                userAddress.Address = AddressPrefix + " " + userAddress.Address.Replace(" ", "");
            }

            if (at == "add")
            {
                work.UserAddressRepository.Insert(userAddress);
                work.Save();
            }
            else if (at == "edit")
            {
                //userAddress.ID = aid;
                work.UserAddressRepository.Update(userAddress);
                work.Save();
            }

            //更新默认设置
            if (userAddress.Is_Default == 1)
            {
                UserAddressService.SetDefault(LoginedUserModel.ID, userAddress.ID);
            }
            //work.Dispose();
            //return JavaScript("alert('请输入验证码')");
            return RedirectToAction("Index");
        }

        #endregion

        #region 删除收货人地址

        /// <summary>
        /// 删除收货人地址
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        public ActionResult DeleteAdd(int aid)
        {
            UserAddressService.Delete(aid);
            return RedirectToAction("Index");
        }

        #endregion

        #region 创建订单
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="newVOrder"></param>
        /// <param name="user_address_selected">用户已选地址文本</param>
        /// <param name="userAddressID">地址ID</param>
        /// <param name="discount_amount_coupon">优惠券金额</param>
        /// <param name="discount_amount_hongbao">红包金额</param>
        /// <param name="coupon">用户优惠券ID</param>
        /// <param name="hongbao">用户红包ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrder(OrderPayModel newVOrder, string user_address_selected = "", decimal discount_amount_coupon = 0, decimal discount_amount_hongbao = 0, string payway_name = "alipay", int ispurchase = 0)
        {
            //try
            //{
            if (ModelState.IsValid)
            {
                if (LoginedUserModel == null)
                {
                    Response.Redirect("/mobile/Login");
                }

                string cart = CookieHelper.GetValue("orderCartIds");
                if (string.IsNullOrEmpty(cart))
                {
                    Response.Redirect("/Mobile/");
                }

                Order orderModel = OrderService.CreateOrder(LoginedUserModel, CurrentShopModel, newVOrder, cart, user_address_selected, discount_amount_coupon, discount_amount_hongbao, "mobile", ispurchase);
                //return RedirectToAction("Index", "Pay", new { ID = order.ID });
                //2017-12-06 调整，手机直接转到支付，不显示直接页面
                return RedirectToAction("Pay", "Pay", new { pay_amount = orderModel.O_PayAmount, payway = payway_name, order_no = orderModel.O_OrderNo });
            }
            //}
            //catch (Exception e)
            //{
            //    return RedirectToAction("Index", new { info = e.Message });
            //}
            return RedirectToAction("Index");
        }

        #endregion

        #region 获取(计算)订单价格统计信息 -json

        /// <summary>
        /// 获取(计算)订单价格统计信息
        /// </summary>
        /// <param name="addressid"></param>
        /// <param name="shipping_way"></param>
        /// <param name="payway"></param>
        /// <param name="coupon">优惠券金额</param>
        /// <param name="hongbao">红包金额</param>
        /// <returns></returns>
        public ActionResult GetOrderTongJiJson(int addressid, int shipping_way, int payway, decimal coupon = 0, decimal hongbao = 0)
        {
            json.Data = OrderService.GetOrderTongJiJsonObject(LoginedUserModel, CurrentShopModel, addressid, shipping_way, payway, coupon, hongbao);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}