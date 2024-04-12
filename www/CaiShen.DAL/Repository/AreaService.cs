using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class AreaService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Area GetModel(int ID)
        {
            var list = work.AreaRepository.Get(m => m.ID == ID, null).ToList<Area>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return null;
        }
        #endregion

        #region 获取地区名称

        /// <summary>
        /// 获取地区名称,逗号隔开
        /// </summary>
        /// <param name="areaids">areaids</param>
        /// <returns></returns>
        public static string GetAreaNames(string areaids)
        {
            List<string> areaidsArr = areaids.Split(',').AsQueryable().Where(m => m != "").ToList();
            List<string> list = work.Context.Areas.Where(m => areaidsArr.Contains(m.ID.ToString())).Select(m => m.Area_Name).ToList();
            return string.Join(",", list.ToArray());
        }

        #endregion

    }
}
