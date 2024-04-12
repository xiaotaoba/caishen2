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
    public class CheckLoginController : Controller
    {
        private User _usermodel;
        //private UserShop _shopmodel;
        public User LoginedUserModel
        {
            get
            {
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
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = UserService.GetLoginedModel();
            if (user != null)
            {
                _usermodel = user;
                ViewBag.LoginUser = user;
            }

            string agent = Request.ServerVariables["Http_User_Agent"].ToLower();
            string openid = CookieHelper.GetValue("openid");
            string u = Request.QueryString["u"];
            if (!string.IsNullOrEmpty(u))
            {
                CookieHelper.SetValue("u", u, ConfigHelper.CookieExpries);
            }

            Log.WriteLog("CheckLoginController，user为null：" + (user == null ? "true" : "false") + ",openid:" + openid + ",u:" + u + ",url:" + Request.Url.ToString(), "oauth", DateTime.Now.ToString("yyyyMMddHH"));

            if (user == null)
            {
                if (string.IsNullOrEmpty(openid) && agent.Contains("micromessenger"))//如果是微信客户端打开 
                {
                    Response.Redirect(Url.Action("GotoOauth", "WeiXin", new { state = Request.Url.ToString() }));
                    //else if (user != null && string.IsNullOrEmpty(user.U_OpenId))
                    //{
                    //    UnitOfWork work = new UnitOfWork();
                    //   var  newUser = work.Context.Users.Where(m=>m.ID==user.ID).First();
                    //    string headimgurl = CookieHelper.GetValue("wx_headimgurl");
                    //    string nickname = CookieHelper.GetValue("wx_nickname");
                    //    //绑定openid
                    //    newUser.U_OpenId = openid;
                    //    if (string.IsNullOrEmpty(newUser.U_Thumbnail))
                    //    {
                    //        newUser.U_Thumbnail = headimgurl;
                    //    }
                    //    if (string.IsNullOrEmpty(newUser.U_NickName))
                    //    {
                    //        newUser.U_NickName = nickname;
                    //    }
                    //    work.UserRepository.Update(newUser);
                    //    work.Save();

                    //    UserService.SetCacheUser(newUser.U_UserName, newUser);
                    //}
                }
                else
                {
                    //微信登录，并绑定账号，自动登录
                    UnitOfWork work = new UnitOfWork();
                    var rstUser = work.UserRepository.Get(m => m.U_OpenId == openid && m.U_IsDelete == 0 && m.U_Is_Enable == 1).ToList();
                    if (rstUser != null && rstUser.Count() > 0)
                    {
                        _usermodel = rstUser[0];
                        CookieHelper.SetValue(ConfigHelper.CookieUserName, _usermodel.U_UserName, ConfigHelper.CookieExpries);
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }

}