using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Models;
using PagedList;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class ActivityController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        #region 商学院

        public ActionResult Index()
        {
            int art_type = Convert.ToInt16(DataConfig.ArticleTypeEnum.滚动图片);
            ViewBag.ScrollPhotos = work.ArticleRepository.Get(m => m.ArticleTypeID == art_type || m.Art_IsRecommend == 1).Where(m=>m.Art_IsEnable==1).OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID).ToList();
            //ViewBag.TeamInfo = work.ArticleRepository.GetByID(42);
            //单页内容-团队介绍
            ViewBag.TeamInfo = work.ArticleRepository.Get(m => m.ArticleTypeID == 14).Where(m => m.Art_IsEnable == 1).OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID).ToList();
            ViewBag.SystemInfo = work.SystemInfoRepository.Get().FirstOrDefault();
            ViewBag.Courses = work.GoodsRepository.Get(m => m.G_IsRecommend == 1 && m.G_Status == 1).Take(6).ToList();
            return View();
        }

        public ActionResult Info(int ID = 0)
        {
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
                else
                {
                    Response.Redirect("/Mobile/Activity/");
                    Response.End();
                }
            }
            else
            {
                Response.Redirect("/Mobile/Activity/");
                Response.End();
            }
            return View();
        }

        #endregion

        #region 培训活动

        public ActionResult List(int ID = 0, string sort = "", int asc = 0, int page = 1)
        {
            ViewBag.ID = ID;
            ViewBag.sort = sort;
            ViewBag.asc = asc;
            ViewBag.page = page;

            int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);
            var rst = work.Context.Advertisements.Where(m => m.AD_IsEnable == 1);

            rst = rst.OrderByDescending(m => m.AD_Sort).ThenByDescending(m => m.ID);
            int pageSize = 40;
            return View(rst.ToPagedList(page, pageSize));
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
                var model = work.Context.Advertisements.Where(m => m.ID == ID).FirstOrDefault();
                if (model != null)
                {
                    ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);

                    if (LoginedUserModel != null)
                    {
                        //ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == LoginedUserModel.U_Province);
                        //ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == LoginedUserModel.U_City);
                        var rstD = work.Context.Departments.Where(m => m.ID == LoginedUserModel.U_DepartmentID);
                        if (rstD != null && rstD.Count() > 0)
                        {
                            ViewBag.Department = rstD.FirstOrDefault();
                        }
                    }

                    model.AD_Click = model.AD_Click + 1;
                    work.AdvertisementRepository.Update(model);
                    work.Save();

                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.培训活动));

                    return View(model);
                }
                else
                {
                    Response.Redirect("/Mobile/Activity/List");
                    Response.End();
                }
            }
            else
            {
                Response.Redirect("/Mobile/Activity/List");
                Response.End();
            }
            return View();
        }


        /// <summary>
        /// 活动报名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdvertisementRecordAdd(string username, string tel, int ad_id = 0, string zhiwei = "", string address = "")
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录!" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var existCount = work.Context.AdvertisementRecords.Where(m => m.UserID == LoginedUserModel.ID && ad_id == m.AdvertisementID).Count();
                if (existCount > 0)
                {
                    json.Data = new { status = "-1", msg = "您已参与报名，请勿重复提交！" };
                }
                else
                {

                    AdvertisementRecord newModel = new AdvertisementRecord();
                    newModel.ADR_UserName = username;
                    newModel.ADR_Tel = tel;
                    if (LoginedUserModel != null)
                    {
                        newModel.UserID = LoginedUserModel.ID;
                    }
                    newModel.AdvertisementID = ad_id;
                    newModel.ADR_Address = address;
                    newModel.ADR_Position = zhiwei;
                    work.AdvertisementRecordRepository.Insert(newModel);
                    work.Save();

                    json.Data = new { status = "success", msg = "报名成功!" };
                }
            }
            catch
            {
                json.Data = new { status = "-1", msg = "操作失败!" };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 市场动态

        public ActionResult News(int ID = 0, int page = 1)
        {
            ViewBag.ID = ID;
            ViewBag.page = page;

            int art_type = Convert.ToInt16(DataConfig.ArticleTypeEnum.康复资讯);
            var rst = work.Context.Articles.Where(m => m.Art_IsEnable == 1 && m.ArticleTypeID == art_type);
            rst = rst.OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID);

            int pageSize = 40;
            return View(rst.ToPagedList(page, pageSize));
            //return View();
        }

        public ActionResult NewsDetail(int ID = 0)
        {
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
                else
                {
                    Response.Redirect("/Mobile/Activity/News");
                    Response.End();
                }
            }
            else
            {
                Response.Redirect("/Mobile/Activity/News");
                Response.End();
            }
            return View();
        }

        #endregion
    }
}