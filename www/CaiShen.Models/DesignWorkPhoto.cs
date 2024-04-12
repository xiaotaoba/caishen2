using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 作品相册
    /// </summary>
    public class DesignWorkPhoto
    {
        public DesignWorkPhoto()
        {
            this.DWP_IsFirst = 1;
            this.DWP_CreateTime = DateTime.Now;
            this.DWP_Sort = 0;
            this.DesignWorkID = 0;
        }
        public int ID { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [StringLength(300)]
        [Display(Name = "图片地址")]
        public string DWP_Image { get; set; }

        /// <summary>
        /// 图片标题
        /// </summary>
        [StringLength(50)]
        [Display(Name = "图片标题")]
        public string DWP_Title { get; set; }

        /// <summary>
        /// 是否首图
        /// </summary>
        [Display(Name = "是否首图")]
        public int DWP_IsFirst { get; set; }

        /// <summary>
        /// 设计作品ID
        /// </summary>
        [Display(Name = "设计作品ID")]
        public int DesignWorkID { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int DWP_Sort { get; set; }

        public DateTime DWP_CreateTime { get; set; }
    }
}
