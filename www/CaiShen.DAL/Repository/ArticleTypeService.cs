using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class ArticleTypeService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static ArticleType GetModel(int ID)
        {
            var list = work.ArticleTypeRepository.Get(m => m.ID == ID, null).ToList<ArticleType>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new ArticleType();
        }
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        public static string GetName(int ID)
        {
            var list = work.ArticleTypeRepository.Get(m => m.ID == ID, null).ToList<ArticleType>();
            if (list.Count() > 0)
            {
                return list[0].AT_Name;
            }
            return "无";
        }

        /// <summary>
        /// 获取类型+文章
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="top">子类个数</param>
        /// <param name="articleCount">文章条数</param>
        /// <returns></returns>
        public static List<ArticleTypeVModel> GetTypeWithArticle(int parentid, int top = 0, int articleCount = 0)
        {
            if (top == 0)
                top = 5;
            if (articleCount == 0)
                articleCount = 5;
            //分类,取5个
            List<ArticleType> articleTypes = work.Context.ArticleTypes.Where(m => m.AT_ParentID == parentid).OrderByDescending(m => m.AT_Sort).ThenBy(m => m.ID).Take(top).ToList();
            List<int> articleTypesIds = articleTypes.Select(m => m.ID).ToList();
            //所有分类下文章
            List<Article> articleList = work.Context.Articles.Where(m => m.Art_IsEnable == 1 && articleTypesIds.Contains(m.ArticleTypeID)).OrderByDescending(m => m.Art_Sort).ThenByDescending(m => m.Art_CreateTime).ToList();

            //分类集合
            List<ArticleTypeVModel> atVModelList = new List<ArticleTypeVModel>();
            foreach (var item in articleTypes)
            {
                ArticleTypeVModel atVModel = new ArticleTypeVModel();
                atVModel.ArticleType = item;
                atVModel.ArticleList = articleList.Where(m => m.ArticleTypeID == item.ID).Take(articleCount).ToList();

                atVModelList.Add(atVModel);
            }
            return atVModelList;
        }
    }
}
