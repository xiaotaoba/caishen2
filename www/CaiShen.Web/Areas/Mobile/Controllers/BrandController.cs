using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class BrandController : CheckLoginController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Join()
        {
            return View();
        }
	}
}