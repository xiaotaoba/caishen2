using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class WarehouseGoods
    {
        /// <summary>
        /// 仓库产品配置
        /// </summary>
        public WarehouseGoods()
        {
            this.WarehouseID = 0;
            this.Sort = 0;
        }
        public int ID { get; set; }

        [Display(Name = "仓库")]
        public int WarehouseID { get; set; }

        /// <summary>
        /// GoodsID  
        /// </summary>
        [Display(Name = "仓库产品")]
        public int GoodsID { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int Sort { get; set; }
    }
}
