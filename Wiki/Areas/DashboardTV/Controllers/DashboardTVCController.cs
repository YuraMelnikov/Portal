using System.Linq;
using System.Linq.Dynamic;
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
                var projectList = db.DashboardTV_DataForProjectPortfolio.GroupBy(d => d.orderNumber).OrderBy(d => d.Key).ToList();
                OrderForDashboardTV[] dataList = new OrderForDashboardTV[projectList.Count];
                for (int i = 0; i < projectList.Count; i++)
                {
                    dataList[i] = new OrderForDashboardTV();
                    dataList[i].Current = 0;

                    dataList[i].OrderNumber = projectList[i].Key;
                    string indexOrder = dataList[i].OrderNumber;
                    dataList[i].DataOtgruzkiBP = db.DashboardTV_DataForProjectPortfolio.First(d => d.orderNumber == indexOrder).dataOtgruzkiBP;
                    int countDeals = db.DashboardTV_DataForProjectPortfolio.Where(d => d.orderNumber == indexOrder).Count();
                    dataList[i].Deals = new DealsForDashboardTV[countDeals];
                }
                var portfolioList = db.DashboardTV_DataForProjectPortfolio.AsNoTracking().ToList();
                int j = 0;
                string orderNumberList = "";
                foreach (var dataInList in portfolioList.OrderBy(d => d.orderNumber))
                {
                    if(orderNumberList != dataInList.orderNumber)
                    {
                        j = 0;
                        orderNumberList = dataInList.orderNumber;
                    }
                    DealsForDashboardTV dealsForDashboardTV = new DealsForDashboardTV();
                    dealsForDashboardTV.TCPM = dataInList.tcpm;
                    dealsForDashboardTV.From = dataInList.from;
                    dealsForDashboardTV.To = dataInList.to;
                    dataList.First(d => d.OrderNumber == dataInList.orderNumber).Deals[j] = dealsForDashboardTV;
                    j++;
                }
                return Json(dataList, JsonRequestBehavior.AllowGet);
            }
        }
    }
}