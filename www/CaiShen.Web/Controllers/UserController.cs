using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Models;
using Pannet.DAL;
using Pannet.Utility;
using PagedList;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using Pannet.Web.Attribute;
using Pannet.DAL.Repository;
using System.IO;
using System.Data.OleDb;
using System.Data;

//using System.ComponentModel.DataAnnotations;

namespace Pannet.Web.Controllers
{
    public class UserController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        public int im_UserCount = 0;//导入用户
        public int im_SkipCount = 0;//跳过记录
        private int tag_education = Convert.ToInt16(DataConfig.TagTypeEnum.学历);

        #region 会员列表

        [CheckPermission]
        public ActionResult Index(string field = "username", string keyword = "", string ddlUserRole = "", string ddlUserLevel = "", int DepartmentID = 0, int page = 1, string action = "")
        {
            ViewBag.Field = field;
            ViewBag.action = action;
            ViewBag.Keyword = keyword;
            ViewBag.UserRoles = work.UserRoleRepository.Get();
            ViewBag.UserLevels = work.UserLevelRepository.Get();
            ViewBag.Role = ddlUserRole;
            ViewBag.Level = ddlUserLevel;
            ViewBag.DepartmentID = DepartmentID;
            ViewBag.Departments = work.DepartmentRepository.Get();

            #region 批量操作

            if (!string.IsNullOrEmpty(action))
            {
                string ids = Request.Form["ids"];
                if (!string.IsNullOrEmpty(ids))
                {
                    string[] arrIds = ids.Trim(',').Split(',');
                    if (action == "delete")//批量删除
                    {
                        User model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.UserRepository.GetByID(Convert.ToInt32(a_id));
                                if (model != null)
                                {
                                    work.UserRepository.Delete(model);
                                    work.Save();
                                }
                            }
                        }
                    }
                    //else if (action == "update")//批量更新
                    //{
                    //    Goods model;
                    //    foreach (var a_id in arrIds)
                    //    {
                    //        if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                    //        {
                    //            model = work.GoodsRepository.GetByID(Convert.ToInt32(a_id));

                    //            model.G_Sort = Convert.ToInt32(Request.Form["sort_" + a_id]);
                    //            model.G_Name = Request.Form["name_" + a_id];
                    //            model.G_MarketPrice = Convert.ToDecimal(Request.Form["marketprice_" + a_id]);
                    //            model.G_Price = Convert.ToDecimal(Request.Form["price_" + a_id]);
                    //            model.G_Unit = Request.Form["unit_" + a_id];
                    //            model.G_UnitInfo = Request.Form["unitinfo_" + a_id];
                    //            model.G_SortMobile = Convert.ToInt32(Request.Form["mobilesort_" + a_id]);

                    //            work.GoodsRepository.Update(model);
                    //            work.Save();
                    //        }
                    //    }
                    //}
                    else if (action == "qy")//启用
                    {
                        User model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.UserRepository.GetByID(Convert.ToInt32(a_id));

                                model.U_Is_Enable = 1;

                                work.UserRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                    else if (action == "bqy")//不启用
                    {
                        User model;
                        foreach (var a_id in arrIds)
                        {
                            if (!string.IsNullOrEmpty(a_id) && a_id != "0")
                            {
                                model = work.UserRepository.GetByID(Convert.ToInt32(a_id));

                                model.U_Is_Enable = 0;

                                work.UserRepository.Update(model);
                                work.Save();
                            }
                        }
                    }
                }

            }


            #endregion


            var rst = work.Context.Users//.Where(m => m.U_IsDelete == 0)
                .GroupJoin(work.Context.Departments, u => u.U_DepartmentID, d => d.ID, (u, d) => new { u, d });

            if (DepartmentID != 0)
            {
                rst = rst.Where(m => m.d.Where(d => d.ID == DepartmentID).Count() > 0 || m.d.Where(d => d.Dep_FollowID == DepartmentID).Count() > 0);
            }

            if (ddlUserRole != "")
            {
                int _role = Convert.ToInt32(ddlUserRole);
                rst = rst.Where(m => m.u.UserRoleID == _role);
            }
            if (ddlUserLevel != "")
            {
                int _level = Convert.ToInt32(ddlUserLevel);
                rst = rst.Where(m => m.u.UserLevelID == _level);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                switch (field)
                {
                    case "username": rst = rst.Where(m => m.u.U_UserName.Contains(keyword)); break;
                    case "name": rst = rst.Where(m => m.u.U_RealName.Contains(keyword)); break;
                    case "phone": rst = rst.Where(m => m.u.U_Phone.Contains(keyword)); break;
                    case "email": rst = rst.Where(m => m.u.U_Email.Contains(keyword)); break;
                    default: break;
                };
            }
            rst = rst.OrderByDescending(m => m.u.ID);

