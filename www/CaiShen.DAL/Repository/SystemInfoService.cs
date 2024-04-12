using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;
using System.Web;
using System.Web.Script.Serialization;

namespace Pannet.DAL.Repository
{
    public class SystemInfoService
    {
        private static UnitOfWork work = new UnitOfWork();
        private static string alipayConfigPath = "Config/alipay.config";
        private static string wxConfigPath = "Config/weixin.config";
        private static string percentConfigPath = "Config/percent.config";
        private static string smtpConfigPath = "Config/SmtpSetting.config";
        /// <summary>
        /// 管理后台域名
        /// </summary>
        //private static string ManagerUrl = WebSiteConfig.ManagerDomain;
        /// <summary>
        /// 前台域名
        /// </summary>
        private static string WebSiteUrl = WebSiteConfig.WebSiteDomain;
        public static JavaScriptSerializer jss = new JavaScriptSerializer();

        #region 已作废-2017-12-11不允许获取远程配置

        //public static WeixinConfig GetWeixinConfigRemote()
        //{
        //    try
        //    {
        //        WebClient req = new WebClient();
        //        string jsonConfig = req.Post(ManagerUrl.Trim('/') + "/SystemInfo/GetWeixinConfig", "");
        //        return jss.Deserialize<WeixinConfig>(jsonConfig);
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}
        //public static AlipayConfig GetAlipayConfigRemote()
        //{
        //    try
        //    {
        //        WebClient req = new WebClient();
        //        string jsonConfig = req.Post(ManagerUrl.Trim('/') + "/SystemInfo/GetAlipayConfig", "");
        //        return jss.Deserialize<AlipayConfig>(jsonConfig);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        //public static ProfitPercentConfig GetProfitPercentConfigRemote()
        //{
        //    try
        //    {
        //        WebClient req = new WebClient();
        //        string jsonConfig = req.Post(ManagerUrl.Trim('/') + "/SystemInfo/GetPercentConfig", "");
        //        return jss.Deserialize<ProfitPercentConfig>(jsonConfig);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region 获取配置(从配置文件)

        /// <summary>
        /// 获取支付宝支付配置
        /// </summary>
        /// <returns></returns>
        public static AlipayConfig GetAlipayConfig()
        {
            AlipayConfig model = new AlipayConfig();
            model.Key = ConfigHelper.GetConfigValue(alipayConfigPath, "al:Key");
            model.Mainname = ConfigHelper.GetConfigValue(alipayConfigPath, "al:Mainname");
            model.Partner = ConfigHelper.GetConfigValue(alipayConfigPath, "al:Partner");
            model.Seller_email = ConfigHelper.GetConfigValue(alipayConfigPath, "al:Seller_email");
            return model;
        }

        /// <summary>
        /// 获取微信支付配置
        /// </summary>
        /// <returns></returns>
        public static WeixinConfig GetWeixinConfig()
        {
            WeixinConfig model = new WeixinConfig();
            model.AppID = ConfigHelper.GetConfigValue(wxConfigPath, "wx:AppID");
            model.AppSecret = ConfigHelper.GetConfigValue(wxConfigPath, "wx:AppSecret");
            model.EncodingAESKey = ConfigHelper.GetConfigValue(wxConfigPath, "wx:EncodingAESKey");
            model.Token = ConfigHelper.GetConfigValue(wxConfigPath, "wx:Token");
            model.WeixinName = ConfigHelper.GetConfigValue(wxConfigPath, "wx:WeixinName");
            model.MCHID = ConfigHelper.GetConfigValue(wxConfigPath, "wx:MCHID");
            model.KEY = ConfigHelper.GetConfigValue(wxConfigPath, "wx:KEY");
            model.TenpayNotify = ConfigHelper.GetConfigValue(wxConfigPath, "wx:TenpayNotify");
            return model;
        }
        /// <summary>
        /// 系统配置
        /// </summary>
        /// <returns></returns>
        public static ProfitPercentConfig GetPercentConfig()
        {
            ProfitPercentConfig model = new ProfitPercentConfig();
            model.Join_Partner_Profit_Percent = Convert.ToDouble(ConfigHelper.GetConfigValue(percentConfigPath, "pp:Join_Partner"));
            model.Join_Platform_Profit_Percent = Convert.ToDouble(ConfigHelper.GetConfigValue(percentConfigPath, "pp:Join_Platform"));
            model.Order_Partner_Profit_Percent = Convert.ToDouble(ConfigHelper.GetConfigValue(percentConfigPath, "pp:Order_Partner"));
            model.Order_Shop_Profit_Percent = Convert.ToDouble(ConfigHelper.GetConfigValue(percentConfigPath, "pp:Order_Shop"));
            model.Shop_Yufu_Percent = Convert.ToDouble(ConfigHelper.GetConfigValue(percentConfigPath, "pp:Shop_Yufu"));
            model.Shop_JieSuan_Day = ConfigHelper.GetConfigValue(percentConfigPath, "pp:Shop_JieSuanDay");
            model.Order_TuiHuo_Days = Convert.ToInt16(ConfigHelper.GetConfigValue(percentConfigPath, "pp:Order_TuiHuoDays"));
            return model;
        }
        /// <summary>
        /// 邮箱配置
        /// </summary>
        /// <returns></returns>
        public static SMTPConfig GetSmtpConfig()
        {
            Models.SMTPConfig model = new SMTPConfig();
            model.Server = ConfigHelper.GetConfigValue(smtpConfigPath, "Server");
            model.Authentication = ConfigHelper.GetConfigValue(smtpConfigPath, "Authentication");
            model.Sender = ConfigHelper.GetConfigValue(smtpConfigPath, "Sender");
            model.User = ConfigHelper.GetConfigValue(smtpConfigPath, "User");
            model.Password = ConfigHelper.GetConfigValue(smtpConfigPath, "Password");
            return model;
        }
        #endregion

