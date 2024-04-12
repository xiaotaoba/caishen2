using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.Models;
using Pannet.Utility;
using Pannet.Web.Attribute;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using System.Web.UI;
using Pannet.DAL.Repository;
using PagedList;

namespace Pannet.Web.Controllers
{
    public class ManagerController : Controller
    {

        public UnitOfWork work = new UnitOfWork();

        [CheckLogin]
        public ActionResult Index()
        {
            return View(ManagerService.GetLoginModel());
        }

        //欢迎页
        [CheckLogin]
        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Fanli()
        {
            Log.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":手动返利开始！", "定时任务", DateTime.Now.ToString("yyyyMMdd"));

            int rs = UserService.FanJiFen();
            if (rs == 1)
            {
                ViewBag.Message = "今天已返利！";
            }
            else if (rs == 2)
            {
                ViewBag.Message = "无需返利记录！";
            }
            else if (rs == 0)
            {
                ViewBag.Message = "今日返利完成！";
            }
            return View("Welcome");
        }



        //退出
        public void Logout()
        {
            CookieHelper.Delete(ConfigHelper.CookieAdminName);
            Response.Redirect("/Manager/Login");
        }

        #region 登录

        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(AdminLoginVModel loginModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    loginModel.Password = UtilityClass.GetMD5(loginModel.Password);
                    //var user = context.Users.Where(u => u.U_UserName == username & u.U_Pwd == password).Select(p => p.UserRoles.Where(ur => ur.RoleID == role)).FirstOrDefault();
                    //var user = work.Context.Users.Where(u => u.U_UserName == username & u.U_Pwd == password).Include(u => u.UserRoles).ToList();
                    var user = work.ManagerRepository.Get(u => u.UserName == loginModel.UserName & u.Password == loginModel.Password, null).ToList<Manager>();
                    if (user.Count() > 0)
                    {
                        //ViewBag.LoginMessage = string.Format("登录成功！user:{0},role:{1}", user.Count(), user[0].UserRoles.Where(ur => ur.RoleID == role).Count());

                        CookieHelper.SetValue(ConfigHelper.CookieAdminName, user[0].UserName, ConfigHelper.CookieExpries);
                        Manager model = user.FirstOrDefault();
                        LogService.Add(model.UserName, model.ID, "登录管理后台", model.ID.ToString());
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.LoginMessage = string.Format("账号或密码错误");
                        //ModelState.AddModelError("password", "用户名或密码错误");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.LoginMessage = string.Format("登录失败：{0}", e.Message);
                }
            }
            return View();
        }

        #endregion

        #region 管理员管理

        //列表
        [CheckLogin]
        [CheckPermission]
        public ActionResult AdminList(int GroupID = 0, string keyword = "")
        {
            ViewBag.LoginedAdminModel = ViewData["manager"] as Manager;
            //work.Context.Configuration.LazyLoadingEnabled = false;
            if (!string.IsNullOrEmpty(keyword) && GroupID != 0)
            {
                ViewBag.Keyword = keyword;
                //IEnumerable<Manager> adminList = work.Context.Managers.Where(m => m.UserName.Contains(keyword)).Include(m => m.ManagerWithGroup.Select(mg => mg.Group)).Select(m => new
                //{
                //    Manager = m,
                //    ManagerWithGroup = m.ManagerWithGroup.Where(mg => mg.ManagerGroupID == GroupID)
                //}).ToList().Select(m => m.Manager).ToList();
                var adminList = work.Context.Managers.Where(m => m.UserName.Contains(keyword) & m.ManagerWithGroup.FirstOrDefault().ManagerGroupID == GroupID).ToList();

                return View(adminList);
                //return View(work.ManagerRepository.Get(m => m.UserName.Contains(keyword) & m., null, "Manager,Group"));
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                return View(work.ManagerRepository.Get(m => m.UserName.Contains(keyword)));
            }
            else if (GroupID != 0)
            {
                ////.Include(m => m.ManagerWithGroup.Select(mg => mg.Group))
                //var adminList = work.Context.Managers.Select(m => new
                //{
                //    m,
                //    ManagerWithGroup = m.ManagerWithGroup.Where(mg => mg.ManagerGroupID == GroupID)
                //}).AsEnumerable().Select(x => x.m).ToList();
                ////Group group = work.Context.Groups.Find(GroupID);
                ////var adminList = work.Context.Entry(group).Collection(m=>m.ManagerWithGroup).Query();

                var adminList = work.Context.Managers.Where(m => m.ManagerWithGroup.FirstOrDefault().ManagerGroupID == GroupID).ToList();
                return View(adminList);
            }
            else
            {
                return View(work.ManagerRepository.Get());
                //return View(work.ManagerRepository.Get(null, null, "ManagerWithGroup.Group"));
            }
            //return View();
        }

        //显示
        [CheckLogin]
        [CheckPermission]
        public ActionResult AdminAdd(int ID = 0)
        {
            //角色
            ViewBag.Groups = work.ManagerGroupRepository.Get();

            if (ID != 0)
            {
                var manager = work.ManagerRepository.Get(g => g.ID == ID).FirstOrDefault<Manager>();
                if (manager != null)
                {
                    AdminEditVModel editModel = new AdminEditVModel();
                    editModel.ID = manager.ID; 
                    editModel.Phone = manager.Phone;
                    editModel.UserName = manager.UserName;
                    editModel.GroupID = work.Context.ManagerWithGroups.Where(m=>m.ManagerID==manager.ID).Select(m=>m.ManagerGroupID).FirstOrDefault();
                    return View(editModel);
                }
            }
            return View();
        }

        //提交
        [CheckLogin]
        [CheckPermission]
        [HttpPost]
        public ActionResult AdminAdd(AdminEditVModel _model, int ID = 0)
        {
            //角色
            ViewBag.Groups = work.ManagerGroupRepository.Get();

            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    var manager = work.ManagerRepository.Get(g => g.UserName == _model.UserName);
                    if (manager.Count() > 0)
                    {
                        //return json("角色名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("UserName", "管理账号已存在");
                        return View();
                    }

                    if (string.IsNullOrEmpty(_model.Password))
                    {
                        ModelState.AddModelError("Password", "密码不能为空");
                        return View();
                    }

                    Manager managerModel = new Manager();
                    managerModel.UserName = _model.UserName;
                    managerModel.Phone = _model.Phone;
                    managerModel.Password = UtilityClass.GetMD5(_model.Password);

                    //添加管理员
                    work.ManagerRepository.Insert(managerModel);
                    work.Save();

                    Manager newManager = work.ManagerRepository.Get(u => u.UserName == managerModel.UserName).FirstOrDefault();

                    //添加管理员角色关系
                    work.ManagerWithGroupRepository.Insert(new ManagerWithGroup { ManagerGroupID = _model.GroupID, ManagerID = newManager.ID });
                    work.Save();
                    work.Dispose();

                    LogService.Add(ManagerService.GetLoginModel(), "新增管理员", newManager.ID.ToString());
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                }//编辑
                else
                {
                    var oldManager = work.ManagerRepository.Get(g => g.ID == ID).FirstOrDefault<Manager>();
                    var manager = work.ManagerRepository.Get(g => g.UserName == _model.UserName & g.ID != ID);
                    if (manager.Count() > 0)
                    {
                        //return json("角色名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("UserName", "管理账号已存在");
                        return View();
                    }

                    oldManager.UserName = _model.UserName;
                    oldManager.Phone = _model.Phone;
                    //密码不为空，则修改
                    if (!string.IsNullOrEmpty(_model.Password))
                    {
                        oldManager.Password = UtilityClass.GetMD5(_model.Password);
                    }

                    work.ManagerRepository.Update(oldManager);
                    //work.Save();
                    //work.Dispose();

                    work.Context.ManagerWithGroups.AddOrUpdate(m => m.ManagerID, new ManagerWithGroup { ManagerGroupID = _model.GroupID, ManagerID = oldManager.ID });
                    work.Save();
                    work.Dispose();

                    LogService.Add(ManagerService.GetLoginModel(), "编辑管理员信息", oldManager.ID.ToString());
                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    return RedirectToAction("AdminList");

                }
            }
            return View();
        }

        //删除
        [CheckLogin]
        [CheckPermission]
        public ActionResult AdminDelete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1 || ID == 2)
                {
                    return RedirectToAction("AdminList");
                }
                var nanager = work.ManagerRepository.Get(g => g.ID == ID).FirstOrDefault<Manager>();
                work.ManagerRepository.Delete(nanager);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("AdminList");
        }

        #endregion

        #region  管理员角色

        //显示
        [CheckLogin]
        //[CheckPermission]
        public ActionResult GroupAdd(int ID = 0)
        {
            if (ID != 0)
            {
                var group = work.ManagerGroupRepository.Get(g => g.ID == ID).FirstOrDefault<ManagerGroup>();
                return View(group);
            }
            return View(new ManagerGroup());
        }

        //提交
        [CheckLogin]
        //[CheckPermission]
        [HttpPost]
        public ActionResult GroupAdd(ManagerGroup groupModel, int ID = 0)
        {
            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    var group = work.ManagerGroupRepository.Get(g => g.Name == groupModel.Name);
                    if (group.Count() > 0)
                    {
                        //return json("角色名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("Name", "角色名称已存在");
                    }
                    else
                    {
                        work.ManagerGroupRepository.Insert(groupModel);
                        work.Save();
                        work.Dispose();

                        LogService.Add(ManagerService.GetLoginModel(), "新增管理员角色", "0");
                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var oldGroup = work.ManagerGroupRepository.Get(g => g.ID == ID).FirstOrDefault<ManagerGroup>();
                    var group = work.ManagerGroupRepository.Get(g => g.Name == groupModel.Name & g.ID != ID);
                    if (group.Count() > 0)
                    {
                        //return json("角色名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("Name", "角色名称已存在");
                    }
                    else
                    {
                        oldGroup.Name = groupModel.Name;
                        oldGroup.Desc = groupModel.Desc;
                        oldGroup.Limits = groupModel.Limits;// 只获取到一个值; Request.Form["limits"];//

                        work.ManagerGroupRepository.Update(oldGroup);
                        work.Save();
                        work.Dispose();

                        LogService.Add(ManagerService.GetLoginModel(), "编辑管理员角色", ID.ToString());
                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                    }
                }
            }
            return View(groupModel);
        }

        //列表
        [CheckLogin]
        [CheckPermission]
        public ActionResult GroupList()
        {
            return View(work.ManagerGroupRepository.Get());
        }

        //删除
        [CheckLogin]
        //[CheckPermission]
        public ActionResult GroupDelete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("GroupList");
                }
                var group = work.ManagerGroupRepository.Get(g => g.ID == ID).FirstOrDefault<ManagerGroup>();
                work.ManagerGroupRepository.Delete(group);
                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "删除管理员角色", ID.ToString());
            }
            return RedirectToAction("GroupList");
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CheckLogin]
        public JsonResult CheckGroupName(string Name, int ID = 0)
        {
            bool result = true;
            if (ID == 0)//新增
            {
                var group = work.ManagerGroupRepository.Get(g => g.Name == Name);
                if (group.Count() > 0)
                {
                    result = false;
                }
            }
            else
            {
                var oldGroup = work.ManagerGroupRepository.Get(g => g.ID == ID).FirstOrDefault<ManagerGroup>();
                var group = work.ManagerGroupRepository.Get(g => g.Name == Name & g.ID != ID);
                if (group.Count() > 0)
                {
                    result = false;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 操作记录

        //列表
        [CheckLogin]
        [CheckPermission]
        public ActionResult AdminLogs(int ManagerID = 0, int page = 1)
        {
            ViewBag.ManagerID = ManagerID;
            var rst = work.Context.ManagerLogs.AsQueryable();
            if (ManagerID != 0)
            {
                rst = rst.Where(m => m.ManagerID == ManagerID);
            }
            rst = rst.OrderByDescending(u => u.ID);//skip必须
            //return View(rst.ToList());

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
            //return View();
        }


        //删除
        [CheckLogin]
        [CheckPermission]
        public ActionResult LogDelete(int ID = 0)
        {
            if (ID != 0)
            {
                var group = work.ManagerLogRepository.Get(g => g.ID == ID).FirstOrDefault();
                work.ManagerLogRepository.Delete(group);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("AdminLogs");
        }

        #endregion
    }
}