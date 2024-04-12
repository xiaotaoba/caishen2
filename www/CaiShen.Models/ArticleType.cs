using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class ArticleType
    {
        public ArticleType()
        {
            this.AT_IsEnable = 1;
            this.AT_IsRecommend = 0;
            this.AT_Sort = 0;
            this.AT_ParentID = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "类型名称由2-20个字符组成")]
        [Display(Name = "类型名称")]
        public string AT_Name { get; set; }

        [StringLength(500)]
        [Display(Name = "类型描述")]
        public string AT_Desc { get; set; }

        [Display(Name = "是否启用")]
        public int AT_IsEnable { get; set; }

        [Display(Name = "是否推荐")]
        public int AT_IsRecommend { get; set; }

        [Display(Name = "排序号")]
        public int AT_Sort { get; set; }

        [Display(Name = "父类型")]
        public int AT_ParentID { get; set; }

    }


    public class ArticleTypeVModel
    {
        public ArticleType ArticleType { get; set; }
        public List<Article> ArticleList { get; set; }
    }
}