        #region 更新配置文件（当前站点：可以前台或后台）

        /// <summary>
        /// 更新支付宝支付配置
        /// </summary>
        /// <returns></returns>
        public static void SetAlipayConfig(AlipayConfig newModel)
        {
            ConfigHelper.SetConfigValue(alipayConfigPath, "al:Seller_email", newModel.Seller_email);
            ConfigHelper.SetConfigValue(alipayConfigPath, "al:Partner", newModel.Partner);
            ConfigHelper.SetConfigValue(alipayConfigPath, "al:Key", newModel.Key);
            ConfigHelper.SetConfigValue(alipayConfigPath, "al:Mainname", newModel.Mainname);
        }

        /// <summary>
        /// 更新微信支付配置
        /// </summary>
        /// <returns></returns>
        public static void SetWeixinConfig(WeixinConfig newModel)
        {
            ConfigHelper.SetConfigValue(wxConfigPath, "wx:AppID", newModel.AppID);
            ConfigHelper.SetConfigValue(wxConfigPath, "wx:AppSecret", newModel.AppSecret);
            ConfigHelper.SetConfigValue(wxConfigPath, "wx:EncodingAESKey", newModel.EncodingAESKey);
            ConfigHelper.SetConfigValue(wxConfigPath, "wx:Token", newModel.Token);
            ConfigHelper.SetConfigValue(wxConfigPath, "wx:WeixinName", newModel.WeixinName);
            ConfigHelper.SetConfigValue(wxConfigPath, "wx:MCHID", newModel.MCHID);
            ConfigHelper.SetConfigValue(wxConfigPath, "wx:KEY", newModel.KEY);
            ConfigHelper.SetConfigValue(wxConfigPath, "wx:TenpayNotify", newModel.TenpayNotify);
        }
        /// <summary>
        /// 更新系统配置
        /// </summary>
        /// <returns></returns>
        public static void SetPercentConfig(ProfitPercentConfig newModel)
        {
            ConfigHelper.SetConfigValue(percentConfigPath, "pp:Join_Partner", newModel.Join_Partner_Profit_Percent.ToString());
            ConfigHelper.SetConfigValue(percentConfigPath, "pp:Join_Platform", newModel.Join_Platform_Profit_Percent.ToString());
            ConfigHelper.SetConfigValue(percentConfigPath, "pp:Order_Partner", newModel.Order_Partner_Profit_Percent.ToString());
            ConfigHelper.SetConfigValue(percentConfigPath, "pp:Order_Shop", newModel.Order_Shop_Profit_Percent.ToString());
            ConfigHelper.SetConfigValue(percentConfigPath, "pp:Shop_Yufu", newModel.Shop_Yufu_Percent.ToString());
            ConfigHelper.SetConfigValue(percentConfigPath, "pp:Shop_JieSuanDay", newModel.Shop_JieSuan_Day.ToString());
            ConfigHelper.SetConfigValue(percentConfigPath, "pp:Order_TuiHuoDays", newModel.Order_TuiHuo_Days.ToString());
        }
        /// <summary>
        /// 更新邮箱配置
        /// </summary>
        /// <returns></returns>
        public static void SetSmtpConfig(SMTPConfig newModel)
        {
            ConfigHelper.SetConfigValue(smtpConfigPath, "Server", newModel.Server.ToString());
            ConfigHelper.SetConfigValue(smtpConfigPath, "Authentication", newModel.Authentication.ToString());
            ConfigHelper.SetConfigValue(smtpConfigPath, "Sender", newModel.Sender.ToString());
            ConfigHelper.SetConfigValue(smtpConfigPath, "User", newModel.User.ToString());
            ConfigHelper.SetConfigValue(smtpConfigPath, "Password", newModel.Password.ToString());
        }
        #endregion

        #region 更新配置文件（前台）

        /// <summary>
        /// 更新前台微信配置
        /// </summary>
        /// <returns></returns>
        public static void UpdateClientWeixinConfig()
        {
            try
            {
                WebClient req = new WebClient();
                req.Post(WebSiteUrl.Trim('/') + "/SystemInfo/UpdateConfig?type=wxpay", "");
            }
            catch
            {
            }

        }
        /// <summary>
        /// 更新前台支付宝配置
        /// </summary>
        /// <returns></returns>
        public static void UpdateClientAlipayConfig()
        {
            try
            {
                WebClient req = new WebClient();
                req.Post(WebSiteUrl.Trim('/') + "/SystemInfo/UpdateConfig?type=alipay", "");
                //return jss.Deserialize<WeixinConfig>(jsonConfig);
            }
            catch
            {
            }

        }
        /// <summary>
        /// 更新前台系统配置
        /// </summary>
        /// <returns></returns>
        public static void UpdateClientPercentConfig()
        {
            try
            {
                WebClient req = new WebClient();
                req.Post(WebSiteUrl.Trim('/') + "/SystemInfo/UpdateConfig?type=percent", "");
                //return jss.Deserialize<WeixinConfig>(jsonConfig);
            }
            catch
            {
            }

        }

        #endregion

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static SystemInfo GetModel()
        {
            var model = work.SystemInfoRepository.Get().FirstOrDefault();
            if (model != null)
            {
                return model;
            }
            return new SystemInfo();
        }

        #endregion
    }
}
