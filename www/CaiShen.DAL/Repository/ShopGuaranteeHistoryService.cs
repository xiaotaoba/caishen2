using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ShopGuaranteeHistoryService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 保障金额度 变动记录
        /// </summary>
        /// <param name="shopid"></param>
        /// <param name="money">调整额度</param>
        /// <param name="totalmoney">保障金额度</param>
        /// <param name="restmoney">剩余可用额度</param>
        /// <param name="type">1增加，0减少</param>
        /// <param name="category">分类</param>
        /// <param name="thing">说明</param>
        /// <param name="recordid">相关记录id</param>
        /// <param name="_operator">操作人</param>
        /// <returns></returns>
        public static bool Insert(int shopid, decimal money, decimal totalmoney, decimal restmoney, int type, string category, string thing, int recordid, string _operator)
        {
            //添加调整记录
            ShopGuaranteeHistory newModel = new ShopGuaranteeHistory();
            newModel.UserShopID = shopid;
            newModel.Amount = money;
            newModel.Category = category;
            newModel.Is_Delete = 0;
            newModel.Time = DateTime.Now;
            newModel.RecordID = recordid;
            newModel.TotalAmount = totalmoney;
            newModel.RestAmount = restmoney;
            newModel.Thing = thing;
            newModel.Type = type;
            newModel.Operator = _operator;

            work.ShopGuaranteeHistoryRepository.Insert(newModel);
            work.Save();
            return true;
        }

    }
}
