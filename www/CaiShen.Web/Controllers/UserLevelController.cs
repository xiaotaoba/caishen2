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
    public class UserLevelController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 用户等级

        //默认用户等级列表
        [CheckPermission]
        public ActionResult Index()
        {
            return View(work.UserLevelRepository.Get());
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult UserLevelAdd(int ID = 0)
        {
            if (ID != 0)
            {
                var role = work.UserLevelRepository.Get(g => g.ID == ID).FirstOrDefault<UserLevel>();
                return View(role);
            }
            return View();
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userLevelModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult UserLevelAdd(UserLevel userLevelModel, int ID = 0)
        {
            if (ModelState.IsValid)
            {
                if (userLevelModel.ID != 0)
                {
                    var oldModel = work.UserLevelRepository.Get(g => g.ID == ID).FirstOrDefault<UserLevel>();
                    var exsitModels = work.UserLevelRepository.Get(g => g.Level_Name == userLevelModel.Level_Name & g.ID != ID);
                    if (exsitModels.Count() > 0)
                    {
                        ModelState.AddModelError("Name", "等级名称已存在");
                    }
                    else
                    {
                        oldModel.Level_Name = userLevelModel.Level_Name;
                        oldModel.Level_Desc = userLevelModel.Level_Desc;
                        oldModel.Level_Discount_Percent = userLevelModel.Level_Discount_Percent;
                        oldModel.Level_Is_Enable = userLevelModel.Level_Is_Enable;
                        oldModel.Level_Is_Special = userLevelModel.Level_Is_Special;
                        oldModel.Level_Money_Begin = userLevelModel.Level_Money_Begin;
                        oldModel.Level_Money_End = userLevelModel.Level_Money_End;
                        oldModel.Level_Partner_Rebate_Percent = userLevelModel.Level_Partner_Rebate_Percent;
                        oldModel.Level_Shop_Rebate_Percent = userLevelModel.Level_Shop_Rebate_Percent;

                        work.UserLevelRepository.Update(oldModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                    }
                }
                else//新增
                {
                    var role = work.UserLevelRepository.Get(g => g.Level_Name == userLevelModel.Level_Name);
                    if (role.Count() > 0)
                    {
                        //return json("角色名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("Name", "等级名称已存在");
                    }
                    else
                    {
                        work.UserLevelRepository.Insert(userLevelModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
            }
            return View(userLevelModel);
        }

        /// <summary>
        /// 删除用户等级
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult UserLevelDelete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1 || ID == 2 || ID == 3 || ID == 4 || ID == 5)
                {
                    return RedirectToAction("Index");
                }
                var role = work.UserLevelRepository.Get(g => g.ID == ID).FirstOrDefault<UserLevel>();
                work.UserLevelRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}