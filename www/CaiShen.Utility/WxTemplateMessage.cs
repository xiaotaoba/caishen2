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
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin;
using Senparc.Weixin.MP.CommonAPIs;
using System.Web.Configuration;
using Senparc.Weixin.MP.AdvancedAPIs;

/// <summary>
/// Common 的摘要说明
/// </summary>
/// 
namespace Pannet.Utility
{
    public class WxTemplateMessage
    {
        private static object lockobj = new object();
        private static bool OpenWeixinStatus = true;
        private static string wxConfigPath = "Config/weixin.config";
        private static string appid = ConfigHelper.GetConfigValue(wxConfigPath, "wx:AppID");//WebConfigurationManager.AppSettings["WeixinAppId"];
        private static string appsecret = ConfigHelper.GetConfigValue(wxConfigPath, "wx:AppSecret");// WebConfigurationManager.AppSettings["WeixinAppSecret"];
        private static string templateid = "1nRIIyXb5fJTs2pl3amyKX-jqyMBsrSEoPW86RIlCFY";
        public WxTemplateMessage()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 发送购买成功通知
        /// </summary>
        /// <param name="opendid"></param>
        /// <param name="product"></param>
        /// <param name="price"></param>
        /// <param name="time"></param>
        /// <param name="code"></param>
        /// <param name="isonline"></param>
        public static void SendMessage(string opendid, string product, string price, string time, string code, bool isonline)
        {
            string logpath = "wxtemplate";
            string logfilename = DateTime.Now.ToString("yyyyMMdd");
            if (OpenWeixinStatus)//开启微信发送
            {

                SendTemplateMessageResult rs = null;
                try
                {
                    OrderTemplateModel data = new OrderTemplateModel();
                    data.first = new TemplateDataItem("您好，谢谢购物。");
                    data.product = new TemplateDataItem(product);
                    data.price = new TemplateDataItem(price + "元");
                    data.time = new TemplateDataItem(Convert.ToDateTime(time).ToString("yyyy年MM月dd日 HH时mm分"));
                    if (isonline)
                    {
                        data.remark = new TemplateDataItem("祝您生活愉快！");
                    }
                    else
                    {
                        data.remark = new TemplateDataItem("消费码：" + code);
                    }

                    //发送考勤模板消息
                    //rs = TemplateApi.SendTemplateMessage(appid, opendid, templateid, "#ff0000", "", data);
                    rs = TemplateApi.SendTemplateMessage(appid, opendid, templateid, "http://wx.test.com/MyOrder_P.aspx", data);
                }
                catch (Exception ex1)
                {
                    Log.WriteLog(":错误(" + ex1.Message + ")\r\n", logpath, logfilename);
                    return;
                }
                if (rs.errcode == ReturnCode.请求成功)
                {
                    Log.WriteLog(":微信消息发送成功!\r\n", logpath, logfilename);

                }
                else
                {
                    Log.WriteLog(":微信消息发送失败:" + rs.errmsg + "!\r\n", logpath, logfilename);
                }
            }
            else
            {
                Log.WriteLog(":已关闭微信发送！\r\n", logpath, logfilename);
            }
        }

