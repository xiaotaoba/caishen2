using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ReturnOrderService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static ReturnOrder GetModel(int ID)
        {
            var list = work.ReturnOrderRepository.Get(m => m.ID == ID, null).ToList<ReturnOrder>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new ReturnOrder();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static ReturnOrder GetModelByReturnOrderNo(string returnOrderNo)
        {
            var list = work.ReturnOrderRepository.Get(m => m.RO_ReturnOrderNo == returnOrderNo, null).ToList<ReturnOrder>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new ReturnOrder();
        }

        #endregion

        #region 获得生成的订单编号

        public static string GetReturnOrderNo(int orderDetailID)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + orderDetailID.ToString().PadLeft(8, '0') + Assistant.GetRandomNumber(2);
        }

        #endregion

        #region  是否存在记录

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="orderDetailID"></param>
        /// <param name="status">退货处理状态</param>
        /// <returns></returns>
        public static bool ExistStatus(int orderDetailID, int status)
        {
            var rs_count = work.Context.ReturnOrders.Where(m => m.OrderDetailID == orderDetailID && m.RO_Status == status).Count();
            if (rs_count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="orderDetailID"></param>
        /// <param name="status">退货处理状态</param>
        /// <param name="existReturnOrder">当前存在的实体</param>
        /// <returns></returns>
        public static bool ExistStatus(int orderDetailID, int status, out ReturnOrder existReturnOrder)
        {
            var rs = work.Context.ReturnOrders.Where(m => m.OrderDetailID == orderDetailID && m.RO_Status == status).ToList();
            if (rs != null && rs.Count() > 0)
            {
                existReturnOrder = rs[0];
                return true;
            }
            existReturnOrder = null;
            return false;
        }

        /// <summary>
        /// 获取存在的实体
        /// </summary>
        /// <param name="orderDetailID"></param>
        /// <param name="status">退货处理状态</param>
        /// <returns></returns>
        public static ReturnOrder GetModel(int orderDetailID, int status)
        {
            var rs = work.Context.ReturnOrders.Where(m => m.OrderDetailID == orderDetailID && m.RO_Status == status).ToList();
            if (rs != null && rs.Count() > 0)
            {
                return rs[0];
            }
            return null;
        }

        #endregion
    }
}
