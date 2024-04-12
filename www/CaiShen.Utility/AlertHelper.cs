using System;
using System.Collections.Specialized;
using System.Web;
namespace Pannet.Utility
{
    public static class AlertHelper
    {
        /// <summary>
        /// 提示文本
        /// </summary>
        /// <param name="typeClass">success、info、warning、danger</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetAlertDiv(string typeClass, string text)
        {
            switch (typeClass)
            {
                case "success": return string.Format(" <div class=\"alert alert-success\" role=\"alert\"><i class=\"icon-ok\"></i>{0}</div>", text);
                case "info": return string.Format(" <div class=\"alert alert-info\" role=\"alert\"><i class=\"glyphicon glyphicon-exclamation-sign\"></i>{0}</div>", text);
                case "warning": return string.Format(" <div class=\"alert alert-warning\" role=\"alert\"><i class=\"glyphicon glyphicon-exclamation-sign\"></i>{0}</div>", text);
                case "danger": return string.Format(" <div class=\"alert alert-danger\" role=\"alert\"><i class=\"glyphicon glyphicon-exclamation-sign\"></i>{0}</div>", text);
                default: return string.Format(" <div class=\"alert alert-success\" role=\"alert\"><i class=\"glyphicon glyphicon-exclamation-sign\"></i>{0}</div>", text);
            }
        }
    }

}