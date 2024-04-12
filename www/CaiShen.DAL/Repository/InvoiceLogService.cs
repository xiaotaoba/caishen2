using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;
using EntityFramework.Extensions;

namespace Pannet.DAL.Repository
{
    public class InvoiceLogService
    {
        private static UnitOfWork work = new UnitOfWork();


        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static InvoiceLog GetModel(int ID)
        {
            var list = work.InvoiceLogRepository.Get(m => m.ID == ID, null).ToList<InvoiceLog>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new InvoiceLog();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="invoiceNumber">发票申请单号</param>
        /// <returns></returns>
        public static InvoiceLog GetModelByNumber(string invoiceNumber)
        {
            var model = work.InvoiceLogRepository.Get(m => m.Inv_InvoiceNumber == invoiceNumber, null).FirstOrDefault<InvoiceLog>();
            return model;
        }

        #endregion

        #region 更新记录状态

        /// <summary>
        /// 更新记录状态
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateStatus(int ID, int status)
        {
            work.Context.InvoiceLogs.Where(m => m.ID == ID).Update(m => new InvoiceLog { Inv_Status = status });
            work.Save();
            return true;
        }

        #endregion

        /// <summary>
        /// 生成申请发票编号 - 统一用订单编号生成方式处理
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetInvoiceNumber(int userID)
        {
            return OrderService.GetOrderNo(0, userID, "03");
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="invoiceNumber">发票申请单号</param>
        /// <returns></returns>
        public static bool ExistNumber(string invoiceNumber)
        {
            var model = work.InvoiceLogRepository.Get(m => m.Inv_InvoiceNumber == invoiceNumber, null).FirstOrDefault<InvoiceLog>();
            if (model == null || model.ID == 0)
            {
                return false;
            }
            return true;
        }


    }
}
