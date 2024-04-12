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
    public class CommentController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 通用评价

        //默认评价列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int page = 1)
        {
            ViewBag.keyword = keyword;

            int comm_type = Convert.ToInt16(DataConfig.CommentTypeEnum.课程);
            var rst = work.Context.Comments
                .Join(work.Context.Users, c => c.Comm_UserId, u => u.ID, (c, u) => new { c, u })
                .Join(work.Context.Goods, m => m.c.Comm_P_Id, g => g.ID, (m, g) => new { m.c, m.u, g })
                .Select(m => new CommentGoodsVModel
                {
                    Comment = m.c,
                    GoodsName = m.g.G_Name,
                    UserName =m.u.U_UserName
                })
                .Where(m => m.Comment.Comm_Type == comm_type);

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.Comment.Comm_Content.Contains(keyword) || m.GoodsName.Contains(keyword) || m.UserName.Contains(keyword));
            }

            rst = rst.OrderByDescending(m => m.Comment.ID);

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 详细页
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Detail(int ID = 0, string tips = "")
        {

            return View();
        }
        /// <summary>
        /// 详细页-保存
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult Detail(int ID = 0, string ReplyContent = "", int status = -1)
        {
            //ViewBag.ID = ID;
            //ViewBag.orderDetailID = orderDetailID;

            //if (ID != 0)
            //{
            //    var model = work.CommentRepository.Get(m => m.ID == ID).FirstOrDefault<OrderComment>();

            //    if (!string.IsNullOrEmpty(OC_ReplyContent))
            //    {
            //        model.OC_ReplyContent = OC_ReplyContent;
            //    }
            //    //if (status != -1)
            //    //{
            //    //    model.OC_Status = status;
            //    //}
            //    work.CommentRepository.Update(model);
            //    work.Save();

            //    return View(model);
            //}
            return RedirectToAction("Detail", new { tips = "success" });
        }

        /// <summary>
        /// 删除通用
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var model = work.CommentRepository.Get(m => m.ID == ID).FirstOrDefault<Comment>();
                if (model != null)
                {
                    work.CommentRepository.Delete(model);
                    work.Save();
                    work.Dispose();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}