            var rst_paged = rst.Select(m => m.u);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst_paged.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.UserRepository.Get(m => m.ID == ID).FirstOrDefault();
                if (model != null)
                {
                    work.UserRepository.Delete(model);
                    //model.U_IsDelete = 1;
                    //work.UserRepository.Update(model);
                    work.Save();
                    work.Dispose();
                }
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region 新增会员信息

        /// <summary>
        /// 新增会员信息
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult MemberAdd(int RoleID = 0)
        {
            ViewBag.RoleID = RoleID;
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.UserRoles = work.UserRoleRepository.Get();
            ViewBag.Shops = work.UserShopRepository.Get();
            ViewBag.Departments = work.DepartmentRepository.Get();
            ViewBag.Educations = work.TagRepository.Get(m => m.T_Type == tag_education);

            User userModel = new User();
            userModel.U_Pwd = ConfigHelper.GetConfigString("DefaultPwd");
            return View(userModel);
        }

        /// <summary>
        /// 新增会员信息-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult MemberAdd(User editUser)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.UserRoles = work.UserRoleRepository.Get();
            ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == editUser.U_Province);
            ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == editUser.U_City);
            ViewBag.Shops = work.UserShopRepository.Get();
            ViewBag.Departments = work.DepartmentRepository.Get();
            ViewBag.Educations = work.TagRepository.Get(m => m.T_Type == tag_education);

            if (ModelState.IsValid)
            {
                if (editUser.UserRoleID == 0)
                {
                    ModelState.AddModelError("UserRoleID", string.Format("请选择角色！", editUser.U_Email));
                    return View(editUser);
                }

                User user = work.UserRepository.Get(u => u.U_UserName == editUser.U_UserName).FirstOrDefault<User>();

                if (user != null && user.ID != 0)
                {
                    ModelState.AddModelError("U_UserName", string.Format("账号{0}已存在！", editUser.U_UserName));
                    return View(editUser);
                }
                if (!string.IsNullOrEmpty(editUser.U_Email))
                {
                    user = work.UserRepository.Get(u => u.U_Email == editUser.U_Email).FirstOrDefault<User>();
                    if (user != null && user.ID != 0)
                    {
                        ModelState.AddModelError("U_Email", string.Format("邮箱{0}已存在！", editUser.U_Email));
                        return View(editUser);
                    }
                }
                if (!string.IsNullOrEmpty(editUser.U_Phone))
                {
                    user = work.UserRepository.Get(u => u.U_Phone == editUser.U_Phone).FirstOrDefault<User>();
                    if (user != null && user.ID != 0)
                    {
                        ModelState.AddModelError("U_Phone", string.Format("手机号码{0}已存在！", editUser.U_Phone));
                        return View(editUser);
                    }
                }
                editUser.UserLevelID = Convert.ToInt16(DataConfig.LevelEnum.ZhuCe);
                editUser.U_Pwd = UtilityClass.GetMD5(editUser.U_Pwd);


                work.UserRepository.Insert(editUser);
                work.Save();
                work.Dispose();

                //保存之后，恢复密码为默认密码；
                editUser.U_Pwd = ConfigHelper.GetConfigString("DefaultPwd");
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            //return JavaScript("alert('修改成功!');");

            return View(editUser);
        }

        #endregion

        #region 编辑会员信息

        /// <summary>
        /// 资料修改
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult MemberEdit(int ID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.Shops = work.UserShopRepository.Get();
            ViewBag.Departments = work.DepartmentRepository.Get();
            ViewBag.Educations = work.TagRepository.Get(m => m.T_Type == tag_education);

            if (ID != 0)
            {
                UserPayInfo payinfo = work.PayInfoRepository.Get(p => p.UserID == ID).FirstOrDefault<UserPayInfo>();
                if (payinfo == null)
                {
                    payinfo = new UserPayInfo();
                }
                User userModel = work.UserRepository.Get(m => m.ID == ID).FirstOrDefault<User>();
                EditUserVModel editUser = new EditUserVModel
                {
                    //支付信息

                    AlipayNO = payinfo.AlipayNO,
                    BankAddress = payinfo.BankAddress,
                    BankName = payinfo.BankName,
                    BankNO = payinfo.BankNO,
                    Wechat = payinfo.Wechat,

                    //用户信息
                    Company = userModel.U_Company,
                    Phone = userModel.U_Phone,
                    QQ = userModel.U_QQ,
                    Email = userModel.U_Email,
                    RealName = userModel.U_RealName,
                    NickName = userModel.U_NickName,
                    UserName = userModel.U_UserName,
                    DepartmentID = userModel.U_DepartmentID,
                    Thumbnail = userModel.U_Thumbnail,
                    OpenId = userModel.U_OpenId,
                    U_Birthday = userModel.U_Birthday,
                    U_TiXianAmount = userModel.U_TiXianAmount,

                    //地址
                    Province = userModel.U_Province,
                    City = userModel.U_City,
                    Region = userModel.U_Region,
                    Address = userModel.U_Address,
                    ShopName = userModel.U_ShopName,
                    //状态
                    IsCheck = userModel.U_Is_Check,
                    IsEnable = userModel.U_Is_Enable,
                    UserShopID = userModel.UserShopID,
                    Referrer = userModel.Referrer,
                    Education = userModel.U_Education
                };

                ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == userModel.U_Province);
                ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == userModel.U_City);

                return View(editUser);
            }
            return View(new EditUserVModel());
        }

        /// <summary>
        /// 资料修改-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult MemberEdit(EditUserVModel editUser, int ID = 0)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.Shops = work.UserShopRepository.Get();
            ViewBag.ID = ID;
            ViewBag.Departments = work.DepartmentRepository.Get();
            ViewBag.Educations = work.TagRepository.Get(m => m.T_Type == tag_education);


            if (ModelState.IsValid)
            {
                User user = work.UserRepository.Get(u => u.U_UserName == editUser.UserName).FirstOrDefault<User>();

                ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == editUser.Province);
                ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == editUser.City);

                //用户信息
                user.U_Company = editUser.Company;
                user.U_QQ = editUser.QQ;
                user.U_RealName = editUser.RealName;
                user.U_UserName = editUser.UserName;
                user.U_NickName = editUser.NickName;
                user.U_Height = editUser.Height;
                user.U_HopeWeight = editUser.HopeWeight;
                user.U_Weight = editUser.Weight;
                user.U_DepartmentID = editUser.DepartmentID;
                user.U_Education = editUser.Education;
                user.U_Thumbnail = editUser.Thumbnail;
                user.U_OpenId = editUser.OpenId;
                user.U_Birthday = editUser.U_Birthday;
                user.U_TiXianAmount = editUser.U_TiXianAmount;

                //地址
                user.U_Province = editUser.Province;
                user.U_City = editUser.City;
                user.U_Region = editUser.Region;
                user.U_Address = editUser.Address;
                user.U_ShopName = editUser.ShopName;
                //状态
                user.U_Is_Check = editUser.IsCheck;
                user.U_Is_Enable = editUser.IsEnable;
                user.UserShopID = editUser.UserShopID;
                user.Referrer = editUser.Referrer;

                if (user.U_Email != editUser.Email && !string.IsNullOrEmpty(editUser.Email))//修改邮箱
                {
                    var existUser = work.UserRepository.Get(u => u.U_Email == editUser.Email);
                    if (existUser.Count() > 0)
                    {
                        ModelState.AddModelError("Email", string.Format("邮箱{0}已存在！", editUser.Email));
                        return View(editUser);
                    }
                    else
                    {
                        user.U_Email = editUser.Email;
                    }
                }
                if (user.U_Phone != editUser.Phone && !string.IsNullOrEmpty(editUser.Phone))//修改手机号码
                {
                    var existUser = work.UserRepository.Get(u => u.U_Phone == editUser.Phone);
                    if (existUser.Count() > 0)
                    {
                        ModelState.AddModelError("Phone", string.Format("手机号码{0}已存在！", editUser.Phone));
                        return View(editUser);
                    }
                    else
                    {
                        user.U_Phone = editUser.Phone;
                    }
                }

                work.UserRepository.Update(user);

                //
                //work.Context.Entry(LoginedUserModel).State = EntityState.Modified;
                //work.Save();
                //work.Dispose();

                //支付信息
                UserPayInfo payinfo = new UserPayInfo();
                payinfo.AlipayNO = editUser.AlipayNO;
                payinfo.BankAddress = editUser.BankAddress;
                payinfo.BankName = editUser.BankName;
                payinfo.BankNO = editUser.BankNO;
                payinfo.UserID = user.ID;
                payinfo.Wechat = editUser.Wechat;

                work.Context.UserPayInfos.AddOrUpdate(p => p.UserID, payinfo);
                work.Save();
                work.Dispose();

                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            //return JavaScript("alert('修改成功!');");

            return View(editUser);
        }

        #endregion

        #region 调整会员角色

        /// <summary>
        /// 调整会员角色
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ChangeUserRole(int UserID = 0, int page = 1)
        {
            //调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserRoleHistorys.Where(m => m.UserID == UserID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.UserRoles = work.UserRoleRepository.Get();
            ViewBag.User = work.Context.Users.Where(m => m.ID == UserID).FirstOrDefault<User>();
            ViewBag.ID = UserID;

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 调整会员角色-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult ChangeUserRole(int UserID, int Role, string ddlUserRole, string title, int page = 1)
        {
            //展示调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserRoleHistorys.Where(m => m.UserID == UserID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.UserRoles = work.UserRoleRepository.Get();
            ViewBag.User = work.Context.Users.Where(m => m.ID == UserID).FirstOrDefault<User>();
            ViewBag.ID = UserID;

            if (string.IsNullOrEmpty(ddlUserRole))
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请选择角色");
            }
            else if (string.IsNullOrEmpty(title))
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请填写调整说明");
            }
            else if (ddlUserRole != Role.ToString())//改变角色
            {
                //调整角色
                User oldModel = ViewBag.User;
                oldModel.UserRoleID = Convert.ToInt16(ddlUserRole);
                work.UserRepository.Update(oldModel);

                //添加调整记录
                UserRoleHistory roleHistory = new UserRoleHistory();
                roleHistory.UserID = UserID;
                roleHistory.Current_Role_ID = Convert.ToInt16(ddlUserRole);
                roleHistory.Operator = LoginedAdminModel.UserName;
                roleHistory.Prev_Role_ID = Role;
                roleHistory.Time = DateTime.Now;
                roleHistory.Title = title;

                work.UserRoleHistoryRepository.Insert(roleHistory);
                work.Save();
            }

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        //删除调整记录
        [CheckPermission]
        public ActionResult UserRoleHistoryDelete(int ID = 0, int UserID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1)
                //{
                //    return RedirectToAction("ChangeUserRole", new { UserID = UserID });
                //}
                var m = work.UserRoleHistoryRepository.Get(g => g.ID == ID).FirstOrDefault<UserRoleHistory>();
                work.UserRoleHistoryRepository.Delete(m);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("ChangeUserRole", new { UserID = UserID });
        }


        #endregion

        #region 调整会员等级

        /// <summary>
        /// 调整会员等级
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ChangeUserLevel(int UserID = 0, int page = 1)
        {
            //调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserLevelHistorys.Where(m => m.UserID == UserID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.UserLevels = work.UserLevelRepository.Get();
            ViewBag.User = work.Context.Users.Where(m => m.ID == UserID).FirstOrDefault<User>();
            ViewBag.ID = UserID;

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 调整会员等级-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult ChangeUserLevel(int UserID, int Level, string ddlUserLevel, string title, int page = 1)
        {
            //展示调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserLevelHistorys.Where(m => m.UserID == UserID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.UserLevels = work.UserLevelRepository.Get();
            ViewBag.User = work.Context.Users.Where(m => m.ID == UserID).FirstOrDefault<User>();
            ViewBag.ID = UserID;

            if (string.IsNullOrEmpty(ddlUserLevel))
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请选择角色");
            }
            else if (string.IsNullOrEmpty(title))
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请填写调整说明");
            }
            else if (ddlUserLevel != Level.ToString())//改变角色
            {
                //调整角色
                User oldModel = ViewBag.User;
                oldModel.UserLevelID = Convert.ToInt16(ddlUserLevel);
                work.UserRepository.Update(oldModel);

                //添加调整记录
                UserLevelHistory levelHistory = new UserLevelHistory();
                levelHistory.UserID = UserID;
                levelHistory.Current_Level_ID = Convert.ToInt16(ddlUserLevel);
                levelHistory.Operator = LoginedAdminModel.UserName;
                levelHistory.Prev_Level_ID = Level;
                levelHistory.Time = DateTime.Now;
                levelHistory.Title = title;

                work.UserLevelHistoryRepository.Insert(levelHistory);
                work.Save();
            }

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        //删除调整记录
        [CheckPermission]
        public ActionResult UserLevelHistoryDelete(int ID = 0, int UserID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1)
                //{
                //    return RedirectToAction("ChangeUserLevel", new { UserID = UserID });
                //}
                var m = work.UserLevelHistoryRepository.Get(g => g.ID == ID).FirstOrDefault<UserLevelHistory>();
                work.UserLevelHistoryRepository.Delete(m);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("ChangeUserLevel", new { UserID = UserID });
        }


        #endregion

        #region 调整会员账户余额

        /// <summary>
        /// 调整会员账户余额
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ChangeUserAmount(int UserID = 0, int page = 1)
        {
            //调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserAmountHistorys.Where(m => m.UserID == UserID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.User = work.Context.Users.Where(m => m.ID == UserID).FirstOrDefault<User>();
            ViewBag.ID = UserID;

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 调整会员账户余额-post
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="amount">变动金额</param>
        /// <param name="type">1增加，0减少</param>
        /// <param name="thing">备注</param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult ChangeUserAmount(int UserID, decimal amount, int type, string thing, int page = 1, int amount_type = 0)
        {
            //展示调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserAmountHistorys.Where(m => m.UserID == UserID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.User = work.Context.Users.Where(m => m.ID == UserID).FirstOrDefault<User>();
            ViewBag.ID = UserID;


            if (ModelState.IsValid)
            {

                if (amount <= 0)
                {
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请填写金额");
                    ModelState.AddModelError("amount", string.Format("请填写金额"));
                }
                else if (string.IsNullOrEmpty(thing))
                {
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请填写调整说明");
                    ModelState.AddModelError("thing", string.Format("请填写调整说明"));
                }
                else
                {
                    User oldModel = ViewBag.User;
                    if (amount_type == Convert.ToInt16(DataConfig.AmountTypeEnum.账户余额))
                    {
                        if (type == 0)//减少
                        {
                            oldModel.U_Amount = oldModel.U_Amount - amount;
                        }
                        else
                        {
                            oldModel.U_Amount = oldModel.U_Amount + amount;
                        }

                        //添加调整记录
                        UserAmountHistoryService.Insert(UserID, amount, oldModel.U_Amount, 0, oldModel.U_LockAmount, type, "调整", thing, UserID, LoginedAdminModel.UserName);
                    }
                    else if (amount_type == Convert.ToInt16(DataConfig.AmountTypeEnum.不可用余额))
                    {
                        if (type == 0)//减少
                        {
                            oldModel.U_LockAmount = oldModel.U_LockAmount - amount;
                        }
                        else
                        {
                            oldModel.U_LockAmount = oldModel.U_LockAmount + amount;
                        }
                        //添加调整记录
                        UserAmountHistoryService.Insert(UserID, 0, oldModel.U_Amount, amount, oldModel.U_LockAmount, type, "调整", thing, UserID, LoginedAdminModel.UserName);
                    }
                    //调整余额
                    work.UserRepository.Update(oldModel);
                    work.Save();

                }
            }
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        //删除调整记录
        [CheckPermission]
        public ActionResult UserAmountHistoryDelete(int ID = 0, int UserID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1)
                //{
                //    return RedirectToAction("ChangeUserAmount", new { UserID = UserID });
                //}
                var m = work.UserAmountHistoryRepository.Get(g => g.ID == ID).FirstOrDefault<UserAmountHistory>();
                work.UserAmountHistoryRepository.Delete(m);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("ChangeUserAmount", new { UserID = UserID });
        }


        /// <summary>
        /// 账单记录
        /// </summary>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult UserAmountHistory(int UserID = 0, int page = 1, string keyword = "")
        {
            ViewBag.UserID = UserID;

            int pageSize = 20;
            int pageNumber = page;
            var rst = work.Context.UserAmountHistorys.Where(m => m.Is_Delete == 0);
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.Thing.Contains(keyword) || m.Operator.Contains(keyword) || m.Category.Contains(keyword));
            }
            rst = rst.OrderByDescending(m => m.ID);

            return View(rst.ToPagedList(pageNumber, pageSize));
        }
        //删除调整记录
        //[CheckPermission]
        public ActionResult UserAmountHistoryDel(int ID = 0, int UserID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1)
                //{
                //    return RedirectToAction("ChangeUserAmount", new { UserID = UserID });
                //}
                var m = work.UserAmountHistoryRepository.Get(g => g.ID == ID).FirstOrDefault<UserAmountHistory>();
                work.UserAmountHistoryRepository.Delete(m);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("UserAmountHistory", new { UserID = UserID });
        }

        #endregion

        #region 调整会员积分

        /// <summary>
        /// 调整会员积分
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ChangeUserScore(int UserID = 0, int page = 1)
        {
            //调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserScoreHistorys.Where(m => m.UserID == UserID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.User = work.Context.Users.Where(m => m.ID == UserID).FirstOrDefault<User>();
            ViewBag.ID = UserID;

            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 调整会员积分-post
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="score">变动积分</param>
        /// <param name="type">1增加，0减少</param>
        /// <param name="thing">备注</param>
        /// <param name="page"></param>
        /// <param name="score_type">积分类型：</param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult ChangeUserScore(int UserID, int score, int type, string thing, int page = 1, int score_type = 0)
        {
            //展示调整记录
            int pageSize = 12;
            int pageNumber = page;
            var rst = work.Context.UserScoreHistorys.Where(m => m.UserID == UserID);
            rst = rst.OrderByDescending(m => m.ID);

            ViewBag.User = work.Context.Users.Where(m => m.ID == UserID).FirstOrDefault<User>();
            ViewBag.ID = UserID;


            if (ModelState.IsValid)
            {

                if (score <= 0)
                {
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请填写积分数量");
                    ModelState.AddModelError("score", string.Format("请填写积分数量"));
                }
                else if (string.IsNullOrEmpty(thing))
                {
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请填写调整说明");
                }
                else
                {
                    User oldModel = ViewBag.User;
                    if (score_type == Convert.ToInt16(DataConfig.ScoreTypeEnum.账户积分))
                    {
                        if (type == 0)//减少
                        {
                            oldModel.U_Score = oldModel.U_Score - score;
                        }
                        else
                        {
                            oldModel.U_Score = oldModel.U_Score + score;
                        }

                        //添加调整记录
                        UserScoreHistoryService.Insert(UserID, score, oldModel.U_Score, 0, oldModel.U_LockScore, type, "调整", thing, UserID, LoginedAdminModel.UserName);
                    }
                    else
                    {
                        if (type == 0)//减少
                        {
                            oldModel.U_LockScore = oldModel.U_LockScore - score;
                        }
                        else
                        {
                            oldModel.U_LockScore = oldModel.U_LockScore + score;
                        }
                        //添加调整记录
                        UserScoreHistoryService.Insert(UserID, 0, oldModel.U_Score, score, oldModel.U_LockScore, type, "调整", thing, UserID, LoginedAdminModel.UserName);
                    }

                    //调整积分
                    work.UserRepository.Update(oldModel);
                    work.Save();

                }
            }
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        //删除调整记录
        [CheckPermission]
        public ActionResult UserScoreHistoryDelete(int ID = 0, int UserID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1)
                //{
                //    return RedirectToAction("ChangeUserScore", new { UserID = UserID });
                //}
                var m = work.UserScoreHistoryRepository.Get(g => g.ID == ID).FirstOrDefault<UserScoreHistory>();
                work.UserScoreHistoryRepository.Delete(m);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("ChangeUserScore", new { UserID = UserID });
        }


        #endregion

        #region 用户修改密码

        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeUserPwd(int UserID = 0)
        {
            ViewBag.ID = UserID;
            return View(new ChangePwdVModel());
        }

        /// <summary>
        /// 用户修改密码-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeUserPwd(ChangePwdVModel newModel, int UserID = 0)
        {
            ViewBag.ID = UserID;


            User user = work.UserRepository.Get(u => u.ID == UserID).FirstOrDefault<User>();

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
            work.Dispose();

            ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "修改成功");
            //return JavaScript("alert('修改成功!');");

            return View(newModel);
        }

        #endregion

        #region 导入会员资料

        [CheckPermission]
        public ActionResult ImportUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportUserSave()
        {
            string filename = string.Empty;
            HttpPostedFileBase postFile = Request.Files["postFile"];
            if (postFile == null)
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", string.Format("请选择导入xls文件！", im_UserCount, im_SkipCount));
            }
            else
            {
                //try
                //{
                filename = UpLoadXls(postFile);//上传XLS文件
                ImportXlsToData(filename);//将XLS文件的数据导入数据库                
                if (filename != string.Empty && System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);//删除上传的XLS文件
                }
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", string.Format("数据导入成功！共导入会员数据{0}条，跳过{1}条", im_UserCount, im_SkipCount)); ;
                //}
                //catch (Exception ex)
                //{
                //    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", ex.Message + string.Format("数据导入中断！共导入会员数据{0}条，跳过{1}条", im_UserCount, im_SkipCount));
                //}
            }
            return View("ImportUser");
        }
        /// <summary>
        /// 上传Excel文件
        /// </summary>
        /// <param name="inputfile">上传的控件名</param>
        /// <returns></returns>
        private string UpLoadXls(HttpPostedFileBase postFile)
        {
            string orifilename = string.Empty;
            string uploadfilepath = string.Empty;
            string modifyfilename = string.Empty;
            string fileExtend = "";//文件扩展名
            int fileSize = 0;//文件大小
            try
            {
                if (postFile.ContentLength > 0)
                {
                    //得到文件的大小
                    fileSize = postFile.ContentLength;
                    if (fileSize == 0)
                    {
                        throw new Exception("导入的Excel文件大小为0，请检查是否正确！");
                    }
                    //得到扩展名
                    fileExtend = Path.GetExtension(postFile.FileName).ToLower();
                    if (!fileExtend.Contains("xls"))
                    {
                        throw new Exception("你选择的文件格式不正确，只能导入EXCEL文件！");
                    }
                    //路径
                    uploadfilepath = Server.MapPath("~/Upload/import");
                    //新文件名
                    modifyfilename = System.Guid.NewGuid().ToString();
                    modifyfilename += fileExtend;
                    //判断是否有该目录
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(uploadfilepath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    orifilename = uploadfilepath + "\\" + modifyfilename;
                    //如果存在,删除文件
                    if (System.IO.File.Exists(orifilename))
                    {
                        System.IO.File.Delete(orifilename);
                    }
                    // 上传文件
                    postFile.SaveAs(orifilename);
                }
                else
                {
                    throw new Exception("请选择要导入的Excel文件!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orifilename;
        }


        //// <summary>
        /// 从Excel提取数据--》Dataset
        /// </summary>
        /// <param name="filename">Excel文件路径名</param>
        private void ImportXlsToData(string fileName)
        {
            //try
            //{
            if (fileName == string.Empty)
            {
                throw new ArgumentNullException("Excel文件上传失败！");
            }

            string oleDBConnString = String.Empty;
            oleDBConnString = "Provider=Microsoft.Jet.OLEDB.4.0;";
            oleDBConnString += "Data Source=";
            oleDBConnString += fileName;
            oleDBConnString += ";Extended Properties=Excel 8.0;";
            OleDbConnection oleDBConn = null;
            OleDbDataAdapter oleAdMaster = null;
            DataTable m_tableName = new DataTable();
            DataSet ds = new DataSet();

            oleDBConn = new OleDbConnection(oleDBConnString);
            oleDBConn.Open();
            m_tableName = oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (m_tableName != null && m_tableName.Rows.Count > 0)
            {

                m_tableName.TableName = m_tableName.Rows[0]["TABLE_NAME"].ToString();

            }
            string sqlMaster;
            sqlMaster = " SELECT *  FROM [" + m_tableName.TableName + "A:CV]";
            oleAdMaster = new OleDbDataAdapter(sqlMaster, oleDBConn);
            oleAdMaster.Fill(ds, "m_tableName");
            oleAdMaster.Dispose();
            oleDBConn.Close();
            oleDBConn.Dispose();

            AddDatasetToSQL(ds, 14);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        /// <summary>
        /// 将Dataset的数据导入数据库
        /// </summary>
        /// <param name="pds">数据集</param>
        /// <param name="Cols">数据集列数</param>
        /// <returns></returns>
        private bool AddDatasetToSQL(DataSet pds, int Cols)
        {
            int ic, ir;

            ic = pds.Tables[0].Columns.Count;
            //if (pds.Tables[0].Columns.Count < Cols)
            //{
            //    throw new Exception("导入Excel格式错误！Excel只有" + ic.ToString() + "列");
            //}
            ir = pds.Tables[0].Rows.Count;
            if (pds != null && pds.Tables[0].Rows.Count > 0)
            {
                User newModel = null;
                foreach (DataRow dr in pds.Tables[0].Rows)
                {
                    if (dr[0].ToString() == "用户名" || string.IsNullOrEmpty(dr[0].ToString()) || string.IsNullOrEmpty(dr[1].ToString()) || string.IsNullOrEmpty(dr[4].ToString()))
                    {
                        im_SkipCount++;
                        continue;
                    }

                    string department_str = dr[3].ToString();
                    string role_str = dr[2].ToString();
                    newModel = new Models.User();

                    newModel.U_UserName = dr[0].ToString();
                    newModel.U_RealName = dr[1].ToString();
                    newModel.U_NickName = dr[1].ToString();
                    newModel.U_Phone = dr[4].ToString();
                    newModel.U_Email = dr[5].ToString();
                    newModel.U_Address = dr[6].ToString();
                    newModel.U_ShopName = dr[7] == null ? "" : dr[7].ToString();
                    newModel.U_Province = 0;
                    newModel.U_City = 0;
                    newModel.U_Region = 0;
                    newModel.U_Pwd = UtilityClass.GetMD5(ConfigHelper.GetConfigString("DefaultPwd"));
                    newModel.UserLevelID = Convert.ToInt16(DataConfig.LevelEnum.ZhuCe);//默认等级
                    newModel.UserRoleID = Convert.ToInt16(DataConfig.RoleEnum.注册会员);//角色
                    newModel.U_DepartmentID = 0;//部门

                    //部门
                    Department departmentModel = work.DepartmentRepository.Get(m => m.Dep_Name == department_str).First();
                    if (departmentModel != null)
                    {
                        newModel.U_DepartmentID = departmentModel.ID;
                    }

                    //角色
                    UserRole roleModel = work.UserRoleRepository.Get(m => m.Role_Name == role_str).First();
                    if (roleModel != null)
                    {
                        newModel.UserRoleID = roleModel.ID;
                    }

                    User user = work.UserRepository.Get(u => u.U_UserName == newModel.U_UserName).FirstOrDefault<User>();

                    if (user != null && user.ID != 0)//跳过已存在用户名
                    {
                        im_SkipCount++;
                        continue;
                    }

                    user = work.UserRepository.Get(u => u.U_Phone == newModel.U_Phone).FirstOrDefault<User>();

                    if (user != null && user.ID != 0)//跳过已存在手机
                    {
                        im_SkipCount++;
                        continue;
                    }
                    if (!string.IsNullOrEmpty(newModel.U_Address) && !newModel.U_Address.Contains("总部"))
                    {
                        string[] address_arr = newModel.U_Address.Trim().Split(' ');

                        string province_name = "";
                        string city_name = "";
                        string region_name = "";
                        Area province = null;
                        Area city = null;
                        Area region = null;
                        if (address_arr.Length == 3)
                        {
                            province_name = address_arr[0].Replace("省", "");
                            city_name = address_arr[1].Replace("市", "");
                            region_name = address_arr[2].Replace("区", "");

                            province = work.AreaRepository.Get(u => u.Area_Name.Contains(province_name)).FirstOrDefault();
                            city = work.AreaRepository.Get(u => u.Area_Name.Contains(city_name)).FirstOrDefault();
                            region = work.AreaRepository.Get(u => u.Area_Name.Contains(region_name)).FirstOrDefault();
                        }
                        else if (address_arr.Length == 2)
                        {
                            province_name = address_arr[0].Replace("省", "");
                            city_name = address_arr[1].Replace("市", "");
                            province = work.AreaRepository.Get(u => u.Area_Name.Contains(province_name)).FirstOrDefault();
                            city = work.AreaRepository.Get(u => u.Area_Name.Contains(city_name)).FirstOrDefault();
                        }
                        else
                        {
                            province_name = address_arr[0].Replace("省", "");
                            province = work.AreaRepository.Get(u => u.Area_Name.Contains(province_name)).FirstOrDefault();
                        }

                        if (province != null)
                        {
                            newModel.U_Province = province.ID;
                        }
                        if (city != null)
                        {
                            newModel.U_City = city.ID;
                        }
                        if (region != null)
                        {
                            newModel.U_Region = region.ID;
                        }
                    }
                    work.UserRepository.Insert(newModel);

                    im_UserCount++;
                }

                work.Save();
                work.Dispose();

                // LitMessage.Text = "";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('添加成功！');", true);
            }
            else
            {
                throw new Exception("导入数据为空！");
            }
            return true;
        }


        #endregion

        #region 用户统计

        //用户统计
        [CheckPermission]
        public ActionResult TongJi(string keyword = "", int page = 1, string action = "", string time_start = "", string time_end = "", int province = 0, int city = 0, int region = 0, int DepartmentID = 0)
        {
            ViewBag.keyword = keyword;
            ViewBag.action = action;
            ViewBag.province = province;
            ViewBag.city = city;
            ViewBag.region = region;
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.DepartmentID = DepartmentID;
            ViewBag.Departments = work.DepartmentRepository.Get();

            var rst = work.Context.Users
                .Join(work.Context.Departments, u => u.U_DepartmentID, d => d.ID, (u, d) => new { u, d })
                .GroupJoin(work.Context.Tests, ud => ud.u.ID, t => t.UserID, (ud, t) => new UserTongJiVModel
                {
                    User = ud.u,
                    Department = ud.d,
                    TestCount = t.Count(),
                    TestHegeCount = t.Where(m => m.T_WrongCount == 0).Count(),
                    TestVideoCount = t.Select(m => m.T_GoodsArticleID).Distinct().Count(),
                    VideoCount = work.Context.BrowseRecords.Where(m => m.UserID == ud.u.ID && m.BR_ItemType == 6).Select(m => m.BR_ItemID).Distinct().Count(),
                    PPTCount = work.Context.BrowseRecords.Where(m => m.UserID == ud.u.ID && m.BR_ItemType == 7).Select(m => m.BR_ItemID).Distinct().Count()
                });

            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.User.U_UserName.Contains(keyword) || m.User.U_NickName.Contains(keyword) || m.User.U_RealName.Contains(keyword) || m.User.U_ShopName.Contains(keyword));
            }
            if (province != 0)
            {
                rst = rst.Where(m => m.User.U_Province == province);
            }
            if (city != 0)
            {
                rst = rst.Where(m => m.User.U_City == city);
            }
            if (DepartmentID != 0)
            {
                rst = rst.Where(m => m.User.U_DepartmentID == DepartmentID || m.Department.Dep_FollowID == DepartmentID);
            }
            //if (region != 0)
            //{
            //    rst = rst.Where(m => m.reg == region);
            //}
            rst = rst.Where(m => m.User.U_IsDelete == 0);

            rst = rst.OrderByDescending(m => m.User.ID);

            if (action == "export")//导出
            {
                string fileName = "用户统计数据" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                ExportTongJi(rst.ToList(), fileName);
                try
                {
                }
                catch (Exception ex)
                {
                    Response.End();
                }
            }

            int pageSize = 20;
            int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 导出用户统计数据

        public void ExportTongJi(List<UserTongJiVModel> list, string fileName)
        {
            HttpResponseBase resp;
            resp = HttpContext.Response;
            resp.Charset = "utf-8";
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GBK");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            resp.ContentType = "application/ms-excel";
            string colHeaders = "", ls_item = "";

            colHeaders += "账号" + "\t";
            colHeaders += "姓名" + "\t";
            colHeaders += "部门" + "\t";
            colHeaders += "门店" + "\t";
            colHeaders += "学习视频" + "\t";
            colHeaders += "学习课件" + "\t";
            colHeaders += "测试视频" + "\t";
            colHeaders += "测试次数(满分次数)" + "\t";
            colHeaders += "测试合格率" + "\t";
            colHeaders += "学习课件次数" + "\t";
            colHeaders += "评论次数" + "\t";
            colHeaders += "培训需求提交次数" + "\n";

            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息 

            //逐行处理数据   
            foreach (var itemv in list)
            {
                User user = itemv.User;
                Department department = itemv.Department;
                ls_item += user.U_UserName + "\t";
                ls_item += user.U_RealName + "\t";
                ls_item += department.Dep_Name + "\t";
                ls_item += user.U_ShopName + "\t";
                ls_item += itemv.VideoCount + "\t";
                ls_item += itemv.PPTCount + "\t";
                ls_item += itemv.TestVideoCount + "\t";
                ls_item += itemv.TestCount + "(" + @itemv.TestHegeCount + ")" + "\t";
                ls_item += (itemv.TestCount != 0 ? Math.Round(itemv.TestHegeCount * 100.0 / itemv.TestCount, 2) : 0) + "%\t";
                ls_item += user.U_CoursePPTCount + "\t";
                ls_item += user.U_CommentCount + "\t";
                ls_item += user.U_DemandCount + "\n";

                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }
        #endregion
    }
}