using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order
    {
        public Order()
        {
            this.O_IsDelete = 0;
            this.O_Status = 0;
            this.O_CreateTime = DateTime.Now;
            this.O_Type = 0;
            this.O_IsInvoice = 0;
            this.O_PayWay = 0;
            this.O_ShippingWay = 0;
            this.O_PayScore = 0;
            this.O_GiveScore = 0;
            this.O_DiscountAmount = 0;
            this.O_PayAmount = 0;
            this.O_TotalAmount = 0;
            this.O_TotalShopPrice = 0;
            this.O_IsSettlement_Shop = 0;
            this.O_IsPurchase = 0;

        }
        public int ID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "订单编号")]
        public string O_OrderNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime O_CreateTime { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        [Display(Name = "付款时间")]
        public DateTime? O_PayTime { get; set; }

        /// <summary>
        /// 买家
        /// </summary>
        [Display(Name = "买家")]
        public int UserID { get; set; }

        /// <summary>
        /// 店铺
        /// </summary>
        [Display(Name = "店铺")]
        public int UserShopID { get; set; }

        /// <summary>
        /// 产品总价格+含运费+附加费(优惠前)
        /// </summary>
        [Display(Name = "总金额（含运费）")]
        public decimal O_TotalAmount { get; set; }

        /// <summary>
        /// 总加盟商价(不含邮费和附加费，如不含设计费，只商品价格) (优惠前)
        /// </summary>
        [Display(Name = "总加盟商价")]
        public decimal O_TotalShopPrice { get; set; }

        /// <summary>
        /// 总成本价
        /// </summary>
        [Display(Name = "总成本价")]
        public decimal O_TotalCostPrice { get; set; }
        /// <summary>
        /// 产品总价格 + 含运费 + 附加费 - 优惠额 = 应付金额（含运费）
        /// </summary>
        [Display(Name = "应付金额（含运费）")]
        public decimal O_NeedPayAmount { get; set; }

        /// <summary>
        /// 本次实际需付金额(含运费)，可能是定金
        /// </summary>
        [Display(Name = "实付金额（含运费）")]
        public decimal O_PayAmount { get; set; }

        /// <summary>
        /// 剩余待付金额，余款
        /// </summary>
        [Display(Name = "剩余待付金额")]
        public decimal O_RestPayAmount { get; set; }

        /// <summary>
        /// 总优惠金额（需要减去的，优惠券，红包等）
        /// </summary>
        [Display(Name = "优惠金额")]
        public decimal O_DiscountAmount { get; set; }

        /// <summary>
        /// 附加金额（如：设计费、打包费等）
        /// </summary>
        [Display(Name = "附加金额")]
        public decimal O_AdditionAmount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        [Display(Name = "运费")]
        public decimal O_PostFee { get; set; }

        /// <summary>
        /// 默认0:现金购物 ，1:积分兑换
        /// </summary>
        [Display(Name = "订单类型")]
        public int O_Type { get; set; }

        /// <summary>
        /// 需付积分
        /// </summary>
        [Display(Name = "需付积分")]
        public int O_PayScore { get; set; }

        /// <summary>
        /// 赠送积分
        /// </summary>
        [Display(Name = "赠送积分")]
        public int O_GiveScore { get; set; }

        [StringLength(100)]
        [Display(Name = "订单备注")]
        public string O_Remark { get; set; }

        /// <summary>
        /// 是否开发票
        /// </summary>
        [Display(Name = "是否开发票")]
        public int O_IsInvoice { get; set; }

        /// <summary>
        /// 开发票状态(0未开票，1已开票)
        /// </summary>
        [Display(Name = "开票状态")]
        public int O_InvoiceState { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [StringLength(50)]
        [Display(Name = "发票抬头")]
        public string O_InvoiceTitle { get; set; }

        /// <summary>
        /// 企业发票必须提供纳税人识别号或统一社会信用代码
        /// </summary>
        [StringLength(50)]
        [Display(Name = "企业税号")]
        public string O_BusinessTax { get; set; }

        /// <summary>
        /// 收件人地址详细：收件人,电话,省,市,区,详细,邮编 组成
        /// </summary>
        [StringLength(200)]
        [Display(Name = "收件人地址")]
        public string O_Address { get; set; }


        /// <summary>
        ///收货人信息ID
        /// </summary>
        [Display(Name = "收货人信息")]
        public int UserAddressID { get; set; }

        /// <summary>
        /// 用户IP
        /// </summary>
        [StringLength(50)]
        [Display(Name = "用户IP")]
        public string O_IP { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        [Display(Name = "配送方式")]
        public int O_ShippingWay { get; set; }

        /// <summary>
        /// 约定上门派送时间
        /// </summary>
        [Display(Name = "派送时间")]
        [StringLength(50)]
        public string O_ShippingTime { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [Display(Name = "支付方式")]
        public int O_PayWay { get; set; }


        [Display(Name = "是否删除")]
        public int O_IsDelete { get; set; }

        /// <summary>
        /// 默认0:待付款,1已付款,2交易成功, 详细见：DataConfig.OrderStatusEnum
        /// </summary>
        [Display(Name = "订单状态")]
        public int O_Status { get; set; }

        /// <summary>
        /// 默认0:待发货,1已发货,2已收货,-1:已退货 ,3部分发货(?)
        /// </summary>
        [Display(Name = "发货状态")]
        public int O_ShippingStatus { get; set; }

        /// <summary>
        /// 是否已评价
        /// </summary>
        [Display(Name = "是否已评价")]
        public int O_IsComment { get; set; }

        /// <summary>
        /// 加盟商是否已结算
        /// </summary>
        [Display(Name = "加盟商是否已结算")]
        public int O_IsSettlement_Shop { get; set; }
        /// <summary>
        /// 是否加盟商进货
        /// </summary>
        [Display(Name = "是否加盟商进货")]
        public int O_IsPurchase { get; set; }

        /// <summary>
        /// 下单平台，默认pc,其他如mobile,weixin
        /// </summary>
        [Display(Name = "下单平台")]
        [StringLength(50)]
        public string O_Platform { get; set; }

    }

    public class OrderVModel
    {
        public Order Order { get; set; }
        public User User { get; set; }
        public UserShop UserShop { get; set; }
    }

    /// <summary>
    /// 订单创建实体
    /// </summary>
    public class OrderPayModel
    {
        public OrderPayModel()
        {
            this.hongbao = 0;
            this.coupon = 0;
        }
        /// <summary>
        /// 总金额(含邮费+附加费) 优惠前
        /// </summary>
        [Display(Name = "总金额")]
        public decimal total_amount { get; set; }

        /// <summary>
        /// 产品总价格 + 含运费 + 附加费 - 优惠额 = 应付金额（含运费）
        /// </summary>
        [Display(Name = "应付金额（含运费）")]
        public decimal needpay_amount { get; set; }

        /// <summary>
        /// 本次实际支付，可能是定金
        /// </summary>
        [Display(Name = "实付金额（含运费）")]
        public decimal pay_amount { get; set; }

        [Display(Name = "运费")]
        public decimal postFee { get; set; }


        [StringLength(100)]
        [Display(Name = "订单备注")]
        public string remark { get; set; }

        /// <summary>
        /// 是否开发票
        /// </summary>
        [Display(Name = "是否开发票")]
        public int invoice { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [StringLength(50)]
        [Display(Name = "发票抬头")]
        public string invoice_title { get; set; }

        /// <summary>
        /// 企业发票必须提供纳税人识别号或统一社会信用代码
        /// </summary>
        [StringLength(50)]
        [Display(Name = "企业税号")]
        public string invoice_number { get; set; }

        /// <summary>
        /// 收件人地址详细：收件人,电话,省,市,区,详细,邮编 组成
        /// </summary>
        [StringLength(200)]
        [Display(Name = "收件人地址")]
        public string address { get; set; }

        /// <summary>
        ///收货人信息ID
        /// </summary>
        [Display(Name = "收货人信息")]
        public int addressid { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        [Display(Name = "配送方式")]
        public int shipping_way { get; set; }

        /// <summary>
        /// 约定上门派送时间
        /// </summary>
        [Display(Name = "派送时间")]
        public string shipping_time { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [Display(Name = "支付方式")]
        public int payway { get; set; }

        /// <summary>
        /// 优惠券ID
        /// </summary>
        [Display(Name = "优惠券ID")]
        public int coupon { get; set; }

        /// <summary>
        /// 红包ID
        /// </summary>
        [Display(Name = "红包ID")]
        public int hongbao { get; set; }

    }

    /// <summary>
    /// 用户订单统计实体
    /// </summary>
    public class UserOrderTJVModel
    {
        public UserOrderTJVModel()
        {
            this.total_amount = 0;
            this.total_count = 0;
            this.needpay_amount = 0;
            this.pay_amount = 0;
            this.post_fee = 0;
            this.discount_amount = 0;
            this.addition_amount = 0;
            this.total_shop_price = 0;
            //this.total_score = 0;
        }
        /// <summary>
        /// 总金额(含邮费+附加费)优惠前
        /// </summary>
        public decimal total_amount { get; set; }

        /// <summary>
        /// 总加盟商价(不含邮费和附加费，只商品价格) 优惠前
        /// </summary>
        [Display(Name = "总加盟商价")]
        public decimal total_shop_price { get; set; }
        /// <summary>
        /// 总成本价
        /// </summary>
        [Display(Name = "总成本价")]
        public decimal total_cost_price { get; set; }
        /// <summary>
        /// 总件数
        /// </summary>
        public int total_count { get; set; }
        /// <summary>
        /// 应付(含邮费)
        /// </summary>
        public decimal needpay_amount { get; set; }
        /// <summary>
        /// 本次实付(含邮费)（如：预付定金）
        /// </summary>
        public decimal pay_amount { get; set; }
        /// <summary>
        /// 邮费
        /// </summary>
        public decimal post_fee { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal discount_amount { get; set; }

        /// <summary>
        /// 附件费用(如：设计费)
        /// </summary>
        public decimal addition_amount { get; set; }

        ///// <summary>
        ///// 订单可赠送总积分
        ///// </summary>
        //public int total_score { get; set; }
    }


    /// <summary>
    /// 用户订单实体
    /// </summary>
    public class UserOrderVModel
    {
        public Order Order { get; set; }
        public UserShop UserShop { get; set; }
        public List<OrderDetailVModel> OrderDetailVList { get; set; }
    }

    /// <summary>
    /// 店铺订单实体
    /// </summary>
    public class ShopOrderVModel
    {
        public Order Order { get; set; }
        public User User { get; set; }
        public List<OrderDetailVModel> OrderDetailVList { get; set; }
        /// <summary>
        /// 是否需要发货，即存在未发货产品
        /// </summary>
        public Boolean NeedShipping { get; set; }
    }

    /// <summary>
    /// 店铺订单统计实体
    /// </summary>
    public class ShopOrderTJVModel
    {
        public ShopOrderTJVModel()
        {
            this.total_amount = 0;
            this.total_count = 0;
            this.total_ispay_amount = 0;
            this.total_ispay_count = 0;
            this.total_nopay_count = 0;
            this.total_nopay_amount = 0;
        }
        /// <summary>
        /// 总订单金额（需支付金额）
        /// </summary>
        public decimal total_amount { get; set; }
        /// <summary>
        /// 总订单数量
        /// </summary>
        public int total_count { get; set; }
        /// <summary>
        /// 总已支付订单金额，含交易完成，预付
        /// </summary>
        public decimal total_ispay_amount { get; set; }
        /// <summary>
        /// 总已支付订单数量
        /// </summary>
        public int total_ispay_count { get; set; }
        /// <summary>
        /// 总待付款订单数量
        /// </summary>
        public int total_nopay_count { get; set; }
        /// <summary>
        /// 总待付款订单金额
        /// </summary>
        public decimal total_nopay_amount { get; set; }
    }
}
