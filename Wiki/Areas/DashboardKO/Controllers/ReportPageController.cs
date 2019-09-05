using System.Web.Mvc;
using System.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Wiki.Areas.DashboardKO.Controllers
{
    public class ReportPageController : Controller
    {
        //readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        //JavaScriptSerializer js = new JavaScriptSerializer();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUsersQuaResult()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOQuaHumen
                    .AsNoTracking()
                    .OrderByDescending(d => d.normHoure)
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResult[] data = new Models.UserResult[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResult();
                }
                for(int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].user;
                    data[i].count = query[i].normHoure;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersM1()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOM1
                    .AsNoTracking()
                    .OrderByDescending(d => d.normHoure)
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResultMonth[] data = new Models.UserResultMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResultMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].user;
                    data[i].count = query[i].normHoure;
                    data[i].month = query[i].month;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersM2()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOM2
                    .AsNoTracking()
                    .OrderByDescending(d => d.normHoure)
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResultMonth[] data = new Models.UserResultMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResultMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].user;
                    data[i].count = query[i].normHoure;
                    data[i].month = query[i].month;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUsersM3()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOM3
                    .AsNoTracking()
                    .OrderByDescending(d => d.normHoure)
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResultMonth[] data = new Models.UserResultMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResultMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].user;
                    data[i].count = query[i].normHoure;
                    data[i].month = query[i].month;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDevisionQuaResult()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKO_Quartal
                    .AsNoTracking()
                    .GroupBy(d => d.devision).Select(g => new { Dev = g.Key, Total = g.Sum(x => x.normHoure)})
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResult[] data = new Models.UserResult[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResult();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].Dev;
                    data[i].count = (int)query[i].Total;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDevisionM1Result()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOM1
                    .AsNoTracking()
                    .GroupBy(d => d.devision).Select(g => new { Dev = g.Key, Total = g.Sum(x => x.normHoure), MonthName = g.FirstOrDefault().month })
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResultMonth[] data = new Models.UserResultMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResultMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].Dev;
                    data[i].count = (int)query[i].Total;
                    data[i].month = query[i].MonthName;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDevisionsM2Result()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOM2
                    .AsNoTracking()
                    .GroupBy(d => d.devision).Select(g => new { Dev = g.Key, Total = g.Sum(x => x.normHoure), MonthName = g.FirstOrDefault().month })
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResultMonth[] data = new Models.UserResultMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResultMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].Dev;
                    data[i].count = (int)query[i].Total;
                    data[i].month = query[i].MonthName;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDevisionsM3Result()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOM3
                    .AsNoTracking()
                    .GroupBy(d => d.devision).Select(g => new { Dev = g.Key, Total = g.Sum(x => x.normHoure), MonthName = g.FirstOrDefault().month })
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResultMonth[] data = new Models.UserResultMonth[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResultMonth();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].Dev;
                    data[i].count = (int)query[i].Total;
                    data[i].month = query[i].MonthName;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemainingWorkAll()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKORemainingWorkAll
                    .AsNoTracking()
                    .OrderByDescending(d => d.remainingWork)
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResultWithDevision[] data = new Models.UserResultWithDevision[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResultWithDevision();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].user;
                    data[i].count = (int)query[i].remainingWork;
                    data[i].devision = query[i].devision;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemainingWork()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKORemainingWork
                    .AsNoTracking()
                    .OrderByDescending(d => d.remainingWork)
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResultWithDevision[] data = new Models.UserResultWithDevision[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResultWithDevision();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].user;
                    data[i].count = (int)query[i].remainingWork;
                    data[i].devision = query[i].devision;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemainingDevisionWorkAll()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKORemainingWorkAll
                    .AsNoTracking()
                    .GroupBy(d => d.devision).Select(g => new { Dev = g.Key, Total = g.Sum(x => x.remainingWork) })
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResult[] data = new Models.UserResult[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResult();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].Dev;
                    data[i].count = (int)query[i].Total;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemainingDevisionWork()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKORemainingWork
                    .AsNoTracking()
                    .GroupBy(d => d.devision).Select(g => new { Dev = g.Key, Total = g.Sum(x => x.remainingWork) })
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResult[] data = new Models.UserResult[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new Models.UserResult();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].Dev;
                    data[i].count = (int)query[i].Total;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}