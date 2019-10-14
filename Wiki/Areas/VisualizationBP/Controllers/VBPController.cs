using System.Web.Mvc;
using System.Data.Entity;
using Wiki.Areas.DashboardBP.Models;
using System.Collections.Generic;
using System;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Linq;

namespace Wiki.Areas.VisualizationBP.Controllers
{
    public class VBPController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}