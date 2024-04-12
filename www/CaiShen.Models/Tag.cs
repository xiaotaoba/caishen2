using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag
    {
        public Tag()
        {
            this.T_ShowTimes = 0;
            this.T_Type = 0;
            this.T_FollowID = 0;
        }
        public int ID { get; set; }


        /// <summary>
        /// 分类，如：1设计作品风格，2设计作品户型，3产品类...
        /// </summary>
        [Display(Name = "分类")]
        public int T_Type { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        [Display(Name = "标签名称")]
        [StringLength(50)]
        public string T_Name { get; set; }

        /// <summary>
        /// 上级ID，默认0为一级
        /// </summary>
        [Display(Name = "上级")]
        public int T_FollowID { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int T_Sort { get; set; }

        /// <summary>
        /// 展示次数
        /// </summary>
        [Display(Name = "展示次数")]
        public int T_ShowTimes { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public int GC_IsEnable { get; set; }
    }
}
