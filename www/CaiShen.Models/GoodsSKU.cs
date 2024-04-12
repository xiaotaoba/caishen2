using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class GoodsSKU
    {
        public GoodsSKU()
        {
            this.SKU_Price = 0;
            this.SKU_HasMorePrice = 0;
            this.SKU_CostPrice = 0;
            this.SKU_DistributorPrice = 0;
            this.SKU_ShopPrice = 0;
            this.SKU_Volume = 0;
            this.SKU_Weight = 0;
            this.SKU_ExpandArea = 0;
            this.SKU_SquareWeight = 0;
            this.SKU_ClientPriceRate = 1.3;
            this.SKU_ShopPriceRate = 1.1;

        }
        public int ID { get; set; }

        [Display(Name = "产品ID")]
        public int GoodsID { get; set; }

        /// <summary>
        /// 格式：名ID:值ID;名2ID:值2ID;
        /// </summary>
        [Display(Name = "属性名值对ID字符串")]
        public string Properties { get; set; }
        /// <summary>
        /// 格式：值名称1_值名称2_值名称3
        /// </summary>
        [Display(Name = "属性值名称字符串")]
        public string PropertiesName { get; set; }

        /// <summary>
        /// 销售价格/终端价=加盟商价格*终端价比例，可自定义终端价
        /// </summary>
        [Display(Name = "价格")]
        public decimal SKU_Price { get; set; }
        /// <summary>
        /// 终端价比例，默认1.3
        /// </summary>
        public double SKU_ClientPriceRate { get; set; }
        /// <summary>
        /// 加盟商价/门店 = 成本总价*加盟商价比例,可自定义
        /// </summary>
        [Display(Name = "加盟商价")]
        public decimal SKU_ShopPrice { get; set; }
        /// <summary>
        /// 加盟商价比例，默认1.1
        /// </summary>
        public double SKU_ShopPriceRate { get; set; }
        /// <summary>
        /// 分销商价
        /// </summary>
        [Display(Name = "分销商价")]
        public decimal SKU_DistributorPrice { get; set; }

        /// <summary>
        /// 成本价格
        /// </summary>
        [Display(Name = "成本价格")]
        public decimal SKU_CostPrice { get; set; }

        /// <summary>
        /// 库存量
        /// </summary>
        [Display(Name = "库存量")]
        public int SKU_Count { get; set; }

        /// <summary>
        /// 备用字段
        /// </summary>
        [StringLength(20)]
        [Display(Name = "商家编码")]
        public string SKU_ShopCode { get; set; }

        /// <summary>
        /// 备用字段
        /// </summary>
        [StringLength(20)]
        [Display(Name = "产品编码")]
        public string SKU_GoodsCode { get; set; }

        /// <summary>
        ///体积(m³)
        /// </summary>
        [Display(Name = "体积(m³)")]
        public double SKU_Volume { get; set; }

        /// <summary>
        /// 重量(kg)
        /// </summary>
        [Display(Name = "重量(kg)")]
        public double SKU_Weight { get; set; }

        /// <summary>
        ///展开面积(m²)
        /// </summary>
        [Display(Name = "展开面积(m²)")]
        public double SKU_ExpandArea { get; set; }

        /// <summary>
        /// 平方克重，1平方米产品的重量
        /// </summary>
        [Display(Name = "平方克重(g)")]
        public double SKU_SquareWeight { get; set; }

        /// <summary>
        /// 当前SKU是否有更多阶梯优惠价格，购买数量越多，价格越便宜，0否，1是
        /// </summary>
        public int SKU_HasMorePrice { get; set; }
    }

    public class GoodsSKUVModel
    {
        public GoodsSKUVModel()
        {
            this.Price = 0;
            this.GoodsID = 0;
            this.Price = 0;
            this.CostPrice = 0;
            this.ShopPrice = 0;
            this.Weight = 0;
            this.Volume = 0;
            this.ExpandArea = 0;
            this.SquareWeight = 0;
            this.ClientPriceRate = 1.3;
            this.ShopPriceRate = 1.1;
        }
        public int ID { get; set; }
        public int GoodsID { get; set; }
        /// <summary>
        /// 属性名值对ID字符串 格式：名ID:值ID;名2ID:值2ID;
        /// </summary>
        public string Properties { get; set; }
        /// <summary>
        /// 属性值名称字符串 格式：值名称1_值名称2_值名称3
        /// </summary>
        public string PropertiesName { get; set; }
        /// <summary>
        /// 销售价格/终端价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 成本价格
        /// </summary>
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 加盟商价/门店
        /// </summary>
        public decimal ShopPrice { get; set; }
        /// <summary>
        /// 分销商价
        /// </summary>
        public decimal DistributorPrice { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 备用字段-商家编码
        /// </summary>
        public string ShopCode { get; set; }
        /// <summary>
        /// 备用字段-产品编码
        /// </summary>
        public string GoodsCode { get; set; }
        /// <summary>
        ///体积(m³)
        /// </summary>
        public double Volume { get; set; }
        /// <summary>
        /// 重量(kg)
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        ///展开面积(m²)
        /// </summary>
        public double ExpandArea { get; set; }

        /// <summary>
        /// 平方克重，1平方米产品的重量
        /// </summary>
        public double SquareWeight { get; set; }
        /// <summary>
        /// 终端价比例，默认1.3
        /// </summary>
        public double ClientPriceRate { get; set; }
        /// <summary>
        /// 加盟商价比例，默认1.1
        /// </summary>
        public double ShopPriceRate { get; set; }
    }
}
