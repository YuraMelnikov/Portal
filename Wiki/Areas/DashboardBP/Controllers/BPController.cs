using System.Web.Mvc;
using Wiki.Areas.DashboardBP.Models;
using System.Data.Entity;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Wiki.Areas.DashboardBP.Controllers
{
    public class BPController : Controller
    {
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        public ActionResult Index()
        {
            return View();
        }

        bool CreateNewBP()
        {
            NewBP bp = new NewBP();
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
                    .ToList();
                var data = GetGanttData(query);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        DataGanttProjectsPortfolio GetGanttData(List<DashboardBP_ProjectList> listTasks)
        {
            int projectsCounter = listTasks.Count;
            Project[] projectsArray = new Project[projectsCounter];
            int countTasks = 0;
            foreach (var data in listTasks)
            {
                countTasks += data.DashboardBP_TasksList.Count;
            }
            Task[] tasksArray = new Task[countTasks];

            for (int i = 0; i < projectsCounter; i++)
            {
                Project project = new Project();
                project.name = listTasks[i].PZ_PlanZakaz.PlanZakaz.ToString();
                project.id = listTasks[i].id.ToString();
                project.owner = listTasks[i].PZ_PlanZakaz.AspNetUsers.CiliricalName;
                projectsArray[i] = project;
            }
            countTasks = 0;
            foreach (var data in listTasks)
            {
                foreach (var dataTasksList in data.DashboardBP_TasksList.ToList())
                {
                    Task task = new Task();
                    task.id = dataTasksList.id.ToString() + dataTasksList.ProjectTask.id_TASK_WBS;
                    task.completed = (int)dataTasksList.percentWorkCompleted;
                    task.end = dataTasksList.finishDate.ToShortDateString();
                    task.name = dataTasksList.ProjectTask.sName;
                    task.owner = "";
                    task.parent = dataTasksList.id_DashboardBP_ProjectList.ToString();
                    task.start = dataTasksList.startDate.ToShortDateString();
                    tasksArray[countTasks] = task;
                    countTasks++;
                }
            }

            DataGanttProjectsPortfolio portfolio = new DataGanttProjectsPortfolio();
            portfolio.projects = projectsArray;
            portfolio.tasks = tasksArray;
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