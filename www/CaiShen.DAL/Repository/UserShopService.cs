using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;
using System.Web;

namespace Pannet.DAL.Repository
{
    public class UserShopService
    {
        private static UnitOfWork work = new UnitOfWork();
        /// <summary>
        /// 主店铺ID
        /// </summary>
        public static int defaultShopID = 20;
        #region 获得店铺编号

        /// <summary>
        /// 获得店铺编号 格式：ShopID(5)
        /// </summary>
        /// <param name="UserShopID"></param>
        /// <returns></returns>
        public static string GetShopNumber(int UserShopID)
        {
            return UserShopID.ToString().PadLeft(5, '0');
        }

        #endregion

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static UserShop GetModel(int ID)
        {
            var model = work.UserShopRepository.Get(m => m.ID == ID, null).FirstOrDefault();
            if (model != null)
            {
                return model;
            }
            return new UserShop();
        }
        /// <summary>
        /// 获取门店
        /// </summary>
        /// <param name="ID">不包含门店ID，0取所有门店</param>
        /// <returns></returns>
        public static List<UserShop> GetModels(int ID)
        {
            if (ID == 0)
            {
                return work.Context.UserShops.AsNoTracking().Where(m => m.Shop_Is_Enable == 1).OrderByDescending(m => m.ID).ToList();
            }
            else
            {
                return work.Context.UserShops.AsNoTracking().Where(m => m.Shop_Is_Enable == 1 && m.ID != ID).OrderByDescending(m => m.ID).ToList();
            }
        }

        #endregion

        #region 当前登录店铺信息

        /// <summary>
        /// 当前登录店铺
        /// </summary>
        /// <returns></returns>
        public static UserShop GetCurrentShop()
        {
            string domain = GetCurrentShopDomain();
            object cacheShop = GetCacheShop(domain);
            if (cacheShop == null)
            {
                UserShop currentShop = work.Context.UserShops.Where(m => m.Shop_URL == domain && m.Shop_Is_Enable == 1).FirstOrDefault();
                if (currentShop == null || currentShop.ID == 0)
                {
                    currentShop = work.Context.UserShops.Where(m => m.ID == defaultShopID).FirstOrDefault();
                }
                SetCacheShop(domain, currentShop);

                return currentShop;
            }
            else
            {
                return cacheShop as UserShop;
            }
        }
        /// <summary>
        /// 获取当前店铺ID
        /// </summary>
        /// <returns>0主站</returns>
        public static int GetCurrentShopID()
        {
            return GetCurrentShop().ID;
            //string domain = GetCurrentShopDomain();
            //object cacheShopID = GetCacheShopID(domain);
            //if (cacheShopID == null)
            //{
            //    int shopID = 3;
            //    UserShop currentShop = work.Context.UserShops.Where(m => m.Shop_URL == domain).FirstOrDefault();
            //    if (currentShop != null && currentShop.ID != 0)
            //    {
            //        shopID = currentShop.ID;
            //    }
            //    SetCacheShopID(domain, shopID);

            //    return shopID;
            //}
            //else
            //{
            //    return Convert.ToInt32(cacheShopID);
            //}
        }

        #region 店铺缓存

        /// <summary>
        /// 设置当前门店ID缓存
        /// </summary>
        /// <param name="domain">domain</param>
        /// <param name="shopID">shopID</param>
        /// <returns></returns>
        public static void SetCacheShopID(string domain, int shopID)
        {
            Utility.DataCache.SetCache("shopid_" + domain, shopID);
        }

        /// <summary>
        /// 获取门店ID缓存
        /// </summary>
        /// <param name="domain">domain</param>
        /// <returns></returns>
        public static object GetCacheShopID(string domain)
        {
            return Utility.DataCache.GetCache("shopid_" + domain);
        }


        /// <summary>
        /// 获取门店缓存
        /// </summary>
        /// <param name="domain">domain</param>
        /// <returns></returns>
        public static object GetCacheShop(string domain)
        {
            return Utility.DataCache.GetCache("shop_" + domain);
        }

        /// <summary>
        /// 设置当前门店缓存
        /// </summary>
        /// <param name="domain">domain</param>
        /// <param name="shop">shop</param>
        /// <returns></returns>
        public static void SetCacheShop(string domain, UserShop shop)
        {
            Utility.DataCache.SetCache("shop_" + domain, shop);
        }


        #endregion


