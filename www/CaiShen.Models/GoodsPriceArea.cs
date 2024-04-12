using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class GoodsPriceArea
    {
        /// <summary>
        /// 产品定价区域
        /// </summary>
        public GoodsPriceArea()
        {
            this.GoodsID = 0;
            this.Sort = 0;
        }
        public int ID { get; set; }

        [Display(Name = "产品")]
        public int GoodsID { get; set; }

        /// <summary>
        /// 区域标题
        /// </summary>
        [StringLength(50)]
        [Display(Name = "区域标题")]
        public string Title { get; set; }

        /// <summary>
        /// 包含地区，逗号隔开的area_id
        /// </summary>
        [Display(Name = "包含地区")]
        public string AreaIds { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int Sort { get; set; }
    }
}
