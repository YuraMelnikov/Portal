using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace Wiki.Areas.VisualizationBP.Controllers
{
    public class VBPController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSppedometrThisYear1Month()
        {
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}