using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class OrderDetailService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static OrderDetail GetModel(int ID)
        {
            var list = work.OrderDetailRepository.Get(m => m.ID == ID, null).ToList<OrderDetail>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new OrderDetail();
        }

        #endregion

        #region  是否存在记录

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="shippingStatus"></param>
        /// <returns></returns>
        public static bool ExistShippingStatus(int orderID, int shippingStatus)
        {
            var rs_count = work.Context.OrderDetails.Where(m => m.OrderID == orderID && m.OD_ShippingStatus == shippingStatus).Count();
            if (rs_count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="orderDetailIds"></param>
        /// <param name="shippingStatus"></param>
        /// <returns></returns>
        public static bool ExistShippingStatus(Int32[] orderDetailIds, int shippingStatus)
        {
            var rs_count = work.Context.OrderDetails.Where(m => orderDetailIds.Contains(m.ID) && m.OD_ShippingStatus == shippingStatus).Count();
            if (rs_count > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="orderDetailIds">逗号隔开orderdetail id</param>
        /// <param name="shippingStatus"></param>
        /// <returns></returns>
        public static bool ExistShippingStatus(string orderDetailIds, int shippingStatus)
        {
            Int32[] idsArr = UtilityClass.ConvertIntArr(orderDetailIds);
            return ExistShippingStatus(idsArr, shippingStatus);
        }
        #endregion

        #region 更新订单状态

        /// <summary>
        /// 更新订单发货状态
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateShippingStatusByOrderID(int OrderID, int status)
        {
            var rst = work.OrderDetailRepository.Get(m => m.OrderID == OrderID);
            if (rst != null)
            {
                foreach (var item in rst)
                {
                    item.OD_ShippingStatus = status;
                    work.OrderDetailRepository.Update(item);
                    work.Save();
                }
            }
            return true;
        }

        /// <summary>
        /// 更新订单发货状态
        /// </summary>
        /// <param name="ids">订单详细ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateShippingStatus(Int32[] ids, int status)
        {
            var rst = work.OrderDetailRepository.Get(m => ids.Contains(m.ID));
            if (rst != null)
            {
                foreach (var item in rst)
                {
                    item.OD_ShippingStatus = status;
                    work.OrderDetailRepository.Update(item);
                    work.Save();
                }
            }
            return true;
        }
        /// <summary>
        /// 更新订单发货状态
        /// </summary>
        /// <param name="ID">订单详细ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateShippingStatus(int ID, int status)
        {
            var rst = work.OrderDetailRepository.Get(m => m.ID == ID);
            if (rst != null)
            {
                foreach (var item in rst)
                {
                    item.OD_ShippingStatus = status;
                    work.OrderDetailRepository.Update(item);
                    work.Save();
                }
            }
            return true;
        }



        /// <summary>
        /// 更新订单支付状态
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdatePayStatusByOrderID(int OrderID, int status)
        {
            var rst = work.OrderDetailRepository.Get(m => m.OrderID == OrderID);
            if (rst != null)
            {
                foreach (var item in rst)
                {
                    item.OD_PayStatus = status;
                    work.OrderDetailRepository.Update(item);
                    work.Save();
                }
            }
            return true;
        }

        /// <summary>
        /// 更新订单支付状态
        /// </summary>
        /// <param name="ID">订单详细ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdatePayStatus(int ID, int status)
        {
            var rst = work.OrderDetailRepository.Get(m => m.ID == ID);
            if (rst != null)
            {
                foreach (var item in rst)
                {
                    item.OD_PayStatus = status;
                    work.OrderDetailRepository.Update(item);
                    work.Save();
                }
            }
            return true;
        }

        /// <summary>
        /// 更新订单支付状态
        /// </summary>
        /// <param name="ids">订单详细ID</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdatePayStatus(Int32[] ids, int status)
        {
            var rst = work.OrderDetailRepository.Get(m => ids.Contains(m.ID));
            if (rst != null)
            {
                foreach (var item in rst)
                {
                    item.OD_PayStatus = status;
                    work.OrderDetailRepository.Update(item);
                    work.Save();
                }
            }
            return true;
        }

        #endregion

    }
}
