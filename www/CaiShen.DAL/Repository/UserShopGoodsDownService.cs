using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserShopGoodsDownService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获得下架商品数量

        /// <summary>
        /// 获得下架商品数量
        /// </summary>
        /// <param name="userShopID">门店ID</param>
        /// <returns></returns>
        public static int GetCount(int userShopID)
        {
            return work.Context.UserShopGoodsDowns.Where(m => m.ShopID == userShopID).Count();
        }

        #endregion

    }
}
