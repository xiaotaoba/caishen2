using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;
using Pannet.Utility;

namespace Pannet.Web.Controllers
{
    public class DepartmentController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 科室

        //默认科室
        [CheckPermission]
        public ActionResult Index()
        {
            ViewBag.Parents = work.DepartmentRepository.Get(g => g.Dep_FollowID == 0);
            return View(work.DepartmentRepository.Get());
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Add(int ID = 0)
        {
            ViewBag.Parents = work.DepartmentRepository.Get(g => g.Dep_FollowID == 0);
            if (ID != 0)
            {
                var role = work.DepartmentRepository.Get(g => g.ID == ID).FirstOrDefault<Department>();
                return View(role);
            }
            return View(new Department());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult Add(Department newModel, int ID = 0)
        {
            ViewBag.Parents = work.DepartmentRepository.Get(g => g.Dep_FollowID == 0);

            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    var role = work.DepartmentRepository.Get(g => g.Dep_Name == newModel.Dep_Name);
                    if (role.Count() > 0)
                    {
                        //return json("科室名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("Name", "科室名称已存在");
                    }
                    else
                    {
                        work.DepartmentRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var oldGroup = work.DepartmentRepository.Get(g => g.ID == ID).FirstOrDefault<Department>();
                    var group = work.DepartmentRepository.Get(g => g.Dep_Name == newModel.Dep_Name & g.ID != ID);
                    if (group.Count() > 0)
                    {
                        //return json("科室名称已存在", jsonrequestbehavior.allowget);
                        ModelState.AddModelError("Name", "科室名称已存在");
                    }
                    else
                    {
                        oldGroup.Dep_Name = newModel.Dep_Name;
                        oldGroup.Dep_Desc = newModel.Dep_Desc;
                        oldGroup.Dep_Limit = newModel.Dep_Limit;
                        oldGroup.Dep_FollowID = newModel.Dep_FollowID;

                        work.DepartmentRepository.Update(oldGroup);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除科室
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1 || ID == 2 || ID == 3)
                //{
                //    return RedirectToAction("Index");
                //}
                var model = work.DepartmentRepository.Get(g => g.ID == ID).FirstOrDefault<Department>();
                if (model != null)
                {
                    work.DepartmentRepository.Delete(model);
                    work.Save();
                    work.Dispose();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}