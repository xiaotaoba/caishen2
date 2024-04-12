using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class OrderChangeLogService
    {
        private static UnitOfWork work = new UnitOfWork();

        public static void Add(string username, int userid, string content, int orderid)
        {
            OrderChangeLog logModel = new OrderChangeLog();
            logModel.OCL_UserName = username;
            logModel.ManagerID = userid;
            logModel.OCL_Content = content;
            logModel.OrderID = orderid;
            logModel.OCL_IP = IPHelper.GetIP();
            logModel.OCL_CreateTime = DateTime.Now;

            work.OrderChangeLogRepository.Insert(logModel);
            work.Save();
            //work.Dispose();不能释放
        }

        public static void Add(Manager model, string content, int orderid)
        {
            if (model != null)
                Add(model.UserName, model.ID, content, orderid);
        }
        public static void Add(User model, string content, int orderid)
        {
            if (model != null)
                Add(model.U_UserName, model.ID, content, orderid);
        }
    }
}
