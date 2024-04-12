using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class AdvertisementType
    {
        public AdvertisementType()
        {
            this.ADT_IsEnable = 1;
            this.ADT_Sort = 0;
            this.ADT_Class = 0;
            this.ADT_Height = 0;
            this.ADT_Width = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "分类名称由2-20个字符组成")]
        [Display(Name = "分类名称")]
        public string ADT_Name { get; set; }

        [StringLength(500)]
        [Display(Name = "分类描述")]
        public string ADT_Desc { get; set; }

        [Display(Name = "是否启用")]
        public int ADT_IsEnable { get; set; }

        [Display(Name = "高度")]
        public int ADT_Height { get; set; }

        [Display(Name = "宽度")]
        public int ADT_Width { get; set; }

        /// <summary>
        /// 0文字，1图文
        /// </summary>
        [Display(Name = "展示形式")]
        public int ADT_Class { get; set; }


        [Display(Name = "排序号")]
        public int ADT_Sort { get; set; }


    }
}
