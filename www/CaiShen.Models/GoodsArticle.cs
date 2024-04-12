using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class GoodsArticle
    {
        public GoodsArticle()
        {
            this.GA_IsEnable = 1;
            this.GA_IsRecommend = 0;
            this.GA_Sort = 0;
            this.GA_ShowTimes = 0;
            this.GA_IsVideo = 0;
            this.GA_CreateTime = DateTime.Now;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}由{2}-{1}个字符组成")]
        [Display(Name = "标题")]
        public string GA_Title { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0}由{2}-{1}个字符组成")]
        [Display(Name = "简短标题")]
        public string GA_ShortTitle { get; set; }

        /// <summary>
        /// 关联课程
        /// </summary>
        [Display(Name = "课程ID")]
        public int GoodsID { get; set; }

        [Display(Name = "内容")]
        public string GA_Content { get; set; }

        [Display(Name = "是否显示")]
        public int GA_IsEnable { get; set; }

        [Display(Name = "是否推荐")]
        public int GA_IsRecommend { get; set; }

        [Display(Name = "排序号")]
        public int GA_Sort { get; set; }

        [StringLength(200)]
        [Display(Name = "关键词")]
        public string GA_Keywords { get; set; }

        [StringLength(200)]
        [Display(Name = "网页描述")]
        public string GA_Description { get; set; }

        [StringLength(50)]
        [Display(Name = "作者")]
        public string GA_Author { get; set; }

        [Display(Name = "是否视频")]
        public int GA_IsVideo { get; set; }

        [StringLength(300)]
        [Display(Name = "视频链接")]
        public string GA_VideoUrl { get; set; }

        [StringLength(200)]
        [Display(Name = "展示图片")]
        public string GA_Image { get; set; }

        /// <summary>
        /// 视频时长，单位：秒
        /// </summary>
        [Display(Name = "视频时长")]
        public int GA_TimeLength { get; set; }

        [Display(Name = "浏览次数")]
        public int GA_ShowTimes { get; set; }


        [Display(Name = "创建时间")]
        public DateTime GA_CreateTime { get; set; }

    }
}
