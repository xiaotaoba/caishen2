using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class CouponService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static CouponInfo GetModel(int ID)
        {
            var list = work.CouponInfoRepository.Get(m => m.ID == ID, null).ToList<CouponInfo>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new CouponInfo();
        }

        #endregion

        #region  获取优惠券个数（可用）

        /// <summary>
        /// 获取优惠券个数（可用）
        /// </summary>
        /// <returns></returns>
        public static int GetCount(int UserID = 0)
        {
            var rst = work.Context.UserCoupons
              .Join(work.Context.CouponInfos, uc => uc.CouponInfoID, c => c.ID, (uc, c) => new { uc, c })
              .Where(m => m.uc.UCP_IsDelete == 0 && m.uc.UCP_Status == 0 && m.c.CP_EndTime > DateTime.Now && m.uc.UserID == UserID);
            return rst.Count();
        }

        #endregion

        #region  获取优惠券总金额（可用）

        /// <summary>
        /// 获取优惠券总金额（可用）
        /// </summary>
        /// <returns></returns>
        public static decimal GetTotalAmount(int UserID = 0)
        {
            var rst = work.Context.UserCoupons
              .Join(work.Context.CouponInfos, uc => uc.CouponInfoID, c => c.ID, (uc, c) => new { uc, c })
              .Where(m => m.uc.UCP_IsDelete == 0 && m.uc.UCP_Status == 0 && m.c.CP_EndTime > DateTime.Now && m.uc.UserID == UserID);
            if (rst != null && rst.Count() > 0)
            {
                return rst.Sum(m => m.c.CP_Amount);
            }
            return 0;
        }

        #endregion

        #region 获得本次购物可使用优惠券

        /// <summary>
        /// 获得本次购物可使用优惠券
        /// </summary>
        /// <param name="ShopID"></param>
        /// <param name="UserID"></param>
        /// <param name="BuyGoodsList"></param>
        /// <param name="totalAmount">总订单费用（不含运费、未减免）</param>
        /// <returns></returns>
        public static List<CouponInfo> GetListOfOrder(int ShopID, int UserID, List<Goods> BuyGoodsList, decimal totalAmount)
        {
            //用户所有可用优惠券
            var rst = work.Context.UserCoupons
             .Join(work.Context.CouponInfos, uc => uc.CouponInfoID, c => c.ID, (uc, c) => new { uc, c })
             .Where(m => m.uc.UCP_IsDelete == 0 && m.uc.UCP_Status == 0 && m.c.CP_BeginTime < DateTime.Now && m.c.CP_EndTime > DateTime.Now && m.uc.UserID == UserID)
             .Where(m => m.c.CP_Status != 2 && m.c.CP_Status != 3 && m.c.CP_IsDelete == 0)//未结束，未终止，未删除
             .Where(m => m.c.CP_NeedAmount <= totalAmount)//购物达到满减额度
             .Select(m => m.c).OrderByDescending(m => m.CP_Amount);



            if (rst != null && rst.Count() > 0)
            {
                List<CouponInfo> useful_list = new List<CouponInfo>();//本次可用优惠券列表
                foreach (CouponInfo item in rst.ToList())
                {
                    if (item.CP_UsingRange == Convert.ToInt32(DataConfig.CouponUsingRangeEnum.平台通用类))
                    {
                        useful_list.Add(item);
                    }
                    else if (item.CP_UsingRange == Convert.ToInt32(DataConfig.CouponUsingRangeEnum.店铺通用类))
                    {
                        int[] usingRangeIds = UtilityClass.ConvertIntArr(item.CP_UsingRangeIds);
                        if (usingRangeIds.Contains(ShopID))//适用门店
                        {
                            useful_list.Add(item);
                        }

                    }
                    //else if (item.CP_UsingRange == Convert.ToInt32(DataConfig.CouponUsingRangeEnum.品类通用类))
                    //{
                    //    int[] usingRangeIds = UtilityClass.ConvertIntArr(item.CP_UsingRangeIds);
                    //    foreach (int categoryId in usingRangeIds)
                    //    {
                    //        foreach (Goods goodsItem in BuyGoodsList)
                    //        {
                    //            if (goodsItem.GoodsCategoryID == categoryId)
                    //            {
                    //                useful_list.Add(item);
                    //            }
                    //        }
                    //    }
                    //}
                    //else if (item.CP_UsingRange == Convert.ToInt32(DataConfig.CouponUsingRangeEnum.特定商品使用))
                    //{
                    //    int[] usingRangeIds = UtilityClass.ConvertIntArr(item.CP_UsingRangeIds);
                    //    foreach (int goodsId in usingRangeIds)
                    //    {
                    //        foreach (Goods goodsItem in BuyGoodsList)
                    //        {
                    //            if (goodsItem.ID == goodsId)
                    //            {
                    //                useful_list.Add(item);
                    //            }
                    //        }
                    //    }
                    //}
                }

                return useful_list;
            }

            return null;
        }

        #endregion

        #region 更新红包使用状态

        /// <summary>
        /// 更新红包使用状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="status">0未使用，1已使用</param>
        /// <returns></returns>
        public static bool UpdateStatus(int ID, int status)
        {
            UserHongBao model = work.UserHongBaoRepository.GetByID(ID);
            if (model != null)
            {
                model.UHB_Status = status;

                work.UserHongBaoRepository.Update(model);
                work.Save();
            }
            return true;
        }

        #endregion


    }
}
