using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class OrderCommentService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static OrderComment GetModel(int ID)
        {
            var list = work.OrderCommentRepository.Get(m => m.ID == ID, null).ToList<OrderComment>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new OrderComment();
        }

        /// <summary>
        /// 获取产品评价数
        /// </summary>
        /// <returns></returns>
        public static int GetCommentCount(int ShopID = 0, int GoodsID = 0)
        {
            var rs = work.Context.OrderComment
                  .Join(work.Context.OrderDetails, oc => oc.OrderDetailID, od => od.ID, (oc, od) => new { oc.OrderDetailID, od })
                  .Join(work.Context.GoodsSKUs, m => m.od.GoodsSKUID, GSKU => GSKU.ID, (m, GSKU) => new { m.od,m.OrderDetailID,GSKU.GoodsID});
            if (ShopID == 0)
            {
                rs = rs.Where(m => m.GoodsID == GoodsID);
            }
            else {
                rs = rs.Where(m => m.od.UserShopID==ShopID && m.GoodsID == GoodsID);
            }
                
            return rs.Count();
        }
    }
}
