using System.Web.Mvc;
using System.Linq;
using System;
using System.Data.Entity;

namespace Wiki.Areas.VisualizationBP.Controllers
{
    public class VBPController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetHSSPlanToYear()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardHSSPlan.AsNoTracking().ToList();
                int[] data = new int[2];
                data[0] = (int)query[0].plan - (int)query[0].fact;
                data[1] = (int)query[0].fact;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}