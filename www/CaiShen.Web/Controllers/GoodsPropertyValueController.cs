using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;
using Pannet.Utility;
using PagedList;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using EntityFramework.Extensions;  

namespace Pannet.Web.Controllers
{
    public class GoodsPropertyValueController : Controller
    {
        private UnitOfWork work = new UnitOfWork();

        #region 属性列表

        /// <summary>
        /// 默认属性列表
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Index()
        {
            ////产品类型
            //ViewBag.GoodsTypes = work.GoodsTypeRepository.Get();
            //ViewBag.GoodsTypeID = GoodsTypeID;

            //var rst = work.Context.Property.AsQueryable();
            //if (GoodsTypeID != 0)
            //{
            //    rst = rst.Where(m => m.GoodsTypeID == GoodsTypeID);
            //}
            //rst = rst.OrderByDescending(m => m.ID);

            //int pageSize = 12;
            //int pageNumber = page;
            //return View(rst.ToPagedList(pageNumber, pageSize));
            return View();
        }

        #endregion

        #region 添加/编辑处理

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="pvModel"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Add(PropertyValue pvModel, int GoodsID = 0)
        {
            JsonResult json = new JsonResult
            {
                Data = new
                {
                    status = "0",
                    msg = "添加成功！"
                }
            };
            if (ModelState.IsValid)
            {
                var existModel = work.GoodsPropertyValueRepository.Get(m => m.PropertyID == pvModel.PropertyID & m.PropertyValueID == pvModel.ID & m.GoodsID == GoodsID);
                if (existModel.Count() > 0)
                {
                    //更新
                    GoodsPropertyValue newModel = existModel.FirstOrDefault<GoodsPropertyValue>();
                    newModel.GoodsID = GoodsID;
                    newModel.GPV_ColorHEX = pvModel.PV_ColorHEX;
                    newModel.GPV_ColorImage = pvModel.PV_ColorImage;
                    //newModel.GPV_FileUrl = pvModel.
                    newModel.GPV_Increment = pvModel.PV_Increment;
                    newModel.GPV_IsSKU = 1;
                    newModel.GPV_Max = pvModel.PV_Max;
                    newModel.GPV_Min = pvModel.PV_Min;
                    newModel.GPV_Multiple = pvModel.PV_Multiple;
                    newModel.GPV_Price = pvModel.PV_Price;
                    newModel.GPV_SKU_ID = 0;
                    newModel.GPV_Unit = pvModel.PV_Unit;
                    newModel.PropertyID = pvModel.PropertyID;
                    newModel.PropertyValueID = pvModel.ID;

                    work.GoodsPropertyValueRepository.Update(newModel);
                    //work.Context.GoodsPropertyValues.AddOrUpdate(m => m.ManagerID, new ManagerWithGroup { ManagerGroupID = _model.GroupID, ManagerID = oldManager.ID });
                    work.Save();
                    work.Dispose();

                    json.Data = new { status = "0", msg = "已存在！" };
                    return Json(json);
                }
                else
                {
                    GoodsPropertyValue newModel = new GoodsPropertyValue();
                    newModel.GoodsID = GoodsID;
                    newModel.GPV_ColorHEX = pvModel.PV_ColorHEX;
                    newModel.GPV_ColorImage = pvModel.PV_ColorImage;
                    //newModel.GPV_FileUrl = pvModel.
                    newModel.GPV_Increment = pvModel.PV_Increment;
                    newModel.GPV_IsSKU = 1;
                    newModel.GPV_Max = pvModel.PV_Max;
                    newModel.GPV_Min = pvModel.PV_Min;
                    newModel.GPV_Multiple = pvModel.PV_Multiple;
                    newModel.GPV_Price = pvModel.PV_Price;
                    newModel.GPV_SKU_ID = 0;
                    newModel.GPV_Unit = pvModel.PV_Unit;
                    newModel.PropertyID = pvModel.PropertyID;
                    newModel.PropertyValueID = pvModel.ID;

                    work.GoodsPropertyValueRepository.Insert(newModel);
                    //work.Context.GoodsPropertyValues.AddOrUpdate(m => m.ManagerID, new ManagerWithGroup { ManagerGroupID = _model.GroupID, ManagerID = oldManager.ID });
                    work.Save();
                    work.Dispose();
                }

                //ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

            }
            //return View();

            return Json(json);
        }

