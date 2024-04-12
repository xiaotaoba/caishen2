using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class GoodsCategoryService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public static GoodsCategory GetModel(int ID)
        {
            var list = work.GoodsCategoryRepository.Get(m => m.ID == ID, null).ToList<GoodsCategory>();
            if (list.Count() > 0)
            {
                return list[0];
            }
            return new GoodsCategory();
        }
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        public static string GetName(int ID)
        {
            if (ID == 0)
            {
                return "无";
            }
            var list = work.Context.GoodsCategorys.AsNoTracking().Where(m => m.ID == ID).ToList<GoodsCategory>();
            if (list.Count() > 0)
            {
                return list[0].GC_Name;
            }
            return "不存在";
        }

        ///// <summary>
        ///// 获取头部推荐分类+产品
        ///// </summary>
        ///// <param name="top"></param>
        ///// <returns></returns>
        //public static List<GoodsCategoryHeadVModel> GetHeadCategorys(int top)
        //{
        //    int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);//
        //    if (top == 0)
        //        top = 5;
        //    //一级分类,取5个
        //    List<GoodsCategory> goodsCategory = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0).OrderByDescending(m => m.GC_Sort).ThenBy(m => m.ID).Take(top).ToList();

        //    //一级分类集合
        //    List<GoodsCategoryHeadVModel> gcVModelList = new List<GoodsCategoryHeadVModel>();
        //    foreach (var item in goodsCategory)
        //    {
        //        //一级分类
        //        GoodsCategoryHeadVModel gcVModel = new GoodsCategoryHeadVModel();
        //        //一级类所有子类+当前类ID
        //        List<GoodsCategory> subGoodsCategory = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == item.ID).ToList();
        //        List<int> listSubGoodsCategoryIds = subGoodsCategory.Select(m => m.ID).ToList();
        //        listSubGoodsCategoryIds.Add(item.ID);

        //        //一级类所有推荐产品
        //        List<Goods> GoodsList = work.GoodsRepository.Get(m => m.G_Status == goodsStatusOn && listSubGoodsCategoryIds.Contains(m.GoodsCategoryID) && m.G_IsRecommend == 1).ToList();

        //        //二级分类集合
        //        List<GoodsCategoryHeadVModel> gcVModelListSub = new List<GoodsCategoryHeadVModel>();

        //        foreach (var itemSub in subGoodsCategory)
        //        {
        //            //二级分类
        //            GoodsCategoryHeadVModel gcVModelSub = new GoodsCategoryHeadVModel();

        //            //二级类所有推荐产品
        //            List<Goods> GoodsListSub = work.GoodsRepository.Get(m => m.G_Status == goodsStatusOn && m.GoodsCategoryID == itemSub.ID && m.G_IsRecommend == 1).ToList();
        //            gcVModelSub.GoodsCategory = itemSub;
        //            gcVModelSub.GoodsList = GoodsListSub;
        //            gcVModelSub.GoodsCategorysListSub = null;

        //            gcVModelListSub.Add(gcVModelSub);
        //        }

        //        gcVModel.GoodsCategory = item;
        //        gcVModel.GoodsList = GoodsList;
        //        gcVModel.GoodsCategorysListSub = gcVModelListSub;


        //        gcVModelList.Add(gcVModel);
        //    }
        //    return gcVModelList;
        //}

        #region 获取头部现货专区分类+产品

        /// <summary>
        /// 获取头部现货专区分类+产品
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public static List<GoodsCategoryHeadVModel> GetHeadCategorys(int top)
        {
            int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);
            if (top == 0)
                top = 5;

            //一级分类,取5个
            List<GoodsCategory> goodsCategory = work.Context.GoodsCategorys.Where(m => m.GC_ParentID == 0).OrderByDescending(m => m.GC_Sort).ThenBy(m => m.ID).Take(top).ToList();
            List<int> goodsCategoryIds = goodsCategory.Select(m => m.ID).ToList();
            //所有二级分类 
            List<GoodsCategory> subGoodsCategoryAll = work.Context.GoodsCategorys.Where(m => goodsCategoryIds.Contains(m.GC_ParentID)).ToList();
            List<int> subGoodsCategoryAllIds = subGoodsCategoryAll.Select(m => m.ID).ToList();
            //合并一级分类ID
            subGoodsCategoryAllIds = subGoodsCategoryAllIds.Union(goodsCategoryIds).ToList();
            //所有推荐产品
            List<Goods> GoodsListAll = work.Context.Goods.Where(m => m.G_Status == goodsStatusOn && subGoodsCategoryAllIds.Contains(m.GoodsCategoryID) && m.G_IsRecommend == 1).ToList();

            //一级分类实体集合
            List<GoodsCategoryHeadVModel> gcVModelList = new List<GoodsCategoryHeadVModel>();
            foreach (var item in goodsCategory)
            {
                //单个一级分类
                GoodsCategoryHeadVModel gcVModel = new GoodsCategoryHeadVModel();

                if (item.ID == 36)//现货
                {
                    //Log.WriteLog(string.Format("==================================\r\n进入现货：{0}", item.GC_Name), "Goods", DateTime.Now.ToString("yyyyMMdd"));
                    //现货分类下 --所有推荐产品
                    List<Goods> GoodsList = work.Context.Goods.Where(m => m.G_Status == goodsStatusOn && m.G_IsRecommend == 1 && m.G_IsExist == 1).OrderByDescending(m => m.G_Sort).ThenByDescending(m => m.ID).ToList();
                    //Log.WriteLog(string.Format("==================================\r\n推荐产品个数：{0}", GoodsList.Count), "Goods", DateTime.Now.ToString("yyyyMMdd"));

                    //包含现货产品的其他分类 + 产品集合
                    List<GoodsCategoryHeadVModel> gcVModelListSub = new List<GoodsCategoryHeadVModel>();

                    //包含现货产品的其他分类
                    List<GoodsCategory> subGoodsCategory = work.Context.GoodsCategorys
                        .Join(work.Context.Goods, gc => gc.ID, g => g.GoodsCategoryID, (gc, g) => new { gc, g })
                        .Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsExist == 1 && m.g.G_IsRecommend == 1)
                        .Select(m => m.gc).Distinct().Take(6).ToList();

                    foreach (var itemSub in subGoodsCategory)
                    {
                        //二级分类
                        GoodsCategoryHeadVModel gcVModelSub = new GoodsCategoryHeadVModel();

                        gcVModelSub.GoodsCategory = itemSub;
                        //当前二级类 - 所有推荐产品
                        gcVModelSub.GoodsList = GoodsList.Where(m => m.GoodsCategoryID == itemSub.ID).ToList();
                        gcVModelSub.GoodsCategorysListSub = null;

                        gcVModelListSub.Add(gcVModelSub);
                    }
                    //Log.WriteLog(string.Format("==================================\r\n推荐分类个数：{0}", gcVModelListSub.Count), "Goods", DateTime.Now.ToString("yyyyMMdd"));

                    gcVModel.GoodsCategory = item;
                    gcVModel.GoodsList = GoodsList.Take(6).ToList();
                    gcVModel.GoodsCategorysListSub = gcVModelListSub;
                }
                else
                {
                    //一级类所有子类+当前类ID
                    List<GoodsCategory> subGoodsCategory = subGoodsCategoryAll.Where(m => m.GC_ParentID == item.ID).ToList();
                    //List<int> listSubGoodsCategoryIds = subGoodsCategory.Select(m => m.ID).ToList();
                    //listSubGoodsCategoryIds.Add(item.ID);

                    //一级类（包含其二级分类）下所有推荐产品
                    //List<Goods> GoodsList = work.Context.Goods.Where(m => m.G_Status == goodsStatusOn && listSubGoodsCategoryIds.Contains(m.GoodsCategoryID) && m.G_IsRecommend == 1).ToList();
                    //List<Goods> GoodsList = GoodsListAll.Where(m => listSubGoodsCategoryIds.Contains(m.GoodsCategoryID)).ToList();


                    //二级分类集合
                    List<GoodsCategoryHeadVModel> gcVModelListSub = new List<GoodsCategoryHeadVModel>();

                    foreach (var itemSub in subGoodsCategory)
                    {
                        //单个二级分类
                        GoodsCategoryHeadVModel gcVModelSub = new GoodsCategoryHeadVModel();

                        gcVModelSub.GoodsCategory = itemSub;
                        gcVModelSub.GoodsList = GoodsListAll.Where(m => m.GoodsCategoryID == itemSub.ID).ToList();
                        gcVModelSub.GoodsCategorysListSub = null;

                        gcVModelListSub.Add(gcVModelSub);
                    }

                    gcVModel.GoodsCategory = item;
                    gcVModel.GoodsList = GoodsListAll.Where(m => m.GoodsCategoryID == item.ID).ToList();
                    gcVModel.GoodsCategorysListSub = gcVModelListSub;
                }

                gcVModelList.Add(gcVModel);
            }
            return gcVModelList;
        }

        #endregion

        #region 获取头部现货专区分类+产品 -备份

        /// <summary>
        /// 获取头部现货专区分类+产品
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public static List<GoodsCategoryHeadVModel> GetHeadCategorysBak(int top)
        {
            int goodsStatusOn = Convert.ToInt16(DataConfig.GoodsStatusEnum.上架);//
            if (top == 0)
                top = 5;

            //一级分类,取5个
            List<GoodsCategory> goodsCategory = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == 0).OrderByDescending(m => m.GC_Sort).ThenBy(m => m.ID).Take(top).ToList();

            //一级分类集合
            List<GoodsCategoryHeadVModel> gcVModelList = new List<GoodsCategoryHeadVModel>();
            foreach (var item in goodsCategory)
            {
                //一级分类
                GoodsCategoryHeadVModel gcVModel = new GoodsCategoryHeadVModel();

                if (item.ID == 36)//现货
                {
                    //Log.WriteLog(string.Format("==================================\r\n进入现货：{0}", item.GC_Name), "Goods", DateTime.Now.ToString("yyyyMMdd"));
                    //现货下推荐产品推荐产品
                    List<Goods> GoodsList = work.GoodsRepository.Get(m => m.G_Status == goodsStatusOn && m.G_IsRecommend == 1 && m.G_IsExist == 1).OrderByDescending(m => m.G_Sort).ThenByDescending(m => m.ID).Take(6).ToList();

                    //Log.WriteLog(string.Format("==================================\r\n推荐产品个数：{0}", GoodsList.Count), "Goods", DateTime.Now.ToString("yyyyMMdd"));

                    //包含现货产品的其他分类 + 产品集合
                    List<GoodsCategoryHeadVModel> gcVModelListSub = new List<GoodsCategoryHeadVModel>();

                    //包含现货产品的其他分类
                    List<GoodsCategory> subGoodsCategory = work.Context.GoodsCategorys
                        .Join(work.Context.Goods, gc => gc.ID, g => g.GoodsCategoryID, (gc, g) => new { gc, g })
                        .Where(m => m.g.G_Status == goodsStatusOn && m.g.G_IsExist == 1 && m.g.G_IsRecommend == 1)
                        .Select(m => m.gc).Distinct().Take(6).ToList();

                    foreach (var itemSub in subGoodsCategory)
                    {
                        //二级分类
                        GoodsCategoryHeadVModel gcVModelSub = new GoodsCategoryHeadVModel();

                        //二级类所有推荐产品
                        List<Goods> GoodsListSub = work.GoodsRepository.Get(m => m.G_Status == goodsStatusOn && m.GoodsCategoryID == itemSub.ID && m.G_IsRecommend == 1 && m.G_IsExist == 1).Take(12).ToList();
                        gcVModelSub.GoodsCategory = itemSub;
                        gcVModelSub.GoodsList = GoodsListSub;
                        gcVModelSub.GoodsCategorysListSub = null;

                        gcVModelListSub.Add(gcVModelSub);
                    }
                    //Log.WriteLog(string.Format("==================================\r\n推荐分类个数：{0}", gcVModelListSub.Count), "Goods", DateTime.Now.ToString("yyyyMMdd"));

                    gcVModel.GoodsCategory = item;
                    gcVModel.GoodsList = GoodsList;
                    gcVModel.GoodsCategorysListSub = gcVModelListSub;
                }
                else
                {
                    //一级类所有子类+当前类ID
                    List<GoodsCategory> subGoodsCategory = work.GoodsCategoryRepository.Get(m => m.GC_ParentID == item.ID).ToList();
                    List<int> listSubGoodsCategoryIds = subGoodsCategory.Select(m => m.ID).ToList();
                    listSubGoodsCategoryIds.Add(item.ID);

                    //一级类所有推荐产品
                    List<Goods> GoodsList = work.GoodsRepository.Get(m => m.G_Status == goodsStatusOn && listSubGoodsCategoryIds.Contains(m.GoodsCategoryID) && m.G_IsRecommend == 1).ToList();

                    //二级分类集合
                    List<GoodsCategoryHeadVModel> gcVModelListSub = new List<GoodsCategoryHeadVModel>();

                    foreach (var itemSub in subGoodsCategory)
                    {
                        //二级分类
                        GoodsCategoryHeadVModel gcVModelSub = new GoodsCategoryHeadVModel();

                        //二级类所有推荐产品
                        List<Goods> GoodsListSub = work.GoodsRepository.Get(m => m.G_Status == goodsStatusOn && m.GoodsCategoryID == itemSub.ID && m.G_IsRecommend == 1).ToList();
                        gcVModelSub.GoodsCategory = itemSub;
                        gcVModelSub.GoodsList = GoodsListSub;
                        gcVModelSub.GoodsCategorysListSub = null;

                        gcVModelListSub.Add(gcVModelSub);
                    }

                    gcVModel.GoodsCategory = item;
                    gcVModel.GoodsList = GoodsList;
                    gcVModel.GoodsCategorysListSub = gcVModelListSub;
                }

                gcVModelList.Add(gcVModel);
            }
            return gcVModelList;
        }

        #endregion
    }
}
