using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Pannet.DAL;
using Pannet.DAL.Repository;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using StackExchange.Profiling.EntityFramework6;
using System.Data.Entity;
using Pannet.Utility;
using System.Threading;
using System.Net;
using System.IO;

namespace Pannet.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //MiniProfilerEF6.Initialize();
            Database.SetInitializer<PannetContext>(null);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            //定义定时器  
            int time = 1000 * 60 * 10; //6小时刷新一次6 * 60 * 60 * 1000;
            System.Timers.Timer myTimer = new System.Timers.Timer(time);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;

        }

        void myTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                Log.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":AutoTask is Working!", "定时任务", DateTime.Now.ToString("yyyyMMdd"));
                MyTask();
            }

            catch (Exception ee)
            {
                Log.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ee.ToString(), "定时任务", DateTime.Now.ToString("yyyyMMdd"));
            }

        }
        /// <summary>
        /// 我的定时任务
        /// </summary>
        void MyTask()
        {
            UserService.FanJiFen();
        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
            string appid = WebSiteConfig.WeixinAppId;
            string appsecret = WebSiteConfig.WeixinAppSecret;
            //if (!Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.CheckRegistered(appid))
            //{
            //    Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.Register(appid, appsecret);
            //}
            if (!Senparc.Weixin.MP.Containers.AccessTokenContainer.CheckRegistered(appid))
            {
                Senparc.Weixin.MP.Containers.AccessTokenContainer.Register(appid, appsecret);
            }
        }
        protected virtual void Application_BeginRequest()
        {
            //MiniProfiler.Start();
        }

        protected virtual void Application_EndRequest()
        {
            //MiniProfiler.Stop();

            //  在应用程序关闭时运行的代码
           // Log.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":Application End!", "定时任务", DateTime.Now.ToString("yyyyMMdd"));

            ////下面的代码是关键，可解决IIS应用程序池自动回收的问题  
            //Thread.Sleep(100000);

            //try
            //{
            //    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://caishen.0553666.cn");
            //    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            //    Stream receiveStream = myHttpWebResponse.GetResponseStream();
            //}
            //catch
            //{
            //}
        }

    }
}
