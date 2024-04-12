using System;
using System.Collections.Specialized;
using System.Web;
namespace Pannet.Utility
{
    public static class IPHelper
    {
        public static string GetIP()
        {
            HttpRequest request = HttpContext.Current.Request;
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                result = "0.0.0.0";
            }
            return result.Split(',')[0].Trim();
        }
    }

}