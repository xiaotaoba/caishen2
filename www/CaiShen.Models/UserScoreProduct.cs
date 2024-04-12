using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserScoreProduct
    {
        public UserScoreProduct()
        {
            this.UserShopID = 0;
            this.ScoreProductID = 0;
            this.UserID = 0;
            this.USP_Status = 0;
            this.USP_Time = DateTime.Now;
            this.USP_IsDelete = 0;
        }
        public int ID { get; set; }

        /// <summary>
        ///积分兑换商品ID
        /// </summary>
        [Display(Name = "积分兑换商品")]
        public int ScoreProductID { get; set; }

        /// <summary>
        ///会员
        /// </summary>
        [Display(Name = "会员")]
        public int UserID { get; set; }

        [Display(Name = "兑换时间")]
        public DateTime USP_Time { get; set; }

        /// <summary>
        /// 默认0:平台，是否为门店自定义商品
        /// </summary>
        [Display(Name = "门店")]
        public int UserShopID { get; set; }

        [Display(Name = "收货地址ID")]
        public int UserAddressID { get; set; }

        [Display(Name = "收货地址")]
        [StringLength(100)]
        public string USP_Address { get; set; }

        [Display(Name = "快递公司")]
        [StringLength(50)]
        public string USP_ShippingCompany { get; set; }

        [Display(Name = "快递单号")]
        [StringLength(50)]
        public string USP_ShippingNo { get; set; }

        /// <summary>
        /// 是否删除,0否 1是
        /// </summary>
        [Display(Name = "是否删除")]
        public int USP_IsDelete { get; set; }

        /// <summary>
        /// 默认：待发货，已发货，兑换完成
        /// </summary>
        [Display(Name = "状态")]
        public int USP_Status { get; set; }
    }

    /// <summary>
    /// 用户积分兑换记录
    /// </summary>
    public class UserScoreProductVmodel
    {
        public UserScoreProduct UserScoreProduct { get; set; }
        public ScoreProduct ScoreProduct { get; set; }

        public string U_UserName { get; set; }

    }
}
