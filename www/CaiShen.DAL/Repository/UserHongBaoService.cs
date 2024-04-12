using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserHongBaoService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static UserHongBao GetModel(int ID)
        {
            var list = work.UserHongBaoRepository.Get(m => m.ID == ID, null).ToList<UserHongBao>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new UserHongBao();
        }

        #endregion

        #region  获取红包个数（可用）

        /// <summary>
        /// 获取红包个数（可用）
        /// </summary>
        /// <returns></returns>
        public static int GetHongBaoCount(int UserID = 0)
        {
            var rs = work.Context.UserHongBaos.Where(m => m.UserID == UserID && m.UHB_ExpirationTime > DateTime.Now && m.UHB_Status == 0 && m.UHB_IsDelete == 0);
            return rs.Count();
        }

        #endregion

        #region  获取红包总金额（可用）

        /// <summary>
        /// 获取红包总金额（可用）
        /// </summary>
        /// <returns></returns>
        public static decimal GetHongBaoTotalAmount(int UserID = 0)
        {
            var rst = work.Context.UserHongBaos.Where(m => m.UserID == UserID && m.UHB_ExpirationTime > DateTime.Now && m.UHB_Status == 0 && m.UHB_IsDelete == 0);
            if (rst != null && rst.Count() > 0)
            {
                return rst.Sum(m => m.UBH_Amount);
            }
            return 0;
        }

        #endregion

        #region 获得本次购物可使用红包

        /// <summary>
        /// 获得本次购物可使用红包
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="BuyGoodsList"></param>
        /// <param name="ShopID">0 保留</param>
        /// <returns></returns>
        public static List<UserHongBao> GetListOfOrder(int UserID, int ShopID = 0)
        {
            //用户所有可用红包
            var rst = work.Context.UserHongBaos
             .Where(m => m.UHB_IsDelete == 0 && m.UHB_Status == 0 && m.UHB_ExpirationTime > DateTime.Now && m.UserID == UserID);//未删除、未使用、未过期

            if (rst != null && rst.Count() > 0)
            {
                return rst.ToList();
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
