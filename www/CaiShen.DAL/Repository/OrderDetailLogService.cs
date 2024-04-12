using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class OrderDetailLogService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 新增订单详细修改记录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userid"></param>
        /// <param name="content"></param>
        /// <param name="orderid"></param>
        /// <param name="orderdetailid"></param>
        /// <param name="type">记录类型,0：修改成本，1：修改加盟商价格 等</param>
        public static void Add(string username, int userid, string content, int orderid, int orderdetailid, int type = 0)
        {
            OrderDetailLog logModel = new OrderDetailLog();
            logModel.ODL_UserName = username;
            logModel.ManagerID = userid;
            logModel.ODL_Content = content;
            logModel.OrderID = orderid;
            logModel.ODL_IP = IPHelper.GetIP();
            logModel.ODL_CreateTime = DateTime.Now;
            logModel.OrderDetailID = orderdetailid;
            logModel.ODL_Type = type;

            work.OrderDetailLogRepository.Insert(logModel);
            work.Save();
            //work.Dispose();不能释放
        }

        /// <summary>
        /// 新增订单详细修改记录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userid"></param>
        /// <param name="content"></param>
        /// <param name="orderid"></param>
        /// <param name="orderdetailid"></param>
        /// <param name="type">记录类型,0：修改成本，1：修改加盟商价格 等</param>
        public static void Add(Manager model, string content, int orderid, int orderdetailid, int type = 0)
        {
            if (model != null)
                Add(model.UserName, model.ID, content, orderid, orderdetailid, type);
        }
    }
}
