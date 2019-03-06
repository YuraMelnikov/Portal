using System.Web.Mvc;

namespace Wiki.Areas.Service.Controllers
{
    public class ServiceAdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}