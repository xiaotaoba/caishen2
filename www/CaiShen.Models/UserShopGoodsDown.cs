using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserShopGoodsDown
    {
        /// <summary>
        /// 门店下架商品
        /// </summary>
        public UserShopGoodsDown()
        {
            this.ShopID = 0;
            this.GoodsID = 0;
        }
        public int ID { get; set; }

        [Display(Name = "门店")]
        public int ShopID { get; set; }

        /// <summary>
        /// GoodsID  
        /// </summary>
        [Display(Name = "下架产品")]
        public int GoodsID { get; set; }
    }
}
