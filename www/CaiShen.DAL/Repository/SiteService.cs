using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pannet.Models;
using Pannet.Utility;
using System.Web;
using System.Configuration;

namespace Pannet.DAL.Repository
{
    public class SiteService
    {
        private static UnitOfWork work = new UnitOfWork();

        public static string GetPriceWithUnit(decimal price)
        {
            return string.Format("￥{0}", price.ToString("0.00"));
        }
        public static string GetPrice(decimal price)
        {
            return string.Format("{0}", price.ToString("0.00"));
        }
        /// <summary>
        /// 价格保留2位小数，并四舍五入
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal ToDecimalPrice(decimal price)
        {
            return Math.Round(price, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 获取赠送积分数量
        /// </summary>
        /// <param name="amount">消费金额</param>
        /// <returns></returns>
        public static int GetScoreByAmount(decimal amount)
        {
            return Convert.ToInt32(Math.Floor(amount));
        }

        /// <summary>
        /// 包含域名完整路径
        /// </summary>
        /// <param name="filepath">绝对路径</param>
        /// <returns></returns>
        public static string GetImgUrl(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return "";
            }
            if (filepath.ToLower().Contains("http://"))
            {
                return filepath;
            }
            else if (filepath.Contains("goods"))//门店上传产品图片
            {
                return string.Format("{0}{1}", WebSiteConfig.WebSiteDomain.Trim('/'), filepath);
            }
            else if (filepath.Contains("shop"))//门店二维码图片
            {
                return string.Format("{0}{1}", WebSiteConfig.WebSiteDomain.Trim('/'), filepath);
            }
            else if (filepath.Contains("comment"))//评价图片
            {
                return string.Format("{0}{1}", WebSiteConfig.WebSiteDomain.Trim('/'), filepath);
            }
            else
            {
                return string.Format("{0}{1}", WebSiteConfig.ImgDomain.Trim('/'), filepath);
            }
        }

        /// <summary>
        ///店铺设置url转成完整url地址含http://
        /// </summary>
        /// <returns>0主站</returns>
        public static string ConvertToDomain(string shop_url)
        {
            return string.Format("http://{0}.{1}", shop_url, WebSiteConfig.WebSiteMainDomain);
        }
        /// <summary>
        /// 是否是主站D
        /// </summary>
        /// <returns></returns>
        public static bool IsMainSite()
        {
            string domain = HttpContext.Current.Request.Url.Host;
            string mainDomain = WebSiteConfig.WebSiteMainDomain;
            if (domain == mainDomain || domain == "www." + mainDomain)
            {
                return true;
            }
            return false;
        }


    }
}
