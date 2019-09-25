using Microsoft.ProjectServer.Client;
using System;
using System.Linq;

namespace Wiki.Models
{
    public class ProjectServer_UpdateMustStartOnCRUD : ProjectServer
    {
        public ProjectServer_UpdateMustStartOnCRUD()
        {
            UpdateTasks();
        }

        public ProjectServer_UpdateMustStartOnCRUD(int id_PZ_PlanZakaz, string taskWBS, DateTime dateTime)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                try
                {
                    ProjectServer_UpdateMustStartOn projectServer_UpdateMustStartOn = new ProjectServer_UpdateMustStartOn();
                    Guid guid = db.PZ_PlanZakaz.Find(id_PZ_PlanZakaz).ProjectUID.Value;
                    projectServer_UpdateMustStartOn.ActualStart = dateTime;
                    projectServer_UpdateMustStartOn.ProjectUID = guid;
                    projectServer_UpdateMustStartOn.TaskUID = db.PWA_EmpTaskAll.First(d => d.ProjectUID == guid && d.TaskWBS == taskWBS).TaskUID;
                    db.ProjectServer_UpdateMustStartOn.Add(projectServer_UpdateMustStartOn);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
            UpdateTasks();
        }

        private int ReadAndUpdateProject()
        {
            try
            {
                var tasksList = _db.ProjectServer_UpdateMustStartOn.ToList();
                if (tasksList.Count > 0)
                {
                    try
                    {
                        foreach (var dataList in tasksList)
                        {
                            ProjectContext context = new ProjectContext(PwaPath);
                            string nameProject = _db.PWA_EmpProject.First(d => d.ProjectUID == dataList.ProjectUID).ProjectName;
                            var projCollection = context.LoadQuery(context.Projects.Where(p => p.Name == nameProject));
                            context.ExecuteQuery();
                            PublishedProject project = projCollection.First();
                            DraftProject draft = project.CheckOut();
                            context.Load(draft, p => p.StartDate,
                                                p => p.Description);
                            string taskName = _db.PWA_EmpTaskAll.First(d => d.TaskUID == dataList.TaskUID).TaskName;
                            context.Load(draft.Tasks, dt => dt.Where(t => t.Name == taskName));
                            context.Load(draft.Assignments, da => da.Where(a => a.Task.Name == taskName));
                            context.ExecuteQuery();
                            DraftTask task = draft.Tasks.First();
                            task.ConstraintType = ConstraintType.MustStartOn;
                            task.ConstraintStartEnd = dataList.ActualStart;
                            draft.Update();
                            JobState jobState = context.WaitForQueue(draft.Publish(true), 20);
                        }
                        return 1;
                    }
                    catch
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public void UpdateTasks()
        {
            try
            {
                var projectServerUpdateTasks = _db.ProjectServer_UpdateMustStartOn.ToList();
                if (projectServerUpdateTasks.Count > 0)
                {
                    if (ReadAndUpdateProject() == 1)
                        RemoveProjectServer_UpdateTasks();
                }
            }
            catch
            {

            }
        }

        private void RemoveProjectServer_UpdateTasks()
        {
            foreach (var data in _db.ProjectServer_UpdateMustStartOn.ToList())
            {
                _db.ProjectServer_UpdateMustStartOn.Remove(data);
                _db.SaveChanges();
            }
        }
    }
}