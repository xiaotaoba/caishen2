using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ShippingAreaService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static ShippingArea GetModel(int ID)
        {
            var model = work.ShippingAreaRepository.Get(m => m.ID == ID, null).FirstOrDefault();
            if (model != null)
            {
                return model;
            }
            return new ShippingArea();
        }
        #endregion

        #region 计算邮费 - 单个产品

        #region 201711前一版计费方式，已作废
        ///// <summary>
        ///// 计算邮费 - 单个产品--201711前一版计费方式，已作废
        ///// </summary>
        ///// <param name="shippingArea">运费模板配送地区</param>
        ///// <param name="goods">产品</param>
        ///// <param name="count">购买数量</param>
        ///// <param name="totalPrice">产品总价</param>
        ///// <param name="firstFee">返回首件（重、体积）最大价格</param>
        ///// <returns></returns>
        //public static decimal GetShippingFee(ShippingArea shippingArea, Goods goods, int count, decimal totalPrice, out decimal firstFee)
        //{
        //    double totalWeight = goods.G_Weight * count;
        //    double totalVolume = goods.G_Volume * count;
        //    decimal totalCountFee = 0;
        //    decimal totalWeightFee = 0;
        //    decimal totalVolumeFee = 0;
        //    //返回邮费（最大值）
        //    decimal rstFee = 0;
        //    firstFee = 0;

        //    //包邮
        //    if (shippingArea.SA_IsFree == 1)
        //    {
        //        //价格包邮
        //        if (shippingArea.SA_IsPriceFree == 1)
        //        {
        //            if (totalPrice <= shippingArea.SA_PriceFree)
        //            {
        //                return 0;
        //            }
        //        }
        //        //体积包邮
        //        if (shippingArea.SA_IsVolumeFree == 1)
        //        {
        //            if (totalVolume <= shippingArea.SA_VolumeFree)
        //            {
        //                return 0;
        //            }
        //        }
        //        //重量包邮
        //        if (shippingArea.SA_IsWeightFree == 1)
        //        {
        //            if (totalWeight <= shippingArea.SA_WeightFree)
        //            {
        //                return 0;
        //            }
        //        }
        //        //件数包邮
        //        if (shippingArea.SA_IsCountFree == 1)
        //        {
        //            if (count >= shippingArea.SA_CountFree)
        //            {
        //                return 0;
        //            }
        //        }
        //    }

        //    //不符合包邮条件，仍然走不包邮计算
        //    //按件
        //    if (shippingArea.SA_CountFirst < count && shippingArea.SA_ReCount > 0)
        //    {
        //        //续件次数
        //        int recount = (count - shippingArea.SA_CountFirst + 1) / shippingArea.SA_ReCount + 1;
        //        totalCountFee = shippingArea.SA_CountFirstFee + shippingArea.SA_ReCountFee * recount;
        //    }
        //    else
        //    {
        //        totalCountFee = shippingArea.SA_CountFirstFee;
        //    }
        //    rstFee = totalCountFee;
        //    firstFee = shippingArea.SA_CountFirstFee;

        //    //按重量
        //    if (shippingArea.SA_WeightFirst < totalWeight && shippingArea.SA_ReWeight > 0)
        //    {
        //        //续次数
        //        double recount = (totalWeight - shippingArea.SA_WeightFirst) / shippingArea.SA_ReWeight;
        //        if (recount != Math.Floor(recount))
        //        {
        //            recount = Math.Floor(recount) + 1;
        //        }
        //        totalWeightFee = shippingArea.SA_WeightFirstFee + shippingArea.SA_ReWeightFee * Convert.ToInt32(recount);
        //    }
        //    else
        //    {
        //        totalWeightFee = shippingArea.SA_WeightFirstFee;
        //    }
        //    rstFee = rstFee > totalWeightFee ? rstFee : totalWeightFee;
        //    firstFee = firstFee > shippingArea.SA_WeightFirstFee ? firstFee : shippingArea.SA_WeightFirstFee;

        //    //按体积
        //    if (shippingArea.SA_VolumeFirst < totalVolume && shippingArea.SA_ReVolume > 0)
        //    {
        //        //续次数
        //        double recount = (totalVolume - shippingArea.SA_VolumeFirst) / shippingArea.SA_ReVolume;
        //        if (recount != Math.Floor(recount))
        //        {
        //            recount = Math.Floor(recount) + 1;
        //        }
        //        totalVolumeFee = shippingArea.SA_VolumeFirstFee + shippingArea.SA_ReVolumeFee * Convert.ToInt32(recount);
        //    }
        //    else
        //    {
        //        totalVolumeFee = shippingArea.SA_VolumeFirstFee;
        //    }
        //    rstFee = rstFee > totalVolumeFee ? rstFee : totalVolumeFee;
        //    firstFee = firstFee > shippingArea.SA_VolumeFirstFee ? firstFee : shippingArea.SA_VolumeFirstFee;

        //    return rstFee;
        //}
        #endregion

        /// <summary>
        /// 计算邮费 - 单个产品--201711114版
        /// </summary>
        /// <param name="shippingArea">运费模板配送地区</param>
        /// <param name="goods">产品</param>
        /// <param name="count">购买数量</param>
        /// <param name="totalPrice">产品总价</param>
        /// <param name="firstFee">返回首件（重、体积）最大价格</param>
        /// <returns></returns>
        public static decimal GetShippingFee(ShippingArea shippingArea, Goods goods, int count, decimal totalPrice, out decimal firstFee)
        {
            double totalWeight = goods.G_Weight * count;
            double totalVolume = goods.G_Volume * count;
            decimal totalCountFee = 0;
            decimal totalWeightFee = 0;
            decimal totalVolumeFee = 0;
            //返回邮费（最大值）
            decimal rstFee = 0;
            firstFee = 0;

            //包邮
            if (shippingArea.SA_IsFree == 1)
            {
                //价格包邮
                if (shippingArea.SA_IsPriceFree == 1)
                {
                    if (totalPrice <= shippingArea.SA_PriceFree)
                    {
                        return 0;
                    }
                }
                //体积包邮
                if (shippingArea.SA_IsVolumeFree == 1)
                {
                    if (totalVolume <= shippingArea.SA_VolumeFree)
                    {
                        return 0;
                    }
                }
                //重量包邮
                if (shippingArea.SA_IsWeightFree == 1)
                {
                    if (totalWeight <= shippingArea.SA_WeightFree)
                    {
                        return 0;
                    }
                }
                //件数包邮
                if (shippingArea.SA_IsCountFree == 1)
                {
                    if (count >= shippingArea.SA_CountFree)
                    {
                        return 0;
                    }
                }
            }

            //不符合包邮条件，仍然走不包邮计算
            //按件
            if (shippingArea.SA_CountFirst < count && shippingArea.SA_ReCount > 0)
            {
                //续件次数
                int recount = (count - shippingArea.SA_CountFirst + 1) / shippingArea.SA_ReCount + 1;
                totalCountFee = shippingArea.SA_CountFirstFee + shippingArea.SA_ReCountFee * recount;
            }
            else
            {
                totalCountFee = shippingArea.SA_CountFirstFee;
            }
            rstFee = totalCountFee;
            firstFee = shippingArea.SA_CountFirstFee;

            //按重量
            if (shippingArea.SA_WeightFirst < totalWeight && shippingArea.SA_ReWeight > 0)
            {
                //续次数
                double recount = (totalWeight - shippingArea.SA_WeightFirst) / shippingArea.SA_ReWeight;
                if (recount != Math.Floor(recount))
                {
                    recount = Math.Floor(recount) + 1;
                }
                totalWeightFee = shippingArea.SA_WeightFirstFee + shippingArea.SA_ReWeightFee * Convert.ToInt32(recount);
            }
            else
            {
                totalWeightFee = shippingArea.SA_WeightFirstFee;
            }
            rstFee = rstFee > totalWeightFee ? rstFee : totalWeightFee;
            firstFee = firstFee > shippingArea.SA_WeightFirstFee ? firstFee : shippingArea.SA_WeightFirstFee;

            //按体积
            if (shippingArea.SA_VolumeFirst < totalVolume && shippingArea.SA_ReVolume > 0)
            {
                //续次数
                double recount = (totalVolume - shippingArea.SA_VolumeFirst) / shippingArea.SA_ReVolume;
                if (recount != Math.Floor(recount))
                {
                    recount = Math.Floor(recount) + 1;
                }
                totalVolumeFee = shippingArea.SA_VolumeFirstFee + shippingArea.SA_ReVolumeFee * Convert.ToInt32(recount);
            }
            else
            {
                totalVolumeFee = shippingArea.SA_VolumeFirstFee;
            }
            rstFee = rstFee > totalVolumeFee ? rstFee : totalVolumeFee;
            firstFee = firstFee > shippingArea.SA_VolumeFirstFee ? firstFee : shippingArea.SA_VolumeFirstFee;

            return rstFee;
        }

        #endregion

        #region 计算邮费 - 总费用

        /// <summary>
        /// 计算邮费 - 按物流-重量计费
        /// </summary>
        /// <param name="shippingArea">运费模板配送地区</param>
        /// <param name="weight">重量</param>
        /// <returns></returns>
        public static decimal GetWeightFee(ShippingArea shippingArea, double weight)
        {
            double totalWeight = weight;
            decimal totalWeightFee = 0;

            //按重量
            if (shippingArea.SA_WeightFirst < totalWeight && shippingArea.SA_ReWeight > 0)
            {
                //续次数
                double recount = (totalWeight - shippingArea.SA_WeightFirst) / shippingArea.SA_ReWeight;
                if (recount != Math.Floor(recount))
                {
                    recount = Math.Floor(recount) + 1;
                }
                totalWeightFee = shippingArea.SA_WeightFirstFee + shippingArea.SA_ReWeightFee * Convert.ToInt32(recount);
                WriteLog(string.Format("totalWeightFee：{0}，SA_ReWeightFee：{1}，recount：{2}", totalWeightFee, shippingArea.SA_ReWeightFee, recount));
            }
            else
            {
                totalWeightFee = shippingArea.SA_WeightFirstFee;
            }
            if (totalWeightFee < shippingArea.SA_WeightMinFee)
            {
                totalWeightFee = shippingArea.SA_WeightMinFee;
            }
            return totalWeightFee;
        }

        /// <summary>
        /// 计算邮费 - 按物流-体积计费
        /// </summary>
        /// <param name="shippingArea">运费模板配送地区</param>
        /// <param name="volume">体积</param>
        /// <returns></returns>
        public static decimal GetVolumeFee(ShippingArea shippingArea, double volume)
        {
            double totalVolume = volume;
            decimal totalVolumeFee = 0;

            //按体积
            if (shippingArea.SA_VolumeFirst < totalVolume && shippingArea.SA_ReVolume > 0)
            {
                //续次数
                double recount = (totalVolume - shippingArea.SA_VolumeFirst) / shippingArea.SA_ReVolume;
                if (recount != Math.Floor(recount))
                {
                    recount = Math.Floor(recount) + 1;
                }
                totalVolumeFee = shippingArea.SA_VolumeFirstFee + shippingArea.SA_ReVolumeFee * Convert.ToInt32(recount);
                WriteLog(string.Format("totalVolumeFee：{0}，SA_ReVolumeFee：{1}，recount：{2}", totalVolumeFee, shippingArea.SA_ReVolumeFee, recount));
            }
            else
            {
                totalVolumeFee = shippingArea.SA_VolumeFirstFee;
            }
            if (totalVolumeFee < shippingArea.SA_VolumeMinFee)
            {
                totalVolumeFee = shippingArea.SA_VolumeMinFee;
            }
            return totalVolumeFee;
        }
        /// <summary>
        /// 计算邮费 - 按快递计费
        /// </summary>
        /// <param name="shippingArea">运费模板配送地区</param>
        /// <param name="weight">重量</param>
        /// <returns></returns>
        public static decimal GetExpressFee(ShippingArea shippingArea, double weight)
        {
            double totalWeight = weight;
            decimal totalExpressFee = 0;

            //按体积
            if (shippingArea.SA_ExpressFirst < totalWeight && shippingArea.SA_ReExpress > 0)
            {
                //续次数
                double recount = (totalWeight - shippingArea.SA_ExpressFirst) / shippingArea.SA_ReExpress;
                if (recount != Math.Floor(recount))
                {
                    recount = Math.Floor(recount) + 1;
                }
                totalExpressFee = shippingArea.SA_ExpressFirstFee + shippingArea.SA_ReExpressFee * Convert.ToInt32(recount);
                WriteLog(string.Format("totalExpressFee：{0}，SA_ReExpressFee：{1}，recount：{2}", totalExpressFee, shippingArea.SA_ReExpressFee, recount));
            }
            else
            {
                totalExpressFee = shippingArea.SA_ExpressFirstFee;
            }

            return totalExpressFee;
        }

        #endregion

        #region 重货是否是重

        /// <summary>
        /// 重货是否是重,快递大于物流：重，快递小于物流：轻
        /// </summary>
        /// <param name="shippingArea"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static bool IsWeightLot(ShippingArea shippingArea, double weight)
        {
            if (GetExpressFee(shippingArea, weight) > GetWeightFee(shippingArea, weight))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 泡货是否是体积大

        /// <summary>
        /// 泡货是否是体积大,快递大于物流：大，快递小于物流：小
        /// </summary>
        /// <param name="shippingArea"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static bool IsVolumeBig(ShippingArea shippingArea, double weight, double volume)
        {
            if (GetExpressFee(shippingArea, weight) > GetVolumeFee(shippingArea, volume))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 根据收货地址ID分配运费区域

        /// <summary>
        /// 根据收货地址
        /// </summary>
        /// <param name="shippingTemplateID">运费模板ID</param>
        /// <param name="userAddressID">用户地址ID</param>
        /// <returns></returns>
        public static ShippingArea GetShippingArea(int shippingTemplateID, int userAddressID)
        {
            //地址模板
            UserAddress addressModel = work.UserAddressRepository.GetByID(userAddressID);
            return GetShippingArea(shippingTemplateID, addressModel);
        }

        /// <summary>
        /// 根据收货地址
        /// </summary>
        /// <param name="shippingTemplateID">运费模板ID</param>
        /// <param name="userAddressID">用户地址ID</param>
        /// <returns></returns>
        public static ShippingArea GetShippingArea(int shippingTemplateID, UserAddress addressModel)
        {
            if (addressModel == null)
                return null;

            //var rstModel = work.ShippingTemplateRepository.Get(m => m.ID == shippingTemplateID && m.ST_Is_Enable == 1).FirstOrDefault();
            //if (rstModel == null || rstModel.ST_IsFree == 1)//包邮模板
            //    return null;

            //decimal firstFee = 0;
            var rst_area = work.ShippingAreaRepository.Get(m => m.ShippingTemplateID == shippingTemplateID).ToList();

            //按区域（区-市-省顺序）匹配运费配送地区
            var rst_region = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", addressModel.Region))).ToList();
            if (rst_region.Count() > 0)
            {
                return rst_region[0];
            }

            var rst_city = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", addressModel.City))).ToList();
            if (rst_city.Count() > 0)
            {
                return rst_city[0];
            }

            var rst_province = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", addressModel.Province))).ToList();
            if (rst_province.Count() > 0)
            {
                return rst_province[0];
            }

            //没有配送地区，使用默认
            var rst_default = rst_area.Where(m => m.SA_Is_Default == 1).ToList();
            if (rst_default.Count() > 0)
            {
                return rst_default[0];
            }

            //if (shippingAreaModel != null)
            //{
            //    return ShippingAreaService.GetShippingFee(shippingAreaModel, goodsModel, count, totalPrice, out firstFee);
            //}

            return null;
        }

        #endregion

        #region 根据门店地址__分配运费区域

        /// <summary>
        /// 根据门店地址__分配运费区域
        /// </summary>
        /// <param name="shippingTemplateID">运费模板ID</param>
        /// <param name="shopModel">门店实体</param>
        /// <returns></returns>
        public static ShippingArea GetShippingArea(int shippingTemplateID, UserShop shopModel)
        {

            var rst_area = work.ShippingAreaRepository.Get(m => m.ShippingTemplateID == shippingTemplateID).ToList();

            //按区域（区-市-省顺序）匹配运费配送地区
            var rst_region = rst_area.Where(m => m.AreaIds.Contains(string.Format(",{0},", shopModel.Shop_Region))).ToList();
            if (rst_region.Count() > 0)
            {
                return rst_region[0];
            }

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

            //没有配送地区，使用默认
            var rst_default = rst_area.Where(m => m.SA_Is_Default == 1).ToList();
            if (rst_default.Count() > 0)
            {
                return rst_default[0];
            }
            return null;
        }

        #endregion

        #region 根据已知：重货重量，泡货重量，泡货体积，计算价格

        /// <summary>
        /// 根据已知：重货重量，泡货重量，泡货体积，计算价格
        /// </summary>
        /// <param name="w_totalWeight">重货总重量(KG)</param>
        /// <param name="l_totalWeight">泡货总重量(KG)</param>
        /// <param name="l_totalVolume">泡货总体积(m³)</param>
        /// <returns>总的运费</returns>
        public static decimal GetShippingFee(double w_totalWeight, double l_totalWeight, double l_totalVolume, ShippingArea areaModel)
        {
            decimal totalFee = 0;
            WriteLog(string.Format("进入：GetShippingFee，w_totalWeight：{0}，l_totalWeight：{1}，l_totalVolume：{2}", w_totalWeight, l_totalWeight, l_totalVolume));

            //B.混合重货产品和泡货产品时
            if (w_totalWeight > 0 && l_totalVolume > 0)
            {
                //B1：重货产品重量轻+泡货产品体积小----重量汇总，按快递算
                if (!ShippingAreaService.IsWeightLot(areaModel, w_totalWeight) && !ShippingAreaService.IsVolumeBig(areaModel, l_totalWeight, l_totalVolume))
                {
                    totalFee = totalFee + ShippingAreaService.GetExpressFee(areaModel, w_totalWeight + l_totalWeight);
                    WriteLog(string.Format("B1:totalFee：{0}，w_totalWeight：{1}，l_totalWeight：{2}", totalFee, w_totalWeight, l_totalWeight));
                }
                //B2：重货产品重量轻+泡货产品体积大----重货产品按快递算+轻货产品按体积算， 相加，提送货费取一次加，提送货费取一次
                else if (!ShippingAreaService.IsWeightLot(areaModel, w_totalWeight) && ShippingAreaService.IsVolumeBig(areaModel, l_totalWeight, l_totalVolume))
                {
                    totalFee = totalFee + ShippingAreaService.GetExpressFee(areaModel, w_totalWeight) + ShippingAreaService.GetVolumeFee(areaModel, l_totalVolume);
                    WriteLog(string.Format("B2:totalFee：{0}，w_totalWeight：{1}，l_totalWeight：{2}", totalFee, w_totalWeight, l_totalWeight));
                    //提货费
                    if (l_totalVolume < Convert.ToDouble(areaModel.SA_VolumeTihuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_VolumeTihuoFee;
                        WriteLog(string.Format("B2提货费:totalFee：{0}，SA_VolumeTihuoFee：{1}，l_totalVolume：{2}", totalFee, areaModel.SA_VolumeTihuoFree, l_totalVolume));
                    }
                    //送货费
                    if (l_totalVolume < Convert.ToDouble(areaModel.SA_VolumeSonghuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_VolumeSonghuoFee;
                        WriteLog(string.Format("B2送货费:totalFee：{0}，SA_VolumeTihuoFee：{1}，l_totalVolume：{2}", totalFee, areaModel.SA_VolumeSonghuoFee, l_totalVolume));
                    }
                }
                //B3：重货产品重量重+泡货产品体积小----重货产品和轻货产品重量汇总，按物流 重量算，相加，提送货费取一次加，提送货费取一次
                else if (ShippingAreaService.IsWeightLot(areaModel, w_totalWeight) && !ShippingAreaService.IsVolumeBig(areaModel, l_totalWeight, l_totalVolume))
                {
                    totalFee = totalFee + ShippingAreaService.GetWeightFee(areaModel, w_totalWeight + l_totalWeight);
                    WriteLog(string.Format("B3:totalFee：{0}，w_totalWeight：{1}，l_totalWeight：{2}", totalFee, w_totalWeight, l_totalWeight));
                    //提货费
                    if ((w_totalWeight + l_totalWeight) < Convert.ToDouble(areaModel.SA_WeightTihuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_WeightTihuoFee;
                        // Log.WriteLog(string.Format("B3提货费:totalFee：{0}，SA_WeightTihuoFree：{1}，l_totalWeight：{2}", totalFee, areaModel.SA_WeightTihuoFree, l_totalWeight));
                    }
                    //送货费
                    if ((w_totalWeight + l_totalWeight) < Convert.ToDouble(areaModel.SA_WeightSonghuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_WeightSonghuoFee;
                        WriteLog(string.Format("B3送货费:totalFee：{0}，SA_WeightSonghuoFee：{1}，l_totalWeight：{2}", totalFee, areaModel.SA_WeightSonghuoFee, l_totalWeight));
                    }
                }
                //B4：重货产品重量重+泡货产品体积大----重货产品按物流重量算，轻货产品按物 流体积算，相加，提送货费取一次加，提送货费取一次
                else if (ShippingAreaService.IsWeightLot(areaModel, w_totalWeight) && ShippingAreaService.IsVolumeBig(areaModel, l_totalWeight, l_totalVolume))
                {
                    #region 重货-重

                    totalFee = totalFee + ShippingAreaService.GetWeightFee(areaModel, w_totalWeight);
                    WriteLog(string.Format("B4重货-重:totalFee：{0}，w_totalWeight：{1}，l_totalWeight：{2}", totalFee, w_totalWeight, l_totalWeight));

                    //提货费
                    if (w_totalWeight < Convert.ToDouble(areaModel.SA_WeightTihuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_WeightTihuoFee;
                        WriteLog(string.Format("B4重货-提货费:totalFee：{0}，SA_WeightTihuoFee：{1}，l_totalWeight：{2}", totalFee, areaModel.SA_WeightTihuoFee, l_totalWeight));
                    }
                    //送货费
                    if (w_totalWeight < Convert.ToDouble(areaModel.SA_WeightSonghuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_WeightSonghuoFee;
                        WriteLog(string.Format("B4重货-送货费:totalFee：{0}，SA_WeightSonghuoFee：{1}，l_totalWeight：{2}", totalFee, areaModel.SA_WeightSonghuoFee, l_totalWeight));
                    }
                    #endregion

                    #region 泡货 -体积

                    totalFee = totalFee + ShippingAreaService.GetVolumeFee(areaModel, l_totalVolume);
                    WriteLog(string.Format("B4泡货 -体积:totalFee：{0}，l_totalVolume：{1}，l_totalWeight：{2}", totalFee, l_totalVolume, l_totalWeight));

                    //提货费
                    if (l_totalVolume < Convert.ToDouble(areaModel.SA_VolumeTihuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_VolumeTihuoFee;
                        WriteLog(string.Format("B4泡货 -提货费:totalFee：{0}，SA_VolumeTihuoFee：{1}，l_totalVolume：{2}", totalFee, areaModel.SA_VolumeTihuoFee, l_totalVolume));
                    }
                    //送货费
                    if (l_totalVolume < Convert.ToDouble(areaModel.SA_VolumeSonghuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_VolumeSonghuoFee;
                        WriteLog(string.Format("B4泡货 -送货费:totalFee：{0}，SA_VolumeSonghuoFee：{1}，l_totalVolume：{2}", totalFee, areaModel.SA_VolumeSonghuoFee, l_totalVolume));
                    }

                    #endregion
                }

            }
            //A1.只有重货
            else if (w_totalWeight > 0)
            {
                //重-物流
                if (ShippingAreaService.IsWeightLot(areaModel, w_totalWeight))
                {
                    totalFee = totalFee + ShippingAreaService.GetWeightFee(areaModel, w_totalWeight);
                    WriteLog(string.Format("A1.只有重货-重-物流:totalFee：{0}，w_totalWeight：{1}，l_totalWeight：{2}", totalFee, w_totalWeight, l_totalWeight));
                    //提货费
                    if (w_totalWeight < Convert.ToDouble(areaModel.SA_WeightTihuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_WeightTihuoFee;
                        WriteLog(string.Format("A1.只有重货-重-提货费:totalFee：{0}，w_totalWeight：{1}，areaModel.SA_WeightTihuoFee：{2}", totalFee, w_totalWeight, areaModel.SA_WeightTihuoFee));
                    }
                    //送货费
                    if (w_totalWeight < Convert.ToDouble(areaModel.SA_WeightSonghuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_WeightSonghuoFee;
                        WriteLog(string.Format("A1.只有重货-重-送货费:totalFee：{0}，w_totalWeight：{1}，areaModel.SA_WeightSonghuoFree：{2}", totalFee, w_totalWeight, areaModel.SA_WeightSonghuoFree));
                    }
                }
                //轻-快递
                else
                {
                    totalFee = totalFee + ShippingAreaService.GetExpressFee(areaModel, w_totalWeight);
                    WriteLog(string.Format("A1.只有重货-轻-快递:totalFee：{0}，w_totalWeight：{1}，l_totalWeight：{2}", totalFee, w_totalWeight, l_totalWeight));
                }
            }
            //A2.只有泡货
            else if (l_totalVolume > 0)
            {
                //体积大-物流
                if (ShippingAreaService.IsVolumeBig(areaModel, l_totalWeight, l_totalVolume))
                {
                    totalFee = totalFee + ShippingAreaService.GetVolumeFee(areaModel, l_totalVolume);
                    WriteLog(string.Format("A2.只有泡货-体积大:totalFee：{0}，l_totalWeight：{1}，l_totalVolume：{2}", totalFee, l_totalWeight, l_totalVolume));
                    //提货费
                    if (l_totalVolume < Convert.ToDouble(areaModel.SA_VolumeTihuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_VolumeTihuoFee;
                        WriteLog(string.Format("A2.只有泡货-提货费:totalFee：{0}，areaModel.SA_VolumeTihuoFee：{1}，l_totalVolume：{2}", totalFee, areaModel.SA_VolumeTihuoFee, l_totalVolume));
                    }
                    //送货费
                    if (l_totalVolume < Convert.ToDouble(areaModel.SA_VolumeSonghuoFree))
                    {
                        totalFee = totalFee + areaModel.SA_VolumeSonghuoFee;
                        WriteLog(string.Format("A2.只有泡货-送货费:totalFee：{0}，areaModel.SA_VolumeSonghuoFee：{1}，l_totalVolume：{2}", totalFee, areaModel.SA_VolumeSonghuoFee, l_totalVolume));
                    }
                }
                //体积小-快递
                else
                {
                    totalFee = totalFee + ShippingAreaService.GetExpressFee(areaModel, l_totalWeight);
                    WriteLog(string.Format("A2.只有泡货-体积小:totalFee：{0}，l_totalWeight：{1}，l_totalVolume：{2}", totalFee, l_totalWeight, l_totalVolume));
                }

            }

            return totalFee;
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
                Log.WriteLog(content, "fee", DateTime.Now.ToString("yyyyMMdd"));
            }
        }
    }
}
