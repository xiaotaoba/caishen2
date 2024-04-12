using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;using Pannet.Utility;

namespace Pannet.Web.Controllers
{
    public class UserRoleController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 用户角色

        //默认用户角色
        [CheckPermission]
        public ActionResult Index()
        {
            return View(work.UserRoleRepository.Get());
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult UserRoleAdd(int ID = 0)
        {
            if (ID != 0)
            {
                var role = work.UserRoleRepository.Get(g => g.ID == ID).FirstOrDefault<UserRole>();
                return View(role);
            }
            return View();
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult UserRoleAdd(UserRole userRoleModel, int ID = 0)
        {
            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    var role = work.UserRoleRepository.Get(g => g.Role_Name == userRoleModel.Role_Name);
                    if (role.Count() > 0)
                    {
                        //return json("角色名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("Name", "角色名称已存在");
                    }
                    else
                    {
                        work.UserRoleRepository.Insert(userRoleModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var oldGroup = work.UserRoleRepository.Get(g => g.ID == ID).FirstOrDefault<UserRole>();
                    var group = work.UserRoleRepository.Get(g => g.Role_Name == userRoleModel.Role_Name & g.ID != ID);
                    if (group.Count() > 0)
                    {
                        //return json("角色名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("Name", "角色名称已存在");
                    }
                    else
                    {
                        oldGroup.Role_Name = userRoleModel.Role_Name;
                        oldGroup.Role_Desc = userRoleModel.Role_Desc;

                        work.UserRoleRepository.Update(oldGroup);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(userRoleModel);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult UserRoleDelete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1 || ID == 2 || ID == 3)
                {
                    return RedirectToAction("Index");
                }
                var role = work.UserRoleRepository.Get(g => g.ID == ID).FirstOrDefault<UserRole>();
                work.UserRoleRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}