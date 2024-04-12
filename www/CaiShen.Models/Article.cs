using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class Article
    {
        public Article()
        {
            this.Art_IsEnable = 1;
            this.Art_IsRecommend = 0;
            this.Art_Sort = 0;
            this.ArticleTypeID = 0;
            this.Art_ShowTimes = 0;
            this.Art_IsUrl = 0;
            this.Art_CreateTime = DateTime.Now;
            this.Art_WX_Type = 1;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}由{2}-{1}个字符组成")]
        [Display(Name = "文章标题")]
        public string Art_Title { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0}由{2}-{1}个字符组成")]
        [Display(Name = "简短标题")]
        public string Art_ShortTitle { get; set; }

        [Display(Name = "文章内容")]
        public string Art_Content { get; set; }

        [Display(Name = "是否显示")]
        public int Art_IsEnable { get; set; }

        [Display(Name = "是否推荐")]
        public int Art_IsRecommend { get; set; }

        [Display(Name = "排序号")]
        public int Art_Sort { get; set; }

        [StringLength(200)]
        [Display(Name = "关键词")]
        public string Art_Keywords { get; set; }

        [StringLength(200)]
        [Display(Name = "网页描述")]
        public string Art_Description { get; set; }

        [StringLength(50)]
        [Display(Name = "作者")]
        public string Art_Author { get; set; }

        [StringLength(50)]
        [Display(Name = "来源")]
        public string Art_From { get; set; }

        [Display(Name = "是否外部链接")]
        public int Art_IsUrl { get; set; }

        [Display(Name = "浏览次数")]
        public int Art_ShowTimes { get; set; }

        [StringLength(300)]
        [Display(Name = "外部链接")]
        public string Art_Url { get; set; }

        [StringLength(200)]
        [Display(Name = "展示图片")]
        public string Art_Image { get; set; }

        [Display(Name = "分类")]
        public int ArticleTypeID { get; set; }

        [Display(Name = "创建时间")]
        public DateTime Art_CreateTime { get; set; }

        /// <summary>
        /// 发送对象类型：1:按部门，2按openid，最多10人
        /// </summary>
        [Display(Name = "发送对象类型")]
        public int Art_WX_Type { get; set; }

        /// <summary>
        /// Art_WX_Type=2时，填写接收人
        /// </summary>
        [StringLength(500)]
        public string Art_WX_Openids { get; set; }

        /// <summary>
        /// Art_WX_Type=1时，选择部门ID，0是发送给所有人
        /// </summary>
        public int Art_WX_DepartmentID { get; set; }

    }

    public class ArticleVModel
    {
        public ArticleType ArticleType { get; set; }
        public Article Article { get; set; }
    }
}
