using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 优惠券
    /// </summary>
    public class UserCoupon
    {
        public UserCoupon()
        {
            this.UCP_IsDelete = 0;
            this.UCP_Status = 0;
            this.CouponInfoID = 0;
            this.UCP_ReceiveTime = DateTime.Now;
            this.UserID = 0;
        }
        public int ID { get; set; }

        /// <summary>
        ///优惠券ID
        /// </summary>
        [Display(Name = "优惠券ID")]
        public int CouponInfoID { get; set; }

        /// <summary>
        ///UserID
        /// </summary>
        [Display(Name = "UserID")]
        public int UserID { get; set; }

        /// <summary>
        /// 领取时间
        /// </summary>
        [Display(Name = "领取时间")]
        public DateTime UCP_ReceiveTime { get; set; }

        /// <summary>
        /// 是否删除,0否 1是
        /// </summary>
        [Display(Name = "是否删除")]
        public int UCP_IsDelete { get; set; }

        /// <summary>
        /// 优惠券状态 0-未使用 1-已使用
        /// </summary>
        [Display(Name = "状态")]
        public int UCP_Status { get; set; }
    }

    /// <summary>
    /// 优惠券
    /// </summary>
    public class UserCouponVmodel
    {
        public UserCoupon UserCoupon { get; set; }
        public CouponInfo CouponInfo { get; set; }

        public string U_UserName { get; set; }

    }
}
