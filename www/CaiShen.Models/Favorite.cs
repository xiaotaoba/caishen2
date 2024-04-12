using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class Favorite
    {
        /// <summary>
        /// 收藏夹
        /// </summary>
        public Favorite()
        {
            this.GoodsID = 0;
            this.ShopID = 0;
            this.UserID = 0;
            this.Fav_Time = DateTime.Now;
        }
        public int ID { get; set; }

        [Display(Name = "产品ID")]
        public int GoodsID { get; set; }

        [Display(Name = "门店ID")]
        public int ShopID { get; set; }

        [Display(Name = "用户ID")]
        public int UserID { get; set; }

        public DateTime Fav_Time { get; set; }
    }

    public class FavoriteVModel
    {
        public Favorite Favorite { get; set; }
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Price { get; set; }
    }
}
