using System;
using System.Collections.Specialized;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Pannet.Utility
{

    /// <summary>
    /// web.config操作类
    /// </summary>
    public sealed class ConfigHelper
    {
        public static string CookieUserName
        {
            get
            {
                return GetConfigString("CookieUserName");
            }
        }
        /// <summary>
        /// 过期时间，单位秒
        /// </summary>
        public static Int32 CookieExpries
        {
            get
            {
                string expries = GetConfigString("CookieExpries");
                if (string.IsNullOrEmpty(expries))
                    return 3600;//默认一小时
                return Convert.ToInt32(expries);
            }
        }
        public static string CookieAdminName
        {
            get
            {
                return GetConfigString("CookieAdminName");
            }
        }
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            string CacheKey = "AppSettings-" + key;
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings[key];
                    if (objModel != null)
                    {
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }
                }
                catch
                {
                    return "";
                }
            }
            if (objModel == null)
                return "";
            return objModel.ToString();
        }

        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = bool.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }
            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = decimal.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }
        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = int.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }

        /// <summary>
        /// 写入web.config
        /// </summary>
        /// <param name="item">appSettings等</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void WriteConfig(string item, string key, string value)
        {
            if (item == "")
            {
                item = "appSettings";
            }
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            AppSettingsSection appSection = (AppSettingsSection)config.GetSection(item);
            if (appSection.Settings[key] == null)
            {
                appSection.Settings.Add(key, value);
                config.Save();
            }
            else
            {
                appSection.Settings.Remove(key);
                appSection.Settings.Add(key, value);
                config.Save();
            }
        }

        /// <summary>
        /// 获取自定义 config 文件中的 appsetting 节点值
        /// </summary>
        /// <param name="configPath">如：config/a.config</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValue(string configPath, string key)
        {
            //Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            if (configPath != "")
            {
                string fullConfigPath = AppDomain.CurrentDomain.BaseDirectory + configPath;
                if (!File.Exists(fullConfigPath))
                    throw new Exception(string.Format("配置文件不存在：{0}", fullConfigPath));

                ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
                ecf.ExeConfigFilename = fullConfigPath;
                config = ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);
            }
            return config.AppSettings.Settings[key].Value;
        }
        /// <summary>
        /// /设置自定义 config 文件中的 appsetting 节点值
        /// </summary>
        /// <param name="configPath">如：config/a.config</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetConfigValue(string configPath, string key, string value)
        {

            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            if (configPath != "")
            {
                string fullConfigPath = AppDomain.CurrentDomain.BaseDirectory + configPath;
                if (!File.Exists(fullConfigPath))
                    throw new Exception(string.Format("配置文件不存在：{0}", fullConfigPath));

                ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
                ecf.ExeConfigFilename = fullConfigPath;
                config = ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);
            }

            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Remove(key);
            }
            config.AppSettings.Settings[key].Value = value;
            config.Save();

            return true;
        }
    }
}
