using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class MessageController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        public ActionResult Feedback()
        {
            return View();
        }

        #region 咨询产品留言

        /// <summary>
        /// 咨询产品留言
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddConsult(string title, string tel)
        {
            //if (LoginedUserModel == null)
            //{
            //    json.Data = new { status = "-1", msg = "请先登录!" };
            //    return Json(json.Data, JsonRequestBehavior.AllowGet);
            //}
            try
            {
                //当前店铺
                UserShop currentUserShop = UserShopService.GetCurrentShop();

                ConsultMessage newModel = new ConsultMessage();
                newModel.Title = title;
                newModel.Tel = tel;
                if (LoginedUserModel != null)
                {
                    newModel.UserID = LoginedUserModel.ID;
                }
                if (currentUserShop != null)
                {
                    newModel.ShopName = currentUserShop.Shop_Name;
                    newModel.UserShopID = currentUserShop.ID;
                }
                work.ConsultMessageRepository.Insert(newModel);
                work.Save();
                //work.Dispose();
                string mobile = ConfigHelper.GetConfigString("SmsOrderMobile");
                if (!string.IsNullOrEmpty(currentUserShop.Shop_Mobile) && PageValidate.IsNumber(currentUserShop.Shop_Mobile.Replace(",", "")))//如果联系电话是手机，且不为空，发送短信提示，否则发送到总商城默认手机上。
                {
                    mobile = currentUserShop.Shop_Mobile;
                }
                //SmsService.SendSms(mobile, currentUserShop.Shop_Name, "新留言", "处理");

                json.Data = new { status = "success", msg = "提交成功!" };

            }
            catch
            {
                json.Data = new { status = "-1", msg = "操作失败!" };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 培训需求提交

        /// <summary>
        /// 培训需求提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PeiXunApply(string username, string company, string content, string tel, string address)
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录!" };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }
            try
            {

                ConsultMessage newModel = new ConsultMessage();
                newModel.Title = "培训需求留言";
                newModel.Tel = tel;
                newModel.UserName = username;
                newModel.ShopName = company;
                newModel.Content = content;
                newModel.Address = address;
                if (LoginedUserModel != null)
                {
                    newModel.UserID = LoginedUserModel.ID;
                }
                work.ConsultMessageRepository.Insert(newModel);
                work.Save();
                //work.Dispose();
                //SmsService.SendSms(mobile, currentUserShop.Shop_Name, "新留言", "处理");

                json.Data = new { status = "success", msg = "提交成功!" };

            }
            catch
            {
                json.Data = new { status = "-1", msg = "操作失败!" };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}