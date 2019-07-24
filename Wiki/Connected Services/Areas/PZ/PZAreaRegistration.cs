using System.Web.Mvc;

namespace Wiki.Areas.PZ
{
    public class PZAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PZ";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PZ_default",
                "PZ/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}