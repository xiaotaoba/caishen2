using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 退货/售后处理表
    /// </summary>
    public class ReturnOrder
    {
        public ReturnOrder()
        {
            this.RO_IsDelete = 0;
            this.RO_PayStatus = 0;
            this.OrderDetailID = 0;
            this.RO_Type = 0;
            this.RO_CreateTime = DateTime.Now;
        }
        public int ID { get; set; }

        [Display(Name = "退货单号")]
        [StringLength(50)]
        public string RO_ReturnOrderNo { get; set; }

        [Display(Name = "买家")]
        public int UserID { get; set; }

        [Display(Name = "店铺")]
        public int UserShopID { get; set; }

        [Display(Name = "订单产品详情ID")]
        public int OrderDetailID { get; set; }

        /// <summary>
        /// 默认0:退货 ，1:仅退款
        /// </summary>
        [Display(Name = "退货类型")]
        public int RO_Type { get; set; }

        [Display(Name = "退货原因")]
        [StringLength(50)]
        public string RO_Reason { get; set; }

        [Display(Name = "详细说明")]
        [StringLength(200)]
        public string RO_Description { get; set; }

        [Display(Name = "图片")]
        [StringLength(1000)]
        public string RO_Images { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        [Display(Name = "退款金额")]
        public decimal RO_Amount { get; set; }

        /// <summary>
        /// 默认0:未退款,1已退款
        /// </summary>
        [Display(Name = "退款状态")]
        public int RO_PayStatus { get; set; }

        /// <summary>
        /// 默认0:待退货,1已退货,2已收退货
        /// </summary>
        [Display(Name = "退货状态")]
        public int RO_ShippingStatus { get; set; }

        [Display(Name = "是否删除")]
        public int RO_IsDelete { get; set; }

        /// <summary>
        /// 默认0:已申请(待商家确认),1商家已同意退货,2商家不同意退货,3退货完成
        /// </summary>
        [Display(Name = "处理状态")]
        public int RO_Status { get; set; }

        /// <summary>
        /// 商家不同意理由，不同意时填写
        /// </summary>
        [Display(Name = "商家不同意理由")]
        [StringLength(200)]
        public string RO_ShopReason { get; set; }

        /// <summary>
        /// 申请退货时间
        /// </summary>
        [Display(Name = "申请时间")]
        public DateTime RO_CreateTime { get; set; }

        /// <summary>
        /// 商家处理时间
        /// </summary>
        [Display(Name = "商家处理时间")]
        public DateTime? RO_HandleTime { get; set; }

    }


    public class ReturnOrderVModel
    {
        public User User { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public ReturnOrder ReturnOrder { get; set; }
        public UserShop UserShop { get; set; }
    }


}
