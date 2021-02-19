using System.Web.Mvc;

namespace Wiki.Areas.CrossTable
{
    public class CrossTableAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CrossTable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CrossTable_default",
                "CrossTable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}