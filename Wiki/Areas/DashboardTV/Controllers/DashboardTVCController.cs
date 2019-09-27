using Newtonsoft.Json;
using System;
using System.Data.Entity;
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
                    monthPlan,
                    glyphicon = GetGlyphicon(dataList.inThisMonthPercent),
                    glyphicon1 = GetGlyphicon(dataList.inThisMonthPercent)
                });
                return Json(new { data });
            }
        }

        private string GetGlyphicon(int percent)
        {
            if(percent >= 100)
                return "<span class=" + '\u0022' + "glyphicon glyphicon-arrow-up text-success" + '\u0022' + ">" + "</span>";
            else
                return "<span class=" + '\u0022' + "glyphicon glyphicon-arrow-down text-danger" + '\u0022' + ">" + "</span>";
        }

        public JsonResult GetProjectsPortfolio()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var projectList = db.DashboardTV_DataForProjectPortfolio
                    .GroupBy(d => d.orderNumber)
                    .OrderBy(d => d.Key)
                    .ToList();
                DateTime minDate = GetMinDate();
                DateTime maxDate = GetMaxDate();
                int countMonthDifferent = GetMinthDifferent(maxDate, minDate);
                OrderForDashboardTV[] dataList = new OrderForDashboardTV[projectList.Count + 1];
                dataList[0] = new OrderForDashboardTV();
                dataList[0].Current = 0;
                dataList[0].Color = "#910000";
                dataList[0].OrderNumber = "";
                dataList[0].DataOtgruzkiBP = DateTime.Now;
                dataList[0].Deals = new DealsForDashboardTV[countMonthDifferent];
                dataList[0].Milestone = false;
                int countForDeals = 0;
                for (DateTime i = minDate; i < maxDate; i = i.AddMonths(1))
                {
                    DealsForDashboardTV dealsForDashboardTV = new DealsForDashboardTV();
                    dealsForDashboardTV.TCPM = 0;
                    dealsForDashboardTV.From = new DateTime(i.Year, i.Month, 1);
                    dealsForDashboardTV.To = GetCorrectFinishDate(i);
                    dealsForDashboardTV.Milestone = false;
                    dealsForDashboardTV.Color = "#910000";
                    dataList[0].Deals[countForDeals] = dealsForDashboardTV;
                    countForDeals++;
                }
                for (int i = 1; i < projectList.Count + 1; i++)
                {
                    dataList[i] = new OrderForDashboardTV();
                    dataList[i].Current = 0;
                    dataList[i].Color = "#2b908f";
                    dataList[i].OrderNumber = projectList[i - 1].Key;
                    string indexOrder = dataList[i].OrderNumber;
                    dataList[i].DataOtgruzkiBP = db.DashboardTV_DataForProjectPortfolio.First(d => d.orderNumber == indexOrder).dataOtgruzkiBP;
                    int countDeals = db.DashboardTV_DataForProjectPortfolio.Where(d => d.orderNumber == indexOrder).Count() + 1;
                    dataList[i].Deals = new DealsForDashboardTV[countDeals];
                    dataList[i].Milestone = false;
                }
                var verificationList = db.PlanVerificationItems
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .ToList();
                foreach (var dataInVerifList in verificationList)
                {
                    foreach(var dataInOrderList in dataList)
                    {
                        try
                        {
                            if (dataInVerifList.PZ_PlanZakaz.PlanZakaz.ToString() == dataInOrderList.OrderNumber)
                            {
                                DealsForDashboardTV dealsForDashboardTV = new DealsForDashboardTV();
                                DateTime dateMilestone = GetDateMilestone(dataInVerifList);
                                dealsForDashboardTV.TCPM = 0;
                                dealsForDashboardTV.From = dateMilestone;
                                dealsForDashboardTV.To = dateMilestone;
                                dealsForDashboardTV.Milestone = true;
                                dealsForDashboardTV.Color = "#910000";
                                dataInOrderList.Deals[0] = dealsForDashboardTV;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                foreach(var dataInDataList in dataList)
                {
                    try
                    {
                        if (dataInDataList.Deals[0] == null)
                        {
                            DealsForDashboardTV dealsForDashboardTV = new DealsForDashboardTV();
                            DateTime dateMilestone = DateTime.Now.AddDays(60);
                            dealsForDashboardTV.TCPM = 0;
                            dealsForDashboardTV.From = dateMilestone;
                            dealsForDashboardTV.To = dateMilestone;
                            dealsForDashboardTV.Milestone = true;
                            dealsForDashboardTV.Color = "#910000";
                            dataInDataList.Deals[0] = dealsForDashboardTV;
                        }
                    }
                    catch
                    {

                    }
                }
                var portfolioList = db.DashboardTV_DataForProjectPortfolio.AsNoTracking().ToList();
                int j = 1;
                string orderNumberList = "";
                foreach (var dataInList in portfolioList.OrderBy(d => d.orderNumber))
                {
                    if(orderNumberList != dataInList.orderNumber)
                    {
                        j = 1;
                        orderNumberList = dataInList.orderNumber;
                    }
                    DealsForDashboardTV dealsForDashboardTV = new DealsForDashboardTV();
                    dealsForDashboardTV.TCPM = dataInList.tcpm;
                    dealsForDashboardTV.From = dataInList.from;
                    dealsForDashboardTV.To = dataInList.to;
                    dealsForDashboardTV.Milestone = false;
                    dealsForDashboardTV.Color = "#2b908f";
                    dataList.First(d => d.OrderNumber == dataInList.orderNumber).Deals[j] = dealsForDashboardTV;
                    j++;
                }
                return Json(dataList.OrderBy(d => d.DataOtgruzkiBP), JsonRequestBehavior.AllowGet);
            }
        }

        private DateTime GetCorrectFinishDate(DateTime dateTime)
        {
            try
            {
                return new DateTime(dateTime.Year, dateTime.Month, 31);
            }
            catch
            {
                try
                {
                    return new DateTime(dateTime.Year, dateTime.Month, 30);
                }
                catch
                {
                    try
                    {
                        return new DateTime(dateTime.Year, dateTime.Month, 29);
                    }
                    catch
                    {
                        return new DateTime(dateTime.Year, dateTime.Month, 28);
                    }
                }
            }

        }

        private int GetMinthDifferent(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }

        private DateTime GetMinDate()
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-90);

            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        private DateTime GetMaxDate()
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(120);

            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        private DateTime GetDateMilestone(PlanVerificationItems planVerificationItems)
        {
            if (planVerificationItems.planDate != null)
                return planVerificationItems.planDate.Value;
            else if (planVerificationItems.verificationDateInPrj != null)
                return planVerificationItems.verificationDateInPrj.Value;
            else
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.PZ_PlanZakaz.First(d => d.Id == planVerificationItems.id_PZ_PlanZakaz).dataOtgruzkiBP;
                }
            }

        }

        [HttpPost]
        public JsonResult GetTablePlanChack()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PlanVerificationItems
                    .AsNoTracking()
                    .Where(d => d.verificationDateInPrj > DateTime.Now && d.factDate == null && d.planDate != null)
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .OrderBy(d => d.planDate.Value)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    order = dataList.PZ_PlanZakaz.PlanZakaz,
                    customer = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    planDate = JsonConvert.SerializeObject(dataList.planDate, settings).Replace(@"""", ""),
                    factDate = JsonConvert.SerializeObject(dataList.verificationDateInPrj, settings).Replace(@"""", ""),
                    deviation = GetDay(dataList.planDate.Value, dataList.verificationDateInPrj.Value)

                });
                return Json(new { data });
            }
        }

        private int GetDay(DateTime d1, DateTime d2)
        {
            TimeSpan t = d1 - d2;
            return (int)t.TotalDays;
        }
    }
}