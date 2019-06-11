using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Wiki.Areas.DashboardBP.Models
{
    public class TasksList
    {
        public bool CreateTask(Guid projectUID, int id_DashboardBP_ProjectList)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var tasksPWAList = db.PWA_TasksForBP.AsNoTracking().Where(d => d.ProjectUID == projectUID).ToList();
                var tasksBPList = db.ProjectTask.Where(d => d.isActiveBP).ToList();
                foreach (var dataBP in tasksBPList)
                {
                    if(db.PWA_TasksForBP.Where(d => d.TaskWBS == dataBP.id_TASK_WBS).Count() > 0)
                    {
                        PWA_TasksForBP taskPWA = db.PWA_TasksForBP.First(d => d.TaskWBS == dataBP.id_TASK_WBS);
                        DashboardBP_TasksList task = new DashboardBP_TasksList
                        {
                            id_ProjectTask = dataBP.id,
                            id_DashboardBP_ProjectList = id_DashboardBP_ProjectList,
                            startDate = taskPWA.TaskStartDate.Value,
                            finishDate = taskPWA.TaskFinishDate.Value,
                            deadline = taskPWA.TaskDeadline.Value,
                            bStartDate = taskPWA.TaskBaseline0StartDate.Value,
                            bFinishDate = taskPWA.TaskBaseline0FinishDate.Value,
                            duration = (double)taskPWA.TaskDuration,
                            bDuration = (double)taskPWA.TaskBaseline0Duration,
                            work = (double)taskPWA.TaskWork,
                            bWork = (double)taskPWA.TaskBaseline0Work,
                            actualWork = (double)taskPWA.TaskActualWork,
                            remainingWork = (double)taskPWA.TaskRemainingWork,
                            percentCompleted = (double)taskPWA.TaskPercentCompleted,
                            percentWorkCompleted = (double)taskPWA.TaskPercentWorkCompleted,
                            isCritical = taskPWA.TaskIsCritical.Value,
                            priority = (int)taskPWA.TaskPriority,
                            id_AspNetUsers = taskPWA.ResourceUID.ToString()
                        };
                        db.DashboardBP_TasksList.Add(task);
                        db.SaveChanges();
                    }

                }
            }
            return true; 
        } 

    }
}