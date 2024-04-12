using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    /// <summary>
    /// 订单附加或减免费用清单
    /// </summary>
    public class OrderFeeListService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region  新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="orderID"></param>
        /// <param name="thing"></param>
        /// <param name="amount"></param>
        /// <param name="type">费用类型 默认：0减免，1增加</param>
        /// <returns></returns>
        public static int Add(int userID, int orderID, string thing, decimal amount, int type)
        {
            OrderFeeList model = new OrderFeeList();
            model.OFL_Amount = amount;
            model.OFL_Thing = thing;
            model.OFL_type = type;
            model.OrderID = orderID;
            model.UserID = userID;

            work.OrderFeeListRepository.Insert(model);
            work.Save();

            return model.ID;
        }
        #endregion
    }
}
