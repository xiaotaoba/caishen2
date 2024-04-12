using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserShopGoods
    {
        /// <summary>
        /// 门店出售产品配置
        /// </summary>
        public UserShopGoods()
        {
            this.ShopID = 0;
            this.Sort = 0;
        }
        public int ID { get; set; }

        [Display(Name = "门店")]
        public int ShopID { get; set; }

        /// <summary>
        /// GoodsID  
        /// </summary>
        [Display(Name = "出售产品")]
        public int GoodsID { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int Sort { get; set; }
    }
}
