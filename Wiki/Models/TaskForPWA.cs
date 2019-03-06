using System;

namespace Wiki.Models
{
    public class TaskForPWA
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public void CreateTaskRKD(Guid projectUID, Guid taskUID, DateTime dateFinish, int percentCompleted)
        {
            if (projectUID != null || taskUID != null)
            {
                ProjectServer_UpdateTasks newTask = new ProjectServer_UpdateTasks
                {
                    finishDate = dateFinish,
                    projectUID = projectUID,
                    taskUID = taskUID,
                    percentComplited = percentCompleted
                };
                db.ProjectServer_UpdateTasks.Add(newTask);
                db.SaveChanges();
            }
        }

        public void CreateTaskOTK_PO(Guid projectUID, string taskName, string email)
        {
            ProjectServer_CreateTasks newTask = new ProjectServer_CreateTasks
            {
                update = false,
                ProjectUID = projectUID,
                TaskName = SubstringTaskNameReclamatoin(taskName),
                Resource = email
            };
            db.ProjectServer_CreateTasks.Add(newTask);
            db.SaveChanges();
            ProjectServer projectServer = new ProjectServer();
            projectServer.CreateTasks();
        }

        private string SubstringTaskNameReclamatoin(string textReclamation)
        {
            string reclamation = "Рекламация № " + textReclamation;
            if (reclamation.Length > 50)
                reclamation = reclamation.Substring(0, 50);
            return reclamation; 
        }

    }
}