using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 订单产品详情表
    /// </summary>
    public class OrderDetail
    {
        public OrderDetail()
        {
            this.OD_IsDelete = 0;
            this.OD_PayStatus = 0;
            this.OD_Type = 0;
            this.OD_PayScore = 0;
            this.OD_GiveScore = 0;
            this.OD_DiscountAmount = 0;
            this.OD_PayAmount = 0;
            this.OD_TotalAmount = 0;
            this.OrderID = 0;
            this.UserID = 0;
            this.UserShopID = 0;
            this.OD_IsHasDesignFile = 0;
            this.OD_DesignFee = 0;
            this.OD_PostFee = 0;
            this.OD_HiddenPostFee = 0;
            this.OD_TotalShopPrice = 0;
            this.OD_IsComment = 0;
            this.OD_IsSettlement_Supplier = 0;
            this.OD_TotalCostPrice = 0;
            this.OD_IsDesign = 0;
        }
        public int ID { get; set; }

        [Display(Name = "订单ID")]
        public int OrderID { get; set; }

        [Display(Name = "UserID")]
        public int UserID { get; set; }

        [Display(Name = "UserShopID")]
        public int UserShopID { get; set; }

        /// <summary>
        /// 默认0:非现货 需填写供应商ID; 1:现货，需填写仓库ID
        /// </summary>
        [Display(Name = "是否现货")]
        public int OD_IsExist { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [Display(Name = "供应商")]
        public int SupplierID { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        [Display(Name = "仓库")]
        public int WarehouseID { get; set; }

        [Display(Name = "产品标题")]
        [StringLength(100)]
        public string OD_GoodsName { get; set; }

        /// <summary>
        /// 产品单价标价
        /// </summary>
        [Display(Name = "产品单价")]
        public decimal OD_UnitPrice { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        [Display(Name = "购买数量")]
        public int OD_Count { get; set; }

        /// <summary>
        /// SKUID
        /// </summary>
        [Display(Name = "SKUID")]
        public int GoodsSKUID { get; set; }

        /// <summary>
        /// GoodsID
        /// </summary>
        [Display(Name = "GoodsID")]
        public int GoodsID { get; set; }

        /// <summary>
        /// 所选属性，属性名称值字符串
        /// </summary>
        [Display(Name = "所选属性")]
        [StringLength(500)]
        public string OD_PropertiesName { get; set; }

        /// <summary>
        /// 总金额（含运费+隐藏运费+设计费） 优惠前
        /// </summary>
        [Display(Name = "总金额")]
        public decimal OD_TotalAmount { get; set; }

        /// <summary>
        /// 总加盟商价（不含运费和隐藏运费和设计费，只商品价格）优惠前
        /// </summary>
        [Display(Name = "总加盟商价")]
        public decimal OD_TotalShopPrice { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        [Display(Name = "实付金额")]
        public decimal OD_PayAmount { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        [Display(Name = "优惠金额")]
        public decimal OD_DiscountAmount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        [Display(Name = "运费")]
        public decimal OD_PostFee { get; set; }
        /// <summary>
        ///  隐藏运费(门店和仓库同区域时，默认计算邮费，外面展示是包邮)
        /// </summary>
        [Display(Name = "隐藏运费")]
        public decimal OD_HiddenPostFee { get; set; }

        /// <summary>
        /// 总成本价格
        /// </summary>
        [Display(Name = "总成本价格")]
        public decimal OD_TotalCostPrice { get; set; }

        /// <summary>
        /// 非现货才需要设计稿，是否有设计稿，有设计稿设计费0，无设计稿：收设计费，0：不需要设计稿，1：有设计稿 2：无设计稿
        /// </summary>
        [Display(Name = "是否有设计稿")]
        public int OD_IsHasDesignFile { get; set; }

        /// <summary>
        /// 如果是非现货，客户选择有设计稿，需要上传设计稿文件
        /// </summary>
        [Display(Name = "设计稿文件")]
        [StringLength(300)]
        public string OD_DesignFile { get; set; }

        /// <summary>
        /// 设计费
        /// </summary>
        [Display(Name = "设计费")]
        public decimal OD_DesignFee { get; set; }

        /// <summary>
        /// 默认0:现金购物 ，1:积分兑换
        /// </summary>
        [Display(Name = "类型")]
        public int OD_Type { get; set; }

        /// <summary>
        /// 需付积分
        /// </summary>
        [Display(Name = "需付积分")]
        public int OD_PayScore { get; set; }

        /// <summary>
        /// 赠送积分
        /// </summary>
        [Display(Name = "赠送积分")]
        public int OD_GiveScore { get; set; }


        [Display(Name = "是否删除")]
        public int OD_IsDelete { get; set; }

        /// <summary>
        /// 默认0:待付款,1已付款 和 订单对应
        /// </summary>
        [Display(Name = "订单支付状态")]
        public int OD_PayStatus { get; set; }

        /// <summary>
        /// 默认0:待发货,1已发货,2已收货,-1:已退货  和 订单对应
        /// </summary>
        [Display(Name = "发货状态")]
        public int OD_ShippingStatus { get; set; }

        /// <summary>
        /// 默认0:未评价，1已评价
        /// </summary>
        [Display(Name = "是否评价")]
        public int OD_IsComment { get; set; }

        /// <summary>
        /// 供应商是否已结算
        /// </summary>
        [Display(Name = "是否已结算")]
        public int OD_IsSettlement_Supplier { get; set; }

        /// <summary>
        /// 是否设计类产品
        /// </summary>
        [Display(Name = "设计类产品")]
        public int OD_IsDesign { get; set; }
    }

    public class OrderDetailVModel
    {
        public OrderDetail OrderDetail { get; set; }
        public string PhotoUrl { get; set; }
    }

}
