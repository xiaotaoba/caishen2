using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using System.IO;

using Pannet.DAL;
using Pannet.Models;
using Pannet.Utility;

using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.CommonAPIs;
using Pannet.DAL.Repository;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class WeiXinController : Controller
    {
        private UnitOfWork work = new UnitOfWork();
        private string Token = WebSiteConfig.WeixinToken;//与微信公众账号后台的Token设置保持一致，区分大小写。
        private static object lockobj = new object();

        #region 验证 Signature+Token

        public ActionResult Index()
        {
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echostr = Request["echostr"];

            if (Request.HttpMethod == "GET")
            {
                //get method - 仅在微信后台填写URL验证时触发
                if (CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent(echostr); //返回随机字符串则表示验证通过
                }
                else
                {
                    WriteContent("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token) + "。" +
                                "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
                }
                Response.End();
            }
            else
            {
                //post method - 当有用户想公众账号发送消息时触发
                if (!CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent("参数错误！");
                }

                //post method - 当有用户想公众账号发送消息时触发
                var postModel = new PostModel()
                {
                    Signature = Request.QueryString["signature"],
                    Msg_Signature = Request.QueryString["msg_signature"],
                    Timestamp = Request.QueryString["timestamp"],
                    Nonce = Request.QueryString["nonce"],
                    //以下保密信息不会（不应该）在网络上传播，请注意
                    Token = Token,
                    EncodingAESKey = WebSiteConfig.WeixinEncodingAESKey,//根据自己后台的设置保持一致
                    AppId = WebSiteConfig.WeixinAppId//根据自己后台的设置保持一致
                };

                //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
                var maxRecordCount = 10;

                //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                var messageHandler = new CustomMessageHandler(Request.InputStream, postModel, maxRecordCount);

                try
                {
                    //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                    messageHandler.RequestDocument.Save(
                        Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Request_" +
                                       messageHandler.RequestMessage.FromUserName + ".txt"));
                    //执行微信处理过程
                    messageHandler.Execute();
                    //测试时可开启，帮助跟踪数据
                    messageHandler.ResponseDocument.Save(
                        Server.MapPath("~/App_Data/" + DateTime.Now.Ticks + "_Response_" +
                                       messageHandler.ResponseMessage.ToUserName + ".txt"));
                    if (messageHandler.ResponseDocument.ToString().Contains("paihang"))
                    {
                        WriteContent("<xml><ToUserName><![CDATA[" + messageHandler.RequestMessage.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + messageHandler.RequestMessage.ToUserName + "]]></FromUserName><CreateTime>1528270728</CreateTime><MsgType><![CDATA[hardware]]></MsgType><HardWare><MessageView><![CDATA[myrank]]></MessageView><MessageAction><![CDATA[ranklist]]></MessageAction></HardWare><FuncFlag>0</FuncFlag></xml>");
                    }
                    else
                    {
                        WriteContent(messageHandler.ResponseDocument.ToString());
                    }
                }
                catch (Exception ex)
                {
                    using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt")))
                    {
                        tw.WriteLine(ex.Message);
                        tw.WriteLine(ex.InnerException.Message);
                        if (messageHandler.ResponseDocument != null)
                        {
                            tw.WriteLine(messageHandler.ResponseDocument.ToString());
                        }
                        tw.Flush();
                        tw.Close();
                    }
                }
                finally
                {
                    Response.End();
                }
            }

            return View();
        }

        private void WriteContent(string str)
        {
            Response.Output.Write(str);
        }

        /// <summary>
        /// 最简单的Page_Load写法（本方法仅用于演示过程，针对未加密消息，未实际在DEMO演示中使用到）
        /// </summary>
        private void MiniProcess()
        {
            string signature = Request["signature"];
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];
            string echostr = Request["echostr"];

            if (Request.HttpMethod == "GET")
            {
                //get method - 仅在微信后台填写URL验证时触发
                if (CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent(echostr); //返回随机字符串则表示验证通过
                }
                else
                {
                    WriteContent("failed:" + signature + "," + CheckSignature.GetSignature(timestamp, nonce, Token));
                }

            }
            else
            {
                //post method - 当有用户想公众账号发送消息时触发
                if (!CheckSignature.Check(signature, timestamp, nonce, Token))
                {
                    WriteContent("参数错误！");
                }

                //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                var messageHandler = new CustomMessageHandler(Request.InputStream, null);
                //执行微信处理过程
                messageHandler.Execute();
                //输出结果
                WriteContent(messageHandler.ResponseDocument.ToString());
            }
            Response.End();
        }

        #endregion

        #region Oauth 2.0

        /// <summary>
        /// 微信网页授权验证
        /// </summary>
        /// <param name="state">回传请求标识-已URL编码 </param>
        /// <param name="u">推荐人</param>
        /// <returns></returns>
        public ActionResult GotoOauth(string state, string u)
        {
            string agent = Request.ServerVariables["Http_User_Agent"].ToLower();
            Log.WriteLog("GotoOauth，agent:" + agent, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
            //if (agent.Contains("micromessenger"))//如果是微信客户端打开，直接使用微信登录或注册
            //{
            string appid = WebSiteConfig.WeixinAppId;
            string rediectUrl = WebSiteConfig.WebSiteDomain + "/Mobile/WeiXin/Oauth";
            if (!string.IsNullOrEmpty(u))
            {
                CookieHelper.SetValue("u", u, ConfigHelper.CookieExpries);
            }
            if (string.IsNullOrEmpty(state))
            {
                //state = Server.UrlEncode(Request.Url.ToString());
                //state = Server.UrlEncode(string.Format("{0}/Mobile/Login", WebSiteConfig.WebSiteDomain));
                state = Server.UrlEncode(string.Format("{0}/Mobile/Login", WebSiteConfig.WebSiteDomain));
            }

            //state = Server.UrlEncode(state);
            Response.Redirect(OAuthApi.GetAuthorizeUrl(appid, rediectUrl, state, OAuthScope.snsapi_userinfo));
            //}
            //return null;
            return null;
        }

        public void Oauth()
        {
            lock (lockobj)
            {
                if (Request.QueryString["code"] != null && Request.QueryString["code"].ToString() != "" && string.IsNullOrEmpty(CookieHelper.GetValue("openid")))//已登录不允许进入
                {
                    string code = Request.QueryString["code"].ToString();
                    string refer = "";
                    string url = "";
                    string state = Request.QueryString["state"];
                    object cacheCodeObj = DataCache.GetCache("code");
                    string cacheCode = cacheCodeObj == null ? "" : cacheCodeObj.ToString();
                    string u = CookieHelper.GetValue("u");//保留2小时
                    if (Request.UrlReferrer != null)
                    {
                        refer = Request.UrlReferrer.ToString();
                    }
                    if (Request.Url != null)
                    {
                        url = Request.Url.ToString();
                    }
                    Log.WriteLog("进入 oauth2, code:" + code + ",url:" + url + ",refer:" + refer + ",state:" + state + ",cacheCode:" + cacheCode + ",u:" + u, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                    //Response.Write(code);
                    //获得access_token
                    string appid = WebSiteConfig.WeixinAppId;
                    string appsecret = WebSiteConfig.WeixinAppSecret;

                    //if (Session["code"] != null && code == Session["code"].ToString())
                    //{
                    //    Log.WriteLog("相同code:" + code + ",url:" + url + ",refer:" + refer, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                    //    return;
                    //}
                    //else
                    //{
                    //    Session["code"] = code;
                    //    Log.WriteLog("不相同code:" + code + ",url:" + url + ",refer:" + refer, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                    //}
                    if (!string.IsNullOrEmpty(cacheCode) && code == cacheCode)
                    {
                        Log.WriteLog("相同code:" + code, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                        return;
                    }
                    else
                    {
                        DataCache.SetCache("code", code);
                        Log.WriteLog("不相同code:" + code, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                    }


                    //根据oauth接口获取openid
                    OAuthAccessTokenResult tokenResult = OAuthApi.GetAccessToken(appid, appsecret, code);
                    string openid = tokenResult.openid;
                    Log.WriteLog("获得openid:" + openid, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                    CookieHelper.SetValue("openid", openid, ConfigHelper.CookieExpries);
                    //获取微信用户信息
                    OAuthUserInfo authUser = OAuthApi.GetUserInfo(tokenResult.access_token, openid);
                    if (authUser != null)
                    {
                        Log.WriteLog("获取微信用户信息：昵称" + authUser.nickname + ",headimgurl:" + authUser.headimgurl, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                        CookieHelper.SetValue("wx_headimgurl", authUser.headimgurl, ConfigHelper.CookieExpries);
                        CookieHelper.SetValue("wx_nickname", authUser.nickname, ConfigHelper.CookieExpries);
                    }

                    #region 自动创建用户

                    ////是否存在记录
                    //User user = work.Context.Users.Where(m => m.U_OpenId == openid).FirstOrDefault();
                    //if (user == null)//不存在，当前微信公众号未注册
                    //{
                    //    ////根据高级接口获取全局access_token
                    //    ////string access_token = AccessTokenContainer.TryGetAccessToken(appid, appsecret);
                    //    //UserInfoJson userJson = UserApi.Info(appid, openid);

                    //    //user = new User();
                    //    //if (userJson.subscribe == 0)//未关注
                    //    //{
                    //    //    Log.WriteLog("未注册-未关注openid:" + openid + ",url:" + url + ",refer:" + refer, "oauth", DateTime.Now.ToString("yyyyMMddHH"));

                    //    //    user.U_OpenId = openid;
                    //    //    user.U_Gender = 0;
                    //    //    user.U_Thumbnail = "";
                    //    //}
                    //    //else//已关注
                    //    //{
                    //    //    Log.WriteLog("未注册-已关注openid:" + openid + ",url:" + url + ",refer:" + refer, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                    //    //    user.U_OpenId = userJson.openid;
                    //    //    //user.WX_subscribe_time = userJson.subscribe_time.ToString();
                    //    //    //user.WX_country = userJson.country;
                    //    //    //user.WX_province = userJson.province;
                    //    //    user.U_Address = userJson.country + userJson.province + userJson.city;
                    //    //    user.U_Thumbnail = userJson.headimgurl;
                    //    //    user.U_Gender = userJson.sex;
                    //    //    user.U_NickName = userJson.nickname;
                    //    //    //user.WX_unionid = userJson.unionid;
                    //    //    //user.WX_groupid = userJson.groupid;
                    //    //    //user.WX_tagid_list = userJson.tagid_list.ToString();
                    //    //}

                    //    //user.U_UserName = openid;
                    //    //user.UserRoleID = Convert.ToInt16(DataConfig.RoleEnum.注册会员);
                    //    //user.UserLevelID = Convert.ToInt16(DataConfig.LevelEnum.ZhuCe);
                    //    //user.U_Is_Check = 1;
                    //    //user.U_Is_Enable = 1;
                    //    //user.UserShopID = 0;
                    //    //user.U_CreateTime = DateTime.Now;
                    //    //string referrer = CookieHelper.GetValue("u");//保留2小时
                    //    //if (string.IsNullOrEmpty(referrer))
                    //    //{
                    //    //    user.Referrer = 0;
                    //    //}
                    //    //else
                    //    //{
                    //    //    user.Referrer = Convert.ToInt16(referrer);
                    //    //    //有推荐人，发放推荐奖励
                    //    //    User referUser = work.UserRepository.GetByID(Convert.ToInt16(referrer));
                    //    //    if (referUser != null)
                    //    //    {
                    //    //        //签到送积分
                    //    //        int score = Convert.ToInt16(ConfigHelper.GetConfigString("ShareScore"));
                    //    //        referUser.U_Score = referUser.U_Score + score;
                    //    //        work.UserRepository.Update(referUser);

                    //    //        UserScoreHistoryService.Insert(referUser.ID, score, referUser.U_Score, 0, referUser.U_LockScore, 1, "收入", "分享奖励积分", referUser.ID, user.U_UserName);
                    //    //    }
                    //    //}
                    //    //work.UserRepository.Insert(user);
                    //    //work.Save();
                    //    //user.U_UserName = "HY" + user.ID;
                    //    //work.UserRepository.Update(user);
                    //    //work.Save();

                    //    ////    #region 添加会员推荐关系

                    //    ////    //string referrer = state;
                    //    ////    //if (referrer == "yh" || (!string.IsNullOrEmpty(referrer) && referrer.Contains("do_")))//默认state，不作为推荐人
                    //    ////    //{
                    //    ////    //    referrer = "";
                    //    ////    //}
                    //    ////    //添加会员推荐关系
                    //    ////    //userinfo.Referrer = referrer;
                    //    ////    //if (!string.IsNullOrEmpty(referrer))
                    //    ////    //{
                    //    ////    //    WangZhicn.Control.MemberShip.Add(referrer, userinfo.UserLoginId);
                    //    ////    //}
                    //    ////    //if (userJson.subscribe == 0)//未关注
                    //    ////    //{
                    //    ////    //    Response.Redirect(WangZhicn.Common.WebURL + "/info.aspx?id=5");//提示未关注
                    //    ////    //    Response.End();
                    //    ////    //}
                    //    ////    //else
                    //    ////    //{
                    //    ////    //    CheckStateRedirect(userinfo, state);
                    //    ////    //}
                    //    ////    #endregion

                    //    CheckStateRedirect(user, state);

                    //}
                    //else
                    //{
                    //    Log.WriteLog("已注册openid:" + openid, "oauth", DateTime.Now.ToString("yyyyMMddHH"));

                    //    //如果头像为空(还没关注公众号)
                    //    if (string.IsNullOrEmpty(user.U_Thumbnail))
                    //    {
                    //        Log.WriteLog("无头像", "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                    //        //根据高级接口获取全局access_token
                    //        //string access_token = AccessTokenContainer.TryGetAccessToken(appid, appsecret);
                    //        UserInfoJson userJson = UserApi.Info(appid, openid);
                    //        if (userJson.subscribe == 0)//未关注
                    //        {
                    //            //Log.WriteLog("未关注", "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                    //            //Response.Redirect(WebSiteConfig.WebSiteDomain + "/info.aspx?id=5");//提示未关注
                    //            //Response.End();
                    //            CheckStateRedirect(user, state);
                    //        }
                    //        else
                    //        {
                    //            Log.WriteLog("已关注，更新头像", "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                    //            //更新头像和昵称
                    //            //wxUser.WX_country = userJson.country;
                    //            //wxUser.WX_province = userJson.province;
                    //            //wxUser.WX_city = userJson.city;
                    //            //wxUser.WX_sex = userJson.sex;
                    //            //wxUser.WX_nickname = userJson.nickname;
                    //            user.U_Thumbnail = userJson.headimgurl;

                    //            work.UserRepository.Update(user);
                    //            work.Save();
                    //        }
                    //    }

                    //    CheckStateRedirect(user, state);
                    //}
                    #endregion

                    var rstUser = work.UserRepository.Get(m => m.U_OpenId == openid);
                    if (rstUser != null && rstUser.Count() > 0)
                    {
                        CheckStateRedirect(rstUser.First(), state);
                    }
                    else
                    {
                        NewUser(openid, authUser, state);
                        //CheckStateRedirect(null, state);
                    }
                }
            }
            CheckStateRedirect(null, "");
            //return View();
        }
        /// <summary>
        /// 判断state并做跳转处理
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="state"></param>
        private void CheckStateRedirect(User userModel, string state)
        {
            //Session["wxUser"] = wxUser;
            //CookieHelper.SetValue(ConfigHelper.CookieWeiXinOpenidName, wxUser.WX_openid, 7200);//保留2小时登录名

            ////领取优惠券
            //if (!string.IsNullOrEmpty(state) && state.Contains("do_getq_"))
            //{
            //    Response.Redirect("GetQuan.aspx?Q_ID=" + state.Replace("do_getq_", ""));
            //}
            //else
            //{
            //    Response.Redirect("/Home/Index");
            //}
            //if (string.IsNullOrEmpty(state) || state == "pannet")
            //{
            //    Response.Redirect("/Home/Index");
            //}
            //else
            //{
            //    Response.Redirect(state);
            //}

            if (userModel != null)
            {
                Log.WriteLog("已绑定用户：" + userModel.U_UserName, "oauth", DateTime.Now.ToString("yyyyMMddHH"));
                CookieHelper.SetValue(ConfigHelper.CookieUserName, userModel.U_UserName, ConfigHelper.CookieExpries);
                ////登录更新缓存
                //UserService.SetCacheUser(userModel.U_UserName, userModel);
            }

            //if (!string.IsNullOrEmpty(state))
            //{
            //    Response.Redirect(Server.UrlDecode(state));
            //}
            //else
            //{
            //    Response.Redirect("/Mobile/Member");
            //}
            Response.Redirect("/shen/?openid=" + CookieHelper.GetValue("openid"));
            Response.End();
        }


        #region 创建新用户

        private void NewUser(string openid, OAuthUserInfo authUser, string state)
        {
            User user = work.Context.Users.Where(m => m.U_OpenId == openid).FirstOrDefault();
            if (user == null)//不存在，当前微信公众号未注册
            {
                user = new User();
                user.U_UserName = openid;
                user.UserRoleID = Convert.ToInt16(DataConfig.RoleEnum.注册会员);
                user.UserLevelID = Convert.ToInt16(DataConfig.LevelEnum.ZhuCe);
                user.U_Is_Check = 0;
                user.U_Is_Enable = 1;
                user.U_CreateTime = DateTime.Now;
                user.U_IsDelete = 0;
                if (authUser != null)
                {
                    user.U_NickName = string.IsNullOrEmpty(authUser.nickname) ? "未设置" : authUser.nickname;
                    user.U_RealName = string.IsNullOrEmpty(authUser.nickname) ? "未设置" : authUser.nickname;
                    user.U_Thumbnail = string.IsNullOrEmpty(authUser.headimgurl) ? "#" : authUser.headimgurl;
                }
                else
                {
                    user.U_NickName = "未设置";
                    user.U_RealName = "未设置";
                    user.U_Thumbnail = "#";
                }
                string referrer = CookieHelper.GetValue("u");//保留2小时
                if (string.IsNullOrEmpty(referrer))
                {
                    user.Referrer = 0;
                }
                else
                {
                    user.Referrer = Convert.ToInt32(referrer);
                }
                user.U_OpenId = openid;

                work.UserRepository.Insert(user);
                work.Save();
                user.U_UserName = "HY" + user.ID;
                work.UserRepository.Update(user);
                work.Save();

            }
            CheckStateRedirect(user, state);
        }

        #endregion


        #endregion

    }
}