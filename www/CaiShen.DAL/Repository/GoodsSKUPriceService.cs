using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class GoodsSKUPriceService
    {
        private static UnitOfWork work = new UnitOfWork();

        #region  获取产品SKUPrice价格(SKU价格配置) Json

        /// <summary>
        /// 获取产品SKUPrice价格(SKU价格配置的单价) Json
        /// </summary>
        /// <param name="goodsid"></param>
        /// <param name="skuid"></param>
        /// <param name="skucount"></param>
        /// <returns></returns>
        public static object GetSKUPrice(int goodsid = 0, int skuid = 0, int skucount = 0)
        {
            GoodsSKUPriceResult skuPriceRst = GetSKUPriceResult(goodsid, skuid, skucount);
            return new { status = skuPriceRst.status, msg = skuPriceRst.msg, skuprice = skuPriceRst.skuprice, shopprice = skuPriceRst.shopprice, costprice = skuPriceRst.costprice };
        }

        /// <summary>
        /// 获取产品SKUPrice价格(SKU价格配置的单价) 结果
        /// </summary>
        /// <param name="goodsid"></param>
        /// <param name="skuid"></param>
        /// <param name="skucount"></param>
        /// <returns></returns>
        public static GoodsSKUPriceResult GetSKUPriceResult(int goodsid = 0, int skuid = 0, int skucount = 0)
        {
            List<GoodsSKUPrice> list = null;
            GoodsSKUPriceResult jsonData = new GoodsSKUPriceResult { status = "success", msg = "", skuprice = 0, shopprice = 0, costprice = 0 };

            //门店所属定价区域
            GoodsPriceArea priceAreaModel = GoodsPriceAreaService.GetGoodsPriceArea(UserShopService.GetCurrentShop());
            //没有所属定价区域，查询默认价格配置
            if (priceAreaModel == null || priceAreaModel.ID == 0)
            {
                //默认——价格配置
                list = work.Context.GoodsSKUPrices.Where(m => m.SKUID == skuid).OrderByDescending(m => m.ID).ToList();
            }
            else
            {
                //指定定价区域——价格配置
                list = work.Context.GoodsSKUPrices.Where(m => m.SKUID == skuid && m.GoodsPriceAreaID == priceAreaModel.ID).OrderByDescending(m => m.ID).ToList();

                if (list == null || list.Count < 1)
                {
                    //默认——价格配置
                    list = work.Context.GoodsSKUPrices.Where(m => m.SKUID == skuid).OrderByDescending(m => m.ID).ToList();
                }
            }
            //没有价格配置
            if (list == null || list.Count < 1)
            {
            }
            else
            {
                GoodsSKUPrice existModel = list.Where(m => m.Count > skucount || m.Count == skucount).OrderBy(m => m.Count).FirstOrDefault();
                if (existModel == null)
                {
                    existModel = list.OrderByDescending(m => m.Count).FirstOrDefault();
                }
                jsonData = new GoodsSKUPriceResult { status = "success", msg = "", skuprice = GoodsService.GetFinalPrice(existModel.ClientPrice / existModel.Count, existModel.ShopPrice / existModel.Count, goodsid), shopprice = existModel.ShopPrice / existModel.Count, costprice = existModel.CostTotalPrice / existModel.Count };
            }
            return jsonData;
        }
        #endregion
    }
}
