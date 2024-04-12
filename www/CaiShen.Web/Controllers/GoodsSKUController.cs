using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Web.Attribute;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Utility;
using PagedList;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using EntityFramework.Extensions;

namespace Pannet.Web.Controllers
{
    public class GoodsSKUController : Controller
    {
        private UnitOfWork work = new UnitOfWork();

        #region 产品SKU列表

        /// <summary>
        /// 默认列表
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
        public ActionResult Add(GoodsSKUVModel newModel)
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
                var existlist = work.GoodsSKURepository.Get(m => m.GoodsID == newModel.GoodsID & m.Properties == newModel.Properties);
                if (existlist.Count() > 0)
                {
                    //更新
                    GoodsSKU existModel = existlist.FirstOrDefault<GoodsSKU>();

                    existModel.PropertiesName = newModel.PropertiesName;
                    existModel.SKU_Count = newModel.Count;
                    existModel.SKU_GoodsCode = newModel.GoodsCode;
                    existModel.SKU_ShopCode = newModel.ShopCode;
              
                    existModel.SKU_CostPrice = newModel.CostPrice;
                    if (newModel.ShopPrice == 0)
                    {
                        existModel.SKU_ShopPrice = Convert.ToDecimal(newModel.ShopPriceRate) * newModel.CostPrice;
                    }
                    else
                    {
                        existModel.SKU_ShopPrice = newModel.ShopPrice;
                    }
                    if (newModel.Price == 0)
                    {
                        existModel.SKU_Price = Convert.ToDecimal(newModel.ClientPriceRate) * existModel.SKU_ShopPrice;
                    }
                    else
                    {
                        existModel.SKU_Price = newModel.Price;
                    }
                    existModel.GoodsID = newModel.GoodsID;
                    existModel.Properties = newModel.Properties;
                    existModel.SKU_Weight = newModel.Weight;
                    existModel.SKU_Volume = newModel.Volume;
                    existModel.SKU_DistributorPrice = newModel.DistributorPrice;
                    existModel.SKU_SquareWeight = newModel.SquareWeight;
                    existModel.SKU_ExpandArea = newModel.ExpandArea;
                    existModel.SKU_ClientPriceRate = newModel.ClientPriceRate;
                    existModel.SKU_ShopPriceRate = newModel.ShopPriceRate;

                    work.GoodsSKURepository.Update(existModel);

                    work.Save();
                    work.Dispose();

                    json.Data = new { status = "0", msg = "已存在！" };
                    return Json(json);
                }
                else
                {
                    GoodsSKU addModel = new GoodsSKU();

                    addModel.Properties = newModel.Properties;
                    addModel.PropertiesName = newModel.PropertiesName;
                    addModel.SKU_Count = newModel.Count;
                    addModel.SKU_GoodsCode = newModel.GoodsCode;
                    addModel.SKU_ShopCode = newModel.ShopCode;
                    addModel.SKU_CostPrice = newModel.CostPrice;
                    if (newModel.ShopPrice == 0)
                    {
                        addModel.SKU_ShopPrice = Convert.ToDecimal(newModel.ShopPriceRate) * newModel.CostPrice;
                    }
                    else
                    {
                        addModel.SKU_ShopPrice = newModel.ShopPrice;
                    }
                    if (newModel.Price == 0)
                    {
                        addModel.SKU_Price = Convert.ToDecimal(newModel.ClientPriceRate) * addModel.SKU_ShopPrice;
                    }
                    else
                    {
                        addModel.SKU_Price = newModel.Price;
                    }
                    addModel.GoodsID = newModel.GoodsID;
                    addModel.SKU_Weight = newModel.Weight;
                    addModel.SKU_Volume = newModel.Volume;
                    addModel.SKU_DistributorPrice = newModel.DistributorPrice;
                    addModel.SKU_SquareWeight = newModel.SquareWeight;
                    addModel.SKU_ExpandArea = newModel.ExpandArea;
                    addModel.SKU_ClientPriceRate = newModel.ClientPriceRate;
                    addModel.SKU_ShopPriceRate = newModel.ShopPriceRate;

                    work.GoodsSKURepository.Insert(addModel);
                    //work.Context.GoodsPropertyValues.AddOrUpdate(m => m.ManagerID, new ManagerWithGroup { ManagerGroupID = _model.GroupID, ManagerID = oldManager.ID });
                    work.Save();
                    //work.Dispose();
                }

