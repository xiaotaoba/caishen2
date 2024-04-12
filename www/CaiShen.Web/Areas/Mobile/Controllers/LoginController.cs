using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Utility;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using System.Text;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class LoginController : Controller
    {
        public UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        #region 登录页面

        public ActionResult Index(string returnurl, int role = 3)
        {
            ViewBag.returnurl = returnurl;
            ViewBag.Role = role;
            string rememberPwd = CookieHelper.GetValue("RememberPwd");
            if (!string.IsNullOrEmpty(rememberPwd))
            {
                ViewBag.Pwd = Utility.UtilityClass.Decrypt(rememberPwd);
            }

            //微信登录，并绑定账号，自动登录
            string openid = CookieHelper.GetValue("openid");
            if (!string.IsNullOrEmpty(openid))
            {
                var rstUser = work.UserRepository.Get(m => m.U_OpenId == openid && m.U_IsDelete == 0 && m.U_Is_Enable == 1);
                if (rstUser != null && rstUser.Count() > 0)
                {
                    CheckStateRedirect(rstUser.First());
                }
            }

            string agent = Request.ServerVariables["Http_User_Agent"].ToLower();
            //微信未授权，跳转至授权页面
            //string openid = CookieHelper.GetValue("openid");
            if (agent.Contains("micromessenger") && string.IsNullOrEmpty(openid))
            {
                Response.Redirect(Url.Action("GotoOauth", "WeiXin", new { state = Request.Url.ToString() }));
                Response.End();
            }

            return View();
        }

        public ActionResult Login(string returnurl, int role = 3)
        {
            ViewBag.returnurl = returnurl;
            ViewBag.Role = role;
            return RedirectToAction("Index", new { returnurl, role });
        }
        #endregion

        #region 判断state并做跳转处理

        /// <summary>
        /// 判断state并做跳转处理
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="state"></param>
        private void CheckStateRedirect(User userModel)
        {
            if (userModel != null)
            {
                CookieHelper.SetValue(ConfigHelper.CookieUserName, userModel.U_UserName, ConfigHelper.CookieExpries);
                //登录更新缓存
                UserService.SetCacheUser(userModel.U_UserName, userModel);

                Response.Redirect("/Mobile/Member");
                Response.End();
            }
        }

        #endregion

        #region 登录处理

        /// <summary>
        /// 登录处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(UserLoginVModel loginModel, string returnurl, int role = 3)
        {
            ViewBag.returnurl = returnurl;
            ViewBag.Role = role;
            string openid = CookieHelper.GetValue("openid");

            if (ModelState.IsValid)
            {
                try
                {
                    string md5Password = UtilityClass.GetMD5(loginModel.Password);
                    //var user = context.Users.Where(u => u.U_UserName == username & u.U_Pwd == password).Select(p => p.UserRoles.Where(ur => ur.RoleID == role)).FirstOrDefault();
                    //var user = work.Context.Users.Where(u => u.U_UserName == username & u.U_Pwd == password).Include(u => u.UserRoles).ToList();
                    var user = work.UserRepository.Get(u => u.U_UserName == loginModel.UserName && u.U_IsDelete == 0 && u.U_Is_Enable == 1, null, "").ToList<User>();
                    if (user.Count() > 0)
                    {

                        if (user[0].U_Pwd == md5Password)
                        {
                            //if (!string.IsNullOrEmpty(user[0].U_OpenId) && openid != user[0].U_OpenId)//已绑定微信，和目前登录微信不一致
                            //{
                            //    ViewBag.MessageInfo = string.Format("与账号绑定微信不一致，请更换账号");
                            //}
                            //else
                            //{

                                //ViewBag.MessageInfo = string.Format("登录成功！user:{0},role:{1}", user.Count(), user[0].UserRoles.Where(ur => ur.RoleID == role).Count());

                                CookieHelper.SetValue(ConfigHelper.CookieUserName, user[0].U_UserName, ConfigHelper.CookieExpries);

                                //记住密码
                                if (loginModel.IsRememberPwd)
                                {
                                    CookieHelper.SetValue("RememberPwd", Utility.UtilityClass.Encrypt(loginModel.Password), ConfigHelper.CookieExpries);
                                }
                                else
                                {
                                    CookieHelper.Delete("RememberPwd");
                                }

                                if (!string.IsNullOrEmpty(openid) && string.IsNullOrEmpty(user[0].U_OpenId))
                                {
                                    string headimgurl = CookieHelper.GetValue("wx_headimgurl");
                                    string nickname = CookieHelper.GetValue("wx_nickname");

                                    //绑定openid
                                    user[0].U_OpenId = openid;
                                    if (string.IsNullOrEmpty(user[0].U_Thumbnail))
                                    {
                                        user[0].U_Thumbnail = headimgurl;
                                    }
                                    if (string.IsNullOrEmpty(user[0].U_NickName))
                                    {
                                        user[0].U_NickName = nickname;
                                    }
                                    work.UserRepository.Update(user[0]);
                                    work.Save();
                                }

                                //登录更新缓存
                                UserService.SetCacheUser(loginModel.UserName, user[0]);
                                if (md5Password == UtilityClass.GetMD5(ConfigHelper.GetConfigString("DefaultPwd")))//如果是默认密码，转至修改密码；
                                {
                                    return RedirectToAction("InfoPassword", "Member");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(returnurl) && !returnurl.ToLower().Contains("login") && !returnurl.ToLower().Contains("register"))//不是注册页或者登录页进来
                                    {
                                        Response.Redirect(returnurl);
                                    }
                                    else
                                    {

                                        return RedirectToAction("Index", "Member");
                                    }
                                }
                            //}
                        }
                        else
                        {
                            ViewBag.MessageInfo = string.Format("输入密码错误");
                        }
                    }
                    else
                    {
                        ViewBag.MessageInfo = string.Format("手机号未注册！");
                        //ModelState.AddModelError("LoginError", "用户名或密码错误");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.MessageInfo = string.Format("登录失败：{0}", e.Message);
                }
            }
            return View("Index", loginModel);
        }

        #endregion

        #region 忘记密码

        public ActionResult ForgetPwd()
        {
            return View();
        }

        /// <summary>
        /// 忘记密码 -post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //public ActionResult ForgetPwd(UserForgetPwdVModel newModel, string returnurl)
        public ActionResult ForgetPwd(string email, string returnurl)
        {
            ViewBag.returnurl = returnurl;
            ViewBag.email = email;
            //if (ModelState.IsValid)
            //{

            //    User user = work.UserRepository.Get(u => u.U_Phone == newModel.U_Phone).FirstOrDefault();
            //    if (user == null || user.ID == 0)
            //    {
            //        ModelState.AddModelError("U_Phone", string.Format("手机号码不存在！", newModel.U_Phone));
            //        return View(newModel);
            //    }
            //    else
            //    {

            //        if (newModel.U_Pwd != newModel.U_Pwd2)
            //        {
            //            ModelState.AddModelError("U_Pwd", "输入新密码不一致");
            //            return View(newModel);
            //        }

            //        user.U_Pwd = UtilityClass.GetMD5(newModel.U_Pwd);
            //        work.UserRepository.Update(user);
            //        work.Save();
            //        //work.Dispose();

            //        //更新缓存
            //        UserService.SetCacheUser(newModel.U_Phone, user);

            //        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "密码重置成功，请重新<a href='red'>登录</a>！");
            //    }

            //}
            User user = work.UserRepository.Get(u => u.U_Email == email).FirstOrDefault();
            if (string.IsNullOrEmpty(email) || user == null)
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("danger", "邮箱不存在！");
                return View();
            }
            string newPwd = UtilityClass.RndNum(6).ToUpper();
            StringBuilder body = new StringBuilder();
            body.AppendFormat("<p>您好：{0}</p>", user.U_UserName);
            body.AppendFormat("<p>您的新密码是：<b>{0}</b>，请牢记！为了方便记忆，请及时修改密码！</p>", newPwd);
            body.AppendFormat("<p>《卓牧鸟康复》</p>");

            //try
            //{
            MailSender.Send(email, "找回密码，卓牧鸟康复", body.ToString());
            //更新密码
            user.U_Pwd = UtilityClass.GetMD5(newPwd);
            work.UserRepository.Update(user);
            work.Save();

            //MessageBox.ShowAndRedirect(this, "密码已成功发送至" + email + "!", "/Login.aspx");
            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "密码已成功发送至" + email + "!");
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.ShowAndRedirect(this, "密码找回失败，请稍后重试！", "/Login.aspx");
            //    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "密码找回失败，请稍后重试！" + ex.Message);
            //}

            return View();
        }

        #endregion

        #region 是否存在手机号码

        /// <summary>
        /// 是否存在手机号码
        /// </summary>
        /// <returns></returns>
        public ActionResult ExistMobile(string mobile)
        {
            var user = work.UserRepository.Get(u => u.U_Phone == mobile);
            if (user == null || user.Count() < 1)
            {
                json.Data = new { status = "-1", msg = string.Format("手机号码不存在！", mobile) };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }

            json.Data = new { status = "success", msg = "" };

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 退出

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            CookieHelper.Delete(ConfigHelper.CookieUserName);
            CookieHelper.Delete("wx_headimgurl");
            CookieHelper.Delete("wx_nickname");
            CookieHelper.Delete("openid");
            CookieHelper.Delete("u");
            //Response.Redirect("/Mobile/");
        }

        #endregion
    }
}