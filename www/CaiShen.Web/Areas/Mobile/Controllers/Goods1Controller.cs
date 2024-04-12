using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.DAL;
using Pannet.DAL.Repository;
using Pannet.Models;
using PagedList;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class Goods1Controller : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };
        public ActionResult Index(int ID = 0, string sort = "", int asc = 0, int page = 1, int design = 0)
        {
            ViewBag.ID = ID;
            ViewBag.sort = sort;
            ViewBag.asc = asc;
            ViewBag.page = page;
            ViewBag.design = design;

            //一级分类集合
            ViewBag.GoodCategoryListParent = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0).OrderByDescending(m => m.GC_Sort).ThenBy(m => m.ID);

            int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);
            var rst = work.Context.Goods.Where(m => m.G_Status == goodsStatusOn);
            if (design == 1)
            {
                rst = work.Context.Goods.Where(m => m.G_IsDesign == 1);
            }
            if (ID == 36)//现货
            {
                GoodsCategory categoryModel = work.GoodsCategoryRepository.GetByID(ID);
                ViewBag.GoodCategoryParent = categoryModel;
                //二级分类
                List<GoodsCategory> listSub = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == categoryModel.ID).OrderByDescending(m => m.GC_Sort).ThenBy(m => m.ID).ToList();
                List<Int32> listIDSub = listSub.Select(m => m.ID).ToList();
                listIDSub.Add(categoryModel.ID);

                ViewBag.GoodCategoryListSub = listSub;

                rst = rst.Where(m => m.G_IsExist == 1);
            }
            else if (ID != 0)
            {
                //ViewBag.GoodCategory
                GoodsCategory categoryModel = work.GoodsCategoryRepository.GetByID(ID);

                if (categoryModel == null)
                {
                    Response.Redirect("/Category/");
                    return View();
                }

                //当前为一级
                if (categoryModel != null && categoryModel.GC_ParentID == 0)
                {
                    ViewBag.GoodCategoryParent = categoryModel;
                    //二级分类
                    List<GoodsCategory> listSub = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == categoryModel.ID).OrderByDescending(m => m.GC_Sort).ThenBy(m => m.ID).ToList();
                    List<Int32> listIDSub = listSub.Select(m => m.ID).ToList();
                    listIDSub.Add(categoryModel.ID)
                        ;
                    ViewBag.GoodCategoryListSub = listSub;

                    rst = rst.Where(m => listIDSub.Contains(m.GoodsCategoryID));
                }
                else//当前为二级
                {
                    ViewBag.GoodCategory = categoryModel;
                    ViewBag.GoodCategoryParent = work.GoodsCategoryRepository.GetByID(categoryModel.GC_ParentID);
                    //二级分类
                    ViewBag.GoodCategoryListSub = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == categoryModel.GC_ParentID).OrderByDescending(m => m.GC_Sort).ThenBy(m => m.ID);

                    rst = rst.Where(m => m.GoodsCategoryID == categoryModel.ID);

                }
            }

            int currentShopID = CurrentShopModel.ID;
            if (currentShopID != 0)//不是主站
            {
                //下架商品ID
                List<int> rst_down_goodsids = work.Context.UserShopGoodsDowns.Where(m => m.ShopID == currentShopID).Select(m => m.GoodsID).ToList();
                rst = rst.Where(m => !rst_down_goodsids.Contains(m.ID));
            }

            if (sort == "")
            {
                rst = rst.OrderByDescending(m => m.G_IsRecommend).ThenByDescending(m => m.G_Sort).ThenByDescending(m => m.ID);
            }
            else if (sort == "price")
            {
                if (asc == 1)
                {
                    rst = rst.OrderBy(m => m.G_Price).ThenByDescending(m => m.ID);
                }
                else
                {
                    rst = rst.OrderByDescending(m => m.G_Price).ThenByDescending(m => m.ID);
                }
            }
            else
            {
                switch (sort)
                {
                    case "show": rst = rst.OrderByDescending(m => m.G_ShowTimes); break;
                    case "time": rst = rst.OrderByDescending(m => m.G_CreateTime); break;
                    case "sale": rst = rst.OrderByDescending(m => m.G_SaleCount); break;
                    default: break;
                };
            }
            //rst = rst.OrderByDescending(m => m.ID);

            int pageSize = 40;
            int pageNumber = page;
            PagedList.PagedList<Pannet.Models.Goods> pagedRst = rst.ToPagedList(pageNumber, pageSize) as PagedList.PagedList<Pannet.Models.Goods>;

            #region 主图处理

            List<int> pagedRstIds = pagedRst.Select(m => m.ID).ToList();
            //返回产品的主图
            List<GoodsPhoto> GoodsPhotoList = work.Context.GoodsPhotos.Where(m => pagedRstIds.Contains(m.GoodsID)).ToList();
            foreach (var item in pagedRst)
            {
                GoodsPhoto photoModel = GoodsPhotoList.Where(m => m.GoodsID == item.ID).OrderByDescending(m => m.GP_IsFirst).FirstOrDefault();
                if (photoModel != null)
                {
                    item.G_Image = photoModel.GP_Image;
                }
            }

            #endregion

            return View(pagedRst);
            //return View();
        }

        public ActionResult Detail(int ID = 0, int u = 0)
        {
            if (u != 0)
            {
                ViewBag.Referrer = u;
            }
            else
            {
                ViewBag.Referrer = "";
            }
            if (ID != 0)
            {
                var model = work.Context.Goods.Where(m => m.ID == ID).FirstOrDefault<Goods>();
                if (model != null)
                {
                    #region 销售属性

                    var rst = work.Context.Property
                        .Join(work.Context.GoodsPropertyValues, p => p.ID, gp => gp.PropertyID, (p, gp) => new { p, gp })
                        .Join(work.Context.PropertyValues, gp => gp.gp.PropertyValueID, pv => pv.ID, (gp, pv) => new
                        {
                            gp.p,
                            pv = new
                            {
                                PV_ColorHEX = gp.gp.GPV_ColorHEX,
                                PV_ColorImage = gp.gp.GPV_ColorImage,
                                PV_Increment = gp.gp.GPV_Increment,
                                PV_IsFile = pv.PV_IsFile,
                                PV_IsEnable = pv.PV_IsEnable,
                                PV_Max = gp.gp.GPV_Max,
                                PV_Min = gp.gp.GPV_Min,
                                PV_Multiple = gp.gp.GPV_Multiple,
                                PV_Name = pv.PV_Name,
                                PV_Price = gp.gp.GPV_Price,
                                PV_ShowType = pv.PV_ShowType,
                                PV_Sort = pv.PV_Sort,
                                PV_Unit = string.IsNullOrEmpty(gp.gp.GPV_Unit) ? pv.PV_Unit : gp.gp.GPV_Unit,
                                ID = pv.ID
                            },
                            GoodsID = gp.gp.GoodsID
                        })
                        .Where(m => m.p.GoodsTypeID == model.GoodsTypeID & m.GoodsID == model.ID).ToList();
                    List<Property> listProperty = rst.Select(m => m.p).Distinct().OrderByDescending(m => m.Prop_Sort).ToList();
                    List<int> listParentID = rst.Select(m => m.p.Prop_ParentID).Distinct().Where(m => m != 0).ToList();

                    //所选属性父级
                    List<Property> listPropertyParent = new List<Property>();
                    listPropertyParent = work.Context.Property.Where(m => listParentID.Contains(m.ID)).ToList();

                    //与父类整合在一起
                    foreach (var item in listPropertyParent)
                    {
                        listProperty.Add(item);
                    }
                    //重新排序
                    listProperty = listProperty.Distinct().OrderByDescending(m => m.Prop_Sort).ThenBy(m => m.ID).ToList();

                    List<PropertyVModel> list = new List<PropertyVModel>();
                    //属性值
                    List<PropertyValue> listPropertyValue = new List<PropertyValue>();
                    foreach (var item in listProperty)
                    {
                        int _propertyid = item.ID;
                        listPropertyValue = rst.Where(m => m.p.ID == _propertyid).ToList().Select(m => new PropertyValue
                        {

                            PV_ColorHEX = m.pv.PV_ColorHEX,
                            PV_ColorImage = m.pv.PV_ColorImage,
                            PV_Increment = m.pv.PV_Increment,
                            PV_IsFile = m.pv.PV_IsFile,
                            PV_IsEnable = m.pv.PV_IsEnable,
                            PV_Max = m.pv.PV_Max,
                            PV_Min = m.pv.PV_Min,
                            PV_Multiple = m.pv.PV_Multiple,
                            PV_Name = m.pv.PV_Name,
                            PV_Price = m.pv.PV_Price,
                            PV_ShowType = m.pv.PV_ShowType,
                            PV_Sort = m.pv.PV_Sort,
                            PV_Unit = m.pv.PV_Unit,
                            ID = m.pv.ID

                        }).OrderByDescending(m => m.PV_Sort).ThenBy(m => m.ID).ToList();

                        PropertyVModel vmodel = new PropertyVModel();
                        vmodel.Property = item;
                        vmodel.Values = listPropertyValue;

                        list.Add(vmodel);
                    }
                    #endregion

                    //虚拟实体
                    ViewBag.PropertyVModels = list;
                    //所有属性
                    ViewBag.Properties = listProperty;
                    //产品相册
                    ViewBag.Photos = work.Context.GoodsPhotos.Where(m => m.GoodsID == ID).OrderByDescending(m => m.GP_IsFirst).ThenByDescending(m => m.GP_Sort).ToList();
                    //店铺ID
                    //UserShop currentShopModel = UserShopService.GetCurrentShop();
                    ViewBag.currentUserShop = CurrentShopModel;
                    ViewBag.ShopID = CurrentShopModel.ID;
                    ViewBag.ID = ID;

                    if (!string.IsNullOrEmpty(model.G_UnitInfo))
                    {
                        model.G_UnitInfo = string.Format("（{0}）", model.G_UnitInfo);
                    }

                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.课程));

                    return View(model);
                }
                else
                {
                    Response.Redirect("/Mobile/Goods/");
                    Response.End();
                    //return RedirectToAction("Index", "Category");
                }

            }
            else
            {
                Response.Redirect("/Mobile/Goods/");
                Response.End();
                //return RedirectToAction("Index", "Category");
            }
            return View();
        }

        #region 获取产品SKU JSON

        /// <summary>
        /// 获取产品 Json
        /// </summary>
        /// <param name="goodsid"></param>
        /// <returns></returns>
        public ActionResult GetSKUJson(int goodsid)
        {
            List<GoodsSKUVModel> listB = GoodsSKUService.GetSKUWithFinalPrice(goodsid);
            var rst = listB.Select(m => new { m.CostPrice, m.Count, m.Price, m.Properties, m.ShopPrice, m.ID });
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取非现货产品价格JSON-- 已作废-20171025

        ///// <summary>
        ///// 获取非现货产品价格JSON
        ///// </summary>
        ///// <param name="totalCostPrice">总成本价格</param>
        ///// <returns>总终端销售价格</returns>
        //public ActionResult GetClientPrice(decimal totalCostPrice)
        //{
        //    json.Data = new { status = "success", msg = "", price = DataConfig.GetClientPrice(totalCostPrice) };

        //    return Json(json.Data, JsonRequestBehavior.AllowGet);
        //    //return null;
        //}

        #endregion

        #region 获取白卡纸盒价格---JSON

        /// <summary>
        /// 获取白卡纸盒价格---JSON
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="count"></param>
        /// <param name="pvid">纸张克重 350(815)，400(属性值ID：816)</param>
        /// <param name="hx_pvid">盒形1(1360)，盒形2(属性值ID：1361)</param>
        /// <returns></returns>
        public ActionResult GetBaikaClientPrice(double length, double width, double height, Int32 count, int pvid, int hx_pvid, int goodsid = 0)
        {
            json.Data = GoodsService.GetBaikaPrice(length, width, height, count, pvid, hx_pvid, goodsid);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取UV彩盒价格---JSON

        /// <summary>
        /// 获取UV彩盒价格---JSON
        /// </summary>
        /// <param name="length">长（mm）</param>
        /// <param name="width">款（mm）</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public ActionResult GetUVClientPrice(double length, double width, Int32 count, int goodsid = 0)
        {
            json.Data = GoodsService.GetUVPrice(length, width, count, goodsid);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 按需定制

        public ActionResult Custom()
        {
            ViewBag.GoodsTypes = work.GoodsTypeRepository.Get();

            return View();
        }

        #region 获取属性名称+属性值 JSON

        /// <summary>
        /// 获取属性 Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPropertyJson(int typeid)
        {
            List<Property> listArea = work.PropertyRepository.Get(m => m.GoodsTypeID == typeid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取属性值 Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPropertyValueJson(int propertyid)
        {
            List<PropertyValue> listArea = work.PropertyValueRepository.Get(m => m.PropertyID == propertyid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 获取-打样-价格---JSON

        /// <summary>
        /// 获取-打样-价格---JSON
        /// </summary>
        /// <param name="length">长（mm）</param>
        /// <param name="width">宽（mm）</param>
        /// <param name="isfumo">覆膜1是，0不覆膜</param>
        /// <param name="count">数量</param>
        /// <param name="type">type:1彩色包装软盒打样,2:彩色瓦楞盒打样,3纸袋打样,4:纸袋打样2</param>
        /// <param name="goodsid">产品ID</param>
        /// <returns></returns>
        public ActionResult GetDaYangClientPrice(double length, double width, int isfumo, Int32 count, int type = 1, int goodsid = 0)
        {
            json.Data = GoodsService.GetDaYangPrice(length, width, isfumo, count, type, goodsid);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #region 作废20180416
        ///// <summary>
        ///// 获取彩色包装软盒打样--总成本价JSON
        ///// </summary>
        ///// <param name="length">长（mm）</param>
        ///// <param name="width">宽（mm）</param>
        ///// <param name="isfumo">覆膜1是，0不覆膜</param>
        ///// <param name="count">数量</param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public ActionResult GetDaYang1ClientPrice(double length, double width, int isfumo, Int32 count, int goodsid = 0)
        //{
        //    PriceResultModel priceModel = GoodsService.GetDaYang1Price(length, width, isfumo, count, 1);
        //    json.Data = new { status = "success", msg = "", price = GoodsService.GetFinalPrice(priceModel.ClientPrice, priceModel.ShopPrice, goodsid), shopprice = priceModel.ShopPrice, costprice = priceModel.CostPrice };
        //    return Json(json.Data, JsonRequestBehavior.AllowGet);
        //}
        ///// <summary>
        ///// 彩色瓦楞盒打样--总成本价JSON
        ///// </summary>
        ///// <param name="length">长（mm）</param>
        ///// <param name="width">宽（mm）</param>
        ///// <param name="isfumo">覆膜1是，0不覆膜</param>
        ///// <param name="count">数量</param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public ActionResult GetDaYang2ClientPrice(double length, double width, int isfumo, Int32 count, int goodsid = 0)
        //{
        //    PriceResultModel priceModel = GoodsService.GetDaYang1Price(length, width, isfumo, count, 2);
        //    json.Data = new { status = "success", msg = "", price = GoodsService.GetFinalPrice(priceModel.ClientPrice, priceModel.ShopPrice, goodsid), shopprice = priceModel.ShopPrice, costprice = priceModel.CostPrice };
        //    return Json(json.Data, JsonRequestBehavior.AllowGet);
        //}
        ///// <summary>
        ///// 获取 纸袋打样--总成本价JSON
        ///// </summary>
        ///// <param name="length">长（mm）</param>
        ///// <param name="width">宽（mm）</param>
        ///// <param name="isfumo">覆膜1是，0不覆膜</param>
        ///// <param name="count">数量</param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public ActionResult GetDaYang3ClientPrice(double length, double width, int isfumo, Int32 count, int goodsid = 0)
        //{
        //    PriceResultModel priceModel = GoodsService.GetDaYang2Price(length, width, count, 3);
        //    json.Data = new { status = "success", msg = "", price = GoodsService.GetFinalPrice(priceModel.ClientPrice, priceModel.ShopPrice, goodsid), shopprice = priceModel.ShopPrice, costprice = priceModel.CostPrice };
        //    return Json(json.Data, JsonRequestBehavior.AllowGet);
        //}
        ///// <summary>
        ///// 获取 纸袋打样2--总成本价JSON
        ///// </summary>
        ///// <param name="length">长（mm）</param>
        ///// <param name="width">宽（mm）</param>
        ///// <param name="isfumo">覆膜1是，0不覆膜</param>
        ///// <param name="count">数量</param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public ActionResult GetDaYang4ClientPrice(double length, double width, int isfumo, Int32 count, int goodsid = 0)
        //{
        //    PriceResultModel priceModel = GoodsService.GetDaYang2Price(length, width, count, 4);
        //    json.Data = new { status = "success", msg = "", price = GoodsService.GetFinalPrice(priceModel.ClientPrice, priceModel.ShopPrice, goodsid), shopprice = priceModel.ShopPrice, costprice = priceModel.CostPrice };
        //    return Json(json.Data, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #endregion

        #region 获取产品价格JSON 包含隐藏运费--20171114

        /// <summary>
        /// 获取产品价格JSON 包含隐藏运费
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="totalGoodsCount">总个数</param>
        /// <param name="totalClientPrice">总终端价格</param>
        /// <returns>总终端销售价格</returns>
        public ActionResult GetClientPriceWithPostFee(int goodsID, int totalGoodsCount, decimal totalClientPrice)
        {
            decimal postFee = OrderService.GetGoodsHiddenShippingFee(goodsID, totalGoodsCount, UserShopService.GetCurrentShop());

            json.Data = new { status = "success", msg = "", price = totalClientPrice + postFee };

            return Json(json.Data, JsonRequestBehavior.AllowGet);
            //return null;
        }

        /// <summary>
        /// 获取产品隐藏运费
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="totalGoodsCount">总个数</param>
        /// <returns></returns>
        public ActionResult GetAddPrice(int goodsID, int totalGoodsCount)
        {
            decimal postFee = 0;//2017-12-18取消同区域隐藏运费计算 OrderService.GetGoodsHiddenShippingFee(goodsID, totalGoodsCount, UserShopService.GetCurrentShop());

            json.Data = new { status = "success", msg = "", price = postFee };

            return Json(json.Data, JsonRequestBehavior.AllowGet);
            //return null;
        }
        #endregion

        #region 获取通用不干胶--总终端价---JSON

        ///// <summary>
        ///// 获取通用不干胶--总终端价
        ///// </summary>
        ///// <param name="length"></param>
        ///// <param name="width"></param>
        ///// <param name="count"></param>
        ///// <param name="fm_pvid">表面处理：1480不覆膜，1481覆亮膜，1482覆亚膜</param>
        ///// <returns></returns>
        //public ActionResult GetBuGanJiaoClientPrice(double length, double width, Int32 count, int fm_pvid)
        //{
        //    decimal clientPrice = 0;
        //    if (fm_pvid == 1480)
        //    {
        //        clientPrice = DataConfig.GetBuGanJiaoClientPrice(length, width, count, "1");
        //    }
        //    else if (fm_pvid == 1481)
        //    {
        //        clientPrice = DataConfig.GetBuGanJiaoClientPrice(length, width, count, "2");
        //    }
        //    else if (fm_pvid == 1482)
        //    {
        //        clientPrice = DataConfig.GetBuGanJiaoClientPrice(length, width, count, "3");
        //    }
        //    json.Data = new { status = "success", msg = "", price = clientPrice };

        //    return Json(json.Data, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region 获取不干胶--总终端价---JSON

        /// <summary>
        /// 获取不干胶--总终端价
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="count"></param>
        /// <param name="cz_pvid">材质类型：1牛皮纸不干胶，2透明不干胶，3涤纶不干胶(金），4普通铜版纸不干胶，5普通铜版纸不干胶覆亮膜，6普通铜版纸不干胶覆哑膜</param>
        /// <param name="fm_pvid">1506不覆膜,1507覆亚膜,1508覆亮膜:</param>
        /// <returns></returns>
        public ActionResult GetBuGanJiaoClientPriceNew(double length, double width, Int32 count, int cz_pvid, int fm_pvid, int goodsid = 0)
        {
            json.Data = GoodsService.GetBuGanJiaoPriceNew(length, width, count, cz_pvid, fm_pvid, goodsid);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取单件产品SKUPrice JSON

        /// <summary>
        /// 获取单件产品SKUPrice价格 Json
        /// </summary>
        /// <param name="goodsid"></param>
        /// <param name="skuid"></param>
        /// <param name="skucount"></param>
        /// <returns></returns>
        public ActionResult GetSKUPrice(int goodsid = 0, int skuid = 0, int skucount = 0)
        {
            json.Data = GoodsSKUPriceService.GetSKUPrice(goodsid, skuid, skucount);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取产品价格 JSON

        /// <summary>
        /// 获取产品价格 Json
        /// </summary>
        /// <param name="goodsid"></param>
        /// <param name="skuid"></param>
        /// <param name="skucount"></param>
        /// <returns></returns>
        public ActionResult GetPrice(int goodsid = 0, int skuid = 0, int skucount = 0)
        {
            json.Data = GoodsService.GetPrice(goodsid, skuid, skucount);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取PVC异形卡价格--JSON

        /// <summary>
        /// 获取PVC异形卡价格--JSON
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="count"></param>
        /// <param name="cz_pvid">材质类型：PVC亮光卡，PVC哑光卡，PVC磨砂卡</param>
        /// <param name="fm_pvid">1506不覆膜,1507覆亚膜,1508覆亮膜:</param>
        /// <returns></returns>
        public ActionResult GetPVCPrice(double length, double width, Int32 count, int cz_pvid, int goodsid = 0)
        {
            json.Data = GoodsService.GetPVCPrice(length, width, count, cz_pvid, goodsid);
            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}