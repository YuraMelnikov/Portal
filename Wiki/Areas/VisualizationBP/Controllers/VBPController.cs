using System.Web.Mvc;
using System.Data.Entity;
using Wiki.Areas.DashboardBP.Models;
using System.Collections.Generic;
using System;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Linq;

namespace Wiki.Areas.VisualizationBP.Controllers
{
    public class VBPController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
            JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
            JavaScriptSerializer js = new JavaScriptSerializer();
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
                if (listTasks[i].contractDate < listTasks[i].PZ_PlanZakaz.dataOtgruzkiBP)
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
    }
}