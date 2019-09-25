using Microsoft.ProjectServer.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki.Models
{
    public class ProjectServer
    {
        private const string PwaPath = "http://tpserver/PWA/";
        private readonly PortalKATEKEntities _db = new PortalKATEKEntities();
        private List<Guid> _removeGuid = new List<Guid>();

        public void CreateTasks()
        {
            var projectServerCreateTasks = _db.ProjectServer_CreateTasks.GroupBy(d => d.ProjectUID).ToList();
            foreach (var data in projectServerCreateTasks)
            {
                if (CreateTasksInProject(data) == 1)
                    _removeGuid.Add(data.Key);
            }
            RemoveProjectServer_CreateTasks();
        }

        public void UpdateTasks()
        {
            var projectServerUpdateTasks = _db.ProjectServer_UpdateTasks.GroupBy(d => d.projectUID).ToList();
            foreach (var data in projectServerUpdateTasks)
            {
                if (ReadAndUpdateProject() == 1)
                    _removeGuid.Add(data.Key);
            }
            RemoveProjectServer_UpdateTasks();
        }

        private void RemoveProjectServer_UpdateTasks()
        {
            foreach (var dataPr in _removeGuid)
            {
                foreach (var data in _db.ProjectServer_UpdateTasks.Where(d => d.projectUID == dataPr).ToList())
                {
                    _db.ProjectServer_UpdateTasks.Remove(data);
                    _db.SaveChanges();
                }
            }
        }

        private void RemoveProjectServer_CreateTasks()
        {
            foreach (var dataPr in _removeGuid)
            {
                foreach (var data in _db.ProjectServer_CreateTasks.Where(d => d.ProjectUID == dataPr).ToList())
                {
                    _db.ProjectServer_CreateTasks.Remove(data);
                    _db.SaveChanges();
                }
            }
        }

        private int UpdateTasksInProject(IGrouping<Guid, ProjectServer_UpdateTasks> data)
        {
            try
            {
                using (ProjectContext projectCont1 = new ProjectContext(PwaPath))
                {
                    string nameProject = _db.PWA_EmpProject.First(d => d.ProjectUID == data.Key).ProjectName;
                    var projCollection = projectCont1.LoadQuery(projectCont1.Projects.Where(p => p.Name == nameProject));
                    projectCont1.ExecuteQuery();
                    PublishedProject proj2Edit = projCollection.First();
                    DraftProject projCheckedOut = proj2Edit.CheckOut();
                    projectCont1.Load(projCheckedOut.Tasks);
                    projectCont1.ExecuteQuery();
                    DraftTaskCollection catskill = projCheckedOut.Tasks;



                    foreach (DraftTask task in catskill)
                    {
                        try
                        {
                            if (task.Name != null && task.Id == _db.ProjectServer_UpdateTasks.First(d => d.taskUID == task.Id).taskUID)
                            {
                                ProjectServer_UpdateTasks projectServer_UpdateTasks = _db.ProjectServer_UpdateTasks.First(d => d.taskUID == task.Id);
                                var nk = projectServer_UpdateTasks.nk;
                                if (nk != null)
                                {
                                    int time = (int)nk;
                                    projectCont1.Load(task.CustomFields);
                                    projectCont1.ExecuteQuery();
                                    foreach (CustomField cus in task.CustomFields)
                                    {
                                        if (cus.Name == "НК")
                                        {
                                            string intname = cus.InternalName;
                                            task[intname] = time;
                                        }
                                    }
                                    if (task.PercentComplete == 0)
                                    {
                                        task.Work = time + "ч";
                                    }
                                    else
                                    {
                                        int factWork = Convert.ToInt32(task.Work.Substring(0, task.Work.Length - 1)) - Convert.ToInt32(task.RemainingWork.Substring(0, task.RemainingWork.Length - 1));
                                        if (factWork < time)
                                            task.Work = time - factWork + "ч";
                                    }
                                    QueueJob qJob1 = projCheckedOut.Update();
                                    JobState jobState1 = projectCont1.WaitForQueue(qJob1, 10);
                                }
                                else
                                {
                                    ProjectServer_UpdateTasks pTask = _db.ProjectServer_UpdateTasks.First(d => d.taskUID == task.Id);
                                    if (pTask.percentComplited != null)
                                        task.PercentComplete = (int)pTask.percentComplited;
                                    if (pTask.finishDate != null)
                                        task.Finish = (DateTime)pTask.finishDate;
                                    if (task.IsMilestone == true)
                                        task.Start = (DateTime)pTask.finishDate;
                                    QueueJob qJob1 = projCheckedOut.Update();
                                    JobState jobState1 = projectCont1.WaitForQueue(qJob1, 10);
                                }

                            }
                        }
                        catch
                        {
                        }
                    }
                    projCheckedOut.Publish(true);
                    QueueJob qJob = projectCont1.Projects.Update();
                    JobState jobState = projectCont1.WaitForQueue(qJob, 20);
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        private int CreateTasksInProject(IGrouping<Guid, ProjectServer_CreateTasks> data)
        {
            try
            {
                using (ProjectContext projectCont1 = new ProjectContext(PwaPath))
                {
                    string nameProject = _db.PWA_EmpProject.First(d => d.ProjectUID == data.Key).ProjectName;
                    var projCollection = projectCont1.LoadQuery(projectCont1.Projects.Where(p => p.Name == nameProject));
                    projectCont1.ExecuteQuery();
                    PublishedProject proj2Edit = projCollection.First();
                    DraftProject projCheckedOut = proj2Edit.CheckOut();

                    foreach (var taskInList in _db.ProjectServer_CreateTasks.Where(d => d.ProjectUID == data.Key).ToList())
                    {
                        Guid taskId = Guid.NewGuid();
                        Task task = projCheckedOut.Tasks.Add(new TaskCreationInformation()
                        {
                            Id = taskId,
                            Name = taskInList.TaskName,
                            Notes = "new Task",
                            IsManual = false,
                            Duration = "1d",
                            Start = DateTime.Now
                        });
                        projCheckedOut.Update();
                        // Create a local resource and assign the task to him
                        projCheckedOut.Update();
                        Guid resourceGuid =
                            (Guid)_db.AspNetUsers.First(d => d.Email == taskInList.Resource).ResourceUID;
                        DraftAssignment assignment = projCheckedOut.Assignments.Add(new AssignmentCreationInformation()
                        {
                            ResourceId = resourceGuid,
                            TaskId = taskId
                        });
                        projCheckedOut.Update();
                    }

                    projCheckedOut.Publish(true);
                    QueueJob qJob = projectCont1.Projects.Update();
                    JobState jobState = projectCont1.WaitForQueue(qJob, 20);
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Read and update the project,
        /// this method need a project named "New Project" with a task "New task" and assign to a local resource named "New local resource" already created.
        /// Basically please run CreateProjectWithTaskAndAssignment() before running this to avoid exceptions
        /// </summary>
        public int ReadAndUpdateProject()
        {
            try
            {
                var tasksList = _db.ProjectServer_UpdateTasks.ToList();
                if (tasksList.Count > 0)
                {
                    try
                    {
                        foreach (var dataList in tasksList)
                        {
                            ProjectContext context = new ProjectContext(PwaPath);
                            string nameProject = _db.PWA_EmpProject.First(d => d.ProjectUID == dataList.projectUID).ProjectName;
                            var projCollection = context.LoadQuery(context.Projects.Where(p => p.Name == nameProject));
                            context.ExecuteQuery();
                            PublishedProject project = projCollection.First();
                            DraftProject draft = project.CheckOut();
                            context.Load(draft, p => p.StartDate,
                                                p => p.Description);
                            string taskName = _db.PWA_EmpTask.First(d => d.TaskUID == dataList.taskUID).TaskName;
                            context.Load(draft.Tasks, dt => dt.Where(t => t.Name == taskName));
                            context.Load(draft.Assignments, da => da.Where(a => a.Task.Name == taskName));
                            context.ExecuteQuery();
                            DraftTask task = draft.Tasks.First();
                            draft.Description += "(description updated)";
                            task.Start = DateTime.Now;
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
    }
}