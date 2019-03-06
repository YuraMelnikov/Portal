using System.Web.Mvc;

namespace Wiki.Areas.Service.Controllers
{
    public class ServiceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}