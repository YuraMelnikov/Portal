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
            using (ReportKATEKEntities db = new ReportKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardDH
                    .AsNoTracking()
                    .Where(a => a.Year <= DateTime.Now.Year)
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
                    data[i].Rate = query[i].Rate;
                    data[i].SSM = query[i].SSMR;
                    data[i].SSW = query[i].SSW;
                    data[i].IK = query[i].IK;
                    data[i].PK = query[i].PK;
                    data[i].PI = query[i].PI;
                    data[i].Profit = query[i].Profit;
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
                    data[i].PSSW = (double)(query[i].ХССЗП / round);
                    data[i].FSSW = (double)((double)query[i].Издержки__ЗП_ПО_ / round);
                    data[i].RSSW = (double)(query[i].ОтклЗППО / round);
                    data[i].PPK = (double)(query[i].ХППК / round);
                    data[i].FPK = (double)((double)query[i].Издержки___по_кредиту / round);
                    data[i].RPK = (double)(query[i].ОтклППК / round);
                    data[i].PPI = (double)(query[i].ХПИ / round);
                    data[i].FPI = (double)((double)query[i].Постоянные_издержки / round);
                    data[i].RPI = (double)(query[i].ОтклПИ / round);
                    data[i].PIK = (double)(query[i].ХИК / round);
                    data[i].FIK = (double)((double)query[i].Коммерческие_издержки / round);
                    data[i].RIK = (double)(query[i].ОтклКИ / round);
                    data[i].PSSM = (double)(query[i].ХСС / round);
                    data[i].FSSM = (double)(query[i].ХССФакт / round);
                    data[i].RSSM = (double)(query[i].ОтклСС / round);
                    data[i].FS1 = (double)((double)query[i].Коммерческие_издержки_прочие / round);
                    data[i].FS2 = (double)((double)query[i].Условно_ПИ / round);
                    data[i].PFull = (double)(query[i].Плановые_издержки / round);
                    data[i].FFull = (double)(query[i].Фактические_издержки / round);
                    data[i].RFull = (double)(query[i].Откл_издержек / round);
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

        public JsonResult GetN()
        {
            using (ReportKATEKEntities db = new ReportKATEKEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var query = db.DashboardDN.AsNoTracking().OrderBy(a => a.month).ToList();
                int sizeArray = query.Count;
                GeneralN[] data = new GeneralN[sizeArray];
                for (int i = 0; i < sizeArray; i++)
                {
                    data[i] = new GeneralN();
                    data[i].Month = query[i].month;
                    data[i].Ns = Math.Round(query[i].sn, 2);
                    data[i].Nsv = Math.Round(query[i].svn, 2);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDSVN()
        {
            using (ReportKATEKEntities db = new ReportKATEKEntities())
            {
                int filtYear = DateTime.Now.Year - 1;
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var query = db.PercentSredniyVzveshenniyNOP.AsNoTracking().Where(a => a.year >= filtYear).ToList();
                int sizeArray = query.Count;
                GeneralDSVN[] data = new GeneralDSVN[sizeArray];
                for (int i = 0; i < sizeArray; i++)
                {
                    data[i] = new GeneralDSVN();
                    data[i].Month = query[i].Месяц;
                    data[i].Dsvn = Math.Round(query[i].Средний_взвешеный_НОП, 2);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetNCustomer()
        {
            using (ReportKATEKEntities db = new ReportKATEKEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var query = db.DashboardDPercent.AsNoTracking().OrderByDescending(a => a.Data).ToList();
                int sizeArray = query.Count;
                GeneralPercentCustomer[] data = new GeneralPercentCustomer[sizeArray];
                for(int i = 0; i < sizeArray; i++)
                {
                    data[i] = new GeneralPercentCustomer();
                    data[i].Customer = query[i].Customer;
                    data[i].Percent = Math.Round(query[i].Data, 2);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetNLast120()
        {
            using (ReportKATEKEntities db = new ReportKATEKEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var query = db.SVNLast120.AsNoTracking().OrderBy(a => a.month).ToList();
                int sizeArray = query.Count;
                GeneralN[] data = new GeneralN[sizeArray];
                for (int i = 0; i < sizeArray; i++)
                {
                    data[i] = new GeneralN();
                    data[i].Month = query[i].month;
                    data[i].Ns = Math.Round(query[i].data, 2);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}