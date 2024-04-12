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
    public class ClubController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };
        public ActionResult Index(int ID = 0, string sort = "", int asc = 0, int page = 1, int design = 0)
        {
            ViewBag.ID = ID;
            ViewBag.sort = sort;
            ViewBag.asc = asc;
            ViewBag.page = page;
            ViewBag.design = design;

            var rst = work.Context.DesignWorks.Where(m => m.DW_Is_Enable == 1);
            rst = rst.OrderByDescending(m => m.ID);
            int pageSize = 40;
            return View(rst.ToPagedList(page, pageSize));
        }

        public ActionResult Detail(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.Context.DesignWorks.Where(m => m.ID == ID).FirstOrDefault<DesignWork>();
                if (model != null)
                {
                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.设计作品));

                    return View(model);
                }
                else
                {
                    Response.Redirect("/Mobile/Club/");
                    Response.End();
                }

            }
            else
            {
                Response.Redirect("/Mobile/Club/");
                Response.End();
            }
            return View();
        }
    }
}