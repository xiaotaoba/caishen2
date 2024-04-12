using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pannet.Models
{
    public class Navigation
    {
        public Navigation()
        {
            this.Nav_FollowID = 0;
            this.Nav_Order = 0;
            this.Nav_IsEnable = 1;
            this.Nav_Type = 1;
            this.Nav_IsRecommend = 0;
            this.Nav_Target = "_blank";
        }

        public int ID { get; set; }

        /// <summary>
        /// 导航名称
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "标题")]
        public string Nav_Name { get; set; }

        /// <summary>
        /// 短标题
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "短标题")]
        public string Nav_ShortName { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        [StringLength(50)]
        [Display(Name = "英文名称")]
        public string Nav_EnName { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [StringLength(500)]
        [Display(Name = "链接地址")]
        public string Nav_Url { get; set; }

        /// <summary>
        /// 打开方式 _blank _top _parent
        /// </summary>
        [StringLength(20)]
        [Display(Name = "打开方式")]
        public string Nav_Target { get; set; }

        /// <summary>
        /// 上级导航
        /// </summary>
        [Display(Name = "上级导航")]
        public int Nav_FollowID { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int Nav_Order { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public int Nav_IsEnable { get; set; }
        /// <summary>
        /// 导航分类(默认1推荐分类产品)
        /// </summary>
        [Display(Name = "分类")]
        public int Nav_Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        [Display(Name = "备注")]
        public string Nav_Remark { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        [Display(Name = "推荐")]
        public int Nav_IsRecommend { get; set; }

    }
}
