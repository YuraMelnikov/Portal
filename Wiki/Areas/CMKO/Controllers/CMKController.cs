using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System;
using Newtonsoft.Json;
using Wiki.Areas.CMKO.Types;
using Wiki.Areas.DashboardKO.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Wiki.Areas.CMKO
{
    public class CMKController : Controller
    {
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly JsonSerializerSettings settingsDNY = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly PortalKATEKEntities db = new PortalKATEKEntities();

        public JsonResult AddCalend(ProductionCalendar data)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            db.ProductionCalendar.Add(data);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddCategory(CMKO_TaxCatigories data)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            db.CMKO_TaxCatigories.Add(data);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOptimization(CMKO_Optimization optimization)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            optimization.id_AspNetUsersCreate = db.AspNetUsers.First(d => d.Email == login).Id;
            optimization.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            optimization.datetimeCreate = DateTime.Now;
            optimization.dateTimeUpdate = DateTime.Now;
            optimization.histiryEdit = "";
            db.CMKO_Optimization.Add(optimization);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddPeriod(CMKO_PeriodResult data)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            data.close = false;
            db.CMKO_PeriodResult.Add(data);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddTeach(CMKO_Teach data)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            data.id_AspNetUsersCreate = db.AspNetUsers.First(d => d.Email == login).Id;
            data.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            data.datetimeCreate = DateTime.Now;
            data.datetimeUpdate = DateTime.Now;
            data.historyEdit = "";
            if (data.description == null)
                data.description = "";
            db.CMKO_Teach.Add(data);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccuredFact()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                bool statusManager = GetStatusManagerUser();
                int counterUsers = 1;

                if(statusManager == true)
                {
                    counterUsers = db.CMKO_ThisAccrued.AsNoTracking().Count();
                }
                Types.UserResult[] data = new Types.UserResult[counterUsers];
                for (int i = 0; i < counterUsers; i++)
                {
                    data[i] = new Types.UserResult();
                }
                List<CMKO_ThisAccrued> accuredList = new List<CMKO_ThisAccrued>();
                if (statusManager == true)
                {
                    accuredList = db.CMKO_ThisAccrued
                        .AsNoTracking()
                        .Include(d => d.AspNetUsers)
                        .OrderBy(d => d.AspNetUsers.CiliricalName)
                        .ToList();
                }
                else
                {
                    accuredList = db.CMKO_ThisAccrued
                        .AsNoTracking()
                        .Include(d => d.AspNetUsers)
                        .Where(d => d.AspNetUsers.Email == login)
                        .OrderBy(d => d.AspNetUsers.CiliricalName)
                        .ToList();
                }
                for (int i = 0; i < counterUsers; i++)
                {
                    string tmp = accuredList[i].id_AspNetUsers;
                    CMKO_ThisIndicatorsUsers cMKO_ThisIndicatorsUsers = db.CMKO_ThisIndicatorsUsers.AsNoTracking().First(d => d.id_AspNetUsers == tmp);
                    data[i].CiliricName = accuredList[i].AspNetUsers.CiliricalName;
                    data[i].BonusReversed = (int)accuredList[i].bonusFact;
                    data[i].Accrued = (int)accuredList[i].accruedTotalFact;
                    data[i].Accruedg = (int)db.CMKO_ThisAccruedG.AsNoTracking().First(d => d.id_AspNetUsers == tmp).accruedTotalFact;
                    data[i].Manager = GetManagerAccuedFact(accuredList[i].id_AspNetUsers);
                    data[i].BonusQuality = (int)cMKO_ThisIndicatorsUsers.qualityBonus;
                    data[i].Speed = (int)cMKO_ThisIndicatorsUsers.speed1 + (int)cMKO_ThisIndicatorsUsers.speed2 + (int)cMKO_ThisIndicatorsUsers.speed3;
                    data[i].Optimization = (int)cMKO_ThisIndicatorsUsers.optimization;
                    data[i].Teach = (int)cMKO_ThisIndicatorsUsers.teach;
                    data[i].Tax = ((int)cMKO_ThisIndicatorsUsers.tax1 + (int)cMKO_ThisIndicatorsUsers.tax2 + (int)cMKO_ThisIndicatorsUsers.tax3) * -1;
                    data[i].Rate = ((int)cMKO_ThisIndicatorsUsers.rate1 + (int)cMKO_ThisIndicatorsUsers.rate2 + (int)cMKO_ThisIndicatorsUsers.rate3) * -1;
                    if (statusManager == false)
                    {
                        data[i].UserAccured = data[i].BonusReversed + data[i].Accrued + data[i].Accruedg + data[i].Manager + data[i].BonusQuality +
                            data[i].Speed + data[i].Optimization + data[i].Teach + data[i].Tax + data[i].Rate;
                    }
                    else
                    {
                        data[i].UserAccured = data[i].BonusReversed + data[i].Accrued + data[i].Accruedg + data[i].Manager + data[i].BonusQuality +
                            data[i].Speed + data[i].Optimization + data[i].Teach + data[i].Tax + data[i].Rate;
                    }
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        int GetManagerAccuedFact(string id_AspNetUsers)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int accued = 0;
                var bujet = db.CMKO_ThisDeductionsBonusFund.First();
                if (id_AspNetUsers == "4ebb8e70-7637-40b4-8c6e-3cd30a451d76")
                {
                    accued += (int)bujet.deductionsMKOMFact;
                    accued += (int)bujet.deductionsMKOEFact;
                    accued += (int)(GetParthNHKBEPlan("4ebb8e70-7637-40b4-8c6e-3cd30a451d76") * bujet.deductionsMEFact);
                }
                else if (id_AspNetUsers == "5ba3227f-ac84-4d65-ad87-632044217841")
                {
                    accued += (int)(GetParthNHKBEPlan("5ba3227f-ac84-4d65-ad87-632044217841") * bujet.deductionsMEFact);
                }
                else if (id_AspNetUsers == "8294e987-b175-4444-b300-8cb729448b38")
                {
                    accued += (int)bujet.deductionsMMFact;
                }
                return accued;
            }
        }

        double GetParthNHKBEFact(string id_AspNetUsers)
        {

            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                double parth = 1;
                var listUsersData = db.CMKO_ThisIndicatorsUsers
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .ToList();
                double pt1 = listUsersData.Where(d => d.AspNetUsers.Devision == 16).Sum(d => d.nhFact);
                double pt2 = listUsersData.Where(d => d.AspNetUsers.Devision == 3).Sum(d => d.nhFact);
                double sum12 = pt1 + pt2;
                if (id_AspNetUsers == "4ebb8e70-7637-40b4-8c6e-3cd30a451d76")
                {
                    parth = pt1 / sum12;
                }
                else
                {
                    parth = pt2 / sum12;
                }
                return parth;
            }
        }

        public JsonResult GetAccuredPlan()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                bool statusManager = GetStatusManagerUser();
                int counterUsers = 1;
                if (statusManager == true)
                {
                    counterUsers = db.CMKO_ThisAccrued.AsNoTracking().Count();
                }
                Types.UserResult[] data = new Types.UserResult[counterUsers];
                for (int i = 0; i < counterUsers; i++)
                {
                    data[i] = new Types.UserResult();
                }
                List<CMKO_ThisAccrued> accuredList = new List<CMKO_ThisAccrued>();
                if (statusManager == true)
                {
                    accuredList = db.CMKO_ThisAccrued
                        .AsNoTracking()
                        .Include(d => d.AspNetUsers)
                        .OrderBy(d => d.AspNetUsers.CiliricalName)
                        .ToList();
                }
                else
                {
                    accuredList = db.CMKO_ThisAccrued
                        .AsNoTracking()
                        .Include(d => d.AspNetUsers)
                        .Where(d => d.AspNetUsers.Email == login)
                        .ToList();
                }
                for (int i = 0; i < counterUsers; i++)
                {
                    string id_AspNetUsers = accuredList[i].id_AspNetUsers;
                    CMKO_ThisIndicatorsUsers cMKO_ThisIndicatorsUsers = db.CMKO_ThisIndicatorsUsers.First(d => d.id_AspNetUsers == id_AspNetUsers);
                    data[i].CiliricName = accuredList[i].AspNetUsers.CiliricalName;
                    data[i].BonusReversed = (int)accuredList[i].bonusPlan;
                    data[i].Accrued = (int)accuredList[i].accruedTotalPlan;
                    data[i].Accruedg = (int)db.CMKO_ThisAccruedG.AsNoTracking().First(d => d.id_AspNetUsers == id_AspNetUsers).accruedTotalPlan;
                    data[i].Manager = GetManagerAccuedPlan(accuredList[i].id_AspNetUsers);
                    data[i].BonusQuality = (int)cMKO_ThisIndicatorsUsers.qualityBonus;
                    data[i].Speed = (int)cMKO_ThisIndicatorsUsers.speed1 + (int)cMKO_ThisIndicatorsUsers.speed2 + (int)cMKO_ThisIndicatorsUsers.speed3;
                    data[i].Optimization = (int)cMKO_ThisIndicatorsUsers.optimization;
                    data[i].Teach = (int)cMKO_ThisIndicatorsUsers.teach;
                    data[i].Tax = ((int)cMKO_ThisIndicatorsUsers.tax1 + (int)cMKO_ThisIndicatorsUsers.tax2 + (int)cMKO_ThisIndicatorsUsers.tax3) * -1;
                    data[i].Rate = ((int)cMKO_ThisIndicatorsUsers.rate1 + (int)cMKO_ThisIndicatorsUsers.rate2 + (int)cMKO_ThisIndicatorsUsers.rate3) * -1;
                    if (statusManager == false)
                    {
                        data[i].UserAccured = data[i].BonusReversed + data[i].Accrued + data[i].Accruedg + data[i].Manager + data[i].BonusQuality +
                            data[i].Speed + data[i].Optimization + data[i].Teach + data[i].Tax + data[i].Rate;
                    }
                    else
                    {
                        data[i].UserAccured = data[i].BonusReversed + data[i].Accrued + data[i].Accruedg + data[i].Manager + data[i].BonusQuality +
                            data[i].Speed + data[i].Optimization + data[i].Teach + data[i].Tax + data[i].Rate;
                    }
                }


                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        int GetManagerAccuedPlan(string id_AspNetUsers)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int accued = 0;
                var bujet = db.CMKO_ThisDeductionsBonusFund.First();
                if (id_AspNetUsers == "4ebb8e70-7637-40b4-8c6e-3cd30a451d76")
                {
                    accued += (int)bujet.deductionsMKOMPlan;
                    accued += (int)bujet.deductionsMKOEPlan;
                    accued += (int)(GetParthNHKBEPlan("4ebb8e70-7637-40b4-8c6e-3cd30a451d76") * bujet.deductionsMEPlan);
                }
                else if (id_AspNetUsers == "5ba3227f-ac84-4d65-ad87-632044217841")
                {
                    accued += (int)(GetParthNHKBEPlan("5ba3227f-ac84-4d65-ad87-632044217841") * bujet.deductionsMEPlan);
                }
                else if (id_AspNetUsers == "8294e987-b175-4444-b300-8cb729448b38")
                {
                    accued += (int)bujet.deductionsMMPlan;
                }
                return accued;
            }
        }

        double GetParthNHKBEPlan(string id_AspNetUsers)
        {

            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                double parth = 1;
                var listUsersData = db.CMKO_ThisIndicatorsUsers
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .ToList();
                double pt1 = listUsersData.Where(d => d.AspNetUsers.Devision == 16).Sum(d => d.nhPlan);
                double pt2 = listUsersData.Where(d => d.AspNetUsers.Devision == 3).Sum(d => d.nhPlan);
                double sum12 = pt1 + pt2;
                if (id_AspNetUsers == "4ebb8e70-7637-40b4-8c6e-3cd30a451d76")
                {
                    parth = pt1 / sum12;
                }
                else
                {
                    parth = pt2 / sum12;
                }
                return parth;
            }
        }

        public JsonResult GetCalend(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.ProductionCalendar
                .AsNoTracking()
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idCalend = dataList.id,
                periodCalend = dataList.period,
                timeToOnePersonCalend = dataList.timeToOnePerson
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCalendList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.ProductionCalendar
                .AsNoTracking()
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkProductionCalend(login, dataList.id),
                dataList.period,
                dataList.timeToOnePerson
            });
            return Json(new { data });
        }

        public JsonResult GetCategory(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_TaxCatigories
                .AsNoTracking()
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idCategory = dataList.id,
                nameCategory = dataList.catigoriesName,
                selaryCategory = dataList.salary
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCategoryList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_TaxCatigories
                .AsNoTracking()
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkCategory(login, dataList.id),
                nameCategory = dataList.catigoriesName,
                selaryCategory = dataList.salary
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult GetCurencyList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CurencyBYN
                .AsNoTracking()
                .OrderByDescending(d => d.date)
                .Take(120)
                .ToList();
            var data = query.Select(dataList => new
            {
                date = JsonConvert.SerializeObject(dataList.date, settings).Replace(@"""", ""),
                dataList.USD
            });
            return Json(new { data });
        }

        public JsonResult GetGAccrued()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                List<CMKO_ThisAccruedG> fundData;
                if (GetStatusManagerUser() == true)
                {
                    fundData = db.CMKO_ThisAccruedG
                                        .AsNoTracking()
                                        .Include(d => d.AspNetUsers)
                                        .Where(d => d.accruedTotalPlan > 0)
                                        .OrderByDescending(d => d.accruedTotalPlan)
                                        .ToList();
                }
                else
                {
                    fundData = db.CMKO_ThisAccruedG
                                        .AsNoTracking()
                                        .Include(d => d.AspNetUsers)
                                        .Where(d => d.accruedTotalPlan > 0)
                                        .Where(d => d.AspNetUsers.Email == login)
                                        .OrderByDescending(d => d.accruedTotalPlan)
                                        .ToList();
                }
                int coluntList = fundData.Count;
                SummaryWageFundUser[] summaryWageFund = new SummaryWageFundUser[coluntList];
                for (int i = 0; i < coluntList; i++)
                {
                    summaryWageFund[i] = new SummaryWageFundUser
                    {
                        FullName = fundData[i].AspNetUsers.CiliricalName,
                        Plan = (int)fundData[i].accruedTotalPlan - (int)fundData[i].accruedTotalFact,
                        Fact = (int)fundData[i].accruedTotalFact
                    };
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetHSSKBE()
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

        public JsonResult GetHSSKBM()
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
                    data[i].count = query[i].KBM;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetHSSPO()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOHssPO
                    .AsNoTracking()
                    .OrderBy(d => d.quart)
                    .ToList();
                int maxCounterValue = query.Count();
                UserResultWithDevision[] data = new UserResultWithDevision[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UserResultWithDevision();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].quart;
                    data[i].count = (int)query[i].hss;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetManpowerFirstPeriod()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Count();
                if (query > 0)
                {
                    if (GetStatusManagerUser() == true)
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(login, JsonRequestBehavior.AllowGet);
                    }
                }

                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetManpowerSecondPeriod()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Count();
                if (query > 0)
                {
                    if(GetStatusManagerUser() == true)
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(login, JsonRequestBehavior.AllowGet);
                    }
                }

                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetManpowerThreePeriod()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Count();
                if (query > 0)
                {
                    if (GetStatusManagerUser() == true)
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(login, JsonRequestBehavior.AllowGet);
                    }
                }

                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetNhUsersThisQua()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {





                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisIndicatorsUsers
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .OrderByDescending(d => d.nhPlan)
                    .ToList();
                int coluntList = fundData.Count;
                SummaryWageFundUser[] summaryWageFund = new SummaryWageFundUser[coluntList];
                for (int i = 0; i < coluntList; i++)
                {
                    summaryWageFund[i] = new SummaryWageFundUser
                    {
                        FullName = fundData[i].AspNetUsers.CiliricalName,
                        Plan = (int)fundData[i].nhPlan - (int)fundData[i].nhFact,
                        Fact = (int)fundData[i].nhFact
                    };
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOptimization(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Optimization
                .AsNoTracking()
                .Where(d => d.id == id)
                .Include(d => d.CMKO_PeriodResult)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers2)
                .ToList();
            var data = query.Select(dataList => new
            {
                idOptimization = dataList.id,
                dateCreate = dataList.datetimeCreate.ToShortDateString() + " " + dataList.datetimeCreate.ToShortTimeString(),
                userCreate = dataList.AspNetUsers.CiliricalName,
                autor = dataList.id_AspNetUsersIdea,
                textData = dataList.description,
                dataList.CMKO_PeriodResult.period
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOptimizationList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Optimization
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.CMKO_PeriodResult)
                .Include(d => d.AspNetUsers2)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkOptimization(login, dataList.id),
                dateCreate = dataList.datetimeCreate.ToShortDateString() + " " + dataList.datetimeCreate.ToShortTimeString(),
                userCreate = dataList.AspNetUsers.CiliricalName,
                autor = dataList.AspNetUsers2.CiliricalName,
                textData = dataList.description,
                dataList.CMKO_PeriodResult.period
            });
            return Json(new { data });
        }

        public JsonResult GetOverflowsBujet()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisOverflowsBujet.AsNoTracking().ToList();
                SummaryWageFund[] summaryWageFund = new SummaryWageFund[3];
                for (int i = 0; i < 3; i++)
                {
                    summaryWageFund[i] = new SummaryWageFund();
                    if (i == 0)
                    {
                        summaryWageFund[i].DevisionId = 0;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.Sum(d => d.KBM) + fundData.Sum(d => d.KBE));
                    }
                    else if (i == 1)
                    {
                        summaryWageFund[i].DevisionId = 1;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.Sum(d => d.KBM));
                    }
                    else
                    {
                        summaryWageFund[i].DevisionId = 2;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.Sum(d => d.KBE));
                    }
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPeriodList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_PeriodResult
                .AsNoTracking()
                .ToList();
            var data = query.Select(dataList => new
            {
                dataList.period
            });
            return Json(new { data });
        }

        public JsonResult GetRemainingBonus()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisFinalBonus.AsNoTracking().First();
                SummaryWageFund[] summaryWageFund = new SummaryWageFund[3];
                for (int i = 0; i < 3; i++)
                {
                    summaryWageFund[i] = new SummaryWageFund();
                    if (i == 0)
                    {
                        summaryWageFund[i].DevisionId = 0;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.mPlan + fundData.ePlan) - Convert.ToInt32(fundData.mFact + fundData.eFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.mFact + fundData.eFact);
                    }
                    else if (i == 1)
                    {
                        summaryWageFund[i].DevisionId = 1;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.mPlan) - Convert.ToInt32(fundData.mFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.mFact);
                    }
                    else
                    {
                        summaryWageFund[i].DevisionId = 2;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.ePlan) - Convert.ToInt32(fundData.eFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.eFact);
                    }
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSpeedUser(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.DashboardKO_UsersMonthPlan
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.ProductionCalendar)
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idSpeedUser = dataList.id,
                userNameSpeedUser = dataList.AspNetUsers.CiliricalName,
                periodSpeedUser = dataList.ProductionCalendar.period,
                coefSpeedUser = dataList.k
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateDateReport()
        {
            string connString = @"Data Source=TSQLSERVER; initial catalog=PortalKATEK; Integrated Security=False;  Connection Timeout=3000; User Id=sas; Password=123qweASD; MultipleActiveResultSets=True; App=EntityFramework;";
            string queryString = "EXEC msdb.dbo.sp_start_job N'UpdateDashboard'";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                try
                {
                    command.ExecuteScalar();
                }
                catch
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                connection.Close();
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCoefManager(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_ThisCoefManager
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.CMKO_PeriodResult)
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idCMKO_ThisCoefManager = dataList.id,
                userCMKO_ThisCoefManager = dataList.AspNetUsers.CiliricalName,
                periodCMKO_ThisCoefManager = dataList.CMKO_PeriodResult.period,
                coefCMKO_ThisCoefManager = dataList.coef
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSpeedUserList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.DashboardKO_UsersMonthPlan
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.ProductionCalendar)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkSpeedUser(login, dataList.id),
                dataList.ProductionCalendar.period,
                user = dataList.AspNetUsers.CiliricalName,
                coef = dataList.k
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult GetManagUsersCoefList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_ThisCoefManager
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.CMKO_PeriodResult)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkCoefManager(login, dataList.id),
                dataList.CMKO_PeriodResult.period,
                user = dataList.AspNetUsers.CiliricalName,
                dataList.coef
            });
            return Json(new { data });
        }

        public JsonResult GetSummaryWageFundG()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisDeductionsBonusFund.AsNoTracking().First();
                SummaryWageFund[] summaryWageFund = new SummaryWageFund[3];
                for (int i = 0; i < 3; i++)
                {
                    summaryWageFund[i] = new SummaryWageFund();
                    if (i == 0)
                    {
                        summaryWageFund[i].DevisionId = 0;
                        summaryWageFund[i].Plan = (Convert.ToInt32(fundData.deductionsGMPlan) + Convert.ToInt32(fundData.deductionsGEPlan)) - (Convert.ToInt32(fundData.deductionsGMFact) + Convert.ToInt32(fundData.deductionsGEFact));
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsGMFact) + Convert.ToInt32(fundData.deductionsGEFact);
                    }
                    else if (i == 1)
                    {
                        summaryWageFund[i].DevisionId = 1;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.deductionsGMPlan) - Convert.ToInt32(fundData.deductionsGMFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsGMFact);
                    }
                    else
                    {
                        summaryWageFund[i].DevisionId = 2;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.deductionsGEPlan) - Convert.ToInt32(fundData.deductionsGEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsGEFact);
                    }
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUserName()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    return Json(db.AspNetUsers.First(d => d.Email == login).CiliricalName, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetSummaryWageFundManager()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisDeductionsBonusFund.AsNoTracking().First();
                SummaryWageFund[] summaryWageFund = new SummaryWageFund[3];
                for (int i = 0; i < 3; i++)
                {
                    summaryWageFund[i] = new SummaryWageFund();
                    if (i == 0)
                    {
                        summaryWageFund[i].DevisionId = 0;
                        summaryWageFund[i].Plan = (Convert.ToInt32(fundData.deductionsMKOMPlan) + Convert.ToInt32(fundData.deductionsMKOEPlan)) - (Convert.ToInt32(fundData.deductionsMKOMFact) + Convert.ToInt32(fundData.deductionsMKOEFact));
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsMKOMFact) + Convert.ToInt32(fundData.deductionsMKOEFact);
                    }
                    else if (i == 1)
                    {
                        summaryWageFund[i].DevisionId = 1;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.deductionsMMPlan) - Convert.ToInt32(fundData.deductionsMMFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsMMFact);
                    }
                    else
                    {
                        summaryWageFund[i].DevisionId = 2;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.deductionsMEPlan) - Convert.ToInt32(fundData.deductionsMEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsMEFact);
                    }
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSummaryWageFundWorker()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisWageFund.AsNoTracking().First();
                SummaryWageFund[] summaryWageFund = new SummaryWageFund[3];
                for (int i = 0; i < 3; i++)
                {
                    summaryWageFund[i] = new SummaryWageFund();
                    if (i == 0)
                    {
                        summaryWageFund[i].DevisionId = 0;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.wageFundOverflowsWorkerMPlan + fundData.wageFundOverflowsWorkerEPlan) - Convert.ToInt32(fundData.wageFundOverflowsWorkerMFact + fundData.wageFundOverflowsWorkerEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.wageFundOverflowsWorkerMFact + fundData.wageFundOverflowsWorkerEFact);
                    }
                    else if (i == 1)
                    {
                        summaryWageFund[i].DevisionId = 1;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.wageFundOverflowsWorkerMPlan) - Convert.ToInt32(fundData.wageFundOverflowsWorkerMFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.wageFundOverflowsWorkerMFact);
                    }
                    else
                    {
                        summaryWageFund[i].DevisionId = 2;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.wageFundOverflowsWorkerEPlan) - Convert.ToInt32(fundData.wageFundOverflowsWorkerEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.wageFundOverflowsWorkerEFact);
                    }
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTeach(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Teach
                .AsNoTracking()
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idTeach = dataList.id,
                teacherTeach = dataList.id_AspNetUsersTeacher,
                studentTeach = dataList.id_AspNetUsersTeach,
                periodTeach = dataList.id_CMKO_PeriodResult,
                costTeach = dataList.cost,
                descriptionTeach = dataList.description
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTeachList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Teach
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.AspNetUsers2)
                .Include(d => d.CMKO_PeriodResult)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkTeach(login, dataList.id),
                dateCreate = dataList.datetimeCreate.ToShortDateString() + " " + dataList.datetimeCreate.ToShortTimeString(),
                userCreate = dataList.AspNetUsers2.CiliricalName,
                teacher = dataList.AspNetUsers.CiliricalName,
                student = dataList.AspNetUsers1.CiliricalName,
                dataList.description,
                dataList.CMKO_PeriodResult.period
            });
            return Json(new { data });
        }

        public JsonResult GetTimesheet()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOTimesheet
                    .AsNoTracking()
                    .ToList();
                var queryUsers = db.DashboardKOTimesheet
                    .AsNoTracking()
                    .OrderByDescending(d => d.user)
                    .GroupBy(d => d.user)
                    .ToList();
                queryUsers = queryUsers.OrderByDescending(d => d.Key).ToList();
                var queryDate = db.DashboardKOTimesheet
                    .AsNoTracking()
                    .OrderBy(d => d.date)
                    .GroupBy(d => d.date)
                    .ToList();
                int usersCount = queryUsers.Count();
                int dateCount = queryDate.Count();
                int maxCounterValue = usersCount * dateCount + 1;
                TimesheetElamaent[] data = new TimesheetElamaent[maxCounterValue];
                DashboardKOTimesheet dashboardKOTimesheet = new DashboardKOTimesheet();
                data[0] = new TimesheetElamaent("", "", usersCount, dateCount, 0);
                for (int i = 1; i < maxCounterValue; i++)
                {
                    for (int j = 0; j < usersCount; j++)
                    {
                        for (int k = 0; k < dateCount; k++)
                        {
                            try
                            {
                                dashboardKOTimesheet = query.First(d => d.user == queryUsers[j].Key && d.date.Ticks == queryDate[k].Key.Ticks);
                            }
                            catch
                            {
                                dashboardKOTimesheet = new DashboardKOTimesheet();
                            }
                            data[i] = new TimesheetElamaent(queryUsers[j].Key, queryDate[k].Key.ToShortDateString(), j, k, dashboardKOTimesheet.work);
                            i++;
                        }
                    }
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUser(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.AspNetUsers
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Include(d => d.Devision1)
                .ToList();
            var data = query.Select(dataList => new
            {
                idUser = dataList.Id,
                ciliricNameUser = dataList.CiliricalName,
                devisionNameUser = dataList.Devision1.name,
                categoryUser = dataList.id_CMKO_TaxCatigories,
                dateToCMKO = JsonConvert.SerializeObject(dataList.dateToCMKO, settingsDNY).Replace(@"""", ""),
                taxUser = dataList.tax
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUsersList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.AspNetUsers
                .AsNoTracking()
                .Where(d => d.LockoutEnabled == true)
                .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                .Include(d => d.CMKO_TaxCatigories)
                .Include(d => d.Devision1)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkUser(login, dataList.Id),
                ciliricName = dataList.CiliricalName,
                devisionName = dataList.Devision1.name,
                category = dataList.CMKO_TaxCatigories.catigoriesName,
                dateToCMKO = JsonConvert.SerializeObject(dataList.dateToCMKO, settings).Replace(@"""", ""),
                dataList.tax
            });
            return Json(new { data });
        }

        public JsonResult GetWithheldToBonusFund()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisWithheldToBonusFund.AsNoTracking().First();
                SummaryWageFund[] summaryWageFund = new SummaryWageFund[3];
                for (int i = 0; i < 3; i++)
                {
                    summaryWageFund[i] = new SummaryWageFund();
                    if (i == 0)
                    {
                        summaryWageFund[i].DevisionId = 0;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.reclamationMPlan + fundData.reclamationEPlan) - Convert.ToInt32(fundData.reclamationMFact + fundData.reclamationEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.reclamationMFact + fundData.reclamationEFact);
                    }
                    else if (i == 1)
                    {
                        summaryWageFund[i].DevisionId = 1;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.reclamationMPlan) - Convert.ToInt32(fundData.reclamationMFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.reclamationMFact);
                    }
                    else
                    {
                        summaryWageFund[i].DevisionId = 2;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.reclamationEPlan) - Convert.ToInt32(fundData.reclamationEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.reclamationEFact);
                    }
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            string login = HttpContext.User.Identity.Name;
            DateTime dateForFiltPZ = DateTime.Now.AddDays(-90);
            ViewBag.periodTeach = new SelectList(db.CMKO_PeriodResult
                                                       .Where(d => d.close == false)
                                                       .OrderByDescending(d => d.period), "period", "period");
            ViewBag.categoryUser = new SelectList(db.CMKO_TaxCatigories.OrderBy(d => d.catigoriesName), "id", "catigoriesName");
            ViewBag.period = new SelectList(db.CMKO_PeriodResult.Where(d => d.close == false).OrderByDescending(x => x.period), "period", "period");
            if (login == "myi@katek.by")
            {
                 ViewBag.autor = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO != null)
                    .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO == null)
                    .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");

            }
            else if (login == "Kuchynski@katek.by")
            {
                ViewBag.autor = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO != null)
                    .Where(d => d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO == null)
                    .Where(d => d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
            }
            else if (login == "nrf@katek.by")
            {
                ViewBag.autor = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.Devision == 15)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO != null)
                    .Where(d => d.Devision == 15)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO == null)
                    .Where(d => d.Devision == 15)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
            }
            else if (login == "fvs@katek.by")
            {
                ViewBag.autor = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.Devision == 3)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO != null)
                    .Where(d => d.Devision == 3)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO == null)
                    .Where(d => d.Devision == 3)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
            }
            else 
            {
                ViewBag.autor = new SelectList(db.AspNetUsers.Where(d => d.Id == ""), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers.Where(d => d.Id == ""), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers.Where(d => d.Id == ""), "Id", "CiliricalName");
                ViewBag.periodTeach = new SelectList(db.CMKO_PeriodResult.Where(d => d.period == ""), "period", "period");
                ViewBag.categoryUser = new SelectList(db.CMKO_TaxCatigories.Where(d => d.id == 0), "id", "catigoriesName");
                ViewBag.period = new SelectList(db.CMKO_PeriodResult.Where(d => d.period == ""), "period", "period");
            }
            int devision = 0;
            try
            {
                devision = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            }
            catch
            {

            }
            if (GetStatusManagerUser() == true)
            {
                ViewBag.LeavelUser = 2;
            }
            else if (devision == 3 || devision == 15 || devision == 16 || login == "Rusak@katek.by")
            {
                ViewBag.LeavelUser = 1;
            }
            else
            {
                ViewBag.LeavelUser = 0;
            }
            ViewBag.PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > dateForFiltPZ).OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_DevisionReclamation = new SelectList(db.Devision.OrderBy(d => d.name), "id", "name");
            ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16 || d.Devision == 15)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            ViewBag.id_Reclamation_CountErrorFirst = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_PF = new SelectList(db.PF.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");

            return View();
        }

        bool GetStatusManagerUser()
        {
            string login = HttpContext.User.Identity.Name;
            if (login == "Kuchynski@katek.by" || login == "bav@katek.by" ||
                login == "fvs@katek.by" || login == "myi@katek.by" ||
                login == "laa@katek.by" || login == "nrf@katek.by" || login == "Rusak@katek.by")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public JsonResult RemoveOptimization(CMKO_Optimization optimization)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Optimization updateOptimization = db.CMKO_Optimization.First(d => d.id == optimization.id);
            db.Entry(updateOptimization).State = EntityState.Deleted;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveTeach(CMKO_Teach postData)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Teach updateData = db.CMKO_Teach.First(d => d.id == postData.id);
            db.Entry(updateData).State = EntityState.Deleted;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCalend(ProductionCalendar postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            ProductionCalendar updateData = db.ProductionCalendar.First(d => d.id == postData.id);
            if (updateData.timeToOnePerson != postData.timeToOnePerson)
                updateData.timeToOnePerson = postData.timeToOnePerson;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCategory(CMKO_TaxCatigories postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_TaxCatigories updateData = db.CMKO_TaxCatigories.First(d => d.id == postData.id);
            if (updateData.catigoriesName != postData.catigoriesName)
                updateData.catigoriesName = postData.catigoriesName;
            if (updateData.salary != postData.salary)
                updateData.salary = postData.salary;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateOptimization(CMKO_Optimization optimization)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Optimization updateOptimization = db.CMKO_Optimization.First(d => d.id == optimization.id);
            updateOptimization.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            updateOptimization.dateTimeUpdate = DateTime.Now;
            if (updateOptimization.description != optimization.description)
                updateOptimization.description = optimization.description;
            if (updateOptimization.id_AspNetUsersIdea != optimization.id_AspNetUsersIdea)
                updateOptimization.id_AspNetUsersIdea = optimization.id_AspNetUsersIdea;
            if (updateOptimization.id_CMKO_PeriodResult != optimization.id_CMKO_PeriodResult)
                updateOptimization.id_CMKO_PeriodResult = optimization.id_CMKO_PeriodResult;
            db.Entry(updateOptimization).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSpeedUser(DashboardKO_UsersMonthPlan postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            DashboardKO_UsersMonthPlan updateData = db.DashboardKO_UsersMonthPlan.First(d => d.id == postData.id);
            if (updateData.k != postData.k)
                updateData.k = postData.k;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatCoefManager(CMKO_ThisCoefManager postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_ThisCoefManager updateData = db.CMKO_ThisCoefManager.First(d => d.id == postData.id);
            if (updateData.coef != postData.coef)
                updateData.coef = postData.coef;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTeach(CMKO_Teach postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Teach updateData = db.CMKO_Teach.First(d => d.id == postData.id);
            updateData.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            updateData.datetimeUpdate = DateTime.Now;
            if (updateData.description != postData.description && postData.description != null)
                updateData.description = postData.description;
            if (updateData.id_AspNetUsersTeach != postData.id_AspNetUsersTeach)
                updateData.id_AspNetUsersTeach = postData.id_AspNetUsersTeach;
            if (updateData.id_AspNetUsersTeacher != postData.id_AspNetUsersTeacher)
                updateData.id_AspNetUsersTeacher = postData.id_AspNetUsersTeacher;
            if (updateData.cost != postData.cost)
                updateData.cost = postData.cost;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateUser(AspNetUsers postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            AspNetUsers updateData = db.AspNetUsers.First(d => d.Id == postData.Id);
            if (updateData.id_CMKO_TaxCatigories != postData.id_CMKO_TaxCatigories)
                updateData.id_CMKO_TaxCatigories = postData.id_CMKO_TaxCatigories;
            if (updateData.dateToCMKO != postData.dateToCMKO)
                updateData.dateToCMKO = postData.dateToCMKO;
            if (updateData.tax != postData.tax)
                updateData.tax = postData.tax;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        string GetEditLinkCategory(string login, int id)
        {
            if (login == "myi@katek.by" || login == "Kuchynski@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetCategory('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        string GetEditLinkOptimization(string login, int id)
        {
            if (login == "myi@katek.by" || login == "Kuchynski@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOptimization('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        string GetEditLinkProductionCalend(string login, int id)
        {
            if (login == "myi@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetCalend('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        string GetEditLinkSpeedUser(string login, int id)
        {
            if (login == "myi@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetSpeedUser('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        string GetEditLinkCoefManager(string login, int id)
        {
            if (login == "myi@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by" || login == "Kuchynski@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetCoefManager('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        string GetEditLinkTeach(string login, int id)
        {
            if (login == "myi@katek.by" || login == "Kuchynski@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetTeach('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }
        
        string GetEditLinkUser(string login, string id)
        {
            if (login == "myi@katek.by" || login == "Kuchynski@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetUser('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        public JsonResult GetSalaryAndRateWorkers()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisIndicatorsUsers
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .OrderBy(d => d.AspNetUsers.CiliricalName)
                    .ToList();
                int coluntList = fundData.Count;
                SummaryWageFundUser[] summaryWageFund = new SummaryWageFundUser[coluntList];
                for (int i = 0; i < coluntList; i++)
                {
                    summaryWageFund[i] = new SummaryWageFundUser
                    {
                        FullName = fundData[i].AspNetUsers.CiliricalName,
                        Plan = (int)(fundData[i].rate1 + fundData[i].rate2 + fundData[i].rate3),
                        Fact = (int)(fundData[i].tax1 + fundData[i].tax2 + fundData[i].tax3)
                    };
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCoefWorker()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisIndicatorsUsers
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .OrderBy(d => d.AspNetUsers.CiliricalName)
                    .ToList();
                int coluntList = fundData.Count;
                CoefUserView[] summaryWageFund = new CoefUserView[coluntList];
                for (int i = 0; i < coluntList; i++)
                {
                    summaryWageFund[i] = new CoefUserView(fundData[i].AspNetUsers.CiliricalName, Math.Round(fundData[i].coefError, 3));
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult GetCoefWorkerG()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisIndicatorsUsers
                    .AsNoTracking()
                    .Where(d => d.nhGFact > 0)
                    .Include(d => d.AspNetUsers)
                    .OrderBy(d => d.AspNetUsers.CiliricalName)
                    .ToList();
                int coluntList = fundData.Count;
                CoefUserView[] summaryWageFund = new CoefUserView[coluntList];
                for (int i = 0; i < coluntList; i++)
                {
                    summaryWageFund[i] = new CoefUserView(fundData[i].AspNetUsers.CiliricalName, Math.Round(fundData[i].coefErrorG, 3));
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTextNamePeriod()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string[] periodNames = new string[3];
                try
                {
                    periodNames[0] = db.DashboardKOMP1.First().period;
                }
                catch
                {

                }
                try
                {
                    periodNames[1] = db.DashboardKOMP2.First().period;
                }
                catch
                {

                }
                try
                {
                    periodNames[2] = db.DashboardKOMP3.First().period;
                }
                catch
                {

                }

                return Json(periodNames, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRamarksUsersList()
        {
            bool filt = GetStatusManagerUser();
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            List<CMKO_RemarksList> query;
            if(filt == true)
            {
                query = db.CMKO_RemarksList
                            .Include(d => d.AspNetUsers)
                            .Include(d => d.Reclamation)
                            .Include(d => d.Reclamation_CountError)
                            .ToList();
            }
            else
            {
                query = db.CMKO_RemarksList
                            .Include(d => d.AspNetUsers)
                            .Include(d => d.Reclamation)
                            .Include(d => d.Reclamation_CountError)
                            .Where(d => d.AspNetUsers.Email == login)
                            .ToList();
            }
            var data = query.Select(dataList => new
            {
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + dataList.id_Reclamation + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                idRemark = dataList.id_Reclamation,
                textData = dataList.Reclamation.text,
                dataList.Reclamation_CountError.count,
                user = dataList.AspNetUsers.CiliricalName
            });
            return Json(new { data });
        }

        public JsonResult GetRamarksGUsersList()
        {
            bool filt = GetStatusManagerUser();
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            List<CMKO_RemarksListG> query;
            if (filt == true)
            {
                query = db.CMKO_RemarksListG
                            .Include(d => d.AspNetUsers)
                            .Include(d => d.Reclamation)
                            .Include(d => d.Reclamation_CountError)
                            .ToList();
            }
            else
            {
                query = db.CMKO_RemarksListG
                            .Include(d => d.AspNetUsers)
                            .Include(d => d.Reclamation)
                            .Include(d => d.Reclamation_CountError)
                            .Where(d => d.AspNetUsers.Email == login)
                            .ToList();
            }
            var data = query.Select(dataList => new
            {
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + dataList.id_Reclamation + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                idRemark = dataList.id_Reclamation,
                textData = dataList.Reclamation.text,
                dataList.Reclamation_CountError.count,
                user = dataList.AspNetUsers.CiliricalName
            });
            return Json(new { data });
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

        public JsonResult GetUsersMMP1_1()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Васюхневич Илья")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_10()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Жук Марина Владимировна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_11()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Климович Ксения Сергеевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_12()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Кучинский Андрей")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_13()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Тимашкова Юлия Сергеевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_14()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Тиханский Максим Васильевич")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_15()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Филончик Валентина Сергеевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_2()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Волкова Алёна Александровна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_3()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Глебик Оксана Анатольевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_4()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Кальчинский Александр Владимирович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_5()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Маляревич Павел Анатольевич")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_6()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Носик Роман Федорович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_7()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Фейгина Анастасия Аркадьевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_8()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Добыш Константин Викторович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_9()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Жибуль Дмитрий Олегович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_16()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Рачкевич Виталий Игоревич")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_16()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Рачкевич Виталий Игоревич")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_16()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Рачкевич Виталий Игоревич")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_1()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Васюхневич Илья")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_10()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Жук Марина Владимировна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_11()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Климович Ксения Сергеевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_12()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Кучинский Андрей")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_13()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Тимашкова Юлия Сергеевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_14()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Тиханский Максим Васильевич")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_15()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Филончик Валентина Сергеевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_2()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Волкова Алёна Александровна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_3()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Глебик Оксана Анатольевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_4()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Кальчинский Александр Владимирович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_5()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Маляревич Павел Анатольевич")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_6()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Носик Роман Федорович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_7()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Фейгина Анастасия Аркадьевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_8()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Добыш Константин Викторович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_9()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Жибуль Дмитрий Олегович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_1()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Васюхневич Илья")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_10()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Жук Марина Владимировна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_11()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Климович Ксения Сергеевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_12()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Кучинский Андрей")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_13()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Тимашкова Юлия Сергеевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_14()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Тиханский Максим Васильевич")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_15()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Филончик Валентина Сергеевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_2()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Волкова Алёна Александровна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_3()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Глебик Оксана Анатольевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_4()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Кальчинский Александр Владимирович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_5()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Маляревич Павел Анатольевич")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_6()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Носик Роман Федорович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_7()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Фейгина Анастасия Аркадьевна")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_8()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Добыш Константин Викторович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_9()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Жибуль Дмитрий Олегович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP1_17()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP1
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Стрельченок Евгений Александрович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP2_17()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP2
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Стрельченок Евгений Александрович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersMMP3_17()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOMP3
                    .AsNoTracking()
                    .Where(d => d.ciliricalName == "Стрельченок Евгений Александрович")
                    .OrderByDescending(d => d.ciliricalName)
                    .ToList();
                int maxCounterValue = query.Count();
                UsersKOPlamMonth[] data = new UsersKOPlamMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UsersKOPlamMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].period = query[i].period;
                    data[i].ciliricalName = query[i].ciliricalName;
                    data[i].plan = (int)query[i].plan;
                    data[i].plan10 = (int)query[i].plan10;
                    data[i].plan20 = (int)query[i].plan20;
                    data[i].plan30 = (int)query[i].plan30;
                    data[i].normHoure = (int)query[i].normHoure;
                    data[i].normHoureFact = (int)query[i].normHoureFact;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetUsersQuartResultTable(int id)
        {
            string period = "2019.4";
            if (id < 2)
                period = "2019.4";
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            List<CMKO_SummaryResultToMonth> query;
            query = db.CMKO_SummaryResultToMonth
                        .Include(d => d.AspNetUsers)
                        .Where(d => d.id_CMKO_PeriodResult == period)
                        .ToList();
            var data = query.Select(dataList => new
            {
                dataList.AspNetUsers.CiliricalName,
                dataList.coefManager,
                dataList.coefError,
                dataList.coefErrorG,
                dataList.nh,
                dataList.nhG
            });
            return Json(new { data });
        }

        public JsonResult GetUserQuartResultTable(int id)
        {
            string period = "2019.4";
            if (id < 2)
                period = "2019.4";
            bool filt = GetStatusManagerUser();
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            List<CMKO_SummaryResultToMonth> query;
            if (filt == true)
            {
                query = db.CMKO_SummaryResultToMonth
                            .Include(d => d.AspNetUsers)
                            .Where(d => d.id_CMKO_PeriodResult == period)
                            .ToList();
            }
            else
            {
                query = db.CMKO_SummaryResultToMonth
                            .Include(d => d.AspNetUsers)
                            .Where(d => d.id_CMKO_PeriodResult == period)
                            .Where(d => d.AspNetUsers.Email == login)
                            .ToList();
            }
            var data = query.Select(dataList => new
            {
                dataList.AspNetUsers.CiliricalName,
                dataList.coefManager,
                dataList.coefError,
                dataList.coefErrorG,
                dataList.nh,
                dataList.nhG,
                dataList.ordersAccrue,
                dataList.remainingBonusAccrue,
                dataList.gAccrue,
                dataList.managerAccrue,
                dataList.teachAccrue,
                dataList.optimizationAccrue,
                dataList.timeUpAccrue,
                dataList.qualityAccrue,
                dataList.rate,
                dataList.tax,
                dataList.result
            });
            return Json(new { data });
        }
    }
}