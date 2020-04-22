using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json;
using NLog;
using Wiki.Areas.ApproveCD.Models;
using System.Security.Cryptography;

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
            PortalKATEKEntities db = new PortalKATEKEntities();
            string login = HttpContext.User.Identity.Name;
            @ViewBag.LeavelUser = GetUserLeavel(login);
            @ViewBag.NewOrders = new SelectList(db.PZ_PlanZakaz
                .Include(d => d.ApproveCDOrders)
                .Where(d => d.dataOtgruzkiBP > DateTime.Now && d.ApproveCDOrders.Count == 0)
                .OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.OrdersForQuestion = new SelectList(db.PZ_PlanZakaz
                .Include(d => d.ApproveCDOrders)
                .Where(d => d.dataOtgruzkiBP > DateTime.Now && d.ApproveCDOrders.Count != 0)
                .OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.OrdersForTask = new SelectList(db.PZ_PlanZakaz
                .Include(d => d.ApproveCDOrders)
                .Where(d => d.dataOtgruzkiBP > DateTime.Now && d.ApproveCDOrders.Count != 0)
                .OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.ASPUsers = new SelectList(db.AspNetUsers
                .Where(a => a.LockoutEnabled == true)
                .Where(a => a.Devision == 3 || a.Devision == 15 || a.Devision == 16 || a.Email == "bav@katek.by" || a.Email == "maj@katek.by")
                .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            return View();
        }

        private int GetUserLeavel(string login)
        {
            int devision = GetDevisionId(login);
            if (login == "myi@katek.by" || login == "bav@katek.by")
                return 4;
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
                        ver = "v." + dataList.numberVersion1 + "." + dataList.numberVersion2,
                        dateLastLoad = JsonConvert.SerializeObject(GetDateLastUpload(dataList.id_ApproveCDOrders), shortSetting).Replace(@"""", ""),
                        dataList.ApproveCDOrders.description
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

        private DateTime? GetDateLastUpload(int idOrder)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var list = db.ApproveCDActions
                    .Where(a => a.ApproveCDVersions.id_ApproveCDOrders == idOrder && a.id_RKD_VersionWork == 4)
                    .OrderByDescending(a => a.datetime)
                    .ToList();
                try
                {
                    return list[0].datetime;
                }
                catch
                {
                    return null;
                }
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
                        ver = "v." + dataList.numberVersion1 + "." + dataList.numberVersion2,
                        dateLastLoad = JsonConvert.SerializeObject(GetDateLastUpload(dataList.id_ApproveCDOrders), shortSetting).Replace(@"""", ""),
                        dataList.ApproveCDOrders.description
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
                if(login == "myi@katek.by")
                {
                    return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrderByIdForEdit('" + idOrder + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
                }
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
        private string GetEditQueLink(string login, int idQue)
        {
            try
            {
                int devision = GetDevisionId(login);
                if (devision == 3 || devision == 15 || devision == 16 || devision == 4 || login == "myi@katek.by")
                {
                    return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetQuestionByIdForEdit('" + idQue + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
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
                int minDays = -32;
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
                    var actionsList = db.ApproveCDActions
                        .Include(a => a.AspNetUsers)
                        .Include(a => a.ApproveCDVersions.ApproveCDOrders.PZ_PlanZakaz)
                        .Include(a => a.TypeRKD_Mail_Version)
                        .Where(a => a.datetime > dateFilt)
                        .ToList();
                    foreach (var dataInList in actionsList)
                    {
                        TaskApproveCD taskApproveCD = new TaskApproveCD(dataInList.datetime, dataInList.TypeRKD_Mail_Version.name,
                            null, null,
                            dataInList.ApproveCDVersions.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz.ToString(), 3);
                        tasksList.Add(taskApproveCD);
                    }
                    var data = tasksList.Select(dataList => new
                    {
                        dateAction = JsonConvert.SerializeObject(dataList.dateTime, longUsSetting).Replace(@"""", ""),
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

        public JsonResult AddOrders(int[] newOrders)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    if(newOrders.Length > 0)
                    {
                        foreach (var data in newOrders)
                        {
                            ApproveCDOrders approveCDOrders = new ApproveCDOrders
                            {
                                id_PZ_PlanZakaz = data,
                                id_AspNetUsersM = "4f91324a-1918-4e62-b664-d8cd89a19d95",
                                id_AspNetUsersE = "8363828f-bba2-4a89-8ed8-d7f5623b4fa8",
                                description = ""
                            };
                            db.ApproveCDOrders.Add(approveCDOrders);
                            db.SaveChanges();
                            ApproveCDVersions approveCDVersions = new ApproveCDVersions
                            {
                                id_ApproveCDOrders = approveCDOrders.id,
                                id_RKD_VersionWork = 12,
                                numberVersion1 = 0,
                                numberVersion2 = 0,
                                activeVersion = true
                            };
                            db.ApproveCDVersions.Add(approveCDVersions);
                            db.SaveChanges();
                        }
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.AddOrders: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddQuestion(string question, int orderIdForQuestion)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    ApproveCDQuestions approveCDQuestions = new ApproveCDQuestions
                    {
                        id_ApproveCDOrders = db.ApproveCDOrders.First(d => d.id_PZ_PlanZakaz == orderIdForQuestion).id,
                        dateTimeCreate = DateTime.Now,
                        id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                        textQuestion = question,
                        active = false
                    };
                    db.ApproveCDQuestions.Add(approveCDQuestions);
                    db.SaveChanges();
                    EmailApproveCDQue emailApproveCDQue = new EmailApproveCDQue(approveCDQuestions.id, login);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.AddQuestion: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddTask(int ordersForTask, string aSPUsers,
            DateTime? deadline, string taskData)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    ApproveCDTasks approveCDTasks = new ApproveCDTasks
                    {
                        id_ApproveCDOrders = db.ApproveCDOrders.First(a => a.id_PZ_PlanZakaz == ordersForTask).id,
                        id_AspNetUsers = aSPUsers,
                        text = taskData,
                        deadline = deadline,
                        dateEvent = DateTime.Now
                    };
                    db.ApproveCDTasks.Add(approveCDTasks);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.AddTask: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetQuestionById(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.ApproveCDQuestions
                        .Include(a => a.ApproveCDOrders.PZ_PlanZakaz)
                        .Include(a => a.ApproveCDQuestionCorr.Select(b => b.AspNetUsers))
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.id == id)
                        .ToList();

                    var data = query.Select(dataList => new
                    {
                        idQue = dataList.id,
                        orderQue = dataList.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz,
                        dateCreateQue = dataList.dateTimeCreate.ToString().Substring(0, 10),
                        autorQue = dataList.AspNetUsers.CiliricalName,
                        histQue = GetQuestionData(dataList.id).Replace("</br>","\n"),
                        questionTextU = dataList.textQuestion
                    });
                    return Json(data.First(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.GetQuestionById: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateQuestion(int idQue, string commitQue)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    ApproveCDQuestionCorr approveCDQuestionCorr = new ApproveCDQuestionCorr
                    {
                        datetimeCreate = DateTime.Now,
                        id_ApproveCDQuestions = idQue,
                        id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                        textData = commitQue
                    };
                    db.ApproveCDQuestionCorr.Add(approveCDQuestionCorr);
                    db.SaveChanges();
                    EmailApproveCDQue emailApproveCDQue = new EmailApproveCDQue(idQue, login);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.UpdateQuestion: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetOrderById(int id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.ApproveCDOrders
                        .Include(a => a.PZ_PlanZakaz.PZ_Client)
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.id == id)
                        .ToList();

                    var data = query.Select(dataList => new
                    {
                        hideIdOrder = dataList.id,
                        descriptionOrder = GetDescriptionOrder(dataList),
                        numVerCD = GetNumVer(dataList.id),
                        showAction = GetShowAction(dataList.id, login)
                    }); 
                    return Json(data.First(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.GetOrderById: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetNumVer(int idOrder)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var ver = db.ApproveCDVersions.First(a => a.activeVersion == true && a.id_ApproveCDOrders == idOrder);
                if (ver.id_RKD_VersionWork == 4)
                {
                    return ver.numberVersion1 + "." + ver.numberVersion2;
                }
                else
                {
                    short num1 = 0;
                    if (ver.numberVersion1 == 0)
                        num1 = 1;
                    else
                        num1 = ver.numberVersion1;
                    int num2 = 0;
                    if (ver.numberVersion1 == 0)
                        num2 = 0;
                    else
                        num2 = ver.numberVersion2 + 1;
                    return num1 + "." + num2;
                }
            }
        }

        private string GetDescriptionOrder(ApproveCDOrders approveCDOrders)
        {
            string description = "№ заказа: " + approveCDOrders.PZ_PlanZakaz.PlanZakaz.ToString() + "\n";
            description += "Заказчик: " + approveCDOrders.PZ_PlanZakaz.PZ_Client.NameSort + "\n";
            description += "Контрактное наименование: " + approveCDOrders.PZ_PlanZakaz.Name + "\n";
            description += "Наименование по ТУ: " + approveCDOrders.PZ_PlanZakaz.nameTU + "\n";
            description += "Статус согласования: " + GetStateName(approveCDOrders.id);
            return description;
        }

        private string GetStateName(int idOrder)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                return db.ApproveCDVersions.First(a => a.activeVersion == true && a.id_ApproveCDOrders == idOrder).RKD_VersionWork.name;
            }
        }

        private string GetShowAction(int idOrder, string login)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                int idState = db.ApproveCDVersions.First(a => a.activeVersion == true && a.id_ApproveCDOrders == idOrder).id_RKD_VersionWork;
                int devision = GetDevisionId(login);
                if (login == "myi@katek.by")
                {
                    if (idState == 4 || idState == 8 || idState == 12)
                    {
                        return "3";
                    }
                    if (idState == 5)
                    {
                        return "5";
                    }
                    if (idState == 2)
                    {
                        return "4";
                    }
                }
                if (devision == 3 || devision == 15 || devision == 16)
                {
                    if (idState == 4 || idState == 8 || idState == 12)
                    {
                        return "3";
                    }
                }
                if (devision == 4)
                {
                    if (idState == 5)
                    {
                        return "5";
                    }
                    if (idState == 2)
                    {
                        return "4";
                    }
                }
                return "";
            }
        }

        [HttpPost]
        public JsonResult UpdateOrderLoadVer(string hideIdOrder, string numVerCD, string linkKD)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                int numhideIdOrder = Convert.ToInt32(hideIdOrder);
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    short num1 = (short)Convert.ToInt32(numVerCD.Substring(0, numVerCD.IndexOf('.')));
                    short num2 = (short)Convert.ToInt32(numVerCD.Substring(numVerCD.IndexOf('.') + 1, numVerCD.Length - numVerCD.IndexOf('.') - 1));
                    ApproveCDVersions approveCDVersions = db.ApproveCDVersions
                        .First(a => a.activeVersion == true && a.id_ApproveCDOrders == numhideIdOrder);
                    if(approveCDVersions.numberVersion1 != num1 || approveCDVersions.numberVersion2 != num2)
                    {
                        ApproveCDVersions newApproveCDVersions = new ApproveCDVersions
                        {
                            id_ApproveCDOrders = numhideIdOrder,
                            id_RKD_VersionWork = 2,
                            numberVersion1 = num1,
                            numberVersion2 = num2,
                            activeVersion = true
                        };
                        db.ApproveCDVersions.Add(newApproveCDVersions);
                        approveCDVersions.activeVersion = false;
                        db.Entry(approveCDVersions).State = EntityState.Modified;
                        db.SaveChanges();
                        ApproveCDActions approveCDActions = new ApproveCDActions
                        {
                            id_ApproveCDVersions = newApproveCDVersions.id,
                            datetime = DateTime.Now,
                            id_AspNetUsers = db.AspNetUsers.First(a => a.Email == login).Id,
                            id_RKD_VersionWork = 1,
                            counterKBE = 0,
                            counterKBM = 0
                        };
                        db.ApproveCDActions.Add(approveCDActions);
                        db.SaveChanges();
                        EmailApproveCD emailApproveCD = new EmailApproveCD(numhideIdOrder, login, 1, linkKD);
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        approveCDVersions.id_RKD_VersionWork = 2;
                        db.Entry(approveCDVersions).State = EntityState.Modified;
                        ApproveCDActions approveCDActions = new ApproveCDActions
                        {
                            id_ApproveCDVersions = approveCDVersions.id,
                            datetime = DateTime.Now,
                            id_AspNetUsers = db.AspNetUsers.First(a => a.Email == login).Id,
                            id_RKD_VersionWork = 3,
                            counterKBE = 0,
                            counterKBM = 0
                        };
                        db.ApproveCDActions.Add(approveCDActions);
                        db.SaveChanges();
                        EmailApproveCD emailApproveCD = new EmailApproveCD(numhideIdOrder, login, 1, linkKD);
                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.UpdateOrderLoadVer: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateOrderGetTSToKOUpdate(int hideIdOrder, string commitTSToKO)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    ApproveCDVersions approveCDVersions = db.ApproveCDVersions
                        .First(a => a.activeVersion == true && a.id_ApproveCDOrders == hideIdOrder);
                    approveCDVersions.id_RKD_VersionWork = 4;
                    db.Entry(approveCDVersions).State = EntityState.Modified;
                    db.SaveChanges();
                    ApproveCDActions approveCDActions = new ApproveCDActions
                    {
                        id_ApproveCDVersions = approveCDVersions.id,
                        datetime = DateTime.Now,
                        id_AspNetUsers = db.AspNetUsers.First(a => a.Email == login).Id,
                        id_RKD_VersionWork = 2,
                        counterKBE = 0,
                        counterKBM = 0
                    };
                    db.ApproveCDActions.Add(approveCDActions);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.UpdateOrderGetTSToKOUpdate: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateOrderGetTSToKOComplited(int hideIdOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    ApproveCDVersions approveCDVersions = db.ApproveCDVersions
                        .First(a => a.activeVersion == true && a.id_ApproveCDOrders == hideIdOrder);
                    approveCDVersions.id_RKD_VersionWork = 5;
                    db.Entry(approveCDVersions).State = EntityState.Modified;
                    db.SaveChanges();
                    ApproveCDActions approveCDActions = new ApproveCDActions
                    {
                        id_ApproveCDVersions = approveCDVersions.id,
                        datetime = DateTime.Now,
                        id_AspNetUsers = db.AspNetUsers.First(a => a.Email == login).Id,
                        id_RKD_VersionWork = 4,
                        counterKBE = 0,
                        counterKBM = 0
                    };
                    db.ApproveCDActions.Add(approveCDActions);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.UpdateOrderGetTSToKOComplited: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateOrderGetCustomerUpdate(int hideIdOrder, string commitTS, 
            int counterM, int counterE)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    ApproveCDVersions approveCDVersions = db.ApproveCDVersions
                        .First(a => a.activeVersion == true && a.id_ApproveCDOrders == hideIdOrder);
                    approveCDVersions.id_RKD_VersionWork = 8;
                    db.Entry(approveCDVersions).State = EntityState.Modified;
                    db.SaveChanges();
                    ApproveCDActions approveCDActions = new ApproveCDActions
                    {
                        id_ApproveCDVersions = approveCDVersions.id,
                        datetime = DateTime.Now,
                        id_AspNetUsers = db.AspNetUsers.First(a => a.Email == login).Id,
                        id_RKD_VersionWork = 7,
                        counterKBM = counterM,
                        counterKBE = counterE
                    };
                    db.ApproveCDActions.Add(approveCDActions);
                    db.SaveChanges();
                    EmailApproveCD emailApproveCD = new EmailApproveCD(hideIdOrder, login, 2, commitTS);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.UpdateOrderGetCustomerUpdate: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateOrderGetCustomerComplited(int hideIdOrder, string commitTS)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    ApproveCDVersions approveCDVersions = db.ApproveCDVersions
                        .First(a => a.activeVersion == true && a.id_ApproveCDOrders == hideIdOrder);
                    approveCDVersions.id_RKD_VersionWork = 10;
                    db.Entry(approveCDVersions).State = EntityState.Modified;
                    db.SaveChanges();
                    ApproveCDActions approveCDActions = new ApproveCDActions
                    {
                        id_ApproveCDVersions = approveCDVersions.id,
                        datetime = DateTime.Now,
                        id_AspNetUsers = db.AspNetUsers.First(a => a.Email == login).Id,
                        id_RKD_VersionWork = 8,
                        counterKBE = 0,
                        counterKBM = 0
                    };
                    db.ApproveCDActions.Add(approveCDActions);
                    db.SaveChanges();
                    EmailApproveCD emailApproveCD = new EmailApproveCD(hideIdOrder, login, 3, commitTS);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.UpdateOrderGetCustomerComplited: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetConcretTaskTable(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    List<TaskApproveCD> tasksList = new List<TaskApproveCD>();
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var tasks = db.ApproveCDTasks
                        .Include(a => a.ApproveCDOrders.PZ_PlanZakaz)
                        .Where(a => a.id_ApproveCDOrders == id)
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
                        .Where(a => a.id_ApproveCDOrders == id)
                        .ToList();
                    foreach (var dataInList in questions)
                    {
                        TaskApproveCD taskApproveCD = new TaskApproveCD(dataInList.dateTimeCreate, GetQuestionText(dataInList.id) + GetQuestionData(dataInList.id),
                            null, null,
                            dataInList.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz.ToString(), 2);
                        tasksList.Add(taskApproveCD);
                    }
                    var actionsList = db.ApproveCDActions
                        .Include(a => a.AspNetUsers)
                        .Include(a => a.ApproveCDVersions.ApproveCDOrders.PZ_PlanZakaz)
                        .Include(a => a.TypeRKD_Mail_Version)
                        .Where(a => a.ApproveCDVersions.id_ApproveCDOrders == id)
                        .ToList();
                    foreach (var dataInList in actionsList)
                    {
                        TaskApproveCD taskApproveCD = new TaskApproveCD(dataInList.datetime, dataInList.TypeRKD_Mail_Version.name,
                            null, null,
                            dataInList.ApproveCDVersions.ApproveCDOrders.PZ_PlanZakaz.PlanZakaz.ToString(), 3);
                        tasksList.Add(taskApproveCD);
                    }
                    var data = tasksList.Select(dataList => new
                    {
                        dateAction = JsonConvert.SerializeObject(dataList.dateTime, longUsSetting).Replace(@"""", ""),
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
                logger.Error("Wiki.Areas.ApproveCD.Controllers.GetConcretTaskTable: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CloseQuestion(int idQue, string commitQue)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    if(commitQue != null)
                    {
                        if(commitQue != "")
                        {
                            ApproveCDQuestionCorr approveCDQuestionCorr = new ApproveCDQuestionCorr
                            {
                                datetimeCreate = DateTime.Now,
                                id_ApproveCDQuestions = idQue,
                                id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                                textData = commitQue
                            };
                            db.ApproveCDQuestionCorr.Add(approveCDQuestionCorr);
                            db.SaveChanges();
                        }
                    }
                    var que = db.ApproveCDQuestions.Find(idQue);
                    que.active = true;
                    db.Entry(que).State = EntityState.Modified;
                    db.SaveChanges();
                    EmailApproveCDQue emailApproveCDQue = new EmailApproveCDQue(idQue, login);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.UpdateQuestion: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateDescription(int idOrder, string description)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    var order = db.ApproveCDOrders.Find(idOrder);
                    order.description = description;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Wiki.Areas.ApproveCD.Controllers.UpdateDescription: " + ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}