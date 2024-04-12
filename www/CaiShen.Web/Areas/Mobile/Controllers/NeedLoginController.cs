using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Models;
using Pannet.Utility;
using Pannet.DAL;
using Pannet.DAL.Repository;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class NeedLoginController : Controller
    {
        private User _usermodel;
        //private UserShop _shopmodel;
        private UnitOfWork work = new UnitOfWork();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = UserService.GetLoginedModel();
            if (user != null)
            {
                _usermodel = user;
                ViewBag.LoginUser = user;
            }
            else
            {
                filterContext.HttpContext.Response.Redirect("/mobile/login?returnurl=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.Url.ToString()));
            }
            //_shopmodel = UserShopService.GetCurrentShop();

            base.OnActionExecuting(filterContext);
        }
        public User LoginedUserModel
        {
            get
            {
                if (_usermodel == null || _usermodel.ID == 0)
                {
                    HttpContext.Response.Redirect("/mobile/login?returnurl=" + HttpContext.Server.UrlEncode(HttpContext.Request.Url.ToString()));
                }
                return _usermodel;
            }
        }
        public UserShop CurrentShopModel
        {
            get
            {
                return new UserShop();
            }
        }
    }

}