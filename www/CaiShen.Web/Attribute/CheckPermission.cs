using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Models;
using Pannet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pannet.Web.Attribute
{
    /// <summary>
    /// 验证权限
    /// </summary>
    public class CheckPermissionAttribute : ActionFilterAttribute
    {
        filterContextInfo fc;
        private UnitOfWork work = new UnitOfWork();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //判断权限
            fc = new filterContextInfo(filterContext);
            Manager loginedModel = filterContext.Controller.ViewData["manager"] as Manager;
            bool isHasLimit = false;
            string actionName = fc.controllerName + "/" + fc.actionName;
            if (loginedModel == null)
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
                        loginedModel = user[0];
                    }
                    else
                    {
                        filterContext.HttpContext.Response.Redirect("/Manager/Login");
                    }
                }

            }
            if (loginedModel.ID == 2)//admin
            {
                //所有权限
                isHasLimit = true;
            }
            else
            {
                if (fc.ID != null && fc.ID == "0")
                {
                    actionName += "/0";
                }
                isHasLimit = ManagerService.HasLimit(actionName, loginedModel);
            }

            if (!isHasLimit)
            {
                //filterContext.HttpContext.Response.Write(string.Format("无访问权限！{0}", actionName));这个方法不行，只是改变相应内容，该访问内容也会访问到。
                //filterContext.HttpContext.Response.End();
                filterContext.Result = new ContentResult { Content = string.Format(" 无访问权限！{0}", actionName) };// 直接返回 return Content("抱歉,你不具有当前操作的权限！")
                //bool isstate = true;
                ////islogin = false;
                //if (isstate)//如果满足
                //{
                //    //逻辑代码
                //    // filterContext.Result = new HttpUnauthorizedResult();//直接URL输入的页面地址跳转到登陆页  
                //    // filterContext.Result = new RedirectResult("http://www.***.com");//也可以跳到别的站点
                //    //filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "product", action = "Default" }));
                //}
                //else
                //{
                //    filterContext.Result = new ContentResult { Content = @"抱歉,你不具有当前操作的权限！" };// 直接返回 return Content("抱歉,你不具有当前操作的权限！")
                //}
            }
            else {
                base.OnActionExecuting(filterContext);
            }
        }
        //public static filterContextInfo contextInfo { get; set; }
    }
    public class filterContextInfo
    {
        public filterContextInfo(ActionExecutingContext filterContext)
        {
            #region 获取链接中的字符

            // 获取域名
            domainName = filterContext.HttpContext.Request.Url.Authority;

            //获取模块名称
            //  module = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();

            //获取 controllerName 名称
            controllerName = filterContext.RouteData.Values["controller"].ToString();

            //获取ACTION 名称
            actionName = filterContext.RouteData.Values["action"].ToString();

            //获取ACTION 名称
            if (filterContext.RouteData.Values["ID"] != null)
            {
                this.ID = "0";
            }
            else
            {

            }
            #endregion
        }

        /// <summary>
        /// 获取域名
        /// </summary>
        public string domainName { get; set; }
        /// <summary>
        /// 获取模块名称
        /// </summary>
        public string module { get; set; }
        /// <summary>
        /// 获取 controllerName 名称
        /// </summary>
        public string controllerName { get; set; }
        /// <summary>
        /// 获取ACTION 名称
        /// </summary>
        public string actionName { get; set; }

        /// <summary>
        /// 是否有ID参数
        /// </summary>
        public string ID { get; set; }

    }
}