        /// <summary>
        /// 获取当前店铺 主机名不含.test.com
        /// </summary>
        /// <returns>0主站</returns>
        public static string GetCurrentShopDomain()
        {
            string domain = HttpContext.Current.Request.Url.Host;
            if (!domain.Contains("."))
            {
                return "www";
            }
            return domain.Substring(0, domain.IndexOf("."));
        }
        /// <summary>
        /// 获取店铺完整url地址
        /// </summary>
        /// <param name="shopurl">店铺域名主机名</param>
        /// <returns></returns>
        public static string GetShopDomainFull(string shopurl)
        {
            return string.Format("http://{0}.{1}", shopurl, WebSiteConfig.WebSiteMainDomain);
        }
        /// <summary>
        /// 当前店铺预付款定金比列
        /// </summary>
        /// <returns></returns>
        public static double GetCurrentShopYufuPercent()
        {
            UserShop currentShop = GetCurrentShop();// GetModel(shopID);
            if (currentShop.Shop_Yufu_Percent == 0)
            {
                return WebSiteConfig.Shop_Yufu_Percent;
            }
            else
            {
                return currentShop.Shop_Yufu_Percent;
            }
        }

        /// <summary>
        /// 当前店铺是否支持预付定金
        /// </summary>
        /// <returns></returns>
        public static bool IsYufu()
        {
            UserShop currentShop = GetCurrentShop();
            if (currentShop.Shop_IsYufu == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 当前店铺,是否支持指定金额的订单 预付定金
        /// </summary>
        /// <param name="payAmount"></param>
        /// <returns></returns>
        public static bool IsYufu(decimal payAmount)
        {
            UserShop currentShop = GetCurrentShop();
            if (currentShop.Shop_IsYufu == 0)
            {
                return false;
            }
            if (payAmount > currentShop.RestGuaranteeMoney)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否支持货到付款
        /// </summary>
        /// <returns></returns>
        public static bool IsDaofu()
        {
            UserShop currentShop = GetCurrentShop();
            if (currentShop.Shop_IsDaofu == 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 当前店铺,是否支持货到付款
        /// </summary>
        /// <param name="payAmount"></param>
        /// <param name="shopID">当前店铺ID</param>
        /// <returns></returns>
        public static bool IsDaofu(decimal payAmount)
        {
            UserShop currentShop = GetCurrentShop();
            if (currentShop.Shop_IsDaofu == 0)
            {
                return false;
            }
            if (payAmount > currentShop.RestGuaranteeMoney)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 获得商品数量

        /// <summary>
        /// 获得出售中商品数量
        /// </summary>
        /// <param name="userShopID">门店ID</param>
        /// <returns></returns>
        public static int GetCountOfInSell(int userShopID)
        {
            var rst = work.Context.Goods.Where(m => m.UserShopID == 0);

            //下架商品ID
            List<int> rst_down_goodsids = work.Context.UserShopGoodsDowns.Where(m => m.ShopID == userShopID).Select(m => m.GoodsID).ToList();

            return rst.Where(m => !rst_down_goodsids.Contains(m.ID)).OrderByDescending(m => m.ID).Count();
        }

        #endregion

        #region 判断收货地和当前门店是否同一个区域
        /// <summary>
        /// 判断收货地和当前门店是否同一个区域
        /// </summary>
        /// <param name="userAddressID">用户地址ID</param>
        /// <returns></returns>
        public static bool CheckIsSameArea(int userAddressID)
        {
            //地址模板
            UserAddress addressModel = work.UserAddressRepository.GetByID(userAddressID);
            if (addressModel == null)
                return false;
            return CheckIsSameArea(addressModel);
        }
        /// <summary>
        /// 判断收货地和当前门店是否同一个区域
        /// </summary>
        /// <param name="addressModel">用户地址</param>
        /// <returns></returns>
        public static bool CheckIsSameArea(UserAddress addressModel)
        {
            UserShop shopModel = UserShopService.GetCurrentShop();
            //地址模板
            if (addressModel == null)
                return false;
            //门店所属定价区域
            GoodsPriceArea priceAreaModel = GoodsPriceAreaService.GetGoodsPriceArea(shopModel);
            Log.WriteLog(string.Format("收货地是否和门店同一个区域：{0}\n{1}\n{2} \n", shopModel.Shop_Name + shopModel.Shop_Province + ":" + shopModel.Shop_City, addressModel.ID + ":" + addressModel.Province + ":" + addressModel.City, JsonHelper.SerializeObject(priceAreaModel)), "fee", DateTime.Now.ToString("yyyyMMdd"));
            if (priceAreaModel != null && priceAreaModel.ID != 0)
            {
                if (priceAreaModel.AreaIds.Contains(string.Format(",{0},", addressModel.Province)) || priceAreaModel.AreaIds.Contains(string.Format(",{0},", addressModel.City)))
                {
                    return true;
                }
            }
            else
            {
                //或者同一个城市或同一个省份
                if (shopModel.Shop_City == addressModel.City || shopModel.Shop_Province == addressModel.Province)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
