using System.Web.Mvc;
using System.Linq;
using System;
using System.Data.Entity;
using Newtonsoft.Json;
using Wiki.Models;

namespace Wiki.Areas.VisualizationBP.Controllers
{
    public class VBPController : Controller
    {
        JsonSerializerSettings shortDateString = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetHSSPlanToYear()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardHSSPlan.AsNoTracking().ToList();
                int[] data = new int[2];
                data[0] = ((int)query[0].plan - (int)query[0].fact) / 1000;
                data[1] = ((int)query[0].fact) / 1000;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRatePlanToYear()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardRatePlan.AsNoTracking().ToList();
                int[] data = new int[2];
                data[0] = ((int)query[0].plan - (int)query[0].fact) / 1000;
                data[1] = ((int)query[0].fact) / 1000;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemainingHSS()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardRemaining.AsNoTracking().ToList();
                int[] data = new int[2];
                data[0] = ((int)query[0].fact) / 1000;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTaskThisDayTable()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                DateTime thisDay = DateTime.Today;
                var query = db.DashboardBP_ProjectTasks
                    .AsNoTracking()
                    .Include(d => d.DashboardBP_ProjectList.PZ_PlanZakaz)
                    .Include(d => d.WBS)
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.percentComplited != 100)
                    .Where(d => d.basicStart == thisDay || d.basicFinish == thisDay)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    orderNumber = dataList.DashboardBP_ProjectList.PZ_PlanZakaz.PlanZakaz,
                    taskName = dataList.WBS.WBSLongCiliricName,
                    executorName = new AspNetUsersContext().GetCiliricalName(dataList.AspNetUsers),
                    basicStartDate = JsonConvert.SerializeObject(dataList.basicStart, shortDateString).Replace(@"""", ""),
                    startDate = JsonConvert.SerializeObject(dataList.start, shortDateString).Replace(@"""", ""),
                    basicFinishDate = JsonConvert.SerializeObject(dataList.basicFinish, shortDateString).Replace(@"""", ""),
                    finishDate = JsonConvert.SerializeObject(dataList.finish, shortDateString).Replace(@"""", ""),
                    remainingWork = Math.Round(dataList.remainingWork, 1)
                });

                return Json(new { data });
            }
        }

        public JsonResult GetVarianceTasksTable()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                DateTime thisDay = DateTime.Today.AddDays(-1);
                var query = db.DashboardBP_ProjectTasks
                    .AsNoTracking()
                    .Include(d => d.DashboardBP_ProjectList.PZ_PlanZakaz)
                    .Include(d => d.WBS)
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.percentComplited != 100)
                    .Where(d => d.basicStart == thisDay || d.basicFinish == thisDay)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    orderNumber = dataList.DashboardBP_ProjectList.PZ_PlanZakaz.PlanZakaz,
                    taskName = dataList.WBS.WBSLongCiliricName,
                    executorName = new AspNetUsersContext().GetCiliricalName(dataList.AspNetUsers),
                    basicStartDate = JsonConvert.SerializeObject(dataList.basicStart, shortDateString).Replace(@"""", ""),
                    startDate = JsonConvert.SerializeObject(dataList.start, shortDateString).Replace(@"""", ""),
                    basicFinishDate = JsonConvert.SerializeObject(dataList.basicFinish, shortDateString).Replace(@"""", ""),
                    finishDate = JsonConvert.SerializeObject(dataList.finish, shortDateString).Replace(@"""", ""),
                    remainingWork = Math.Round(dataList.remainingWork, 1)
                });

                return Json(new { data });
            }
        }

        public JsonResult GetCountComments()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int cont = db.DashboardBPComments.Where(d => d.counterState1 != d.counterState2).Count();

                return Json(cont, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCommentsList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardBPComments.AsNoTracking().Where(d => d.counterState1 != d.counterState2).ToList();
                var data = query.Select(datalist => new
                {
                    datalist.orderNumber,
                    datalist.taskName,
                    datalist.notes,
                    datalist.workerName
                });

                return Json(new { data });
            }
        }
    }
}