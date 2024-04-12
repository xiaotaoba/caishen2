using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Utility;
using PagedList;
using System.Data.Entity.Migrations;


namespace Pannet.Web.Controllers
{
    public class SystemInfoController : CheckLoginController
    {
        //迁移至systeminfoService中 20174-12-11
        //private string wxConfigPath = "Config/weixin.config";
        //private string alipayConfigPath = "Config/alipay.config";
        //private string percentConfigPath = "Config/percent.config";
        private UnitOfWork work = new UnitOfWork();

        #region 系统信息

        //系统信息
        [CheckPermission]
        public ActionResult Index(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.SystemInfoRepository.Get(m => m.ID == ID).FirstOrDefault<SystemInfo>();
                return View(model);
            }
            else
            {
                var model = work.SystemInfoRepository.Get().FirstOrDefault<SystemInfo>();
                if (model != null)
                {
                    return View(model);
                }
            }
            return View(new SystemInfo());
        }


        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(SystemInfo newModel)
        {
            if (ModelState.IsValid)
            {
                if (newModel.ID == 0)//新增
                {
                    //work.SystemInfoRepository.Insert(newModel);
                    //work.Save();
                    //work.Dispose();

                    //ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                    //return RedirectToAction("Index", new { ID = newModel.ID });
                }
                else
                {
                    work.SystemInfoRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
            }
            return View(newModel);
        }


        #endregion

        #region 微信配置

        //迁移至systeminfoService中 20174-12-11
        //public WeixinConfig GetWeixinConfigModel()
        //{
        //    WeixinConfig model = new WeixinConfig();
        //    model.AppID = ConfigHelper.GetConfigValue(wxConfigPath, "wx:AppID");
        //    model.AppSecret = ConfigHelper.GetConfigValue(wxConfigPath, "wx:AppSecret");
        //    model.EncodingAESKey = ConfigHelper.GetConfigValue(wxConfigPath, "wx:EncodingAESKey");
        //    model.Token = ConfigHelper.GetConfigValue(wxConfigPath, "wx:Token");
        //    model.WeixinName = ConfigHelper.GetConfigValue(wxConfigPath, "wx:WeixinName");
        //    return model;
        //}
        //微信配置
        [CheckPermission]
        public ActionResult WeixinConfig()
        {
            WeixinConfig model = SystemInfoService.GetWeixinConfig();
            return View(model);
        }

        /// <summary>
        /// 微信配置 - 编辑保存
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult WeixinConfig(WeixinConfig newModel)
        {
            if (ModelState.IsValid)
            {
                SystemInfoService.SetWeixinConfig(newModel);

                //一并保存到数据库-2017-12-11
                WeixinConfig existModel = work.Context.WeixinConfigs.AsNoTracking().FirstOrDefault();
                if (existModel != null)
                {
                    newModel.ID = existModel.ID;
                    work.Context.WeixinConfigs.AddOrUpdate(m => m.ID, newModel);
                }
                else
                {
                    work.Context.WeixinConfigs.AddOrUpdate(m => m.AppID, newModel);
                }
                work.Save();
                work.Dispose();

                //更新前台配置
                SystemInfoService.UpdateClientWeixinConfig();

                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            return View(newModel);
        }

        /// <summary>
        /// 获取微信配置
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public JsonResult GetWeixinConfig()
        {
            WeixinConfig model = SystemInfoService.GetWeixinConfig();

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 支付宝配置

        //支付宝配置
        [CheckPermission]
        public ActionResult AlipayConfig()
        {
            AlipayConfig model = SystemInfoService.GetAlipayConfig();
            return View(model);
        }

        /// <summary>
        /// 支付宝配置 - 编辑保存
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult AlipayConfig(AlipayConfig newModel)
        {
            if (ModelState.IsValid)
            {
                SystemInfoService.SetAlipayConfig(newModel);

                //一并保存到数据库-2017-12-11
                AlipayConfig existModel = work.Context.AlipayConfigs.AsNoTracking().FirstOrDefault();
                if (existModel != null)
                {
                    newModel.ID = existModel.ID;
                    work.Context.AlipayConfigs.AddOrUpdate(m => m.ID, newModel);
                }
                else
                {
                    work.Context.AlipayConfigs.AddOrUpdate(m => m.Seller_email, newModel);
                }
                work.Save();
                work.Dispose();

                //更新前台配置
                SystemInfoService.UpdateClientAlipayConfig();

                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            return View(newModel);
        }

        /// <summary>
        /// 获取支付宝配置
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public JsonResult GetAlipayConfig(string t = "")
        {
            AlipayConfig model = SystemInfoService.GetAlipayConfig();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 系统配置


        //系统配置
        [CheckPermission]
        public ActionResult ProfitPercentConfig()
        {
            ProfitPercentConfig model = SystemInfoService.GetPercentConfig();
            return View(model);
        }

        /// <summary>
        /// 系统配置 - 编辑保存
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult ProfitPercentConfig(ProfitPercentConfig newModel)
        {
            if (ModelState.IsValid)
            {
                SystemInfoService.SetPercentConfig(newModel);

                //一并保存到数据库-2018-03-15
                ProfitPercentConfig existModel = work.Context.ProfitPercentConfigs.AsNoTracking().FirstOrDefault();
                if (existModel != null)
                {
                    newModel.ID = existModel.ID;
                    work.Context.ProfitPercentConfigs.AddOrUpdate(m => m.ID, newModel);
                }
                else
                {
                    work.Context.ProfitPercentConfigs.Add(newModel);
                }
                work.Save();
                work.Dispose();

                //更新前台配置
                SystemInfoService.UpdateClientPercentConfig();
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            return View(newModel);
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public JsonResult GetPercentConfig(string t = "")
        {
            ProfitPercentConfig model = SystemInfoService.GetPercentConfig();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 邮箱配置


        //邮箱配置
        [CheckPermission]
        public ActionResult SmtpConfig()
        {
            SMTPConfig model = SystemInfoService.GetSmtpConfig();
            return View(model);
        }

        /// <summary>
        /// 邮箱配置 - 编辑保存
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult SmtpConfig(SMTPConfig newModel)
        {
            SystemInfoService.SetSmtpConfig(newModel);

            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            return View(newModel);
        }

        #endregion

        #region 发送微信模板消息

        //发送微信模板消息
        [CheckPermission]
        public ActionResult SendWxTemplateMessage()
        {
            return View();
        }

        /// <summary>
        /// 发送微信模板消息 - 编辑保存
        /// </summary>
        /// <param name="type">1按部门发送，2指定openid发送</param>
        /// <param name="first"></param>
        /// <param name="keyword1"></param>
        /// <param name="keyword2"></param>
        /// <param name="remark"></param>
        /// <param name="url">文章链接</param>
        /// <param name="openids"></param>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult SendWxTemplateMessage(int type, string first, string keyword1, string keyword2, string remark, string url, string openids = "", int departmentID = 0)
        {
            int index = 0;
            if (type == 1)
            {
                var openidList = work.Context.Users
                    .Join(work.Context.Departments, u => u.U_DepartmentID, d => d.ID, (u, d) => new { u, d })
                    .Where(m => m.d.ID == departmentID || m.d.Dep_FollowID == departmentID)
                    .Where(m => m.u.U_OpenId != "" && m.u.U_Is_Enable == 1 && m.u.U_IsDelete == 0)
                    .Select(m => m.u.U_OpenId).Distinct().ToList();
                if (openidList != null && openids.Count() > 0)
                {
                    openids = string.Join(",", openids);
                }
            }
            openids = openids.Trim(',');

            if (!string.IsNullOrEmpty(openids))
            {
                List<string> openidList = openids.Split(',').ToList();
                foreach (var item in openidList)
                {
                    WxTemplateMessage.SendHuiBoMessage(item, first, keyword1, keyword2, remark, url);
                    index++;
                }
            }

            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", string.Format("发送成功，共发送 {0} 条", index.ToString()));

            return View();
        }

        #endregion
    }
}