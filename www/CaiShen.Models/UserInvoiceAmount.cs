using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 用户开票总额
    /// </summary>
    public class UserInvoiceAmount
    {
        public UserInvoiceAmount()
        {
            this.UserID = 0;
            this.UIA_TotalAmount = 0;
            this.UIA_InvoicedAmount = 0;
            this.UIA_RestAmount = 0;
        }
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        /// <summary>
        /// 累计消费金额
        /// </summary>
        [Display(Name = "累计消费金额")]
        public decimal UIA_TotalAmount { get; set; }

        /// <summary>
        /// 累计已开票金额
        /// </summary>
        [Display(Name = "已开票金额")]
        public decimal UIA_InvoicedAmount { get; set; }

        /// <summary>
        /// 待开票金额
        /// </summary>
        [Display(Name = "待开票金额")]
        public decimal UIA_RestAmount { get; set; }

    }
}
