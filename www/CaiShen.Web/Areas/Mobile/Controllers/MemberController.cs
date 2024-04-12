using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using Pannet.Utility;
using PagedList;
using Pannet.DAL.Repository;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class MemberController : NeedLoginController
    {
        public UnitOfWork work = new UnitOfWork();
        private int tag_education = Convert.ToInt16(DataConfig.TagTypeEnum.学历);
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        #region 用户中心

        /// <summary>
        /// 用户中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Department departmentModel = work.DepartmentRepository.GetByID(LoginedUserModel.U_DepartmentID);
            if (departmentModel != null)
            {
                ViewBag.DepartmentName = departmentModel.Dep_Name;
            }
            else
            {
                ViewBag.DepartmentName = "未设置";
            }
            //重新获取，避免缓存
            User user = UserService.GetModel(LoginedUserModel.ID);
            if (user == null)
            {
                Response.Redirect("/Mobile/Login/logout");
                Response.End();
            }
            int goods_type_video = Convert.ToInt16(DataConfig.GoodsTypeEnum.商品类);
            int goods_type_ppt = Convert.ToInt16(DataConfig.GoodsTypeEnum.康复类);
            //int goods_type_jy = Convert.ToInt16(DataConfig.GoodsTypeEnum.讲议);

            //参与调查次数
            int questionnaireCount = work.QuestionnaireRecordRepository.Get(m => m.UserID == LoginedUserModel.ID).Select(m => m.Quest_ID).Distinct().Count();
            //培训活动报名次数
            int activityCount = work.AdvertisementRecordRepository.Get(m => m.UserID == LoginedUserModel.ID).Count();

            user.U_CommentCount = work.CommentRepository.Get(m => m.Comm_UserId == user.ID).Count();
            user.U_CourseVideoCount = work.Context.GoodsArticleRecords
                .Join(work.Context.Goods, gar => gar.GoodsID, g => g.ID, (gar, g) => new { gar, g })
                .Where(m => m.gar.UserID == user.ID && m.g.GoodsTypeID == goods_type_video && m.gar.GAR_State == 1).Select(m => m.gar.GoodsArticleID).Distinct().Count();

            user.U_CoursePPTCount = work.Context.GoodsArticleRecords
               .Join(work.Context.Goods, gar => gar.GoodsID, g => g.ID, (gar, g) => new { gar, g })
                .Where(m => m.gar.UserID == user.ID && m.g.GoodsTypeID == goods_type_ppt).Select(m => m.gar.GoodsArticleID).Distinct().Count();

            //user.U_CourseJiangYiCount = work.Context.GoodsArticleRecords
            //    .Join(work.Context.Goods, gar => gar.GoodsID, g => g.ID, (gar, g) => new { gar, g })
            //    .Where(m => m.gar.UserID == user.ID && m.g.GoodsTypeID == goods_type_jy).Select(m => m.gar.GoodsArticleID).Distinct().Count();

            user.U_DemandCount = work.Context.ConsultMessages.Where(m => m.UserID == LoginedUserModel.ID).Count();

            user.U_StudyCount = user.U_CourseVideoCount + user.U_CoursePPTCount + user.U_CourseJiangYiCount + user.U_CommentCount + user.U_DemandCount + questionnaireCount + activityCount;
            ViewBag.activityCount = activityCount;

            work.UserRepository.Update(user);
            work.Save();
            UserService.SetCacheUser(LoginedUserModel.U_UserName, user);

            //数量统计
            int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);
            TongJiCountVModel countModel = new TongJiCountVModel();
            //countModel.VideoCount = work.Context.GoodsArticles
            //    .Join(work.Context.Goods, ga => ga.GoodsID, g => g.ID, (ga, g) => new { ga, g })
            //    .Where(m => m.g.G_Status == goodsStatusOn && m.g.GoodsTypeID == goods_type_video).Select(m => m.ga.ID).Distinct().Count();
            //countModel.PPTCount = work.Context.Goods.Where(m => m.G_Status == goodsStatusOn && m.GoodsTypeID == goods_type_ppt).Select(m => m.ID).Count();
            ////countModel.JiangYiCount = work.Context.Goods.Where(m => m.G_Status == goodsStatusOn &&  m.GoodsTypeID == goods_type_jy).Select(m => m.ID).Count();

            //countModel.PeiXunCount = work.Context.Advertisements.Where(m => m.AD_IsEnable == 1).Count();
            //countModel.QuestionnaireCount = work.Context.Questionnaires.Where(m => m.Quest_Status == 1).Count();
            //countModel.DesignWorkCount = work.DesignWorkRepository.Get().Count();

            ViewBag.CountModel = countModel;

            ViewBag.TuijianCount = work.Context.Users.Where(m => m.Referrer == LoginedUserModel.ID).Count();

            //平台公告
            int art_type = Convert.ToInt16(DataConfig.ArticleTypeEnum.平台公告);
            List<Article> articleList = work.Context.Articles.Where(m => m.Art_IsEnable == 1 && m.ArticleTypeID == art_type).OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID).Take(2).ToList();
            ViewBag.NoticeList = articleList;
            ViewBag.NoticeCount = work.Context.Articles.Where(m => m.Art_IsEnable == 1 && m.ArticleTypeID == art_type).Count();
            return View(user);
        }
        #endregion

        #region 用户信息修改

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            if (LoginedUserModel != null)
            {
                ViewBag.ID = LoginedUserModel.ID;

                ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
                ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == LoginedUserModel.U_Province);
                ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == LoginedUserModel.U_City);
                ViewBag.EducationTags = work.TagRepository.Get(m => m.T_Type == tag_education);
                ViewBag.Departments = work.DepartmentRepository.Get();

                return View(LoginedUserModel);
            }
            return RedirectToAction("Index");
            ////return View(new EditUserVModel());
            //var user = work.Context.Users.AsNoTracking().Where(u => u.ID == LoginedUserModel.ID).FirstOrDefault<User>();
            //UserService.SetCacheUser(LoginedUserModel.U_UserName, user);
            //return View(LoginedUserModel);
        }

        /// <summary>
        /// 用户信息修改-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Info(User editUser)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.EducationTags = work.TagRepository.Get(m => m.T_Type == tag_education);
            ViewBag.Departments = work.DepartmentRepository.Get();

            //if (ModelState.IsValid)
            //{
            User user = work.UserRepository.Get(u => u.ID == editUser.ID).FirstOrDefault<User>();

            ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == editUser.U_Province);
            ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == editUser.U_City);

            if (user.U_Phone != editUser.U_Phone)//修改手机号码
            {
                var existUser = work.UserRepository.Get(u => u.U_Phone == editUser.U_Phone);
                if (existUser.Count() > 0)
                {
                    ModelState.AddModelError("U_Phone", string.Format("手机号码{0}已存在！", editUser.U_Phone));
                    return View(editUser);
                }
                else
                {
                    user.U_Phone = editUser.U_Phone;
                }
            }
            user.U_Education = editUser.U_Education;
            user.U_NickName = editUser.U_NickName;
            user.U_RealName = editUser.U_RealName;
            user.U_Province = editUser.U_Province;
            user.U_City = editUser.U_City;
            user.U_Region = editUser.U_Region;
            user.U_DepartmentID = editUser.U_DepartmentID;
            user.U_Gender = editUser.U_Gender;
            user.U_Email = editUser.U_Email;
            user.U_Company = editUser.U_Company;

            work.UserRepository.Update(user);
            work.Save();
            UserService.SetCacheUser(user.U_UserName, user);

            //
            //work.Context.Entry(LoginedUserModel).State = EntityState.Modified;
            //work.Save();
            ////work.Dispose();

            ////支付信息
            //UserPayInfo payinfo = new UserPayInfo();
            //payinfo.AlipayNO = editUser.AlipayNO;
            //payinfo.BankAddress = editUser.BankAddress;
            //payinfo.BankName = editUser.BankName;
            //payinfo.BankNO = editUser.BankNO;
            //payinfo.UserID = user.ID;
            //payinfo.Wechat = editUser.Wechat;

            //work.Context.UserPayInfos.AddOrUpdate(p => p.UserID, payinfo);
            //work.Dispose();

            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            //}
            //return JavaScript("alert('修改成功!');");

            return View(editUser);
        }


        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <returns></returns>
        public ActionResult InfoNickName()
        {
            return View(LoginedUserModel);
        }
        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InfoNickName(string nickName)
        {
            User user = work.UserRepository.GetByID(LoginedUserModel.ID);
            if (user != null)
            {
                if (nickName.Length < 2)
                {
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("danger", "请输入不少于2个字符的昵称");
                    return View(user);
                }
                user.U_NickName = UtilityClass.SQLFilter(nickName);
                work.UserRepository.Update(user);
                work.Save();

                UserService.SetCacheUser(LoginedUserModel.U_UserName, user);
                return RedirectToAction("Info");
            }

            return View(user);
        }
        /// <summary>
        /// 修改性别
        /// </summary>
        /// <returns></returns>
        public ActionResult InfoGender()
        {
            return View(LoginedUserModel);
        }
        /// <summary>
        /// 修改性别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InfoGender(int gender = 0)
        {
            User user = work.UserRepository.GetByID(LoginedUserModel.ID);
            if (user != null)
            {
                user.U_Gender = gender;
                work.UserRepository.Update(user);
                work.Save();

                UserService.SetCacheUser(LoginedUserModel.U_UserName, user);
                return RedirectToAction("Info");
            }

            return View(user);
        }
        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="url">头像URL</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InfoThumb(string url)
        {
            User user = LoginedUserModel;
            if (user != null)
            {
                user.U_Thumbnail = url;
                work.UserRepository.Update(user);
                work.Save();
            }
            json.Data = new { status = "1", msg = "修改成功！" };
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改基础设置
        /// </summary>
        /// <returns></returns>
        public ActionResult InfoSet()
        {
            return View(LoginedUserModel);
        }
        /// <summary>
        /// 修改基础设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InfoSet(User newUser)
        {
            User user = work.UserRepository.GetByID(LoginedUserModel.ID);
            if (user != null)
            {
                user.U_Height = newUser.U_Height;
                user.U_Birthday = newUser.U_Birthday;
                user.U_HopeWeight = newUser.U_HopeWeight;
                user.U_Weight = newUser.U_Weight;
                work.UserRepository.Update(user);
                work.Save();

                UserService.SetCacheUser(LoginedUserModel.U_UserName, user);
                return RedirectToAction("Index");
            }

            return View(user);
        }

        #endregion

        #region 用户支付信息

        /// <summary>
        /// 用户支付信息修改
        /// </summary>
        /// <returns></returns>
        public ActionResult InfoPay()
        {
            if (LoginedUserModel != null)
            {
                ViewBag.ID = LoginedUserModel.ID;

                UserPayInfo payinfo = work.PayInfoRepository.Get(p => p.UserID == LoginedUserModel.ID).FirstOrDefault<UserPayInfo>();
                if (payinfo == null)
                {
                    payinfo = new UserPayInfo();
                }

                return View(payinfo);
            }
            return View(new UserPayInfo());
        }

        /// <summary>
        /// 用户支付信息修改-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InfoPay(UserPayInfo newModel)
        {

            if (ModelState.IsValid)
            {
                newModel.UserID = LoginedUserModel.ID;

                work.Context.UserPayInfos.AddOrUpdate(p => p.UserID, newModel);
                work.Save();
                //work.Dispose();

                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            //return JavaScript("alert('修改成功!');");

            return View(newModel);
        }

        #endregion

        #region 用户修改密码

        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult InfoPassword()
        {
            if (LoginedUserModel != null)
            {
                ViewBag.ID = LoginedUserModel.ID;

                ChangePwdVModel pwdModel = new ChangePwdVModel();

                return View(pwdModel);
            }
            return View(new ChangePwdVModel());
        }

        /// <summary>
        /// 用户修改密码-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InfoPassword(ChangePwdVModel newModel)
        {

            if (ModelState.IsValid)
            {
                User user = work.UserRepository.Get(u => u.ID == LoginedUserModel.ID).FirstOrDefault<User>();

                if (UtilityClass.GetMD5(newModel.Password) != user.U_Pwd)
                {
                    ModelState.AddModelError("Password", "原密码输入错误");
                    return View(newModel);
                }
                if (newModel.NewPassword != newModel.NewPassword2)
                {
                    ModelState.AddModelError("NewPassword", "输入新密码不一致");
                    return View(newModel);
                }
                if (user != null)
                {
                    user.U_Pwd = UtilityClass.GetMD5(newModel.NewPassword);
                }
                work.UserRepository.Update(user);
                work.Save();
                //work.Dispose();

                //ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "修改成功");
                return RedirectToAction("Index", "Member");

            }
            //return JavaScript("alert('修改成功!');");

            return View(newModel);
        }

        #endregion

        #region 充值+提现

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge()
        {
            return View();
        }

        #region 提现

        /// <summary>
        /// 提现记录
        /// </summary>
        /// <returns></returns>
        public ActionResult TiXian(int page = 1)
        {
            var rst = work.Context.TiXians.Where(m => m.UserID == LoginedUserModel.ID);
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 申请提现
        /// </summary>
        /// <returns></returns>
        public ActionResult TiXianApply(int ID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.ShowApplyBtn = 1;
            ViewBag.User = work.Context.Users.Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault<User>();
            ViewBag.TiXianUsers = work.Context.TiXians.Where(m => m.UserID == LoginedUserModel.ID).Select(m => new TiXianUserVModel
            {
                BankName = m.TX_BankName,
                BankNumber = m.TX_BankNumber,
                UserName = m.TX_UserName
            }).Distinct().ToList();

            var existListCount = work.Context.TiXians.Where(m => m.UserID == LoginedUserModel.ID).Where(m => m.TX_Status == 0 || m.TX_Status == 1).Count();//存在待处理或已审核记录，暂不允许提交申请
            if (existListCount > 0)
            {
                ViewBag.ShowApplyBtn = 0;
            }

            if (ID != 0)
            {
                TiXian model = work.Context.TiXians.Where(m => m.ID == ID).FirstOrDefault();
                return View(model);
            }
            return View(new TiXian());
        }
        /// <summary>
        /// 申请提现
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TiXianApply(TiXian newModel, int ShowApplyBtn = 1)
        {
            ViewBag.ID = newModel.ID;
            ViewBag.ShowApplyBtn = ShowApplyBtn;
            User user = work.Context.Users.AsNoTracking().Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault();
            ViewBag.User = user;

            if (ModelState.IsValid)
            {
                #region 验证
                if (string.IsNullOrEmpty(newModel.TX_UserName))
                {
                    ModelState.AddModelError("TX_UserName", "请输入收款人姓名");
                    return View(newModel);
                }

                if (string.IsNullOrEmpty(newModel.TX_BankName))
                {
                    ModelState.AddModelError("TX_BankName", "请输入收款银行名称");
                    return View(newModel);
                }
                if (string.IsNullOrEmpty(newModel.TX_BankNumber))
                {
                    ModelState.AddModelError("TX_BankNumber", "请输入收款银行账号");
                    return View(newModel);
                }
                if (newModel.TX_Amount <1)
                {
                    ModelState.AddModelError("TX_Amount", "请输入提现金额，且不能小于1元");
                    return View(newModel);
                }
                if (newModel.TX_Amount > LoginedUserModel.U_Amount)
                {
                    ModelState.AddModelError("TX_Amount", "提现金额错误！");
                    return View(newModel);
                }
                #endregion


                if (user != null)
                {

                    newModel.UserID = LoginedUserModel.ID;
                    newModel.TX_Number = TiXianService.GetTiXianNumber(LoginedUserModel.ID);
                    user.U_Amount = user.U_Amount - newModel.TX_Amount;
                    //添加金额调整记录
                    UserAmountHistoryService.Insert(user.ID, newModel.TX_Amount, user.U_Amount, 0, user.U_LockAmount, 0, "提现", "提现锁定金额", user.ID, user.U_UserName);
                    //user.U_LockAmount = user.U_LockAmount + newModel.TX_Amount;
                    //UserAmountHistoryService.Insert(user.ID, 0, user.U_Amount, newModel.TX_Amount, user.U_LockAmount, 1, "提现", "申请提现锁定金额", user.ID, user.U_UserName);
                    work.UserRepository.Update(user);
                    work.TiXianRepository.Insert(newModel);
                    work.Save();

                    UserService.SetCacheUser(user.U_UserName, user);
                }

                //返回列表
                return RedirectToAction("TiXian");
            }
            return View(newModel);
        }
        /// <summary>
        /// 申请提现 - 取消
        /// </summary>
        /// <returns></returns>
        public ActionResult TiXianApplyCancel(int ID = 0)
        {
            ViewBag.ID = ID;
            if (ID != 0)
            {
                TiXian existModel = work.Context.TiXians.Where(m => m.UserID == LoginedUserModel.ID && m.ID == ID).FirstOrDefault();//存在
                User user = work.Context.Users.AsNoTracking().Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault();
                if (existModel != null && user != null && existModel.TX_Status == Convert.ToInt16(DataConfig.TiXianStatusEnum.待处理))
                {
                    user.U_Amount = user.U_Amount + existModel.TX_Amount;
                    UserAmountHistoryService.Insert(user.ID, existModel.TX_Amount, user.U_Amount, 0, user.U_LockAmount, 1, "取消提现", "返还提现金额", ID, user.U_UserName);
                    //user.U_LockAmount = user.U_LockAmount - existModel.TX_Amount;
                    //UserAmountHistoryService.Insert(user.ID, 0, user.U_Amount, existModel.TX_Amount, user.U_LockAmount, 0, "取消提现", "返还提现金额", ID, user.U_UserName);
                    work.UserRepository.Update(user);
                    work.Save();

                    existModel.TX_Status = Convert.ToInt16(DataConfig.TiXianStatusEnum.已取消);

                    work.TiXianRepository.Update(existModel);
                    work.Save();
                }

                //TiXianService.UpdateStatus(ID, Convert.ToInt16(DataConfig.TiXianStatusEnum.取消));
            }
            return RedirectToAction("TiXian");
        }

        #endregion

        #endregion

        #region 我的资产（账单）

        public ActionResult AmountList(int page = 1)
        {
            //收支明细
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserAmountHistorys.Where(m => m.UserID == LoginedUserModel.ID && m.Is_Delete == 0);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.ID = LoginedUserModel.ID;
            User user = work.Context.Users.AsNoTracking().Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault<User>();
            ViewBag.User = user;
            //登录更新缓存
            UserService.SetCacheUser(user.U_UserName, user);
            return View(rst.ToPagedList(pageNumber, pageSize));
        }
        //ActionResult
        public ActionResult UserAmountHistoryDelete(int ID = 0)
        {
            if (ID != 0)
            {
                var m = work.UserAmountHistoryRepository.Get(g => g.ID == ID && g.UserID == LoginedUserModel.ID).FirstOrDefault<UserAmountHistory>();
                m.Is_Delete = 1;

                work.UserAmountHistoryRepository.Update(m);
                work.Save();
                //work.Dispose();
            }
            return RedirectToAction("AmountList");
        }

        #endregion

        #region 我的积分兑换礼品

        public ActionResult ScoreGift(int page = 1)
        {
            var rst = work.Context.UserScoreProducts
                .Join(work.Context.ScoreProducts, usp => usp.ScoreProductID, sp => sp.ID, (usp, sp) => new { usp, sp })
                .Join(work.Context.Users, usp => usp.usp.UserID, u => u.ID, (usp, u) => new { usp.usp, usp.sp, u })
                .Select(m => new UserScoreProductVmodel
                {
                    UserScoreProduct = m.usp,
                    ScoreProduct = m.sp,
                    U_UserName = m.u.U_UserName
                }).Where(m => m.UserScoreProduct.USP_IsDelete == 0);
            rst = rst.OrderByDescending(m => m.UserScoreProduct.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }
        //删除记录
        public ActionResult ScoreGiftDelete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.UserScoreProductRepository.Get(m => m.ID == ID && LoginedUserModel.ID == m.UserID).FirstOrDefault<UserScoreProduct>();
                model.USP_IsDelete = 1;
                work.UserScoreProductRepository.Update(model);
                //work.UserScoreProductRepository.Delete(model);
                work.Save();
                //work.Dispose();
            }

            return RedirectToAction("ScoreGift");
        }


        #region 选择收货地址

        /// <summary>
        /// 选择收货地址
        /// </summary>
        /// <param name="usp_id"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult ScoreGiftChooseAddress(int usp_id)
        {
            //收货地址列表
            UserScoreProduct model = work.UserScoreProductRepository.GetByID(usp_id);
            ViewBag.Address = work.UserAddressRepository.Get(m => m.UserID == LoginedUserModel.ID).OrderByDescending(m => m.Is_Default).ThenByDescending(m => m.Time).ToList();
            if (model != null)
            {
                ViewBag.addressid = model.UserAddressID;
            }
            else
            {
                ViewBag.addressid = 0;
            }
            ViewBag.usp_id = usp_id;

            return View();
        }

        /// <summary>
        /// 设置收货地址
        /// </summary>
        /// <param name="usp_id"></param>
        /// <param name="addressid"></param>
        /// <returns></returns>
        public ActionResult ScoreGiftChooseAddressSave(int usp_id, int addressid)
        {
            var user = LoginedUserModel;
            if (user == null)
            {
                json.Data = new { status = "0", msg = "请先登录！" };
            }
            else
            {
                //兑换记录
                UserScoreProduct newModel = work.UserScoreProductRepository.GetByID(usp_id);
                UserAddress address = work.UserAddressRepository.GetByID(addressid);
                if (newModel != null && address != null && address.ID != 0)
                {
                    newModel.UserAddressID = address.ID;
                    newModel.USP_Address = string.Format("{0}, {1} {2}", address.Address, address.UserName, address.Mobile + " " + address.Tel);

                    work.UserScoreProductRepository.Update(newModel);
                    work.Save();
                    //work.Dispose();

                    json.Data = new { status = "1", msg = "设置成功" };
                }
                else
                {
                    json.Data = new { status = "0", msg = "参数错误！" };
                }
            }

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 我的订单（个人）

        #region 个人-订单列表

        public ActionResult Order(string time_from = "", string time_end = "", string keyword = "", int status = 0, int page = 1)
        {
            ViewBag.page = page;
            ViewBag.time_from = time_from;
            ViewBag.time_end = time_end;
            ViewBag.keyword = keyword;
            ViewBag.status = status;

            var rst = work.Context.Orders
                 .Join(work.Context.OrderDetails, o => o.ID, od => od.OrderID, (o, od) => new { o, od })
                .Where(m => m.o.UserID == LoginedUserModel.ID && m.o.O_IsDelete == 0);

            string sql = rst.ToString();

            //开始日期
            if (!string.IsNullOrEmpty(time_from))
            {
                DateTime _time_from = Convert.ToDateTime(time_from);
                rst = rst.Where(m => m.o.O_CreateTime > _time_from);
            }
            //结束日期
            if (!string.IsNullOrEmpty(time_end))
            {
                DateTime _time_end = Convert.ToDateTime(time_end).AddDays(1);
                rst = rst.Where(m => m.o.O_CreateTime < _time_end);
            }
            //订单号
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.o.O_OrderNo.Contains(keyword) || m.od.OD_GoodsName.Contains(keyword));
            }

            //待付款
            if (status == 1)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.待付款);
                rst = rst.Where(m => m.o.O_Status == paystatus);
            }
            //已付款，待发货
            if (status == 2)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.已付款);
                int shippingstatus = Convert.ToInt16(DataConfig.OrderShippingStatusEnum.待发货);
                rst = rst.Where(m => m.o.O_Status == paystatus && m.o.O_ShippingStatus == shippingstatus);
            }
            //已付款，待收货
            if (status == 3)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.已付款);
                int shippingstatus = Convert.ToInt16(DataConfig.OrderShippingStatusEnum.已发货);
                rst = rst.Where(m => m.o.O_Status == paystatus && m.o.O_ShippingStatus == shippingstatus);
            }
            //交易成功，待评价
            if (status == 4)
            {
                int paystatus = Convert.ToInt16(DataConfig.OrderStatusEnum.交易成功);
                rst = rst.Where(m => m.o.O_Status == paystatus && m.o.O_IsComment == 0);
            }

            List<UserOrderVModel> listOrder = new List<UserOrderVModel>();
            listOrder = rst.Select(m => new { m.o }).Distinct().ToList().Select(m => new UserOrderVModel
            {
                Order = m.o,
                UserShop = null,
                OrderDetailVList = null
            }).OrderByDescending(m => m.Order.ID).ToList();

            //所有订单详细
            List<Int32> listOrderIds = listOrder.Select(m => m.Order.ID).ToList();
            List<OrderDetail> orderDetailList = work.Context.OrderDetails.Where(m => listOrderIds.Contains(m.OrderID) && m.OD_IsDelete == 0).ToList();
            //所有产品照片
            List<Int32> orderGoodsIds = orderDetailList.Select(m => m.GoodsID).ToList();
            List<GoodsPhoto> goodsPhotoList = work.Context.GoodsPhotos.Where(m => orderGoodsIds.Contains(m.GoodsID)).ToList();

            foreach (var item in listOrder)
            {
                item.OrderDetailVList = work.Context.OrderDetails
                                        .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                                        .Select(m => new OrderDetailVModel
                                        {
                                            OrderDetail = m.od,
                                            PhotoUrl = m.g.G_Image
                                        }).Where(m => m.OrderDetail.OrderID == item.Order.ID).Distinct().ToList();
            }

            int pageSize = 12;
            int pageNumber = page;
            return View(listOrder.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 个人-订单详细

        /// <summary>
        /// 订单详细
        /// </summary>
        /// <param name="ID">订单ID</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(int ID, int page = 1)
        {
            ViewBag.page = page;
            ViewBag.ID = ID;
            if (ID != 0)
            {
                //订单详情 + 店铺
                Order order = work.OrderRepository.GetByID(ID);
                ViewBag.Order = order;
                if (order != null)
                {
                    ViewBag.UserShop = work.UserShopRepository.GetByID(order.UserShopID);
                }

                //配送记录
                var rst = work.Context.OrderShippings
                    .Join(work.Context.Orders, os => os.OrderID, o => o.ID, (os, o) => new { os, o })
                    .Where(m => m.o.ID == ID && m.o.UserID == LoginedUserModel.ID && m.o.O_IsDelete == 0)
                    .Select(m => m.os).OrderByDescending(m => m.OS_CreateTime);

                List<UserOrderShippingVModel> listOrderShipping = new List<UserOrderShippingVModel>();
                listOrderShipping = rst.ToList().Select(m => new UserOrderShippingVModel
                {
                    OrderShipping = m,
                    //UserShop = m.o,
                    OrderDetailVList = null
                }).OrderByDescending(m => m.OrderShipping.OS_CreateTime).Distinct().ToList();

                foreach (var item in listOrderShipping)
                {
                    //配送物品，订单详细id
                    string orderDetailIds = item.OrderShipping.OrderDetailIds;
                    string[] od_ids_arr = orderDetailIds.Split(',');

                    var rst2 = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.UserID == LoginedUserModel.ID && m.od.OrderID == ID && od_ids_arr.Contains(m.od.ID.ToString()));
                    List<OrderDetailVModel> listOrderDetailV = null;
                    listOrderDetailV = rst2.Select(m => new OrderDetailVModel
                    {
                        OrderDetail = m.od,
                        PhotoUrl = m.g.G_Image
                    }).Distinct().ToList();
                    item.OrderDetailVList = listOrderDetailV;
                }

                int pageSize = 12;
                int pageNumber = page;

                //未配送
                List<OrderDetailVModel> listOrderDetailV2 = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.UserID == LoginedUserModel.ID && m.od.OrderID == ID).Where(m => m.od.OD_ShippingStatus == 0 || m.od.OD_ShippingStatus == 4).Select(m => new OrderDetailVModel
                   {
                       OrderDetail = m.od,
                       PhotoUrl = m.g.G_Image
                   }).Distinct().ToList();
                ViewBag.OrderDetailNoShipping = listOrderDetailV2;

                return View(listOrderShipping.ToPagedList(pageNumber, pageSize));

            }
            return RedirectToAction("Order");
        }

        #endregion

        #region 个人-删除订单记录

        //删除记录
        public ActionResult OrderDelete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.OrderRepository.Get(m => m.ID == ID && m.UserID == LoginedUserModel.ID).FirstOrDefault<Order>();
                model.O_IsDelete = 1;
                work.OrderRepository.Update(model);
                work.Save();
                //work.Dispose();
            }
            return RedirectToAction("Order");
        }

        #endregion

        #region 个人-确认收货

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderShippingId">订单运送ID</param>
        /// <param name="ID">订单ID</param>
        /// <returns></returns>
        public ActionResult OrderDetailReceipt(int orderShippingId = 0, int orderID = 0)
        {
            if (orderShippingId != 0)
            {
                //OrderShipping model = work.OrderShippingRepository.Get(m => m.OrderID == orderID && m.ID == orderShippingId).FirstOrDefault();
                OrderShipping model = work.Context.OrderShippings
                    .Join(work.Context.Orders, os => os.OrderID, o => o.ID, (os, o) => new { os, o })
                    .Where(m => m.os.ID == orderShippingId && m.o.ID == orderID && m.o.UserID == LoginedUserModel.ID)
                    .Select(m => m.os).FirstOrDefault();


                if (model != null)
                {
                    //修改 订单包裹运送状态及时间
                    model.OS_DeliveryTime = DateTime.Now;
                    model.OS_Status = Convert.ToInt32(DataConfig.OrderShippingDeliveryStatusEnum.已签收);
                    work.OrderShippingRepository.Update(model);
                    work.Save();

                    //修改物品配送状态，订单详细id
                    OrderDetailService.UpdateShippingStatus(UtilityClass.ConvertIntArr(model.OrderDetailIds), Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已收货));
                    if (OrderDetailService.ExistShippingStatus(orderID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.待发货)) || OrderDetailService.ExistShippingStatus(orderID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已发货)))
                    {
                        //OrderService.UpdateShippingStatus(ID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.部分发货));
                    }
                    else
                    {
                        OrderService.UpdateShippingStatus(orderID, Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已收货));
                    }
                    //work.Dispose();
                }
            }
            return RedirectToAction("OrderDetail", new { ID = orderID });
        }

        #endregion

        #region 个人-申请退货

        /// <summary>
        /// 申请退货
        /// </summary>
        /// <param name="orderDetailID">订单详细ID</param>
        /// <returns></returns>
        public ActionResult ReturnOrderApply(int orderDetailID = 0, int type = 0)
        {
            ViewBag.orderDetailID = orderDetailID;
            ViewBag.type = type;

            if (orderDetailID != 0)
            {
                //是否已申请退货，且状态是待处理中
                int ro_status_0 = Convert.ToInt16(DataConfig.ReturnOrderStatusEnum.已申请待商家确认);
                var returnOrderModel = work.ReturnOrderRepository.Get(m => m.OrderDetailID == orderDetailID && m.UserID == LoginedUserModel.ID && m.RO_Status == ro_status_0).FirstOrDefault<ReturnOrder>();
                if (returnOrderModel != null)
                {
                    return RedirectToAction("ReturnOrderDetail", new { ID = returnOrderModel.ID });
                }

                int shipping_status = Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已收货);
                int shipping_status_wait = Convert.ToInt32(DataConfig.OrderShippingStatusEnum.待发货);
                List<OrderDetailVModel> listOrderDetailV = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.UserID == LoginedUserModel.ID && m.od.ID == orderDetailID).Where(m => m.od.OD_ShippingStatus == shipping_status || m.od.OD_ShippingStatus == shipping_status_wait).Select(m => new OrderDetailVModel
                   {
                       OrderDetail = m.od,
                       PhotoUrl = m.g.G_Image
                   }).Distinct().ToList();
                List<int> orderDetailIds_notshow = new List<int>();//不展示的产品（不允许退货）
                foreach (var item in listOrderDetailV)
                {
                    ProfitPercentConfig sysConfig = SystemInfoService.GetPercentConfig();
                    DateTime returnTimeMin = DateTime.Now.AddDays(-sysConfig.Order_TuiHuo_Days);
                    //如果发货时间超过15天，不允许退货
                    var rstShipping = work.Context.OrderShippings.Where(m => ("," + m.OrderDetailIds + ",").Contains("," + orderDetailID + ",") && m.OS_CreateTime < returnTimeMin).FirstOrDefault();
                    if (rstShipping != null && rstShipping.ID != 0)
                    {
                        //已发货且过了退货期，不允许退货
                        orderDetailIds_notshow.Add(item.OrderDetail.ID);
                    }
                }
                listOrderDetailV = listOrderDetailV.Where(m => !orderDetailIds_notshow.Contains(m.OrderDetail.ID)).ToList();

                ViewBag.OrderDetails = listOrderDetailV;
            }
            return View(new ReturnOrder());
        }

        /// <summary>
        /// 申请退货-post
        /// </summary>
        /// <param name="orderDetailID">订单详细ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReturnOrderApply(ReturnOrder newModel, int orderDetailID = 0)
        {
            ViewBag.orderDetailID = newModel.OrderDetailID;
            ViewBag.type = newModel.RO_Type;

            if (ModelState.IsValid)
            {
                Models.OrderDetail orderDetailModel = work.OrderDetailRepository.GetByID(newModel.OrderDetailID);
                if (orderDetailModel == null)
                {
                    return RedirectToAction("ReturnOrder");
                }
                int shipping_status = Convert.ToInt32(DataConfig.OrderShippingStatusEnum.已收货);
                int shipping_status_wait = Convert.ToInt32(DataConfig.OrderShippingStatusEnum.待发货);
                List<OrderDetailVModel> listOrderDetailV = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.UserID == LoginedUserModel.ID && m.od.ID == newModel.OrderDetailID).Where(m => m.od.OD_ShippingStatus == shipping_status || m.od.OD_ShippingStatus == shipping_status_wait).Select(m => new OrderDetailVModel
                   {
                       OrderDetail = m.od,
                       PhotoUrl = m.g.G_Image
                   }).Distinct().ToList();
                List<int> orderDetailIds_notshow = new List<int>();//不展示的产品（不允许退货）
                foreach (var item in listOrderDetailV)
                {
                    ProfitPercentConfig sysConfig = SystemInfoService.GetPercentConfig();
                    DateTime returnTimeMin = DateTime.Now.AddDays(-sysConfig.Order_TuiHuo_Days);
                    //如果发货时间超过15天，不允许退货
                    var rstShipping = work.Context.OrderShippings.Where(m => ("," + m.OrderDetailIds + ",").Contains("," + orderDetailID + ",") && m.OS_CreateTime < returnTimeMin).FirstOrDefault();
                    if (rstShipping != null && rstShipping.ID != 0)
                    {
                        //已发货且过了退货期，不允许退货
                        orderDetailIds_notshow.Add(item.OrderDetail.ID);
                    }
                }
                listOrderDetailV = listOrderDetailV.Where(m => !orderDetailIds_notshow.Contains(m.OrderDetail.ID)).ToList();
                ViewBag.OrderDetails = listOrderDetailV;
                if (listOrderDetailV == null || listOrderDetailV.Count() < 1)
                {
                    ModelState.AddModelError("RO_Description", "请选择退货商品");
                    return View(newModel);
                }
                if (newModel.RO_Reason == "" || newModel.RO_Reason == "0")
                {
                    ModelState.AddModelError("RO_Reason", "请选择退货理由");
                    return View(newModel);
                }
                if (newModel.RO_Amount > orderDetailModel.OD_PayAmount)
                {
                    ModelState.AddModelError("RO_Amount", "最多退款金额" + orderDetailModel.OD_PayAmount + "元");
                    return View(newModel);
                }
                if (string.IsNullOrEmpty(newModel.RO_Description))
                {
                    ModelState.AddModelError("RO_Description", "详细说明不能为空");
                    return View(newModel);
                }

                newModel.UserID = LoginedUserModel.ID;
                newModel.UserShopID = listOrderDetailV[0].OrderDetail.UserShopID;
                newModel.RO_ReturnOrderNo = ReturnOrderService.GetReturnOrderNo(orderDetailModel.ID);

                work.ReturnOrderRepository.Insert(newModel);
                work.Save();
                //work.Dispose();

                return RedirectToAction("ReturnOrder");
            }
            return View(newModel);
        }

        #endregion

        #region 个人-取消交易

        /// <summary>
        /// 个人-取消交易
        /// </summary>
        /// <param name="ID">订单ID</param>
        /// <returns></returns>
        public ActionResult OrderCancel(int ID = 0)
        {
            if (ID != 0)
            {
                //只有待付款交易才能取消
                int order_status = Convert.ToInt16(DataConfig.OrderStatusEnum.待付款);
                Order model = work.OrderRepository.Get(m => m.ID == ID && m.UserID == LoginedUserModel.ID && m.O_Status == order_status).FirstOrDefault();

                if (model != null)
                {
                    model.O_Status = Convert.ToInt16(DataConfig.OrderStatusEnum.交易关闭); ;
                    work.OrderRepository.Update(model);
                    work.Save();

                    UserLogService.Add(LoginedUserModel.U_UserName, LoginedUserModel.ID, "取消交易：" + model.O_OrderNo, model.ID.ToString());

                    //work.Dispose();
                }
            }
            return RedirectToAction("Order");
        }

        #endregion

        #region 个人-退货记录

        /// <summary>
        /// 个人-退货记录
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="paystatus"></param>
        /// <param name="shippingstatus"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ReturnOrder(string keyword = "", string status = "", string paystatus = "", string shippingstatus = "", int page = 1)
        {
            ViewBag.User = work.Context.Users.Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault<User>();

            ViewBag.keyword = keyword;
            ViewBag.status = status;
            ViewBag.paystatus = paystatus;
            ViewBag.shippingstatus = shippingstatus;

            var rst = work.Context.ReturnOrders
                .Join(work.Context.Users, ro => ro.UserID, u => u.ID, (ro, u) => new { ro, u })
                .Join(work.Context.UserShops, rou => rou.ro.UserShopID, us => us.ID, (rou, us) => new
                {
                    ro = rou.ro,
                    u = rou.u,
                    us
                })
                .Join(work.Context.OrderDetails, m => m.ro.OrderDetailID, od => od.ID, (m, od) => new
                {
                    ro = m.ro,
                    u = m.u,
                    us = m.us,
                    od
                }).Where(m => m.u.ID == LoginedUserModel.ID);

            if (status != "")
            {
                rst = rst.Where(m => m.ro.RO_Status.ToString() == status);
            }
            if (paystatus != "")
            {
                rst = rst.Where(m => m.ro.RO_PayStatus.ToString() == paystatus);
            }
            if (shippingstatus != "")
            {
                rst = rst.Where(m => m.ro.RO_ShippingStatus.ToString() == shippingstatus);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.ro.RO_ReturnOrderNo.Contains(keyword));
            }
            List<ReturnOrderVModel> list = rst.ToList().Select(m => new ReturnOrderVModel
            {
                ReturnOrder = m.ro,
                User = m.u,
                UserShop = m.us,
                OrderDetail = m.od
            }).OrderByDescending(m => m.ReturnOrder.ID).ToList();

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 详细页
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ReturnOrderDetail(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.ReturnOrderRepository.Get(m => m.ID == ID && m.UserID == LoginedUserModel.ID).FirstOrDefault<ReturnOrder>();

                #region 退货物品信息

                List<OrderDetailVModel> listOrderDetailV = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.ID == model.OrderDetailID).Select(m => new OrderDetailVModel
                   {
                       OrderDetail = m.od,
                       PhotoUrl = m.g.G_Image
                   }).Distinct().ToList();

                ViewBag.OrderDetails = listOrderDetailV;

                #endregion

                #region 退货物流信息

                var rst_returnOrderShipping = work.ReturnOrderShippingRepository.Get(m => m.ReturnOrderID == ID);

                ViewBag.OrderDetailShippings = rst_returnOrderShipping;

                #endregion

                return View(model);
            }
            return View(new ReturnOrder());
        }

        /// <summary>
        /// 取消退货
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ReturnOrderCancel(int ID = 0)
        {
            if (ID != 0)
            {
                var m = work.ReturnOrderRepository.Get(g => g.ID == ID && g.UserID == LoginedUserModel.ID).FirstOrDefault<ReturnOrder>();
                m.RO_Status = Convert.ToInt16(DataConfig.ReturnOrderStatusEnum.已取消);

                work.ReturnOrderRepository.Update(m);
                work.Save();
                //work.Dispose();
            }
            return RedirectToAction("ReturnOrder");
        }

        /// <summary>
        /// 填写退货单
        /// </summary>
        /// <param name="ID">退货记录ID</param>
        /// <returns></returns>
        public ActionResult ReturnOrderShipping(int returnOrderID = 0)
        {
            ViewBag.returnOrderID = returnOrderID;
            if (returnOrderID != 0)
            {

                ReturnOrder returnOrderModel = work.ReturnOrderRepository.GetByID(returnOrderID);

                #region 物品信息

                List<OrderDetailVModel> listOrderDetailV = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.ID == returnOrderModel.OrderDetailID).Select(m => new OrderDetailVModel
                   {
                       OrderDetail = m.od,
                       PhotoUrl = m.g.G_Image
                   }).Distinct().ToList();

                ViewBag.OrderDetails = listOrderDetailV;

                #endregion

                return View(new ReturnOrderShipping());
            }
            return RedirectToAction("ReturnOrder");
        }
        /// <summary>
        /// 填写退货单
        /// </summary>
        /// <param name="ID">退货记录ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReturnOrderShipping(ReturnOrderShipping newModel, int returnOrderID = 0)
        {
            ViewBag.returnOrderID = returnOrderID;
            if (returnOrderID != 0)
            {
                ReturnOrder returnOrderModel = work.ReturnOrderRepository.GetByID(returnOrderID);

                #region 物品信息

                List<OrderDetailVModel> listOrderDetailV = work.Context.OrderDetails
                   .Join(work.Context.Goods, od => od.GoodsID, g => g.ID, (od, g) => new { od, g })
                   .Where(m => m.od.ID == returnOrderModel.OrderDetailID).Select(m => new OrderDetailVModel
                   {
                       OrderDetail = m.od,
                       PhotoUrl = m.g.G_Image
                   }).Distinct().ToList();

                ViewBag.OrderDetails = listOrderDetailV;

                #endregion

                #region 填写退货物流记录

                if (string.IsNullOrEmpty(newModel.ShippingCompany))
                {
                    ModelState.AddModelError("ShippingCompany", "运送公司不能为空");
                    return View(newModel);
                }

                if (string.IsNullOrEmpty(newModel.ShippingNo))
                {
                    ModelState.AddModelError("ShippingNo", "运送单号不能为空");
                    return View(newModel);
                }

                work.ReturnOrderShippingRepository.Insert(newModel);
                work.Save();

                #endregion

                //修改退货记录状态
                returnOrderModel.RO_ShippingStatus = Convert.ToInt16(DataConfig.ReturnOrderShippingStatusEnum.已退货);

                work.ReturnOrderRepository.Update(returnOrderModel);
                work.Save();

                //work.Dispose();
            }
            return RedirectToAction("ReturnOrder");
        }

        //删除记录
        public ActionResult ReturnOrderDelete(int ID = 0)
        {
            //if (ID != 0)
            //{
            //    var m = work.ReturnOrderRepository.Get(g => g.ID == ID && g.UserID == LoginedUserModel.ID).FirstOrDefault<ReturnOrder>();
            //    m.RO_IsDelete = 1;

            //    work.ReturnOrderRepository.Update(m);
            //    work.Save();
            //    //work.Dispose();
            //}
            return RedirectToAction("ReturnOrder");
        }

        #endregion

        #endregion

        #region 收货地址

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
            //收货地址列表
            ViewBag.Address = work.UserAddressRepository.Get(m => m.UserID == LoginedUserModel.ID).OrderByDescending(m => m.Is_Default).ThenByDescending(m => m.Time).ToList();
            return View();
        }

        #endregion

        #region 新增/编辑收货地址

        /// <summary>
        /// 新增/编辑收货地址
        /// </summary>
        /// <param name="at">at=add,at=edit</param>
        /// <param name="aid"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult AddressAdd(int aid = 0, string info = "")
        {
            ViewBag.aid = aid;
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1).ToList();
            UserAddress userAddress = null;
            if (aid != 0)
            {
                userAddress = work.UserAddressRepository.GetByID(aid);
            }
            if (userAddress != null)
            {
                ViewBag.AddressModel = userAddress;
                ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == userAddress.Province).ToList();
                ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == userAddress.City).ToList();
            }
            ViewBag.AddressModel = userAddress;

            return View();
        }

        #endregion

        #region 新增/保存收货地址信息-POST

        /// <summary>
        /// 新增/保存收货地址信息
        /// </summary>
        /// <param name="userAddress"></param>
        /// <param name="cart"></param>
        /// <param name="at"></param>
        /// <param name="aid"></param>
        /// <param name="AddressPrefix"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddressSave(UserAddress userAddress, int aid = 0, string AddressPrefix = "")
        {
            ViewBag.aid = aid;
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1).ToList();

            if (userAddress != null)
            {
                ViewBag.AddressModel = userAddress;
                ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == userAddress.Province).ToList();
                ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == userAddress.City).ToList();

                ViewBag.Province = userAddress.Province;
                ViewBag.City = userAddress.City;
                ViewBag.Region = userAddress.Region;
            }

            if (string.IsNullOrEmpty(userAddress.UserName))
            {
                ModelState.AddModelError("username", "请输入收货人");
            }
            if (userAddress.Province == 0 || userAddress.City == 0 || userAddress.Region == 0)
            {
                ModelState.AddModelError("area", "请选择完整地区");
            }
            if (string.IsNullOrEmpty(userAddress.Address))
            {
                ModelState.AddModelError("address", "请输入详细地址");
            }
            if (string.IsNullOrEmpty(userAddress.Tel) && string.IsNullOrEmpty(userAddress.Mobile))
            {
                ModelState.AddModelError("tel", "手机号码和电话至少填写一项");
            }
            //if (string.IsNullOrEmpty(userAddress.Post_Code))
            //{
            //    ModelState.AddModelError("post", "请输入邮编");
            //}


            if (ModelState.IsValid)
            {
                userAddress.UserID = LoginedUserModel.ID;
                if (!string.IsNullOrEmpty(userAddress.Address))
                {
                    userAddress.Address = AddressPrefix + " " + userAddress.Address.Replace(" ", "");
                }

                if (aid == 0)
                {
                    work.UserAddressRepository.Insert(userAddress);
                    work.Save();
                }
                else
                {
                    //userAddress.ID = aid;
                    work.UserAddressRepository.Update(userAddress);
                    work.Save();
                }

                //更新默认设置
                if (userAddress.Is_Default == 1)
                {
                    UserAddressService.SetDefault(LoginedUserModel.ID, userAddress.ID);
                }

                //work.Dispose();
                return RedirectToAction("Address");
            }
            else
            {
                return View("AddressAdd");
            }
        }

        #endregion

        #region 删除收货人地址

        /// <summary>
        /// 删除收货人地址
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        public ActionResult AddressDelete(int aid)
        {
            if (aid != 0)
            {
                var model = work.UserAddressRepository.Get(m => m.ID == aid).FirstOrDefault<UserAddress>();
                work.UserAddressRepository.Delete(model);
                work.Save();
                //work.Dispose();
            }
            return RedirectToAction("Address");
        }

        #endregion

        #endregion

        #region 收藏夹

        /// <summary>
        /// 收藏夹列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Favorite(int page = 1)
        {
            ViewBag.page = page;

            var rst = work.Context.Favorites.Join(work.Context.Goods, c => c.GoodsID, g => g.ID, (c, g) => new
            {
                c,
                g.G_Name,
                g.G_Image,
                g.G_Price
            }).Where(m => m.c.UserID == LoginedUserModel.ID)
            .Select(m => new FavoriteVModel
            {
                Favorite = m.c,
                Title = m.G_Name,
                PhotoUrl = m.G_Image,
                Price = m.G_Price
            }).OrderByDescending(m => m.Favorite.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        public ActionResult FavoriteDelete(int ID)
        {
            if (ID != 0)
            {
                var model = work.FavoriteRepository.Get(m => m.ID == ID).FirstOrDefault<Favorite>();
                work.FavoriteRepository.Delete(model);
                work.Save();
                //work.Dispose();
            }
            return RedirectToAction("Favorite");
        }



        #endregion

        #region 浏览记录

        /// <summary>
        /// 浏览记录
        /// </summary>
        /// <returns></returns>
        public ActionResult BrowseRecord(int page = 1)
        {
            ViewBag.page = page;
            int itemType = Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.课程);
            var rst = work.Context.BrowseRecords.Join(work.Context.Goods, br => br.BR_ItemID, g => g.ID, (br, g) => new
            {
                br,
                g.G_Name,
                g.G_Image,
                g.G_Price
            }).Where(m => m.br.UserID == LoginedUserModel.ID && m.br.BR_ItemType == itemType)
            .Select(m => new BrowseRecordGoodsVModel
            {
                BrowseRecord = m.br,
                Title = m.G_Name,
                PhotoUrl = m.G_Image,
                Price = m.G_Price
            }).OrderByDescending(m => m.BrowseRecord.ID);

            int pageSize = 50;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 积分挑战

        /// <summary>
        /// 积分挑战记录
        /// </summary>
        /// <returns></returns>
        public ActionResult TiaoZhan(int page = 1)
        {
            //var rst = work.Context.Where(m => m.UserID == LoginedUserModel.ID);
            //rst = rst.OrderByDescending(m => m.ID);

            //int pageSize = 20;
            //int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View();
        }

        /// <summary>
        /// 发起挑战
        /// </summary>
        /// <returns></returns>
        public ActionResult TiaoZhanNew(int ID = 0)
        {
            //ViewBag.ID = ID;
            //ViewBag.ShowApplyBtn = 1;
            //ViewBag.User = work.Context.Users.Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault<User>();

            //var existListCount = work.Context.TiXians.Where(m => m.UserID == LoginedUserModel.ID).Where(m => m.TX_Status == 0 || m.TX_Status == 1).Count();//存在待处理或已审核记录，暂不允许提交申请
            //if (existListCount > 0)
            //{
            //    ViewBag.ShowApplyBtn = 0;
            //}

            //if (ID != 0)
            //{
            //    TiXian model = work.Context.TiXians.Where(m => m.ID == ID).FirstOrDefault();
            //    return View(model);
            //}
            return View();
        }
        /// <summary>
        /// 发起挑战
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TiaoZhanNew(TiXian newModel, int ShowApplyBtn = 1)
        {
            //ViewBag.ID = newModel.ID;
            //ViewBag.ShowApplyBtn = ShowApplyBtn;
            //User user = work.Context.Users.AsNoTracking().Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault();
            //ViewBag.User = user;

            //if (ModelState.IsValid)
            //{
            //    #region 验证
            //    if (string.IsNullOrEmpty(newModel.TX_UserName))
            //    {
            //        ModelState.AddModelError("TX_UserName", "请输入收款人姓名");
            //        return View(newModel);
            //    }

            //    if (string.IsNullOrEmpty(newModel.TX_BankName))
            //    {
            //        ModelState.AddModelError("TX_BankName", "请输入收款银行名称");
            //        return View(newModel);
            //    }
            //    if (string.IsNullOrEmpty(newModel.TX_BankNumber))
            //    {
            //        ModelState.AddModelError("TX_BankNumber", "请输入收款银行账号");
            //        return View(newModel);
            //    }
            //    if (newModel.TX_Amount <= 1)
            //    {
            //        ModelState.AddModelError("TX_Amount", "请输入提现金额，且不能小于1元");
            //        return View(newModel);
            //    }
            //    if (newModel.TX_Amount > LoginedUserModel.U_Amount)
            //    {
            //        ModelState.AddModelError("TX_Amount", "提现金额错误！");
            //        return View(newModel);
            //    }
            //    #endregion


            //    if (user != null)
            //    {

            //        newModel.UserID = LoginedUserModel.ID;
            //        newModel.TX_Number = TiXianService.GetTiXianNumber(LoginedUserModel.ID);
            //        user.U_Amount = user.U_Amount - newModel.TX_Amount;
            //        //添加金额调整记录
            //        UserAmountHistoryService.Insert(user.ID, newModel.TX_Amount, user.U_Amount, 0, user.U_LockAmount, 0, "提现", "申请提现锁定金额", user.ID, user.U_UserName);
            //        user.U_LockAmount = user.U_LockAmount + newModel.TX_Amount;
            //        UserAmountHistoryService.Insert(user.ID, 0, user.U_Amount, newModel.TX_Amount, user.U_LockAmount, 1, "提现", "申请提现锁定金额", user.ID, user.U_UserName);
            //        work.UserRepository.Update(user);
            //        work.TiXianRepository.Insert(newModel);
            //        work.Save();

            //        UserService.SetCacheUser(user.U_UserName, user);
            //    }

            //    //返回列表
            //    return RedirectToAction("TiXian");
            //}
            return View(newModel);
        }
        /// <summary>
        /// 发起挑战 - 取消
        /// </summary>
        /// <returns></returns>
        public ActionResult TiaoZhanCancel(int ID = 0)
        {
            //ViewBag.ID = ID;
            //if (ID != 0)
            //{
            //    TiXian existModel = work.Context.TiXians.Where(m => m.UserID == LoginedUserModel.ID && m.ID == ID).FirstOrDefault();//存在
            //    User user = work.Context.Users.AsNoTracking().Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault();
            //    if (existModel != null && user != null && existModel.TX_Status == Convert.ToInt16(DataConfig.TiXianStatusEnum.待处理))
            //    {
            //        user.U_Amount = user.U_Amount + existModel.TX_Amount;
            //        UserAmountHistoryService.Insert(user.ID, existModel.TX_Amount, user.U_Amount, 0, user.U_LockAmount, 1, "取消提现", "取消提现解除锁定金额", ID, user.U_UserName);
            //        user.U_LockAmount = user.U_LockAmount - existModel.TX_Amount;
            //        UserAmountHistoryService.Insert(user.ID, 0, user.U_Amount, existModel.TX_Amount, user.U_LockAmount, 0, "取消提现", "取消提现解除锁定金额", ID, user.U_UserName);
            //        work.UserRepository.Update(user);
            //        work.Save();

            //        existModel.TX_Status = Convert.ToInt16(DataConfig.TiXianStatusEnum.已取消);

            //        work.TiXianRepository.Update(existModel);
            //        work.Save();
            //    }

            //    //TiXianService.UpdateStatus(ID, Convert.ToInt16(DataConfig.TiXianStatusEnum.取消));
            //}
            return RedirectToAction("TiXian");
        }


        #endregion

        #region 签到积分

        public ActionResult ScoreList(int page = 1)
        {
            //收支明细
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserScoreHistorys.Where(m => m.UserID == LoginedUserModel.ID && m.Is_Delete == 0);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.ID = LoginedUserModel.ID;
            ViewBag.User = work.Context.Users.Where(m => m.ID == LoginedUserModel.ID).FirstOrDefault<User>();

            return View(rst.ToPagedList(pageNumber, pageSize));
        }
        //删除记录
        public ActionResult UserScoreHistoryDelete(int ID = 0)
        {
            if (ID != 0)
            {
                var m = work.UserScoreHistoryRepository.Get(g => g.ID == ID && g.UserID == LoginedUserModel.ID).FirstOrDefault<UserScoreHistory>();
                m.Is_Delete = 1;

                work.UserScoreHistoryRepository.Update(m);
                work.Save();
                //work.Dispose();
            }
            return RedirectToAction("ScoreList");
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        public ActionResult ScoreSign(int page = 1)
        {
            var rst = work.Context.UserScoreHistorys.Where(m => m.UserID == LoginedUserModel.ID && m.Is_Delete == 0);
            rst = rst.OrderByDescending(m => m.ID);

            //获取当前签到数据
            DateTime timeFrom = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
            ViewBag.SignList = work.Context.SignIns.AsNoTracking().Where(m => m.UserID == LoginedUserModel.ID).Where(m => m.Sign_CreateTime > timeFrom).OrderByDescending(m => m.ID).ToList();
            ViewBag.TotalCount = work.Context.SignIns.AsNoTracking().Where(m => m.UserID == LoginedUserModel.ID).Count();


            return View(rst.ToPagedList(page, 50));
        }
        /// <summary>
        /// 签到操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScoreSignDo()
        {
            DateTime dateFrom = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            SignIn existModel = work.SignInRepository.Get(m => m.UserID == LoginedUserModel.ID && m.Sign_CreateTime > dateFrom).FirstOrDefault();
            if (existModel == null)
            {
                existModel = new SignIn();
                existModel.Sign_Content = "签到";
                existModel.Sign_IP = IPHelper.GetIP();
                existModel.UserID = LoginedUserModel.ID;
                existModel.Sign_CreateTime = DateTime.Now;

                work.SignInRepository.Insert(existModel);

                //签到送积分
                User user = work.UserRepository.GetByID(LoginedUserModel.ID);
                int score = Convert.ToInt16(ConfigHelper.GetConfigString("SignInScore"));
                user.U_Score = user.U_Score + score;

                work.UserRepository.Update(user);
                work.Save();

                UserService.SetCacheUser(user.U_UserName, user);

                UserScoreHistoryService.Insert(user.ID, score, user.U_Score, 0, user.U_LockScore, 1, "收入", "签到送积分", existModel.ID, user.U_UserName);

                json.Data = new { status = "success", msg = "签到成功" };
            }
            else
            {
                json.Data = new { status = "success", msg = "今日已签到" };
            }
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 积分排行榜

        /// <summary>
        /// 积分排行榜
        /// </summary>
        /// <returns></returns>
        public ActionResult ScoreRanking(int page = 1)
        {
            var rst = work.Context.Users.Where(m => m.U_IsDelete == 0 && m.U_Is_Enable == 1).OrderByDescending(m => m.U_Score).Take(100).ToList();
            ViewBag.MyRanking = work.Context.Users.Where(m => m.U_IsDelete == 0 && m.U_Is_Enable == 1 && m.U_Score > LoginedUserModel.U_Score).Count() + 1;
            int pageSize = 100;
            return View(rst.ToPagedList(page, pageSize));
        }

        #endregion

        #region 培训需求

        /// <summary>
        /// 培训需求
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult PeiXunApply(int page = 1)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            var rst = work.Context.ConsultMessages.Where(m => m.UserID == LoginedUserModel.ID).OrderByDescending(m => m.ID);
            int pageSize = 50;
            return View(rst.ToPagedList(page, pageSize));
        }


        #endregion

        #region 培训活动报名记录

        public ActionResult ActivityList(int page = 1)
        {
            var rst = work.Context.AdvertisementRecords
               .Join(work.Context.Advertisements, ar => ar.AdvertisementID, a => a.ID, (ar, a) => new { ar, a })
               .Select(m => new AdvertisementRecordVModel
               {
                   AdvertisementRecord = m.ar,
                   Title = m.a.AD_Title
               })
            .Where(m => m.AdvertisementRecord.UserID == LoginedUserModel.ID)
            .OrderByDescending(m => m.AdvertisementRecord.ID);
            int pageSize = 50;
            return View(rst.ToPagedList(page, pageSize));
        }

        #endregion

        #region 用户推广

        /// <summary>
        /// 用户推广二维码
        /// </summary>
        /// <returns></returns>
        public ActionResult InfoReferrer(int page = 1)
        {
            ViewBag.ReferrerUrl = "";
            ViewBag.QRCodeUrl = "";
            ViewBag.page = page;
            if (LoginedUserModel != null)
            {
                //推荐链接
                string referrerUrl = WebSiteConfig.WebSiteDomain + "/shen/?u=" + LoginedUserModel.ID;
                ViewBag.ReferrerUrl = referrerUrl;
                //推荐链接二维码图片地址
                ViewBag.QRCodeUrl = QRCodeHelper.CreateQrCodeCard(referrerUrl, LoginedUserModel.ID);
                //ViewBag.QRCodeUrl = QRCodeHelper.CreateQrCode(referrerUrl, LoginedUserModel.ID);
                
                //ViewBag.UserList = work.Context.Users.Where(m => m.Referrer == LoginedUserModel.ID).ToList();
                //return View(LoginedUserModel);

                var rst = work.Context.Users.Where(m => m.Referrer == LoginedUserModel.ID).OrderByDescending(m => m.ID);
                return View(rst.ToPagedList(page, 20));
            }
            return View();
        }
        #endregion

    }
}