using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    /// <summary>
    /// 红包
    /// </summary>
    public class HongBao
    {
        public HongBao()
        {
            this.UserShopID = 0;
            this.HB_Amount = 0;
            this.HB_RestAmount = 0;
            this.HB_Count = 0;
            this.HB_CreateUser = 0;
            this.HB_IsDelete = 0;
            this.HB_MaxAmount = 0;
            this.HB_MinAmount = 0;
            this.HB_Status = 0;
            this.HB_Type = 1;
            this.HB_UserLevel = 0;
            this.UserShopID = 0;
            this.HB_Sort = 0;
        }
        public int ID { get; set; }

        /// <summary>
        ///创建人ID，管理员ID或门店用户ID
        /// </summary>
        [Display(Name = "创建人")]
        public int HB_CreateUser { get; set; }

        /// <summary>
        ///红包名称
        /// </summary>
        [Display(Name = "红包名称")]
        [StringLength(100)]
        [Required]
        public string HB_Name { get; set; }

        /// <summary>
        ///发放红包金额
        /// </summary>
        [Display(Name = "发放红包金额")]
        [Required]
        public decimal HB_Amount { get; set; }

        /// <summary>
        ///剩余红包金额
        /// </summary>
        [Display(Name = "剩余红包金额")]
        [Required]
        public decimal HB_RestAmount { get; set; }

        /// <summary>
        ///最大红包金额
        /// </summary>
        [Display(Name = "最大红包金额")]
        [Required]
        public decimal HB_MaxAmount { get; set; }

        /// <summary>
        ///最小红包金额
        /// </summary>
        [Display(Name = "最小红包金额")]
        [Required]
        public decimal HB_MinAmount { get; set; }

        /// <summary>
        ///红包类型(默认1:注册红包,备用2:活动红包)
        /// </summary>
        [Display(Name = "红包类型")]
        public int HB_Type { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        [Display(Name = "活动开始时间")]
        public DateTime HB_BeginTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        [Display(Name = "活动结束时间")]
        public DateTime HB_EndTime { get; set; }

        /// <summary>
        /// 红包有效期-如：领取15天内有效
        /// </summary>
        [Display(Name = "红包有效期")]
        public int HB_ValidDate { get; set; }

        /// <summary>
        /// 发放数量
        /// </summary>
        [Display(Name = "发放数量")]
        public int HB_Count { get; set; }

        /// <summary>
        /// 剩余发放数量
        /// </summary>
        [Display(Name = "剩余发放数量")]
        public int HB_RestCount { get; set; }

        /// <summary>
        /// 适用会员等级 默认0：不限
        /// </summary>
        [Display(Name = "适用会员等级")]
        public int HB_UserLevel { get; set; }

        /// <summary>
        /// 红包说明
        /// </summary>
        [Display(Name = "红包说明")]
        [StringLength(500)]
        public string HB_Description { get; set; }

        /// <summary>
        /// 默认0:平台通用，是否为门店红包
        /// </summary>
        [Display(Name = "所属门店")]
        public int UserShopID { get; set; }

        /// <summary>
        /// 是否删除,0否 1是
        /// </summary>
        [Display(Name = "是否删除")]
        public int HB_IsDelete { get; set; }

        /// <summary>
        /// 状态 0-未开始 1-已开始 2-已结束 3-已中止 4-已领完 
        /// </summary>
        [Display(Name = "状态")]
        public int HB_Status { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int HB_Sort { get; set; }

    }
}
