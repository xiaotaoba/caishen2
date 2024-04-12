using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL.Repository;
using Pannet.Utility;
using Pannet.Models;
using Pannet.DAL;
using PagedList;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class ArticleController : CheckLoginController
    {
        public UnitOfWork work = new UnitOfWork();

        //康复课堂
        public ActionResult Index(int ID = 1, int page = 1,string order ="tj",string keyword = "")
        {
            ViewBag.ID = ID;
            ViewBag.page = page;
            ViewBag.order = order;
            ViewBag.keyword = keyword;

            //类型
            if (ID != 0)
            {
                ArticleType typeModel = work.ArticleTypeRepository.GetByID(ID);
                //一级
                if (typeModel != null && typeModel.AT_ParentID == 0)
                {
                    ViewBag.ArticleTypeParent = typeModel;
                }
                else
                { //二级
                    ViewBag.ArticleTypeParent = work.ArticleTypeRepository.GetByID(typeModel.AT_ParentID);
                    ViewBag.ArticleType = typeModel;
                }
            }

            var rst = work.Context.Articles.AsQueryable();
            if (ID != 0)
            {
                rst = rst.Where(m => m.ArticleTypeID == ID);
            }
             if (keyword!="")
            {
               rst = rst.Where(m => m.Art_Title.Contains(keyword));
            }
            if (order == "" || order == "tj")
            {
                rst = rst.OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.Art_CreateTime).ThenByDescending(m => m.ID);
            }
            else {
                rst = rst.OrderByDescending(m => m.Art_CreateTime).ThenByDescending(m => m.ID);
            }
            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View();
        }

        #region 常见问题
        public ActionResult FAQ(int ID = 1, int page = 1)
        {
            ViewBag.ID = ID;
            ViewBag.page = page;

            //类型
            if (ID != 0)
            {
                ArticleType typeModel = work.ArticleTypeRepository.GetByID(ID);
                //一级
                if (typeModel != null && typeModel.AT_ParentID == 0)
                {
                    ViewBag.ArticleTypeParent = typeModel;
                }
                else
                { //二级
                    ViewBag.ArticleTypeParent = work.ArticleTypeRepository.GetByID(typeModel.AT_ParentID);
                    ViewBag.ArticleType = typeModel;
                }
            }

            var rst = work.Context.Articles.Where(m => m.Art_IsEnable == 1);
            if (ID != 0)
            {
                rst = rst.Where(m => m.ArticleTypeID == ID);
            }
            rst = rst.OrderByDescending(m => m.Art_IsRecommend).OrderByDescending(m => m.Art_Sort).OrderByDescending(m => m.Art_CreateTime).OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View();
        }
        public ActionResult FAQDetail(int ID = 0)
        {
            if (ID != 0)
            {
                Article model = work.ArticleRepository.GetByID(ID);
                if (model == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.ArticleType = work.ArticleTypeRepository.GetByID(model.ArticleTypeID);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
            //return View();
        }
        #endregion

        public ActionResult List(int type = 0, int page = 1)
        {
            if (type == 0)
            {
                type = Convert.ToInt16(DataConfig.ArticleTypeEnum.平台公告);
            }

            ViewBag.type = type;
            ViewBag.page = page;

            ViewBag.TypeModel = work.ArticleTypeRepository.GetByID(type);

            var rst = work.Context.Articles.Where(m => m.Art_IsEnable == 1 && m.ArticleTypeID == type);
            rst = rst.OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID);

            int pageSize = 40;
            return View(rst.ToPagedList(page, pageSize));
            //return View();
        }
        /// <summary>
        /// 案例见证
        /// </summary>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Cases(int type = 0, int page = 1)
        {
            if (type == 0)
            {
                type = 11;// Convert.ToInt16(DataConfig.ArticleTypeEnum.作业本通知);
            }

            ViewBag.type = type;
            ViewBag.page = page;

            ViewBag.TypeModel = work.ArticleTypeRepository.GetByID(type);

            var rst = work.Context.Articles.Where(m => m.Art_IsEnable == 1 && m.ArticleTypeID == type);
            rst = rst.OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID);

            int pageSize = 40;
            return View(rst.ToPagedList(page, pageSize));
            //return View();
        }

        public ActionResult Detail(int ID = 0)
        {
            if (LoginedUserModel == null)
            {
                //return RedirectToAction("Index", "Login",);
                Response.Redirect("/mobile/login?returnurl=" + Server.UrlEncode(Request.Url.ToString()));
                Response.End();
            }
            if (ID != 0)
            {
                var model = work.Context.Articles.Where(m => m.ID == ID).FirstOrDefault();
                if (model != null)
                {
                    model.Art_ShowTimes = model.Art_ShowTimes + 1;
                    work.ArticleRepository.Update(model);
                    work.Save();
                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.文章));

                    return View(model);
                }
            }
            return RedirectToAction("List");
        }

        public ActionResult Info(int ID = 0)
        {
            if (LoginedUserModel == null)
            {
                Response.Redirect("/mobile/login?returnurl=" + Server.UrlEncode(Request.Url.ToString()));
                Response.End();
            }
            if (ID != 0)
            {
                var model = work.Context.Articles.Where(m => m.ID == ID).FirstOrDefault();
                if (model != null)
                {
                    model.Art_ShowTimes = model.Art_ShowTimes + 1;
                    work.ArticleRepository.Update(model);
                    work.Save();
                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.文章));

                    return View(model);
                }
            }
            return RedirectToAction("List");
        }
    }
}