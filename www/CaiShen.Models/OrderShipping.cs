using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 订单配送(发货)记录
    /// </summary>
    public class OrderShipping
    {
        public OrderShipping()
        {
            this.OS_IsDelete = 0;
            this.OrderID = 0;
            this.OS_Status = 0;
            this.OS_CreateTime = DateTime.Now;
        }
        public int ID { get; set; }

        [Display(Name = "订单ID")]
        public int OrderID { get; set; }

        /// <summary>
        /// 运送公司
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50)]
        [Display(Name = "运送公司")]
        public string ShippingCompany { get; set; }

        /// <summary>
        /// 运送单号
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50)]
        [Display(Name = "运送单号")]
        public string ShippingNo { get; set; }

        /// <summary>
        /// 客服电话
        /// </summary>
        [StringLength(50)]
        [Display(Name = "客服电话")]
        public string ShippingTel { get; set; }

        /// <summary>
        /// 发货地址
        /// </summary>
        [StringLength(200)]
        [Display(Name = "发货地址")]
        public string ShippingAddress { get; set; }

        /// <summary>
        /// 退货地址信息
        /// </summary>
        [StringLength(200)]
        [Display(Name = "退货地址信息")]
        public string ReturnAddress { get; set; }

        /// <summary>
        /// 运送产品，逗号隔开，订单详情ID字符串
        /// </summary>
        [StringLength(500)]
        [Display(Name = "运送产品")]
        public string OrderDetailIds { get; set; }

        [Display(Name = "是否删除")]
        public int OS_IsDelete { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        [Display(Name = "发货时间")]
        public DateTime OS_CreateTime { get; set; }

        /// <summary>
        /// 默认0:等待揽收,1已揽收,2运输中,3正在投递,4:已签收,5:未妥投,6:转窗投
        /// </summary>
        [Display(Name = "配送状态")]
        public int OS_Status { get; set; }

        /// <summary>
        /// 确认收货时间
        /// </summary>
        [Display(Name = "确认收货时间")]
        public DateTime? OS_DeliveryTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(200)]
        public string OS_Remark { get; set; }
    }

    /// <summary>
    /// 用户订单详细-派送记录实体
    /// </summary>
    public class UserOrderShippingVModel
    {
        public OrderShipping OrderShipping { get; set; }
        public List<OrderDetailVModel> OrderDetailVList { get; set; }
    }

}
