using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pannet.Models
{
    public class OrderDetailLog
    {
        public OrderDetailLog()
        {
            this.ODL_CreateTime = DateTime.Now;
            this.ODL_Type = 0;
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "操作人")]
        public string ODL_UserName { get; set; }

        /// <summary>
        /// 门店或管理员
        /// </summary>
        [Display(Name = "操作人ID")]
        [Required]
        public int ManagerID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "订单ID")]
        public int OrderID { get; set; }

        /// <summary>
        /// 订单详细ID
        /// </summary>
        [Display(Name = "订单详细ID")]
        public int OrderDetailID { get; set; }

        /// <summary>
        /// 记录类型,0：修改成本，1：修改加盟商价格 等
        /// </summary>
        [Display(Name = "记录类型")]
        public int ODL_Type { get; set; }

        [StringLength(50)]
        [Display(Name = "IP地址")]
        public string ODL_IP { get; set; }

        [StringLength(300)]
        [Display(Name = "操作内容")]
        public string ODL_Content { get; set; }

        public DateTime ODL_CreateTime { get; set; }
    }

    /// 调整订单详细 成本价格、加盟商价格等使用实体
    /// </summary>
    public class OrderDetailChangePriceVModel
    {
        public OrderDetailChangePriceVModel()
        {
        }
        /// <summary>
        ///原价格
        /// </summary>
        [Display(Name = "原价格")]
        public decimal Old_Price { get; set; }

        /// <summary>
        /// 新价格
        /// </summary>
        [Required]
        [Display(Name = "新价格")]
        public decimal New_Price { get; set; }

        /// <summary>
        /// 调整说明
        /// </summary>
        [StringLength(260)]
        [Required]
        [Display(Name = "调整说明")]
        public string Remark { get; set; }
    }
}
