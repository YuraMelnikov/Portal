using System.Web.Mvc;
using Wiki.Areas.DashboardBP.Models;

namespace Wiki.Areas.DashboardBP.Controllers
{
    public class BPController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult CreateBasicPlan()
        {
            NewBP bp = new NewBP();
            return View();
        }

        public string RenderUserMenu()
        {
            string login = "Войти";
            try
            {
                if (HttpContext.User.Identity.Name != "")
                    login = HttpContext.User.Identity.Name;
            }
            catch
            {
                login = "Войти";
            }
            return login;
        }
    }
}