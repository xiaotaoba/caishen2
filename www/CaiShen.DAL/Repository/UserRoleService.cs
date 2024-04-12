using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserRoleService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static UserRole GetModel(int ID)
        {
            var list = work.UserRoleRepository.Get(m => m.ID == ID, null).ToList<UserRole>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new UserRole();
        }
       
    }
}
