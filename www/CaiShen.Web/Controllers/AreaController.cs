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
    public class AreaController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 地区

        //默认列表
        [CheckPermission]
        public ActionResult Index()
        {
            return View(work.AreaRepository.Get());
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add(int ID = 0)
        {
            if (ID != 0)
            {
                var model = work.AreaRepository.Get(m => m.ID == ID).FirstOrDefault<Area>();
                return View(model);
            }
            return View(new Brand());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Add(Area newModel, int ID = 0)
        {
            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    var role = work.AreaRepository.Get(m => m.Area_Name == newModel.Area_Name);
                    if (role.Count() > 0)
                    {
                        ModelState.AddModelError("Area_Name", "名称已存在");
                    }
                    else
                    {
                        work.AreaRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var oldModel = work.AreaRepository.Get(m => m.ID == ID).FirstOrDefault<Area>();
                    var existModel = work.AreaRepository.Get(m => m.Area_Name == newModel.Area_Name & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("Area_Name", "名称已存在");
                    }
                    else
                    {
                        oldModel.Area_Kind = newModel.Area_Kind;
                        oldModel.Area_Letter = newModel.Area_Letter;
                        oldModel.Area_Name = newModel.Area_Name;
                        oldModel.Area_Order = newModel.Area_Order;
                        oldModel.Area_ParentID = newModel.Area_ParentID;
                        oldModel.Area_Type = newModel.Area_Type;

                        work.AreaRepository.Update(oldModel);
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
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                var role = work.AreaRepository.Get(m => m.ID == ID).FirstOrDefault<Area>();
                work.AreaRepository.Delete(role);
                work.Save();
                work.Dispose();
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region 获取省、市、区信息

        /// <summary>
        /// 获取Area Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetAreaJson(int parentid)
        {
            List<Area> listArea = work.AreaRepository.Get(m => m.Area_ParentID == parentid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}