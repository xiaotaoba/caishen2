using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.IO;

/// <summary>
/// Common 的摘要说明
/// </summary>
/// 
namespace Pannet.Utility
{
    public class Log
    {
        private static object lockobj = new object();
        public Log()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        #region 写日志

        /// <summary>
        /// 写日志(用于跟踪)
        /// </summary>
        public static void WriteLog(string content, string directory, string filename)
        {
            lock (lockobj)
            {
                string filepath = AppDomain.CurrentDomain.BaseDirectory + string.Format("logs/{0}/{1}.txt", directory, filename);
                string fullDirectory = AppDomain.CurrentDomain.BaseDirectory + string.Format("logs/{0}/", directory);
                if (!Directory.Exists(fullDirectory))
                    Directory.CreateDirectory(fullDirectory);
                StreamWriter sr = null;
                try
                {
                    if (!File.Exists(filepath))
                    {
                        sr = File.CreateText(filepath);
                    }
                    else
                    {
                        sr = File.AppendText(filepath);
                    }
                    sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",user:"+ CookieHelper.GetValue(ConfigHelper.CookieUserName)+" ——>" + ":" + content);
                }
                catch
                {
                }
                finally
                {
                    if (sr != null)
                        sr.Close();
                }
            }
        }

        #endregion

    }
}