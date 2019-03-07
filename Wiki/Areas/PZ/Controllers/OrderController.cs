using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using DataTables;

namespace Wiki.Areas.PZ.Controllers
{
    public class OrderController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Table()
        {
            var formData = HttpContext.Request.Form;
            var response = db.PZ_PlanZakaz.ToList();

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}