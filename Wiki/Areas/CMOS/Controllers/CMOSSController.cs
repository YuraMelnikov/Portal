using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wiki.Areas.CMOS.Controllers
{
    public class CMOSSController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}