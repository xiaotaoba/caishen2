using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserAmountHistoryService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 新增金额变动记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="amount">调整金额</param>
        /// <param name="totalamount">剩余总金额</param>
        /// <param name="amount">调整锁定金额</param>
        /// <param name="totalamount">剩余锁定总金额</param>
        /// <param name="type">1增加，0减少</param>
        /// <param name="category">分类</param>
        /// <param name="thing">说明</param>
        /// <param name="recordid">相关记录id</param>
        /// <param name="_operator">操作人</param>
        /// <param name="_operator">备注：第三方平台信息</param>
        /// <returns></returns>
        public static bool Insert(int userid, decimal amount, decimal totalamount, decimal lockamount, decimal locktotalamount, int type, string category, string thing, int recordid, string _operator, string remark = "")
        {
            //添加调整记录
            UserAmountHistory newModel = new UserAmountHistory();
            newModel.UserID = userid;
            newModel.Amount = amount;
            newModel.Category = category;
            newModel.Is_Delete = 0;
            newModel.Time = DateTime.Now;
            newModel.RecordID = recordid;
            newModel.TotalAmount = totalamount;
            newModel.Thing = thing;
            newModel.Type = type;
            newModel.Operator = _operator;
            newModel.Remark = remark;
            newModel.LockAmount = lockamount;
            newModel.LockTotalAmount = locktotalamount;

            work.UserAmountHistoryRepository.Insert(newModel);
            work.Save();
            return true;
        }

        /// <summary>
        /// 是否包含相关记录
        /// </summary>
        /// <param name="thing">订单号，交易单号等数据</param>
        /// <returns></returns>
        public static bool ExistThing(string thing)
        {
            var rst = work.UserAmountHistoryRepository.Get(m => m.Thing.Contains(thing)).ToList();
            if (rst.Count > 0)
            {
                return true;
            }
            return false;
        }

    }
}
