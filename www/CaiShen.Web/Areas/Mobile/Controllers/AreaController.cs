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
    public class AreaController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 地区

        //默认列表
        public ActionResult Index()
        {
            return View(work.AreaRepository.Get());
        }

        #endregion

        #region 获取省、市、区信息

        /// <summary>
        /// 获取Area Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAreaJson(int parentid)
        {
            List<Area> listArea = work.AreaRepository.Get(m => m.Area_ParentID == parentid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}