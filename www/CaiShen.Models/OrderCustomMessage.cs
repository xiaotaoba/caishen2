using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 按需定制
    /// </summary>
    public class OrderCustomMessage
    {
        public OrderCustomMessage()
        {
            this.OC_CreateTime = DateTime.Now;
            this.UserID = 0;
            this.UserShopID = 0;

        }
        public int ID { get; set; }

        [Display(Name = "用户")]
        public int UserID { get; set; }

        [StringLength(50)]
        [Display(Name = "店铺名称")]
        public string ShopName { get; set; }

        [Display(Name = "店铺")]
        public int UserShopID { get; set; }

        [StringLength(50)]
        [Display(Name = "产品类型")]
        public string OC_Type { get; set; }

        [StringLength(50)]
        [Display(Name = "产品名称")]
        public string OC_Name { get; set; }

        [StringLength(50)]
        [Display(Name = "数量")]
        public string OC_Count { get; set; }

        /// <summary>
        /// 产品尺寸
        /// </summary>
        [StringLength(50)]
        [Display(Name = "产品尺寸")]
        public string OC_Size { get; set; }

        /// <summary>
        /// 其他要求
        /// </summary>
        [StringLength(500)]
        [Display(Name = "其他要求")]
        public string OC_Remark { get; set; }

        /// <summary>
        /// 产品属性
        /// </summary>
        [StringLength(500)]
        [Display(Name = "产品属性")]
        public string OC_Properties { get; set; }

        [StringLength(500)]
        [Display(Name = "附件地址")]
        public string OC_File { get; set; }

        [StringLength(50)]
        [Display(Name = "联系人")]
        public string OC_UserName { get; set; }

        [StringLength(50)]
        [Display(Name = "联系方式")]
        public string OC_Tel { get; set; }

        [StringLength(500)]
        [Display(Name = "收货地址")]
        public string OC_Address { get; set; }

        [Display(Name = "是否阅读")]
        public int OC_IsRead { get; set; }

        /// <summary>
        /// 跟踪反馈
        /// </summary>
        [StringLength(500)]
        [Display(Name = "跟踪反馈")]
        public string OC_Reply { get; set; }

        /// <summary>
        /// 0未确认，1待付款，2已付款，3已发货，4交易完成
        /// </summary>
        [Display(Name = "状态")]
        public int OC_Status { get; set; }

        [Display(Name = "价格")]
        public decimal OC_Price { get; set; }

        /// <summary>
        /// 物流信息
        /// </summary>
        [StringLength(100)]
        [Display(Name = "物流信息")]
        public string OC_ShippingInfo { get; set; }

        /// <summary>
        /// 定制单号
        /// </summary>
        [StringLength(50)]
        [Display(Name = "定制单号")]
        public string OC_OrderNo { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        [Display(Name = "提交时间")]
        public DateTime OC_CreateTime { get; set; }
    }
}
