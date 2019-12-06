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

        [HttpPost]
        public JsonResult GetProjectTasksStates(int id)
        {
            int countBlocks = 5;
            ProjectTasksState projectTasksState = new ProjectTasksState(countBlocks);
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var tasksList = db.DashboardBPTaskInsert.AsNoTracking().Include(d => d.AspNetUsers).Where(d => d.id_PZ_PlanZakaz == id).OrderBy(d => d.TaskIndex).ToList();
                projectTasksState.BlockProjectTasksStates[0] = GetTasksStartBlock(tasksList.Where(d => d.TaskOutlineLevel == 1).ToList());
                projectTasksState.BlockProjectTasksStates[1] = GetTasksPfBlock(tasksList.Where(d => d.TaskOutlineLevel == 2 || d.TaskOutlineLevel == 3).Where(d => d.TaskIsSummary == true).ToList());
                //03 - finalBlock
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
            for(int i = 0; i < countElementsTasks; i++)
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
            int countElements = 0;
            int countElementsTasks = 4;

            foreach (var data in inputList)
            {
                if(data.TaskWBS1 != "ОС")
                    break;
                countElements++;
            }
            string[] elementsArray = new string[countElements];
            for(int i = 0; i < countElements; i++)
            {
                elementsArray[i] = inputList[i].TaskName;
            }
            BlockProjectTasksState blockProjectTasksState = new BlockProjectTasksState(countElements);
            for (int i = 0; i < countElements; i++)
            {
                blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates =
                    new ElementDataProjectTasksState[countElementsTasks];
            }
            for (int i = 0; i < countElements; i++)
            {
                blockProjectTasksState.ElementProjectTasksStates[i].Name = elementsArray[i];
            }
            for(int i = 0; i < countElements; i++)
            {
                for(int j = 0; j < countElementsTasks; j++)
                {
                    switch (j)
                    {
                        case 0:
                            blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Name = "Согласование";
                            break;
                        case 1:
                            blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Name = "Разработка";
                            break;
                        case 2:
                            blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Name = "Комплектация";
                            break;
                        case 3:
                            blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Name = "Производство";
                            break;
                        default:
                            blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[j].Name = "";
                            break;
                    }
                }
            }





            //for (int i = 0; i < countElementsTasks; i++)
            //{
            //    blockProjectTasksState.ElementProjectTasksStates[i].ElementDataProjectTasksStates[i] = new ElementDataProjectTasksState(inputList.First(d => d.TaskName == wbsArray[i]));
            //}
            return blockProjectTasksState;
        }
    }
}