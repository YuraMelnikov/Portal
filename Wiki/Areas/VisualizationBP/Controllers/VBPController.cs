using System.Web.Mvc;
using System.Linq;
using System;
using System.Data.Entity;
using Newtonsoft.Json;
using Wiki.Models;
using System.Collections.Generic;
using Wiki.Areas.VisualizationBP.Types;

namespace Wiki.Areas.VisualizationBP.Controllers
{
    public class VBPController : Controller
    {
        JsonSerializerSettings shortDateString = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetHSSPlanToYear()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardHSSPlan.AsNoTracking().ToList();
                int[] data = new int[2];
                data[0] = ((int)query[0].plan - (int)query[0].fact) / 1000;
                data[1] = ((int)query[0].fact) / 1000;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRatePlanToYear()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardRatePlan.AsNoTracking().ToList();
                int[] data = new int[2];
                data[0] = ((int)query[0].plan - (int)query[0].fact) / 1000;
                data[1] = ((int)query[0].fact) / 1000;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemainingHSS()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardRemaining.AsNoTracking().ToList();
                int[] data = new int[2];
                data[0] = ((int)query[0].fact) / 1000;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetNoPlaningHSS()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int query = (int)db.DashboardBP_HSSPO.AsNoTracking().Sum(d => d.xSsmNoplaning);
                int[] data = new int[2];
                data[0] = query / 1000;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTaskThisDayTable()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                DateTime thisDay = DateTime.Today;
                var query = db.DashboardBP_ProjectTasks
                    .AsNoTracking()
                    .Include(d => d.DashboardBP_ProjectList.PZ_PlanZakaz)
                    .Include(d => d.WBS)
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.percentComplited != 100)
                    .Where(d => d.basicStart == thisDay || d.basicFinish == thisDay)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    orderNumber = dataList.DashboardBP_ProjectList.PZ_PlanZakaz.PlanZakaz,
                    taskName = dataList.WBS.WBSLongCiliricName,
                    executorName = new AspNetUsersContext().GetCiliricalName(dataList.AspNetUsers),
                    basicStartDate = JsonConvert.SerializeObject(dataList.basicStart, shortDateString).Replace(@"""", ""),
                    startDate = JsonConvert.SerializeObject(dataList.start, shortDateString).Replace(@"""", ""),
                    basicFinishDate = JsonConvert.SerializeObject(dataList.basicFinish, shortDateString).Replace(@"""", ""),
                    finishDate = JsonConvert.SerializeObject(dataList.finish, shortDateString).Replace(@"""", ""),
                    remainingWork = Math.Round(dataList.remainingWork, 1)
                });
                return Json(new { data });
            }
        }

        public JsonResult GetVarianceTasksTable()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                DateTime thisDay = DateTime.Today.AddDays(-1);
                var query = db.DashboardBP_ProjectTasks
                    .AsNoTracking()
                    .Include(d => d.DashboardBP_ProjectList.PZ_PlanZakaz)
                    .Include(d => d.WBS)
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.percentComplited != 100)
                    .Where(d => d.basicStart == thisDay || d.basicFinish == thisDay)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    orderNumber = dataList.DashboardBP_ProjectList.PZ_PlanZakaz.PlanZakaz,
                    taskName = dataList.WBS.WBSLongCiliricName,
                    executorName = new AspNetUsersContext().GetCiliricalName(dataList.AspNetUsers),
                    basicStartDate = JsonConvert.SerializeObject(dataList.basicStart, shortDateString).Replace(@"""", ""),
                    startDate = JsonConvert.SerializeObject(dataList.start, shortDateString).Replace(@"""", ""),
                    basicFinishDate = JsonConvert.SerializeObject(dataList.basicFinish, shortDateString).Replace(@"""", ""),
                    finishDate = JsonConvert.SerializeObject(dataList.finish, shortDateString).Replace(@"""", ""),
                    remainingWork = Math.Round(dataList.remainingWork, 1)
                });
                return Json(new { data });
            }
        }

        public JsonResult GetCountComments()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int cont = db.DashboardBPComments.Where(d => d.counterState1 != d.counterState2).Count();
                return Json(cont, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCommentsList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardBPComments.AsNoTracking().Where(d => d.counterState1 != d.counterState2).ToList();
                var data = query.Select(datalist => new
                {
                    datalist.orderNumber,
                    datalist.taskName,
                    datalist.notes,
                    datalist.workerName
                });
                return Json(new { data });
            }
        }

        [HttpPost]
        public JsonResult GetWorkpowerManufacturing()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardBPManpowerManuf
                    .AsNoTracking()
                    .Include(d => d.DashboardBPDevisionCoef.Devision)
                    .Include(d => d.ProductionCalendar)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    devision = dataList.DashboardBPDevisionCoef.Devision.name,
                    countPrj = dataList.manpowerPrj,
                    workPrj = Math.Round(dataList.planWork, 0),
                    workDay = dataList.workday,
                    workMode = Math.Round(dataList.workMode, 2),
                    dataList.ProductionCalendar.period,
                    coefDefaultWork = dataList.DashboardBPDevisionCoef.coefDefaultWork
                });
                return Json(new { data });
            }
        }

        public JsonResult GetProjectTasksStates(string id)
        {
            int ids = Convert.ToInt32(id);
            int countBlocks = 5;
            ProjectTasksState projectTasksState = new ProjectTasksState(countBlocks);
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var tasksList = db.DashboardBPTaskInsert.AsNoTracking().Include(d => d.AspNetUsers).Where(d => d.id_PZ_PlanZakaz == ids).OrderBy(d => d.TaskIndex).ToList();
                projectTasksState.BlockProjectTasksStates[0] = GetTasksStartBlock(tasksList.Where(d => d.TaskOutlineLevel == 1).ToList());
                projectTasksState.BlockProjectTasksStates[1] = GetTasksPfBlock(tasksList.Where(d => d.TaskOutlineLevel == 2 || d.TaskOutlineLevel == 3).Where(d => d.TaskIsSummary == true).ToList());
                projectTasksState.BlockProjectTasksStates[2] = GetTasksFinalBlock(ids);
                //04 - docBlock
                //05 - shBlock
                var query = db.DashboardBPManpowerManuf.AsNoTracking().Include(d => d.DashboardBPDevisionCoef.Devision).Include(d => d.ProductionCalendar).ToList();

                var data = query.Select(dataList => new
                {
                    devision = 0
                });
                return Json(new { data });
            }
        }

        BlockProjectTasksState GetTasksStartBlock(List<DashboardBPTaskInsert> inputList)
        {
            int countElements = 1;
            int countElementsTasks = 6;
            string nameElement = "Начало разработки";
            string[] wbsArray = new string[] { "ПР", "ПЭ", "РМ", "РЭ", "СМ", "СЭ" };
            BlockProjectTasksState blockProjectTasksState = new BlockProjectTasksState(countElements, nameElement);
            for (int i = 0; i < countElements; i++)
            {
                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates =
                    new ElementDataProjectTasksState[countElementsTasks];
            }
            for (int i = 0; i < countElementsTasks; i++)
            {
                try
                {
                    blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[i] = new ElementDataProjectTasksState(inputList.First(d => d.TaskName == wbsArray[i]));
                }
                catch
                {

                }
            }
            return blockProjectTasksState;
        }

        BlockProjectTasksState GetTasksPfBlock(List<DashboardBPTaskInsert> inputList)
        {
            int id = inputList[0].id_PZ_PlanZakaz;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                int countElements = 0;
                int countElementsTasks = 4;

                foreach (var data in inputList)
                {
                    if (data.TaskWBS1 != "ОС")
                        break;
                    countElements++;
                }
                Elementnames[] elementsArray = new Elementnames[countElements];
                for (int i = 0; i < countElements; i++)
                { 
                    elementsArray[i].taskName = inputList[i].TaskName;
                    elementsArray[i].wbsName = inputList[i].TaskWBS2;
                }
                BlockProjectTasksState blockProjectTasksState = new BlockProjectTasksState(countElements);
                for (int i = 0; i < countElements; i++)
                {
                    blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates =
                        new ElementDataProjectTasksState[countElementsTasks];
                }
                for (int i = 0; i < countElements; i++)
                {
                    blockProjectTasksState.ElementProjectTasksStates[i].Name = elementsArray[i].taskName;
                    blockProjectTasksState.ElementProjectTasksStates[i].WBS = elementsArray[i].wbsName;
                }
                for (int i = 0; i < countElements; i++)
                {
                    for (int j = 0; j < countElementsTasks; j++)
                    {
                        blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j] = new ElementDataProjectTasksState();
                        switch (j)
                        {
                            case 0:
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j] = GetElementDataProjectTasksStateKBM(blockProjectTasksState.ElementProjectTasksStates[i].WBS, id);
                                break;
                            case 1:
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j] = GetElementDataProjectTasksStateKBE(blockProjectTasksState.ElementProjectTasksStates[i].WBS, id);
                                break;
                            case 2:
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j] = GetElementDataProjectTasksStateManufacturing(blockProjectTasksState.ElementProjectTasksStates[i].WBS, id);
                                break;
                            case 3:
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Name = "";
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].FinishDate = DateTime.Now;
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].RemainingWork = 0;
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].StartDate = DateTime.Now;
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Users = "";
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Work = 0;
                                break;
                            default:
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Name = "";
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].FinishDate = DateTime.Now;
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].RemainingWork = 0;
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].StartDate = DateTime.Now;
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Users = "";
                                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Work = 0;
                                break;
                        }
                    }
                }
                return blockProjectTasksState;
            }
        }

        ElementDataProjectTasksState GetElementDataProjectTasksStateKBM(string wbs, int id)
        {
            DateTime defaulTime = DateTime.Now;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                ElementDataProjectTasksState elementDataProjectTasksState = new ElementDataProjectTasksState();
                var tasksList = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Where(d => d.TaskWBS2 == wbs && d.id_PZ_PlanZakaz == id && d.AspNetUsers.Devision == 15)
                    .Include(d => d.AspNetUsers)
                    .ToList();
                elementDataProjectTasksState.Name = "Разработка КБМ";
                try
                {
                    elementDataProjectTasksState.StartDate = tasksList.Min(d => d.TaskStartDate);
                }
                catch
                {
                    elementDataProjectTasksState.StartDate = defaulTime;
                }
                try
                {
                    elementDataProjectTasksState.FinishDate = tasksList.Max(d => d.TaskfinishDate);
                }
                catch
                {
                    elementDataProjectTasksState.FinishDate = defaulTime;
                }
                elementDataProjectTasksState.Work = tasksList.Sum(d => d.TaskWork.Value);
                elementDataProjectTasksState.RemainingWork = tasksList.Sum(d => d.TaskRemainingWork.Value);
                elementDataProjectTasksState.Users = "";
                foreach (var ciliricalName in tasksList)
                {
                    elementDataProjectTasksState.Users += ciliricalName.AspNetUsers.CiliricalName + "; ";
                }
                return elementDataProjectTasksState;
            }
        }

        ElementDataProjectTasksState GetElementDataProjectTasksStateKBE(string wbs, int id)
        {
            DateTime defaulTime = DateTime.Now;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                ElementDataProjectTasksState elementDataProjectTasksState = new ElementDataProjectTasksState();
                var tasksList = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Where(d => d.TaskWBS2 == wbs && d.id_PZ_PlanZakaz == id)
                    .Where(d => d.AspNetUsers.Devision == 3 || d.AspNetUsers.Devision == 16)
                    .Include(d => d.AspNetUsers)
                    .ToList();
                elementDataProjectTasksState.Name = "Разработка КБЭ";
                try
                {
                    elementDataProjectTasksState.StartDate = tasksList.Min(d => d.TaskStartDate);
                }
                catch
                {
                    elementDataProjectTasksState.StartDate = defaulTime;
                }
                try
                {
                    elementDataProjectTasksState.FinishDate = tasksList.Max(d => d.TaskfinishDate);
                }
                catch
                {
                    elementDataProjectTasksState.FinishDate = defaulTime;
                }
                elementDataProjectTasksState.Work = tasksList.Sum(d => d.TaskWork.Value);
                elementDataProjectTasksState.RemainingWork = tasksList.Sum(d => d.TaskRemainingWork.Value);
                elementDataProjectTasksState.Users = "";
                foreach (var ciliricalName in tasksList)
                {
                    elementDataProjectTasksState.Users += ciliricalName.AspNetUsers.CiliricalName + "; ";
                }
                return elementDataProjectTasksState;
            }
        }

        ElementDataProjectTasksState GetElementDataProjectTasksStateManufacturing(string wbs, int id)
        {
            DateTime defaulTime = DateTime.Now;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                ElementDataProjectTasksState elementDataProjectTasksState = new ElementDataProjectTasksState();
                var tasksList = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Where(d => d.TaskWBS2 == wbs && d.id_PZ_PlanZakaz == id)
                    .Where(d => d.AspNetUsers.Devision == 8 || d.AspNetUsers.Devision == 9 || d.AspNetUsers.Devision == 10 || d.AspNetUsers.Devision == 22 || d.AspNetUsers.Devision == 20)
                    .Include(d => d.AspNetUsers)
                    .ToList();
                elementDataProjectTasksState.Name = "Производство";
                try
                {
                    elementDataProjectTasksState.StartDate = tasksList.Min(d => d.TaskStartDate);
                }
                catch
                {
                    elementDataProjectTasksState.StartDate = defaulTime;
                }
                try
                {
                    elementDataProjectTasksState.FinishDate = tasksList.Max(d => d.TaskfinishDate);
                }
                catch
                {
                    elementDataProjectTasksState.FinishDate = defaulTime;
                }
                elementDataProjectTasksState.Work = tasksList.Sum(d => d.TaskWork.Value);
                elementDataProjectTasksState.RemainingWork = tasksList.Sum(d => d.TaskRemainingWork.Value);
                elementDataProjectTasksState.Users = "";
                foreach (var ciliricalName in tasksList)
                {
                    elementDataProjectTasksState.Users += ciliricalName.AspNetUsers.CiliricalName + "; ";
                }
                return elementDataProjectTasksState;
            }
        }

        BlockProjectTasksState GetTasksFinalBlock(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var tasksLiat = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.TaskIsSummary == false && d.TaskOutlineLevel == 2 && d.TaskWBS1 == "ОС")
                    .ToList();
                int countElements = 2;
                int countManufacturingStep = 3;
                int countDevStep = 5;
                BlockProjectTasksState blockProjectTasksState = new BlockProjectTasksState(countElements);
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates = new ElementDataProjectTasksState[countManufacturingStep];
                blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates = new ElementDataProjectTasksState[countDevStep];
                blockProjectTasksState.ElementProjectTasksStates[0].Name = "Общая сборка - разработка";
                blockProjectTasksState.ElementProjectTasksStates[1].Name = "Общая сборка - производство";

                var ps = tasksLiat.First(d => d.TaskWBS2 == "ОСПВ");
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].Work = (double)ps.TaskWork;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].StartDate = ps.TaskStartDate;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].FinishDate = ps.TaskfinishDate;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].RemainingWork = (double)ps.TaskRemainingWork;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].Users = ps.AspNetUsers.CiliricalName;

                var sm = tasksLiat.First(d => d.TaskWBS2 == "*1СМ");
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].Work = (double)sm.TaskWork;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].StartDate = sm.TaskStartDate;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].FinishDate = sm.TaskfinishDate;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].RemainingWork = (double)sm.TaskRemainingWork;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].Users = sm.AspNetUsers.CiliricalName;

                var se = tasksLiat.First(d => d.TaskWBS2 == "*1СЭ");
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].Work = (double)se.TaskWork;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].StartDate = se.TaskStartDate;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].FinishDate = se.TaskfinishDate;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].RemainingWork = (double)se.TaskRemainingWork;
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].Users = se.AspNetUsers.CiliricalName;

                var listM = tasksLiat.Where(d => d.AspNetUsers.Devision == 15 && d.TaskWBS2 != "*1СМ").ToList();
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].Work = (double)listM.Sum(d => d.TaskWork);
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].StartDate = listM.Min(d => d.TaskStartDate);
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].FinishDate = listM.Max(d => d.TaskfinishDate);
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].RemainingWork = (double)listM.Sum(d => d.TaskRemainingWork);
                foreach(var user in listM)
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].Users += user.AspNetUsers.CiliricalName + "; ";
                }

                var listE = tasksLiat.Where(d => d.AspNetUsers.Devision == 3 || d.AspNetUsers.Devision == 16).Where(d => d.TaskWBS2 != "*1СЭ" && d.TaskWBS2 != "ОСПВ").ToList();
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].Work = (double)listM.Sum(d => d.TaskWork);
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].StartDate = listM.Min(d => d.TaskStartDate);
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].FinishDate = listM.Max(d => d.TaskfinishDate);
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].RemainingWork = (double)listM.Sum(d => d.TaskRemainingWork);
                foreach (var user in listM)
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].Users += user.AspNetUsers.CiliricalName + "; ";
                }

                return blockProjectTasksState;
            }
        }
    }
}