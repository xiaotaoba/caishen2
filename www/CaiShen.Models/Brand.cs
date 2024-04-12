using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class Brand
    {
        public Brand()
        {
            this.B_IsEnable = 1;
            this.B_IsRecommend = 0;
            this.B_Sort = 0;
            this.B_CreateTime = DateTime.Now;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "类型名称由2-20个字符组成")]
        [Display(Name = "品牌名称")]
        public string B_Name { get; set; }

        [StringLength(100)]
        [Display(Name = "英文名称")]
        public string B_NameEn { get; set; }

        [Display(Name = "品牌描述")]
        public string B_Desc { get; set; }

        [StringLength(200)]
        [Display(Name = "品牌LOGO")]
        public string B_Image { get; set; }

        [StringLength(200)]
        [Display(Name = "品牌官方地址")]
        public string B_Url { get; set; }

        [Display(Name = "是否启用")]
        public int B_IsEnable { get; set; }

        [Display(Name = "是否推荐")]
        public int B_IsRecommend { get; set; }

        [Display(Name = "排序号")]
        public int B_Sort { get; set; }

        public DateTime B_CreateTime { get; set; }


    }
}
