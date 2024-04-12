using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserScoreProductService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static UserScoreProduct GetModel(int ID)
        {
            var list = work.UserScoreProductRepository.Get(m => m.ID == ID, null).ToList<UserScoreProduct>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new UserScoreProduct();
        }

        #endregion

        #region  获取礼品兑换成功次数

        /// <summary>
        /// 获取礼品兑换成功次数
        /// </summary>
        /// <returns></returns>
        public static int GetExchangeCount(int ShopID = 0, int ScoreProductID = 0)
        {
            var rs = work.Context.UserScoreProducts.AsQueryable();
            if (ShopID == 0)
            {
                rs = rs.Where(m => m.ScoreProductID == ScoreProductID);
            }
            else
            {
                rs = rs.Where(m => m.UserShopID == ShopID && m.ScoreProductID == ScoreProductID);
            }
            return rs.Count();
        }

        #endregion

    }
}
