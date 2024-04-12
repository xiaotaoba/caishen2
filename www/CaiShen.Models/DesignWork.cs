using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class DesignWork
    {

        public DesignWork()
        {
            this.DW_Is_Enable = 1;
            this.DW_CreateTime = DateTime.Now;
            this.DW_Type = 0;
            this.UserID = 0;
            this.DW_Style = 0;
            this.DW_IsRecommend = 0;
            this.DW_ShowTimes = 0;

        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "团队名称由2-20个字符组成")]
        [Display(Name = "成员名称")]
        public string DW_Name { get; set; }

        [StringLength(50)]
        [Display(Name = "成员编号")]
        public string DW_Number { get; set; }

        /// <summary>
        /// 设计者
        /// </summary>
        [StringLength(50)]
        [Display(Name = "设计者")]
        public string DW_Author { get; set; }

        /// <summary>
        /// 展示图片
        /// </summary>
        [StringLength(300)]
        [Display(Name = "展示图片")]
        public string DW_Image { get; set; }

        /// <summary>
        /// 个人描述
        /// </summary>
        [Display(Name = "个人描述")]
        public string DW_Desc { get; set; }

        [Display(Name = "省份")]
        public int DW_Province { get; set; }

        [Display(Name = "城市")]
        public int DW_City { get; set; }

        [Display(Name = "地区")]
        public int DW_Region { get; set; }

        [StringLength(300)]
        [Display(Name = "个人网址")]
        public string DW_URL { get; set; }

        /// <summary>
        /// 关联用户ID默认0
        /// </summary>
        [Display(Name = "会员ID")]
        public int UserID { get; set; }

        [Display(Name = "是否启用")]
        public int DW_Is_Enable { get; set; }

        [Display(Name = "创建时间")]
        public DateTime DW_CreateTime { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        [Display(Name = "推荐")]
        public int DW_IsRecommend { get; set; }

        [Display(Name = "排序号")]
        public int DW_Sort { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Display(Name = "浏览次数")]
        public int DW_ShowTimes { get; set; }
        /// <summary>
        /// 所属分类
        /// </summary>
        [Display(Name = "所属分类")]
        public int DW_Type { get; set; }

        /// <summary>
        /// 临床经验
        /// </summary>
        [Display(Name = "临床经验")]
        public int DW_Style { get; set; }

        /// <summary>
        /// 专长标签，多选逗号隔开
        /// </summary>
        [Display(Name = "专长标签")]
        [StringLength(200)]
        public string DW_TypeTags { get; set; }

        /// <summary>
        /// 评分(1星，2、3、4、5星)
        /// </summary>
        [Display(Name = "评分")]
        public int DW_Star { get; set; }

    }
}
