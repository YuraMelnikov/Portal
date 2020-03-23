using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Areas.DashboardKO.Models;

namespace Wiki.Areas.DashboardD.Controllers
{
    public class DashboardDDController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string RenderUserMenu()
        {
            string login = "Войти";
            try
            {
                if (HttpContext.User.Identity.Name != "")
                    login = HttpContext.User.Identity.Name;
            }
            catch
            {
                login = "Войти";
            }
            return login;
        }

        public JsonResult GetGeneralD()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.CMKO_ThisHSS
                    .AsNoTracking()
                    .OrderBy(d => d.quartal)
                    .ToList();


                int maxCounterValue = query.Count();
                UserResultWithDevision[] data = new UserResultWithDevision[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UserResultWithDevision();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].quartal;
                    data[i].count = query[i].KBE;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}