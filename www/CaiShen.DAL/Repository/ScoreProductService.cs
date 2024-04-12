using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ScoreProductService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region 获取实体

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static ScoreProduct GetModel(int ID)
        {
            var list = work.ScoreProductRepository.Get(m => m.ID == ID, null).ToList<ScoreProduct>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new ScoreProduct();
        }

        #endregion
    }
}
