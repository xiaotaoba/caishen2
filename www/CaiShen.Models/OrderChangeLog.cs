using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pannet.Models
{
    public class OrderChangeLog
    {
        public OrderChangeLog()
        {
            this.OCL_CreateTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "操作人")]
        public string OCL_UserName { get; set; }

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

        [StringLength(20)]
        [Display(Name = "IP地址")]
        public string OCL_IP { get; set; }

        [StringLength(300)]
        [Display(Name = "操作内容")]
        public string OCL_Content { get; set; }

        public DateTime OCL_CreateTime { get; set; }
    }

    public class OrderChangeAmountVModel
    {
        public OrderChangeAmountVModel()
        {
        }
        /// <summary>
        ///订单金额/原价格
        /// </summary>
        [Display(Name = "订单金额")]
        public decimal Old_PayAmount { get; set; }

        /// <summary>
        /// 新订单金额
        /// </summary>
        [Required]
        [Display(Name = "新订单金额")]
        public decimal New_PayAmount { get; set; }
        
        /// <summary>
        /// 调整说明
        /// </summary>
        [StringLength(260)]
        [Required]
        [Display(Name = "调整说明")]
        public string Remark { get; set; }
    }
    /// <summary>
    /// 调整订单详细 成本价格、加盟商价格、运费等使用实体
    /// </summary>
    public class OrderChangePriceVModel
    {
        public OrderChangePriceVModel()
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
