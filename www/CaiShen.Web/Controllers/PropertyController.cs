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
using Pannet.DAL.Repository;

namespace Pannet.Web.Controllers
{
    public class PropertyController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 属性名称

        /// <summary>
        /// 默认属性列表
        /// </summary>
        /// <param name="GoodsTypeID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Index(int GoodsTypeID = 0, int page = 1)
        {
            //产品类型
            ViewBag.GoodsTypes = work.GoodsTypeRepository.Get().OrderBy(m => m.GT_Name);
            ViewBag.GoodsTypeID = GoodsTypeID;

            var rst = work.Context.Property.AsQueryable();
            if (GoodsTypeID != 0)
            {
                rst = rst.Where(m => m.GoodsTypeID == GoodsTypeID);
            }
            rst = rst.OrderBy(m => m.GoodsTypeID).ThenByDescending(m => m.Prop_Sort).ThenBy(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="GoodsTypeID">产品类型ID</param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add(int ID = 0, int GoodsTypeID = 0)
        {
            if (GoodsTypeID == 0)
            {
                //产品类型为0 ，跳转至产品类型页面
                return RedirectToAction("Index", "GoodsType");
            }
            //产品类型
            ViewBag.GoodsTypes = work.GoodsTypeRepository.Get(m => m.ID == GoodsTypeID).OrderBy(m => m.GT_Name).ToList();
            ViewBag.GoodsTypeID = GoodsTypeID;
            ViewBag.ID = ID;

            if (ID != 0)
            {
                var model = work.PropertyRepository.Get(m => m.ID == ID).FirstOrDefault<Property>();
                ViewBag.PropertyParents = work.PropertyRepository.Get(m => m.ID != ID & m.Prop_ParentID == 0 & m.GoodsTypeID == GoodsTypeID).OrderByDescending(m => m.Prop_Sort).ToList();

                return View(model);
            }
            else
            {
                ViewBag.PropertyParents = work.PropertyRepository.Get(m => m.Prop_ParentID == 0 & m.GoodsTypeID == GoodsTypeID).OrderByDescending(m => m.Prop_Sort).ToList();
            }
            return View(new Property());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Add(Property newModel, int ID = 0, int GoodsTypeID = 0)
        {
            if (GoodsTypeID == 0)
            {
                //产品类型为0 ，跳转至产品类型页面
                return RedirectToAction("Index", "GoodsType");
            }
            //产品类型
            List<GoodsType> goodsTypes = work.GoodsTypeRepository.Get(m => m.ID == GoodsTypeID).OrderBy(m => m.GT_Name).ToList();
            ViewBag.GoodsTypes = goodsTypes;
            ViewBag.GoodsTypeID = GoodsTypeID;
            ViewBag.ID = ID;
            if (goodsTypes.Count() > 0)
            {
                //是否存在数量属性
                int existCount = work.PropertyRepository.Get(m => m.GoodsTypeID == newModel.GoodsTypeID && m.Prop_IsNumber == 1 && m.ID != newModel.ID).Count();
                if (existCount > 0 && newModel.Prop_IsNumber == 1)
                {
                    ModelState.AddModelError("Prop_Name", "当前产品类型下已存在数量属性");
                    return View(newModel);
                }

            }

            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    ViewBag.PropertyParents = work.PropertyRepository.Get(m => m.Prop_ParentID == 0 & m.GoodsTypeID == GoodsTypeID).OrderByDescending(m => m.Prop_Sort).ToList();
                    var role = work.PropertyRepository.Get(m => m.Prop_Name == newModel.Prop_Name & m.GoodsTypeID == newModel.GoodsTypeID);
                    if (role.Count() > 0)
                    {
                        ModelState.AddModelError("Prop_Name", "所选类型属性名称已存在");
                    }
                    else
                    {
                        work.PropertyRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        LogService.Add(ManagerService.GetLoginModel(), "新增产品属性:" + newModel.Prop_Name, newModel.ID.ToString());

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    ViewBag.PropertyParents = work.PropertyRepository.Get(m => m.ID != ID & m.Prop_ParentID == 0 & m.GoodsTypeID == GoodsTypeID).OrderByDescending(m => m.Prop_Sort).ToList();
                    //var oldModel = work.PropertyRepository.Get(m => m.ID == ID).FirstOrDefault<Property>();
                    var existModel = work.PropertyRepository.Get(m => m.Prop_Name == newModel.Prop_Name & m.ID != ID & m.GoodsTypeID == newModel.GoodsTypeID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("Prop_Name", "所选类型属性名称已存在");
                    }
                    else
                    {
                        //oldModel.GoodsTypeID = newModel.GoodsTypeID;
                        //oldModel.Prop_IsColor = newModel.Prop_IsColor;
                        //oldModel.Prop_IsEnable = newModel.Prop_IsEnable;
                        //oldModel.Prop_IsHasSon = newModel.Prop_IsHasSon;
                        //oldModel.Prop_IsNumber = newModel.Prop_IsNumber;
                        //oldModel.Prop_IsPrice = newModel.Prop_IsPrice;
                        //oldModel.Prop_IsSale = newModel.Prop_IsSale;
                        //oldModel.Prop_Name = newModel.Prop_Name;
                        //oldModel.Prop_ParentID = newModel.Prop_ParentID;
                        //oldModel.Prop_ShowType = newModel.Prop_ShowType;
                        //oldModel.Prop_Sort = newModel.Prop_Sort;

                        work.PropertyRepository.Update(newModel);
                        work.Save();
                        work.Dispose();

                        LogService.Add(ManagerService.GetLoginModel(), "编辑产品属性:" + newModel.Prop_Name, newModel.ID.ToString());


                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        return RedirectToAction("Index", new { GoodsTypeID = GoodsTypeID });
                    }
                }
            }
            return View(newModel);
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Delete(int ID = 0, int GoodsTypeID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1 || ID == 2 || ID == 3)
                //{
                //    return RedirectToAction("Index");
                //}
                var role = work.PropertyRepository.Get(m => m.ID == ID).FirstOrDefault<Property>();
                work.PropertyRepository.Delete(role);
                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "删除产品属性", ID.ToString());

            }
            return RedirectToAction("Index", new { GoodsTypeID = GoodsTypeID });
        }

