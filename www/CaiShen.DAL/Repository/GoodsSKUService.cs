using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;

namespace Pannet.DAL.Repository
{
    public class GoodsSKUService
    {
        private static UnitOfWork work = new UnitOfWork();

        /// <summary>
        /// 获取数据库GoodsSKU集合
        /// </summary>
        /// <param name="goodsid"></param>
        /// <returns></returns>
        public static List<GoodsSKUVModel> GetSKU(int goodsid)
        {
            List<GoodsSKU> list = work.Context.GoodsSKUs.AsNoTracking().Where(m => m.GoodsID == goodsid).ToList();
            List<GoodsSKUVModel> listB = new List<GoodsSKUVModel>();
            foreach (var item in list)
            {
                listB.Add(new GoodsSKUVModel
                {
                    Properties = item.Properties,
                    PropertiesName = item.PropertiesName,
                    Count = item.SKU_Count,
                    GoodsCode = item.SKU_GoodsCode,
                    ShopCode = item.SKU_ShopCode,
                    Price = item.SKU_Price,
                    CostPrice = item.SKU_CostPrice,
                    ShopPrice = item.SKU_ShopPrice,
                    GoodsID = item.GoodsID,
                    Volume = item.SKU_Volume,
                    Weight = item.SKU_Weight,
                    DistributorPrice = item.SKU_DistributorPrice,
                    ExpandArea = item.SKU_ExpandArea,
                    SquareWeight = item.SKU_SquareWeight,
                    ShopPriceRate = item.SKU_ShopPriceRate,
                    ClientPriceRate = item.SKU_ClientPriceRate,
                    ID = item.ID
                });
            }
            return listB;
        }
        /// <summary>
        /// 获取客户端显示的GoodsSKU集合（销售价格做了处理）
        /// </summary>
        /// <param name="goodsid"></param>
        /// <returns></returns>
        public static List<GoodsSKUVModel> GetSKUWithFinalPrice(int goodsid)
        {
            //Goods goodsModel = work.GoodsRepository.GetByID(goodsid);
            List<GoodsSKU> list = work.Context.GoodsSKUs.AsNoTracking().Where(m => m.GoodsID == goodsid).ToList();
            List<GoodsSKUVModel> listB = new List<GoodsSKUVModel>();
            foreach (var item in list)
            {

                listB.Add(new GoodsSKUVModel
                {
                    Properties = item.Properties,
                    PropertiesName = item.PropertiesName,
                    Count = item.SKU_Count,
                    GoodsCode = item.SKU_GoodsCode,
                    ShopCode = item.SKU_ShopCode,
                    //Price = goodsModel.G_IsExist == 1 ? item.SKU_Price : item.SKU_CostPrice,//如果是非现货，使用价格比例表计算
                    //20170913，调整除非标品，其他都按后台输入，不使用成本价格比例
                    //Price = item.SKU_Price, 2017-11-22修改，店铺使用折扣后价格
                    Price = GoodsService.GetFinalPrice(item.SKU_Price, item.SKU_ShopPrice, goodsid),
                    ShopPrice = item.SKU_ShopPrice,
                    CostPrice = item.SKU_CostPrice,
                    GoodsID = item.GoodsID,
                    Volume = item.SKU_Volume,
                    Weight = item.SKU_Weight,
                    DistributorPrice = item.SKU_DistributorPrice,
                    ExpandArea = item.SKU_ExpandArea,
                    SquareWeight = item.SKU_SquareWeight,
                    ShopPriceRate = item.SKU_ShopPriceRate,
                    ClientPriceRate = item.SKU_ClientPriceRate,
                    ID = item.ID
                });
            }
            return listB;
        }
    }
}
