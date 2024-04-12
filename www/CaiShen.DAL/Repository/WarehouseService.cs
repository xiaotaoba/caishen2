using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;
using System.Web;
using System.Configuration;

namespace Pannet.DAL.Repository
{
    public class WarehouseService
    {
        private static UnitOfWork work = new UnitOfWork();


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Warehouse GetModel(int ID)
        {
            var model = work.WarehouseRepository.Get(m => m.ID == ID, null).FirstOrDefault();
            if (model != null)
            {
                return model;
            }
            return new Warehouse();
        }

        ///// <summary>
        ///// 分配仓库ID -根据收货地（2017-11-14调整不根据收货地确认仓库，根据门店地址）
        ///// </summary>
        ///// <param name="userAddressID">用户收货地</param>
        ///// <param name="goodsID">产品ID</param>
        ///// <returns></returns>
        //public static int GetWarehouseID(int userAddressID = 0, int goodsID = 0)
        //{
        //    UserAddress addressModel = work.UserAddressRepository.GetByID(userAddressID);
        //    if (addressModel == null)
        //        addressModel = new UserAddress();

        //    //Area areaRegion = work.AreaRepository.GetByID(addressModel.Region);
        //    //按区域（区-市-省顺序）匹配仓库
        //    var rst_region = work.Context.WarehouseAreas
        //        .Join(work.Context.Warehouses, wa => wa.WarehouseID, w => w.ID, (wa, w) => new { wa, w })
        //        .Where(m => m.wa.AreaIds.Contains("," + addressModel.Region + ",") && m.w.Is_Enable == 1).ToList().Select(m => m.wa).OrderByDescending(m => m.Sort).ToList();
        //    if (rst_region.Count() > 0)
        //        return rst_region[0].WarehouseID;

        //    var rst_city = work.Context.WarehouseAreas
        //        .Join(work.Context.Warehouses, wa => wa.WarehouseID, w => w.ID, (wa, w) => new { wa, w })
        //        .Where(m => m.wa.AreaIds.Contains("," + addressModel.City + ",") && m.w.Is_Enable == 1).ToList().Select(m => m.wa).OrderByDescending(m => m.Sort).ToList();
        //    if (rst_city.Count() > 0)
        //        return rst_city[0].WarehouseID;

        //    var rst_province = work.Context.WarehouseAreas
        //        .Join(work.Context.Warehouses, wa => wa.WarehouseID, w => w.ID, (wa, w) => new { wa, w })
        //        .Where(m => m.wa.AreaIds.Contains("," + addressModel.Province + ",") && m.w.Is_Enable == 1).ToList().Select(m => m.wa).OrderByDescending(m => m.Sort).ToList();
        //    if (rst_province.Count() > 0)
        //        return rst_province[0].WarehouseID;

        //    //返回产品默认仓库
        //    if (goodsID != 0)
        //    {
        //        Goods goodsModel = work.GoodsRepository.GetByID(goodsID);
        //        if (goodsModel != null)
        //            return goodsModel.WarehouseID;
        //    }

        //    //返回一个仓库
        //    var rst_default = work.WarehouseRepository.Get(m => m.Is_Enable == 1).FirstOrDefault();
        //    return rst_default.ID;
        //}

