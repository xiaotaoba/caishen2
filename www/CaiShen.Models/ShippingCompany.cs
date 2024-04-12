using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class ShippingCompany
    {
        /// <summary>
        /// 运送方式(公司)，如：顺丰，德邦
        /// </summary>
        public ShippingCompany()
        {
            this.SC_Is_Enable = 1;
            this.SC_IsCount = 0;
            this.SC_IsWeight = 1;
            this.SC_IsVolume = 0;
            this.SC_Sort = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "名称由2-20个字符组成")]
        [Display(Name = "运送方式名称")]
        public string SC_Name { get; set; }


        [StringLength(200)]
        [Display(Name = "备注")]
        public string SC_Remark { get; set; }

        /// <summary>
        /// 默认0 ，所属平台
        /// </summary>
        [Display(Name = "所属会员")]
        public int UserID { get; set; }

        [Display(Name = "按件数计价")]
        public int SC_IsCount { get; set; }

        [Display(Name = "按重量计价")]
        public int SC_IsWeight { get; set; }

        [Display(Name = "按体积计价")]
        public int SC_IsVolume { get; set; }

        [Display(Name = "是否启用")]
        public int SC_Is_Enable { get; set; }


        [Display(Name = "排序号")]
        public int SC_Sort { get; set; }
    }
}