        #endregion

        #region 属性值

        /// <summary>
        /// 属性值列表
        /// </summary>
        /// <param name="PropertyID"></param>
        /// <param name="GoodsTypeID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult PropertyValueList(int PropertyID = 0, int GoodsTypeID = 0, int page = 1)
        {
            //产品属性名称
            ViewBag.Propertys = work.PropertyRepository.Get(m => m.ID == PropertyID);
            ViewBag.GoodsTypeID = GoodsTypeID;
            ViewBag.PropertyID = PropertyID;

            var rst = work.Context.PropertyValues.AsQueryable();
            if (GoodsTypeID != 0)
            {
                rst = rst.Where(m => m.PropertyID == PropertyID);
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 添加/编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="GoodsTypeID">产品类型ID</param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult PropertyValueAdd(int ID = 0, int PropertyID = 0, int GoodsTypeID = 0)
        {
            if (PropertyID == 0)
            {
                //产品属性名称ID为0 ，跳转至产品属性名称列表页面
                return RedirectToAction("Index", "Property", new { GoodsTypeID = GoodsTypeID });
            }
            //产品属性名称
            ViewBag.Propertys = work.PropertyRepository.Get(m => m.ID == PropertyID);
            ViewBag.GoodsTypeID = GoodsTypeID;
            ViewBag.PropertyID = PropertyID;
            ViewBag.ID = ID;

            if (ID != 0)
            {
                var model = work.PropertyValueRepository.Get(m => m.ID == ID).FirstOrDefault<PropertyValue>();
                return View(model);
            }

            return View(new PropertyValue());
        }

        /// <summary>
        /// 添加/编辑处理
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult PropertyValueAdd(PropertyValue newModel, int ID = 0, int PropertyID = 0, int GoodsTypeID = 0)
        {
            if (PropertyID == 0)
            {
                //产品属性名称ID为0 ，跳转至产品属性名称列表页面
                return RedirectToAction("Index", "Property", new { GoodsTypeID = GoodsTypeID });
            }
            //产品属性名称
            ViewBag.Propertys = work.PropertyRepository.Get(m => m.ID == PropertyID);
            ViewBag.GoodsTypeID = GoodsTypeID;
            ViewBag.PropertyID = PropertyID;
            ViewBag.ID = ID;

            if (ModelState.IsValid)
            {
                if (ID == 0)//新增
                {
                    var existModel = work.PropertyValueRepository.Get(m => m.PV_Name == newModel.PV_Name & m.PropertyID == newModel.PropertyID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("PV_Name", "当前属性名称已存在相同属性值");
                    }
                    else
                    {
                        work.PropertyValueRepository.Insert(newModel);
                        work.Save();
                        work.Dispose();

                        LogService.Add(ManagerService.GetLoginModel(), "新增属性值：" + newModel.PV_Name, newModel.ID.ToString());
                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                    }
                }
                else
                {
                    var oldModel = work.PropertyValueRepository.Get(m => m.ID == ID).FirstOrDefault<PropertyValue>();
                    var existModel = work.PropertyValueRepository.Get(m => m.PV_Name == newModel.PV_Name & m.ID != ID & m.PropertyID == newModel.PropertyID);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("PV_Name", "当前属性名称已存在相同属性值");
                    }
                    else
                    {
                        oldModel.PropertyID = newModel.PropertyID;
                        oldModel.PV_ColorHEX = newModel.PV_ColorHEX;
                        oldModel.PV_ColorImage = newModel.PV_ColorImage;
                        oldModel.PV_Increment = newModel.PV_Increment;
                        oldModel.PV_IsEnable = newModel.PV_IsEnable;
                        oldModel.PV_IsFile = newModel.PV_IsFile;
                        oldModel.PV_Max = newModel.PV_Max;
                        oldModel.PV_Min = newModel.PV_Min;
                        oldModel.PV_Multiple = newModel.PV_Multiple;
                        oldModel.PV_Name = newModel.PV_Name;
                        oldModel.PV_Price = newModel.PV_Price;
                        oldModel.PV_ShowType = newModel.PV_ShowType;
                        oldModel.PV_Sort = newModel.PV_Sort;
                        oldModel.PV_Unit = newModel.PV_Unit;

                        work.PropertyValueRepository.Update(oldModel);
                        work.Save();
                        work.Dispose();

                        ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                        LogService.Add(ManagerService.GetLoginModel(), "编辑属性值:" + oldModel.PV_Name, oldModel.ID.ToString());
                        return RedirectToAction("PropertyValueList", new { GoodsTypeID = GoodsTypeID, PropertyID = PropertyID });
                    }
                }
            }
            return RedirectToAction("PropertyValueList", new { GoodsTypeID = GoodsTypeID, PropertyID = PropertyID });
            // return View(newModel);
        }

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult PropertyValueDelete(int ID = 0, int PropertyID = 0, int GoodsTypeID = 0)
        {
            if (ID != 0)
            {
                //if (ID == 1 || ID == 2 || ID == 3)
                //{
                //    return RedirectToAction("Index");
                //}
                var role = work.PropertyValueRepository.Get(m => m.ID == ID).FirstOrDefault<PropertyValue>();
                work.PropertyValueRepository.Delete(role);
                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "删除属性值", ID.ToString());

            }
            return RedirectToAction("PropertyValueList", new { GoodsTypeID = GoodsTypeID, PropertyID = PropertyID });
        }

        #endregion

        #region 获取属性名称+属性值 JSON

        /// <summary>
        /// 获取属性 Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetPropertyJson(int typeid)
        {
            List<Property> listArea = work.PropertyRepository.Get(m => m.GoodsTypeID == typeid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取属性值 Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetPropertyValueJson(int propertyid)
        {
            List<PropertyValue> listArea = work.PropertyValueRepository.Get(m => m.PropertyID == propertyid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}