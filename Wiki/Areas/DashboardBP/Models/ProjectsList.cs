using System;
using System.Linq;

namespace Wiki.Areas.DashboardBP.Models
{
    public class ProjectsList
    {
        public bool CreatePZList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var list = db.PZ_PlanZakaz.AsNoTracking().Where(d => d.dataOtgruzkiBP > DateTime.Now && d.ProjectUID != null).ToList();
                int idState = db.DashboardBP_State.AsNoTracking().First(d => d.active == true).id;
                foreach (var data in list)
                {
                    DashboardBP_ProjectList project = new DashboardBP_ProjectList
                    {
                        id_PZ_PlanZakaz = data.Id,
                        id_DashboardBP_State = idState,
                        contractDate = data.DateSupply,
                        planDateComplited = data.dataOtgruzkiBP
                    };
                    db.DashboardBP_ProjectList.Add(project);
                    db.SaveChanges();
                    new TasksList().CreateTask(data.ProjectUID.Value, project.id);
                }
            }
            return true;
        }
    }
}