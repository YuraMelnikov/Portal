using System.Web.Mvc;

namespace Wiki.Areas.NotesOrders
{
    public class NotesOrdersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NotesOrders";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NotesOrders_default",
                "NotesOrders/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}