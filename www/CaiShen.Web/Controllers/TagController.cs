using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Models;
using Pannet.DAL;
using Pannet.Utility;
using PagedList;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using Pannet.Web.Attribute;
using Pannet.DAL.Repository;
using System.Text;

//using System.ComponentModel.DataAnnotations;

namespace Pannet.Web.Controllers
{
    public class TagController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        #region 标签管理

        /// <summary>
        /// 标签管理
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Index(string keyword = "", int type = 0, int follow = -1, int ID = 0, int page = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.type = type;
            ViewBag.follow = follow;
            ViewBag.Parents = work.TagRepository.Get(m => m.T_FollowID == 0);

            int pageSize = 12;

            var rst = work.Context.Tags.AsQueryable();
            if (type != 0)
            {
                rst = rst.Where(m => m.T_Type == type);
            }
            if (follow != -1)
            {
                rst = rst.Where(m => m.T_FollowID == follow);
            }
            if (keyword != "")
            {
                rst = rst.Where(m => m.T_Name.Contains(keyword));
            }
            rst = rst.OrderByDescending(m => m.ID);

            return View(rst.ToPagedList(page, pageSize));
        }
        #endregion

        #region 添加/编辑

        /// <summary>
        /// 添加/编辑
        /// </summary>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Add(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.TagRepository.Get(m => m.ID == ID).FirstOrDefault<Tag>();
                return View(model);
            }
            return View(new Tag());
        }
        // <summary>
        /// 添加/编辑-POST
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[CheckPermission]
        public ActionResult Add(Tag newModel, int ID = 0)
        {
            if (ID == 0)
            {
                work.TagRepository.Insert(newModel);
                work.Save();
                work.Dispose();
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            else
            {
                work.TagRepository.Update(newModel);
                work.Save();
                work.Dispose();
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            return View(newModel);
        }

        #endregion

        #region 删除标签
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="GoodsID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Delete(int ID = 0, int GoodsID = 0)
        {
            if (ID != 0)
            {
                var m = work.TagRepository.Get(g => g.ID == ID).FirstOrDefault();
                if (m != null)
                {
                    work.TagRepository.Delete(m);
                    work.Save();
                    work.Dispose();
                }
            }
            return RedirectToAction("Index", new { GoodsID = GoodsID });
        }


        #endregion

    }
}