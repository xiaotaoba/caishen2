using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class WarehouseGoodsService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获得仓库商品数量

        /// <summary>
        /// 获得仓库商品数量
        /// </summary>
        /// <param name="WarehouseID">仓库ID</param>
        /// <returns></returns>
        public static int GetCount(int WarehouseID)
        {
            return work.Context.WarehouseGoods.Where(m => m.WarehouseID == WarehouseID).Count();
        }

        #endregion

    }
}
