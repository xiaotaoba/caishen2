using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ReturnOrderShippingService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static ReturnOrderShipping GetModel(int ID)
        {
            var list = work.ReturnOrderShippingRepository.Get(m => m.ID == ID, null).ToList<ReturnOrderShipping>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new ReturnOrderShipping();
        }

        #endregion


        #region  是否存在记录

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="returnOrderID">退货记录ID</param>
        /// <returns></returns>
        public static bool ExistByReturnOrderID(int returnOrderID)
        {
            var rst_count = work.Context.ReturnOrderShippings.Where(m => m.ReturnOrderID == returnOrderID).Count();
            if (rst_count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否存在记录，返回退货物流实体
        /// </summary>
        /// <param name="returnOrderID">退货记录ID</param>
        /// <returns></returns>
        public static ReturnOrderShipping GetModelByReturnOrderID(int returnOrderID)
        {
            return work.Context.ReturnOrderShippings.Where(m => m.ReturnOrderID == returnOrderID).FirstOrDefault();
        }

        #endregion
    }
}
