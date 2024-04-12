using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class GoodsType
    {
        public GoodsType()
        {
            this.GT_IsEnable = 1;
            this.GT_IsRecommend = 0;
            this.GT_Sort = 0;
            this.GT_ParentID = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "类型名称由2-20个字符组成")]
        [Display(Name = "类型名称")]
        public string GT_Name { get; set; }

        [Display(Name = "是否启用")]
        public int GT_IsEnable { get; set; }

        [Display(Name = "是否推荐")]
        public int GT_IsRecommend { get; set; }

        [Display(Name = "排序号")]
        public int GT_Sort { get; set; }

        [Display(Name = "父类型")]
        public int GT_ParentID { get; set; }

    }
}
