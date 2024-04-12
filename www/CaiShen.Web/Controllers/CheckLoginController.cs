using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.Web.Controllers
{
    public class CheckLoginController : Controller
    {
        private Manager _adminmodel;
        private UnitOfWork work = new UnitOfWork();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userName = CookieHelper.GetValue(ConfigHelper.CookieAdminName);
            if (!string.IsNullOrEmpty(userName))
            {
                var user = work.Context.Managers.AsNoTracking().Where(u => u.UserName == userName).ToList<Manager>();
                if (user.Count() > 0)
                {
                    _adminmodel = user[0];
                    ViewBag.LoginedAdminModel = user[0];
                    filterContext.Controller.ViewData["manager"] = user[0];
                }
            }
            else
            {
                filterContext.HttpContext.Response.Redirect("/Manager/Login");
            }
            base.OnActionExecuting(filterContext);
        }
        public Manager LoginedAdminModel
        {
            get
            {
                return _adminmodel;
            }
        }
	}
}