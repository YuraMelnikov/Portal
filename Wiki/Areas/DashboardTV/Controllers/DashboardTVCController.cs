using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.DashboardTV.Models;

namespace Wiki.Areas.DashboardTV.Controllers
{
    public class DashboardTVCController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTablePlan()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int monthPlan = db.DashboardTV_MonthPlan.First().data;
                var query = db.DashboardTV_BasicPlanData.AsNoTracking().ToList();
                var data = query.Select(dataList => new
                {
                    dataList.inThisDay,
                    dataList.inThisDayPercent,
                    dataList.inThisMonth,
                    dataList.inThisMonthPercent,
                    monthPlan
                });
                return Json(new { data });
            }
        }

        public JsonResult GetProjectsPortfolio()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int countArray = 0;
                OrderForDashboardTV[] dataList = new OrderForDashboardTV[countArray];







                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}