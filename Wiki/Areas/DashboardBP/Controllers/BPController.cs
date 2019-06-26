using System.Web.Mvc;
using Wiki.Areas.DashboardBP.Models;
using System.Data.Entity;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Web.Script.Serialization;

namespace Wiki.Areas.DashboardBP.Controllers
{
    public class BPController : Controller
    {
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        JavaScriptSerializer js = new JavaScriptSerializer();
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

        public ActionResult Index()
        {
            //CreateNewBP();
            return View();
        }

        bool CreateNewBP()
        {
            NewBP bp = new NewBP();
            return true;
        }

        public JsonResult GetPeriodReport()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string query = "Отчет по состоянию на: " + db.DashboardBP_State.First(d => d.active == true).datetime.ToLongDateString() + " " + db.DashboardBP_State.First(d => d.active == true).datetime.ToShortTimeString();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProjectsPortfolio()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardBP_ProjectList
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                    .Include(d => d.DashboardBP_State)
                    .Include(d => d.DashboardBP_TasksList.Select(s => s.ProjectTask))
                    .Include(d => d.DashboardBP_TasksList.Select(s => s.AspNetUsers))
                    .Where(d => d.DashboardBP_State.active == true)
                    .Where(d => d.PZ_PlanZakaz.Client != 39)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                    .OrderBy(d => d.PZ_PlanZakaz.dataOtgruzkiBP)
                    .ToList();
                var data = GetGanttData(query);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        DataGanttProjectsPortfolio GetGanttData(List<DashboardBP_ProjectList> listTasks)
        {
            int projectsCounter = listTasks.Count * 2;
            Project[] projectsArray = new Project[projectsCounter];
            for (int i = 0, j = 0; i < projectsCounter / 2; i++)
            {
                Project project = new Project();
                project.color = "#058DC7";
                project.name = listTasks[i].PZ_PlanZakaz.PlanZakaz.ToString();
                project.id = listTasks[i].id.ToString();
                project.completed = new Complited { amount = listTasks[i].planProjectPercentCompleted / 100.0 };
                project.owner = listTasks[i].PZ_PlanZakaz.AspNetUsers.CiliricalName;
                project.start = Convert.ToUInt64(js.DeserializeObject(js.Serialize(listTasks[i].planDateStart).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
                project.end = Convert.ToUInt64(js.DeserializeObject(js.Serialize(listTasks[i].PZ_PlanZakaz.dataOtgruzkiBP).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
                project.contractDate = Convert.ToUInt64(js.DeserializeObject(js.Serialize(listTasks[i].contractDate).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
                project.y = i;
                project.milestone = false;
                projectsArray[j] = project;
                j++;
                project = new Project();
                project.name = listTasks[i].PZ_PlanZakaz.PlanZakaz.ToString();
                if(listTasks[i].contractDate < listTasks[i].PZ_PlanZakaz.dataOtgruzkiBP)
                    project.color = "#e64219";
                else
                    project.color = "#19e694";
                project.id = listTasks[i].id.ToString() + "ms";
                project.completed = null;
                project.owner = listTasks[i].PZ_PlanZakaz.AspNetUsers.CiliricalName;
                project.milestone = true;
                project.start = Convert.ToUInt64(js.DeserializeObject(js.Serialize(listTasks[i].contractDate).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
                project.end = Convert.ToUInt64(js.DeserializeObject(js.Serialize(listTasks[i].contractDate).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
                project.contractDate = Convert.ToUInt64(js.DeserializeObject(js.Serialize(listTasks[i].contractDate).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
                project.y = i;
                projectsArray[j] = project;
                j++;
            }
            DataGanttProjectsPortfolio portfolio = new DataGanttProjectsPortfolio();
            portfolio.projects = projectsArray;
            return portfolio;
        }

        public JsonResult GetHSSPlanToYear()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardHSSPlan
                    .AsNoTracking()
                    .Include(d => d.DashboardBP_State)
                    .Where(d => d.DashboardBP_State.active == true)
                    .ToList();
                int[] data = new int[2];
                data[0] = (int)query[0].plan - (int)query[0].hss;
                data[1] = (int)query[0].hss;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemainingHss()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardRemaining
                    .AsNoTracking()
                    .Include(d => d.DashboardBP_State)
                    .Where(d => d.DashboardBP_State.active == true)
                    .ToList();
                int[] data = new int[2];
                data[0] = (int)query[0].plan - (int)query[0].hss;
                data[1] = (int)query[0].hss;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRetePlan()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardRatePlan
                    .AsNoTracking()
                    .Include(d => d.DashboardBP_State)
                    .Where(d => d.DashboardBP_State.active == true)
                    .ToList();
                int[] data = new int[2];
                data[0] = (int)query[0].plan - (int)query[0].rate;
                data[1] = (int)query[0].rate;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetHSSToDay()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int yearStart = DateTime.Now.Year - 1;
                var query = db.DashboardBP_HSSPO
                    .AsNoTracking()
                    .Include(d => d.DashboardBP_State)
                    .Where(d => d.DashboardBP_State.active == true)
                    .Where(d => d.timeByDay.Year >= yearStart)
                    .OrderBy(d => d.month)
                    .GroupBy(d => d.month)
                    .Select(g => new { day = g.Key, Value = g.Sum(x => x.xSsm) })
                    .ToList();
                int[] data = new int[36];
                int countStep = 0;
                for(int i = yearStart; i < DateTime.Now.Year + 2; i++)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        string monthFind = "";
                        if (j.ToString().Length == 1)
                            monthFind = i.ToString() + "." + "0" + j.ToString();
                        else
                            monthFind = i.ToString() + "." + j.ToString();
                        try
                        {
                            data[countStep] = (int)query.First(d => d.day == monthFind).Value;
                        }
                        catch
                        {
                            data[countStep] = 0;
                        }
                        countStep++;
                    }
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}