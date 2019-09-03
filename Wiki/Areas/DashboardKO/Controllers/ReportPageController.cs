using System.Web.Mvc;
using Wiki.Areas.DashboardBP.Models;
using System.Data.Entity;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Web.Script.Serialization;

namespace Wiki.Areas.DashboardKO.Controllers
{
    public class ReportPageController : Controller
    {
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        JavaScriptSerializer js = new JavaScriptSerializer();

        public ActionResult Index()
        {
            return View();
        }
    }
}