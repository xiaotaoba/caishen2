using System.Web.Mvc;

namespace Pannet.Web.Areas.WeiXin
{
    public class MobileAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Mobile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Mobile_default",
                "Mobile/{controller}/{action}/{id}",
                new { controller = "Member", action = "Index",id = UrlParameter.Optional },
                 namespaces: new string[] { "Pannet.Web.Areas.Mobile.Controllers" }
            );
        }
    }
}