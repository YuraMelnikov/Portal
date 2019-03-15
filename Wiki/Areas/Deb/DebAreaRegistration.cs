using System.Web.Mvc;

namespace Wiki.Areas.Deb
{
    public class DebAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Deb";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Deb_default",
                "Deb/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}