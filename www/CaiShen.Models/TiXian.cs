using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 记录用户申请提现
    /// </summary>
    public class TiXian
    {
        public TiXian()
        {
            this.TX_Status = 0;
            this.TX_Amount = 0;
            this.TX_CreateTime = DateTime.Now;
        }
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        /// <summary>
        /// 提现申请单号
        /// </summary>
        [StringLength(50)]
        [Display(Name = "申请开票单号")]
        public string TX_Number { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "提现金额")]
        public decimal TX_Amount { get; set; }

        // <summary>
        /// 收款人
        /// </summary>
        [StringLength(50)]
        [Display(Name = "收款人")]
        public string TX_UserName { get; set; }

        /// <summary>
        /// 收款银行(银行名称、支付宝或微信等)
        /// </summary>
        [StringLength(50)]
        [Display(Name = "收款银行")]
        public string TX_BankName { get; set; }

        /// <summary>
        /// 收款账号（银行卡号，支付宝、微信账号）
        /// </summary>
        [StringLength(50)]
        [Display(Name = "收款账号")]
        public string TX_BankNumber { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [StringLength(50)]
        [Display(Name = "操作人")]
        public string TX_Operator { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        [Display(Name = "申请时间")]
        public DateTime TX_CreateTime { get; set; }

        /// <summary>
        /// 支付时间[调整为“已完成”时间]
        /// </summary>
        [Display(Name = "支付时间")]
        public DateTime? TX_PayTime { get; set; }

        /// <summary>
        /// 处理状态(0:待处理，1已审核，2已完成，3已取消，4审核失败)
        /// </summary>
        [Display(Name = "处理状态")]
        public int TX_Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(300)]
        [Display(Name = "备注")]
        public string TX_Remark { get; set; }
    }
    public class TiXianVModel
    {
        public TiXian TiXian { get; set; }
        public string UserName { get; set; }
    }
    public class TiXianUserVModel
    {
        // <summary>
        /// 收款人
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 收款银行(银行名称、支付宝或微信等)
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 收款账号（银行卡号，支付宝、微信账号）
        /// </summary>
        public string BankNumber { get; set; }

    }
}
