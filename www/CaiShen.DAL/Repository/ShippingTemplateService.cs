using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ShippingTemplateService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static ShippingTemplate GetModel(int ID)
        {
            var model = work.ShippingTemplateRepository.Get(m => m.ID == ID, null).FirstOrDefault();
            if (model != null)
            {
                return model;
            }
            return new ShippingTemplate();
        }

        #region 计算邮费 - 单品 - 已作废（2017-11-14） 使用OrderService.GetShippingFee方法

        ///// <summary>
        ///// 计算邮费 - 单品
        ///// </summary>
        ///// <param name="shopModel">门店</param>
        ///// <param name="goodsID">产品ID</param>
        ///// <param name="count">购买数量</param>
        ///// <param name="totalPrice">总价</param>
        ///// <param name="firstFee">起步价</param>
        ///// <returns></returns>
        //public static decimal GetShippingFee(UserShop shopModel, int goodsID, int count, decimal totalPrice, out decimal firstFee)
        //{
        //    firstFee = 0;
        //    //当前购买产品
        //    Goods goodsModel = new Goods();
        //    if (goodsID != 0)
        //    {
        //        goodsModel = work.Context.Goods.AsNoTracking().Where(m => m.ID == goodsID).FirstOrDefault();
        //        if (goodsModel == null || goodsModel.G_IsFreeShipping == 1)
        //            return 0;
        //    }

        //    var rstModel = work.ShippingTemplateRepository.Get(m => m.ID == goodsModel.G_ShippingTemplateID && m.ST_Is_Enable == 1).FirstOrDefault();
        //    if (rstModel == null || rstModel.ST_IsFree == 1)//包邮模板
        //        return 0;

        //    //decimal firstFee = 0;
        //    var rst_area = work.ShippingAreaRepository.Get(m => m.ShippingTemplateID == goodsModel.G_ShippingTemplateID).ToList();
        //    ShippingArea shippingAreaModel = null;

        //    //按区域（区-市-省顺序）匹配运费配送地区
        //    var rst_region = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_Region))).ToList();
        //    if (rst_region.Count() > 0)
        //    {
        //        shippingAreaModel = rst_region[0];
        //        return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    }

        //    var rst_city = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_City))).ToList();
        //    if (rst_city.Count() > 0)
        //    {
        //        shippingAreaModel = rst_city[0];
        //        return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    }
        //    var rst_province = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_Province))).ToList();
        //    if (rst_province.Count() > 0)
        //    {
        //        shippingAreaModel = rst_province[0];
        //        return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    }

        //    //没有配送地区，使用默认
        //    var rst_default = rst_area.Where(m => m.SA_Is_Default == 1).ToList();
        //    if (rst_default.Count() > 0)
        //    {
        //        shippingAreaModel = rst_default[0];
        //        return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    }

        //    //if (shippingAreaModel != null)
        //    //{
        //    //    return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    //}

        //    return 0;
        //}

        ///// <summary>
        ///// 计算邮费 - 单品
        ///// </summary>
        ///// <param name="userAddressID">用户地址ID</param>
        ///// <param name="goodsID">产品ID</param>
        ///// <param name="count">购买数量</param>
        ///// <param name="totalPrice">总价</param>
        ///// <param name="firstFee">起步价</param>
        ///// <returns></returns>
        //public static decimal GetShippingFee(int userAddressID, int goodsID, int count, decimal totalPrice, out decimal firstFee)
        //{
        //    firstFee = 0;
        //    //地址模板
        //    UserAddress addressModel = work.UserAddressRepository.GetByID(userAddressID);
        //    if (addressModel == null)
        //        addressModel = new UserAddress();

        //    //当前购买产品
        //    Goods goodsModel = new Goods();
        //    if (goodsID != 0)
        //    {
        //        goodsModel = work.Context.Goods.AsNoTracking().Where(m => m.ID == goodsID).FirstOrDefault();
        //        if (goodsModel == null || goodsModel.G_IsFreeShipping == 1)
        //            return 0;
        //    }

        //    var rstModel = work.ShippingTemplateRepository.Get(m => m.ID == goodsModel.G_ShippingTemplateID && m.ST_Is_Enable == 1).FirstOrDefault();
        //    if (rstModel == null || rstModel.ST_IsFree == 1)//包邮模板
        //        return 0;

        //    //decimal firstFee = 0;
        //    var rst_area = work.ShippingAreaRepository.Get(m => m.ShippingTemplateID == goodsModel.G_ShippingTemplateID).ToList();
        //    ShippingArea shippingAreaModel = null;

        //    //按区域（区-市-省顺序）匹配运费配送地区
        //    var rst_region = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", addressModel.Region))).ToList();
        //    if (rst_region.Count() > 0)
        //    {
        //        shippingAreaModel = rst_region[0];
        //        return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    }

        //    var rst_city = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", addressModel.City))).ToList();
        //    if (rst_city.Count() > 0)
        //    {
        //        shippingAreaModel = rst_city[0];
        //        return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    }
        //    var rst_province = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", addressModel.Province))).ToList();
        //    if (rst_province.Count() > 0)
        //    {
        //        shippingAreaModel = rst_province[0];
        //        return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    }

        //    //没有配送地区，使用默认
        //    var rst_default = rst_area.Where(m => m.SA_Is_Default == 1).ToList();
        //    if (rst_default.Count() > 0)
        //    {
        //        shippingAreaModel = rst_default[0];
        //        return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    }

        //    //if (shippingAreaModel != null)
        //    //{
        //    //    return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
        //    //}

        //    return 0;
        //}

        #endregion

      
    }
}
