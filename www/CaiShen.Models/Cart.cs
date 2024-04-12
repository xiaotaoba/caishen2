using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class Cart
    {
        /// <summary>
        /// 购物车
        /// </summary>
        public Cart()
        {
            this.GoodsID = 0;
            this.ShopID = 0;
            this.UserID = 0;
            this.SKUID = 0;
            this.Count = 1;
            this.GoodsCount = 1;
            this.IsHasDesignFile = 0;
            this.DesignFee = 0;
            this.HiddenShippingFee = 0;
            this.CartTotalPrice = 0;
            this.TotalShopPrice = 0;
            this.TotalCostPrice = 0;
            this.Weight = 0;
            this.Volume = 0;
            this.UnitArea = 0;
            this.IsPurchase = 0;
        }
        public int ID { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Display(Name = "产品ID")]
        public int GoodsID { get; set; }

        /// <summary>
        /// 门店ID
        /// </summary>
        [Display(Name = "门店ID")]
        public int ShopID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        /// <summary>
        /// SKUID
        /// </summary>
        [Display(Name = "SKUID")]
        public int SKUID { get; set; }

        /// <summary>
        /// 已选产品属性ID值对，属性ID:值ID;属性ID:值ID;
        /// </summary>
        [Display(Name = "已选产品属性ID")]
        [StringLength(500)]
        public string Properties { get; set; }

        /// <summary>
        /// 已选产品属性名称值对，属性名称:值内容;属性ID:值内容;
        /// </summary>
        [Display(Name = "已选产品属性名称")]
        [StringLength(500)]
        public string PropertiesName { get; set; }

        /// <summary>
        /// 总终端销售价（含设计费，隐藏运费）
        /// </summary>
        [Display(Name = "价格")]
        public decimal CartTotalPrice { get; set; }

        /// <summary>
        /// 总加盟商价(不含设计费，只产品费用)
        /// </summary>
        [Display(Name = "加盟商价")]
        public decimal TotalShopPrice { get; set; }

        /// <summary>
        /// 总成本价格
        /// </summary>
        [Display(Name = "总成本价格")]
        public decimal TotalCostPrice { get; set; }

        /// <summary>
        /// 购买物品SKU数量
        /// </summary>
        [Display(Name = "数量")]
        public int Count { get; set; }

        /// <summary>
        /// 单个SKU包含物品数量
        /// </summary>
        [Display(Name = "单品数量")]
        public int GoodsCount { get; set; }

        /// <summary>
        /// 设计稿0不需要（现货），1有，2无
        /// </summary>
        [Display(Name = "设计稿")]
        public int IsHasDesignFile { get; set; }

        /// <summary>
        /// 设计费用
        /// </summary>
        [Display(Name = "设计费用")]
        public decimal DesignFee { get; set; }

        /// <summary>
        /// 隐藏运费(门店和仓库同区域时，默认计算邮费，外面展示是包邮)
        /// </summary>
        [Display(Name = "隐藏运费")]
        public decimal HiddenShippingFee { get; set; }

        /// <summary>
        /// 单件(可以多个)产品重量(KG)
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 单件(可以多个)产品体积(m³)
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// 单件(可以多个)面积(m²)
        /// </summary>
        public double UnitArea { get; set; }

        /// <summary>
        /// 是否加盟商进货
        /// </summary>
        [Display(Name = "是否加盟商进货")]
        public int IsPurchase { get; set; }
    }

    public class CartVModel
    {
        public CartVModel()
        {
            SupplierID = 0;
            WarehouseID = 0;
            IsWeight = 0;
            Weight = 0;
            Volume = 0;
        }
        public Cart Cart { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }

        /// <summary>
        /// 是否重货 1重货 0泡货
        /// </summary>
        public int IsWeight { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public int SupplierID { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public int WarehouseID { get; set; }

        /// <summary>
        /// 单个产品重量(KG)
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 单个产品体积(m³)
        /// </summary>
        public double Volume { get; set; }
    }

}
