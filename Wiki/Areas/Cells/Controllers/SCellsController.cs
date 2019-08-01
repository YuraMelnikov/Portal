using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wiki.Areas.Cells.Controllers
{
    public class SCellsController : Controller
    {
        public ActionResult Index()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {

            }
            return View();
        }
    }
}