        /// <summary>
        /// 添加/编辑处理 - 批量
        /// </summary>
        /// <param name="pvListJson"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult AddBatch(string pvListJson, int GoodsID = 0)
        {
            JsonResult json = new JsonResult
            {
                Data = new
                {
                    status = "0",
                    msg = "处理完成！"
                }
            };
            //if (ModelState.IsValid)
            //{

            List<PropertyValue> pvModelList = JsonHelper.DeserializeJsonToList<PropertyValue>(pvListJson);

            if (pvModelList != null && pvModelList.Count > 0)
            {
                var rstAll = work.Context.GoodsPropertyValues.Where(m => m.GoodsID == GoodsID).ToList();
                foreach (var pvModel in pvModelList)
                {
                    var existModel = rstAll.Where(m => m.PropertyID == pvModel.PropertyID & m.PropertyValueID == pvModel.ID).FirstOrDefault();
                    if (existModel != null)
                    {
                        //更新
                        GoodsPropertyValue newModel = existModel;
                        newModel.GoodsID = GoodsID;
                        newModel.GPV_ColorHEX = pvModel.PV_ColorHEX;
                        newModel.GPV_ColorImage = pvModel.PV_ColorImage;
                        //newModel.GPV_FileUrl = pvModel.
                        newModel.GPV_Increment = pvModel.PV_Increment;
                        newModel.GPV_IsSKU = 1;
                        newModel.GPV_Max = pvModel.PV_Max;
                        newModel.GPV_Min = pvModel.PV_Min;
                        newModel.GPV_Multiple = pvModel.PV_Multiple;
                        newModel.GPV_Price = pvModel.PV_Price;
                        newModel.GPV_SKU_ID = 0;
                        newModel.GPV_Unit = pvModel.PV_Unit;
                        newModel.PropertyID = pvModel.PropertyID;
                        newModel.PropertyValueID = pvModel.ID;

                        work.GoodsPropertyValueRepository.Update(newModel);

                        //json.Data = new { status = "0", msg = "已存在！" };
                        //return Json(json);
                    }
                    else
                    {
                        GoodsPropertyValue newModel = new GoodsPropertyValue();
                        newModel.GoodsID = GoodsID;
                        newModel.GPV_ColorHEX = pvModel.PV_ColorHEX;
                        newModel.GPV_ColorImage = pvModel.PV_ColorImage;
                        //newModel.GPV_FileUrl = pvModel.
                        newModel.GPV_Increment = pvModel.PV_Increment;
                        newModel.GPV_IsSKU = 1;
                        newModel.GPV_Max = pvModel.PV_Max;
                        newModel.GPV_Min = pvModel.PV_Min;
                        newModel.GPV_Multiple = pvModel.PV_Multiple;
                        newModel.GPV_Price = pvModel.PV_Price;
                        newModel.GPV_SKU_ID = 0;
                        newModel.GPV_Unit = pvModel.PV_Unit;
                        newModel.PropertyID = pvModel.PropertyID;
                        newModel.PropertyValueID = pvModel.ID;

                        work.GoodsPropertyValueRepository.Insert(newModel);
                    }
                }

                json.Data = new { status = "0", msg = "处理完成(" + pvModelList.Count() + ")！" };
                work.Save();
                work.Dispose();
            }
            //ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

            //}
            //return View();

            return Json(json);
        }

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0)
        {
            if (ID != 0)
            {
                var role = work.GoodsPropertyValueRepository.Get(m => m.ID == ID).FirstOrDefault<GoodsPropertyValue>();
                work.GoodsPropertyValueRepository.Delete(role);
                work.Save();
                work.Dispose();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Delete(PropertyValue pvModel, int GoodsID = 0)
        {
            JsonResult json = new JsonResult
            {
                Data = new
                {
                    status = "0",
                    msg = "删除成功！"
                }
            };
            List<GoodsPropertyValue> list = work.GoodsPropertyValueRepository.Get(m => m.PropertyID == pvModel.PropertyID & m.PropertyValueID == pvModel.ID & m.GoodsID == GoodsID).ToList();
            foreach (var item in list)
            {
                work.GoodsPropertyValueRepository.Delete(item);
            }
            work.Save();
            work.Dispose();

            return Json(json);
        }

        /// <summary>
        /// 删除属性值 - 批量
        /// </summary>
        /// <param name="pvListJson"></param>
        /// <param name="GoodsID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult DeleteBatch(string pvListJson, int GoodsID = 0)
        {
            JsonResult json = new JsonResult
            {
                Data = new
                {
                    status = "0",
                    msg = "批量删除成功！"
                }
            };
            List<PropertyValue> pvModelList = JsonHelper.DeserializeJsonToList<PropertyValue>(pvListJson);
            if (pvModelList != null && pvModelList.Count > 0)
            {
                //var rstAll = work.Context.GoodsPropertyValues.Where(m => m.GoodsID == GoodsID).ToList();
                //foreach (var pvModel in pvModelList)
                //{
                //    var existModel = rstAll.Where(m => m.PropertyID == pvModel.PropertyID & m.PropertyValueID == pvModel.ID).FirstOrDefault();
                //    if (existModel != null)
                //    {
                //        work.GoodsPropertyValueRepository.Delete(existModel);
                //    }
                //}
                foreach (var pvModel in pvModelList) {
                    work.Context.GoodsPropertyValues.Where(m => m.PropertyID == pvModel.PropertyID & m.PropertyValueID == pvModel.ID).Delete();
                }
                work.Save();
                work.Dispose();
                json.Data = new { status = "0", msg = "批量删除成功(" + pvModelList.Count() + ")！" };
            }
            return Json(json);
        }

        #endregion

        #region 获取产品属性值 JSON

        /// <summary>
        /// 获取属性值 Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetPropertyValueJson(int goodsid)
        {
            List<GoodsPropertyValue> list = work.GoodsPropertyValueRepository.Get(m => m.GoodsID == goodsid).ToList();
            //List<PropertyValue> listB = new List<PropertyValue>();
            //foreach (var item in list)
            //{
            //    listB.Add(new PropertyValue
            //    {
            //        PV_ColorHEX = item.GPV_ColorHEX,
            //        PV_ColorImage = item.GPV_ColorImage,
            //        PV_IsFile = 0,
            //        PV_Increment = item.GPV_Increment,
            //        PV_Max = item.GPV_Max,
            //        PV_Min = item.GPV_Min,
            //        PV_Multiple = item.GPV_Multiple,
            //        PV_Price = item.GPV_Price,
            //        PV_Unit = item.GPV_Unit,
            //        PropertyID = item.PropertyID,
            //        ID = item.PropertyValueID,
            //    });
            //}
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}