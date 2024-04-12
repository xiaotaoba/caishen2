using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserAmountHistory
    {
        /// <summary>
        /// 会员金额变动历史记录
        /// </summary>
        public UserAmountHistory()
        {
            this.Is_Delete = 0;
            this.Type = 1;
        }
        public int ID { get; set; }

        [Display(Name = "会员ID")]
        public int UserID { get; set; }

        public virtual User User { get; set; }

        /// <summary>
        /// 包含 商城交易单号等其他交易信息
        /// </summary>
        [StringLength(50)]
        [Display(Name = "事项")]
        public string Thing { get; set; }

        [StringLength(20)]
        [Display(Name = "分类")]
        public string Category { get; set; }

        /// <summary>
        /// 备注，第三方交易号等第三方信息
        /// </summary>
        [StringLength(100)]
        [Display(Name = "备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 剩余金额
        /// </summary>
        [Display(Name = "剩余金额")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 变动金额
        /// </summary>
        [Display(Name = "变动金额")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 剩余锁定金额
        /// </summary>
        [Display(Name = "剩余锁定金额")]
        public decimal LockTotalAmount { get; set; }

        /// <summary>
        /// 变动锁定金额
        /// </summary>
        [Display(Name = "变动锁定金额")]
        public decimal LockAmount { get; set; }

        [Display(Name = "时间")]
        public DateTime Time { get; set; }

        [Display(Name = "是否删除")]
        public int Is_Delete { get; set; }

        /// <summary>
        /// 1增加，0减少
        /// </summary>
        [Display(Name = "类型")]
        public int Type { get; set; }

        [Display(Name = "关联记录ID")]
        public int RecordID { get; set; }

        [StringLength(50)]
        [Display(Name = "操作人")]
        public string Operator { get; set; }
    }
    /// <summary>
    /// 结算记录实体
    /// </summary>
    public class UserAmountSettlementVModel
    {
        public UserAmountHistory UserAmountHistory { get; set; }
        public User User { get; set; }
    }
    
}
