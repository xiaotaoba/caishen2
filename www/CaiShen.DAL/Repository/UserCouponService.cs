using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserCouponService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static UserCoupon GetModel(int ID)
        {
            var list = work.UserCouponRepository.Get(m => m.ID == ID, null).ToList<UserCoupon>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new UserCoupon();
        }

        #endregion

        #region 更新优惠券为已使用状态

        /// <summary>
        /// 更新优惠券为已使用状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CouponID"></param>
        /// <returns></returns>
        public static bool UpdateStatusUsed(int UserID, int CouponID)
        {
            UserCoupon model = work.UserCouponRepository.Get(m => m.UserID == UserID && m.CouponInfoID == CouponID && m.UCP_Status == 0).FirstOrDefault();
            if (model != null && model.ID != 0)
            {
                model.UCP_Status = 1;

                work.UserCouponRepository.Update(model);
                work.Save();
            }
            return true;
        }

        #endregion

    }
}
