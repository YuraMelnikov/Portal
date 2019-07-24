using System.Web.Mvc;

namespace Wiki.Areas.ServiceReclamations
{
    public class ServiceReclamationsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ServiceReclamations";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ServiceReclamations_default",
                "ServiceReclamations/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}