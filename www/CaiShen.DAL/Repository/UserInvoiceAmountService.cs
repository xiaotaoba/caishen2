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
    public class UserInvoiceAmountService
    {
        private static UnitOfWork work = new UnitOfWork();


        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static UserInvoiceAmount GetModel(int ID)
        {
            var list = work.UserInvoiceAmountRepository.Get(m => m.ID == ID, null).ToList<UserInvoiceAmount>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new UserInvoiceAmount();
        }

        #endregion

        #region 更新累计消费金额

        /// <summary>
        /// 更新累计消费金额
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static UserInvoiceAmount UpdateTotalAmount(int userID)
        {
            int orderDone = Convert.ToInt16(DataConfig.OrderStatusEnum.交易成功);
            //所有交易完成订单金额
            decimal totalAmount = 0;
            var rst = work.Context.Orders.Where(m => m.UserID == userID && m.O_Status == orderDone).Select(m => m.O_NeedPayAmount);
            if (rst != null && rst.Count() > 0)
            {
                totalAmount = rst.Sum();
            }
            ;
            UserInvoiceAmount existModel = work.Context.UserInvoiceAmounts.Where(m => m.UserID == userID).FirstOrDefault();
            if (existModel == null || existModel.ID == 0)//不存在记录
            {
                existModel = new UserInvoiceAmount();
                existModel.UserID = userID;
                existModel.UIA_TotalAmount = totalAmount;
                existModel.UIA_InvoicedAmount = 0;
                existModel.UIA_RestAmount = totalAmount;

                work.Context.UserInvoiceAmounts.Add(existModel);
            }
            else
            {
                existModel.UIA_TotalAmount = totalAmount;
                decimal restAmount = totalAmount - existModel.UIA_InvoicedAmount;
                if (restAmount < 0)
                {
                    restAmount = 0;
                }
                existModel.UIA_RestAmount = restAmount;
                work.UserInvoiceAmountRepository.Update(existModel);
            }
            work.Save();

            return existModel;
        }
        #endregion

        #region 开发票，增加已开，减少可用额度

        /// <summary>
        /// 开发票，增加已开，减少可用额度
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="amount">本次开票金额</param>
        /// <returns></returns>
        public static UserInvoiceAmount UpdateInvoiceAmountAdd(int userID, decimal amount)
        {
            UserInvoiceAmount existModel = work.Context.UserInvoiceAmounts.Where(m => m.UserID == userID).FirstOrDefault();
            if (existModel == null || existModel.ID == 0)//不存在记录
            {
                int orderDone = Convert.ToInt16(DataConfig.OrderStatusEnum.交易成功);
                //所有交易完成订单金额
                decimal totalAmount = 0;
                var rst = work.Context.Orders.Where(m => m.UserID == userID && m.O_Status == orderDone).Select(m => m.O_NeedPayAmount);
                if (rst != null && rst.Count() > 0)
                {
                    totalAmount = rst.Sum();
                }
                decimal restAmount = totalAmount - amount;
                if (restAmount < 0)
                {
                    restAmount = 0;
                }

                existModel = new UserInvoiceAmount();
                existModel.UserID = userID;
                existModel.UIA_TotalAmount = totalAmount;
                existModel.UIA_InvoicedAmount = amount;
                existModel.UIA_RestAmount = restAmount;

                work.Context.UserInvoiceAmounts.Add(existModel);
            }
            else
            {
                decimal restAmount = existModel.UIA_RestAmount - amount;
                if (restAmount < 0)
                {
                    restAmount = 0;
                }

                existModel.UIA_InvoicedAmount = existModel.UIA_InvoicedAmount + amount;
                existModel.UIA_RestAmount = restAmount;

                work.UserInvoiceAmountRepository.Update(existModel);
            }
            work.Save();

            return existModel;
        }

        #endregion

        #region 取消开发票，减去已开，增加可用额度

        /// <summary>
        /// 取消开发票，减去已开，增加可用额度
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="amount">本次取消开票金额</param>
        /// <returns></returns>
        public static UserInvoiceAmount UpdateInvoiceAmountCancel(int userID, decimal amount)
        {
            UserInvoiceAmount existModel = work.Context.UserInvoiceAmounts.Where(m => m.UserID == userID).FirstOrDefault();
            if (existModel == null || existModel.ID == 0)//不存在记录
            {
                return null;
            }
            else
            {
                existModel.UIA_InvoicedAmount = existModel.UIA_InvoicedAmount - amount;
                existModel.UIA_RestAmount = existModel.UIA_RestAmount + amount;

                work.UserInvoiceAmountRepository.Update(existModel);
            }
            work.Save();

            return existModel;
        }

        #endregion

        #region 是否存在

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static UserInvoiceAmount ExistByUserID(int userID)
        {
            var model = work.UserInvoiceAmountRepository.Get(m => m.UserID == userID, null).FirstOrDefault<UserInvoiceAmount>();
            if (model == null || model.ID == 0)
            {
                return null;
            }
            return model;
        }
        #endregion
    }
}
