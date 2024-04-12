using System;
using System.Collections.Specialized;
using System.Web;

namespace Pannet.Utility
{
    public static class CookieHelper
    {
        public static string WebSiteMainDomain = ConfigHelper.GetConfigString("WebSiteMainDomain");
        public static void Delete(string strCookieName)
        {
            HttpCookie objCookie = new HttpCookie(strCookieName.Trim());
            objCookie.Expires = DateTime.Now.AddYears(-5);
            objCookie.Value = "";
            objCookie.Domain = WebSiteMainDomain;
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        public static string Delete(string strCookieName, string strKeyName, int iExpires)
        {
            if (HttpContext.Current.Request.Cookies[strCookieName] == null)
            {
                return null;
            }
            HttpCookie objCookie = HttpContext.Current.Request.Cookies[strCookieName];
            objCookie.Values.Remove(strKeyName);
            //if (iExpires > 0)
            //{
            //    if (iExpires == 1)
            //    {
            //        objCookie.Expires = DateTime.MaxValue;
            //    }
            //    else
            //    {
            objCookie.Expires = DateTime.Now.AddSeconds((double)iExpires);
            //    }
            //}
            HttpContext.Current.Response.Cookies.Add(objCookie);
            return "success";
        }

        public static string Edit(string strCookieName, string strKeyName, string KeyValue, int iExpires)
        {
            if (HttpContext.Current.Request.Cookies[strCookieName] == null)
            {
                return null;
            }
            HttpCookie objCookie = HttpContext.Current.Request.Cookies[strCookieName];
            objCookie[strKeyName] = HttpContext.Current.Server.UrlEncode(KeyValue.Trim());
            if (iExpires > 0)
            {
                if (iExpires == 1)
                {
                    objCookie.Expires = DateTime.MaxValue;
                }
                else
                {
                    objCookie.Expires = DateTime.Now.AddSeconds((double)iExpires);
                }
            }
            HttpContext.Current.Response.Cookies.Add(objCookie);
            return "success";
        }

        public static string GetValue(string strCookieName)
        {
            if (HttpContext.Current.Request.Cookies[strCookieName] == null || string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[strCookieName].Value))
            {
                return null;
            }
            return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[strCookieName].Value);
        }

        public static string GetValue(string strCookieName, string strKeyName)
        {
            if (HttpContext.Current.Request.Cookies[strCookieName] == null || string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[strCookieName].Value))
            {
                return null;
            }
            string strObjValue = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[strCookieName].Value);
            string strKeyName2 = strKeyName + "=";
            if (strObjValue.IndexOf(strKeyName2) == -1)
            {
                return null;
            }
            return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[strCookieName][strKeyName]);
        }

        public static void SetValue(string strCookieName, string strValue)
        {
            SetValue(strCookieName, strValue, 1);
        }

        public static void SetValue(string strCookieName, NameValueCollection KeyValue, int iExpires)
        {
            HttpCookie objCookie = new HttpCookie(strCookieName.Trim());
            foreach (string key in KeyValue.AllKeys)
            {
                objCookie[key] = HttpContext.Current.Server.UrlEncode(KeyValue[key].Trim());
            }
            if (iExpires > 0)
            {
                if (iExpires == 1)
                {
                    objCookie.Expires = DateTime.MaxValue;
                }
                else
                {
                    objCookie.Expires = DateTime.Now.AddSeconds((double)iExpires);
                }
            }
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        public static void SetValue(string strCookieName, string strValue, int iExpires)
        {
            HttpCookie objCookie = new HttpCookie(strCookieName.Trim());
            objCookie.Value = HttpContext.Current.Server.UrlEncode(strValue.Trim());
            objCookie.Domain = WebSiteMainDomain;
            if (iExpires > 0)
            {
                if (iExpires == 1)
                {
                    objCookie.Expires = DateTime.MaxValue;
                }
                else
                {
                    objCookie.Expires = DateTime.Now.AddSeconds((double)iExpires);
                }
            }
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        public static void SetValue(string strCookieName, NameValueCollection KeyValue, string strDomain, int iExpires)
        {
            HttpCookie objCookie = new HttpCookie(strCookieName.Trim());
            foreach (string key in KeyValue.AllKeys)
            {
                objCookie[key] = HttpContext.Current.Server.UrlEncode(KeyValue[key].Trim());
            }
            objCookie.Domain = strDomain.Trim();
            if (iExpires > 0)
            {
                if (iExpires == 1)
                {
                    objCookie.Expires = DateTime.MaxValue;
                }
                else
                {
                    objCookie.Expires = DateTime.Now.AddSeconds((double)iExpires);
                }
            }
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        public static void SetValue(string strCookieName, string strValue, string strDomain, int iExpires)
        {
            HttpCookie objCookie = new HttpCookie(strCookieName.Trim());
            objCookie.Value = HttpContext.Current.Server.UrlEncode(strValue.Trim());
            objCookie.Domain = strDomain.Trim();
            if (iExpires > 0)
            {
                if (iExpires == 1)
                {
                    objCookie.Expires = DateTime.MaxValue;
                }
                else
                {
                    objCookie.Expires = DateTime.Now.AddSeconds((double)iExpires);
                }
            }
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }
    }

}