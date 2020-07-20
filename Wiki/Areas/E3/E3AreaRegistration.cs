using System.Web.Mvc;

namespace Wiki.Areas.E3
{
    public class E3AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "E3";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "E3_default",
                "E3/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}