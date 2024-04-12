using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class ShippingTemplate
    {
        /// <summary>
        /// 运费模板
        /// </summary>
        public ShippingTemplate()
        {
            this.ST_Is_Enable = 1;
            this.UserID = 0;
            this.ST_UserType = 0;
            //包邮
            this.ST_IsFree = 0;
            this.ST_Sort = 0;
        }
        public int ID { get; set; }

        /// <summary>
        /// 运费模板名称
        /// </summary>
        [StringLength(50)]
        [Display(Name = "运费模板名称")]
        [Required(ErrorMessage = "请输入{0}")]
        public string ST_Title { get; set; }

        /// <summary>
        /// 默认0 平台，1仓库 2供应商，3门店
        /// </summary>
        [Display(Name = "用户类型")]
        public int ST_UserType { get; set; }

        /// <summary>
        /// UserType=0时，默认0;UserType=1时，仓库ID;UserType=2时，供应商ID;UserType=3时，门店ID;
        /// </summary>
        [Display(Name = "所属会员")]
        public int UserID { get; set; }

        [Display(Name = "运送方式")]
        public int ShippingCompanyID { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        [Display(Name = "备注说明")]
        [StringLength(500)]
        public string ST_Remark { get; set; }

        [Display(Name = "是否包邮")]
        public int ST_IsFree { get; set; }

        [Display(Name = "是否启用")]
        public int ST_Is_Enable { get; set; }

        [Display(Name = "排序号")]
        public int ST_Sort { get; set; }
    }
}
