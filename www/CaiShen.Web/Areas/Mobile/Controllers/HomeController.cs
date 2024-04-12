using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pannet.Utility;
using Pannet.Models;
using Pannet.DAL;
using Pannet.DAL.Repository;

namespace Pannet.Web.Areas.Mobile.Controllers
{
    public class HomeController : CheckLoginController
    {
        public UnitOfWork work = new UnitOfWork();
        private int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);//

        public ActionResult Index(int u = 0)
        {
            if (u != 0)
            {
                ViewBag.Referrer = u;
            }
            else
            {
                ViewBag.Referrer = "";
            }
            ViewBag.IsHome = "1";
            //推荐
            ViewBag.GoodsHot = GoodsService.GetHomeRecommendMobile(4);
            ////现货
            //ViewBag.Goods1 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsExist == 1 && m.g.G_IsMobile==1).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();
            ////宣传
            //ViewBag.Goods2 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.gc.GC_ParentID == 1 || m.gc.ID == 1).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsMobile == 1).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();
            ////办公
            //ViewBag.Goods3 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.gc.GC_ParentID == 7 || m.gc.ID == 7).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsMobile == 1).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();
            ////餐饮耗材
            //ViewBag.Goods4 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.gc.GC_ParentID == 29 || m.gc.ID == 29).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsMobile == 1).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();
            ////定制
            //ViewBag.Goods5 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.gc.GC_ParentID == 14 || m.gc.ID == 14).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsMobile == 1).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();
            //所有首页产品
            //var rst_goods1 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsExist == 1 && m.g.G_IsMobile == 1).OrderByDescending(m => m.g.G_IsRecommendMobile).ThenByDescending(m => m.g.G_SortMobile).ThenByDescending(m => m.g.ID).Take(6);
            //var rst_goods2 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.gc.GC_ParentID == 1 || m.gc.ID == 1).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsMobile == 1).OrderByDescending(m => m.g.G_IsRecommendMobile).ThenByDescending(m => m.g.G_SortMobile).ThenByDescending(m => m.g.ID).Take(6);
            //var rst_goods3 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.gc.GC_ParentID == 7 || m.gc.ID == 7).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsMobile == 1).OrderByDescending(m => m.g.G_IsRecommendMobile).ThenByDescending(m => m.g.G_SortMobile).ThenByDescending(m => m.g.ID).Take(6);
            //var rst_goods4 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.gc.GC_ParentID == 29 || m.gc.ID == 29).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsMobile == 1).OrderByDescending(m => m.g.G_IsRecommendMobile).ThenByDescending(m => m.g.G_SortMobile).ThenByDescending(m => m.g.ID).Take(6);
            //var rst_goods5 = work.Context.Goods.Join(work.Context.GoodsCategorys, g => g.GoodsCategoryID, gc => gc.ID, (g, gc) => new { g, gc }).Where(m => m.gc.GC_ParentID == 14 || m.gc.ID == 14).Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsMobile == 1).OrderByDescending(m => m.g.G_IsRecommendMobile).ThenByDescending(m => m.g.G_SortMobile).ThenByDescending(m => m.g.ID).Take(6);
            //var rst = rst_goods1.Union(rst_goods2).Union(rst_goods3).Union(rst_goods4).Union(rst_goods5).ToList();

            //#region 主图处理

            //List<int> goodsIds = rst.Select(m => m.g.ID).ToList();
            ////返回产品的主图
            //List<GoodsPhoto> GoodsPhotoList = work.Context.GoodsPhotos.Where(m => goodsIds.Contains(m.GoodsID)).ToList();
            //foreach (var item in rst)
            //{
            //    if (string.IsNullOrEmpty(item.g.G_MobileImage))//未设置手机预览图
            //    {
            //        GoodsPhoto photoModel = GoodsPhotoList.Where(m => m.GoodsID == item.g.ID).OrderByDescending(m => m.GP_IsFirst).FirstOrDefault();
            //        if (photoModel != null)
            //        {
            //            item.g.G_MobileImage = photoModel.GP_Image;
            //        }
            //    }
            //}

            //#endregion

            ////现货
            //ViewBag.Goods1 = rst.Where(m => m.g.G_IsExist == 1).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();
            ////宣传
            //ViewBag.Goods2 = rst.Where(m => m.gc.GC_ParentID == 1 || m.gc.ID == 1).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();
            ////办公
            //ViewBag.Goods3 = rst.Where(m => m.gc.GC_ParentID == 7 || m.gc.ID == 7).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();
            ////餐饮耗材
            //ViewBag.Goods4 = rst.Where(m => m.gc.GC_ParentID == 29 || m.gc.ID == 29).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();
            ////定制
            //ViewBag.Goods5 = rst.Where(m => m.gc.GC_ParentID == 14 || m.gc.ID == 14).Select(m => m.g).OrderByDescending(m => m.G_IsRecommendMobile).ThenByDescending(m => m.G_SortMobile).ThenByDescending(m => m.ID).Take(6).ToList();

            //康复资讯
            ViewBag.News = work.ArticleRepository.Get(m => m.ArticleTypeID == 1 && m.Art_IsEnable == 1).OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID).Take(8).ToList();
            //搜索热词
            int news_type = Convert.ToInt16(DataConfig.ArticleTypeEnum.搜索推荐关键词);
            ViewBag.Keywords = work.ArticleRepository.Get(m => m.ArticleTypeID == news_type && m.Art_IsEnable == 1).OrderByDescending(m => m.Art_IsRecommend).ThenByDescending(m => m.Art_Sort).ThenByDescending(m => m.ID).Take(4).ToList();
            //滚动图片
            //ViewBag.homeBanner = AdvertisementService.GetByTypeID(10, 20);

            return View();
        }
        public ActionResult Dayang()
        {
            return View();
        }
        /// <summary>
        /// 特惠爆款
        /// </summary>
        /// <returns></returns>
        public ActionResult Sale()
        {
            return View();
        }
        /// <summary>
        /// 数码快印
        /// </summary>
        /// <returns></returns>
        public ActionResult Kuaiyin()
        {
            return View();
        }
    }
}