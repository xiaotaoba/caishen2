using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Utility;
using PagedList;

namespace Pannet.Web.Controllers
{
    public class OrderCommentController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 订单评价

        //默认评价列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int ShopID = 0, int page = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.ShopID = ShopID;
            //ViewBag.shippingstatus = shippingstatus;
            ViewBag.UserShops = work.UserShopRepository.Get();

            var rst = work.Context.OrderComment
                .Join(work.Context.Users, oc => oc.UserID, u => u.ID, (oc, u) => new { oc, u })
                .Join(work.Context.OrderDetails, ocu => ocu.oc.OrderDetailID, od => od.ID, (ocu, od) => new { ocu.oc, ocu.u, od })
                .Join(work.Context.UserShops, ocud => ocud.od.UserShopID, us => us.ID, (ocud, us) => new
                {
                    oc = ocud.oc,
                    u = ocud.u,
                    us
                }).Where(m => m.oc.OC_IsDelete == 0);

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.oc.OC_Content.Contains(keyword));
            }
            if (ShopID != 0)
            {
                rst = rst.Where(m => m.us.ID == ShopID);
            }
            rst = rst.OrderByDescending(m => m.oc.ID);

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            var listPaged = rst.Select(m => new OrderCommentListVModel
            {
                OrderComment = m.oc,
                UserShop = m.us,
                User = m.u
            });

            return View(listPaged.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 详细页
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Detail(int orderDetailID = 0, string tips = "")
        {
            ViewBag.orderDetailID = orderDetailID;
            if (orderDetailID == 0)
            {
                return RedirectToAction("Index");
            }
            OrderDetailVModel listOrderDetailV = work.Context.OrderDetails
                     .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                     .Where(m => m.od.ID == orderDetailID).Select(m => new OrderDetailVModel
                     {
                         OrderDetail = m.od,
                         PhotoUrl = m.g.G_Image
                     }).FirstOrDefault();
            OrderComment orderComment = work.Context.OrderComment.Where(m => m.OrderDetailID == orderDetailID).FirstOrDefault();
            ViewBag.OrderDetail = listOrderDetailV;

            if (!string.IsNullOrEmpty(tips))
            {
                if (tips == "success")
                {
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
            }

            return View(orderComment);
        }
        /// <summary>
        /// 详细页-保存
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Detail(int ID = 0, int orderDetailID = 0, string OC_ReplyContent = "", int status = -1)
        {
            ViewBag.ID = ID;
            ViewBag.orderDetailID = orderDetailID;

            if (ID != 0)
            {
                var model = work.OrderCommentRepository.Get(m => m.ID == ID).FirstOrDefault<OrderComment>();

                if (!string.IsNullOrEmpty(OC_ReplyContent))
                {
                    model.OC_ReplyContent = OC_ReplyContent;
                }
                //if (status != -1)
                //{
                //    model.OC_Status = status;
                //}
                work.OrderCommentRepository.Update(model);
                work.Save();

                return View(model);
            }
            return RedirectToAction("Detail", new { orderDetailID, tips = "success" });
        }

        /// <summary>
        /// 删除订单
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
                var model = work.OrderCommentRepository.Get(m => m.ID == ID).FirstOrDefault<OrderComment>();
                if (model != null)
                {
                    model.OC_IsDelete = 1;

                    work.OrderCommentRepository.Update(model);
                    work.Save();
                    work.Dispose();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}