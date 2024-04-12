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
    public class HealthController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        #region 康复类

        /// <summary>
        /// 康复类
        /// </summary>
        /// <param name="cat">大类ID</param>
        /// <param name="sub">子类ID</param>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int cat = 0, int sub = 0, string keyword = "", string sort = "", int asc = 0, int page = 1)
        {
            ViewBag.cat = cat;
            ViewBag.sub = sub;
            ViewBag.sort = sort;
            ViewBag.asc = asc;
            ViewBag.page = page;
            ViewBag.keyword = keyword;

            int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);
            int g_type = Convert.ToInt16(DataConfig.GoodsTypeEnum.康复类);

            ViewBag.Catagorys = work.GoodsCategoryRepository.Get(m => m.GC_Type == g_type).ToList();
            if (cat != 0)
            {
                ViewBag.CatagorySubs = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == cat).ToList();
            }
            var rst_j = work.Context.Goods
                .Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc })
                .Where(m => m.g.G_Status == goodsStatusOn && m.gc.GC_Type == g_type);
            if (cat != 0)
            {
                rst_j = rst_j.Where(m => m.gc.ID == cat || m.gc.GC_ParentID == cat);
            }
            if (sub != 0)
            {
                rst_j = rst_j.Where(m => m.gc.ID == sub || m.gc.GC_ParentID == sub);
            }
            if (keyword != "")
            {
                rst_j = rst_j.Where(m => m.g.G_Name.Contains(keyword));
            }
            var rst = rst_j.Select(m => m.g);
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
            return View(rst.ToPagedList(page, pageSize));
            //return View();
        }
        #endregion

        #region 详细页面

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
                    int g_type = Convert.ToInt16(DataConfig.GoodsTypeEnum.康复类);

                    ViewBag.Catagorys = work.GoodsCategoryRepository.Get(m => m.GC_Type == g_type).ToList();


                    if (!string.IsNullOrEmpty(model.G_UnitInfo))
                    {
                        model.G_UnitInfo = string.Format("（{0}）", model.G_UnitInfo);
                    }

                    //浏览记录
                    BrowseRecordService.Add(LoginedUserModel, CurrentShopModel, Request.Url.ToString(), model.ID, Convert.ToInt16(DataConfig.BrowseRecordTypeEnum.课程));
                    //康复资讯
                    ViewBag.News = work.ArticleRepository.Get(m => m.ArticleTypeID == 1 && m.Art_IsEnable == 1).OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID).Take(8).ToList();
                    //案例见证
                    ViewBag.Cases = work.ArticleRepository.Get(m => m.ArticleTypeID == 11 && m.Art_IsEnable == 1).OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID).Take(9).ToList();

                    return View(model);
                }
                else
                {
                    Response.Redirect("/Mobile/Health/");
                    Response.End();
                    //return RedirectToAction("Index", "Category");
                }

            }
            else
            {
                Response.Redirect("/Mobile/Health/");
                Response.End();
                //return RedirectToAction("Index", "Category");
            }
            return View();
        }

        #endregion
    }
}