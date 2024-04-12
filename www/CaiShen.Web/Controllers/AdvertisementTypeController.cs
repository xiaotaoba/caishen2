using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;using Pannet.Utility;
using PagedList;

namespace Pannet.Web.Controllers
{
    public class AdvertisementTypeController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 广告类型

        //默认广告类型列表
        [CheckPermission]
        public ActionResult Index(string keyword = "", int page = 1)
        {
            ViewBag.keyword = keyword;

            var rst = work.Context.AdvertisementTypes.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                rst = rst.Where(m => m.ADT_Name.Contains(keyword));
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Add(int ID = 0)
        {
            ViewBag.ID = ID;

            if (ID != 0)
            {
                var model = work.AdvertisementTypeRepository.Get(m => m.ID == ID).FirstOrDefault<AdvertisementType>();
                return View(model);
            }
            return View(new AdvertisementType());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="userRoleModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        [HttpPost]
        public ActionResult Add(AdvertisementType newModel, int ID = 0)
        {
            ViewBag.ID = ID;
            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    var existModel = work.AdvertisementTypeRepository.Get(m => m.ADT_Name == newModel.ADT_Name);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("ADT_Name", "广告类型名称已存在");
                    }
                    else
                    {
                        work.AdvertisementTypeRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var existModel = work.AdvertisementTypeRepository.Get(m => m.ADT_Name == newModel.ADT_Name & m.ID != ID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("ADT_Name", "广告类型名称已存在");
                    }
                    else
                    {
                        work.AdvertisementTypeRepository.Update(newModel);
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
        /// 删除广告类型
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var role = work.AdvertisementTypeRepository.Get(m => m.ID == ID).FirstOrDefault<AdvertisementType>();
                work.AdvertisementTypeRepository.Delete(role);
                work.Save();
                work.Dispose();

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 获取分类JSON

        /// <summary>
        /// 获取分类实体 Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetTypeModelJson(int id)
        {
            AdvertisementType model = work.AdvertisementTypeRepository.Get(m => m.ID == id).FirstOrDefault();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}