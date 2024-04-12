using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class SupplierGoodsService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获得生产商品数量

        /// <summary>
        /// 获得生产商品数量
        /// </summary>
        /// <param name="supplierID">供应商ID</param>
        /// <returns></returns>
        public static int GetCount(int supplierID)
        {
            return work.Context.SupplierGoods.Where(m => m.SupplierID == supplierID).Count();
        }

        #endregion

    }
}
