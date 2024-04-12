using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class ApiController : CheckLoginController
    {
        public UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        public ActionResult Index()
        {
            return View();
        }

        #region 用户信息修改

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Info(int ID = 0)
        {
            if (LoginedUserModel != null)
            {
                json.Data = new { status = "0", msg = "", data = LoginedUserModel };
            }
            else
            {
                json.Data = new { status = "-1", msg = "请先登录" };
            }

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户信息修改-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Info(string nickname = "", string gender = "男", string birthday = "", string address = "")
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录" };
            }
            else
            {
                if (!string.IsNullOrEmpty(nickname))
                {
                    LoginedUserModel.U_NickName = nickname;
                }
                if (!string.IsNullOrEmpty(birthday))
                {
                    LoginedUserModel.U_Birthday = birthday.Replace("日", "-").Replace("月", "-").Replace("年", "-").Trim('-');
                }
                if (!string.IsNullOrEmpty(address))
                {
                    LoginedUserModel.U_Address = address;
                }
                if (gender == "男")
                {
                    LoginedUserModel.U_Gender = 1;
                }
                else
                {
                    LoginedUserModel.U_Gender = 2;
                }
                work.UserRepository.Update(LoginedUserModel);
                work.Save();
                json.Data = new { status = "0", msg = "", data = LoginedUserModel };
            }

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }


        ///// <summary>
        ///// 修改昵称
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult InfoNickName()
        //{
        //    return View(LoginedUserModel);
        //}
        ///// <summary>
        ///// 修改昵称
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult InfoNickName(string nickName)
        //{
        //    User user = work.UserRepository.GetByID(LoginedUserModel.ID);
        //    if (user != null)
        //    {
        //        if (nickName.Length < 2)
        //        {
        //            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("danger", "请输入不少于2个字符的昵称");
        //            return View(user);
        //        }
        //        user.U_NickName = UtilityClass.SQLFilter(nickName);
        //        work.UserRepository.Update(user);
        //        work.Save();

        //        UserService.SetCacheUser(LoginedUserModel.U_UserName, user);
        //        return RedirectToAction("Info");
        //    }

        //    return View(user);
        //}

        ///// <summary>
        ///// 修改性别
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult InfoGender(int gender = 0)
        //{
        //    User user = work.UserRepository.GetByID(LoginedUserModel.ID);
        //    if (user != null)
        //    {
        //        user.U_Gender = gender;
        //        work.UserRepository.Update(user);
        //        work.Save();

        //        UserService.SetCacheUser(LoginedUserModel.U_UserName, user);
        //        return RedirectToAction("Info");
        //    }

        //    return View(user);
        //}


        #endregion

        #region 我的资产（账单）

        /// <summary>
        /// 收支明细
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult AmountList(int page = 1, int pageSize = 10)
        {
            if (LoginedUserModel != null)
            {
                var rst = work.Context.UserAmountHistorys.Where(m => m.UserID == LoginedUserModel.ID && m.Is_Delete == 0);
                rst = rst.OrderByDescending(m => m.ID).Skip((page - 1) * pageSize).Take(pageSize);

                ViewBag.ID = LoginedUserModel.ID;
                User user = work.Context.Users.AsNoTracking().Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault<User>();
                ViewBag.User = user;
                //登录更新缓存
                UserService.SetCacheUser(user.U_UserName, user);
                //return View(rst.ToPagedList(pageNumber, pageSize));

                json.Data = new { status = "0", msg = "", data = rst.ToList() };
            }
            else
            {
                json.Data = new { status = "-1", msg = "请先登录" };
            }

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 文章详情，运势、签

        public ActionResult YunDetail(string shengxiao="")
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录" };
            }
            else
            {
                if (string.IsNullOrEmpty(shengxiao))
                {
                    shengxiao = "龙";
                }
                List<int> list = work.Context.Articles.Where(m => m.ArticleTypeID == 11 && m.Art_IsEnable == 1 && m.Art_Description.Contains(shengxiao)).Select(m => m.ID).ToList();//运势
                int idIndex = new Random().Next(1, list.Count) - 1;
                int aid = list[idIndex];

                var model = work.Context.Articles.Where(m => m.ID == aid && m.Art_IsEnable == 1).FirstOrDefault();
                if (model != null)
                {
                    model.Art_ShowTimes = model.Art_ShowTimes + 1;
                    work.ArticleRepository.Update(model);
                    work.Save();
                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.运势));

                    json.Data = new { status = "0", msg = "", data = model };
                }
                else
                {
                    json.Data = new { status = "-2", msg = "操作失败，请稍后重试", data = model };
                }
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult QianDetail()
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录" };
            }
            else
            {
                List<int> list = work.Context.Articles.Where(m => m.ArticleTypeID == 1 && m.Art_IsEnable == 1).Select(m => m.ID).ToList();//运势
                int idIndex = new Random().Next(1, list.Count) - 1;
                int aid = list[idIndex];

                var model = work.Context.Articles.Where(m => m.ID == aid && m.Art_IsEnable == 1).FirstOrDefault();
                if (model != null)
                {
                    model.Art_ShowTimes = model.Art_ShowTimes + 1;
                    work.ArticleRepository.Update(model);
                    work.Save();
                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.运势));

                    json.Data = new { status = "0", msg = "", data = model };
                }
                else
                {
                    json.Data = new { status = "-2", msg = "操作失败，请稍后重试", data = model };
                }
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NicknameDetail()
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录" };
            }
            else
            {
                List<int> list = work.Context.Articles.Where(m => m.ArticleTypeID == 17 && m.Art_IsEnable == 1).Select(m => m.ID).ToList();//昵称凶吉
                int idIndex = new Random().Next(1, list.Count) - 1;
                int aid = list[idIndex];

                var model = work.Context.Articles.Where(m => m.ID == aid && m.Art_IsEnable == 1).FirstOrDefault();
                if (model != null)
                {
                    model.Art_ShowTimes = model.Art_ShowTimes + 1;
                    work.ArticleRepository.Update(model);
                    work.Save();
                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.昵称));
                    json.Data = new { status = "0", msg = "", data = model };
                }
                else
                {
                    json.Data = new { status = "-2", msg = "操作失败，请稍后重试", data = model };
                }
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ArticleDetail(int ID = 0)
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录" };
            }
            else
            {
                var model = work.Context.Articles.Where(m => m.ID == ID && m.Art_IsEnable == 1).FirstOrDefault();
                if (model != null)
                {
                    model.Art_ShowTimes = model.Art_ShowTimes + 1;
                    work.ArticleRepository.Update(model);
                    work.Save();
                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.运势));

                }
                else
                {
                    model = new Article();
                }
                json.Data = new { status = "0", msg = "", data = model };

            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 我的运势、抽签
        /// </summary>
        /// <param name="type">9抽签，8运势</param>
        /// <returns></returns>
        public ActionResult MyArtDetail(int type = 8)
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录" };
            }
            else
            {
                var aid = work.Context.BrowseRecords.Where(m => m.BR_ItemType == type && m.UserID == LoginedUserModel.ID).OrderByDescending(m => m.ID).Select(m => m.BR_ItemID).FirstOrDefault();
                var model = work.Context.Articles.Where(m => m.ID == aid).FirstOrDefault();
                if (model == null)
                {
                    model = new Article();
                }
                json.Data = new { status = "0", msg = "", data = model };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 平台公告

        public ActionResult AritcleList(int type = 0, int page = 1)
        {
            if (type == 0)
            {
                type = Convert.ToInt16(DataConfig.ArticleTypeEnum.平台公告);
            }

            ViewBag.type = type;
            ViewBag.page = page;

            //ViewBag.TypeModel = work.ArticleTypeRepository.GetByID(type);
            int pageSize = 20;

            var rst = work.Context.Articles.Where(m => m.Art_IsEnable == 1 && m.ArticleTypeID == type);
            rst = rst.OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID).Skip((page - 1) * pageSize).Take(pageSize);
            //json.Data = new { status = "-1", msg = "请先登录" };
            json.Data = new { status = "0", msg = "", data = rst.ToList() };
            //return View(rst.ToPagedList(page, pageSize));
            //return View();
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 按事项支付
        /// <summary>
        /// 按事项支付
        /// </summary>
        /// <param name="thing">kg:开光，jx:敬香,gp:贡品,ys:运势,cq:抽签,nc:昵称</param>
        /// <returns></returns>
        public ActionResult PayThing(string thing = "kg")
        {
            decimal amount = 599;
            string category = "开光";
            if (thing == "kg")
            {
                amount = 599;
                category = "开光";
            }
            else if (thing == "jx")
            {
                amount = 1;
                category = "敬香";
            }
            else if (thing == "gp")
            {
                amount = 500;
                category = "贡品";
            }
            else if (thing == "ys")
            {
                amount = 1;
                category = "运势";
            }
            else if (thing == "cq")
            {
                amount = 1;
                category = "抽签";
            }
            else if (thing == "nc")
            {
                amount = 1;
                category = "昵称";
            }
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录", amount = amount };
            }
            else
            {
                if (LoginedUserModel.U_Amount < amount)
                {
                    json.Data = new { status = "-2", msg = "余额不足，请先充值！", amount = amount };
                }
                else
                {
                    decimal shenyu = LoginedUserModel.U_Amount;
                    decimal newshenyu = shenyu - amount;
                    decimal lockshenyu = LoginedUserModel.U_LockAmount;
                    decimal newlockshenyu = lockshenyu + amount * 10;
                    //bool do_success = OrderService.PaySuccessToDo(order_no, order_no, pay_amount, 3);
                    //UserService.UpdateAmount(LoginedUserModel.ID, -amount);

                    LoginedUserModel.U_Amount = shenyu - amount;
                    LoginedUserModel.U_LockAmount = newlockshenyu;
                    work.UserRepository.Update(LoginedUserModel);
                    work.Save();

                    //更新资金流动记录
                    UserAmountHistoryService.Insert(LoginedUserModel.ID, amount, newshenyu, 0, lockshenyu, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.减少), category, category + "支出", 0, LoginedUserModel.U_UserName, "");

                    //赠送元宝-至锁定余额
                    UserAmountHistoryService.Insert(LoginedUserModel.ID, 0, newshenyu, amount * 10, newlockshenyu, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.增加), category, category + "赠送", 0, LoginedUserModel.U_UserName, "");

                    //开光
                    if (thing == "kg")
                    {
                        //更新开光状态
                        LoginedUserModel.U_Is_Check = 1;
                        work.UserRepository.Update(LoginedUserModel);
                        work.Save();

                        //增加推荐人提现额度
                        User tjUser = work.Context.Users.Where(m => m.ID == LoginedUserModel.Referrer).FirstOrDefault();
                        if (tjUser != null)
                        {
                            tjUser.U_TiXianAmount = tjUser.U_TiXianAmount + 500;
                            work.UserRepository.Update(tjUser);
                            work.Save();

                            //更新资金流动记录
                            UserAmountHistoryService.Insert(tjUser.ID, 0, tjUser.U_Amount, 0, tjUser.U_LockAmount, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.增加), "增加额度", "推荐增加额度500", 0, tjUser.U_UserName, "");
                        }
                    }
                    if (amount > 0)
                    {
                        #region 直推70%
                        User tjUser = work.Context.Users.Where(m => m.ID == LoginedUserModel.Referrer).FirstOrDefault();
                        if (tjUser != null)
                        {
                            decimal amount1 = amount * 0.7M;
                            //赠送元宝-至锁定余额
                            tjUser.U_LockAmount = tjUser.U_LockAmount + amount1;
                            work.UserRepository.Update(tjUser);
                            work.Save();
                            //赠送元宝-记录
                            UserAmountHistoryService.Insert(tjUser.ID, 0, tjUser.U_Amount, amount1, tjUser.U_LockAmount, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.增加), "推荐" + category, "推荐" + category + "赠送1", 0, "系统", "");

                            #region 复推30%
                            User tjUser2 = work.Context.Users.Where(m => m.ID == tjUser.Referrer).FirstOrDefault();
                            if (tjUser2 != null)
                            {
                                decimal amount2 = amount * 0.3M;
                                //赠送元宝-至锁定余额
                                tjUser2.U_LockAmount = tjUser2.U_LockAmount + amount2;
                                work.UserRepository.Update(tjUser2);
                                work.Save();
                                //赠送元宝-记录
                                UserAmountHistoryService.Insert(tjUser2.ID, 0, tjUser2.U_Amount, amount2, tjUser2.U_LockAmount, Convert.ToInt16(DataConfig.AmountHistoryTypeEnum.增加), "推荐" + category, "推荐" + category + "赠送2", 0, "系统", "");
                            }

                            #endregion

                        }

                        #endregion



                    }

                    json.Data = new { status = "0", msg = "支付成功！", amount = amount };
                }
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 收货地址列表

        /// <summary>
        /// 收货地址
        /// </summary>
        /// <param name="at">at=add,at=edit</param>
        /// <param name="aid"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult Address()
        {
            if (LoginedUserModel == null)
            {
                json.Data = new { status = "-1", msg = "请先登录" };
            }
            else
            {
                //收货地址列表
                var list = work.Context.UserAddresses.Where(m => m.UserID == LoginedUserModel.ID).Select(m => new
                {
                    m.Is_Default,
                    m.ID,
                    m.Address,
                    m.Time,
                    m.UserName,
                    m.Mobile,
                    m.Post_Code,
                    m.Tel
                }).OrderByDescending(m => m.Is_Default).ThenByDescending(m => m.Time).ToList();
                //return View();
                json.Data = new { status = "0", msg = "支付成功！", data = list };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}