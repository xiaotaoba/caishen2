using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserShopGoodsSetService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取店铺产品折扣

        /// <summary>
        /// 获取店铺产品折扣
        /// </summary>
        /// <param name="ShopID"></param>
        /// <param name="GoodsID"></param>
        /// <returns></returns>
        public static double GetGoodsDiscount(int ShopID, int GoodsID)
        {
            var rst = work.UserShopGoodsSetRepository.Get(m => m.ShopID == ShopID && m.GoodsID == GoodsID).ToList();
            if (rst != null && rst.Count > 0)
            {
                if (rst[0].UG_Discount == 0)
                {
                    return 1;
                }
                else
                {
                    return rst[0].UG_Discount;
                }
            }
            return 1;
        }
        ///// <summary>
        ///// 获取当前店铺产品折扣
        ///// </summary>
        ///// <param name="ShopID"></param>
        ///// <param name="GoodsID"></param>
        ///// <returns></returns>
        //public static double GetGoodsDiscount(int GoodsID)
        //{
        //    return GetGoodsDiscount(SiteService.GetCurrentShopID(), GoodsID);
        //}
        #endregion
    }
}