        /// <summary>
        /// 推送充值消息
        /// </summary>
        /// <param name="opendid"></param>
        /// <param name="price"></param>
        /// <param name="totalPrice"></param>
        public static void SendRechargeMessage(string opendid, string price, string totalPrice)
        {
            string logpath = "wxtemplate";
            string logfilename = DateTime.Now.ToString("yyyyMMdd");
            if (OpenWeixinStatus)//开启微信发送
            {
                //string access_token = AccessTokenContainer.TryGetToken(appid, appsecret);
                //string access_token = AccessTokenContainer.TryGetAccessToken(appid, appsecret);
                SendTemplateMessageResult rs = null;
                try
                {
                    RechargeTemplateModel data = new RechargeTemplateModel();
                    data.first = new TemplateDataItem("您好，您已经充值成功。");
                    data.keyword1 = new TemplateDataItem(price + "元");
                    data.keyword2 = new TemplateDataItem(DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分"));
                    data.keyword3 = new TemplateDataItem(totalPrice + "元");
                    data.remark = new TemplateDataItem("祝您生活愉快！");


                    //发送模板消息
                    //rs = TemplateApi.SendTemplateMessage(appid, opendid, "W_pT0wFXymuwCp1EV2WQnFtivRr9-0Mc-09W8Br-6iI", "#ff0000", "", data);
                    rs = TemplateApi.SendTemplateMessage(appid, opendid, "W_pT0wFXymuwCp1EV2WQnFtivRr9-0Mc-09W8Br-6iI", "http://wx.hangzhouyuhan.com/MyJiaoyi.aspx", data);

                }
                catch (Exception ex1)
                {
                    Log.WriteLog(":错误(" + ex1.Message + ")\r\n", logpath, logfilename);
                    return;
                }
                if (rs.errcode == ReturnCode.请求成功)
                {
                    Log.WriteLog(":微信消息发送成功!\r\n", logpath, logfilename);

                }
                else
                {
                    Log.WriteLog(":微信消息发送失败:" + rs.errmsg + "!\r\n", logpath, logfilename);
                }

                // }
            }
            else
            {
                Log.WriteLog(":已关闭微信发送！\r\n", logpath, logfilename);
            }
        }

        /// <summary>
        /// 发送《会议回播》消息
        /// </summary>
        /// <param name="opendid"></param>
        /// <param name="first"></param>
        /// <param name="keyword1"></param>
        /// <param name="keyword2"></param>
        /// <param name="remark"></param>
        /// <param name="url"></param>
        public static void SendHuiBoMessage(string opendid, string first, string keyword1, string keyword2, string remark, string url)
        {
            string logpath = "wxtemplate";
            string logfilename = DateTime.Now.ToString("yyyyMMdd");
            if (OpenWeixinStatus)//开启微信发送
            {
                //string access_token = AccessTokenContainer.TryGetToken(appid, appsecret);
                //string access_token = AccessTokenContainer.TryGetAccessToken(appid, appsecret);
                //Eyooyoo.Model.Ks_Templates modelTemp = Eyooyoo.Control.Templates.GetModel(userinfo.User_Id, 1);

                SendTemplateMessageResult rs = null;
                try
                {
                    HuiBoTemplateModel data = new HuiBoTemplateModel();
                    data.first = new TemplateDataItem(first);
                    data.keyword1 = new TemplateDataItem(keyword1);
                    data.keyword2 = new TemplateDataItem(keyword2);
                    data.remark = new TemplateDataItem(remark);

                    //rs = TemplateApi.SendTemplateMessage(appid, opendid, templateid, "#ff0000", "", data);
                    rs = TemplateApi.SendTemplateMessage(appid, opendid, templateid, url, data);
                }
                catch (Exception ex1)
                {
                    Log.WriteLog(":错误(" + ex1.Message + ")\r\n", logpath, logfilename);
                    return;
                }
                if (rs.errcode == ReturnCode.请求成功)
                {
                    Log.WriteLog(":微信消息发送成功!\r\n", logpath, logfilename);

                }
                else
                {
                    Log.WriteLog(":微信消息发送失败:" + rs.errmsg + "!\r\n", logpath, logfilename);
                }

                // }
            }
            else
            {
                Log.WriteLog(":已关闭微信发送！\r\n", logpath, logfilename);
            }
        }

        /// <summary>
        /// 课程开课通知
        /// </summary>
        /// <param name="opendid"></param>
        /// <param name="first"></param>
        /// <param name="keyword1">标题</param>
        /// <param name="keyword2">内容</param>
        /// <param name="keyword3">主讲</param>
        /// <param name="keyword4">时间</param>
        /// <param name="remark">结束语</param>
        /// <param name="url">链接</param>
        public static void SendNewCourseMessage(string opendid, string first, string keyword1, string keyword2, string keyword3, string keyword4, string remark, string url)
        {
            string logpath = "wxtemplate";
            string logfilename = DateTime.Now.ToString("yyyyMMdd");
            if (OpenWeixinStatus)//开启微信发送
            {
                SendTemplateMessageResult rs = null;
                try
                {
                    NewCourseTemplateModel data = new NewCourseTemplateModel();
                    data.first = new TemplateDataItem(first);
                    data.keyword1 = new TemplateDataItem(keyword1);
                    data.keyword2 = new TemplateDataItem(keyword2);
                    data.keyword3 = new TemplateDataItem(keyword3);
                    data.keyword4 = new TemplateDataItem(keyword4);
                    if (string.IsNullOrEmpty(remark))
                    {
                        remark = "请积极参与，感谢您的支持！";
                    }
                    data.remark = new TemplateDataItem(remark);

                    rs = TemplateApi.SendTemplateMessage(appid, opendid, "ENxLen4UgyXwxQYbfh5WMMH0fGDm9cvarNPUHgU9tQM", url, data);
                }
                catch (Exception ex1)
                {
                    Log.WriteLog("SendNewCourseMessage:错误(" + ex1.Message + ")\r\n", logpath, logfilename);
                    return;
                }
                if (rs.errcode == ReturnCode.请求成功)
                {
                    Log.WriteLog("SendNewCourseMessage:微信消息发送成功!\r\n", logpath, logfilename);

                }
                else
                {
                    Log.WriteLog("SendNewCourseMessage:微信消息发送失败:" + rs.errmsg + "!\r\n", logpath, logfilename);
                }

                // }
            }
            else
            {
                Log.WriteLog(":已关闭微信发送！\r\n", logpath, logfilename);
            }
        }
        /// <summary>
        /// 培训报名通知
        /// </summary>
        /// <param name="opendid"></param>
        /// <param name="first"></param>
        /// <param name="keyword1">名称</param>
        /// <param name="keyword2">地点</param>
        /// <param name="keyword3">开始时间</param>
        /// <param name="keyword4">结束时间</param>
        /// <param name="keyword5">培训内容</param>
        /// <param name="remark"></param>
        /// <param name="url"></param>
        public static void SendPeiXunMessage(string opendid, string first, string keyword1, string keyword2, string keyword3, string keyword4, string keyword5, string remark, string url)
        {
            string logpath = "wxtemplate";
            string logfilename = DateTime.Now.ToString("yyyyMMdd");
            if (OpenWeixinStatus)//开启微信发送
            {
                SendTemplateMessageResult rs = null;
                try
                {
                    PeiXunTemplateModel data = new PeiXunTemplateModel();
                    data.first = new TemplateDataItem(first);
                    data.keyword1 = new TemplateDataItem(keyword1);
                    data.keyword2 = new TemplateDataItem(keyword2);
                    data.keyword3 = new TemplateDataItem(keyword3);
                    data.keyword4 = new TemplateDataItem(keyword4);
                    data.keyword5 = new TemplateDataItem(keyword5);
                    data.remark = new TemplateDataItem(remark);

                    rs = TemplateApi.SendTemplateMessage(appid, opendid, "3pkUhqzt7PBdwWBygvQ68qJqGU2HPiP2P1qBCRLbdfU", url, data);
                }
                catch (Exception ex1)
                {
                    Log.WriteLog("SendPeiXunMessage:错误(" + ex1.Message + ")\r\n", logpath, logfilename);
                    return;
                }
                if (rs.errcode == ReturnCode.请求成功)
                {
                    Log.WriteLog("SendPeiXunMessage:微信消息发送成功!\r\n", logpath, logfilename);

                }
                else
                {
                    Log.WriteLog("SendPeiXunMessage:微信消息发送失败:" + rs.errmsg + "!\r\n", logpath, logfilename);
                }

                // }
            }
            else
            {
                Log.WriteLog(":已关闭微信发送！\r\n", logpath, logfilename);
            }
        }
    }

