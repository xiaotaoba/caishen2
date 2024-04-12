using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pannet.Models
{
    /// <summary>
    /// 浏览记录
    /// </summary>
    public class BrowseRecord
    {
        public BrowseRecord()
        {
            this.BR_CreateTime = DateTime.Now;
            this.BR_ItemType = 1;
        }

        public int ID { get; set; }

        [Required]
        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        [Display(Name = "门店")]
        public int ShopID { get; set; }

        /// <summary>
        /// 记录ID，产品ID，文章ID,分类ID等
        /// </summary>
        [Display(Name = "记录ID")]
        public Int32 BR_ItemID { get; set; }

        /// <summary>
        /// 记录类型，产品1，分类2，文章3等
        /// </summary>
        [Display(Name = "记录类型")]
        public int BR_ItemType { get; set; }

        [StringLength(50)]
        [Display(Name = "IP地址")]
        public string BR_IP { get; set; }

        [StringLength(500)]
        [Display(Name = "页面URL")]
        public string BR_URL { get; set; }

        /// <summary>
        /// 浏览时间
        /// </summary>
        public DateTime BR_CreateTime { get; set; }
    }

    public class BrowseRecordGoodsVModel
    {
        public BrowseRecord BrowseRecord { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Price { get; set; }
    }
}
