using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class LogService
    {
        private static UnitOfWork work = new UnitOfWork();

        public static void Add(string username, int userid, string content, string itemid)
        {
            ManagerLog logModel = new ManagerLog();
            logModel.ML_UserName = username;
            logModel.ManagerID = userid;
            logModel.ML_Content = content;
            logModel.ML_ItemID = itemid;
            logModel.ML_IP = IPHelper.GetIP();
            logModel.ML_CreateTime = DateTime.Now;

            work.ManagerLogRepository.Insert(logModel);
            work.Save();
            //work.Dispose();不能释放
        }

        public static void Add(Manager model, string content, string itemid)
        {
            if (model != null)
                Add(model.UserName, model.ID, content, itemid);
        }

    }
}
