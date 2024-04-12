using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class ShopGuaranteeHistory
    {
        /// <summary>
        /// 店铺保障金使用记录（预付款或到付订单使用掉保障金额度，订单结束，返回保障金可用额度）
        /// </summary>
        public ShopGuaranteeHistory()
        {
            this.Is_Delete = 0;
            this.Type = 1;
        }
        public int ID { get; set; }

        [Display(Name = "店铺ID")]
        public int UserShopID { get; set; }

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
        /// 保障金额度
        /// </summary>
        [Display(Name = "保障金额度")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 剩余可用额度
        /// </summary>
        [Display(Name = "剩余可用额度")]
        public decimal RestAmount { get; set; }

        /// <summary>
        /// 变动额度
        /// </summary>
        [Display(Name = "变动额度")]
        public decimal Amount { get; set; }

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
}
