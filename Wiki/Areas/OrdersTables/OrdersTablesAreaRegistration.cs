using System.Web.Mvc;

namespace Wiki.Areas.OrdersTables
{
    public class OrdersTablesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OrdersTables";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OrdersTables_default",
                "OrdersTables/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}