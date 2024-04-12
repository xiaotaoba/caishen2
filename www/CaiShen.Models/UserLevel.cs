using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserLevel
    {
        public UserLevel()
        {
            this.Level_Is_Special = 0;
            this.Level_Is_Enable = 1;
            this.Level_Money_Begin = 0;
            this.Level_Money_End = 0;
            this.Level_Discount_Percent = 0;
            this.Level_Partner_Rebate_Percent = 0;
            this.Level_Shop_Rebate_Percent = 0;
        }
        public int ID { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "会员级别由2-20个字符组成")]
        [Display(Name = "会员级别")]
        public string Level_Name { get; set; }

        [StringLength(200)]
        [Display(Name = "级别描述")]
        public string Level_Desc { get; set; }

        [Display(Name = "充值金额下限")]
        public int Level_Money_Begin { get; set; }

        [Display(Name = "充值金额上限")]
        public int Level_Money_End { get; set; }

        [Display(Name = "折扣(%)")]
        public double Level_Discount_Percent { get; set; }

        [Display(Name = "合伙人返利比(%)")]
        public double Level_Partner_Rebate_Percent { get; set; }

        [Display(Name = "门店返利比(%)")]
        public double Level_Shop_Rebate_Percent { get; set; }

        /// <summary>
        /// 不随充值多少，变动等级
        /// </summary>
        [Display(Name = "是否特殊等级")]
        public int Level_Is_Special { get; set; }

        [Display(Name = "是否启用")]
        public int Level_Is_Enable { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
