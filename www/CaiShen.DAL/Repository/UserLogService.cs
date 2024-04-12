using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserLogService
    {
        private static UnitOfWork work = new UnitOfWork();

        public static void Add(string username, int userid, string content, string itemid)
        {
            UserLog logModel = new UserLog();
            logModel.UL_UserName = username;
            logModel.UserID = userid;
            logModel.UL_Content = content;
            logModel.UL_ItemID = itemid;
            logModel.UL_IP = IPHelper.GetIP();
            logModel.UL_CreateTime = DateTime.Now;

            work.UserLogRepository.Insert(logModel);
            work.Save();
            //work.Dispose();不能释放
        }

        public static void Add(User model, string content, string itemid)
        {
            if (model != null)
                Add(model.U_UserName, model.ID, content, itemid);
        }

    }
}
