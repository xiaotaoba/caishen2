using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class UserScoreHistoryService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 新增积分变动记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="score">调整积分</param>
        /// <param name="totalscore">剩余总积分</param>
        /// <param name="lockscore">调整不可用积分</param>
        /// <param name="locktotalscore">剩余总不可用积分</param>
        /// <param name="type">1增加，0减少</param>
        /// <param name="category">分类</param>
        /// <param name="thing">说明</param>
        /// <param name="recordid">相关记录id</param>
        /// <param name="_operator">操作人</param>
        /// <returns></returns>
        public static bool Insert(int userid, int score, int totalscore, int lockscore, int locktotalscore, int type, string category, string thing, int recordid, string _operator)
        {
            //添加调整记录
            UserScoreHistory newModel = new UserScoreHistory();
            newModel.UserID = userid;
            newModel.Score = score;
            newModel.LockScore = lockscore;
            newModel.Category = category;
            newModel.Is_Delete = 0;
            newModel.Time = DateTime.Now;
            newModel.RecordID = recordid;
            newModel.TotalScore = totalscore;
            newModel.LockTotalScore = locktotalscore;
            newModel.Thing = thing;
            newModel.Type = type;
            newModel.Operator = _operator;

            work.UserScoreHistoryRepository.Insert(newModel);
            work.Save();
            return true;
        }

    }
}
