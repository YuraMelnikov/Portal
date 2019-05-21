using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wiki.Areas.Reclamation.Models;
using System.Web.Mvc;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class TechnicalAdviceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetProtocols()
        {
            var data = new ProtocolTAListView().ProtocolTAViews;
            return Json(new { data });
        }
    }
}