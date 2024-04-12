using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserRoleHistoryService
    {
        private static UnitOfWork work = new UnitOfWork();

        public static void Add(int prev_roleid, int current_roleid, int userid, string title, string _operator)
        {
            UserRoleHistory newModel = new UserRoleHistory();
            newModel.Current_Role_ID = current_roleid;
            newModel.Prev_Role_ID = prev_roleid;
            newModel.UserID = userid;
            newModel.Title = title;
            newModel.Time = DateTime.Now;
            newModel.Operator = _operator;

            work.UserRoleHistoryRepository.Insert(newModel);
            work.Save();
            //work.Dispose();不能释放
        }
    }
}
