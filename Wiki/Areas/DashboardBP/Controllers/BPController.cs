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
            CreateNewBP();
            return View();
        }

        bool CreateNewBP()
        {
            //NewBP bp = new NewBP();
            return true;
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
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                    .OrderBy(d => d.PZ_PlanZakaz.dataOtgruzkiBP)
                    .ToList();
                var data = GetGanttData(query);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        DataGanttProjectsPortfolio GetGanttData(List<DashboardBP_ProjectList> listTasks)
        {
            int projectsCounter = listTasks.Count;
            Project[] projectsArray = new Project[projectsCounter];
            //int countTasks = 0;
            //foreach (var data in listTasks)
            //{
            //    countTasks += data.DashboardBP_TasksList.Count;
            //}
            //Task[] tasksArray = new Task[countTasks];
            for (int i = 0; i < projectsCounter; i++)
            {
                Project project = new Project();
                project.name = listTasks[i].PZ_PlanZakaz.PlanZakaz.ToString();
                project.id = listTasks[i].id.ToString();
                project.completed = new Complited { amount = listTasks[i].planProjectPercentCompleted / 100.0 };
                project.owner = listTasks[i].PZ_PlanZakaz.AspNetUsers.CiliricalName;
                JavaScriptSerializer js = new JavaScriptSerializer();
                project.start = Convert.ToUInt64(js.DeserializeObject(js.Serialize(listTasks[i].planDateStart).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
                project.end = Convert.ToUInt64(js.DeserializeObject(js.Serialize(listTasks[i].PZ_PlanZakaz.dataOtgruzkiBP).Replace("\"\\/Date(", "").Replace(")\\/\"", "")));
                projectsArray[i] = project;
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