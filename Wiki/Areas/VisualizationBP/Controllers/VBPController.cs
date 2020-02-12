using System.Web.Mvc;
using System.Linq;
using System;
using System.Data.Entity;
using Newtonsoft.Json;
using Wiki.Models;
using System.Collections.Generic;
using Wiki.Areas.VisualizationBP.Types;
using Wiki.Areas.DashboardTV.Models;
using Wiki.Areas.DashboardKO.Models;

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
                    .Where(d => d.planWork > 0)
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
            try
            {
                int ids = Convert.ToInt32(id);
                int countBlocks = 5;
                ProjectTasksState projectTasksState = new ProjectTasksState(countBlocks);
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    int id_PZ = db.PZ_PlanZakaz.First(d => d.PlanZakaz == ids).Id;
                    var tasksList = db.DashboardBPTaskInsert.AsNoTracking().Include(d => d.AspNetUsers).Include(d => d.PZ_PlanZakaz).Where(d => d.PZ_PlanZakaz.PlanZakaz == ids).OrderBy(d => d.TaskIndex).ToList();
                    projectTasksState.BlockProjectTasksStates[0] = GetTasksStartBlock(tasksList.Where(d => d.TaskOutlineLevel == 1).ToList());
                    projectTasksState.BlockProjectTasksStates[2] = GetTasksPfBlock(tasksList.Where(d => d.TaskOutlineLevel == 2 || d.TaskOutlineLevel == 3).Where(d => d.TaskIsSummary == true).ToList());
                    projectTasksState.BlockProjectTasksStates[1] = GetTasksFinalBlock(id_PZ);
                    projectTasksState.BlockProjectTasksStates[3] = GetTasksDocBlock(id_PZ);
                    projectTasksState.BlockProjectTasksStates[4] = GetTasksShBlock(id_PZ);
                    return Json(new { projectTasksState });
                }
            }
            catch
            {
                return Json(0);
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
            blockProjectTasksState.ElementProjectTasksStates[0].Name = nameElement;
            for (int i = 0; i < countElementsTasks; i++)
            {
                try
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[i] = new ElementDataProjectTasksState(inputList.First(d => d.TaskWBS1 == wbsArray[i]));
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[i] = null;
                }
            }
            return blockProjectTasksState;
        }

        BlockProjectTasksState GetTasksPfBlock(List<DashboardBPTaskInsert> inputList)
        {
            int id = inputList[0].id_PZ_PlanZakaz;
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
                try
                {
                    elementsArray[i].taskName = inputList[i].TaskName.Substring(0, 28);
                }
                catch
                {
                    elementsArray[i].taskName = inputList[i].TaskName;
                }
                elementsArray[i].wbsName = inputList[i].TaskWBS2;
            }
            BlockProjectTasksState blockProjectTasksState = new BlockProjectTasksState(countElements);
            blockProjectTasksState.Name = "Сборочные единицы";
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
                            blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j] = null;
                            break;
                        default:
                            blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j] = null;
                            break;
                    }
                }
            }
            return blockProjectTasksState;
        }

        ElementDataProjectTasksState GetElementDataProjectTasksStateKBM(string wbs, int id)
        {
            DateTime defaulTime = new DateTime(1900, 1, 1);
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
                    return null;
                }
                try
                {
                    elementDataProjectTasksState.FinishDate = tasksList.Max(d => d.TaskfinishDate);
                }
                catch
                {
                    return null;
                }
                elementDataProjectTasksState.Work = tasksList.Sum(d => d.TaskWork.Value);
                elementDataProjectTasksState.RemainingWork = tasksList.Sum(d => d.TaskRemainingWork.Value);
                elementDataProjectTasksState.Users = "";
                foreach (var ciliricalName in tasksList.GroupBy(d => d.AspNetUsers.CiliricalName))
                {
                    elementDataProjectTasksState.Users += ciliricalName.Key.Substring(0, ciliricalName.Key.IndexOf(' ')) + "; ";
                }
                return elementDataProjectTasksState;
            }
        }

        ElementDataProjectTasksState GetElementDataProjectTasksStateKBE(string wbs, int id)
        {
            DateTime defaulTime = new DateTime(1900, 1, 1);
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
                    return null;
                }
                try
                {
                    elementDataProjectTasksState.FinishDate = tasksList.Max(d => d.TaskfinishDate);
                }
                catch
                {
                    return null;
                }
                elementDataProjectTasksState.Work = tasksList.Sum(d => d.TaskWork.Value);
                elementDataProjectTasksState.RemainingWork = tasksList.Sum(d => d.TaskRemainingWork.Value);
                elementDataProjectTasksState.Users = "";
                foreach (var ciliricalName in tasksList.GroupBy(d => d.AspNetUsers.CiliricalName))
                {
                    elementDataProjectTasksState.Users += ciliricalName.Key.Substring(0, ciliricalName.Key.IndexOf(' ')) + "; ";
                }
                return elementDataProjectTasksState;
            }
        }

        ElementDataProjectTasksState GetElementDataProjectTasksStateManufacturing(string wbs, int id)
        {
            DateTime defaulTime = new DateTime(1900, 1, 1);
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string userName = "";
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                ElementDataProjectTasksState elementDataProjectTasksState = new ElementDataProjectTasksState();
                var tasksList = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Where(d => d.TaskWBS2 == wbs && d.id_PZ_PlanZakaz == id)
                    .Where(d => d.AspNetUsers.Devision == 8 || d.AspNetUsers.Devision == 9 || d.AspNetUsers.Devision == 10 || d.AspNetUsers.Devision == 22 || d.AspNetUsers.Devision == 20)
                    .Include(d => d.AspNetUsers)
                    .ToList();
                elementDataProjectTasksState.Name = "Производство&nbsp;&nbsp;&nbsp;&nbsp;";
                try
                {
                    elementDataProjectTasksState.StartDate = tasksList.Min(d => d.TaskStartDate);
                }
                catch
                {
                    return null;
                }
                try
                {
                    elementDataProjectTasksState.FinishDate = tasksList.Max(d => d.TaskfinishDate);
                }
                catch
                {
                    return null;
                }
                elementDataProjectTasksState.Work = tasksList.Sum(d => d.TaskWork.Value);
                elementDataProjectTasksState.RemainingWork = tasksList.Sum(d => d.TaskRemainingWork.Value);
                elementDataProjectTasksState.Users = "";
                foreach (var ciliricalName in tasksList.GroupBy(d => d.AspNetUsers.CiliricalName))
                {
                    userName = ciliricalName.Key.Substring(0, ciliricalName.Key.IndexOf(' ')) + "; ";
                    if(userName != "Деменков; " & userName != "Ашанин; ")
                        elementDataProjectTasksState.Users += ciliricalName.Key.Substring(0, ciliricalName.Key.IndexOf(' ')) + "; ";
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
                    .Where(d => d.TaskIsSummary == false && d.TaskOutlineLevel == 2 && d.TaskWBS1 == "ОС" && d.id_PZ_PlanZakaz == id)
                    .ToList();
                int countElements = 2;
                int countManufacturingStep = 3;
                int countDevStep = 5;
                BlockProjectTasksState blockProjectTasksState = new BlockProjectTasksState(countElements);
                blockProjectTasksState.Name = "Общая сборка";
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates = new ElementDataProjectTasksState[countDevStep];
                blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates = new ElementDataProjectTasksState[countManufacturingStep];
                blockProjectTasksState.ElementProjectTasksStates[0].Name = "Общая сборка - разработка";
                blockProjectTasksState.ElementProjectTasksStates[1].Name = "Общая сборка - производство";
                try
                {
                    var ps = tasksLiat.First(d => d.TaskWBS2 == "ОСПВ");
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].Name = "Предв. ведомость&nbsp;";
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].Work = (double)ps.TaskWork;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].StartDate = ps.TaskStartDate;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].FinishDate = ps.TaskfinishDate;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].RemainingWork = (double)ps.TaskRemainingWork;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].Users = ps.AspNetUsers.CiliricalName;
                }
                catch
                {

                }
                var sm = tasksLiat.First(d => d.TaskWBS2 == "*1СМ");
                try
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].Name = "Ведомость 1с КБМ";
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].Work = (double)sm.TaskWork;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].StartDate = sm.TaskStartDate;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].FinishDate = sm.TaskfinishDate;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].RemainingWork = (double)sm.TaskRemainingWork;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].Users = sm.AspNetUsers.CiliricalName;
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1] = null;
                }
                try
                {
                    var se = tasksLiat.First(d => d.TaskWBS2 == "*1СЭ");
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].Name = "Ведомость 1с КБЭ";
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].Work = (double)se.TaskWork;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].StartDate = se.TaskStartDate;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].FinishDate = se.TaskfinishDate;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].RemainingWork = (double)se.TaskRemainingWork;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2].Users = se.AspNetUsers.CiliricalName;
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[2] = null;
                }
                try
                {
                    var listM = tasksLiat.Where(d => d.AspNetUsers.Devision == 15 && d.TaskWBS2 != "*1СМ").ToList();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].Name = "Общая сборка КБМ";
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].Work = (double)listM.Sum(d => d.TaskWork);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].StartDate = listM.Min(d => d.TaskStartDate);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].FinishDate = listM.Max(d => d.TaskfinishDate);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].RemainingWork = (double)listM.Sum(d => d.TaskRemainingWork);
                    foreach (var user in listM.GroupBy(d => d.AspNetUsers.CiliricalName))
                    {
                        blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3].Users += user.Key + "; ";
                    }
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[3] = null;
                }
                try
                {
                    var listE = tasksLiat.Where(d => d.AspNetUsers.Devision == 3 || d.AspNetUsers.Devision == 16).Where(d => d.TaskWBS2 != "*1СЭ" && d.TaskWBS2 != "ОСПВ").ToList();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[4] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[4].Name = "Общая сборка КБЭ";
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[4].Work = (double)listE.Sum(d => d.TaskWork);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[4].StartDate = listE.Min(d => d.TaskStartDate);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[4].FinishDate = listE.Max(d => d.TaskfinishDate);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[4].RemainingWork = (double)listE.Sum(d => d.TaskRemainingWork);
                    foreach (var user in listE.GroupBy(d => d.AspNetUsers.CiliricalName))
                    {
                        blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[4].Users += user.Key + "; ";
                    }
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[4] = null;
                }
                try
                {
                    var sn = tasksLiat.First(d => d.TaskWBS2 == "*МСН");
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[0] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[0].Name = "Монтаж СН&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[0].Work = (double)sn.TaskWork;
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[0].StartDate = sn.TaskStartDate;
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[0].FinishDate = sn.TaskfinishDate;
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[0].RemainingWork = (double)sn.TaskRemainingWork;
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[0].Users = sn.AspNetUsers.CiliricalName;
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[0] = null;
                }
                try
                {
                    var listu = tasksLiat.Where(d => d.AspNetUsers.Devision == 9).ToList();
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[1] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[1].Name = "Общая сборка - УСШ";
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[1].Work = (double)listu.Sum(d => d.TaskWork);
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[1].StartDate = listu.Min(d => d.TaskStartDate);
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[1].FinishDate = listu.Max(d => d.TaskfinishDate);
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[1].RemainingWork = (double)listu.Sum(d => d.TaskRemainingWork);
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[1] = null;
                }
                try
                {
                    var liste = tasksLiat.Where(d => d.AspNetUsers.Devision == 10 && d.TaskWBS2 != "*МСН").ToList();
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[2] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[2].Name = "Монтаж ВЦ";
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[2].Work = (double)liste.Sum(d => d.TaskWork);
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[2].StartDate = liste.Min(d => d.TaskStartDate);
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[2].FinishDate = liste.Max(d => d.TaskfinishDate);
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[2].RemainingWork = (double)liste.Sum(d => d.TaskRemainingWork);
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[1].ElementDataProjectTasksStates[2] = null;
                }
                return blockProjectTasksState;
            }
        }

        BlockProjectTasksState GetTasksDocBlock(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int countElements = 1;
                int countDevStep = 2;
                BlockProjectTasksState blockProjectTasksState = new BlockProjectTasksState(countElements);
                blockProjectTasksState.Name = "ЭД и программирование";
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates = new ElementDataProjectTasksState[countDevStep];
                blockProjectTasksState.ElementProjectTasksStates[0].Name = "ЭД и программирование";
                try
                {
                    var listM = db.DashboardBPTaskInsert
                        .AsNoTracking()
                        .Include(d => d.AspNetUsers)
                        .Where(d => d.TaskIsSummary == false && d.TaskWBS1 == "ЭД" && d.id_PZ_PlanZakaz == id)
                        .Where(d => d.AspNetUsers.Devision == 15)
                        .ToList();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].Name = "РЭ и паклист&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;";
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].Work = (double)listM.Sum(d => d.TaskWork);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].StartDate = listM.Min(d => d.TaskStartDate);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].FinishDate = listM.Max(d => d.TaskfinishDate);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].RemainingWork = (double)listM.Sum(d => d.TaskRemainingWork);
                    foreach (var user in listM.GroupBy(d => d.AspNetUsers.CiliricalName))
                    {
                        blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].Users += user.Key + "; ";
                    }
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0] = null;
                }
                try
                {
                    var listE = db.DashboardBPTaskInsert
                        .AsNoTracking()
                        .Include(d => d.AspNetUsers)
                        .Where(d => d.TaskIsSummary == false && d.TaskWBS1 == "ЭД" && d.id_PZ_PlanZakaz == id)
                        .Where(d => d.AspNetUsers.Devision == 3 || d.AspNetUsers.Devision == 16)
                        .ToList();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].Name = "ЭД и программир.";
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].Work = (double)listE.Sum(d => d.TaskWork);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].StartDate = listE.Min(d => d.TaskStartDate);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].FinishDate = listE.Max(d => d.TaskfinishDate);
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].RemainingWork = (double)listE.Sum(d => d.TaskRemainingWork);
                    foreach (var user in listE.GroupBy(d => d.AspNetUsers.CiliricalName))
                    {
                        blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1].Users += user.Key + "; ";
                    }
                }
                catch
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[1] = null;
                }
                return blockProjectTasksState;
            }
        }

        BlockProjectTasksState GetTasksShBlock(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var tasksLiat = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.TaskIsSummary == false && d.TaskWBS1 == "ОТ" && d.id_PZ_PlanZakaz == id)
                    .ToList();
                int countElements = 1;
                int countDevStep = 1;
                BlockProjectTasksState blockProjectTasksState = new BlockProjectTasksState(countElements);
                blockProjectTasksState.Name = "Отгрузка";
                blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates = new ElementDataProjectTasksState[countDevStep];
                blockProjectTasksState.ElementProjectTasksStates[0].Name = "Отгрузка";
                var sm = tasksLiat.First();
                try
                {
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0] = new ElementDataProjectTasksState();
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].FinishDate = sm.TaskfinishDate;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].StartDate = sm.TaskfinishDate;
                    blockProjectTasksState.ElementProjectTasksStates[0].ElementDataProjectTasksStates[0].Name = "Отгрузка&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                catch
                {

                }
                return blockProjectTasksState;
            }
        }

        [HttpPost]
        public JsonResult GetPrjContractDate(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_PlanZakaz
                    .AsNoTracking()
                    .Where(d => d.PlanZakaz == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    orderNumber = dataList.PlanZakaz,
                    prjContractName = dataList.Name,
                    prjName = dataList.nameTU,
                    prjContractDateSh = dataList.DateShipping.ToString().Substring(0, 10),
                    prjDateSh = dataList.dataOtgruzkiBP.ToString().Substring(0, 10),
                    prjShState = GetTimeSpan(dataList.DateShipping, dataList.dataOtgruzkiBP).Days
                }); 
                return Json(new { data });
            }
        }

        TimeSpan GetTimeSpan(DateTime dateSh, DateTime dateManuf)
        {
            TimeSpan timeSpan = dateSh - dateManuf;
            return timeSpan;
        }

        [HttpPost]
        public JsonResult GetPercentDevisionComplited(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int countDevision = 3;
                DiagrammPercentComplitedDevisionToWork[] devisionsArray = new DiagrammPercentComplitedDevisionToWork[countDevision];
                devisionsArray[0] = GetDevisionMPercentComplited(id);
                devisionsArray[1] = GetDevisionEPercentComplited(id);
                devisionsArray[2] = GetDevisionManufPercentComplited(id);

                return Json(new { devisionsArray });
            }
        }

        DiagrammPercentComplitedDevisionToWork GetDevisionMPercentComplited(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var tasksList = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.PZ_PlanZakaz.PlanZakaz == id && d.TaskWBS1 == "ОС" && d.AspNetUsers.Devision == 15)
                    .ToList();

                double work = tasksList.Sum(d => d.TaskWork).Value;
                double remainingWork = tasksList.Sum(d => d.TaskRemainingWork).Value;

                DiagrammPercentComplitedDevisionToWork diagrammPercentComplitedDevisionToWork = new DiagrammPercentComplitedDevisionToWork("КБМ");
                diagrammPercentComplitedDevisionToWork.PercentComplited = (int)(((work - remainingWork) / work) * 100.0);
                diagrammPercentComplitedDevisionToWork.PercentRemainingWork = 100 - diagrammPercentComplitedDevisionToWork.PercentComplited;

                return diagrammPercentComplitedDevisionToWork;
            }
        }

        DiagrammPercentComplitedDevisionToWork GetDevisionEPercentComplited(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var tasksList = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.PZ_PlanZakaz.PlanZakaz == id && d.TaskWBS1 == "ОС")
                    .Where(d => d.AspNetUsers.Devision == 16 || d.AspNetUsers.Devision == 3)
                    .ToList();

                double work = tasksList.Sum(d => d.TaskWork).Value;
                double remainingWork = tasksList.Sum(d => d.TaskRemainingWork).Value;

                DiagrammPercentComplitedDevisionToWork diagrammPercentComplitedDevisionToWork = new DiagrammPercentComplitedDevisionToWork("КБЭ");
                diagrammPercentComplitedDevisionToWork.PercentComplited = (int)(((work - remainingWork) / work) * 100.0);
                diagrammPercentComplitedDevisionToWork.PercentRemainingWork = 100 - diagrammPercentComplitedDevisionToWork.PercentComplited;

                return diagrammPercentComplitedDevisionToWork;
            }
        }

        DiagrammPercentComplitedDevisionToWork GetDevisionManufPercentComplited(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var tasksList = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.PZ_PlanZakaz.PlanZakaz == id && d.TaskWBS1 == "ОС")
                    .Where(d => d.AspNetUsers.Devision == 8 || d.AspNetUsers.Devision == 9
                     || d.AspNetUsers.Devision == 10 || d.AspNetUsers.Devision == 20 || d.AspNetUsers.Devision == 22)
                    .ToList();

                double work = tasksList.Sum(d => d.TaskWork).Value;
                double remainingWork = tasksList.Sum(d => d.TaskRemainingWork).Value;

                DiagrammPercentComplitedDevisionToWork diagrammPercentComplitedDevisionToWork = new DiagrammPercentComplitedDevisionToWork("ПО");
                diagrammPercentComplitedDevisionToWork.PercentComplited = (int)(((work - remainingWork) / work) * 100.0);
                diagrammPercentComplitedDevisionToWork.PercentRemainingWork = 100 - diagrammPercentComplitedDevisionToWork.PercentComplited;

                return diagrammPercentComplitedDevisionToWork;
            }
        }

        public JsonResult GetCriticalRoadGanttToOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var projectList = db.DashboardBPTaskInsert
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers.Devision1)
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.PZ_PlanZakaz.PlanZakaz == id)
                    .Where(d => d.TaskIsCritical == true && d.id_AspNetUsers != null)
                    .OrderBy(d => d.TaskfinishDate)
                    .ToList();
                DateTime minDate = projectList.Min(a => a.TaskStartDate);
                DateTime maxDate = projectList.Max(a => a.TaskfinishDate);
                int countMonthDifferent = GetMinthDifferent(maxDate, minDate);
                OrderForDashboardTV[] dataList = new OrderForDashboardTV[projectList.Count];
                for (int i = 0; i < projectList.Count; i++)
                {
                    dataList[i] = new OrderForDashboardTV();
                    dataList[i].PercentComplited = projectList[i].TaskPercentWorkCompleted;
                    dataList[i].RemainingDuration = projectList[i].TaskRemainingWork.Value;
                    dataList[i].DataOtgruzkiBP = projectList[i].TaskStartDate;
                    dataList[i].ContractDateComplited = projectList[i].TaskfinishDate;
                    dataList[i].OrderNumber = projectList[i].AspNetUsers.Devision1.name + " | " + projectList[i].AspNetUsers.CiliricalName;
                    dataList[i].TaskName = projectList[i].TaskName;
                    dataList[i].Current = 0;
                    dataList[i].Color = "#910000";
                    dataList[i].Milestone = false;
                    dataList[i].Deals = new DealsForDashboardTV[1];
                    DealsForDashboardTV dealsForDashboardTV = new DealsForDashboardTV
                    {
                        User = projectList[i].AspNetUsers.CiliricalName,
                        From = projectList[i].TaskStartDate,
                        To = projectList[i].TaskfinishDate,
                        Milestone = false,
                        Color = "#910000"
                    };
                    dataList[i].Deals[0] = dealsForDashboardTV;
                }
                return Json(dataList.OrderBy(d => d.ContractDateComplited), JsonRequestBehavior.AllowGet);
            }
        }

        private int GetMinthDifferent(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }
        
        public JsonResult GetHSSPO()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOHssPO
                    .AsNoTracking()
                    .OrderBy(d => d.quart)
                    .ToList();
                int maxCounterValue = query.Count();
                UserResultWithDevision[] data = new UserResultWithDevision[maxCounterValue];
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i] = new UserResultWithDevision();
                }
                for (int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].quart;
                    data[i].count = (int)query[i].hss;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
