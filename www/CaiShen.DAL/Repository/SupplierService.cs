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
    public class SupplierService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Supplier GetModel(int ID)
        {
            var model = work.SupplierRepository.Get(m => m.ID == ID, null).FirstOrDefault();
            if (model != null)
            {
                return model;
            }
            return new Supplier();
        }

        #endregion

        #region 获得供应商编号

        /// <summary>
        /// 获得供应商编号 格式：SupplierID(5)
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        public static string GetSupplierNumber(int SupplierID)
        {
            return SupplierID.ToString().PadLeft(5, '0');
        }

        #endregion

        #region 分配供应商ID

        ///// <summary>
        ///// 分配供应商ID--根据收货地
        ///// </summary>
        ///// <returns></returns>
        //public static int GetSupplierID(int userAddressID = 0, int goodsID = 0)
        //{
        //    //当前购买产品
        //    Goods goodsModel = null;
        //    if (goodsID != 0)
        //    {
        //        goodsModel = work.GoodsRepository.GetByID(goodsID);
        //        if (goodsModel == null)
        //            return work.SupplierRepository.Get(m => m.Sup_Is_Enable == 1).FirstOrDefault().ID;
        //    }

        //    return GetSupplierID(userAddressID, goodsModel);
        //}

        /// <summary>
        /// 分配供应商ID--根据收货地
        /// </summary>
        /// <returns></returns>
        public static int GetSupplierID(int userAddressID, Goods goodsModel)
        {
            if (goodsModel == null)
                return work.SupplierRepository.Get(m => m.Sup_Is_Enable == 1).FirstOrDefault().ID;

            UserAddress addressModel = work.UserAddressRepository.GetByID(userAddressID);
            if (addressModel == null)
                return goodsModel.SupplierID;
                //addressModel = new UserAddress();

            //查询所有供应下单产品的供应商
            //供应所有产品
            int supplyGoodsWay_all = Convert.ToInt32(DataConfig.SupplyGoodsWayEnum.生产所有产品);
            var rst_all = work.Context.Suppliers.Where(m => m.Sup_SupplyGoodsWay == supplyGoodsWay_all).Select(m => m.ID);

            //当前分类产品
            int supplyGoodsWay_category = Convert.ToInt32(DataConfig.SupplyGoodsWayEnum.生产指定分类产品);
            var rst_category = work.Context.Suppliers
                .Join(work.Context.SupplierGoodsCategorys, s => s.ID, sgc => sgc.SupplierID, (s, sgc) => new { s, sgc }).Where(m => m.s.Sup_SupplyGoodsWay == supplyGoodsWay_category && m.sgc.GoodsCategoryID == goodsModel.GoodsCategoryID).Select(m => m.s.ID);
            //当前产品
            int supplyGoodsWay_goods = Convert.ToInt32(DataConfig.SupplyGoodsWayEnum.生产自定义产品);
            var rst_goods = work.Context.Suppliers
                .Join(work.Context.SupplierGoods, s => s.ID, sg => sg.SupplierID, (s, sg) => new { s, sg }).Where(m => m.s.Sup_SupplyGoodsWay == supplyGoodsWay_goods && m.sg.GoodsID == goodsModel.ID).Select(m => m.s.ID);

            //整合所有符合条件的供应商
            var rst_supplier = rst_all.Union(rst_category).Union(rst_goods).ToList<Int32>();

            //按区域（区-市-省顺序）匹配供应商
            var rst_region = work.Context.SupplierAreas
                .Join(work.Context.Suppliers, sa => sa.SupplierID, s => s.ID, (sa, s) => new { sa, s })
                .Where(m => m.sa.AreaIds.Contains("," + addressModel.Region + ",") && m.s.Sup_Is_Enable == 1 && rst_supplier.Contains(m.s.ID)).Select(m => m.sa).OrderByDescending(m => m.Sort).ToList();
            if (rst_region.Count() > 0)
                return rst_region[0].SupplierID;

            var rst_city = work.Context.SupplierAreas
                .Join(work.Context.Suppliers, sa => sa.SupplierID, s => s.ID, (sa, s) => new { sa, s })
                .Where(m => m.sa.AreaIds.Contains("," + addressModel.City + ",") && m.s.Sup_Is_Enable == 1 && rst_supplier.Contains(m.s.ID)).Select(m => m.sa).OrderByDescending(m => m.Sort).ToList();
            if (rst_city.Count() > 0)
                return rst_city[0].SupplierID;

            var rst_province = work.Context.SupplierAreas
                .Join(work.Context.Suppliers, sa => sa.SupplierID, s => s.ID, (sa, s) => new { sa, s })
                .Where(m => m.sa.AreaIds.Contains("," + addressModel.Province + ",") && m.s.Sup_Is_Enable == 1 && rst_supplier.Contains(m.s.ID)).Select(m => m.sa).OrderByDescending(m => m.Sort).ToList();
            if (rst_province.Count() > 0)
                return rst_province[0].SupplierID;

            //产品配置的默认供应商
            return goodsModel.SupplierID;

            ////如所有条件都不匹配，返回一个供应商
            //var rst_default = work.SupplierRepository.Get(m => m.Sup_Is_Enable == 1).FirstOrDefault();
            //return rst_default.ID;
        }


        /// <summary>
        /// 分配供应商ID--根据门店
        /// </summary>
        /// <returns></returns>
        public static int GetSupplierID(UserShop shopModel, int goodsID = 0)
        {
            //当前购买产品
            Goods goodsModel = null;
            if (goodsID != 0)
            {
                goodsModel = work.GoodsRepository.GetByID(goodsID);
                if (goodsModel == null)
                    return work.SupplierRepository.Get(m => m.Sup_Is_Enable == 1).FirstOrDefault().ID;
            }
            return GetSupplierID(shopModel, goodsModel);
        }
        /// <summary>
        /// 分配供应商ID--根据门店
        /// </summary>
        /// <returns></returns>
        public static int GetSupplierID(UserShop shopModel, Goods goodsModel)
        {
            if (goodsModel == null)
                return work.SupplierRepository.Get(m => m.Sup_Is_Enable == 1).FirstOrDefault().ID;

            //查询所有供应下单产品的供应商
            //供应所有产品
            int supplyGoodsWay_all = Convert.ToInt32(DataConfig.SupplyGoodsWayEnum.生产所有产品);
            var rst_all = work.Context.Suppliers.Where(m => m.Sup_SupplyGoodsWay == supplyGoodsWay_all).ToList().Select(m => m.ID).ToList();

            //当前分类产品
            int supplyGoodsWay_category = Convert.ToInt32(DataConfig.SupplyGoodsWayEnum.生产指定分类产品);
            var rst_category = work.Context.Suppliers
                .Join(work.Context.SupplierGoodsCategorys, s => s.ID, sgc => sgc.SupplierID, (s, sgc) => new { s, sgc }).Where(m => m.s.Sup_SupplyGoodsWay == supplyGoodsWay_category && m.sgc.GoodsCategoryID == goodsModel.GoodsCategoryID).ToList().Select(m => m.s.ID).ToList();
            //当前产品
            int supplyGoodsWay_goods = Convert.ToInt32(DataConfig.SupplyGoodsWayEnum.生产自定义产品);
            var rst_goods = work.Context.Suppliers
                .Join(work.Context.SupplierGoods, s => s.ID, sg => sg.SupplierID, (s, sg) => new { s, sg }).Where(m => m.s.Sup_SupplyGoodsWay == supplyGoodsWay_goods && m.sg.GoodsID == goodsModel.ID).ToList().Select(m => m.s.ID).ToList();

            //整合所有符合条件的供应商
            var rst_supplier = rst_all.Union(rst_category).Union(rst_goods).Distinct();

            //按区域（区-市-省顺序）匹配供应商
            var rst_region = work.Context.SupplierAreas
                .Join(work.Context.Suppliers, sa => sa.SupplierID, s => s.ID, (sa, s) => new { sa, s })
                .Where(m => m.sa.AreaIds.Contains("," + shopModel.Shop_Region + ",") && m.s.Sup_Is_Enable == 1 && rst_supplier.Contains(m.s.ID)).ToList().Select(m => m.sa).OrderByDescending(m => m.Sort).ToList();
            if (rst_region.Count() > 0)
                return rst_region[0].SupplierID;

            var rst_city = work.Context.SupplierAreas
                .Join(work.Context.Suppliers, sa => sa.SupplierID, s => s.ID, (sa, s) => new { sa, s })
                .Where(m => m.sa.AreaIds.Contains("," + shopModel.Shop_City + ",") && m.s.Sup_Is_Enable == 1 && rst_supplier.Contains(m.s.ID)).ToList().Select(m => m.sa).OrderByDescending(m => m.Sort).ToList();
            if (rst_city.Count() > 0)
                return rst_city[0].SupplierID;

            var rst_province = work.Context.SupplierAreas
                .Join(work.Context.Suppliers, sa => sa.SupplierID, s => s.ID, (sa, s) => new { sa, s })
                .Where(m => m.sa.AreaIds.Contains("," + shopModel.Shop_Province + ",") && m.s.Sup_Is_Enable == 1 && rst_supplier.Contains(m.s.ID)).ToList().Select(m => m.sa).OrderByDescending(m => m.Sort).ToList();
            if (rst_province.Count() > 0)
                return rst_province[0].SupplierID;

            //产品配置的默认供应商
            return goodsModel.SupplierID;

            ////如所有条件都不匹配，返回一个供应商
            //var rst_default = work.SupplierRepository.Get(m => m.Sup_Is_Enable == 1).FirstOrDefault();
            //return rst_default.ID;
        }

        #endregion
    }
}
