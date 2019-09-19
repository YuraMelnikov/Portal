using System.Web.Mvc;

namespace Wiki.Areas.VerifPlan
{
    public class VerifPlanAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "VerifPlan";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "VerifPlan_default",
                "VerifPlan/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}