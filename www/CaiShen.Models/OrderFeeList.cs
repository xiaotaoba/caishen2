using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 订单附加或减免费用清单
    /// </summary>
    public class OrderFeeList
    {
        public OrderFeeList()
        {
        }
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "订单ID")]
        public int OrderID { get; set; }

        /// <summary>
        /// 事项
        /// </summary>
        [StringLength(100)]
        [Display(Name = "事项")]
        public string OFL_Thing { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public decimal OFL_Amount { get; set; }

        /// <summary>
        /// 费用类型 默认：0减免，1增加
        /// </summary>
        [Display(Name = "费用类型")]
        public int OFL_type { get; set; }

    }
}
