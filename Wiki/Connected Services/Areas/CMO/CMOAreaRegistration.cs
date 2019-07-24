using System.Web.Mvc;

namespace Wiki.Areas.CMO
{
    public class CMOAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CMO";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CMO_default",
                "CMO/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}