using System;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.DashboardD.Models;

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
            const int round = 1000;
            using (ReportKATEKEntities db = new ReportKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardDH
                    .AsNoTracking()
                    .OrderBy(d => d.Month)
                    .ToList();
                int maxCounterValue = query.Count();
                GeneralID[] data = new GeneralID[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new GeneralID();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].Month = query[i].Month;
                    data[i].Year = query[i].Year;
                    data[i].Rate = query[i].Rate / round;
                    data[i].SSM = query[i].SSM / round;
                    data[i].SSW = query[i].SSW / round;
                    data[i].IK = query[i].IK / round;
                    data[i].PK = query[i].PK / round;
                    data[i].PI = query[i].PI / round;
                    data[i].Profit = query[i].Profit / round;
                    data[i].MonthNum = GetMonthNum(query[i].Month);
                    data[i].Quart = query[i].Year.ToString() + "." + ((data[i].MonthNum + 2) / 3);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        int GetMonthNum(string month)
        {
            try
            {
                return Convert.ToInt32(month.Substring(5, 2));
            }
            catch
            {
                return Convert.ToInt32(month.Substring(5, 1));
            }
        }

        public JsonResult GetPF()
        {
            int filtYear = DateTime.Now.Year - 3;
            const int round = 1000;
            using (ReportKATEKEntities db = new ReportKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PlanFactCosts.AsNoTracking().Where(d => d.year.Value > filtYear).OrderBy(d => d.Период).ToList();
                int maxCounterValue = query.Count();
                GeneralPlanFact[] data = new GeneralPlanFact[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new GeneralPlanFact();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].Month = query[i].Период;
                    data[i].Year = query[i].year.Value;
                    data[i].PSSW = (int)query[i].ХССЗП / round;
                    data[i].FSSW = (int)query[i].Издержки__ЗП_ПО_ / round;
                    data[i].RSSW = (int)query[i].ОтклЗППО / round;
                    data[i].PPK = (int)query[i].ХППК / round;
                    data[i].FPK = (int)query[i].Издержки___по_кредиту / round;
                    data[i].RPK = (int)query[i].ОтклППК / round;
                    data[i].PPI = (int)query[i].ХПИ / round;
                    data[i].FPI = (int)query[i].Постоянные_издержки / round;
                    data[i].RPI = (int)query[i].ОтклПИ / round;
                    data[i].PIK = (int)query[i].ХПИ / round;
                    data[i].FIK = (int)query[i].Коммерческие_издержки / round;
                    data[i].RIK = (int)query[i].ОтклПИ / round;
                    data[i].PSSM = (int)query[i].ХСС / round;
                    data[i].FSSM = (int)query[i].ХССФакт / round;
                    data[i].RSSM = (int)query[i].ОтклСС / round;
                    data[i].FS1 = (int)query[i].Коммерческие_издержки_прочие / round;
                    data[i].FS2 = (int)query[i].Коммерческие_издержки_Ш / round;
                    data[i].PFull = (int)query[i].Плановые_издержки / round;
                    data[i].FFull = (int)query[i].Фактические_издержки / round;
                    data[i].RFull = (int)query[i].Откл_издержек / round;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCustomerData()
        {
            const double round = 1000.0;
            using (ReportKATEKEntities db = new ReportKATEKEntities())
            {
                int sSSM = 0;
                int sProfit = 0;
                int sRate = 0;
                int sFSSM = 0;
                int cSSM = 0;
                int cProfit = 0;
                int cRate = 0;
                int cFSSM = 0;
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardDHCustomer.AsNoTracking().ToList();
                foreach (var dataInQuery in query)
                {
                    sSSM += dataInQuery.SSM;
                    sProfit += dataInQuery.Profit;
                    sRate += dataInQuery.Rate;
                    sFSSM += dataInQuery.FSSM;
                }
                int maxCounterValue = query.Count();
                GeneralCustomer[] data = new GeneralCustomer[maxCounterValue + 1];
                for (int i = 0; i < maxCounterValue + 1; i++)
                {
                    data[i] = new GeneralCustomer();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].Customer = query[i].Customer;

                    if (Convert.ToDouble(query[i].SSM) / Convert.ToDouble(sSSM) < 0.05)
                    {
                        cSSM += (int)(query[i].SSM / round);
                        data[i].Ssm = 0;
                    }
                    else
                    {
                        data[i].Ssm = (int)(query[i].SSM / round);
                    }
                    if (Convert.ToDouble(query[i].Profit) / Convert.ToDouble(sProfit) < 0.05)
                    {
                        cProfit += (int)(query[i].Profit / round);
                        data[i].Profit = 0;
                    }
                    else
                    {
                        data[i].Profit = (int)(query[i].Profit / round);
                    }
                    if (Convert.ToDouble(query[i].Rate) / Convert.ToDouble(sRate) < 0.05)
                    {
                        cRate += (int)(query[i].Rate / round);
                        data[i].Rate = 0;
                    }
                    else
                    {
                        data[i].Rate = (int)(query[i].Rate / round);
                    }
                    if (Convert.ToDouble(query[i].FSSM) / Convert.ToDouble(sFSSM) < 0.05)
                    {
                        cFSSM += (int)(query[i].FSSM / round);
                        data[i].Fssm = 0;
                    }
                    else
                    {
                        data[i].Fssm = (int)(query[i].FSSM / round);
                    }
                }
                GeneralCustomer generalCustomer1 = new GeneralCustomer
                {
                    Customer = "Прочие",
                    Fssm = cFSSM,
                    Profit = cProfit,
                    Rate = cRate,
                    Ssm = cSSM,
                    PercentFSSM = cFSSM / sFSSM * 100,
                    PercentProfit = cProfit / sProfit * 100,
                    PercentRate = cRate / sRate * 100,
                    PercentSSM = cSSM / sSSM * 100
                };
                data[maxCounterValue] = generalCustomer1;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}