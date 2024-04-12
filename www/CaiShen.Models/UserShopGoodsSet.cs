using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserShopGoodsSet
    {
        /// <summary>
        /// 门店商品设置（价格，推荐，排序）
        /// </summary>
        public UserShopGoodsSet()
        {
            this.ShopID = 0;
            this.GoodsID = 0;
            this.UG_IsRecommend = 0;
            this.UG_Sort = 0;
            this.UG_Discount = 1;
        }
        public int ID { get; set; }

        /// <summary>
        /// 门店ID
        /// </summary>
        [Display(Name = "门店")]
        public int ShopID { get; set; }

        /// <summary>
        /// 产品ID  
        /// </summary>
        [Display(Name = "产品ID")]
        public int GoodsID { get; set; }

        /// <summary>
        ///价格折扣（相对终端价），取值0.1即1折
        /// </summary>
        [Display(Name = "价格折扣")]
        public double UG_Discount { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        [Display(Name = "推荐")]
        public int UG_IsRecommend { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int UG_Sort { get; set; }

        ///<summary>
        ///展示图片
        ///</summary>
        [StringLength(200)]
        [Display(Name = "展示图片")]
        public string UG_Image { get; set; }

        /// <summary>
        /// 手机展示图片
        /// </summary>
        [StringLength(200)]
        [Display(Name = "手机展示图片")]
        public string UG_MobileImage { get; set; }

        /// <summary>
        /// 手机推荐
        /// </summary>
        [Display(Name = "手机推荐")]
        public int UG_IsRecommendMobile { get; set; }

        /// <summary>
        /// 手机排序号
        /// </summary>
        [Display(Name = "手机排序号")]
        public int UG_SortMobile { get; set; }
    }
}
