using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class GoodsService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Goods GetModel(int ID)
        {
            var list = work.GoodsRepository.Get(m => m.ID == ID, null).ToList<Goods>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new Goods();
        }

        #endregion

        #region 更新产品主图

        /// <summary>
        /// 更新产品主图
        /// </summary>
        /// <returns></returns>
        public static bool UpdateImage(int ID, string imgsrc)
        {
            var list = work.GoodsRepository.Get(m => m.ID == ID).ToList<Goods>();
            foreach (var item in list)
            {
                if (item.ID == ID)
                {
                    item.G_Image = imgsrc;
                }
                work.GoodsRepository.Update(item);
                work.Save();
            }
            return true;
        }

        #endregion

        #region 获取相关推荐产品

        /// <summary>
        /// 获取相关推荐产品
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="top">获取数据条数</param>
        /// <returns></returns>
        public static List<Goods> GetByUser(int UserID, int top = 0)
        {
            if (top == 0)
            {
                top = 5;
            }
            return work.GoodsRepository.Get(m => m.G_IsRecommend == 1).OrderByDescending(m => m.G_Sort).ThenByDescending(m => m.ID).Take(top).ToList();
        }

        /// <summary>
        /// 获取相关推荐产品
        /// </summary>
        /// <param name="GoodsID"></param>
        /// <param name="top">获取数据条数</param>
        /// <returns></returns>
        public static List<Goods> GetByGoods(int GoodsID, int top = 0)
        {
            if (top == 0)
            {
                top = 5;
            }
            return work.GoodsRepository.Get(m => m.G_IsRecommend == 1 && m.ID != GoodsID).OrderByDescending(m => m.G_Sort).ThenByDescending(m => m.ID).Take(top).ToList();
        }
        /// <summary>
        /// 获取相关推荐产品
        /// </summary>
        /// <param name="GoodsID"></param>
        /// <param name="top">获取数据条数</param>
        /// <returns></returns>
        public static List<Goods> GetByGoodsCategory(int GoodsCategoryID, int top = 0)
        {
            if (top == 0)
            {
                top = 5;
            }
            return work.GoodsRepository.Get(m => m.GoodsCategoryID == GoodsCategoryID && m.G_IsRecommend == 1).OrderByDescending(m => m.G_Sort).ThenByDescending(m => m.ID).Take(top).ToList();
        }

        #endregion

        #region 获取赠送积分

        /// <summary>
        /// 获取赠送积分
        /// </summary>
        /// <param name="price">实付金额</param>
        /// <returns></returns>
        public static Int32 GetGiveScoreByPrice(decimal price)
        {
            return Convert.ToInt32(price);
        }

        #endregion

        #region 是否支持自提

        /// <summary>
        /// 购买产品中如有不支持自提，所有都不支持自提
        /// </summary>
        /// <param name="cartids"></param>
        /// <returns></returns>
        public static bool IsZiti(Int32[] cartids)
        {
            var rst = work.Context.Goods.Where(m => m.G_IsZiti == 0 && cartids.Contains(m.ID));
            if (rst.Count() > 0)
                return false;
            return true;
        }

        #endregion

        #region 是否是现货产品

        /// <summary>
        /// 是否是现货产品
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static bool IsExist(int goodsID)
        {
            Goods model = work.GoodsRepository.GetByID(goodsID);
            return IsExist(model);
        }

        /// <summary>
        /// 是否是现货产品
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static bool IsExist(Goods model)
        {
            if (model != null)
            {
                if (model.G_IsExist == 1)
                {
                    return true;
                }
                //现货商品区分类ID
                var rst = work.Context.GoodsCategorys.Where(m => m.ID == 4 || m.GC_ParentID == 4);
                rst = rst.Where(m => m.ID == model.GoodsCategoryID);

                if (rst.Count() > 0)
                    return true;
            }
            return false;
        }

        #endregion

        #region 获取店铺首页推荐产品

        /// <summary>
        /// 获取店铺首页推荐产品
        /// </summary>
        /// <param name="top">获取产品数量</param>
        /// <returns></returns>
        public static List<Goods> GetHomeRecommend(int top)
        {
            int currentShopID = UserShopService.GetCurrentShopID();
            List<Int32> goodsDownIds = work.Context.UserShopGoodsDowns.Where(gd => gd.ShopID == currentShopID).Select(gd => gd.GoodsID).ToList<Int32>();
            //var rst = work.Context.Goods.AsNoTracking()
            //        .GroupJoin(work.Context.UserShopGoodsSets, g => g.ID, ug => ug.GoodsID, (g, ug) => new { g, ug })
            //            .Where(m => goodsDownIds.Contains(m.g.ID) == false && m.g.G_Status == 1)//上架产品
            //        .SelectMany(
            //            xy => xy.ug.DefaultIfEmpty(),
            //           (x, y) => new { g = x.g, ug = y })
            //        .Where(m => m.ug.ShopID == currentShopID)
            //        .Select(m => new
            //        {
            //            Goods = m.g,
            //            UG_Discount = m.ug.UG_Discount == 0 ? 1 : m.ug.UG_Discount,
            //            UG_IsRecommend = m.ug.UG_IsRecommend,
            //            UG_Sort = m.ug.UG_Sort,
            //            UG_Image = m.ug.UG_Image,
            //            UG_MobileImage = m.ug.UG_MobileImage
            //        })
            //已设置
            var rst_set = work.Context.Goods.AsNoTracking()
                .Join(work.Context.UserShopGoodsSets, g => g.ID, ug => ug.GoodsID, (g, ug) => new { g, ug })
                .Where(m => goodsDownIds.Contains(m.g.ID) == false && m.g.G_Status == 1 && m.ug.ShopID == currentShopID)
                .Select(m => new ShopGoodsVModel
                {
                    Goods = m.g,
                    UG_Discount = m.ug.UG_Discount == 0 ? 1 : m.ug.UG_Discount,
                    UG_IsRecommend = m.ug.UG_IsRecommend,
                    UG_Sort = m.ug.UG_Sort,
                    UG_Image = m.ug.UG_Image,
                    UG_MobileImage = m.ug.UG_MobileImage,
                    UG_IsRecommendMobile = m.ug.UG_IsRecommendMobile,
                    UG_SortMobile = m.ug.UG_SortMobile
                });
            //已设置产品ID
            List<Int32> setGoodsIds = rst_set.Select(m => m.Goods.ID).Distinct().ToList<Int32>();

            //其他未设置
            var rst_noset = work.Context.Goods
                .Where(m => goodsDownIds.Contains(m.ID) == false && m.G_Status == 1 && !setGoodsIds.Contains(m.ID))
                .Select(m => new ShopGoodsVModel
                {
                    Goods = m,
                    UG_Discount = 1,
                    UG_IsRecommend = 0,
                    UG_Sort = 0,
                    UG_Image = "",
                    UG_MobileImage = "",
                    UG_IsRecommendMobile = 0,
                    UG_SortMobile = 0
                });

            //整合结果
            var rst = rst_set.Union(rst_noset)
                //优先店铺排序，再平台设置排序
            .OrderByDescending(m => m.UG_IsRecommend).ThenByDescending(m => m.UG_Sort).ThenByDescending(m => m.Goods.G_IsRecommend).ThenByDescending(m => m.Goods.G_Sort).ThenByDescending(m => m.Goods.ID).Take(top).ToList();
            List<Goods> goodsList = new List<Goods>();
            Goods goodsModel;
            foreach (var item in rst)
            {
                goodsModel = item.Goods;
                //店铺设置图片，优先使用
                if (!string.IsNullOrEmpty(item.UG_Image))
                {
                    goodsModel.G_Image = item.UG_Image;
                }
                if (!string.IsNullOrEmpty(item.UG_MobileImage))
                {
                    goodsModel.G_MobileImage = item.UG_MobileImage;
                }
                goodsList.Add(goodsModel);
            }
            return goodsList;
        }
        //备份
        //public static List<Goods> GetHomeRecommend_BAK(int top)
        //{
        //    int currentShopID = UserShopService.GetCurrentShopID();
        //    List<Int32> goodsDownIds = work.Context.UserShopGoodsDowns.Where(gd => gd.ShopID == currentShopID).Select(gd => gd.GoodsID).ToList<Int32>();
        //    var rst = work.Context.Goods.AsNoTracking()
        //            .GroupJoin(work.Context.UserShopGoodsSets, g => g.ID, ug => ug.GoodsID, (g, ug) => new { g, ug })
        //                .Where(m => goodsDownIds.Contains(m.g.ID) == false && m.g.G_Status == 1)//上架产品
        //            .Select(m => new
        //            {
        //                Goods = m.g,
        //                UG_Discount = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_Discount).FirstOrDefault() == 0 ? 1 : m.ug.Select(ug => ug.UG_Discount).FirstOrDefault(),
        //                UG_IsRecommend = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_IsRecommend).FirstOrDefault(),
        //                UG_Sort = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_Sort).FirstOrDefault(),
        //                UG_Image = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_Image).FirstOrDefault(),
        //                UG_MobileImage = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_MobileImage).FirstOrDefault()
        //            })
        //        //优先店铺排序，再平台设置排序
        //            .OrderByDescending(m => m.UG_IsRecommend).ThenByDescending(m => m.UG_Sort).ThenByDescending(m => m.Goods.G_IsRecommend).ThenByDescending(m => m.Goods.G_Sort).ThenByDescending(m => m.Goods.ID).Take(top).ToList();
        //    List<Goods> goodsList = new List<Goods>();
        //    Goods goodsModel;
        //    foreach (var item in rst)
        //    {
        //        goodsModel = item.Goods;
        //        //店铺设置图片，优先使用
        //        if (!string.IsNullOrEmpty(item.UG_Image))
        //        {
        //            goodsModel.G_Image = item.UG_Image;
        //        }
        //        if (!string.IsNullOrEmpty(item.UG_MobileImage))
        //        {
        //            goodsModel.G_MobileImage = item.UG_MobileImage;
        //        }
        //        goodsList.Add(goodsModel);
        //    }
        //    return goodsList;
        //}
        /// <summary>
        /// 获取店铺首页推荐产品
        /// </summary>
        /// <param name="top">获取产品数量</param>
        /// <returns></returns>
        public static List<Goods> GetHomeRecommendMobile(int top)
        {
            int currentShopID = UserShopService.GetCurrentShopID();
            List<Int32> goodsDownIds = work.Context.UserShopGoodsDowns.Where(gd => gd.ShopID == currentShopID).Select(gd => gd.GoodsID).ToList<Int32>();
            //下面代码有BUG
            //var rst = work.Context.Goods.AsNoTracking()
            //        .GroupJoin(work.Context.UserShopGoodsSets, g => g.ID, ug => ug.GoodsID, (g, ug) => new { g, ug })
            //            .Where(m => goodsDownIds.Contains(m.g.ID) == false && m.g.G_Status == 1 && m.g.G_IsMobile == 1)//上架产品
            //        .SelectMany(
            //            xy => xy.ug.DefaultIfEmpty(),
            //           (x, y) => new { g = x.g, ug = y })
            //        .Where(m => m.ug.ShopID == currentShopID)
            //        .Select(m => new
            //        {
            //            Goods = m.g,
            //            UG_Discount = m.ug.UG_Discount == 0 ? 1 : m.ug.UG_Discount,
            //            UG_IsRecommend = m.ug.UG_IsRecommend,
            //            UG_Sort = m.ug.UG_Sort,
            //            UG_Image = m.ug.UG_Image,
            //            UG_MobileImage = m.ug.UG_MobileImage,
            //            UG_IsRecommendMobile = m.ug.UG_IsRecommendMobile,
            //            UG_SortMobile = m.ug.UG_SortMobile
            //        })

            //已设置
            var rst_set = work.Context.Goods.AsNoTracking()
                .Join(work.Context.UserShopGoodsSets, g => g.ID, ug => ug.GoodsID, (g, ug) => new { g, ug })
                .Where(m => goodsDownIds.Contains(m.g.ID) == false && m.g.G_Status == 1 && m.ug.ShopID == currentShopID && m.g.G_IsMobile == 1)
                .Select(m => new ShopGoodsVModel
                {
                    Goods = m.g,
                    UG_Discount = m.ug.UG_Discount == 0 ? 1 : m.ug.UG_Discount,
                    UG_IsRecommend = m.ug.UG_IsRecommend,
                    UG_Sort = m.ug.UG_Sort,
                    UG_Image = m.ug.UG_Image,
                    UG_MobileImage = m.ug.UG_MobileImage,
                    UG_IsRecommendMobile = m.ug.UG_IsRecommendMobile,
                    UG_SortMobile = m.ug.UG_SortMobile
                });
            //已设置产品ID
            List<Int32> setGoodsIds = rst_set.Select(m => m.Goods.ID).Distinct().ToList<Int32>();

            //其他未设置
            var rst_noset = work.Context.Goods
                .Where(m => goodsDownIds.Contains(m.ID) == false && m.G_Status == 1 && !setGoodsIds.Contains(m.ID))
                .Select(m => new ShopGoodsVModel
                {
                    Goods = m,
                    UG_Discount = 1,
                    UG_IsRecommend = 0,
                    UG_Sort = 0,
                    UG_Image = "",
                    UG_MobileImage = "",
                    UG_IsRecommendMobile = 0,
                    UG_SortMobile = 0
                });

            //整合结果
            var rst = rst_set.Union(rst_noset)
                //优先店铺排序，再平台设置排序
            .OrderByDescending(m => m.UG_IsRecommendMobile).ThenByDescending(m => m.UG_SortMobile).ThenByDescending(m => m.Goods.G_IsRecommendMobile).ThenByDescending(m => m.Goods.G_SortMobile).ThenByDescending(m => m.Goods.ID).Take(top);
            List<Goods> goodsList = new List<Goods>();
            Goods goodsModel;
            foreach (var item in rst)
            {
                goodsModel = item.Goods;
                //店铺设置图片，优先使用
                if (!string.IsNullOrEmpty(item.UG_Image))
                {
                    goodsModel.G_Image = item.UG_Image;
                }
                if (!string.IsNullOrEmpty(item.UG_MobileImage))
                {
                    goodsModel.G_MobileImage = item.UG_MobileImage;
                }
                goodsList.Add(goodsModel);
            }
            return goodsList;
        }
        //备份
        //public static List<Goods> GetHomeRecommendMobile_BAK(int top)
        //{
        //    int currentShopID = UserShopService.GetCurrentShopID();
        //    var rst = work.Context.Goods
        //            .GroupJoin(
        //                work.Context.UserShopGoodsDowns.Where(gd => gd.ShopID == currentShopID).Select(gd => gd.GoodsID),
        //                goods => goods.ID,
        //                goodsDownID => goodsDownID,
        //                (g, gd) => new { goods = g, goodsDownID = gd })
        //            .GroupJoin(work.Context.UserShopGoodsSets, g => g.goods.ID, ug => ug.GoodsID, (g, ug) => new { g.goods, g.goodsDownID, ug })
        //                .Where(m => m.goodsDownID.Contains(m.goods.ID) == false && m.goods.G_Status == 1 && m.goods.G_IsMobile == 1)//上架产品
        //            .Select(m => new
        //            {
        //                Goods = m.goods,
        //                UG_Discount = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_Discount).FirstOrDefault() == 0 ? 1 : m.ug.Select(ug => ug.UG_Discount).FirstOrDefault(),
        //                UG_IsRecommend = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_IsRecommend).FirstOrDefault(),
        //                UG_Sort = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_Sort).FirstOrDefault(),
        //                UG_Image = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_Image).FirstOrDefault(),
        //                UG_MobileImage = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_MobileImage).FirstOrDefault(),
        //                UG_IsRecommendMobile = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_IsRecommendMobile).FirstOrDefault(),
        //                UG_SortMobile = m.ug.Where(ug => ug.ShopID == currentShopID).Select(ug => ug.UG_SortMobile).FirstOrDefault()
        //            });
        //    //优先店铺排序，再平台设置排序
        //    //rst = rst.OrderByDescending(m => m.UG_IsRecommend).ThenByDescending(m => m.UG_Sort).ThenByDescending(m => m.Goods.G_IsRecommendMobile).ThenByDescending(m => m.Goods.G_SortMobile).ThenByDescending(m => m.Goods.ID).Take(top);
        //    //rst = rst.OrderByDescending(m => m.Goods.G_IsRecommendMobile).ThenByDescending(m => m.Goods.G_SortMobile).ThenByDescending(m => m.Goods.ID).Take(top);
        //    rst = rst.OrderByDescending(m => m.UG_IsRecommendMobile).ThenByDescending(m => m.UG_SortMobile).ThenByDescending(m => m.Goods.G_IsRecommendMobile).ThenByDescending(m => m.Goods.G_SortMobile).ThenByDescending(m => m.Goods.ID).Take(top);
        //    List<Goods> goodsList = new List<Goods>();
        //    Goods goodsModel;
        //    foreach (var item in rst)
        //    {
        //        goodsModel = item.Goods;
        //        //店铺设置图片，优先使用
        //        if (!string.IsNullOrEmpty(item.UG_Image))
        //        {
        //            goodsModel.G_Image = item.UG_Image;
        //        }
        //        if (!string.IsNullOrEmpty(item.UG_MobileImage))
        //        {
        //            goodsModel.G_MobileImage = item.UG_MobileImage;
        //        }
        //        goodsList.Add(goodsModel);
        //    }
        //    return goodsList;
        //}
        #endregion

        #region 获取产品最终销售价格（单个SKU）

        /// <summary>
        /// 单件产品最终出售价格（1.门店购买显示门店价格，2.会员购买显示：终端价格*门店折扣之后的价格）
        /// </summary>
        /// <param name="clientPrice">终端价格</param>
        /// <param name="shopPrice">加盟商价格</param>
        /// <param name="goodsID">购买产品</param>
        /// <param name="shopID">当前店铺</param>
        public static decimal GetFinalPrice(decimal clientPrice, decimal shopPrice, int goodsID)
        {
            //购买者
            User buyUser = UserService.GetLoginedModel();
            int shopID = UserShopService.GetCurrentShopID();

            if (buyUser == null || shopID == 0)
            {
                return clientPrice;
            }

            if (buyUser.UserRoleID == Convert.ToInt32(DataConfig.RoleEnum.加盟店))//如果购买者是加盟商，直接显示加盟商价
            {
                return shopPrice;
            }

            //门店是否设置过折扣
            double discount = 1;
            var existSet = work.Context.UserShopGoodsSets.Where(m => m.ShopID == shopID && m.GoodsID == goodsID).FirstOrDefault();

            if (existSet != null && existSet.UG_Discount != 0)
            {
                discount = existSet.UG_Discount;
            }

            return clientPrice * Convert.ToDecimal(discount);
        }

        #endregion

        #region 主图处理

        /// <summary>
        /// 主图处理
        /// </summary>
        /// <param name="listV"></param>
        /// <returns></returns>
        public static List<Goods> GetGoodsWidthPhoto(List<Goods> goodsList)
        {
            //主图处理
            List<int> goodsListIds = goodsList.Select(m => m.ID).ToList();
            //返回产品的主图
            List<GoodsPhoto> GoodsPhotoList = work.Context.GoodsPhotos.Where(m => goodsListIds.Contains(m.GoodsID)).ToList();
            foreach (var item in goodsList)
            {
                GoodsPhoto photoModel = GoodsPhotoList.Where(m => m.GoodsID == item.ID).OrderByDescending(m => m.GP_IsFirst).FirstOrDefault();
                if (photoModel != null)
                {
                    item.G_Image = photoModel.GP_Image;
                }
            }
            return goodsList;
        }

        #endregion

        #region 获取不干胶价格

        /// <summary>
        /// 获取不干胶价格
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="count"></param>
        /// <param name="cz_pvid">材质类型：1牛皮纸不干胶，2透明不干胶，3涤纶不干胶(金），4普通铜版纸不干胶，5普通铜版纸不干胶覆亮膜，6普通铜版纸不干胶覆哑膜</param>
        /// <param name="fm_pvid">1506不覆膜,1507覆亚膜,1508覆亮膜:</param>
        /// <returns></returns>
        public static GoodsPriceResult GetBuGanJiaoPriceNew(double length, double width, Int32 count, int cz_pvid, int fm_pvid = 1506, int goodsid = 0)
        {
            //decimal clientPrice = 0;
            int type = 1;
            if (cz_pvid == 1500)
            {
                type = 1;
            }
            else if (cz_pvid == 1501)
            {
                type = 2;
            }
            else if (cz_pvid == 1502)
            {
                type = 3;
            }
            else if (cz_pvid == 1503)
            {
                if (fm_pvid == 1507)
                {
                    type = 6;
                }
                else if (fm_pvid == 1508)
                {
                    type = 5;
                }
                else
                {
                    type = 4;
                }
            }
            return GetBuGanJiaoPriceNew(length, width, count, type.ToString(), goodsid);
        }

        /// <summary>
        /// 获取不干胶价格
        /// </summary>
        /// <param name="length">长 mm</param>
        /// <param name="width">宽 mm</param>
        /// <param name="count">数量</param>
        /// <param name="type">1牛皮纸不干胶，2透明不干胶，3涤纶不干胶(金），4普通铜版纸不干胶，5普通铜版纸不干胶覆亮膜，6普通铜版纸不干胶覆哑膜</param>
        /// <returns></returns>
        public static GoodsPriceResult GetBuGanJiaoPriceNew(double length, double width, int count, string type, int goodsid = 0)
        {
            return GetBuGanJiaoPriceNew(length * width / 1000000, count, type, goodsid);
        }

        /// <summary>
        /// 获取不干胶价格
        /// </summary>
        /// <param name="unitArea">单件展开面积（平方米）</param>
        /// <param name="count">数量</param>
        /// <param name="type">1牛皮纸不干胶，2透明不干胶，3涤纶不干胶(金），4普通铜版纸不干胶，5普通铜版纸不干胶覆亮膜，6普通铜版纸不干胶覆哑膜</param>
        /// <returns></returns>
        public static GoodsPriceResult GetBuGanJiaoPriceNew(double unitArea, int count, string type, int goodsid = 0)
        {
            List<PriceResultModel> list = XlsPriceData.GetBuGanJiaoListNew(type);
            return GetPriceModelByUnitArea(unitArea, count, goodsid, list);
        }

        #endregion

        #region 获取白卡纸盒价格

        /// <summary>
        /// 获取白卡纸盒价格
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="count"></param>
        /// <param name="pvid">纸张克重 350(815)，400(属性值ID：816)</param>
        /// <param name="hx_pvid">盒形1(1360)，盒形2(属性值ID：1361)</param>
        /// <returns></returns>
        public static GoodsPriceResult GetBaikaPrice(double length, double width, double height, Int32 count, int pvid, int hx_pvid, int goodsid = 0)
        {
            int type = 1;
            if (pvid == 815)
            {
                type = 1;
            }
            else
            {
                type = 2;
            }
            return GetBaiKaPrice(length, width, height, count, hx_pvid, type, goodsid);
        }

        /// <summary>
        /// 获取白卡纸盒价格
        /// </summary>
        /// <param name="length">长(mm)</param>
        /// <param name="width">宽(mm)</param>
        /// <param name="height">高(mm)</param>
        /// <param name="count">数量</param>
        /// <param name="hexing">盒形1,盒形2</param>
        /// <param name="type">1:350g,2:400g</param>
        /// <returns></returns>
        public static GoodsPriceResult GetBaiKaPrice(double length, double width, double height, int count, int hexing, int type, int goodsid = 0)
        {
            double unitArea = GetBaikaArea(length, width, height, hexing);//length * width / 1000000;
            return GetBaiKaPrice(unitArea, count, type, goodsid);
        }
        /// <summary>
        /// 获取白卡纸盒价格
        /// </summary>
        /// <param name="unitArea">面积(平方米)</param>
        /// <param name="count">数量</param>
        /// <param name="type">1:350g,2:400g</param>
        /// <returns></returns>
        public static GoodsPriceResult GetBaiKaPrice(double unitArea, int count, int type = 1, int goodsid = 0)
        {
            List<PriceResultModel> list = XlsPriceData.GetBaiKaPriceList(type);
            //Log.WriteLog(JsonHelper.SerializeObject(list) + "\n", "xls", DateTime.Now.ToString("yyyyMMdd"));
            return GetPriceModelByUnitArea(unitArea, count, goodsid, list);
        }

        /// <summary>
        /// 计算白卡纸盒展开面积
        /// </summary>
        /// <param name="length">盒长(mm)</param>
        /// <param name="width">盒宽(mm)</param>
        /// <param name="height">盒高(mm)</param>
        /// <param name="hexing">盒形1(属性值ID：1360)，盒形2(属性值ID：1361)</param>
        /// <returns>返回面积 平方米</returns>
        public static double GetBaikaArea(double length, double width, double height, int hexing)
        {
            double length_z = (length + width) * 2 + 20;//展开长(length + width) * 2 + 2;
            double width_z = 0;//展开宽
            if (hexing == 1361 || hexing == 1465 || hexing == 1467)
            {
                hexing = 1;
            }
            else
            {
                hexing = 2;
            }

            if (hexing == 1)
            {
                width_z = (length + 15) * 2 + height;//(length + 1.5) * 2 + height
            }
            else if (hexing == 2)
            {
                width_z = (length + 15) + height + (length / 2) + 20;//(length + 1.5) + height + (length / 2) + 2;

            }
            return length_z * width_z / 1000000;
        }

        #endregion

        #region 获取打样价格

        /// <summary>
        /// 获取--打样1-2价格
        /// </summary>
        /// <param name="length">长(mm)</param>
        /// <param name="width">宽(mm)</param>
        /// <param name="isfumo">覆膜1是，0否</param>
        /// <param name="count">数量</param>
        /// <param name="type">type:1彩色包装软盒打样,2:彩色瓦楞盒打样,3纸袋打样,4:纸袋打样2</param>
        /// <returns></returns>
        public static GoodsPriceResult GetDaYangPrice(double length, double width, int isfumo, int count, int type = 1, int goodsid = 0)
        {
            if (type == 1 || type == 2)
            {
                return GoodsService.GetDaYang1Price(length, width, isfumo, count, type, goodsid);
            }
            else //if (type == 3 || type == 4)
            {
                return GoodsService.GetDaYang2Price(length, width, count, type, goodsid);
            }
        }

        #endregion

        #region 打样1-2价格

        /// <summary>
        /// 获取--打样1-2价格
        /// </summary>
        /// <param name="length">长(mm)</param>
        /// <param name="width">宽(mm)</param>
        /// <param name="isfumo">覆膜1是，0否</param>
        /// <param name="count">数量</param>
        /// <param name="type">type:1彩色包装软盒打样,2:彩色瓦楞盒打样</param>
        /// <returns></returns>
        public static GoodsPriceResult GetDaYang1Price(double length, double width, int isfumo, int count, int type = 1, int goodsid = 0)
        {
            double area = length * width;
            return GetDaYang1Price(area, isfumo, count, type, goodsid);
        }
        /// <summary>
        /// 获取--打样1-2价格
        /// </summary>
        /// <param name="area">面积(平方毫米)</param>
        /// <param name="isfumo">覆膜1是，0否</param>
        /// <param name="count">数量</param>
        /// <param name="type">type:1彩色包装软盒打样,2:彩色瓦楞盒打样</param>
        /// <returns></returns>
        public static GoodsPriceResult GetDaYang1Price(double area, int isfumo, int count, int type = 1, int goodsid = 0)
        {
            GoodsPriceResult priceResult = new GoodsPriceResult { status = "success", msg = "", count = count, price = 0, shopprice = 0, costprice = 0, freight = 0, volume = 0, unitarea = 0, weight = 0 };
            DaYang1AreaPriceModel areaModel = new DaYang1AreaPriceModel();
            if (type == 1)
            {
                var list = DataConfig.DaYang1AreaPriceData.Where(m => m.UnitArea > area || m.UnitArea == area).OrderBy(m => m.UnitArea);//靠上取值
                if (list != null && list.Count() > 0)
                {
                    areaModel = list.First();
                }
                else
                {
                    areaModel = DataConfig.DaYang1AreaPriceData.OrderByDescending(m => m.UnitArea).First();//取最大值
                }
            }
            else
            {
                var list = DataConfig.DaYang2AreaPriceData.Where(m => m.UnitArea > area || m.UnitArea == area).OrderBy(m => m.UnitArea);//靠上取值
                if (list != null && list.Count() > 0)
                {
                    areaModel = list.First();
                }
                else
                {
                    areaModel = DataConfig.DaYang2AreaPriceData.OrderByDescending(m => m.UnitArea).First();//取最大值
                }
            }
            DaYang1CountPriceModel areaPriceModel = areaModel.CountPriceList.Where(m => m.Count < count || m.Count == count).OrderByDescending(m => m.Count).FirstOrDefault();
            UserShop shopModel = UserShopService.GetCurrentShop();
            decimal freight = GetGoodsHiddenShippingFee(goodsid, count, area / 1000000, shopModel);
            double shopPriceRate = 1.1;
            double clientPriceRate = 1.3;
            decimal price = 0;
            decimal shopprice = 0;

            priceResult.unitarea = area / 1000000;
            priceResult.count = count;
            priceResult.freight = freight;
            if (isfumo == 1)
            {
                priceResult.costprice = SiteService.ToDecimalPrice(areaPriceModel.CostPrice1 * count + freight);

            }
            else
            {
                priceResult.costprice = SiteService.ToDecimalPrice(areaPriceModel.CostPrice0 * count + freight);
            }
            shopprice = SiteService.ToDecimalPrice(priceResult.costprice * Convert.ToDecimal(shopPriceRate));
            priceResult.shopprice = shopprice;
            price = SiteService.ToDecimalPrice(priceResult.shopprice * Convert.ToDecimal(clientPriceRate));
            priceResult.price = GoodsService.GetFinalPrice(price, shopprice, goodsid);

            return priceResult;
        }

        #endregion

        #region 打样3-4价格

        /// <summary>
        /// 获取--打样3-4价格（3纸袋打样,4:纸袋打样2）
        /// </summary>
        /// <param name="length">长(mm)</param>
        /// <param name="width">宽(mm)</param>
        /// <param name="count">数量</param>
        /// <param name="type">3纸袋打样,4:纸袋打样2</param>
        /// <returns></returns>
        public static GoodsPriceResult GetDaYang2Price(double length, double width, int count, int type = 3, int goodsid = 0)
        {
            double area = length * width;
            return GetDaYang2Price(area, count, type, goodsid);
        }
        /// <summary>
        /// 获取--打样3-4价格
        /// </summary>
        /// <param name="area">面积(平方毫米)</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static GoodsPriceResult GetDaYang2Price(double area, int count, int type = 3, int goodsid = 0)
        {
            GoodsPriceResult priceResult = new GoodsPriceResult { status = "success", msg = "", count = count, price = 0, shopprice = 0, costprice = 0, freight = 0, volume = 0, unitarea = 0, weight = 0 };
            DaYang2AreaPriceModel areaModel = new DaYang2AreaPriceModel();
            if (type == 3)
            {
                var list = DataConfig.DaYang3AreaPriceData.Where(m => m.UnitArea > area || m.UnitArea == area).OrderBy(m => m.UnitArea);//靠上取值
                if (list != null && list.Count() > 0)
                {
                    areaModel = list.First();
                }
                else
                {
                    areaModel = DataConfig.DaYang3AreaPriceData.OrderByDescending(m => m.UnitArea).First();//取最大值
                }
            }
            else
            {
                var list = DataConfig.DaYang4AreaPriceData.Where(m => m.UnitArea > area || m.UnitArea == area).OrderBy(m => m.UnitArea);//靠上取值
                if (list != null && list.Count() > 0)
                {
                    areaModel = list.First();
                }
                else
                {
                    areaModel = DataConfig.DaYang4AreaPriceData.OrderByDescending(m => m.UnitArea).First();//取最大值
                }

            }
            DaYang2CountPriceModel areaPriceModel = areaModel.CountPriceList.Where(m => m.Count < count || m.Count == count).OrderByDescending(m => m.Count).FirstOrDefault();
            UserShop shopModel = UserShopService.GetCurrentShop();
            decimal freight = GetGoodsHiddenShippingFee(goodsid, count, area / 1000000, shopModel);
            double shopPriceRate = 1.1;
            double clientPriceRate = 1.3;
            decimal price = 0;
            decimal shopprice = 0;

            priceResult.unitarea = area / 1000000;
            priceResult.count = count;
            priceResult.freight = freight;
            priceResult.costprice = SiteService.ToDecimalPrice(areaPriceModel.CostPrice * count + freight);
            shopprice = SiteService.ToDecimalPrice(priceResult.costprice * Convert.ToDecimal(shopPriceRate));
            priceResult.shopprice = shopprice;
            price = SiteService.ToDecimalPrice(priceResult.shopprice * Convert.ToDecimal(clientPriceRate));
            priceResult.price = GoodsService.GetFinalPrice(price, shopprice, goodsid);

            return priceResult;
        }

        #endregion

        #region UV彩盒--数量+单价

        /// <summary>
        /// 获取UV价格
        /// </summary>
        /// <param name="length">长(mm)</param>
        /// <param name="width">宽(mm)</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static GoodsPriceResult GetUVPrice(double length, double width, int count, int goodsid = 0)
        {
            double area = length * width / 1000000;
            return GetUVPrice(area, count, goodsid);
        }
        /// <summary>
        /// 获取UV价格
        /// </summary>
        /// <param name="unitArea">面积(平方米)</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static GoodsPriceResult GetUVPrice(double unitArea, int count, int goodsid = 0)
        {
            List<PriceResultModel> list = XlsPriceData.GetUVPriceList();
            return GetPriceModelByUnitArea(unitArea, count, goodsid, list);
        }

        #endregion

        #region 获取PVC异形卡价格

        /// <summary>
        /// 获取PVC异形卡价格
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="count"></param>
        /// <param name="cz_pvid">材质类型：PVC亮光卡，PVC哑光卡，PVC磨砂卡</param>
        /// <returns></returns>
        public static GoodsPriceResult GetPVCPrice(double length, double width, Int32 count, int cz_pvid, int goodsid = 0)
        {
            //decimal clientPrice = 0;
            string type = "lg";
            if (cz_pvid == 1839)
            {
                type = "lg";
            }
            else if (cz_pvid == 1840)
            {
                type = "yg";
            }
            else if (cz_pvid == 1841)
            {
                type = "ms";
            }
            return GetPVCPrice(length, width, count, type, goodsid);
        }

        /// <summary>
        /// 获取不干胶价格
        /// </summary>
        /// <param name="length">长 mm</param>
        /// <param name="width">宽 mm</param>
        /// <param name="count">数量</param>
        /// <param name="type">材质类型：PVC亮光卡，PVC哑光卡，PVC磨砂卡</param>
        /// <returns></returns>
        public static GoodsPriceResult GetPVCPrice(double length, double width, int count, string type, int goodsid = 0)
        {
            return GetPVCPrice(length * width / 1000000, count, type, goodsid);
        }

        /// <summary>
        /// 获取不干胶价格
        /// </summary>
        /// <param name="unitArea">单件展开面积（平方米）</param>
        /// <param name="count">数量</param>
        /// <param name="type">材质类型：PVC亮光卡，PVC哑光卡，PVC磨砂卡</param>
        /// <returns></returns>
        public static GoodsPriceResult GetPVCPrice(double unitArea, int count, string type, int goodsid = 0)
        {
            List<PriceResultModel> list = XlsPriceData.GetPVCPriceList(type);
            return GetPriceModelByUnitArea(unitArea, count, goodsid, list);
        }


        #endregion

        #region 获取产品价格结果

        #region 现货
        /// <summary>
        /// 获取产品价格结果——现货
        /// </summary>
        /// <param name="goodsid"></param>
        /// <param name="skuid"></param>
        /// <param name="goodscount"></param>
        /// <returns></returns>
        public static GoodsPriceResult GetPrice(int goodsid = 0, int skuid = 0, int goodscount = 0)
        {
            List<GoodsSKUPrice> list = null;
            GoodsSKUPrice existModel = null;
            GoodsPriceResult jsonData = new GoodsPriceResult { status = "success", msg = "", count = goodscount, price = 0, shopprice = 0, costprice = 0, freight = 0, volume = 0, unitarea = 0, weight = 0 };
            bool isHasSKUPrice = false;//是否包含sku价格配置
            UserShop shopModel = UserShopService.GetCurrentShop();
            //门店所属定价区域
            GoodsPriceArea priceAreaModel = GoodsPriceAreaService.GetGoodsPriceArea(shopModel);
            //没有所属定价区域，查询默认价格配置
            if (priceAreaModel == null || priceAreaModel.ID == 0)
            {
                //默认——价格配置 2018.04.11调整默认价格区域是江浙沪使用，其他地区不使用；
                //list = work.Context.GoodsSKUPrices.Where(m => m.SKUID == skuid && m.GoodsPriceAreaID == 0).OrderByDescending(m => m.ID).ToList();
            }
            else
            {
                //指定定价区域——价格配置
                list = work.Context.GoodsSKUPrices.Where(m => m.SKUID == skuid && m.GoodsPriceAreaID == priceAreaModel.ID).OrderByDescending(m => m.ID).ToList();
                //if (list == null || list.Count < 1)
                //{
                //默认——价格配置
                //2018.04.11取消默认价格配置
                //list = work.Context.GoodsSKUPrices.Where(m => m.SKUID == skuid && m.GoodsPriceAreaID == 0).OrderByDescending(m => m.ID).ToList();
                //}
            }

            if (list == null || list.Count < 1)
            {
                //没有价格配置
            }
            else
            {
                isHasSKUPrice = true;
                existModel = list.Where(m => m.Count > goodscount || m.Count == goodscount).OrderBy(m => m.Count).FirstOrDefault();
                if (existModel == null)
                {
                    existModel = list.OrderByDescending(m => m.Count).FirstOrDefault();
                }
            }
            if (isHasSKUPrice && existModel != null)
            {
                //有价格配置，不计单独算隐藏运费
                jsonData.price = SiteService.ToDecimalPrice(GoodsService.GetFinalPrice(existModel.ClientPrice / existModel.Count, existModel.ShopPrice / existModel.Count, goodsid) * goodscount);
                jsonData.shopprice = SiteService.ToDecimalPrice(existModel.ShopPrice / existModel.Count * goodscount);
                jsonData.costprice = SiteService.ToDecimalPrice(existModel.CostPrice / existModel.Count * goodscount);
                jsonData.freight = SiteService.ToDecimalPrice(existModel.Freight / existModel.Count * goodscount);

                #region 计算重量体积
                GoodsSKU skuModel = null;
                GoodsWeightVolume weightVolumeModel = new GoodsWeightVolume();
                if (skuid == 0)
                {
                    jsonData.status = "error";
                    jsonData.msg = "skuid:0";
                }
                else
                {
                    skuModel = work.Context.GoodsSKUs.AsNoTracking().Where(m => m.ID == skuid).FirstOrDefault();
                    if (skuModel == null)
                    {
                        jsonData.status = "error";
                        jsonData.msg = "skuModel:null";
                    }
                    else
                    {
                        Goods goodsModel = work.Context.Goods.AsNoTracking().Where(m => m.ID == goodsid).First();
                        weightVolumeModel = GetGoodsWeightVolume(goodsModel, skuModel);
                        jsonData.weight = weightVolumeModel.UnitWeight;
                        jsonData.volume = weightVolumeModel.UnitVolume;
                    }
                }
                #endregion

                Log.WriteLog(string.Format("单品:" + goodsid + "--存在SKU价格配置：{0}，ShopPriceRate:{1},ClientPriceRate:{2} \n", JsonHelper.SerializeObject(jsonData), existModel.ShopPriceRate, existModel.ClientPriceRate), "fee", DateTime.Now.ToString("yyyyMMdd"));
            }
            else
            {
                GoodsSKU skuModel = null;
                if (skuid == 0)
                {
                    decimal freight = 0;
                    decimal costprice = 0;
                    jsonData.status = "error";
                    jsonData.msg = "skuid:0";

                    Goods goodsModel = work.Context.Goods.AsNoTracking().Where(m => m.ID == goodsid).First();
                    if (goodsModel != null)
                    {
                        if (goodsModel.G_IsDesign != 1)//非设计，计算运费
                        {
                            GoodsWeightVolume weightVolumeModel = new GoodsWeightVolume();
                            weightVolumeModel = GetGoodsWeightVolume(goodsModel, skuModel);

                            jsonData.weight = weightVolumeModel.UnitWeight;
                            jsonData.volume = weightVolumeModel.UnitVolume;
                            freight = GetGoodsHiddenShippingFee(goodsModel, skuModel, goodscount, shopModel);
                        }

                        costprice = SiteService.ToDecimalPrice(goodsModel.G_Price * goodscount);

                        jsonData.freight = freight;
                        jsonData.costprice = costprice;
                        jsonData.shopprice = costprice + freight;
                        jsonData.price = costprice + freight;//SiteService.ToDecimalPrice(GoodsService.GetFinalPrice(price, shopprice, goodsid));
                    }
                }
                else
                {
                    skuModel = work.Context.GoodsSKUs.AsNoTracking().Where(m => m.ID == skuid).FirstOrDefault();
                    if (skuModel == null)
                    {
                        jsonData.status = "error";
                        jsonData.msg = "skuModel:null";
                    }
                    else
                    {
                        decimal freight = 0;
                        decimal costprice = 0;
                        decimal shopprice = 0;
                        decimal price = 0;
                        Goods goodsModel = work.Context.Goods.AsNoTracking().Where(m => m.ID == goodsid).First();
                        if (goodsModel.G_IsDesign != 1)//非设计，计算运费
                        {
                            GoodsWeightVolume weightVolumeModel = new GoodsWeightVolume();
                            weightVolumeModel = GetGoodsWeightVolume(goodsModel, skuModel);

                            jsonData.weight = weightVolumeModel.UnitWeight;
                            jsonData.volume = weightVolumeModel.UnitVolume;
                            freight = GetGoodsHiddenShippingFee(goodsModel, skuModel, goodscount, shopModel);
                        }
                        if (skuModel.SKU_CostPrice != 0)
                        {
                            costprice = SiteService.ToDecimalPrice(skuModel.SKU_CostPrice * goodscount);
                            if (skuModel.SKU_ShopPriceRate == 0)
                            {
                                skuModel.SKU_ShopPriceRate = Convert.ToDouble(skuModel.SKU_ShopPrice / skuModel.SKU_CostPrice);
                            }
                            shopprice = SiteService.ToDecimalPrice((costprice + freight) * Convert.ToDecimal(skuModel.SKU_ShopPriceRate));
                            if (skuModel.SKU_ClientPriceRate == 0)
                            {
                                skuModel.SKU_ClientPriceRate = Convert.ToDouble(skuModel.SKU_Price / skuModel.SKU_ShopPrice);
                            }
                            price = SiteService.ToDecimalPrice(shopprice * Convert.ToDecimal(skuModel.SKU_ClientPriceRate));
                        }
                        else if (skuModel.SKU_ShopPrice != 0)
                        {
                            costprice = SiteService.ToDecimalPrice(skuModel.SKU_ShopPrice * goodscount);
                            shopprice = SiteService.ToDecimalPrice(skuModel.SKU_ShopPrice * goodscount + freight);
                            price = SiteService.ToDecimalPrice(skuModel.SKU_Price * goodscount + freight);
                        }
                        else if (skuModel.SKU_Price != 0)
                        {
                            costprice = SiteService.ToDecimalPrice(skuModel.SKU_Price * goodscount);
                            shopprice = SiteService.ToDecimalPrice(skuModel.SKU_Price * goodscount + freight);
                            price = SiteService.ToDecimalPrice(skuModel.SKU_Price * goodscount + freight);
                        }

                        jsonData.freight = freight;
                        jsonData.costprice = costprice;
                        jsonData.shopprice = shopprice;
                        jsonData.price = SiteService.ToDecimalPrice(GoodsService.GetFinalPrice(price, shopprice, goodsid));

                        Log.WriteLog(string.Format("单品:" + goodsid + "--计算邮费价格：{0}，SKU_ShopPriceRate:{1},SKU_ClientPriceRate:{2} \n", JsonHelper.SerializeObject(jsonData), skuModel.SKU_ShopPriceRate, skuModel.SKU_ClientPriceRate), "fee", DateTime.Now.ToString("yyyyMMdd"));
                    }
                }
            }

            return jsonData;
        }
        #endregion

        #region 定制产品
        /// <summary>
        /// 根据单品面积和购买数量，获取产品的当前价格实体 ——定制产品
        /// </summary>
        /// <param name="unitArea"></param>
        /// <param name="count"></param>
        /// <param name="goodsid"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static GoodsPriceResult GetPriceModelByUnitArea(double unitArea, int count, int goodsid, List<PriceResultModel> list)
        {
            GoodsPriceResult priceResult = new GoodsPriceResult { status = "success", msg = "", count = count, price = 0, shopprice = 0, costprice = 0, freight = 0, volume = 0, unitarea = 0, weight = 0 };

            //确定面积范围
            PriceResultModel areaPriceModel = list.Where(m => m.UnitArea >= unitArea).OrderBy(m => m.UnitArea).FirstOrDefault();
            if (areaPriceModel == null)//配置里没有比unitArea更多面积，取最大面积
            {
                areaPriceModel = list.OrderByDescending(m => m.UnitArea).FirstOrDefault();
            }

            //根据数量确定价格
            decimal clientPrice = 0;
            decimal costPrice = 0;
            decimal costTotalPrice = 0;
            decimal shopPrice = 0;
            decimal freight = 0;
            UserShop shopModel = UserShopService.GetCurrentShop();

            PriceResultModel priceModel = list.Where(m => m.UnitArea == areaPriceModel.UnitArea).Where(m => m.Count >= count).OrderBy(m => m.Count).FirstOrDefault();
            if (priceModel == null)//配置里没有比count大，取最大数量
            {
                priceModel = list.Where(m => m.UnitArea == areaPriceModel.UnitArea).OrderByDescending(m => m.Count).FirstOrDefault();
            }
            Log.WriteLog(JsonHelper.SerializeObject(areaPriceModel) + "\n" + JsonHelper.SerializeObject(priceModel) + "\n", "feibiao", DateTime.Now.ToString("yyyyMMdd"));

            Goods goodsModel = work.Context.Goods.AsNoTracking().Where(m => m.ID == goodsid).First();
            GoodsWeightVolume weightVolumeModel = new GoodsWeightVolume();
            weightVolumeModel = GetGoodsWeightVolume(goodsModel, null, unitArea);

            priceResult.weight = weightVolumeModel.UnitWeight;
            priceResult.volume = weightVolumeModel.UnitVolume;

            //配置中的运费，改成系统计算
            freight = GetGoodsHiddenShippingFee(goodsid, count, unitArea, shopModel);
            if (freight == 0)
            {
                //使用配置中的运费
                freight = SiteService.ToDecimalPrice(priceModel.Freight / priceModel.Count * count);
            }
            if (priceModel.Type == Convert.ToInt16(DataConfig.PriceModelTypeEnum.UV) || priceModel.Type == Convert.ToInt16(DataConfig.PriceModelTypeEnum.PVC) )
            {
                costPrice = SiteService.ToDecimalPrice(priceModel.SquareCostPrice * Convert.ToDecimal(unitArea) * count * goodsModel.G_UnitCount);
                if (costPrice < priceModel.MinCostPrice)
                {
                    costPrice = priceModel.MinCostPrice;
                }
            }
            else
            {
                costPrice = SiteService.ToDecimalPrice(priceModel.CostPrice / priceModel.Count * count);
            }
            costTotalPrice = costPrice + freight;
            shopPrice = SiteService.ToDecimalPrice((costPrice + freight) * Convert.ToDecimal(priceModel.ShopPriceRate));
            clientPrice = SiteService.ToDecimalPrice(shopPrice * Convert.ToDecimal(priceModel.ClientPriceRate));
            if (clientPrice < priceModel.MinClientPrice)
            {
                clientPrice = priceModel.MinClientPrice;
            }

            priceResult.unitarea = unitArea;
            priceResult.count = count;
            priceResult.freight = freight;
            priceResult.costprice = costPrice;
            priceResult.shopprice = shopPrice;
            priceResult.price = GoodsService.GetFinalPrice(clientPrice, shopPrice, goodsid);

            return priceResult;
        }
        #endregion

        #endregion

        #region 获取单品- 隐藏运费（门店和仓库同区域）仓库配送至门店的费用

        /// <summary>
        /// 获取单品- 隐藏运费（门店和仓库同区域）仓库配送至门店的费用
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="skuModel">产品ID</param>
        /// <param name="skucount">件数</param>
        /// <param name="shopModel"></param>
        /// <returns></returns>
        public static decimal GetGoodsHiddenShippingFee(int goodsID, GoodsSKU skuModel, int skucount, UserShop shopModel)
        {
            Goods goodsModel = null;
            if (goodsID != 0)
            {
                goodsModel = work.Context.Goods.AsNoTracking().Where(m => m.ID == goodsID).FirstOrDefault();
                if (goodsModel == null || goodsModel.G_IsFreeShipping == 1)
                    return 0;
            }
            else
            {
                return 0;
            }
            return GetGoodsHiddenShippingFee(goodsModel, skuModel, skucount, shopModel);
        }
        /// <summary>
        /// 获取单品- 隐藏运费（门店和仓库同区域）仓库配送至门店的费用
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="skuModel">产品ID</param>
        /// <param name="skucount">件数</param>
        /// <param name="shopModel"></param>
        /// <returns></returns>
        public static decimal GetGoodsHiddenShippingFee(Goods goodsModel, GoodsSKU skuModel, int skucount, UserShop shopModel)
        {
            decimal totalFee = 0;
            double g_weight = 0;//单个重量(KG)
            double g_volume = 0;//单个体积(m³)
            double w_totalWeight = 0;//重货总重量(KG)
            double l_totalWeight = 0;//泡货总重量(KG)
            double l_totalVolume = 0;//泡货总体积(m³)
            int warehouseID = 0;//仓库
            int supplierID = 0;//供应商
            ShippingArea areaModel = null;
            int skuID = 0;

            if (goodsModel == null || goodsModel.G_IsFreeShipping == 1)
                return 0;

            if (skuModel != null)
            {
                skuID = skuModel.ID;
            }

            #region 分配仓库 + 供应商,确定运送区域

            if (goodsModel.G_IsExist == 1)
            {
                warehouseID = WarehouseService.GetWarehouseID(shopModel, goodsModel.ID);
                Warehouse warehouseModel = work.WarehouseRepository.GetByID(warehouseID);

                Log.WriteLog("单品:" + goodsModel.G_Name + "--仓库：" + warehouseModel.Name, "fee", DateTime.Now.ToString("yyyyMMdd"));
                areaModel = ShippingAreaService.GetShippingArea(warehouseModel.ShippingTemplateID, shopModel);
            }
            else
            {
                supplierID = SupplierService.GetSupplierID(shopModel, goodsModel);
                Supplier supplierModel = work.Context.Suppliers.AsNoTracking().Where(m => m.ID == supplierID).First();
                Log.WriteLog("单品:" + goodsModel.G_Name + "--供应商：" + supplierModel.Sup_Name, "fee", DateTime.Now.ToString("yyyyMMdd"));

                areaModel = ShippingAreaService.GetShippingArea(supplierModel.ShippingTemplateID, shopModel);
            }

            ////同区域计算隐藏运费，否则返回0
            //if (WarehouseAreaService.CheckIsSameArea(warehouseID, shopModel))
            //{
            if (areaModel == null)
            {
                Log.WriteLog("单品：未查询到运费区域", "fee", DateTime.Now.ToString("yyyyMMdd"));
                return 0;
            }
            else
            {
                Log.WriteLog("单品：运费区域:" + areaModel.SA_Title, "fee", DateTime.Now.ToString("yyyyMMdd"));
            }

            #endregion

            #region 计算重量与体积

            GoodsWeightVolume weightVolumeModel = GetGoodsWeightVolume(goodsModel, skuModel);
            g_weight = weightVolumeModel.UnitWeight;
            g_volume = weightVolumeModel.UnitVolume;

            //重货
            if (goodsModel.G_IsWeight == 1)
            {
                w_totalWeight = w_totalWeight + skucount * g_weight;
            }
            else//泡货
            {
                l_totalWeight = l_totalWeight + skucount * g_weight;
                l_totalVolume = l_totalVolume + skucount * g_volume;
            }

            #endregion

            totalFee = ShippingAreaService.GetShippingFee(w_totalWeight, l_totalWeight, l_totalVolume, areaModel);

            return SiteService.ToDecimalPrice(totalFee);
            //}
            //else
            //{
            //    Log.WriteLog("单品:" + goodsModel.G_Name + "--门店仓库不同区域", "fee", DateTime.Now.ToString("yyyyMMdd"));
            //    return 0;
            //}
        }

        /// <summary>
        /// 获取-非标品-单品- 隐藏运费（门店和仓库同区域）仓库配送至门店的费用
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="goodsCount">件数</param>
        /// <param name="unitArea">单个展开面积(m²)</param>
        /// <param name="shopModel"></param>
        /// <returns></returns>
        public static decimal GetGoodsHiddenShippingFee(int goodsID, int goodsCount, double unitArea, UserShop shopModel)
        {
            Goods goodsModel = null;
            if (goodsID != 0)
            {
                goodsModel = work.Context.Goods.AsNoTracking().Where(m => m.ID == goodsID).FirstOrDefault();
                if (goodsModel == null || goodsModel.G_IsFreeShipping == 1)
                    return 0;
            }
            else
            {
                return 0;
            }
            return GetGoodsHiddenShippingFee(goodsModel, goodsCount, unitArea, shopModel);
        }
        /// <summary>
        /// 获取-非标品-单品- 隐藏运费（门店和仓库同区域）仓库配送至门店的费用
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="goodsCount">件数</param>
        /// <param name="unitArea">单个展开面积(m²)</param>
        /// <param name="shopModel"></param>
        /// <returns></returns>
        public static decimal GetGoodsHiddenShippingFee(Goods goodsModel, int goodsCount, double unitArea, UserShop shopModel)
        {
            decimal totalFee = 0;
            double g_weight = 0;//单个重量(KG)
            double g_volume = 0;//单个体积(m³)
            double g_expandarea = unitArea;//单个展开面积(m²)
            //double g_squareweight = 0;//单个平方克重(g)
            double w_totalWeight = 0;//重货总重量(KG)
            double l_totalWeight = 0;//泡货总重量(KG)
            double l_totalVolume = 0;//泡货总体积(m³)
            int warehouseID = 0;//仓库
            int supplierID = 0;//供应商
            ShippingArea areaModel = null;
            int skuID = 0;

            if (goodsModel == null || goodsModel.G_IsFreeShipping == 1)
                return 0;

            #region 分配仓库 + 供应商,确定运送区域

            if (goodsModel.G_IsExist == 1)
            {
                warehouseID = WarehouseService.GetWarehouseID(shopModel, goodsModel.ID);
                Warehouse warehouseModel = work.WarehouseRepository.GetByID(warehouseID);

                Log.WriteLog("非标单品:" + goodsModel.G_Name + "--仓库：" + warehouseModel.Name, "fee", DateTime.Now.ToString("yyyyMMdd"));
                areaModel = ShippingAreaService.GetShippingArea(warehouseModel.ShippingTemplateID, shopModel);
            }
            else
            {
                supplierID = SupplierService.GetSupplierID(shopModel, goodsModel);
                Supplier supplierModel = work.Context.Suppliers.AsNoTracking().Where(m => m.ID == supplierID).First();
                Log.WriteLog("非标单品:" + goodsModel.G_Name + "--供应商：" + supplierModel.Sup_Name, "fee", DateTime.Now.ToString("yyyyMMdd"));

                areaModel = ShippingAreaService.GetShippingArea(supplierModel.ShippingTemplateID, shopModel);
            }

            ////同区域计算隐藏运费，否则返回0
            //if (WarehouseAreaService.CheckIsSameArea(warehouseID, shopModel))
            //{
            if (areaModel == null)
            {
                Log.WriteLog("非标单品：未查询到运费区域", "fee", DateTime.Now.ToString("yyyyMMdd"));
                return 0;
            }
            else
            {
                Log.WriteLog("非标单品：运费区域:" + areaModel.SA_Title, "fee", DateTime.Now.ToString("yyyyMMdd"));
            }

            #endregion

            #region 计算重量与体积

            ////g_weight = goodsModel.G_Weight;
            //g_squareweight = goodsModel.G_SquareWeight;
            //g_volume = goodsModel.G_Volume;
            //if (g_squareweight != 0)
            //{
            //    g_weight = g_squareweight * g_expandarea / 1000;
            //}
            GoodsWeightVolume weightVolumeModel = new GoodsWeightVolume();
            weightVolumeModel = GetGoodsWeightVolume(goodsModel, null, unitArea);

            g_weight = weightVolumeModel.UnitWeight;
            g_volume = weightVolumeModel.UnitVolume;

            //重货
            if (goodsModel.G_IsWeight == 1)
            {
                w_totalWeight = w_totalWeight + goodsCount * g_weight;
            }
            else//泡货
            {
                l_totalWeight = l_totalWeight + goodsCount * g_weight;
                l_totalVolume = l_totalVolume + goodsCount * g_volume;
            }

            #endregion

            totalFee = ShippingAreaService.GetShippingFee(w_totalWeight, l_totalWeight, l_totalVolume, areaModel);

            return SiteService.ToDecimalPrice(totalFee);
            //}
            //else
            //{
            //    Log.WriteLog("单品:" + goodsModel.G_Name + "--门店仓库不同区域", "fee", DateTime.Now.ToString("yyyyMMdd"));
            //    return 0;
            //}
        }


        #endregion

        #region 计算单个产品的重量、体积、面积、平方克重

        /// <summary>
        /// 计算单件重量及体积
        /// </summary>
        /// <param name="goodsModel"></param>
        /// <param name="skuModel"></param>
        /// <param name="unitArea">默认0 现货，是否为指定现货的面积</param>
        /// <returns></returns>
        public static GoodsWeightVolume GetGoodsWeightVolume(Goods goodsModel, GoodsSKU skuModel, double unitArea = 0)
        {
            double g_weight = 0;//单个重量(KG)
            double g_volume = 0;//单个体积(m³)
            double g_expandarea = 0;//单个展开面积(m²)
            double g_squareweight = 0;//单个平方克重(g)

            if (goodsModel != null)
            {
                if (unitArea == 0)
                { //现货
                    g_weight = goodsModel.G_Weight * goodsModel.G_UnitCount;
                    g_volume = goodsModel.G_Volume * goodsModel.G_UnitCount;
                    g_expandarea = goodsModel.G_ExpandArea;
                    g_squareweight = goodsModel.G_SquareWeight;
                    if (g_weight == 0)
                    {
                        g_weight = g_squareweight * g_expandarea * goodsModel.G_UnitCount / 1000;
                    }

                    if (skuModel != null)
                    {
                        if (skuModel.SKU_Weight > 0)
                        {
                            g_weight = skuModel.SKU_Weight * goodsModel.G_UnitCount;
                        }

                        if (skuModel.SKU_ExpandArea > 0 && skuModel.SKU_SquareWeight > 0)
                        {
                            if (g_weight == 0)
                            {
                                g_weight = skuModel.SKU_ExpandArea * skuModel.SKU_SquareWeight / 1000;
                            }
                            g_expandarea = skuModel.SKU_ExpandArea;
                            g_squareweight = skuModel.SKU_SquareWeight;
                        }

                        if (skuModel.SKU_Volume > 0)
                        {
                            g_volume = skuModel.SKU_Volume * goodsModel.G_UnitCount;
                        }
                    }
                }
                else//定制
                {
                    //g_weight = goodsModel.G_Weight;
                    g_squareweight = goodsModel.G_SquareWeight;
                    g_volume = goodsModel.G_Volume * goodsModel.G_UnitCount;
                    if (g_squareweight != 0)
                    {
                        g_weight = g_squareweight * unitArea * goodsModel.G_UnitCount / 1000;
                    }

                }
            }
            return new GoodsWeightVolume { UnitWeight = g_weight, UnitVolume = g_volume };
        }

        #endregion
    }
}
