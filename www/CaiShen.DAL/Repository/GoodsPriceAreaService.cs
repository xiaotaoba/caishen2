using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class GoodsPriceAreaService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static GoodsPriceArea GetModel(int ID)
        {
            var model = work.GoodsPriceAreaRepository.Get(m => m.ID == ID, null).FirstOrDefault();
            if (model != null)
            {
                return model;
            }
            return new GoodsPriceArea();
        }
        #endregion

        #region 根据门店地址__查询所在定价区域

        /// <summary>
        /// 根据门店地址__查询所在定价区域
        /// </summary>
        /// <param name="shopModel">门店实体</param>
        /// <returns></returns>
        public static GoodsPriceArea GetGoodsPriceArea(UserShop shopModel)
        {
            var rst_area = work.Context.GoodsPriceAreas.OrderByDescending(m => m.Sort).ToList();

            if (rst_area == null || rst_area.Count() < 1)
            {
                return null;
            }
            
            ////按区域（区-市-省顺序）匹配运费配送地区
            //var rst_region = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_Region))).ToList();
            //if (rst_region.Count() > 0)
            //{
            //    return rst_region[0];
            //}

            var rst_city = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_City))).ToList();
            if (rst_city.Count() > 0)
            {
                return rst_city[0];
            }

            var rst_province = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_Province))).ToList();
            if (rst_province.Count() > 0)
            {
                return rst_province[0];
            }
            ////没有配送地区，使用默认
            //var rst_default = rst_area.Where(m => m.SA_Is_Default == 1).ToList();
            //if (rst_default.Count() > 0)
            //{
            //    return rst_default[0];
            //}
            return null;
        }

        #endregion

        /// <summary>
        /// 打印日志
        /// </summary>
        private static void WriteLog(string content)
        {
            bool print = true;
            if (print)
            {
                Log.WriteLog(content, "pricearea", DateTime.Now.ToString("yyyyMMdd"));
            }
        }
    }
}
