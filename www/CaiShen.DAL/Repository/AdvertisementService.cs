using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class AdvertisementService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Advertisement GetModel(int ID)
        {
            var list = work.Context.Advertisements.AsNoTracking().Where(m => m.ID == ID).ToList<Advertisement>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new Advertisement();
        }

        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="typeid">广告类型</param>
        /// <param name="top">数量</param>
        /// <returns></returns>
        public static List<Advertisement> GetByTypeID(int typeid, int top)
        {
            return work.Context.Advertisements.AsNoTracking().Where(m => m.AdvertisementTypeID == typeid).OrderByDescending(m => m.AD_Sort).ThenBy(m => m.ID).Take(top).ToList();
        }

        public static string GetState(Advertisement adModel)
        {
            if (adModel != null)
            {
                if (adModel.AD_State == Convert.ToInt16(DataConfig.AdvertisementStateEnum.活动已关闭))
                {
                    return "活动已关闭";
                }
                else if (adModel.AD_State == Convert.ToInt16(DataConfig.AdvertisementStateEnum.活动已结束) || adModel.AD_ActivityEndTime < DateTime.Now)
                {
                    return "活动已结束";
                }
                else if (adModel.AD_State == Convert.ToInt16(DataConfig.AdvertisementStateEnum.活动已开始) || adModel.AD_ActivityBeginTime < DateTime.Now)
                {
                    return "活动已开始";
                }
                else if (adModel.AD_State == Convert.ToInt16(DataConfig.AdvertisementStateEnum.报名结束) || adModel.AD_EndTime < DateTime.Now)
                {
                    return "报名结束";
                }
                else if (adModel.AD_State == Convert.ToInt16(DataConfig.AdvertisementStateEnum.已满员))
                {
                    return "已满员";
                }
                else if (adModel.AD_State == Convert.ToInt16(DataConfig.AdvertisementStateEnum.报名中) || (adModel.AD_BeginTime < DateTime.Now && adModel.AD_EndTime > DateTime.Now))
                {
                    return "报名中";
                }
                else if (adModel.AD_State == Convert.ToInt16(DataConfig.AdvertisementStateEnum.未开始) || adModel.AD_BeginTime > DateTime.Now)
                {
                    return "即将开始";
                }
            }
            return "";
        }
    }
}
