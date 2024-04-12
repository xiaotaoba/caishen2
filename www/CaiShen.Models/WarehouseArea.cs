using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class WarehouseArea
    {
        /// <summary>
        /// 仓库配送地区
        /// </summary>
        public WarehouseArea()
        {
            this.WarehouseID = 0;
            this.IsFreeShipping = 0;
            this.Sort = 0;
            this.IsSameArea = 0;
        }
        public int ID { get; set; }

        [Display(Name = "仓库")]
        public int WarehouseID { get; set; }

        /// <summary>
        /// 配送区域名称
        /// </summary>
        [StringLength(50)]
        [Display(Name = "配送区域名称")]
        public string Title { get; set; }

        /// <summary>
        /// 逗号隔开的area_id
        /// </summary>
        [Display(Name = "配送地区")]
        public string AreaIds { get; set; }

        /// <summary>
        /// 1包邮 0计邮费
        /// </summary>
        [Display(Name = "包邮")]
        public int IsFreeShipping { get; set; }

        /// <summary>
        /// 1是 0不是
        /// </summary>
        [Display(Name = "仓库同区域")]
        public int IsSameArea { get; set; }

        /// <summary>
        /// 排序号或优先级
        /// </summary>
        [Display(Name = "优先级")]
        public int Sort { get; set; }
    }
}
