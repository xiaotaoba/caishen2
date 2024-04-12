using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class NavigationService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Navigation GetModel(int ID)
        {
            var list = work.NavigationRepository.Get(m => m.ID == ID, null).ToList<Navigation>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new Navigation();
        }
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        public static string GetName(int ID)
        {
            var list = work.NavigationRepository.Get(m => m.ID == ID, null).ToList<Navigation>();
            if (list.Count() > 0)
            {
                return list[0].Nav_Name;
            }
            return "无";
        }
    }
}
