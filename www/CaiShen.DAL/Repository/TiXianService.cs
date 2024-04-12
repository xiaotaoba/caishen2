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
    public class TiXianService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static TiXian GetModel(int ID)
        {
            var list = work.TiXianRepository.Get(m => m.ID == ID, null).ToList<TiXian>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new TiXian();
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
            work.Context.TiXians.Where(m => m.ID == ID).Update(m => new TiXian { TX_Status = status });
            work.Save();
            return true;
        }

        #endregion

        #region 生成申请提现编号

        /// <summary>
        /// 生成申请提现编号 - 统一用订单编号生成方式处理
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetTiXianNumber(int userID)
        {
            return OrderService.GetOrderNo(0, userID, "04");
        }

        #endregion

        #region 是否存在

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="invoiceNumber">发票申请单号</param>
        /// <returns></returns>
        public static bool ExistNumber(string txNumber)
        {
            var model = work.TiXianRepository.Get(m => m.TX_Number == txNumber, null).FirstOrDefault<TiXian>();
            if (model == null || model.ID == 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