        /// <summary>
        /// 分配仓库ID - 根据门店
        /// </summary>
        /// <param name="shopModel">门店实体</param>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static int GetWarehouseID(UserShop shopModel, int goodsID = 0)
        {
            //Area areaRegion = work.AreaRepository.GetByID(addressModel.Region);

            //所有可配送该产品的仓库ID
            var rst_goods_warehouseID = work.Context.WarehouseGoods
                    .Join(work.Context.Warehouses, wg => wg.WarehouseID, w => w.ID, (wg, w) => new { wg, w })
                    .Where(m => m.wg.GoodsID == goodsID).Select(m => m.w.ID).Distinct();

            if (rst_goods_warehouseID != null && shopModel != null)
            {
                //按区域（区-市-省顺序）匹配仓库
                var rst_region = work.Context.WarehouseAreas
                    .Join(work.Context.Warehouses, wa => wa.WarehouseID, w => w.ID, (wa, w) => new { wa, w })
                    .Where(m => m.wa.AreaIds.Contains("," + shopModel.Shop_Region + ",") && m.w.Is_Enable == 1 && rst_goods_warehouseID.Contains(m.w.ID)).Select(m => m.wa).OrderByDescending(m => m.Sort).ToList();
                    //.Where(m => m.wa.AreaIds.Contains("," + shopModel.Shop_Region + ",") && m.w.Is_Enable == 1 && rst_goods_warehouseID.Contains(m.w.ID)).ToList().Select(m => m.wa).OrderByDescending(m => m.Sort).ToList();
                if (rst_region != null && rst_region.Count() > 0)
                    return rst_region[0].WarehouseID;

                var rst_city = work.Context.WarehouseAreas
                    .Join(work.Context.Warehouses, wa => wa.WarehouseID, w => w.ID, (wa, w) => new { wa, w })
                    .Where(m => m.wa.AreaIds.Contains("," + shopModel.Shop_City + ",") && m.w.Is_Enable == 1 && rst_goods_warehouseID.Contains(m.w.ID)).Select(m => m.wa).OrderByDescending(m => m.Sort).ToList();
                if (rst_city != null && rst_city.Count() > 0)
                    return rst_city[0].WarehouseID;

                var rst_province = work.Context.WarehouseAreas
                    .Join(work.Context.Warehouses, wa => wa.WarehouseID, w => w.ID, (wa, w) => new { wa, w })
                    .Where(m => m.wa.AreaIds.Contains("," + shopModel.Shop_Province + ",") && m.w.Is_Enable == 1 && rst_goods_warehouseID.Contains(m.w.ID)).Select(m => m.wa).OrderByDescending(m => m.Sort).ToList();
                if (rst_province != null && rst_province.Count() > 0)
                    return rst_province[0].WarehouseID;
            }
            //返回产品默认仓库
            if (goodsID != 0)
            {
                Goods goodsModel = work.GoodsRepository.GetByID(goodsID);
                if (goodsModel != null)
                    return goodsModel.WarehouseID;
            }

            //返回一个仓库
            var rst_default = work.WarehouseRepository.Get(m => m.Is_Enable == 1).FirstOrDefault();
            return rst_default.ID;
        }
        //// <summary>
        ///// 分配仓库ID - 根据门店
        ///// </summary>
        ///// <param name="shopModel">门店实体</param>
        ///// <param name="goodsID"></param>
        ///// <returns></returns>
        //public static Warehouse GetWarehouseModel(UserShop shopModel, int goodsID = 0)
        //{
        //    //Area areaRegion = work.AreaRepository.GetByID(addressModel.Region);
        //    //按区域（区-市-省顺序）匹配仓库
        //    var rst_region = work.Context.WarehouseAreas
        //        .Join(work.Context.Warehouses, wa => wa.WarehouseID, w => w.ID, (wa, w) => new { wa, w })
        //        .Where(m => m.wa.AreaIds.Contains("," + shopModel.Shop_Region + ",") && m.w.Is_Enable == 1).ToList().Select(m => m.w).OrderByDescending(m => m.Sort).ToList();
        //    if (rst_region.Count() > 0)
        //        return rst_region[0];

        //    var rst_city = work.Context.WarehouseAreas
        //        .Join(work.Context.Warehouses, wa => wa.WarehouseID, w => w.ID, (wa, w) => new { wa, w })
        //        .Where(m => m.wa.AreaIds.Contains("," + shopModel.Shop_City + ",") && m.w.Is_Enable == 1).ToList().Select(m => m.w).OrderByDescending(m => m.Sort).ToList();
        //    if (rst_city.Count() > 0)
        //        return rst_city[0];

        //    var rst_province = work.Context.WarehouseAreas
        //        .Join(work.Context.Warehouses, wa => wa.WarehouseID, w => w.ID, (wa, w) => new { wa, w })
        //        .Where(m => m.wa.AreaIds.Contains("," + shopModel.Shop_Province + ",") && m.w.Is_Enable == 1).ToList().Select(m => m.w).OrderByDescending(m => m.Sort).ToList();
        //    if (rst_province.Count() > 0)
        //        return rst_province[0];

        //    //返回产品默认仓库
        //    if (goodsID != 0)
        //    {
        //        Goods goodsModel = work.GoodsRepository.GetByID(goodsID);
        //        if (goodsModel != null)
        //            return work.WarehouseRepository.GetByID(goodsModel.WarehouseID);
        //    }

        //    //返回一个仓库
        //    var rst_default = work.WarehouseRepository.Get(m => m.Is_Enable == 1).FirstOrDefault();
        //    return rst_default;
        //}
    }
}
