using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Models;
using Pannet.DAL;using Pannet.Utility;
using PagedList;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using Pannet.Web.Attribute;
using Pannet.DAL.Repository;

//using System.ComponentModel.DataAnnotations;

namespace Pannet.Web.Controllers
{
    public class WarehouseController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();

        #region 仓库信息列表

        [CheckPermission]
        public ActionResult Index(string field = "name", string keyword = "", int page = 1)
        {
            ViewBag.Field = field;
            ViewBag.Keyword = keyword;

            var rst = work.Context.Warehouses.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                switch (field)
                {
                    case "name": rst = rst.Where(m => m.Name.Contains(keyword)); break;
                    case "number": rst = rst.Where(m => m.Number.Contains(keyword)); break;
                    case "username": rst = rst.Where(m => m.UserName.Contains(keyword)); break;
                    case "tel": rst = rst.Where(m => m.Tel.Contains(keyword)); break;
                    default: break;
                };
            }
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region 新增仓库信息

        /// <summary>
        /// 新增仓库信息
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Add()
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            return View(new Warehouse());
        }

        /// <summary>
        ///  新增仓库信息-post
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission]
        public ActionResult Add(Warehouse newModel)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            if (ModelState.IsValid)
            {
                var existModel = work.WarehouseRepository.Get(m => m.Name == newModel.Name);
                if (existModel.Count() > 0)
                {
                    ModelState.AddModelError("Name", string.Format("仓库名称{0}已存在！", newModel.Name));
                }
                else
                {
                    var existModel2 = work.WarehouseRepository.Get(m => m.Number == newModel.Number);
                    if (existModel2.Count() > 0)
                    {
                        ModelState.AddModelError("Number", string.Format("仓库编号{0}已存在！", newModel.Number));
                    }
                    else
                    {
                        //newModel.CreateTime = DateTime.Now;

                        work.WarehouseRepository.Insert(newModel);

                        work.Save();
                        work.Dispose();
                        LogService.Add(ManagerService.GetLoginModel(), "新增仓库：" + newModel.Name, newModel.ID.ToString());
                    }
                }
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }

            return View(newModel);
        }

        #endregion

        #region 编辑仓库信息

        /// <summary>
        /// 资料编辑
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult Edit(int ID = 0)
        {
            ViewBag.ID = ID;
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            if (ID != 0)
            {

                ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
                Warehouse oldModel = work.WarehouseRepository.Get(m => m.ID == ID).FirstOrDefault<Warehouse>();
                if (oldModel != null)
                {
                    ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.Province);
                    ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.City);
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
        public ActionResult Edit(Warehouse newModel)
        {
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            ViewBag.ID = newModel.ID;
            ViewBag.ShippingTemplates = work.ShippingTemplateRepository.Get().OrderByDescending(m => m.ST_Sort);

            if (ModelState.IsValid)
            {
                //Warehouse oldModel = work.WarehouseRepository.Get(m => m.ID == newModel.ID).FirstOrDefault<Warehouse>();
                Warehouse oldModel = work.Context.Warehouses.AsNoTracking().Where(m => m.ID == newModel.ID).FirstOrDefault<Warehouse>();
                if (oldModel != null)
                {
                    ViewBag.Citys = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.Province);
                    ViewBag.Regions = work.AreaRepository.Get(m => m.Area_ParentID == oldModel.City);
                }


                if (oldModel.Name != newModel.Name)//修改仓库名称
                {
                    var existModel = work.WarehouseRepository.Get(m => m.Name == newModel.Name);
                    if (existModel.Count() > 0)
                    {
                        ModelState.AddModelError("Name", string.Format("仓库名称{0}已存在！", newModel.Name));
                        return View(newModel);
                    }
                    else
                    {
                        oldModel.Name = newModel.Name;
                    }
                }
                if (oldModel.Number != newModel.Number)//修改仓库编号
                {
                    var existUser = work.WarehouseRepository.Get(m => m.Number == newModel.Number);
                    if (existUser.Count() > 0)
                    {
                        ModelState.AddModelError("Number", string.Format("仓库编号{0}已存在！", newModel.Number));
                        return View(newModel);
                    }
                    //else
                    //{
                    //    oldModel.Number = newModel.Number;
                    //}
                }
                //oldModel.Address = newModel.Address;
                //oldModel.City = newModel.City;
                ////oldModel.CreateTime = newModel.CreateTime;
                //oldModel.Desc = newModel.Desc;
                //oldModel.Is_Enable = newModel.Is_Enable;
                //oldModel.Province = newModel.Province;
                //oldModel.Region = newModel.Region;
                //oldModel.Tel = newModel.Tel;
                //oldModel.URL = newModel.URL;
                //oldModel.UserName = newModel.UserName;


                work.WarehouseRepository.Update(newModel);

                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "编辑仓库：" + oldModel.Name, newModel.ID.ToString());
                ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
            }
            //return JavaScript("alert('修改成功!');");

            return View(newModel);
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除仓库
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
                var model = work.WarehouseRepository.Get(m => m.ID == ID).FirstOrDefault<Warehouse>();
                work.WarehouseRepository.Delete(model);
                work.Save();
                work.Dispose();

                LogService.Add(ManagerService.GetLoginModel(), "删除仓库" , ID.ToString());
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region 仓库配送区域

        /// <summary>
        /// 仓库配送区域列表
        /// </summary>
        /// <param name="WarehouseID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult WarehouseArea(int WarehouseID, int page = 1)
        {
            ViewBag.WarehouseID = WarehouseID;
            ViewBag.Warehouse = work.WarehouseRepository.Get(m => m.ID == WarehouseID).FirstOrDefault();

            var rst = work.Context.WarehouseAreas.Where(m => m.WarehouseID == WarehouseID);
            rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 12;
            int pageNumber = page;
            return View(rst.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// 设置配送地区
        /// </summary>
        /// <param name="WarehouseID"></param>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult SetArea(int WarehouseID = 0, int ID = 0)
        {
            ViewBag.WarehouseID = WarehouseID;
            ViewBag.Warehouse = work.WarehouseRepository.Get(m => m.ID == WarehouseID).FirstOrDefault();
            ViewBag.Provinces = work.AreaRepository.Get(m => m.Area_ParentID == 1);
            

            if (ID == 0)
            {
                return View(new WarehouseArea());
            }
            else
            {
                WarehouseArea model = work.WarehouseAreaRepository.GetByID(ID);
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
        /// <param name="WarehouseID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult SetArea(WarehouseArea newModel, int WarehouseID = 0, int ID = 0)
        {
            ViewBag.WarehouseID = WarehouseID;
            ViewBag.Warehouse = work.WarehouseRepository.Get(m => m.ID == WarehouseID).FirstOrDefault();
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
                    work.WarehouseAreaRepository.Insert(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

                }
                else
                {
                    work.WarehouseAreaRepository.Update(newModel);
                    work.Save();
                    work.Dispose();

                    ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");
                }
            }

            return RedirectToAction("WarehouseArea", new { WarehouseID });
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
        public ActionResult DeleteWarehouseArea(int WarehouseID = 0, int ID = 0)
        {
            if (ID != 0)
            {
                if (ID == 1)
                {
                    return RedirectToAction("Index");
                }
                var model = work.WarehouseAreaRepository.Get(m => m.ID == ID).FirstOrDefault<WarehouseArea>();
                work.WarehouseAreaRepository.Delete(model);
                work.Save();
                work.Dispose();
            }
            return RedirectToAction("WarehouseArea", new { WarehouseID });
        }

        #endregion

        #region 仓库-商品列表

        /// <summary>
        /// 仓库-商品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseGoods(int WarehouseID, string field = "name", string keyword = "", int GoodsCategoryID = 0, int page = 1)
        {
            //当前仓库
            Warehouse WarehouseModel = work.WarehouseRepository.Get(m => m.ID == WarehouseID).FirstOrDefault<Warehouse>();
            ViewBag.Warehouse = WarehouseModel;
            ViewBag.WarehouseID = WarehouseID;

            if (WarehouseModel != null)
            {
                ViewBag.Field = field;
                ViewBag.Keyword = keyword;
                ViewBag.GoodsCategorys = work.GoodsCategoryRepository.Get();
                ViewBag.GoodsCategoryID = GoodsCategoryID;

                var rst = work.Context.WarehouseGoods
                    .Join(work.Context.Goods, sg => sg.GoodsID, g => g.ID, (sg, g) => new { sg, g })
                    .Where(m => m.sg.WarehouseID == WarehouseModel.ID);
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
        /// 仓库-不存储商品列表（可加入清单）
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseGoodsOffSaleList(int WarehouseID, string field = "name", string keyword = "", int GoodsCategoryID = 0, int page = 1)
        {
            //当前仓库
            Warehouse WarehouseModel = work.WarehouseRepository.Get(m => m.ID == WarehouseID).FirstOrDefault<Warehouse>();
            ViewBag.Warehouse = WarehouseModel;
            ViewBag.WarehouseID = WarehouseID;

            if (WarehouseModel != null)
            {
                ViewBag.Field = field;
                ViewBag.Keyword = keyword;
                ViewBag.GoodsCategorys = work.GoodsCategoryRepository.Get();
                ViewBag.GoodsCategoryID = GoodsCategoryID;

                var rst = work.Context.Goods.Where(m => m.UserShopID == 0);
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

                //商品ID
                List<int> rst_Warehouse_goodsids = work.Context.WarehouseGoods.Where(m => m.WarehouseID == WarehouseModel.ID).Select(m => m.GoodsID).ToList();

                List<Goods> goodsList = rst.Where(m => !rst_Warehouse_goodsids.Contains(m.ID)).OrderByDescending(m => m.ID).ToList();

                ViewBag.TotalCount = goodsList.Count();

                int pageSize = 20;
                int pageNumber = page;

                return View(goodsList.ToPagedList(pageNumber, pageSize));
            }

            return View();
        }

        #region 加入

        /// <summary>
        ///加入商品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseGoodsAdd(int WarehouseID, int goodsID = 0)
        {
            //当前仓库
            Warehouse WarehouseModel = work.WarehouseRepository.Get(m => m.ID == WarehouseID).FirstOrDefault<Warehouse>();
            ViewBag.Warehouse = WarehouseModel;
            ViewBag.WarehouseID = WarehouseID;

            if (WarehouseModel != null)
            {
                WarehouseGoods model = new WarehouseGoods();
                model.GoodsID = goodsID;
                model.WarehouseID = WarehouseModel.ID;

                work.Context.WarehouseGoods.AddOrUpdate(m => new { m.WarehouseID, m.GoodsID }, model);
                work.Save();
            }
            return RedirectToAction("WarehouseGoodsOffSaleList", new { WarehouseID });
        }
        /// <summary>
        /// 批量-加入商品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseGoodsAddBatch(int WarehouseID)
        {
            string ids = Request.Form["ids"];
            if (!string.IsNullOrEmpty(ids))
            {
                //当前仓库
                Warehouse WarehouseModel = work.WarehouseRepository.Get(m => m.ID == WarehouseID).FirstOrDefault<Warehouse>();
                ViewBag.Warehouse = WarehouseModel;
                ViewBag.WarehouseID = WarehouseID;

                if (WarehouseModel != null)
                {
                    string[] arrGoodsIds = ids.Trim(',').Split(',');
                    WarehouseGoods model = new WarehouseGoods();
                    foreach (var _goodsID in arrGoodsIds)
                    {
                        if (!string.IsNullOrEmpty(_goodsID) && _goodsID != "0")
                        {
                            model.GoodsID = Convert.ToInt32(_goodsID);
                            model.WarehouseID = WarehouseModel.ID;

                            work.Context.WarehouseGoods.AddOrUpdate(m => new { m.WarehouseID, m.GoodsID }, model);
                            work.Save();
                        }
                    }
                    work.Dispose();
                }
            }

            return RedirectToAction("WarehouseGoodsOffSaleList", new { WarehouseID });
        }

        #endregion

        #region 下架(删除)

        /// <summary>
        /// 下架商品--（删除记录）
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseGoodsDelete(int WarehouseID, int goodsID = 0)
        {
            //当前仓库
            Warehouse WarehouseModel = work.WarehouseRepository.Get(m => m.ID == WarehouseID).FirstOrDefault<Warehouse>();
            ViewBag.Warehouse = WarehouseModel;
            ViewBag.WarehouseID = WarehouseID;

            if (WarehouseModel != null)
            {
                var model = work.WarehouseGoodsRepository.Get(m => m.WarehouseID == WarehouseModel.ID && m.GoodsID == goodsID).FirstOrDefault<WarehouseGoods>();
                work.WarehouseGoodsRepository.Delete(model);
                work.Save();
            }
            return RedirectToAction("WarehouseGoods", new { WarehouseID });
        }
        /// <summary>
        /// 下架商品--（删除记录）批量
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseGoodsDeleteBatch(int WarehouseID)
        {
            string ids = Request.Form["ids"];
            if (!string.IsNullOrEmpty(ids))
            {
                //当前仓库
                Warehouse WarehouseModel = work.WarehouseRepository.Get(m => m.ID == WarehouseID).FirstOrDefault<Warehouse>();
                ViewBag.Warehouse = WarehouseModel;
                ViewBag.WarehouseID = WarehouseID;

                if (WarehouseModel != null)
                {
                    string[] arrGoodsIds = ids.Trim(',').Split(',');
                    foreach (var _goodsID in arrGoodsIds)
                    {
                        if (!string.IsNullOrEmpty(_goodsID) && _goodsID != "0")
                        {
                            int _goodsID_Int = Convert.ToInt32(_goodsID);
                            var model = work.WarehouseGoodsRepository.Get(m => m.WarehouseID == WarehouseModel.ID && m.GoodsID == _goodsID_Int).FirstOrDefault<WarehouseGoods>();
                            work.WarehouseGoodsRepository.Delete(model);
                        }
                    }
                    work.Save();
                    work.Dispose();
                }
            }

            return RedirectToAction("WarehouseGoods", new { WarehouseID });
        }

        #endregion

        #endregion

        #region 菜单

        /// <summary>
        /// 菜单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        public PartialViewResult Menu(int WarehouseID = 0, string Action = "")
        {
            ViewBag.WarehouseID = WarehouseID;
            ViewBag.Action = Action;
            ViewBag.Warehouse = work.WarehouseRepository.GetByID(WarehouseID);

            return PartialView("PartialMenu");
        }


        #endregion
    }
}