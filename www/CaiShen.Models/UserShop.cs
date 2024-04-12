using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    public class UserShop
    {

        public UserShop()
        {
            this.Shop_Is_Enable = 1;
            this.Shop_CreateTime = DateTime.Now;
            this.Shop_SaleGoodsWay = 1;
            this.RestGuaranteeMoney = 0;
            this.TotalGuaranteeMoney = 0;
            this.Shop_IsYufu = 0;
            this.Shop_IsDaofu = 0;
            this.Shop_IsZiti = 0;
            this.Shop_Yufu_Percent = 1;
            this.Shop_Type = 1;
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "店铺名称由2-20个字符组成")]
        [Display(Name = "店铺名称")]
        public string Shop_Name { get; set; }

        [StringLength(50)]
        [Display(Name = "店铺编号")]
        public string Shop_Number { get; set; }

        /// <summary>
        /// 店铺描述
        /// </summary>
        [StringLength(2000)]
        [Display(Name = "店铺描述")]
        public string Shop_Desc { get; set; }

        [Display(Name = "省份")]
        public int Shop_Province { get; set; }

        [Display(Name = "城市")]
        public int Shop_City { get; set; }

        [Display(Name = "地区")]
        public int Shop_Region { get; set; }

        [StringLength(500)]
        [Display(Name = "详细地址")]
        public string Shop_Address { get; set; }

        /// <summary>
        /// 地图网址
        /// </summary>
        [StringLength(500)]
        [Display(Name = "地图网址")]
        public string Shop_MapUrl { get; set; }


        [StringLength(50)]
        [Display(Name = "联系电话")]
        public string Shop_Tel { get; set; }

        /// <summary>
        /// 手机号码,门店手机号码，用于接收系统通知短信，最多可设置3个，用英文逗号隔开
        /// </summary>
        [StringLength(50)]
        [Display(Name = "手机号码")]
        public string Shop_Mobile { get; set; }

        [StringLength(50)]
        [Display(Name = "联系人")]
        public string Shop_UserName { get; set; }

        /// <summary>
        /// QQ，多个逗号隔开
        /// </summary>
        [StringLength(100)]
        [Display(Name = "QQ")]
        public string Shop_QQ { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        [StringLength(50)]
        [Display(Name = "微信号")]
        public string Shop_Weixin { get; set; }

        /// <summary>
        /// 微信二维码
        /// </summary>
        [StringLength(200)]
        [Display(Name = "微信二维码")]
        public string Shop_WeixinQrCode { get; set; }

        [StringLength(200)]
        [Display(Name = "店铺网址")]
        public string Shop_URL { get; set; }

        [Display(Name = "会员ID")]
        public int UserID { get; set; }

        //public virtual User User { get; set; }

        /// <summary>
        /// 是否支持自提
        /// </summary>
        [Display(Name = "是否支持自提")]
        public int Shop_IsZiti { get; set; }

        /// <summary>
        /// 是否支持预付款
        /// </summary>
        [Display(Name = "是否支持预付款")]
        public int Shop_IsYufu { get; set; }

        /// <summary>
        /// 是否支持货到付款
        /// </summary>
        [Display(Name = "是否支持货到付款")]
        public int Shop_IsDaofu { get; set; }

        /// <summary>
        /// 预付款比例
        /// </summary>
        [Display(Name = "预付款比例")]
        public double Shop_Yufu_Percent { get; set; }

        /// <summary>
        /// 预付定金-总保障金额度
        /// </summary>
        [Display(Name = "总保障金额度")]
        public decimal TotalGuaranteeMoney { get; set; }

        /// <summary>
        /// 预付定金-剩余保障金额度
        /// </summary>
        [Display(Name = "剩余保障金额度")]
        public decimal RestGuaranteeMoney { get; set; }

        [Display(Name = "是否启用")]
        public int Shop_Is_Enable { get; set; }

        [StringLength(200)]
        [Display(Name = "关闭理由")]
        public string Shop_CloseReason { get; set; }

        [Display(Name = "创建时间")]
        public DateTime Shop_CreateTime { get; set; }

        /// <summary>
        /// 默认1:出售所有产品,2:按分类选择，3:自定义选择产品
        /// </summary>
        [Display(Name = "设置出售产品")]
        public int Shop_SaleGoodsWay { get; set; }

        /// <summary>
        /// 运费模板
        /// </summary>
        [Display(Name = "运费模板")]
        public int ShippingTemplateID { get; set; }

        /// <summary>
        /// 加盟商类型
        /// </summary>
        [Display(Name = "加盟商类型")]
        public int Shop_Type { get; set; }

        /// <summary>
        /// 评分(1星，2、3、4、5星)
        /// </summary>
        [Display(Name = "评分")]
        public int Shop_Star { get; set; }

        /// <summary>
        /// 营业时间
        /// </summary>
        [StringLength(50)]
        [Display(Name = "营业时间")]
        public string Shop_BusinessHours { get; set; }
    }
}
