using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class ExceptionController : Controller
    {
        public ActionResult Index(ExceptionFilter exception)
        {
            ViewBag.Ex = exception.ToString();
            return View();
        }
    }
}