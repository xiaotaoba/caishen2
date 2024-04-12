using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;
using Pannet.Utility;
using PagedList;

namespace Pannet.Web.Controllers
{
    public class WebController : Controller
    {
        private UnitOfWork work = new UnitOfWork();

        public ActionResult Index()
        {
            //Response.Redirect("/Mobile/");
            // Response.End();
            return View();
        }
    }
}