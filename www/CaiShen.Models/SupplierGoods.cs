using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class SupplierGoods
    {
        /// <summary>
        /// 供应商生产产品配置
        /// </summary>
        public SupplierGoods()
        {
            this.SupplierID = 0;
            this.Sort = 0;
        }
        public int ID { get; set; }

        [Display(Name = "供应商")]
        public int SupplierID { get; set; }

        /// <summary>
        /// GoodsID  
        /// </summary>
        [Display(Name = "供应产品")]
        public int GoodsID { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int Sort { get; set; }
    }
}