                //ViewBag.MessageInfo = AlertHelper.GetAlertDiv("success", "保存成功");

            }
            //return View();

            return Json(json);
        }

        /// <summary>
        /// 添加/编辑处理 -- 批量处理
        /// </summary>
        /// <param name="skuListJson"></param>
        /// <param name="GoodsID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult AddBatch(string skuListJson, int GoodsID = 0)
        {
            JsonResult json = new JsonResult
            {
                Data = new
                {
                    status = "0",
                    msg = "批量添加成功！"
                }
            };
            List<GoodsSKUVModel> newModelList = JsonHelper.DeserializeJsonToList<GoodsSKUVModel>(skuListJson);

            if (ModelState.IsValid && newModelList != null)
            {
                var rstAll = work.Context.GoodsSKUs.Where(m => m.GoodsID == GoodsID).ToList();

                foreach (var newModel in newModelList)
                {
                    newModel.GoodsID = GoodsID;
                    var existlist = rstAll.Where(m => m.GoodsID == newModel.GoodsID & m.Properties == newModel.Properties);
                    if (existlist.Count() > 0)
                    {
                        //更新
                        GoodsSKU existModel = existlist.FirstOrDefault<GoodsSKU>();

                        existModel.PropertiesName = newModel.PropertiesName;
                        existModel.SKU_Count = newModel.Count;
                        existModel.SKU_GoodsCode = newModel.GoodsCode;
                        existModel.SKU_ShopCode = newModel.ShopCode;
                        existModel.SKU_CostPrice = newModel.CostPrice;
                        if (newModel.ShopPrice == 0)
                        {
                            existModel.SKU_ShopPrice = Convert.ToDecimal(newModel.ShopPriceRate) * newModel.CostPrice;
                        }
                        else
                        {
                            existModel.SKU_ShopPrice = newModel.ShopPrice;
                        }
                        if (newModel.Price == 0)
                        {
                            existModel.SKU_Price = Convert.ToDecimal(newModel.ClientPriceRate) * existModel.SKU_ShopPrice;
                        }
                        else
                        {
                            existModel.SKU_Price = newModel.Price;
                        }

                        existModel.GoodsID = newModel.GoodsID;
                        existModel.Properties = newModel.Properties;
                        existModel.SKU_Volume = newModel.Volume;
                        existModel.SKU_Weight = newModel.Weight;
                        existModel.SKU_DistributorPrice = newModel.DistributorPrice;
                        existModel.SKU_SquareWeight = newModel.SquareWeight;
                        existModel.SKU_ExpandArea = newModel.ExpandArea;
                        existModel.SKU_ClientPriceRate = newModel.ClientPriceRate;
                        existModel.SKU_ShopPriceRate = newModel.ShopPriceRate;

                        work.GoodsSKURepository.Update(existModel);
                        //json.Data = new { status = "0", msg = "已存在！" };
                        //return Json(json);
                    }
                    else
                    {
                        GoodsSKU addModel = new GoodsSKU();

                        addModel.Properties = newModel.Properties;
                        addModel.PropertiesName = newModel.PropertiesName;
                        addModel.SKU_Count = newModel.Count;
                        addModel.SKU_GoodsCode = newModel.GoodsCode;
                        addModel.SKU_ShopCode = newModel.ShopCode;
                        addModel.SKU_CostPrice = newModel.CostPrice;
                        if (newModel.ShopPrice == 0)
                        {
                            addModel.SKU_ShopPrice = Convert.ToDecimal(newModel.ShopPriceRate) * newModel.CostPrice;
                        }
                        else
                        {
                            addModel.SKU_ShopPrice = newModel.ShopPrice;
                        }
                        if (newModel.Price == 0)
                        {
                            addModel.SKU_Price = Convert.ToDecimal(newModel.ClientPriceRate) * addModel.SKU_ShopPrice;
                        }
                        else
                        {
                            addModel.SKU_Price = newModel.Price;
                        }
                        addModel.GoodsID = newModel.GoodsID;
                        addModel.SKU_Volume = newModel.Volume;
                        addModel.SKU_Weight = newModel.Weight;
                        addModel.SKU_DistributorPrice = newModel.DistributorPrice;
                        addModel.SKU_SquareWeight = newModel.SquareWeight;
                        addModel.SKU_ExpandArea = newModel.ExpandArea;
                        addModel.SKU_ClientPriceRate = newModel.ClientPriceRate;
                        addModel.SKU_ShopPriceRate = newModel.ShopPriceRate;

                        work.GoodsSKURepository.Insert(addModel);
                    }
                }
                json.Data = new { status = "0", msg = "批量添加成功(" + newModelList.Count() + ")！" };
                work.Save();
                work.Dispose();
            }
            //return View();

            return Json(json);
        }

        #endregion

        #region 删除SKU

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
                var role = work.GoodsSKURepository.Get(m => m.ID == ID).FirstOrDefault<GoodsSKU>();
                work.GoodsSKURepository.Delete(role);
                work.Save();
                work.Dispose();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult Delete(string Properties, int goodsID = 0)
        {
            JsonResult json = new JsonResult
            {
                Data = new
                {
                    status = "0",
                    msg = "删除成功！"
                }
            };
            //List<GoodsSKU> list = work.GoodsSKURepository.Get(m => m.Properties.Contains(Properties) && m.GoodsID == goodsID).ToList();

            //if (list != null && list.Count > 0)
            //{
            //    foreach (var item in list)
            //    {
            //        work.GoodsSKURepository.Delete(item);
            //    }

            //    work.Save();
            //    work.Dispose();
            //}

            work.Context.GoodsSKUs.Where(m => m.Properties.Contains(Properties) && m.GoodsID == goodsID).Delete();
            work.Save();
            work.Dispose();

            return Json(json);
        }
        /// <summary>
        /// 删除 - 批量
        /// </summary>
        /// <param name="PropertiesJson"></param>
        /// <param name="GoodsID"></param>
        /// <returns></returns>
        [CheckPermission]
        [HttpPost]
        public ActionResult DeleteBatch(string PropertiesJson, int GoodsID = 0)
        {
            JsonResult json = new JsonResult
            {
                Data = new
                {
                    status = "0",
                    msg = "批量删除成功！"
                }
            };
            List<string> PropertiesList = JsonHelper.DeserializeJsonToList<string>(PropertiesJson);
            if (PropertiesList != null && PropertiesList.Count > 0)
            {
                //List<GoodsSKU> listAll = work.GoodsSKURepository.Get(m => m.GoodsID == GoodsID).ToList();

                foreach (var Properties in PropertiesList)
                {
                    //List<GoodsSKU> list = listAll.Where(m => m.Properties.Contains(Properties)).ToList();

                    //foreach (var item in list)
                    //{
                    //    work.GoodsSKURepository.Delete(item);
                    //}
                    work.Context.GoodsSKUs.Where(m => m.Properties.Contains(Properties) && m.GoodsID == GoodsID).Delete();
                }

                work.Save();
                work.Dispose();

                json.Data = new { status = "0", msg = "批量删除成功(" + PropertiesList.Count() + ")！" };
            }
            return Json(json);
        }


        #endregion

        #region 获取产品SKU JSON

        /// <summary>
        /// 获取产品 Json
        /// </summary>
        /// <returns></returns>
        [CheckPermission]
        public ActionResult GetSKUJson(int goodsid)
        {
            return Json(GoodsSKUService.GetSKU(goodsid), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}