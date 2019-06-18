using System.Web.Mvc;
using Wiki.Areas.DashboardBP.Models;
using System.Data.Entity;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Wiki.Areas.DashboardBP.Controllers
{
    public class BPController : Controller
    {
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        static readonly long DATE1970_TICKS = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
        static readonly Regex DATE_SERIALIZATION_REGEX = new Regex(@"\\/Date\((?<ticks>-?\d+)\)\\/", RegexOptions.Compiled);


        static string ISO8601Serialization(string input)
        {
            return DATE_SERIALIZATION_REGEX.Replace(input, match =>
            {
                var ticks = long.Parse(match.Groups["ticks"].Value) * 10000;
                return new DateTime(ticks + DATE1970_TICKS).ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss.fff");
            });
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
            //int countTasks = 0;
            //foreach (var data in listTasks)
            //{
            //    countTasks += data.DashboardBP_TasksList.Count;
            //}
            //Task[] tasksArray = new Task[countTasks];
            for (int i = 0, j = 0; i < projectsCounter / 2; i++)
            {
                Project project = new Project();
                project.color = "#058DC7";
                project.name = listTasks[i].PZ_PlanZakaz.PlanZakaz.ToString();
                project.id = listTasks[i].id.ToString();
                project.completed = new Complited { amount = listTasks[i].planProjectPercentCompleted / 100.0 };
                project.owner = listTasks[i].PZ_PlanZakaz.AspNetUsers.CiliricalName;
                JavaScriptSerializer js = new JavaScriptSerializer();
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
            //countTasks = 0;
            //foreach (var data in listTasks)
            //{
            //    foreach (var dataTasksList in data.DashboardBP_TasksList.ToList())
            //    {
            //        JavaScriptSerializer js = new JavaScriptSerializer();
            //        Task task = new Task();
            //        task.id = dataTasksList.id.ToString() + dataTasksList.ProjectTask.id_TASK_WBS;
            //        task.completed = dataTasksList.percentWorkCompleted.ToString();
            //        task.end = Convert.ToUInt64(js.DeserializeObject(js.Serialize(dataTasksList.finishDate).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
            //        task.name = dataTasksList.ProjectTask.sName;
            //        task.owner = "";
            //        task.parent = dataTasksList.id_DashboardBP_ProjectList.ToString();
            //        task.start = Convert.ToUInt64(js.DeserializeObject(js.Serialize(dataTasksList.startDate).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
            //        tasksArray[countTasks] = task;
            //        countTasks++;
            //    }
            //}
            DataGanttProjectsPortfolio portfolio = new DataGanttProjectsPortfolio();
            portfolio.projects = projectsArray;
            //portfolio.tasks = tasksArray;
            return portfolio;
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
    }
}