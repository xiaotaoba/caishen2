using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 问卷调查
    /// </summary>
    public class Questionnaire
    {
        public Questionnaire()
        {
            this.Quest_Count = 0;
            this.Quest_IsRecommend = 0;
            this.Quest_Status = 1;
            this.Quest_CreateTime = DateTime.Now;
            this.Quest_Sort = 0;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100)]
        [Display(Name = "标题")]
        public string Quest_Title { get; set; }

        /// <summary>
        /// 问卷调查说明
        /// </summary>
        [StringLength(200)]
        [Display(Name = "问卷调查说明")]
        public string Quest_Description { get; set; }

        /// <summary>
        /// 题数
        /// </summary>
        [Display(Name = "题数")]
        public int Quest_Count { get; set; }

        /// <summary>
        /// 1启用 0不启用
        /// </summary>
        [Display(Name = "状态")]
        public int Quest_Status { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        [Display(Name = "推荐")]
        public int Quest_IsRecommend { get; set; }

        /// <summary>
        /// 展示图片
        /// </summary>
        [StringLength(200)]
        [Display(Name = "展示图片")]
        public string Quest_Image { get; set; }

        /// <summary>
        /// 外部问卷URL
        /// </summary>
        [StringLength(200)]
        [Display(Name = "外部问卷URL")]
        public string Quest_URL { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int Quest_Sort { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime Quest_CreateTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        [Display(Name = "截止时间")]
        public DateTime? Quest_EndTime { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Display(Name = "浏览次数")]
        public int Quest_ShowTimes { get; set; }
    }
}
