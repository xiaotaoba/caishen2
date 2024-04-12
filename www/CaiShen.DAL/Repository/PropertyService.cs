using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class PropertyService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Property GetModel(int ID)
        {
            var list = work.PropertyRepository.Get(m => m.ID == ID, null).ToList<Property>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new Property();
        }

        #endregion

        #region 获取名称

        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        public static string GetName(int ID)
        {
            var list = work.PropertyRepository.Get(m => m.ID == ID, null).ToList<Property>();
            if (list.Count() > 0)
            {
                return list[0].Prop_Name;
            }
            return "无";
        }

        #endregion

    }
}
