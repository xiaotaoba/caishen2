using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 用户开票记录
    /// </summary>
    public class InvoiceLog
    {
        public InvoiceLog()
        {
            this.CreateTime = DateTime.Now;
            this.Inv_InvoiceTime = DateTime.Now;
            this.Inv_AddressID = 0;
            this.Inv_Amount = 0;
            this.Inv_ExpressFee = 0;
            this.Inv_Status = 0;
            this.Inv_TaxAmount = 0;
            this.Inv_UserType = 0;
            this.UserID = 0;
            this.Inv_InvoiceType = 0;
        }
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        /// <summary>
        /// 申请开票单号
        /// </summary>
        [StringLength(50)]
        [Display(Name = "申请开票单号")]
        public string Inv_InvoiceNumber { get; set; }

        /// <summary>
        /// 类型（0企业，1个人）
        /// </summary>
        [Display(Name = "用户类型")]
        public int Inv_UserType { get; set; }
        /// <summary>
        /// 发票类型（普票，专票）
        /// </summary>
        [Display(Name = "发票类型")]
        public int Inv_InvoiceType { get; set; }

        /// <summary>
        /// 发票内容
        /// </summary>
        [StringLength(300)]
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "开票内容")]
        public string Inv_Content { get; set; }

        /// <summary>
        /// 申请金额
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "申请金额")]
        public decimal Inv_Amount { get; set; }

        /// <summary>
        /// 税点金额
        /// </summary>
        [Display(Name = "税点金额")]
        public decimal Inv_TaxAmount { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [StringLength(50)]
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "发票抬头")]
        public string Inv_Title { get; set; }

        /// <summary>
        /// 纳税人识别号
        /// </summary>
        [StringLength(50)]
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "纳税人识别号")]
        public string Inv_BusinessTax { get; set; }

        /// <summary>
        /// 公司注册地址
        /// </summary>
        [StringLength(50)]
        [Display(Name = "公司注册地址")]
        public string Inv_CompanyAddress { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [StringLength(50)]
        [Display(Name = "开户行名称")]
        public string Inv_BankName { get; set; }

        /// <summary>
        /// 开户账号
        /// </summary>
        [StringLength(50)]
        [Display(Name = "开户账号")]
        public string Inv_BankNumber { get; set; }

        /// <summary>
        /// 收件地址ID
        /// </summary>
        [Display(Name = "收件地址ID")]
        public int Inv_AddressID { get; set; }

        /// <summary>
        /// 收件地址
        /// </summary>
        [StringLength(100)]
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "详细地址")]
        public string Inv_Address { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        [StringLength(50)]
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "收件人")]
        public string Inv_Addressee { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(50)]
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "联系方式")]
        public string Inv_Tel { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        [Display(Name = "申请时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 处理时间(开票)
        /// </summary>
        [Display(Name = "处理时间")]
        public DateTime Inv_InvoiceTime { get; set; }

        /// <summary>
        /// 审核未通过原因
        /// </summary>
        [StringLength(100)]
        [Display(Name = "审核未通过原因")]
        public string Inv_FailReason { get; set; }

        /// <summary>
        /// 处理状态(0待支付税点1:待处理，2已寄出，3完成，4已取消)
        /// </summary>
        [Display(Name = "处理状态")]
        public int Inv_Status { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        [StringLength(50)]
        [Display(Name = "快递公司")]
        public string Inv_Express { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        [StringLength(50)]
        [Display(Name = "快递单号")]
        public string Inv_ExpressNumber { get; set; }

        /// <summary>
        /// 申请金额
        /// </summary>
        [Display(Name = "快递费用")]
        public decimal Inv_ExpressFee { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(300)]
        [Display(Name = "备注")]
        public string Inv_Remark { get; set; }

        /// <summary>
        /// 开票订单号
        /// </summary>
        [StringLength(50)]
        [Display(Name = "开票订单号")]
        public string Inv_OrderNo { get; set; }
    }
}
