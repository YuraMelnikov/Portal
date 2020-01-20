using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json;
using NLog;
using Wiki.Areas.ApproveCD.Models;

namespace Wiki.Areas.ApproveCD.Controllers
{
    public class ApproveController : Controller
    {
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly JsonSerializerSettings longUsSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd HH:mm" };
        readonly JsonSerializerSettings longSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy HH:mm" };

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            @ViewBag.LeavelUser = GetUserLeavel(login);
            return View();
        }

        private int GetUserLeavel(string login)
        {
            int devision = GetDevisionId(login);
            if (devision == 3 || devision == 15 || devision == 16)
                return 1;
            if (devision == 4)
                return 2;
            return 3;
        }

        [HttpPost]
        public JsonResult GetNoApproveTable()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.ApproveCDVersions
                        .Include(a => a.ApproveCDOrders.PZ_PlanZakaz.PZ_Client)
                        .Include(a => a.ApproveCDOrders.AspNetUsers)
                        .Include(a => a.ApproveCDOrders.AspNetUsers1)
                        .Include(a => a.RKD_VersionWork)
                        .Include(a => a.RKD_VersionWork)
                        .Where(a => a.activeVersion == true && a.id_RKD_VersionWork != 10)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrderByIdForView('" + dataList.ApproveCDOrders.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        editLink = GetEditOrderLink(login, dataList.id_RKD_VersionWork, dataList.ApproveCDOrders.id),
                        order = dataList.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz,
                        state = dataList.RKD_VersionWork.name,
                        gm = dataList.ApproveCDOrders.AspNetUsers.CiliricalName,
                        ge = dataList.ApproveCDOrders.AspNetUsers1.CiliricalName,
                        customer = dataList.ApproveCDOrders.PZ_PlanZakaz.PZ_Client.NameSort,
                        dateOpen = JsonConvert.SerializeObject(dataList.ApproveCDOrders.PZ_PlanZakaz.DateCreate, shortSetting).Replace(@"""", ""),
                        contractDate = JsonConvert.SerializeObject(dataList.ApproveCDOrders.PZ_PlanZakaz.DateShipping, shortSetting).Replace(@"""", ""),
                        ver = "v." + dataList.numberVersion1 + "." + dataList.numberVersion2
                    });
                    return Json(new { data });
                }
            }
            catch(Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.GetNoApproveTable: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetApproveTable()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.ApproveCDVersions
                        .Include(a => a.ApproveCDOrders.PZ_PlanZakaz.PZ_Client)
                        .Include(a => a.ApproveCDOrders.AspNetUsers)
                        .Include(a => a.ApproveCDOrders.AspNetUsers1)
                        .Include(a => a.RKD_VersionWork)
                        .Include(a => a.RKD_VersionWork)
                        .Where(a => a.activeVersion == true && a.id_RKD_VersionWork == 10)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrderByIdForView('" + dataList.ApproveCDOrders.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        editLink = "",
                        order = dataList.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz,
                        state = dataList.RKD_VersionWork.name,
                        gm = dataList.ApproveCDOrders.AspNetUsers.CiliricalName,
                        ge = dataList.ApproveCDOrders.AspNetUsers1.CiliricalName,
                        customer = dataList.ApproveCDOrders.PZ_PlanZakaz.PZ_Client.NameSort,
                        dateOpen = JsonConvert.SerializeObject(dataList.ApproveCDOrders.PZ_PlanZakaz.DateCreate, shortSetting).Replace(@"""", ""),
                        contractDate = JsonConvert.SerializeObject(dataList.ApproveCDOrders.PZ_PlanZakaz.DateShipping, shortSetting).Replace(@"""", ""),
                        ver = "v." + dataList.numberVersion1 + "." + dataList.numberVersion2
                    });
                    return Json(new { data });
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.GetApproveTable: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetEditOrderLink(string login, int stateId, int idOrder)
        {
            try
            {
                int devision = GetDevisionId(login);
                if(devision == 3 || devision == 15 || devision == 16)
                {
                    if (stateId == 4 || stateId == 8 || stateId == 12)
                    {
                        return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrderByIdForEdit('" + idOrder + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
                    }
                }
                if(devision == 4)
                {
                    if (stateId == 2 || stateId == 5 || stateId == 16)
                    {
                        return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrderByIdForEdit('" + idOrder + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        private int GetDevisionId(string login)
        {
            int devision = 0;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    devision = db.AspNetUsers.First(a => a.Email == login).Devision.Value;
                    if (login == "myi@katek.by")
                        devision = 3;
                }
                catch
                {

                }
            }
            return devision;
        }

        [HttpPost]
        public JsonResult GetNotCloseQuestionsTable()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.ApproveCDQuestions
                        .Include(a => a.ApproveCDOrders.PZ_PlanZakaz)
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.active == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetQuestionByIdForView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        editLink = GetEditQueLink(login, dataList.id),
                        order = dataList.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz,
                        idQue = dataList.id,
                        que = dataList.textQuestion,
                        queData = GetQuestionData(dataList.id),
                        createDate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        createUser = dataList.AspNetUsers.CiliricalName
                        });
                    return Json(new { data });
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.GetNotCloseQuestionsTable: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCloseQuestionsTable()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.ApproveCDQuestions
                        .Include(a => a.ApproveCDOrders.PZ_PlanZakaz)
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.active == true)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetQuestionByIdForView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        editLink = GetEditQueLink(login, dataList.id),
                        order = dataList.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz,
                        idQue = dataList.id,
                        que = dataList.textQuestion,
                        queData = GetQuestionData(dataList.id),
                        createDate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        createUser = dataList.AspNetUsers.CiliricalName
                    });
                    return Json(new { data });
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.GetCloseQuestionsTable: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        private string GetEditQueLink(string login, int idOrder)
        {
            try
            {
                int devision = GetDevisionId(login);
                if (devision == 3 || devision == 15 || devision == 16 || devision == 4)
                {
                    return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrderByIdForEdit('" + idOrder + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        private string GetQuestionData(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    string textData = "";
                    var dataList = db.ApproveCDQuestionCorr
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.id_ApproveCDQuestions == id)
                        .OrderByDescending(a => a.datetimeCreate)
                        .ToList();
                    foreach (var data in dataList)
                    {
                        textData += data.datetimeCreate.ToString().Substring(0, 10) + " | " + data.AspNetUsers.CiliricalName + " | " + data.textData + "</br>";
                    }
                    return textData;
                }
            }
            catch
            {
                return "";
            }
        }

        public JsonResult GetTasksTable()
        {
            try
            {
                int minDays = -60;
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    List<TaskApproveCD> tasksList = new List<TaskApproveCD>();
                    DateTime dateFilt = DateTime.Now.AddDays(minDays);
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var tasks = db.ApproveCDTasks
                        .Include(a => a.ApproveCDOrders.PZ_PlanZakaz)
                        .Where(a => a.dateEvent > dateFilt)
                        .ToList();
                    foreach (var dataInList in tasks)
                    {
                        TaskApproveCD taskApproveCD = new TaskApproveCD(dataInList.dateEvent, dataInList.text,
                            GetUserName(dataInList.id_AspNetUsers), dataInList.deadline, 
                            dataInList.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz.ToString(), 1);
                        tasksList.Add(taskApproveCD);
                    }
                    var questions = db.ApproveCDQuestions
                        .Include(a => a.AspNetUsers)
                        .Include(a => a.ApproveCDOrders.PZ_PlanZakaz)
                        .Where(a => a.dateTimeCreate > dateFilt)
                        .ToList();
                    foreach (var dataInList in questions)
                    {
                        TaskApproveCD taskApproveCD = new TaskApproveCD(dataInList.dateTimeCreate, GetQuestionText(dataInList.id) + GetQuestionData(dataInList.id),
                            null, null,
                            dataInList.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz.ToString(), 2);
                        tasksList.Add(taskApproveCD);
                    }
                    var data = tasksList.Select(dataList => new
                    {
                        dateAction = JsonConvert.SerializeObject(dataList.dateTime, shortSetting).Replace(@"""", ""),
                        dataList.order,
                        dataList.action,
                        dataList.user,
                        deadline = JsonConvert.SerializeObject(dataList.deadline, shortSetting).Replace(@"""", ""),
                        dataList.typeTask
                    });
                    return Json(new { data });
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.GetTasksTable: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetUserName(string id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    return db.AspNetUsers.First(a => a.Id == id).CiliricalName;
                }
            }
            catch
            {
                return "";
            }
        } 

        private string GetQuestionText(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    string textData = "";
                    var data = db.ApproveCDQuestions
                        .Include(a => a.AspNetUsers)
                        .First(a => a.id == id);
                    return textData += data.dateTimeCreate.ToString().Substring(0, 10) + " | " + data.AspNetUsers.CiliricalName + " | " + data.textQuestion + "</br>";
                }
            }
            catch
            {
                return "";
            }
        }

        public string RenderUserMenu()
        {
            string login = "Войти";
            try
            {
                if (HttpContext.User.Identity.Name != "")
                    login = HttpContext.User.Identity.Name;
            }
            catch
            {
                login = "Войти";
            }
            return login;
        }

        //GetOrderById
    }
}