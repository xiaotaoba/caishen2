using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class GoodsSKUPrice
    {
        public GoodsSKUPrice()
        {
            this.ClientPrice = 0;
            this.ClientPriceRate = 1;
            this.CostPrice = 0;
            this.CostTotalPrice = 0;
            this.Count = 1;
            this.DistributorPrice = 0;
            this.Freight = 0;
            this.ShopPrice = 0;
            this.ShopPriceRate = 1;
            this.SKUID = 0;
            this.TaoPrice = 0;
            this.GoodsPriceAreaID = 0;
        }
        public int ID { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int GoodsID { get; set; }

        /// <summary>
        /// SKUID
        /// </summary>
        public int SKUID { get; set; }

        /// <summary>
        /// 产品定价区域ID
        /// </summary>
        public int GoodsPriceAreaID { get; set; }

        /// <summary>
        /// 默认0：终端价=加盟商价格*终端价比例(1.3)，可自定义终端价
        /// </summary>
        public decimal ClientPrice { get; set; }

        /// <summary>
        /// 终端价比例
        /// </summary>
        public double ClientPriceRate { get; set; }

        /// <summary>
        /// 默认0,加盟商价 = 成本总价*加盟商价比例,可自定义
        /// </summary>
        public decimal ShopPrice { get; set; }

        /// <summary>
        /// 加盟商价比例
        /// </summary>
        public double ShopPriceRate { get; set; }

        /// <summary>
        /// 成本价格
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 默认0：成本价格+运费，可自定义价格
        /// </summary>
        public decimal CostTotalPrice { get; set; }

        /// <summary>
        /// 淘宝价格
        /// </summary>
        public decimal TaoPrice { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 分销商价
        /// </summary>
        public decimal DistributorPrice { get; set; }

    }

    public class GoodsSKUPriceVModel
    {
        public GoodsSKUPrice GoodsSKUPrice { get; set; }
        public string GoodsPriceAreaTitle { get; set; }
    }

    public class GoodsSKUPriceResult
    {
        /// <summary>
        /// json获取SKUPrice价格结果状态,success或error
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 结果提示内容
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 成本单个SKU终端价格
        /// </summary>
        public decimal skuprice { get; set; }
        /// <summary>
        /// 成本单个SKU加盟商价格
        /// </summary>
        public decimal shopprice { get; set; }
        /// <summary>
        /// 成本单个SKU成本价格
        /// </summary>
        public decimal costprice { get; set; }
    }
}
