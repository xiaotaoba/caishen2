using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class WarehouseAreaService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static WarehouseArea GetModel(int ID)
        {
            var list = work.WarehouseAreaRepository.Get(m => m.ID == ID, null).ToList<WarehouseArea>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new WarehouseArea();
        }
        #endregion

        #region 获取配送地区名称
        /// <summary>
        /// 获取配送地区名称
        /// </summary>
        /// <param name="areaids">areaids</param>
        /// <returns></returns>
        public static string GetAreaNames(string areaids)
        {
            //List<string> areaidsArr = areaids.Split(',').AsQueryable().Where(m => m != "").ToList();
            //List<string> list = work.Context.Areas.Where(m => areaidsArr.Contains(m.ID.ToString())).Select(m => m.Area_Name).ToList();
            //return string.Join(",", list.ToArray());
            return AreaService.GetAreaNames(areaids);
        }
        #endregion

        #region 判断是否是和仓库同区域

        /// <summary>
        /// 判断是否是和仓库同区域
        /// </summary>
        /// <param name="warehouseID">仓库ID</param>
        /// <param name="userAddressID">用户地址ID</param>
        /// <returns></returns>
        public static bool CheckIsSameArea(int warehouseID, int userAddressID)
        {
            //地址模板
            UserAddress addressModel = work.UserAddressRepository.GetByID(userAddressID);
            if (addressModel == null)
                addressModel = new UserAddress();

            var rst_area = work.WarehouseAreaRepository.Get(m => m.WarehouseID == warehouseID).ToList();

            if (rst_area == null || rst_area.Count < 1)
            {
                return false;
            }

            //按区域（区-市-省顺序）匹配地区
            var rst_region = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", addressModel.Region)) && m.IsSameArea == 1).ToList();
            if (rst_region.Count() > 0)
            {
                return true;
            }

            var rst_city = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", addressModel.City)) && m.IsSameArea == 1).ToList();
            if (rst_city.Count() > 0)
            {
                return true;
            }

            var rst_province = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", addressModel.Province)) && m.IsSameArea == 1).ToList();
            if (rst_province.Count() > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否是和仓库同区域
        /// </summary>
        /// <param name="warehouseID">仓库ID</param>
        /// <param name="shopModel">当前门店</param>
        /// <returns></returns>
        public static bool CheckIsSameArea(int warehouseID, UserShop shopModel)
        {
            var rst_area = work.WarehouseAreaRepository.Get(m => m.WarehouseID == warehouseID).ToList();

            if (rst_area == null || rst_area.Count < 1)
            {
                return false;
            }

            //按区域（区-市-省顺序）匹配地区
            if (shopModel.Shop_Region != 0)
            {
                var rst_region = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_Region)) && m.IsSameArea == 1).ToList();
                if (rst_region.Count() > 0)
                {
                    return true;
                }
            }
            if (shopModel.Shop_City != 0)
            {
                var rst_city = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_City)) && m.IsSameArea == 1).ToList();
                if (rst_city.Count() > 0)
                {
                    return true;
                }
            }
            if (shopModel.Shop_Province != 0)
            {
                var rst_province = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_Province)) && m.IsSameArea == 1).ToList();
                if (rst_province.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
