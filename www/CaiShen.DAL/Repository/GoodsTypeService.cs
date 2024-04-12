using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class GoodsTypeService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static GoodsType GetModel(int ID)
        {
            var list = work.GoodsTypeRepository.Get(m => m.ID == ID, null).ToList<GoodsType>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new GoodsType();
        }
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        public static string GetName(int ID)
        {
            var list = work.GoodsTypeRepository.Get(m => m.ID == ID, null).ToList<GoodsType>();
            if (list.Count() > 0)
            {
                return list[0].GT_Name;
            }
            return "无";
        }
    }
}
