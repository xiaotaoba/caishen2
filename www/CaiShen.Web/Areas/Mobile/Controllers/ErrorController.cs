using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Utility;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class ErrorController : Controller
    {
        public UnitOfWork work = new UnitOfWork();

        public ActionResult Index(string msg)
        {
            ViewBag.MessageInfo = msg;
            return View();
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Error(string msg)
        {
            ViewBag.MessageInfo = msg;
            return View();
        }
    }
}