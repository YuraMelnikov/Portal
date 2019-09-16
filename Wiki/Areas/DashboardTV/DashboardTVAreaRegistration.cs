using System.Web.Mvc;

namespace Wiki.Areas.DashboardTV
{
    public class DashboardTVAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DashboardTV";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DashboardTV_default",
                "DashboardTV/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}