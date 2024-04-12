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
    public class GoodsController : CheckLoginController
    {
        private UnitOfWork work = new UnitOfWork();
        JsonResult json = new JsonResult
        {
            Data = new { }
        };

        #region 商品类

        /// <summary>
        /// 商品类
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
            int g_type = Convert.ToInt16(DataConfig.GoodsTypeEnum.商品类);

            //搜索热词
            int news_type = Convert.ToInt16(DataConfig.ArticleTypeEnum.商品推荐关键词);
            ViewBag.SearchKeywords = work.ArticleRepository.Get(m => m.ArticleTypeID == news_type && m.Art_IsEnable == 1).OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID).Take(4).ToList();


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

        #region 康复类
        public ActionResult HealthIndex(int cat = 0, int sub = 0, string keyword = "", string sort = "", int asc = 0, int page = 1)
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

        #region 商品详细页面

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

        /// <summary>
        /// 获取分类 Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCategoryJsonByType(int typeid, int parentid)
        {
            List<GoodsCategory> listArea = work.GoodsCategoryRepository.Get(m => m.GC_Type == typeid && m.GC_ParentID == parentid).ToList();

            return Json(listArea, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region 获取评价

        /// <summary>
        /// 获取评价
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public ActionResult GetComments(int goodsID = 0, int page = 1, int pagesize = 10)
        {
            var rst = work.Context.OrderComment
               .Join(work.Context.Users, oc => oc.UserID, u => u.ID, (oc, u) => new { oc, u })
               .Join(work.Context.OrderDetails, ocu => ocu.oc.OrderDetailID, od => od.ID, (ocu, od) => new { ocu.oc, ocu.u, od })
               .Where(m => m.oc.OC_IsDelete == 0 && m.od.GoodsID == goodsID)
               .Select(m => new
               {
                   m.oc,
                   m.u
               }).OrderByDescending(m => m.oc.ID).Skip((page - 1) * pagesize).Take(pagesize).ToList();

            List<OrderCommentClient> list = rst.Select(m => new OrderCommentClient
            {
                Content = m.oc.OC_Content,
                NickName = UserService.GetNickName(m.u.U_NickName, m.oc.OC_IsHiddenName),
                Photos = GetPhotoList(m.oc.OC_Images),
                ReplyContent = m.oc.OC_ReplyContent,
                ScoreGoods = m.oc.OC_ScoreGoods,
                Time = m.oc.OC_CreateTime.ToString("yyyy年MM月dd日"),
                UserImg = UserService.GetThumbnail(m.u.U_Thumbnail)
            }).ToList();
            json.Data = new { status = "success", msg = "", list = list };

            return Json(json.Data, JsonRequestBehavior.AllowGet);
        }

        private List<string> GetPhotoList(string photos)
        {
            if (string.IsNullOrEmpty(photos))
            {
                return null;
            }
            else
            {
                string[] list = photos.Split('|');
                List<string> listPhoto = new List<string>();
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        listPhoto.Add(SiteService.GetImgUrl(item));
                    }
                }
                return listPhoto;
            }
        }

        #endregion
    }
}