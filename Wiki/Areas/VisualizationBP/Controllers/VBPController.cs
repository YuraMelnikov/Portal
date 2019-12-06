﻿using System.Web.Mvc;
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
                //01 - startBlock
                //02 - pBlock
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
            List<DashboardBPTaskInsert> outputList = new List<DashboardBPTaskInsert>();

            const string wbsPreDevM = "ПР";
            const string wbsPreDevE = "ПЭ";
            const string wbsRDevM = "РМ";
            const string wbsRdevE = "РЭ";
            const string wbsApprovdM = "СМ";
            const string wbsApprovdE = "СЭ";



            return inputList;
        }

        DashboardBPTaskInsert GetDashboardBPTaskInsert(string wbsName, List<DashboardBPTaskInsert> inputList)
        {
            DashboardBPTaskInsert dashboardBPTaskInsert;
            try
            {
                dashboardBPTaskInsert = inputList.First(d => d.TaskWBS == wbsName);
            }
            catch
            {
                return new DashboardBPTaskInsert();
            }
        }
    }
}