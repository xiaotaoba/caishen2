using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using System.Data.Entity.Validation;
using System.Text;
using Pannet.Utility;
using System.Web.Caching;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class RegisterController : Controller
    {
        public UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        public ActionResult Index(string returnurl)
        {
            ViewBag.returnurl = returnurl;
            //return View();
            return View("Mobile");
        }
        public ActionResult Register(string returnurl)
        {
            ViewBag.returnurl = returnurl;
            return RedirectToAction("Index", new { returnurl = returnurl });
        }
        public ActionResult RegisterMobile(string returnurl)
        {
            ViewBag.returnurl = returnurl;
            return View("Mobile");
        }

        #region 注册

        /// <summary>
        /// 手机注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Mobile(string returnurl)
        {
            return View();
        }

        /// <summary>
        /// 邮箱注册处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(UserEmailRegisterVModel registerModel, string returnurl)
        {
            ViewBag.returnurl = returnurl;
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session["YX_Login_Code"] == null || Session["YX_Login_Code"].ToString() != registerModel.U_Code)
                    {
                        //ViewBag.MessageInfo = string.Format("验证码不正确！", registerModel.U_Email);
                        ModelState.AddModelError("U_Code", "验证码不正确！");
                    }
                    var user = work.UserRepository.Get(u => u.U_Email == registerModel.U_Email);
                    if (user.Count() > 0)
                    {
                        //ViewBag.MessageInfo = string.Format("邮箱{0}已存在！", registerModel.U_Email);
                        ModelState.AddModelError("U_Email", string.Format("邮箱{0}已存在！", registerModel.U_Email));
                    }
                    else
                    {
                        User userModel = new Models.User();
                        userModel.U_UserName = registerModel.U_Email;
                        userModel.U_NickName = registerModel.U_Email;
                        userModel.U_Email = registerModel.U_Email;
                        userModel.U_Pwd = UtilityClass.GetMD5(registerModel.U_Pwd);
                        userModel.UserRoleID = registerModel.Role;
                        userModel.UserLevelID = Convert.ToInt16(DataConfig.LevelEnum.ZhuCe);
                        userModel.U_Is_Enable = 1;
                        userModel.UserShopID = UserShopService.GetCurrentShopID();
                        ////注册会员默认审核通过，供应商，门店需要审核
                        //if (registerModel.Role == Convert.ToInt16(DataConfig.RoleEnum.注册会员))
                        //{
                        //    userModel.U_Is_Check = 1;
                        //}
                        //else
                        //{
                            userModel.U_Is_Check = 0;
                        //}

                        work.UserRepository.Insert(userModel);
                        work.Save();

                        //20171030 发注册红包
                        HongBaoService.SendHongBao(DataConfig.HongBaoTypeEnum.注册红包, userModel.ID);

                        //work.Dispose();

                        CookieHelper.SetValue(ConfigHelper.CookieUserName, userModel.U_UserName, ConfigHelper.CookieExpries);


                        if (!string.IsNullOrEmpty(returnurl) && !returnurl.ToLower().Contains("login") && !returnurl.ToLower().Contains("register"))//不是注册页或者登录页进来
                        {
                            Response.Redirect(returnurl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Member");
                        }
                    }

                }
                catch (DbEntityValidationException e)
                {
                    //foreach (var eve in e.EntityValidationErrors)
                    //{
                    //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    //    foreach (var ve in eve.ValidationErrors)
                    //    {
                    //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                    //            ve.PropertyName, ve.ErrorMessage);
                    //    }
                    //}
                    //throw;
                    ViewBag.MessageInfo = string.Format("注册失败：{0}", e.Message);

                }
            }
            else
            {
                StringBuilder errinfo = new StringBuilder();
                foreach (var s in ModelState.Values)
                {
                    foreach (var p in s.Errors)
                    {
                        errinfo.AppendFormat("{0}\\n", p.ErrorMessage);
                    }
                }

                ViewBag.MessageInfo = string.Format("验证失败{0}", errinfo.ToString());
            }
            return View("Index");
        }

        /// <summary>
        /// 手机注册处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegisterMobile(UserMobileRegisterVModel registerModel, string returnurl)
        {
            ViewBag.returnurl = returnurl;
            if (ModelState.IsValid)
            {
                try
                {
                    //if (Session["YX_Login_Code"] == null || Session["YX_Login_Code"].ToString() != registerModel.U_Code)
                    //{
                    //    //ViewBag.MessageInfo = string.Format("验证码不正确！", registerModel.U_Email);
                    //    ModelState.AddModelError("U_Code", "验证码不正确！");
                    //}


                    var user = work.UserRepository.Get(u => u.U_UserName == registerModel.U_Phone);
                    if (user.Count() > 0)
                    {
                        //ViewBag.MessageInfo = string.Format("手机号码{0}已存在！", registerModel.U_Phone);
                        User existUser = user.First();
                        if (existUser.U_Is_Enable == 1)
                        {
                            ModelState.AddModelError("U_Phone", "手机号码已启用，请直接登录！");
                        }
                        else
                        {
                            existUser.U_Pwd = UtilityClass.GetMD5(registerModel.U_Pwd);
                            existUser.U_Is_Enable = 1;
                            work.UserRepository.Update(existUser);
                            work.Save();

                            CookieHelper.SetValue(ConfigHelper.CookieUserName, existUser.U_UserName, ConfigHelper.CookieExpries);


                            if (!string.IsNullOrEmpty(returnurl) && !returnurl.ToLower().Contains("login") && !returnurl.ToLower().Contains("register"))//不是注册页或者登录页进来
                            {
                                Response.Redirect(returnurl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Member");
                            }
                        }

                    }
                    //else
                    //{
                    //    ModelState.AddModelError("U_Phone", string.Format("手机号码{0}未登记，暂时无法注册！", registerModel.U_Phone));
                    //}
                    else
                    {
                        User userModel = new Models.User();
                        userModel.U_UserName = registerModel.U_Phone;
                        userModel.U_NickName = registerModel.U_Phone;
                        userModel.U_Phone = registerModel.U_Phone;
                        userModel.U_Pwd = UtilityClass.GetMD5(registerModel.U_Pwd);
                        userModel.UserRoleID = registerModel.Role;
                        userModel.UserLevelID = Convert.ToInt16(DataConfig.LevelEnum.ZhuCe);
                        userModel.U_Is_Check = 0;
                        userModel.U_Is_Enable = 1;
                        userModel.UserShopID = UserShopService.GetCurrentShopID();
                        //注册会员默认审核通过，供应商，门店需要审核
                        //if (registerModel.Role == Convert.ToInt16(DataConfig.RoleEnum.注册会员))
                        //{
                        //    userModel.U_Is_Check = 1;
                        //}
                        //else
                        //{
                        //    userModel.U_Is_Check = 0;
                        //}

                        work.UserRepository.Insert(userModel);
                        work.Save();

                        ////发注册红包
                        //HongBaoService.SendHongBao(DataConfig.HongBaoTypeEnum.注册红包, userModel.ID);

                        //work.Dispose();

                        CookieHelper.SetValue(ConfigHelper.CookieUserName, userModel.U_UserName, ConfigHelper.CookieExpries);


                        if (!string.IsNullOrEmpty(returnurl) && !returnurl.ToLower().Contains("login") && !returnurl.ToLower().Contains("register"))//不是注册页或者登录页进来
                        {
                            Response.Redirect(returnurl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Member");
                        }
                    }

                }
                catch (DbEntityValidationException e)
                {
                    //foreach (var eve in e.EntityValidationErrors)
                    //{
                    //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    //    foreach (var ve in eve.ValidationErrors)
                    //    {
                    //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                    //            ve.PropertyName, ve.ErrorMessage);
                    //    }
                    //}
                    //throw;
                    ViewBag.MessageInfo = string.Format("注册失败：{0}", e.Message);

                }
            }
            return View("Mobile");
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
            if (user.Count() > 0)
            {
                json.Data = new { status = "-1", msg = string.Format("手机号码{0}已存在！", mobile) };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }

            json.Data = new { status = "success", msg = "" };

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 是否存在邮箱

        /// <summary>
        /// 是否存在邮箱
        /// </summary>
        /// <returns></returns>
        public ActionResult ExistEmail(string email)
        {
            var user = work.UserRepository.Get(u => u.U_Email == email);
            if (user.Count() > 0)
            {
                json.Data = new { status = "-1", msg = string.Format("邮箱{0}已存在！", email) };
                return Json(json.Data, JsonRequestBehavior.AllowGet);
            }

            json.Data = new { status = "success", msg = "" };

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 发送手机验证码

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult SendMobileCode(string mobile)
        {
            try
            {
                string code = "";
                if (mobile.Length != 11 || !PageValidate.IsNumber(mobile))
                {
                    json.Data = new { status = "-1", msg = "请输入正确手机号码！" };
                }

                object o = DataCache.GetCache(mobile + "_regcode");
                object o_timer = DataCache.GetCache(mobile + "_regcode_timer");
                if (o != null && o.ToString() != "" && o_timer != null && o_timer.ToString() != "")
                {
                    json.Data = new { status = "-1", msg = "无需重复发送！" };
                }
                else
                {
                    code = Assistant.GetRandomNumber(6);
                    SmsService.SendSms(mobile, code);
                    DataCache.SetCache(mobile + "_regcode", code, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(900));
                    DataCache.SetCache(mobile + "_regcode_timer", "true", Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(60));
                    json.Data = new { status = "success", msg = "验证码已发送！" };
                }
            }
            catch (Exception e)
            {
                json.Data = new { status = "-1", msg = e.Message };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 手机验证码是否正确

        /// <summary>
        /// 手机验证码是否正确
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckMobileCode(string mobile, string code)
        {
            if (mobile.Length != 11 || !PageValidate.IsNumber(mobile))
            {
                json.Data = new { status = "-1", msg = "请输入正确手机号码！" };
            }

            object o = DataCache.GetCache(mobile + "_regcode");
            if (o != null && o.ToString() != "" && code == o.ToString())
            {
                json.Data = new { status = "success", msg = "" };
                //清空缓存
                //DataCache.SetCache(mobile + "_regcode", "");
            }
            else
            {
                json.Data = new { status = "-1", msg = "验证码错误！" };
            }

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}