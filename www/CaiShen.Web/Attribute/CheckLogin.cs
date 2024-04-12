using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.Utility;
using Pannet.Models;

namespace Pannet.Web.Attribute
{
    /// <summary>
    /// 需要登录验证
    /// </summary>
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        private Manager _adminmodel;
        private UnitOfWork work = new UnitOfWork();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userName = CookieHelper.GetValue(ConfigHelper.CookieAdminName);
            if (string.IsNullOrEmpty(userName))
            {
                filterContext.HttpContext.Response.Redirect("/Manager/Login");
            }
            else
            {
                var user = work.ManagerRepository.Get(u => u.UserName == userName, null).ToList<Manager>();
                if (user.Count() > 0)
                {
                    _adminmodel = user[0];
                    filterContext.Controller.ViewData["manager"] = user[0];
                }
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