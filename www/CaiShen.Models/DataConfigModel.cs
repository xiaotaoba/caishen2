using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class NameValueModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    #region 商城价格比例实体

    /// <summary>
    /// 商城价格比例
    /// </summary>
    public class PricePercentItem
    {
        /// <summary>
        /// 成本总金额
        /// </summary>
        public decimal TotalCostPrice { get; set; }

        /// <summary>
        /// 加盟商价利润比
        /// </summary>
        public double ShopPricePercent { get; set; }
        /// <summary>
        /// 加盟商总价
        /// </summary>
        public decimal ShopPrice { get; set; }
        /// <summary>
        /// 终端客户利润比
        /// </summary>
        public double ClientPricePercent { get; set; }
        /// <summary>
        /// 终端客户总价
        /// </summary>
        public decimal ClientPrice { get; set; }

        /// <summary>
        /// 分销商利折扣
        /// </summary>
        public double FenxiaoPricePercent { get; set; }
        /// <summary>
        /// 分销商总价
        /// </summary>
        public decimal FenxiaoPrice { get; set; }

        /// <summary>
        /// 分销商利润总价
        /// </summary>
        public decimal FenxiaoProfitPrice { get; set; }
        /// <summary>
        /// 加盟商价利润
        /// </summary>
        public decimal ShopProfit { get; set; }
    }

    #endregion

    #region 白卡纸盒价格实体

    /// <summary>
    /// 白卡纸盒面积价格实体
    /// </summary>
    public class BaiKaAreaPriceModel
    {
        /// <summary>
        /// 单个展开面积，平方米
        /// </summary>
        public double UnitArea { get; set; }
        /// <summary>
        /// 数量成本价/终端价集合
        /// </summary>
        public List<BaiKaAreaCountPriceModel> CountCostPrice { get; set; }
    }
    /// <summary>
    /// 白卡纸盒面积数量价格实体
    /// </summary>
    public class BaiKaAreaCountPriceModel
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 成本平方价
        /// </summary>
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 报价平方价(终端销售价格)
        /// </summary>
        public decimal ClientPrice { get; set; }
    }


    #endregion

    #region 打样 价格（单价）逻辑实体

    #region 打样1-2实体
    /// <summary>
    /// 彩色包装软盒打样打样1-2实体
    /// </summary>
    public class DaYang1AreaPriceModel
    {
        /// <summary>
        /// 单个展开面积，平方毫米
        /// </summary>
        public double UnitArea { get; set; }
        /// <summary>
        /// 数量-价格列表
        /// </summary>
        public List<DaYang1CountPriceModel> CountPriceList { get; set; }
    }
    /// <summary>
    /// 彩色包装软盒打样
    /// </summary>
    public class DaYang1CountPriceModel
    {
        /// <summary>
        /// 打样数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 单个成本价格(覆膜)
        /// </summary>
        public decimal CostPrice1 { get; set; }
        /// <summary>
        /// 单个成本价格(不覆膜)
        /// </summary>
        public decimal CostPrice0 { get; set; }
    }
    #endregion

    #region 打样3-4实体

    /// <summary>
    /// 纸袋打样打样3-4实体
    /// </summary>
    public class DaYang2AreaPriceModel
    {
        /// <summary>
        /// 单个展开面积，平方毫米
        /// </summary>
        public double UnitArea { get; set; }
        /// <summary>
        /// 数量-价格列表
        /// </summary>
        public List<DaYang2CountPriceModel> CountPriceList { get; set; }
    }
    /// <summary>
    /// 纸袋打样
    /// </summary>
    public class DaYang2CountPriceModel
    {
        /// <summary>
        /// 打样数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 单个成本价格
        /// </summary>
        public decimal CostPrice { get; set; }
    }

    #endregion

    #endregion

    #region 定制产品，导入xls价格实体

    /// <summary>
    /// 定制产品，导入xls价格实体
    /// </summary>
    public class PriceResultModel
    {
        public PriceResultModel()
        {
            this.Freight = 0;
            this.UnitArea = 0;
            this.Volume = 0;
            this.Weight = 0;
            this.CostPrice = 0;
            this.CostTotalPrice = 0;
            this.ClientPrice = 0;
            this.ClientPriceRate = 0;
            this.MinClientPrice = 0;
            this.ShopPrice = 0;
            this.ShopPriceRate = 0;
        }
        /// <summary>
        /// 产品类型：1现货，2UV，3不干胶，4白卡彩盒
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 面积平方米,单个产品面积，确定价格区间
        /// </summary>
        public double UnitArea { get; set; }
        /// <summary>
        /// 单件体积（m³）
        /// </summary>
        public double Volume { get; set; }
        /// <summary>
        /// 单件重量（KG）
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 平米成本价
        /// </summary>
        public decimal SquareCostPrice { get; set; }
        /// <summary>
        /// 对应数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// count个成本价
        /// </summary>
        public decimal CostPrice { get; set; }
        /// <summary>
        /// count个总 成本总价 = 成本价+同城运费
        /// </summary>
        public decimal CostTotalPrice { get; set; }
        /// <summary>
        /// count个运费(同城)——对应购物车隐藏运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        ///加盟商价比例 ，加盟商价格= （成本+运费）*加盟商价比例
        /// </summary>
        public double ShopPriceRate { get; set; }
        /// <summary>
        /// count个加盟商价格
        /// </summary>
        public decimal ShopPrice { get; set; }
        /// <summary>
        /// 终端价格比例，终端价格= 加盟商价格*终端价格比例
        /// </summary>
        public double ClientPriceRate { get; set; }
        /// <summary>
        /// count个终端价
        /// </summary>
        public decimal ClientPrice { get; set; }
        /// <summary>
        /// 最低成本价
        /// </summary>
        public decimal MinCostPrice { get; set; }
        /// <summary>
        /// 最低总价(终端)
        /// </summary>
        public decimal MinClientPrice { get; set; }
    }

    #endregion


}
