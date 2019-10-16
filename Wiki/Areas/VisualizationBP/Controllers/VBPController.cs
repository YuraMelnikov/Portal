using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Wiki.Areas.VisualizationBP.Controllers
{
    public class VBPController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSppedometrThisYear1Month(string id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                List<DashboardBP_HSSPOSmall> list = GetHSSSnall(Convert.ToInt32(id));
                var data = list.Select(dataList => new
                {
                    period = dataList.year + "." + dataList.numberMonth,
                    data = dataList.data / 1000
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        List<DashboardBP_HSSPOSmall> GetHSSSnall(int numberMonth)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                if (DateTime.Now.Month < 10)
                {
                    return db.DashboardBP_HSSPOSmall
                        .Where(d => d.numberMonth == numberMonth && d.year == DateTime.Now.Year - 1)
                        .ToList();
                }
                else
                {
                    return db.DashboardBP_HSSPOSmall
                        .Where(d => d.numberMonth == numberMonth && d.year == DateTime.Now.Year)
                        .ToList();
                }
            }
        }
    }
}