using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ArticleService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static Article GetModel(int ID)
        {
            var list = work.ArticleRepository.Get(m => m.ID == ID, null).ToList<Article>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new Article();
        }
       
    }
}
