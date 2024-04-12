using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserLevelService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static UserLevel GetModel(int ID)
        {
            var list = work.UserLevelRepository.Get(m => m.ID == ID, null).ToList<UserLevel>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new UserLevel();
        }

    }
}
