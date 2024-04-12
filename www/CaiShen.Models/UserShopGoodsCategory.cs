using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserShopGoodsCategory
    {
        /// <summary>
        ///门店出售产品分类
        /// </summary>
        public UserShopGoodsCategory()
        {
            this.ShopID = 0;
            this.Sort = 0;
        }
        public int ID { get; set; }

        [Display(Name = "门店")]
        public int ShopID { get; set; }

        /// <summary>
        /// 产品分类 GoodsCategoryID
        /// </summary>
        [Display(Name = "出售产品分类")]
        public int GoodsCategoryID { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int Sort { get; set; }
    }
}
