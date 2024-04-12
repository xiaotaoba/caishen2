using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 退货配送记录
    /// </summary>
    public class ReturnOrderShipping
    {
        public ReturnOrderShipping()
        {
            this.ROS_IsDelete = 0;
            this.ReturnOrderID = 0;
            this.ROS_CreateTime = DateTime.Now;
            this.ROS_Status = 1;
        }
        public int ID { get; set; }

        //[Display(Name = "订单ID")]
        //public int OrderID { get; set; }

        [Display(Name = "退单ID")]
        public int ReturnOrderID { get; set; }

        /// <summary>
        /// 运送公司
        /// </summary>
        [StringLength(50)]
        [Display(Name = "运送公司")]
        public string ShippingCompany { get; set; }

        /// <summary>
        /// 运送单号
        /// </summary>
        [StringLength(50)]
        [Display(Name = "运送单号")]
        public string ShippingNo { get; set; }

        /// <summary>
        /// 默认1 已发货,2已收退货
        /// </summary>
        [Display(Name = "退货状态")]
        public int ROS_Status { get; set; }


        [Display(Name = "是否删除")]
        public int ROS_IsDelete { get; set; }

        /// <summary>
        /// 发退货时间
        /// </summary>
        [Display(Name = "退货时间")]
        public DateTime ROS_CreateTime { get; set; }

        /// <summary>
        /// 收退货时间
        /// </summary>
        [Display(Name = "收货时间")]
        public DateTime? ROS_DeliveryTime { get; set; }

    }



}
