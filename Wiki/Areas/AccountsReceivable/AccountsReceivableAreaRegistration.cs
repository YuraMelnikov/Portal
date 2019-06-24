using System.Web.Mvc;

namespace Wiki.Areas.AccountsReceivable
{
    public class AccountsReceivableAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AccountsReceivable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AccountsReceivable_default",
                "AccountsReceivable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}