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
//using System.ComponentModel.DataAnnotations;

namespace Pannet.Web.Controllers
{
    public class SupplierController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        private int SupplierRoleID =  Convert.ToInt16(DataConfig.RoleEnum.供应商);

        #region 供应商信息列表

        [CheckPermission]
        public ActionResult Index(string field = "name", string keyword = "", int page = 1)
        {
            ViewBag.Field = field;
            ViewBag.Keyword = keyword;

            var rst = work.Context.Suppliers.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                switch (field)
                {
                    case "name": rst = rst.Where(m => m.Sup_Name.Contains(keyword)); break;
                    case "number": rst = rst.Where(m => m.Sup_Number.Contains(keyword)); break;
                    case "username": rst = rst.Where(m => m.Sup_UserName.Contains(keyword)); break;
                    case "tel": rst = rst.Where(m => m.Sup_Tel.Contains(keyword)); break;
                    default: break;
                };
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 新增供应商信息

        /// <summary>
        /// 新增供应商信息
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add()
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.SupplierUser = work.UserRepository.Get(m => m.UserRoleID == SupplierRoleID);
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            return View(new Supplier());
        }

        /// <summary>
        ///  新增供应商信息-post
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult Add(Supplier newModel)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.SupplierUser = work.UserRepository.Get(m => m.UserRoleID == SupplierRoleID);
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            if (ModelState.IsValid)
            {
                var existModel = work.SupplierRepository.Get(m => m.Sup_Name == newModel.Sup_Name);
                if (existModel.Count() > 0)
                {
                    ModelState.AddModelError("Sup_Name", string.Format("供应商名称{0}已存在！", newModel.Sup_Name));
                }
                else
                {
                    var existModel2 = work.SupplierRepository.Get(m => m.Sup_Number == newModel.Sup_Number);
                    if (existModel2.Count() > 0)
                    {
                        ModelState.AddModelError("Sup_Number", string.Format("供应商编号{0}已存在！", newModel.Sup_Number));
                    }
                    else
                    {
                        //newModel.Sup_CreateTime = DateTime.Now;

                        work.SupplierRepository.Insert(newModel);

                        work.Save();
                        work.Dispose();
                        LogService.Add(ManagerService.GetLoginModel(), "新增供应商：" + newModel.Sup_Name, newModel.ID.ToString());
                    }
                }
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }

