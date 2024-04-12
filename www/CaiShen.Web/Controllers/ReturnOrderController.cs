using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;using Pannet.Utility;
using PagedList;

namespace Pannet.Web.Controllers
{
    public class ReturnOrderController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 退货订单

        //默认退货订单列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int ShopID = 0, string status = "", string paystatus = "", string shippingstatus = "", int page = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.status = status;
            ViewBag.paystatus = paystatus;
            ViewBag.shippingstatus = shippingstatus;
            ViewBag.ShopID = ShopID;
            ViewBag.UserShops = work.UserShopRepository.Get();

            var rst = work.Context.ReturnOrders
                .Join(work.Context.Users, ro => ro.UserID, u => u.ID, (ro, u) => new { ro, u })
                .Join(work.Context.UserShops, rou => rou.ro.UserShopID, us => us.ID, (rou, us) => new
                {
                    ro = rou.ro,
                    u = rou.u,
                    us
                })
                .Join(work.Context.OrderDetails, m => m.ro.OrderDetailID, od => od.ID, (m, od) => new
                {
                    ro = m.ro,
                    u = m.u,
                    us = m.us,
                    od
                });

            if (ShopID != 0)
            {
                rst = rst.Where(m => m.ro.UserShopID == ShopID);
            }
            if (status != "")
            {
                rst = rst.Where(m => m.ro.RO_Status.ToString() == status);
            }
            if (paystatus != "")
            {
                rst = rst.Where(m => m.ro.RO_PayStatus.ToString() == paystatus);
            }
            if (shippingstatus != "")
            {
                rst = rst.Where(m => m.ro.RO_ShippingStatus.ToString() == shippingstatus);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.ro.RO_ReturnOrderNo.Contains(keyword));
            }
            List<ReturnOrderVModel> list = rst.ToList().Select(m => new ReturnOrderVModel
            {
                ReturnOrder = m.ro,
                User = m.u,
                UserShop = m.us,
                OrderDetail = m.od
            }).OrderByDescending(m => m.ReturnOrder.ID).ToList();

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 详细页
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Detail(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.ReturnOrderRepository.Get(m => m.ID == ID).FirstOrDefault<ReturnOrder>();
                return View(model);
            }
            return View(new ReturnOrder());
        }


        /// <summary>
        /// 删除退货订单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var model = work.ReturnOrderRepository.Get(m => m.ID == ID).FirstOrDefault<ReturnOrder>();
                if (model != null)
                {
                    model.RO_IsDelete = 1;

                    work.ReturnOrderRepository.Update(model);
                    work.Save();
                    work.Dispose();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

    }
}