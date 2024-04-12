using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Utility;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using PagedList;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class CartController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        /// <summary>
        /// 购物车列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1)
        {
            if (LoginedUserModel == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.page = page;
            ViewBag.currentShop = CurrentShopModel;
            int shopID = CurrentShopModel.ID;

            List<CartVModel> listV = CartService.GetCartVList(LoginedUserModel.ID, shopID);

            ViewBag.cartCount = listV.Select(m => m.Cart.Count).Sum();

            int pageSize = 12;
            return View(listV.ToPagedList(page, pageSize));
        }

        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(Cart cart)
        {
            json.Data = CartService.Add(LoginedUserModel, cart);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 立即购买，如果购物车有就不再加入
        /// </summary>
        /// <returns></returns>
        public ActionResult BuyNowAdd(Cart cart)
        {
            json.Data = CartService.BuyNowAdd(LoginedUserModel, cart);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 购物车修改数量
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateCount(int id, int count)
        {
            json.Data = CartService.UpdateCount(LoginedUserModel, id, count);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 从购物车删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int ID = 0)
        {
            if (LoginedUserModel == null)
            {
                return RedirectToAction("Index", "Login");
            }
            CartService.Delete(ID);
            return RedirectToAction("Index");
        }

        /// <summary>
        ///批量 从购物车删除
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteBatch(string ckbitem = "")
        {
            if (LoginedUserModel == null)
            {
                return RedirectToAction("Index", "Login");
            }

            CartService.DeleteBatch(ckbitem);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 获取购物车统计数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCartTJ(Cart cart)
        {
            json.Data = CartService.GetCartTJ(LoginedUserModel, cart);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

    }
}