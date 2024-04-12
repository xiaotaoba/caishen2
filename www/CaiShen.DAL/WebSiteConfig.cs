using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.DAL.Repository;
using Pannet.Utility;

namespace Pannet.DAL
{
    public class WebSiteConfig
    {
        private static ProfitPercentConfig percentConfig = null;
        private static WeixinConfig weixinConfig = null;
        private static AlipayConfig alipayConfig = null;
         static WebSiteConfig()
        {
            percentConfig = SystemInfoService.GetPercentConfig();
            if (percentConfig != null)
            {
                _Join_Partner_Profit_Percent = percentConfig.Join_Partner_Profit_Percent;
                _Join_Platform_Profit_Percent = percentConfig.Join_Platform_Profit_Percent;
                _Order_Partner_Profit_Percent = percentConfig.Order_Partner_Profit_Percent;
                _Order_Shop_Profit_Percent = percentConfig.Order_Shop_Profit_Percent;
                _Shop_Yufu_Percent = percentConfig.Shop_Yufu_Percent;
            }

            weixinConfig = SystemInfoService.GetWeixinConfig();
            alipayConfig = SystemInfoService.GetAlipayConfig();

        }

        #region 网站配置信息

        public static string CompanyName { get { return "杭州盘网互联信息技术有限公司"; } }
        public static string CompanyUrl { get { return "http://www.pannet.cn/"; } }
        public static string WebSiteName { get { return ConfigHelper.GetConfigString("WebSiteName"); } }
        public static string SystemName { get { return "PNSHOP商城管理系统"; } }
        public static string SystemVersion { get { return "V3.01"; } }
        /// <summary>
        /// 管理后台域名
        /// </summary>
        public static string ManagerDomain { get { return ConfigHelper.GetConfigString("ManagerDomain"); } }
        /// <summary>
        /// 图片站点域名
        /// </summary>
        public static string ImgDomain { get { return ConfigHelper.GetConfigString("ImgDomain"); } }
        /// <summary>
        /// 站点主域名
        /// </summary>
        public static string WebSiteMainDomain { get { return ConfigHelper.GetConfigString("WebSiteMainDomain"); } }
        /// <summary>
        /// 主站点域名
        /// </summary>
        public static string WebSiteDomain { get { return ConfigHelper.GetConfigString("WebSiteDomain"); } }

        #endregion

        #region 微信公众号/微信支付

        /// <summary>
        /// APPID
        /// </summary>
        public static string WeixinAppId
        {
            get { return weixinConfig.AppID; }
        }
        /// <summary>
        /// 公众号、小程序密钥
        /// </summary>
        public static string WeixinAppSecret
        {
            get { return weixinConfig.AppSecret; }
        }
        public static string WeixinEncodingAESKey
        {
            get { return weixinConfig.EncodingAESKey; }
        }
        public static string WeixinToken
        {
            get { return weixinConfig.Token; }
        }
        /// <summary>
        /// 支付密钥
        /// </summary>
        public static string WeixinKEY
        {
            get { return weixinConfig.KEY; }
        }
        /// <summary>
        /// 商户号
        /// </summary>
        public static string WeixinMCHID
        {
            get { return weixinConfig.MCHID; }
        }
        /// <summary>
        /// 支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        public static string WeixinNOTIFY_URL
        {
            get { return weixinConfig.TenpayNotify; }
        }
        #endregion

        #region 收益提成比例

        /// <summary>
        /// 系统默认店铺付款定金比例，在店铺未设置情况下使用
        /// </summary>
        private static double _Shop_Yufu_Percent = 0.3;

        /// <summary>
        /// 订单（经营收益）合作伙伴分成比例
        /// </summary>
        private static double _Order_Partner_Profit_Percent = 0.2;

        /// <summary>
        /// 订单（经营收益）门店分成比例
        /// </summary>
        private static double _Order_Shop_Profit_Percent = 0.8;

        /// 加盟费——合作伙伴分成比例
        /// </summary>
        private static double _Join_Partner_Profit_Percent = 0.49;

        /// <summary>
        /// 加盟费——平台分成比例 51%
        /// </summary>
        private static double _Join_Platform_Profit_Percent = 0.51;


        public static double Shop_Yufu_Percent
        {
            set
            {
                _Shop_Yufu_Percent = value;
            }
            get
            {
                return _Shop_Yufu_Percent;
            }
        }
        public static double Order_Partner_Profit_Percent
        {
            set
            {
                _Order_Partner_Profit_Percent = value;
            }
            get
            {
                return _Order_Partner_Profit_Percent;
            }
        }
        public static double Order_Shop_Profit_Percent
        {
            set
            {
                _Order_Shop_Profit_Percent = value;
            }
            get
            {
                return _Order_Shop_Profit_Percent;
            }
        }
        public static double Join_Partner_Profit_Percent
        {
            set
            {
                _Join_Partner_Profit_Percent = value;
            }
            get
            {
                return _Join_Partner_Profit_Percent;
            }
        }
        public static double Join_Platform_Profit_Percent
        {
            set
            {
                _Join_Platform_Profit_Percent = value;
            }
            get
            {
                return _Join_Platform_Profit_Percent;
            }
        }

        #endregion

    }
}