            return View(newModel);
        }

        #endregion

        #region 编辑供应商信息

        /// <summary>
        /// 资料编辑
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Edit(int ID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.SupplierUser = work.UserRepository.Get(m => m.UserRoleID == SupplierRoleID);
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            if (ID != 0)
            {

                ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
                Supplier oldModel = work.SupplierRepository.Get(m => m.ID == ID).FirstOrDefault<Supplier>();
                if (oldModel != null)
                {
                    ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.Sup_Province);
                    ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.Sup_City);
                }

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
        public ActionResult Edit(Supplier newModel)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ID = newModel.ID;
            ViewBag.SupplierUser = work.UserRepository.Get(m => m.UserRoleID == SupplierRoleID);
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            if (ModelState.IsValid)
            {
                //Supplier oldModel = work.SupplierRepository.Get(m => m.ID == newModel.ID).FirstOrDefault<Supplier>();
                Supplier oldModel = work.Context.Suppliers.AsNoTracking().Where(m => m.ID == newModel.ID).FirstOrDefault<Supplier>();
                if (oldModel != null)
                {
                    ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.Sup_Province);
                    ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.Sup_City);
                }


                if (oldModel.Sup_Name != newModel.Sup_Name)//修改供应商名称
                {
                    var existModel = work.SupplierRepository.Get(m => m.Sup_Name == newModel.Sup_Name);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("Sup_Name", string.Format("供应商名称{0}已存在！", newModel.Sup_Name));
                        return View(newModel);
                    }
                    else
                    {
                        oldModel.Sup_Name = newModel.Sup_Name;
                    }
                }
                if (oldModel.Sup_Number != newModel.Sup_Number)//修改供应商编号
                {
                    var existUser = work.SupplierRepository.Get(m => m.Sup_Number == newModel.Sup_Number);
                    if (existUser.Count() > 0)
                    {
                        ModelState.AddModelError("Sup_Number", string.Format("供应商编号{0}已存在！", newModel.Sup_Number));
                        return View(newModel);
                    }
                    //else
                    //{
                    //    oldModel.Sup_Number = newModel.Sup_Number;
                    //}
                }
                //oldModel.Sup_Address = newModel.Sup_Address;
                //oldModel.Sup_City = newModel.Sup_City;
                //oldModel.Sup_CloseReason = newModel.Sup_CloseReason;
                ////oldModel.Sup_CreateTime = newModel.Sup_CreateTime;
                //oldModel.Sup_Desc = newModel.Sup_Desc;
                //oldModel.Sup_Is_Enable = newModel.Sup_Is_Enable;
                //oldModel.Sup_Province = newModel.Sup_Province;
                //oldModel.Sup_Region = newModel.Sup_Region;
                //oldModel.Sup_Tel = newModel.Sup_Tel;
                //oldModel.Sup_URL = newModel.Sup_URL;
                //oldModel.Sup_UserName = newModel.Sup_UserName;
                //oldModel.UserID = newModel.UserID;
                //oldModel.Sup_SupplyGoodsWay = newModel.Sup_SupplyGoodsWay;


                work.SupplierRepository.Update(newModel);

                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "编辑供应商：" + newModel.Sup_Name, newModel.ID.ToString());
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            //return JavaScript("alert('修改成功!');");

            return View(newModel);
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除供应商
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
                var model = work.SupplierRepository.Get(m => m.ID == ID).FirstOrDefault<Supplier>();
                work.SupplierRepository.Delete(model);
                work.Save();
                work.Dispose();
                LogService.Add(ManagerService.GetLoginModel(), "删除供应商", ID.ToString());

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 供应商生产产品管理

        ////供应商生产产品首页
        //[CheckPermission]
        //public ActionResult SupplyGoods(string field = "name", string keyword = "", int page = 1)
        //{
        //    ViewBag.Field = field;
        //    ViewBag.Keyword = keyword;

        //    var rst = work.Context.Suppliers.AsQueryable();

        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        switch (field)
        //        {
        //            case "name": rst = rst.Where(m => m.Sup_Name.Contains(keyword)); break;
        //            case "number": rst = rst.Where(m => m.Sup_Number.Contains(keyword)); break;
        //            case "username": rst = rst.Where(m => m.Sup_UserName.Contains(keyword)); break;
        //            case "tel": rst = rst.Where(m => m.Sup_Tel.Contains(keyword)); break;
        //            default: break;
        //        };
        //    }
        //    rst = rst.OrderByDescending(m => m.ID);

        //    int pageSize = 12;
        //    int pageNumber = page;
        //    return View(rst.ToPagedList(pageNumber, pageSize));
        //}

        ////供应商生产产品分类列表
        //[CheckPermission]
        //public ActionResult SupplyGoodsCategory(int SupplierID = 0, int page = 1)
        //{
        //    ViewBag.SupplierID = SupplierID;
        //    ViewBag.Supplier = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault();

        //    var rst = work.Context.SupplierGoodsCategorys
        //        .Join(work.Context.GoodsCategorys, sgc => sgc.GoodsCategoryID, gc => gc.ID, (sgc, gc) => new { gc, sgc })
        //        .Where(m => m.sgc.SupplierID == SupplierID).Select(m => m.gc);

        //    rst = rst.OrderByDescending(m => m.ID);

        //    int pageSize = 20;
        //    int pageNumber = page;
        //    return View(rst.ToPagedList(pageNumber, pageSize));
        //}

        ////供应商生产自定义产品列表
        //[CheckPermission]
        //public ActionResult SupplyGoodsList(int SupplierID = 0, int page = 1)
        //{
        //    ViewBag.SupplierID = SupplierID;
        //    ViewBag.Supplier = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault();

        //    var rst = work.Context.SupplierGoods
        //        .Join(work.Context.Goods, sg => sg.GoodsID, g => g.ID, (gs, g) => new { gs, g })
        //        .Where(m => m.gs.SupplierID == SupplierID).Select(m => m.g);

        //    rst = rst.OrderByDescending(m => m.ID);

        //    int pageSize = 20;
        //    int pageNumber = page;
        //    return View(rst.ToPagedList(pageNumber, pageSize));
        //}

        #region 供应商-生产商品列表

        /// <summary>
        /// 供应商-生产商品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierGoods(int SupplierID, string field = "name", string keyword = "", int GoodsCategoryID = 0, int page = 1)
        {
            //当前供应商
            Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
            ViewBag.Supplier = supplierModel;
            ViewBag.SupplierID = SupplierID;

            if (supplierModel.Sup_SupplyGoodsWay == Convert.ToInt16(DataConfig.SupplyGoodsWayEnum.生产所有产品))
            {
                return View("SupplierGoodsAllTips");
            }
            else if (supplierModel.Sup_SupplyGoodsWay == Convert.ToInt16(DataConfig.SupplyGoodsWayEnum.生产指定分类产品))
            {
                return RedirectToAction("SupplierGoodsCategory");// View("SupplierGoodsCategory");
            }
            if (supplierModel != null)
            {
                ViewBag.Field = field;
                ViewBag.Keyword = keyword;
                ViewBag.GoodsCategorys = work.GoodsCategoryRepository.Get();
                ViewBag.GoodsCategoryID = GoodsCategoryID;

                var rst = work.Context.SupplierGoods
                    .Join(work.Context.Goods, sg => sg.GoodsID, g => g.ID, (sg, g) => new { sg, g })
                    .Where(m => m.sg.SupplierID == supplierModel.ID);
                if (GoodsCategoryID != 0)
                {
                    rst = rst.Where(m => m.g.GoodsCategoryID == GoodsCategoryID);
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    switch (field)
                    {
                        case "name": rst = rst.Where(m => m.g.G_Name.Contains(keyword)); break;
                        case "number": rst = rst.Where(m => m.g.G_Number.Contains(keyword)); break;
                        default: break;
                    };
                }

                List<Goods> goodsList = rst.Select(m => m.g).OrderByDescending(m => m.ID).ToList();
                ViewBag.TotalCountOnSale = goodsList.Count();

                int pageSize = 20;
                int pageNumber = page;
                return View(goodsList.ToPagedList(pageNumber, pageSize));
            }

            return View();
        }

        /// <summary>
        /// 供应商-未生产商品列表（可加入生产清单）
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierGoodsOffSaleList(int SupplierID, string field = "name", string keyword = "", int GoodsCategoryID = 0, int page = 1)
        {
            //当前供应商
            Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
            ViewBag.Supplier = supplierModel;
            ViewBag.SupplierID = SupplierID;

            if (supplierModel != null)
            {
                ViewBag.Field = field;
                ViewBag.Keyword = keyword;
                ViewBag.GoodsCategorys = work.GoodsCategoryRepository.Get();
                ViewBag.GoodsCategoryID = GoodsCategoryID;

                var rst = work.Context.Goods
                    //.Join(work.Context.UserShopGoodsDowns.DefaultIfEmpty(), g => g.ID, gd => gd.GoodsID, (g, gd) => new { gd, g }).DefaultIfEmpty();
                        .Where(m => m.UserShopID == 0);
                //.Where(m => (Int32?)m.gd.ShopID == );
                if (GoodsCategoryID != 0)
                {
                    rst = rst.Where(m => m.GoodsCategoryID == GoodsCategoryID);
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    switch (field)
                    {
                        case "name": rst = rst.Where(m => m.G_Name.Contains(keyword)); break;
                        case "number": rst = rst.Where(m => m.G_Number.Contains(keyword)); break;
                        default: break;
                    };
                }

                //生产商品ID
                List<int> rst_supplier_goodsids = work.Context.SupplierGoods.Where(m => m.SupplierID == supplierModel.ID).Select(m => m.GoodsID).ToList();

                List<Goods> goodsList = rst.Where(m => !rst_supplier_goodsids.Contains(m.ID)).OrderByDescending(m => m.ID).ToList();

                ViewBag.TotalCount = goodsList.Count();

                int pageSize = 20;
                int pageNumber = page;

                return View(goodsList.ToPagedList(pageNumber, pageSize));
            }

            return View();
        }

        #region 加入

        /// <summary>
        ///加入生产商品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierGoodsAdd(int SupplierID, int goodsID = 0)
        {
            //当前供应商
            Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
            ViewBag.Supplier = supplierModel;
            ViewBag.SupplierID = SupplierID;

            if (supplierModel != null)
            {
                SupplierGoods model = new SupplierGoods();
                model.GoodsID = goodsID;
                model.SupplierID = supplierModel.ID;

                work.Context.SupplierGoods.AddOrUpdate(m => new { m.SupplierID, m.GoodsID }, model);
                work.Save();
            }
            return RedirectToAction("SupplierGoodsOffSaleList", new { SupplierID });
        }
        /// <summary>
        /// 批量-加入生产商品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierGoodsAddBatch(int SupplierID)
        {
            string ids = Request.Form["ids"];
            if (!string.IsNullOrEmpty(ids))
            {
                //当前供应商
                Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
                ViewBag.Supplier = supplierModel;
                ViewBag.SupplierID = SupplierID;

                if (supplierModel != null)
                {
                    string[] arrGoodsIds = ids.Trim(',').Split(',');
                    SupplierGoods model = new SupplierGoods();
                    foreach (var _goodsID in arrGoodsIds)
                    {
                        if (!string.IsNullOrEmpty(_goodsID) && _goodsID != "0")
                        {
                            model.GoodsID = Convert.ToInt32(_goodsID);
                            model.SupplierID = supplierModel.ID;

                            work.Context.SupplierGoods.AddOrUpdate(m => new { m.SupplierID, m.GoodsID }, model);
                            work.Save();
                        }
                    }
                    work.Dispose();
                }
            }

            return RedirectToAction("SupplierGoodsOffSaleList", new { SupplierID });
        }

        #endregion

        #region 下架(删除)

        /// <summary>
        /// 下架生产商品--（删除生产记录）
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierGoodsDelete(int SupplierID, int goodsID = 0)
        {
            //当前供应商
            Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
            ViewBag.Supplier = supplierModel;
            ViewBag.SupplierID = SupplierID;

            if (supplierModel != null)
            {
                var model = work.SupplierGoodsRepository.Get(m => m.SupplierID == supplierModel.ID && m.GoodsID == goodsID).FirstOrDefault<SupplierGoods>();
                work.SupplierGoodsRepository.Delete(model);
                work.Save();
            }
            return RedirectToAction("SupplierGoods", new { SupplierID });
        }
        /// <summary>
        /// 下架生产商品--（删除生产记录）批量
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierGoodsDeleteBatch(int SupplierID)
        {
            string ids = Request.Form["ids"];
            if (!string.IsNullOrEmpty(ids))
            {
                //当前供应商
                Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
                ViewBag.Supplier = supplierModel;
                ViewBag.SupplierID = SupplierID;

                if (supplierModel != null)
                {
                    string[] arrGoodsIds = ids.Trim(',').Split(',');
                    foreach (var _goodsID in arrGoodsIds)
                    {
                        if (!string.IsNullOrEmpty(_goodsID) && _goodsID != "0")
                        {
                            int _goodsID_Int = Convert.ToInt32(_goodsID);
                            var model = work.SupplierGoodsRepository.Get(m => m.SupplierID == supplierModel.ID && m.GoodsID == _goodsID_Int).FirstOrDefault<SupplierGoods>();
                            work.SupplierGoodsRepository.Delete(model);
                        }
                    }
                    work.Save();
                    work.Dispose();
                }
            }

            return RedirectToAction("SupplierGoods", new { SupplierID });
        }

        #endregion

        #endregion

        #region 供应商-已选-生产商品分类列表
        /// <summary>
        /// 供应商-已选-生产商品分类列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierGoodsCategory(int SupplierID, int page = 1)
        {
            //当前供应商
            Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
            ViewBag.Supplier = supplierModel;
            ViewBag.SupplierID = SupplierID;

            var rst = work.Context.SupplierGoodsCategorys
                .Join(work.Context.GoodsCategorys, sgc => sgc.GoodsCategoryID, gc => gc.ID, (sgc, gc) => new { gc, sgc })
                .Where(m => m.sgc.SupplierID == supplierModel.ID).Select(m => m.gc);

            rst = rst.OrderBy(m => m.GC_ParentID);

            int pageSize = 20;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 供应商-设置-生产商品分类

        /// <summary>
        /// 供应商-设置-生产商品分类
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierGoodsCategorySet(int SupplierID)
        {
            //当前供应商
            Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
            ViewBag.Supplier = supplierModel;
            ViewBag.SupplierID = SupplierID;

            ViewBag.Categorys = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0).ToList();
            return View();
        }

        /// <summary>
        /// 供应商-设置-生产商品分类-save
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SupplierGoodsCategorySet(int SupplierID, string CataIds = "")
        {
            ViewBag.Categorys = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0).ToList();
            ViewBag.SupplierID = SupplierID;

            if (string.IsNullOrEmpty(CataIds))
            {
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("warning", "请选择商品分类");
            }
            else
            {
                //当前供应商
                Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
                ViewBag.Supplier = supplierModel;

                CataIds = CataIds.Trim(',');
                int[] cataIdsArr = UtilityClass.ConvertIntArr(CataIds);

                foreach (int itemid in cataIdsArr)
                {
                    SupplierGoodsCategory newModel = new SupplierGoodsCategory();
                    newModel.GoodsCategoryID = itemid;
                    newModel.Sort = 0;
                    newModel.SupplierID = supplierModel.ID;

                    work.Context.SupplierGoodsCategorys.AddOrUpdate(m => new { m.GoodsCategoryID, m.SupplierID }, newModel);
                    work.Save();
                }
                work.Dispose();

                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            return View();
        }

        #endregion

        #region 供应商-下架-生产商品分类

        /// <summary>
        /// 下架-生产商品分类 (删除生成商品分类记录)
        /// </summary>
        /// <returns></returns>
        public ActionResult SupplierGoodsCategoryDelete(int SupplierID, int GoodsCategoryID = 0)
        {
            //当前供应商
            Supplier supplierModel = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault<Supplier>();
            ViewBag.Supplier = supplierModel;
            ViewBag.SupplierID = SupplierID;

            if (supplierModel != null)
            {
                var model = work.SupplierGoodsCategoryRepository.Get(m => m.SupplierID == supplierModel.ID && m.GoodsCategoryID == GoodsCategoryID).FirstOrDefault<SupplierGoodsCategory>();
                work.SupplierGoodsCategoryRepository.Delete(model);
                work.Save();
            }
            return RedirectToAction("SupplierGoodsCategory", new { SupplierID });
        }

        #endregion

        #region 获取分类JSON

        /// <summary>
        /// 获取分类 Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCategoryJson(int parentid)
        {
            List<GoodsCategory> listArea = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == parentid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 供应商服务区域

        /// <summary>
        /// 供应商务区域列表
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult SupplierArea(int SupplierID, int page = 1)
        {
            ViewBag.SupplierID = SupplierID;
            ViewBag.Supplier = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault();

            var rst = work.Context.SupplierAreas.Where(m => m.SupplierID == SupplierID);
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 设置配送地区
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult SetArea(int SupplierID = 0, int ID = 0)
        {
            ViewBag.SupplierID = SupplierID;
            ViewBag.Supplier = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault();
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);


            if (ID == 0)
            {
                return View(new SupplierArea());
            }
            else
            {
                SupplierArea model = work.SupplierAreaRepository.GetByID(ID);
                if (model != null)
                {
                    List<string> areaidsArr = model.AreaIds.Split(',').AsQueryable().Where(m => m != "").ToList();
                    ViewBag.Areas = work.Context.Areas.Where(m => areaidsArr.Contains(m.ID.ToString())).ToList();
                }

                return View(model);
            }
        }

        /// <summary>
        /// 设置配送地区-POST
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult SetArea(SupplierArea newModel, int SupplierID = 0, int ID = 0)
        {
            ViewBag.SupplierID = SupplierID;
            ViewBag.Supplier = work.SupplierRepository.Get(m => m.ID == SupplierID).FirstOrDefault();
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(newModel.Title))
                {
                    ModelState.AddModelError("Title", string.Format("名称不能为空！"));
                    return View(newModel);
                }

                if (ID == 0)
                {
                    work.SupplierAreaRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                }
                else
                {
                    work.SupplierAreaRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
            }

            return RedirectToAction("SupplierArea", new { SupplierID });
            //return View(newModel);
        }

        #endregion

        #region 删除配送区域
        /// <summary>
        /// 删除配送区域
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult DeleteSupplierArea(int SupplierID = 0, int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var model = work.SupplierAreaRepository.Get(m => m.ID == ID).FirstOrDefault<SupplierArea>();
                work.SupplierAreaRepository.Delete(model);
                work.Save();
                work.Dispose();
            }
            return RedirectToAction("SupplierArea", new { SupplierID });
        }

        #endregion

        #region 菜单

        /// <summary>
        /// 菜单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public PartialViewResult Menu(int SupplierID = 0, string Action = "")
        {
            ViewBag.SupplierID = SupplierID;
            ViewBag.Action = Action;
            ViewBag.Supplier = work.SupplierRepository.GetByID(SupplierID);

            return PartialView("PartialMenu");
        }


        #endregion
    }
}