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
    public class ArticleTypeController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 文章类型

        //默认文章类型列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int page = 1)
        {
            ViewBag.keyword = keyword;

            var rst = work.Context.ArticleTypes.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.AT_Name.Contains(keyword));
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add(int ID = 0)
        {
            ViewBag.ID = ID;

            if (ID != 0)
            {
                ViewBag.Parents = work.ArticleTypeRepository.Get(m => m.ID != ID & m.AT_ParentID == 0).OrderByDescending(m => m.AT_Sort).ToList();
                var model = work.ArticleTypeRepository.Get(m => m.ID == ID).FirstOrDefault<ArticleType>();
                return View(model);
            }
            else
            {
                ViewBag.Parents = work.ArticleTypeRepository.Get(m => m.AT_ParentID == 0).OrderByDescending(m => m.AT_Sort).ToList();
            }
            return View(new ArticleType());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Add(ArticleType newModel, int ID = 0)
        {
            ViewBag.ID = ID;
            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    ViewBag.Parents = work.ArticleTypeRepository.Get(m => m.AT_ParentID == 0).OrderByDescending(m => m.AT_Sort).ToList();
                    var existModel = work.ArticleTypeRepository.Get(m => m.AT_Name == newModel.AT_Name);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("AT_Name", "文章类型名称已存在");
                    }
                    else
                    {
                        work.ArticleTypeRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    ViewBag.Parents = work.ArticleTypeRepository.Get(m => m.ID != ID & m.AT_ParentID == 0).OrderByDescending(m => m.AT_Sort).ToList();
                    //var oldModel = work.ArticleTypeRepository.Get(m => m.ID == ID).FirstOrDefault<ArticleType>();
                    //var oldModel = work.Context.ArticleTypes.AsNoTracking().Where(m => m.ID == ID).FirstOrDefault<ArticleType>();
                    var existModel = work.ArticleTypeRepository.Get(m => m.AT_Name == newModel.AT_Name & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("AT_Name", "文章类型名称已存在");
                    }
                    else
                    {
                        //oldModel.AT_IsEnable = newModel.AT_IsEnable;
                        //oldModel.AT_IsRecommend = newModel.AT_IsRecommend;
                        //oldModel.AT_Name = newModel.AT_Name;
                        //oldModel.AT_ParentID = newModel.AT_ParentID;
                        //oldModel.AT_Sort = newModel.AT_Sort;
                        //oldModel.AT_Desc = newModel.AT_Desc;
                        //newModel.ID = ID;
                        //oldModel = newModel;

                        work.ArticleTypeRepository.Update(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除文章类型
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
                var role = work.ArticleTypeRepository.Get(m => m.ID == ID).FirstOrDefault<ArticleType>();
                work.ArticleTypeRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 获取分类JSON

        /// <summary>
        /// 获取分类 Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetTypeJson(int parentid)
        {
            List<ArticleType> listArea = work.ArticleTypeRepository.Get(m => m.AT_ParentID == parentid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}