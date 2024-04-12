using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pannet.Models
{
    #region 系统配置信息
    /// <summary>
    /// 系统配置信息
    /// </summary>
    public class SystemInfo
    {
        public SystemInfo()
        {
            this.Sys_Is_Enable = 1;
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "网站名称由2-20个字符组成")]
        [Display(Name = "网站名称")]
        public string Sys_SiteName { get; set; }

        [StringLength(100)]
        [Display(Name = "网站Title")]
        public string Sys_WebTitle { get; set; }

        [Display(Name = "版权信息")]
        public string Sys_Copyright { get; set; }

        [StringLength(200)]
        [Display(Name = "网站Keywords")]
        public string Sys_WebKeywords { get; set; }

        [StringLength(200)]
        [Display(Name = "网站Description")]
        public string Sys_WebDescription { get; set; }

        [StringLength(200)]
        [Display(Name = "网站LOGO")]
        public string Sys_LogoUrl { get; set; }

        [StringLength(50)]
        [Display(Name = "公司名称")]
        public string Sys_Company { get; set; }

        [StringLength(50)]
        [Display(Name = "联系地址")]
        public string Sys_Address { get; set; }

        [StringLength(50)]
        [Display(Name = "联系电话")]
        public string Sys_Tel { get; set; }

        [StringLength(100)]
        [Display(Name = "官网网址")]
        public string Sys_Domain { get; set; }

        [Display(Name = "是否启用")]
        public int Sys_Is_Enable { get; set; }

        [StringLength(200)]
        [Display(Name = "关闭理由")]
        public string Sys_CloseReason { get; set; }

    }

    #endregion

    #region 微信配置

    /// <summary>
    /// 微信配置
    /// </summary>
    public class WeixinConfig
    {
        public WeixinConfig()
        {
        }
        public int ID { get; set; }
        /// <summary>
        /// 微信公众号名称
        /// </summary>
        [StringLength(100)]
        [Display(Name = "公众号/小程序名称")]
        public string WeixinName { get; set; }

        [StringLength(100)]
        [Display(Name = "AppID")]
        public string AppID { get; set; }

        [StringLength(100)]
        [Display(Name = "AppSecret")]
        public string AppSecret { get; set; }

        [StringLength(100)]
        [Display(Name = "Token")]
        public string Token { get; set; }

        [StringLength(100)]
        [Display(Name = "EncodingAESKey")]
        public string EncodingAESKey { get; set; }

        /// <summary>
        /// 商户号(MCHID)
        /// </summary>
        [StringLength(100)]
        [Display(Name = "商户号(MCHID)")]
        public string MCHID { get; set; }

        /// <summary>
        /// 商户支付密钥(KEY)
        /// </summary>
        [StringLength(100)]
        [Display(Name = "商户支付密钥(KEY)")]
        public string KEY { get; set; }

        /// <summary>
        /// 支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        [StringLength(300)]
        [Display(Name = "支付结果通知url")]
        public string TenpayNotify { get; set; }

    }

    #endregion

    #region 支付宝配置

    /// <summary>
    /// 支付宝配置
    /// </summary>
    public class AlipayConfig
    {
        public AlipayConfig()
        {
        }
        public int ID { get; set; }
        /// <summary>
        /// 卖家支付宝帐户
        /// </summary>
        [StringLength(100)]
        [Display(Name = "卖家支付宝帐户")]
        public string Seller_email { get; set; }

        /// <summary>
        /// 合作者身份ID
        /// </summary>
        [StringLength(100)]
        [Display(Name = "合作者身份ID")]
        public string Partner { get; set; }

        /// <summary>
        /// 交易安全检验码
        /// </summary>
        [StringLength(100)]
        [Display(Name = "交易安全检验码")]
        public string Key { get; set; }

        /// <summary>
        /// 收款方名称，如：公司名称、网站名称、收款人姓名等
        /// </summary>
        [StringLength(100)]
        [Display(Name = "收款方名称")]
        public string Mainname { get; set; }
    }

    #endregion

    #region 系统参数设置

    /// <summary>
    /// 系统参数设置
    /// </summary>
    public class ProfitPercentConfig
    {
        public ProfitPercentConfig()
        {
        }
        public int ID { get; set; }
        /// <summary>
        /// 系统默认店铺付款定金比例，在店铺未设置情况下使用
        /// </summary>
        [Display(Name = "默认店铺付款定金比例")]
        public double Shop_Yufu_Percent { get; set; }
        /// <summary>
        /// 订单（经营收益）合作伙伴分成比例20%
        /// </summary>
        [Display(Name = "经营收益合作伙伴分成比例")]
        public double Order_Partner_Profit_Percent { get; set; }

        /// <summary>
        /// 订单（经营收益）门店分成比例 80%
        /// </summary>
        [Display(Name = "经营收益门店分成比例")]
        public double Order_Shop_Profit_Percent { get; set; }

        /// <summary>
        /// 加盟费——合作伙伴分成比例49%
        /// </summary>
        [Display(Name = "加盟费合作伙伴分成比例")]
        public double Join_Partner_Profit_Percent { get; set; }

        /// <summary>
        ///加盟费——平台分成比例 51%
        /// </summary>
        [Display(Name = "加盟费平台分成比例")]
        public double Join_Platform_Profit_Percent { get; set; }

        /// <summary>
        ///加盟商结算日
        /// </summary>
        [Display(Name = "加盟商结算日")]
        public string Shop_JieSuan_Day { get; set; }

        /// <summary>
        ///订单允许退货天数
        /// </summary>
        [Display(Name = "订单允许退货天数")]
        public int Order_TuiHuo_Days { get; set; }

        /// <summary>
        ///短信API产品名称
        /// </summary>
        [Display(Name = "短信API产品名称")]
        public int Sms_Product { get; set; }

        /// <summary>
        ///短信API产品域名
        /// </summary>
        [Display(Name = "短信API产品域名")]
        public int Sms_Domain { get; set; }

        /// <summary>
        ///短信accessKeyId
        /// </summary>
        [Display(Name = "短信accessKeyId")]
        public int Sms_AccessKeyId { get; set; }

        /// <summary>
        ///短信accessKeySecret
        /// </summary>
        [Display(Name = "短信accessKeySecret")]
        public int Sms_AccessKeySecret { get; set; }
    }

    #endregion

    #region 邮箱参数设置

    /// <summary>
    /// 邮箱参数设置
    /// </summary>
    public class SMTPConfig
    {
        public SMTPConfig()
        {
        }
        /// <summary>
        /// SMTP服务器,如：smtp.163.com
        /// </summary>
        [Display(Name = "SMTP服务器")]
        public string Server { get; set; }
        /// <summary>
        /// Authentication 身份认证
        /// </summary>
        [Display(Name = "身份认证")]
        public string Authentication { get; set; }

        /// <summary>
        /// 发件邮箱
        /// </summary>
        [Display(Name = "发件邮箱")]
        public string Sender { get; set; }

        /// <summary>
        /// 发件箱账号
        /// </summary>
        [Display(Name = "发件箱账号")]
        public string User { get; set; }

        /// <summary>
        ///发件箱密码
        /// </summary>
        [Display(Name = "发件箱密码")]
        public string Password { get; set; }
    }

    #endregion
}
