using System.Web.Mvc;

namespace Wiki.Areas.VisualizationBP
{
    public class VisualizationBPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "VisualizationBP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "VisualizationBP_default",
                "VisualizationBP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}