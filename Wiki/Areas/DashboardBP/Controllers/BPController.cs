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

        [HttpGet]
        public JsonResult GetProjectsPortfolio()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardBP_ProjectList
                    .AsNoTracking()
                    .Include(d => d.DashboardBP_State)
                    .Include(d => d.DashboardBP_TasksList.Select(s => s.ProjectTask))
                    .Include(d => d.DashboardBP_TasksList.Select(s => s.AspNetUsers))
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.DashboardBP_State.active == true)
                    .ToList();
                var data = GetGanttData(query);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        Project[] GetGanttData(List<DashboardBP_ProjectList> listTasks)
        {
            int counter = listTasks.Count;
            Project[] arrayTasks = new Project[counter];
            for (int i = 0; i < counter; i++)
            {
                Project project = new Project();
                project.Name = listTasks[i].PZ_PlanZakaz.PlanZakaz.ToString();
                int counterTasks = listTasks[i].DashboardBP_TasksList.Count;
                Task[] tasks = new Task[counterTasks];
                counterTasks = 0;
                foreach (var data in listTasks[i].DashboardBP_TasksList.ToList())
                {
                    Task task = new Task();
                    task.Name = data.ProjectTask.sName;
                    task.Id = data.ProjectTask.id_TASK_WBS;
                    //task.Parent = data.ProjectTask.sName;
                    task.Start = data.startDate.ToShortDateString();
                    task.End = data.finishDate.ToShortDateString();
                    task.Completed = (int)data.percentWorkCompleted;
                    //task.Owner = data.AspNetUsers.CiliricalName;
                    tasks[counterTasks] = task;
                    counterTasks++;
                }
                project.Tasks = tasks;
                arrayTasks[i] = project;
            }
            return arrayTasks;
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