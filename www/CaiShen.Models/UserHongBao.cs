using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 用户红包
    /// </summary>
    public class UserHongBao
    {
        public UserHongBao()
        {
            this.UHB_IsDelete = 0;
            this.UHB_Status = 0;
            this.HongBaoID = 0;
            this.UBH_Amount = 0;
            this.UHB_ReceiveTime = DateTime.Now;
            this.UserID = 0;
        }
        public int ID { get; set; }

        /// <summary>
        ///红包ID
        /// </summary>
        [Display(Name = "红包ID")]
        public int HongBaoID { get; set; }

        /// <summary>
        ///UserID
        /// </summary>
        [Display(Name = "UserID")]
        public int UserID { get; set; }

        /// <summary>
        ///红包名称
        /// </summary>
        [StringLength(100)]
        [Display(Name = "红包名称")]
        public string UBH_Title { get; set; }

        /// <summary>
        ///红包金额
        /// </summary>
        [Display(Name = "红包金额")]
        public decimal UBH_Amount { get; set; }

        /// <summary>
        /// 发放时间
        /// </summary>
        [Display(Name = "发放时间")]
        public DateTime UHB_ReceiveTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Display(Name = "过期时间")]
        public DateTime UHB_ExpirationTime { get; set; }

        /// <summary>
        /// 是否删除,0否 1是
        /// </summary>
        [Display(Name = "是否删除")]
        public int UHB_IsDelete { get; set; }

        /// <summary>
        /// 红包使用状态 0-未使用 1-已使用
        /// </summary>
        [Display(Name = "状态")]
        public int UHB_Status { get; set; }
    }

    /// <summary>
    /// 红包
    /// </summary>
    public class UserHongBaoVmodel
    {
        public UserHongBao UserHongBao { get; set; }
        public HongBao HongBao { get; set; }

        public string U_UserName { get; set; }

    }
}
