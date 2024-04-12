using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class SupplierArea
    {
        /// <summary>
        /// 供应商供应地区配置
        /// </summary>
        public SupplierArea()
        {
            this.SupplierID = 0;
            this.Sort = 0;
            this.IsSameArea = 0;
        }
        public int ID { get; set; }

        [Display(Name = "供应商")]
        public int SupplierID { get; set; }

        /// <summary>
        /// 供应模板名称
        /// </summary>
        [StringLength(50)]
        [Display(Name = "供应模板名称")]
        public string Title { get; set; }

        /// <summary>
        /// 逗号隔开的area_id
        /// </summary>
        [Display(Name = "供应地区")]
        public string AreaIds { get; set; }

        /// <summary>
        /// 1是 0不是
        /// </summary>
        [Display(Name = "供应商同区域")]
        public int IsSameArea { get; set; }

        /// <summary>
        /// 排序号或优先级
        /// </summary>
        [Display(Name = "优先级")]
        public int Sort { get; set; }
    }
}
