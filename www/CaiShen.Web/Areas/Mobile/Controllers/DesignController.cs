using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Models;
using PagedList;
using Senparc.Weixin.MP.Helpers;
using System.Configuration;
using Senparc.Weixin.MP.Containers;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class DesignController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        private int tag_type = Convert.ToInt16(DataConfig.TagTypeEnum.团队成员所属分类);
        private int tag_style = Convert.ToInt16(DataConfig.TagTypeEnum.团队成员临床经验);
        private int tag_tag = Convert.ToInt16(DataConfig.TagTypeEnum.团队成员专长标签);
        private string appId = WebSiteConfig.WeixinAppId;
        private string secret = WebSiteConfig.WeixinAppSecret;

        JsonResult json = new JsonResult
        {
            Data = new { }
        };
        public ActionResult Index(int type = 0, int style = 0, int province = 0, int city = 0, string tag = "", int page = 1)
        {
            if (LoginedUserModel == null)
            {
                //return RedirectToAction("Index", "Login",);
                Response.Redirect("/mobile/login?returnurl=" + Server.UrlEncode(Request.Url.ToString()));
                Response.End();
            }

            ViewBag.type = type;
            ViewBag.style = style;
            ViewBag.tag = tag;
            ViewBag.province = province;
            ViewBag.city = city;
            ViewBag.page = page;
            ViewBag.DesignTypes = work.TagRepository.Get(m => m.T_Type == tag_type && m.GC_IsEnable==1);
            ViewBag.DesignStyles = work.TagRepository.Get(m => m.T_Type == tag_style && m.GC_IsEnable == 1);
            ViewBag.DesignTags = work.TagRepository.Get(m => m.T_Type == tag_tag && m.GC_IsEnable == 1);
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);

            try
            {
                string agent = Request.ServerVariables["Http_User_Agent"].ToLower();
                if (agent.Contains("micromessenger"))//如果是微信客户端打开
                {
                    //JsSdkUiPackage jssdkUiPackage = null;
                    //jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, secret, Request.Url.AbsoluteUri);
                    //ViewBag.jssdkUiPackage = jssdkUiPackage;
                }
                //string ticket = string.Empty;
                //ticket = JsApiTicketContainer.GetJsApiTicket(appId);

                //ViewBag.AppId = appId;
                //ViewBag.Timestamp = JSSDKHelper.GetTimestamp();
                //ViewBag.NonceStr = JSSDKHelper.GetNoncestr();
                //ViewBag.Signature = JSSDKHelper.GetSignature(ticket, ViewBag.NonceStr, ViewBag.Timestamp, Request.Url.AbsoluteUri);
            }
            catch (Exception e) { }

            if (province != 0)
            {
                ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == province);
            }

            var rst = work.Context.DesignWorks.Where(m => m.DW_Is_Enable == 1);
            if (province != 0)
            {
                rst = rst.Where(m => m.DW_Province == province);
            }
            if (city != 0)
            {
                rst = rst.Where(m => m.DW_City == city);
            }
            if (type != 0)
            {
                rst = rst.Where(m => m.DW_Type == type);
            }
            if (style != 0)
            {
                rst = rst.Where(m => m.DW_Style == style);
            }
            IQueryable<DesignWork> rst2 = rst;
            if (!string.IsNullOrEmpty(tag))
            {
                List<string> tagArr = tag.Split(',').ToList().Where(m => m != "").ToList();
                for (int i = 0; i < tagArr.Count; i++)
                {
                    string item = tagArr[i];
                    //if (!string.IsNullOrEmpty(item))
                    //{
                    if (i == 0)
                    {
                        rst2 = rst.Where(m => m.DW_TypeTags.Contains(item));
                    }
                    else
                    {
                        var rst3 = rst.Where(m => m.DW_TypeTags.Contains(item));
                        rst2 = rst2.Union(rst3);
                    }
                    //}
                }
            }
            rst2 = rst2.OrderByDescending(m => m.ID);
            if (rst2.Count() < 1)//无相关数据
            {
                ViewBag.ListTJ = work.Context.DesignWorks.Where(m => m.DW_Is_Enable == 1).OrderByDescending(m => m.ID).Take(6).ToList();
            }
            int pageSize = 40;
            return View(rst2.ToPagedList(page, pageSize));
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
                var model = work.Context.DesignWorks.Where(m => m.ID == ID).FirstOrDefault<DesignWork>();
                if (model != null)
                {
                    ViewBag.ScrollPhotos = work.DesignWorkPhotoRepository.Get(m => m.DesignWorkID == model.ID).OrderByDescending(m => m.DWP_IsFirst).ThenByDescending(m => m.DWP_Sort).ToList();

                    model.DW_ShowTimes++;
                    work.DesignWorkRepository.Update(model);
                    work.Save();

                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.设计作品));
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }
    }
}