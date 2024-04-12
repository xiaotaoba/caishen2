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

//using System.ComponentModel.DataAnnotations;

namespace Pannet.Web.Controllers
{
    public class ShippingController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 运送方式（公司）信息列表

        [CheckPermission]
        public ActionResult Index(string field = "name", string keyword = "", int page = 1)
        {
            ViewBag.Field = field;
            ViewBag.Keyword = keyword;

            var rst = work.Context.ShippingCompanys.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                switch (field)
                {
                    case "name": rst = rst.Where(m => m.SC_Name.Contains(keyword)); break;
                    default: break;
                };
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 新增运送公司信息

        /// <summary>
        /// 新增运送公司信息
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add()
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);

            return View(new ShippingCompany());
        }

        /// <summary>
        ///  新增运送公司信息-post
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult Add(ShippingCompany newModel)
        {
            if (ModelState.IsValid)
            {
                var existModel = work.ShippingCompanyRepository.Get(m => m.SC_Name == newModel.SC_Name && m.UserID == 0);
                if (existModel.Count() > 0)
                {
                    ModelState.AddModelError("SC_Name", string.Format("运送公司名称{0}已存在！", newModel.SC_Name));
                }
                else
                {
                    work.ShippingCompanyRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();
                }
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }

            return View(newModel);
        }

        #endregion

        #region 编辑运送公司信息

        /// <summary>
        /// 资料编辑
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Edit(int ID = 0)
        {
            ViewBag.ID = ID;
            if (ID != 0)
            {
                ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
                ShippingCompany oldModel = work.ShippingCompanyRepository.Get(m => m.ID == ID).FirstOrDefault<ShippingCompany>();
                return View(oldModel);
            }
            else
            {
                return RedirectToAction("Add");
            }
        }

        /// <summary>
        /// 资料编辑-post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult Edit(ShippingCompany newModel)
        {
            if (ModelState.IsValid)
            {
                ShippingCompany oldModel = work.Context.ShippingCompanys.AsNoTracking().Where(m => m.ID == newModel.ID).FirstOrDefault<ShippingCompany>();

                if (oldModel.SC_Name != newModel.SC_Name)//修改运送公司名称
                {
                    var existModel = work.ShippingCompanyRepository.Get(m => m.SC_Name == newModel.SC_Name);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("SC_Name", string.Format("运送公司名称{0}已存在！", newModel.SC_Name));
                        return View(newModel);
                    }
                }

                oldModel = newModel;

                work.ShippingCompanyRepository.Update(oldModel);

                work.Save();
                work.Dispose();

                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            //return JavaScript("alert('修改成功!');");

            return View(newModel);
        }

        #endregion

        #region 删除运送公司
        /// <summary>
        /// 删除运送公司
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var model = work.ShippingCompanyRepository.Get(m => m.ID == ID).FirstOrDefault<ShippingCompany>();
                work.ShippingCompanyRepository.Delete(model);
                work.Save();
                work.Dispose();
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 运送配送区域+费用计价

        /// <summary>
        /// 运送配送区域
        /// </summary>
        /// <param name="ShippingTemplateID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ShippingArea(int ShippingTemplateID = 0, int page = 1)
        {
            ViewBag.ShippingTemplateID = ShippingTemplateID;
            ViewBag.ShippingTemplate = work.ShippingTemplateRepository.Get(m => m.ID == ShippingTemplateID).FirstOrDefault();

            var rst = work.Context.ShippingAreas.AsQueryable();
            if (ShippingTemplateID != 0)
            {
                rst = rst.Where(m => m.ShippingTemplateID == ShippingTemplateID);
            }
            else {
                return RedirectToAction("ShippingTemplate");
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 新增、编辑配送区域
        /// </summary>
        /// <param name="ShippingCompanyID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ShippingAreaAdd(int ShippingTemplateID = 0, int ID = 0)
        {
            if (ShippingTemplateID == 0)
            {
                return RedirectToAction("ShippingTemplate");
            }
            ViewBag.ShippingTemplate = work.ShippingTemplateRepository.Get(m => m.ID == ShippingTemplateID).FirstOrDefault();
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);
            ViewBag.ShippingCompanys = work.ShippingCompanyRepository.Get().OrderByDescending(m => m.SC_Sort).ToList();
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);

            if (ID == 0)
            {
                return View(new ShippingArea());
            }
            else
            {
                ShippingArea model = work.ShippingAreaRepository.GetByID(ID);
                if (model != null)
                {
                    //ViewBag.Shipping = work.ShippingCompanyRepository.Get(m => m.ID == model.ShippingCompanyID).FirstOrDefault();
                    List<string> areaidsArr = model.AreaIds.Split(',').AsQueryable().Where(m => m != "").ToList();
                    ViewBag.Areas = work.Context.Areas.Where(m => areaidsArr.Contains(m.ID.ToString())).ToList();
                }

                return View(model);
            }
        }

        /// <summary>
        /// 设置运送地区-POST
        /// </summary>
        /// <param name="ShippingCompanyID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult ShippingAreaAdd(ShippingArea newModel, int ID = 0)
        {
            if (newModel.ShippingTemplateID == 0)
            {
                return RedirectToAction("ShippingTemplate");
            }
            ViewBag.ShippingTemplate = work.ShippingTemplateRepository.Get(m => m.ID == newModel.ShippingTemplateID).FirstOrDefault();
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            //ViewBag.Shipping = work.ShippingCompanyRepository.Get(m => m.ID == newModel.ShippingCompanyID).FirstOrDefault();
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ShippingCompanys = work.ShippingCompanyRepository.Get().OrderByDescending(m => m.SC_Sort).ToList();

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(newModel.SA_Title))
                {
                    ModelState.AddModelError("SA_Title", string.Format("名称不能为空！"));
                    return View(newModel);
                }

                if (ID == 0)
                {
                    work.ShippingAreaRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                }
                else
                {
                    work.ShippingAreaRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
            }

            return RedirectToAction("ShippingArea", new { ShippingTemplateID = newModel.ShippingTemplateID });
            //return View(newModel);
        }

        #endregion

        #region 删除运送区域
        /// <summary>
        /// 删除运送区域
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult DeleteShippingArea(int ShippingCompanyID = 0, int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var model = work.ShippingAreaRepository.Get(m => m.ID == ID).FirstOrDefault<ShippingArea>();
                work.ShippingAreaRepository.Delete(model);
                work.Save();
                work.Dispose();
            }
            return RedirectToAction("ShippingArea", new { ShippingCompanyID });
        }

        #endregion

        #region 运费模板

        /// <summary>
        /// 运费模板
        /// </summary>
        /// <param name="ShippingCompanyID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ShippingTemplate(int ShippingCompanyID = 0, int page = 1)
        {
            ViewBag.ShippingCompanyID = ShippingCompanyID;
            ViewBag.Shipping = work.ShippingCompanyRepository.Get(m => m.ID == ShippingCompanyID).FirstOrDefault();

            var rst = work.Context.ShippingTemplates.AsQueryable();
            //if (ShippingCompanyID != 0)
            //{
            //    rst = rst.Where(m => m.ShippingCompanyID == ShippingCompanyID);
            //}
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 新增、编辑运费模板
        /// </summary>
        /// <param name="ShippingCompanyID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult ShippingTemplateAdd(int ID = 0)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ShippingCompanys = work.ShippingCompanyRepository.Get().OrderByDescending(m => m.SC_Sort).ToList();

            if (ID == 0)
            {
                return View(new ShippingTemplate());
            }
            else
            {
                ShippingTemplate model = work.ShippingTemplateRepository.GetByID(ID);
                if (model != null)
                {
                    //ViewBag.Shipping = work.ShippingCompanyRepository.Get(m => m.ID == model.ShippingCompanyID).FirstOrDefault();
                    //List<string> areaidsArr = model.AreaIds.Split(',').AsQueryable().Where(m => m != "").ToList();
                    //ViewBag.Areas = work.Context.Areas.Where(m => areaidsArr.Contains(m.ID.ToString())).ToList();
                }

                return View(model);
            }
        }

        /// <summary>
        /// 运费模板-POST
        /// </summary>
        /// <param name="ShippingCompanyID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult ShippingTemplateAdd(ShippingTemplate newModel, int ID = 0)
        {
            //  ViewBag.Shipping = work.ShippingCompanyRepository.Get(m => m.ID == newModel.ShippingCompanyID).FirstOrDefault();
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ShippingCompanys = work.ShippingCompanyRepository.Get().OrderByDescending(m => m.SC_Sort).ToList();

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(newModel.ST_Title))
                {
                    ModelState.AddModelError("ST_Title", string.Format("名称不能为空！"));
                    return View(newModel);
                }

                if (ID == 0)
                {
                    work.ShippingTemplateRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                }
                else
                {
                    work.ShippingTemplateRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
            }

            return RedirectToAction("ShippingTemplate");
            //return View(newModel);
        }

        #endregion

        #region 删除运送模板
        /// <summary>
        /// 删除运送模板
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult DeleteShippingTemplate(int ShippingCompanyID = 0, int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var model = work.ShippingTemplateRepository.Get(m => m.ID == ID).FirstOrDefault<ShippingTemplate>();
                work.ShippingTemplateRepository.Delete(model);
                work.Save();
                work.Dispose();
            }
            return RedirectToAction("ShippingTemplate", new { ShippingCompanyID });
        }

        #endregion

        #region 获取运送公司 实体JSON

        /// <summary>
        /// 获取运送公司 实体JSON
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetShippingCompanyJson(int ID)
        {
            ShippingCompany model = work.ShippingCompanyRepository.Get(m => m.ID == ID).FirstOrDefault();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}