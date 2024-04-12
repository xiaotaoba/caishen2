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
    public class CouponInfo
    {
        public CouponInfo()
        {
            this.UserShopID = 0;
            this.CP_Amount = 0;
            this.CP_Count = 0;
            this.CP_CreateUser = 0;
            this.CP_IsDelete = 0;
            this.CP_MaxAmount = 0;
            this.CP_NeedAmount = 0;
            this.CP_Status = 0;
            this.CP_Type = 1;
            this.CP_UserLevel = 0;
            this.CP_UsingRange = 1;
            this.CP_UsingRangeIds = "";
            this.UserShopID = 0;
            this.CP_Sort = 0;
        }
        public int ID { get; set; }

        /// <summary>
        ///创建人ID，管理员ID或门店用户ID
        /// </summary>
        [Display(Name = "创建人")]
        public int CP_CreateUser { get; set; }

        /// <summary>
        ///优惠券名称
        /// </summary>
        [Display(Name = "优惠券名称")]
        [StringLength(100)]
        [Required]
        public string CP_Name { get; set; }

        /// <summary>
        ///活动图片
        /// </summary>
        [Display(Name = "活动图片")]
        [StringLength(300)]
        public string CP_Photo { get; set; }

        /// <summary>
        ///优惠券编号
        /// </summary>
        [Display(Name = "优惠券编号")]
        [StringLength(20)]
        [Required]
        public string CP_NO { get; set; }

        /// <summary>
        ///优惠券面额
        /// </summary>
        [Display(Name = "优惠券面额")]
        [Required]
        public decimal CP_Amount { get; set; }

        /// <summary>
        ///满多少金额可使用
        /// </summary>
        [Display(Name = "满多少金额可使用")]
        [Required]
        public decimal CP_NeedAmount { get; set; }

        /// <summary>
        ///优惠最高金额
        /// </summary>
        [Display(Name = "优惠最高金额")]
        public decimal CP_MaxAmount { get; set; }

        /// <summary>
        ///优惠券类型(默认1:满减券,备用2:折扣券,3:现金券)
        /// </summary>
        [Display(Name = "优惠券类型")]
        public int CP_Type { get; set; }

        /// <summary>
        /// 优惠券使用开始时间
        /// </summary>
        [Display(Name = "优惠券使用开始时间")]
        public DateTime CP_BeginTime { get; set; }

        /// <summary>
        /// 优惠券使用结束时间
        /// </summary>
        [Display(Name = "优惠券使用结束时间")]
        public DateTime CP_EndTime { get; set; }

        /// <summary>
        /// 优惠券数量
        /// </summary>
        [Display(Name = "优惠券数量")]
        public int CP_Count { get; set; }

        /// <summary>
        /// 适用会员等级 默认0：不限
        /// </summary>
        [Display(Name = "适用会员等级")]
        public int CP_UserLevel { get; set; }

        /// <summary>
        /// 优惠券说明
        /// </summary>
        [Display(Name = "优惠券说明")]
        [StringLength(500)]
        public string CP_Description { get; set; }

        /// <summary>
        /// 默认0:平台，是否为门店优惠券
        /// </summary>
        [Display(Name = "所属门店")]
        public int UserShopID { get; set; }

        /// <summary>
        ///优惠券适用范围1:平台通用类,2:店铺通用类,3:品类通用类,4:特定商品使用
        /// </summary>
        [Display(Name = "优惠券适用范围")]
        public int CP_UsingRange { get; set; }

        /// <summary>
        /// 优惠券适用范围指定ID,0或空平台通用，店铺Ids
        /// </summary>
        [Display(Name = "优惠券适用范围指定ID")]
        public string CP_UsingRangeIds { get; set; }

        /// <summary>
        /// 是否删除,0否 1是
        /// </summary>
        [Display(Name = "是否删除")]
        public int CP_IsDelete { get; set; }

        /// <summary>
        /// 优惠券状态 0-未开始 1-已开始 2-已结束 3-已中止 4-已领完 
        /// </summary>
        [Display(Name = "状态")]
        public int CP_Status { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int CP_Sort { get; set; }

    }
}