    #region 考勤模板内容实体

    /// <summary>
    /// 考勤模板内容实体
    /// </summary>
    public class KaoqinTemplateModel
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem childName { get; set; }
        public TemplateDataItem time { get; set; }
        public TemplateDataItem status { get; set; }
        public TemplateDataItem remark { get; set; }
    }

    #endregion

    #region 购买成功通知实体

    /// <summary>
    /// 购买成功通知实体
    /// </summary>
    public class OrderTemplateModel
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem product { get; set; }
        public TemplateDataItem price { get; set; }
        public TemplateDataItem time { get; set; }
        public TemplateDataItem remark { get; set; }
    }

    #endregion

    #region 充值成功通知实体

    /// <summary>
    /// 充值成功通知实体
    /// </summary>
    public class RechargeTemplateModel
    {
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 充值时间
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 账户总额
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem remark { get; set; }
    }

    #endregion

    #region 会议结束通知

    /// <summary>
    /// 会议结束通知-会议回播
    /// </summary>
    public class HuiBoTemplateModel
    {
        /// <summary>
        /// 导语
        /// </summary>
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 结束语
        /// </summary>
        public TemplateDataItem remark { get; set; }
    }

    #endregion

    #region 课程开课通知

    /// <summary>
    ///课程开课通知
    /// </summary>
    public class NewCourseTemplateModel
    {
        /// <summary>
        /// 导语
        /// </summary>
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 主讲
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public TemplateDataItem keyword4 { get; set; }
        /// <summary>
        /// 结束语
        /// </summary>
        public TemplateDataItem remark { get; set; }
    }

    #endregion

    #region 培训通知

    /// <summary>
    ///培训通知
    /// </summary>
    public class PeiXunTemplateModel
    {
        /// <summary>
        /// 导语
        /// </summary>
        public TemplateDataItem first { get; set; }
        /// <summary>
        /// 培训名称
        /// </summary>
        public TemplateDataItem keyword1 { get; set; }
        /// <summary>
        /// 培训地点
        /// </summary>
        public TemplateDataItem keyword2 { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public TemplateDataItem keyword3 { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public TemplateDataItem keyword4 { get; set; }
        /// <summary>
        /// 培训内容
        /// </summary>
        public TemplateDataItem keyword5 { get; set; }
        /// <summary>
        /// 结束语 
        /// </summary>
        public TemplateDataItem remark { get; set; }
    }

    #endregion
}