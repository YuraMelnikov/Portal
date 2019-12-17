using NLog;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class RKDController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string linkToExcelListOrders = @"http://pserver/RKD/ExportToExcelOrders/";

        private int GetPredcessorOrderNumForLink(int id_Order)
        {
            var orderList = db.RKD_Order.Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now == false).OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList();
            int pred = 0;
            int predFin = 0;
            int postFin = 0;
            foreach (var data in orderList)
            {
                if (predFin > 0)
                {
                    postFin = data.id;
                    break;
                }
                if (data.id == id_Order)
                    predFin = pred;
                pred = data.id;
            }
            
            return predFin;
        }
        private int GetPostOrderNumForLink(int id_Order)
        {
            var orderList = db.RKD_Order.Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList();
            int pred = 0;
            int predFin = 0;
            int postFin = 0;
            foreach (var data in orderList)
            {
                if (predFin > 0)
                {
                    postFin = data.id;
                    break;
                }
                if (data.id == id_Order)
                    predFin = pred;
                pred = data.id;
            }
            if (postFin == 0 & predFin == 0)
            {
                int i = 0;
                foreach (var data in orderList)
                {
                    postFin = data.id;
                    i++;
                    if (i == 2)
                        break;
                }
            }
            return postFin;
        }

        private List<RKD_ForTaskList> GetListDespathingForOrder()
        {
            DateTime dateControl = DateTime.Now.AddDays(-30);
            List<RKD_ForTaskList> rKD_ForTaskLists = new List<RKD_ForTaskList>();
            foreach (var data in db.RKD_Despatching.Where(d => d.dateEvent > dateControl).ToList())
            {
                string dataName = "";
                string dataDate = "";
                if (data.id_AspNetUsers != null)
                    dataName = data.AspNetUsers.CiliricalName;
                if (data.dateTaskFinishDate != null)
                    dataDate = data.dateTaskFinishDate.ToString().Substring(0, 10);
                rKD_ForTaskLists.Add(new RKD_ForTaskList((int)data.id_RKD_Order, data.RKD_Order.PZ_PlanZakaz.PlanZakaz, (DateTime)data.dateEvent, data.text, 0, dataName, dataDate));
            }
            foreach (var data in db.RKD_Mail_Version.Where(d => d.dateTimeUpload > dateControl).ToList())
            {
                rKD_ForTaskLists.Add(new RKD_ForTaskList(data.RKD_Version.id_RKD_Order, data.RKD_Version.RKD_Order.PZ_PlanZakaz.PlanZakaz, data.dateTimeUpload, data.TypeRKD_Mail_Version.name, data.id_TypeRKD_Mail_Version, "", ""));
            }
            try
            {
                foreach (var data in db.RKD_Question.Where(d => d.dateUpload > dateControl).ToList())
                {
                    string dataText = data.textQuestion + " : " + data.AspNetUsers.CiliricalName;
                    foreach (var dataQua in db.RKD_QuestionData.Where(d => d.id_RKD_Question == data.id))
                    {
                        dataText += "\n\n\n" + dataQua.textData + " : " + dataQua.AspNetUsers.CiliricalName;
                    }
                    rKD_ForTaskLists.Add(new RKD_ForTaskList(data.id_RKD_Order, data.RKD_Order.PZ_PlanZakaz.PlanZakaz, data.dateUpload, dataText, 100, "", ""));
                }
            }
            catch
            { };
            var returnData = rKD_ForTaskLists.OrderByDescending(d => d.DateEvent).ToList();
            return rKD_ForTaskLists;
        }

        private List<RKD_ForTaskList> GetListDespathingForOrder(int? idOrder)
        {
            List<RKD_ForTaskList> rKD_ForTaskLists = new List<RKD_ForTaskList>();
            foreach (var data in db.RKD_Despatching.Where(d => d.id_RKD_Order == idOrder).ToList())
            {
                string dataName = "";
                string dataDate = "";
                if (data.id_AspNetUsers != null)
                    dataName = data.AspNetUsers.CiliricalName;
                if (data.dateTaskFinishDate != null)
                    dataDate = data.dateTaskFinishDate.ToString().Substring(0, 10);
                rKD_ForTaskLists.Add(new RKD_ForTaskList((int)data.id_RKD_Order, data.RKD_Order.PZ_PlanZakaz.PlanZakaz, (DateTime)data.dateEvent, data.text, 0, dataName, dataDate));
            }
            foreach (var data in db.RKD_Mail_Version.Where(d => d.RKD_Version.id_RKD_Order == idOrder).ToList())
            {
                rKD_ForTaskLists.Add(new RKD_ForTaskList(data.RKD_Version.id_RKD_Order, data.RKD_Version.RKD_Order.PZ_PlanZakaz.PlanZakaz, data.dateTimeUpload, data.TypeRKD_Mail_Version.name, data.id_TypeRKD_Mail_Version, "", ""));
            }
            try
            {
                foreach (var data in db.RKD_Question.Where(d => d.id_RKD_Order == idOrder).ToList())
                {
                    string dataText = data.textQuestion + " : " + data.AspNetUsers.CiliricalName;
                    foreach (var dataQua in db.RKD_QuestionData.Where(d => d.id_RKD_Question == data.id))
                    {
                        dataText += "\n" + dataQua.textData + " : " + dataQua.AspNetUsers.CiliricalName;
                    }
                    rKD_ForTaskLists.Add(new RKD_ForTaskList(data.id_RKD_Order, data.RKD_Order.PZ_PlanZakaz.PlanZakaz, data.dateUpload, dataText, 100, "", ""));
                }
            }
            catch
            { }

            var returnData = rKD_ForTaskLists.OrderByDescending(d => d.DateEvent).ToList();
            return returnData;
        }

        private List<RKD_ForTaskList> GetListDespathingAllData()
        {
            List<RKD_ForTaskList> rKD_ForTaskLists = new List<RKD_ForTaskList>();
            foreach (var data in db.RKD_Despatching.ToList())
            {
                string dataName = "";
                string dataDate = "";
                if (data.id_AspNetUsers != null)
                    dataName = data.AspNetUsers.CiliricalName;
                if (data.dateTaskFinishDate != null)
                    dataDate = data.dateTaskFinishDate.ToString().Substring(0, 10);
                rKD_ForTaskLists.Add(new RKD_ForTaskList((int)data.id_RKD_Order, data.RKD_Order.PZ_PlanZakaz.PlanZakaz, (DateTime)data.dateEvent, data.text, 0, dataName, dataDate));
            }
            foreach (var data in db.RKD_Mail_Version.ToList())
            {
                rKD_ForTaskLists.Add(new RKD_ForTaskList(data.RKD_Version.id_RKD_Order, data.RKD_Version.RKD_Order.PZ_PlanZakaz.PlanZakaz, data.dateTimeUpload, data.TypeRKD_Mail_Version.name, data.id_TypeRKD_Mail_Version, "", ""));
            }
            try
            {
                foreach (var data in db.RKD_Question.ToList())
                {
                    string dataText = data.textQuestion + " : " + data.AspNetUsers.CiliricalName;
                    foreach (var dataQua in db.RKD_QuestionData.Where(d => d.id_RKD_Question == data.id))
                    {
                        dataText += "\n\n\n" + dataQua.textData + " : " + dataQua.AspNetUsers.CiliricalName;
                    }
                    rKD_ForTaskLists.Add(new RKD_ForTaskList(data.id_RKD_Order, data.RKD_Order.PZ_PlanZakaz.PlanZakaz, data.dateUpload, dataText, 100, "", ""));
                }
            }
            catch
            { };
            var returnData = rKD_ForTaskLists.OrderBy(d => d.DateEvent).ToList();
            return rKD_ForTaskLists;
        }

        public ActionResult Debug()
        {
            ProjectServer projectServer = new ProjectServer();
            projectServer.CreateTasks();
            new ProjectServer_UpdateMustStartOnCRUD();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz
                .Include(d => d.RKD_Order)
                .Where(d => d.dataOtgruzkiBP > DateTime.Now && d.RKD_Order.Count == 0)
                .OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
        }

        [HttpPost]
        public ActionResult Debug(int id_PlanZakaz)
        {
            RKD rKD = new RKD(id_PlanZakaz);
            rKD.CreateRKDOrder();
            return RedirectToAction("Index", "RKD");
        }

        public ActionResult Error()
        {
            string login = HttpContext.User.Identity.Name;
            logger.Fatal("Fatal View");
            return View();
        }

        public ActionResult QuestionList()
        {
            string login = HttpContext.User.Identity.Name;
            ViewRKD_Question viewOSCMO = new ViewRKD_Question();
            try
            {
                var activeQuestion = db.RKD_Question.Where(d => d.complited == false).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                var closeQuestion = db.RKD_Question.Where(d => d.complited == true).OrderByDescending(d => d.dateUpload).ToList();

                viewOSCMO.ActiveQuestion = activeQuestion;
                viewOSCMO.CloseQuestion = closeQuestion;
                logger.Debug("RKDController - QuestionList " + login.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - QuestionList " + login.ToString() + " " + ex.Message.ToString());
            }
            return View(viewOSCMO);
        }

        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        public ActionResult CreateQuestion()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.id_RKD_Order = new SelectList(db.RKD_Order.Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PZ_PlanZakaz.PlanZakaz), "id", "PZ_PlanZakaz.PlanZakaz");
                logger.Debug("RKDController - CreateQuestion" + login.ToString());
                return View();
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - CreateQuestion " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        [HttpPost]
        public ActionResult CreateQuestion(RKD_Question rKD_Question)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                rKD_Question.dateUpload = DateTime.Now;
                rKD_Question.userUpload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                rKD_Question.complited = false;
                db.RKD_Question.Add(rKD_Question);
                db.SaveChanges();
                EmailRKDQuestion mail = new EmailRKDQuestion(db.RKD_Order.Find(rKD_Question.id_RKD_Order), login);
                mail.CreateAndSendMail(rKD_Question.textQuestion);
                ViewBag.id_RKD_Order = new SelectList(db.RKD_Order.Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now == false).OrderBy(d => d.PZ_PlanZakaz.PlanZakaz), "id", "PZ_PlanZakaz.PlanZakaz");
                logger.Debug("RKDController - CreateQuestion " + login.ToString());
                return RedirectToAction("QuestionList", "RKD");
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - CreateQuestion" + ex.Message.ToString() + " " + login.ToString());
                return RedirectToAction("Index", "RKD");
            }
        }
        
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        public ActionResult CreateQuestionOnOrder(int? idOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.id_RKD_Order = new SelectList(db.RKD_Order.Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now == false).OrderBy(d => d.PZ_PlanZakaz.PlanZakaz), "id", "PZ_PlanZakaz.PlanZakaz");
                ViewBag.Order = idOrder;
                RKD_Question rKD_Question = new RKD_Question();
                rKD_Question.id_RKD_Order = (int)idOrder;
                logger.Debug("RKDController - CreateQuestionOnOrder " + login.ToString());
                return View(rKD_Question);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - CreateQuestionOnOrder " + ex.Message.ToString() + login.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, GR")]
        [HttpPost]
        public ActionResult CreateQuestionOnOrder(RKD_Question rKD_Question)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                rKD_Question.dateUpload = DateTime.Now;
                rKD_Question.userUpload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                rKD_Question.complited = false;
                db.RKD_Question.Add(rKD_Question);
                db.SaveChanges();
                EmailRKDQuestion mail = new EmailRKDQuestion(db.RKD_Order.Find(rKD_Question.id_RKD_Order), login);
                mail.CreateAndSendMail(rKD_Question.textQuestion);
                ViewBag.id_RKD_Order = new SelectList(db.RKD_Order.Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PZ_PlanZakaz.PlanZakaz), "id", "PZ_PlanZakaz.PlanZakaz");
                logger.Debug("RKDController - CreateQuestionOnOrder " + login.ToString());
                return RedirectToAction("QuestionList", "RKD");
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - CreateQuestionOnOrder " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("QuestionList", "RKD");
            }
        }

        [Authorize(Roles = "Admin, OPTP, OP, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        public ActionResult UpdateQuestion(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Question rKD_Question = db.RKD_Question.Find(id);
                ViewBag.ListQuestion = db.RKD_QuestionData.Where(d => d.id_RKD_Question == id).OrderByDescending(d => d.dateUpload).ToList();
                ViewBag.PlanZakaz = rKD_Question.RKD_Order.PZ_PlanZakaz.PlanZakaz;
                ViewBag.idQue = rKD_Question.id;
                ViewBag.QueText = rKD_Question.textQuestion.ToString();
                logger.Debug("RKDController - UpdateQuestion " + login.ToString());
                return View();
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - UpdateQuestion " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [Authorize(Roles = "Admin, OPTP, OP, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        [HttpPost]
        public ActionResult UpdateQuestion(RKD_Question rKD_Question)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_QuestionData rKD_QuestionData = new RKD_QuestionData();
                rKD_QuestionData.textData = rKD_Question.textQuestion;
                rKD_QuestionData.id_RKD_Question = rKD_Question.id;
                rKD_QuestionData.dateUpload = DateTime.Now;
                rKD_QuestionData.userUpload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                db.RKD_QuestionData.Add(rKD_QuestionData);
                db.SaveChanges();
                int idOrder = db.RKD_Question.Find(rKD_QuestionData.id_RKD_Question).id_RKD_Order;
                EmailRKDQustionUpdate mail = new EmailRKDQustionUpdate(db.RKD_Order.Find(idOrder), login, rKD_QuestionData.id_RKD_Question);
                mail.CreateAndSendMail("");
                logger.Debug("RKDController - UpdateQuestion " + login.ToString());
                return RedirectToAction("QuestionList", "RKD");
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - UpdateQuestion " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("QuestionList", "RKD");
            }
        }

        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        public ActionResult CloseQuestion(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Question rKD_Question = db.RKD_Question.Find(id);
                rKD_Question.complited = true;
                db.Entry(rKD_Question).State = EntityState.Modified;
                db.SaveChanges();
                //to Email data
                string pz = db.RKD_Order.Find(rKD_Question.id_RKD_Order).PZ_PlanZakaz.PlanZakaz.ToString();
                string userUpload = db.AspNetUsers.Find(rKD_Question.userUpload).CiliricalName;
                List<string> arrayMail = new List<string>();
                arrayMail.Add("maj@katek.by");
                arrayMail.Add("nrf@katek.by");
                arrayMail.Add("bav@katek.by");
                arrayMail.Add("Kuchynski@katek.by");
                arrayMail.Add("fvs@katek.by");
                try
                {
                    foreach (var VARIABLE in db.RKD_GIP.Where(d => d.id_RKD_Order == rKD_Question.id_RKD_Order).ToList())
                    {
                        if(VARIABLE.AspNetUsers.LockoutEnabled != false)
                            arrayMail.Add(VARIABLE.AspNetUsers.Email);
                        if (VARIABLE.AspNetUsers1.LockoutEnabled != false)
                            arrayMail.Add(VARIABLE.AspNetUsers1.Email);
                    }
                }
                catch
                {

                }
                string historyMail = "";
                try { historyMail += "Текст вопроса: " + rKD_Question.textQuestion + "\n\n"; }
                catch { }
                try
                {
                    foreach (var data in db.RKD_QuestionData.Where(d => d.id_RKD_Question == rKD_Question.id).OrderByDescending(d => d.dateUpload))
                    {
                        historyMail += data.dateUpload.ToString() + " | " + data.textData + " (" + db.AspNetUsers.Find(data.userUpload).CiliricalName + ")" + "\n\n";
                    }
                }
                catch
                {

                }
                EmailModel emailModel = new EmailModel();
                string subject = "Закрыт вопрос по РКД: " + pz + " (№ вопроса: " + rKD_Question.id.ToString() + ")";
                string body = "Вопрос по РКД: " + pz + "\n\n"
                    + "Текст вопроса: " + rKD_Question.textQuestion + "\n\n"
                    + "История переписки" + "\n\n" + historyMail;
                emailModel.SendEmailList(arrayMail, subject, body);
                logger.Debug("RKDController - CloseQuestion " + login.ToString());
                return RedirectToAction("QuestionList", "RKD");
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - QuestionList " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("QuestionList", "RKD");
            }
        }
        
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        public ActionResult CreateInstitute()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - CreateInstitute " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        [HttpPost]
        public ActionResult CreateInstitute(RKD_Institute rKD_Institute)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                db.RKD_Institute.Add(rKD_Institute);
                db.SaveChanges();
                logger.Debug("RKDController - CreateInstitute " + login.ToString());
                return RedirectToAction("Index", "RKD");
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - CreateInstitute " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }

        public ActionResult Index()
        {
            ViewBag.linkToExcel = linkToExcelListOrders;
            try
            {
                int devision = 0;
                string login = HttpContext.User.Identity.Name;
                if (login != null)
                {
                    try
                    {
                        devision = (int)db.AspNetUsers.Where(d => d.Email == login).First().Devision;
                        if (devision == 15 || devision == 16 || devision == 18)
                            devision = 3;
                    }
                    catch
                    {

                    }
                }
                ViewBag.devision = devision;
                ViewRKD viewRKD = new ViewRKD();
                var datadata = db.RKD_Order
                    .Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                    .ToList()
                    .Where(d => d.RKD_Version.First(z => z.activeVersion == true)
                    .id_RKD_VersionWork != 10)
                    .ToList();
                viewRKD.ActiveOrder = datadata;
                ViewRKD_Question viewOSCMO = new ViewRKD_Question();
                try
                {
                    var activeQuestion = db.RKD_Question.Where(d => d.complited == false).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                    if (activeQuestion.Count != 0)
                    {
                        viewOSCMO.ActiveQuestion = activeQuestion;
                        ViewBag.ActiveQuestion = viewOSCMO.ActiveQuestion.ToList();
                    }
                    else
                    {
                        ViewBag.ActiveQuestion = null;
                    }
                    var dataDespatching = GetListDespathingForOrder().OrderByDescending(d => d.DateEvent);
                    ViewBag.Despatching = dataDespatching;
                    ViewBag.ListDesparting = db.RKD_Despatching.Where(d => d.dateEvent > DateTime.Now.AddDays(-30));
                    logger.Debug("RKDController - Index ");
                }
                catch (Exception ex)
                {
                    logger.Error("RKDController - Index " + " " + ex.Message.ToString());
                }
                logger.Debug("RKDController - Index ");
                return View(viewRKD);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - Index " + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }

        public ActionResult IndexComplited()
        {
            ViewBag.linkToExcel = linkToExcelListOrders;
            try
            {
                int devision = 0;
                string login = HttpContext.User.Identity.Name;
                if (login != null)
                {
                    try
                    {
                        devision = (int)db.AspNetUsers.Where(d => d.Email == login).First().Devision;
                        if (devision == 15 || devision == 16)
                            devision = 3;
                    }
                    catch
                    {

                    }
                }
                ViewBag.devision = devision;
                ViewRKD viewRKD = new ViewRKD();
                var datadata = db.RKD_Order
                    .Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now == false)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                    .ToList()
                    //.Where(d => d.RKD_Version.First(z => z.activeVersion == true).id_RKD_VersionWork == 10)
                    .ToList();
                viewRKD.ActiveOrder = datadata;
                ViewRKD_Question viewOSCMO = new ViewRKD_Question();
                try
                {
                    var activeQuestion = db.RKD_Question.Where(d => d.complited == false).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                    if (activeQuestion.Count != 0)
                    {
                        viewOSCMO.ActiveQuestion = activeQuestion;
                        ViewBag.ActiveQuestion = viewOSCMO.ActiveQuestion.ToList();
                    }
                    else
                    {
                        ViewBag.ActiveQuestion = null;
                    }
                    var dataDespatching = GetListDespathingForOrder().OrderByDescending(d => d.DateEvent);
                    ViewBag.Despatching = dataDespatching;
                    ViewBag.ListDesparting = db.RKD_Despatching.Where(d => d.dateEvent > DateTime.Now.AddDays(-30));
                    logger.Debug("RKDController - Index ");
                }
                catch (Exception ex)
                {
                    logger.Error("RKDController - Index " + " " + ex.Message.ToString());
                }
                logger.Debug("RKDController - Index ");
                return View(viewRKD);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - Index " + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        
        public ActionResult IndexOld()
        {
            ViewBag.linkToExcel = linkToExcelListOrders;
            try
            {
                int devision = 0;
                string login = HttpContext.User.Identity.Name;
                if (login != null)
                {
                    try
                    {
                        devision = (int)db.AspNetUsers.Where(d => d.Email == login).First().Devision;
                        if (devision == 15 || devision == 16)
                            devision = 3;
                    }
                    catch
                    {

                    }
                }
                ViewBag.devision = devision;
                ViewRKD viewRKD = new ViewRKD();
                viewRKD.ActiveOrder = db.RKD_Order.Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now == true).ToList().OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList();
                ViewRKD_Question viewOSCMO = new ViewRKD_Question();
                try
                {
                    var activeQuestion = db.RKD_Question.Where(d => d.complited == false).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                    if (activeQuestion.Count != 0)
                    {
                        viewOSCMO.ActiveQuestion = activeQuestion;
                        ViewBag.ActiveQuestion = viewOSCMO.ActiveQuestion.ToList();
                    }
                    else
                    {
                        ViewBag.ActiveQuestion = null;
                    }
                    logger.Debug("RKDController - IndexOld ");
                }
                catch (Exception ex)
                {
                    logger.Error("RKDController - IndexOld " + " " + ex.Message.ToString());
                }
                logger.Debug("RKDController - IndexOld ");
                return View(viewRKD);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - IndexOld " + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        
        public ActionResult StartDesparting()
        {
            int idOrder = db.RKD_Order.Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now == false).OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).First().id;
            return RedirectToAction("OpenOrder", "RKD", new { idOrder = idOrder });
        }

        public ActionResult OpenOrder(int? idOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                List<int> activeOrderList = new List<int>();
                RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
                //Ожидание готовности первой версии РКД
                if (rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().id_RKD_VersionWork == 12)
                {
                    logger.Debug("RKDController - OpenOrder " + login.ToString());
                    return RedirectToAction("UploadNewVersionRDK", "RKD", new { idOrder = idOrder });
                }
                //Черновой вариант РКД на рассмотрении у ТП
                if (rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().id_RKD_VersionWork == 2)
                {
                    logger.Debug("RKDController - OpenOrder " + login.ToString());
                    return RedirectToAction("SeachTP", "RKD", new { idOrder = idOrder });
                }
                //Черновой вариант РКД на дорботке у КО
                if (rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().id_RKD_VersionWork == 4)
                {
                    logger.Debug("RKDController - OpenOrder " + login.ToString());
                    return RedirectToAction("UploadVersionRDK", "RKD", new { idOrder = idOrder });
                }
                //Ожидание ответа от Заказчика
                if (rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().id_RKD_VersionWork == 5)
                {
                    logger.Debug("RKDController - OpenOrder " + login.ToString());
                    return RedirectToAction("GetInfClient", "RKD", new { idOrder = idOrder });
                }
                //Ожидание ежедневного совещания (пришел ответ от Заказчика)
                if (rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().id_RKD_VersionWork == 7)
                {
                    logger.Debug("RKDController - OpenOrder " + login.ToString());
                    return RedirectToAction("Edit", "RKD", new { id = idOrder });
                }
                //Ожидание новой версии РКД
                if (rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().id_RKD_VersionWork == 8)
                {
                    logger.Debug("RKDController - OpenOrder " + login.ToString());
                    return RedirectToAction("UploadNewVersionRDK", "RKD", new { idOrder = idOrder });
                }
                //РКД согласовано Заказчиком
                if (rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().id_RKD_VersionWork == 10)
                {
                    logger.Debug("RKDController - OpenOrder " + login.ToString());
                    return RedirectToAction("DetailsClose", "RKD", new { id = idOrder });
                }
                //РКД УСЛОВНО согласовано Заказчиком
                if (rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().id_RKD_VersionWork == 16)
                {
                    logger.Debug("RKDController - OpenOrder " + login.ToString());
                    return RedirectToAction("GetInfClient", "RKD", new { idOrder = idOrder });
                }
                else
                {
                    logger.Error("RKDController - OpenOrder " + login.ToString() + " " + "нет такого условия в списе выбора! (МЮИ)");
                    return RedirectToAction("Error", "RKD");
                }
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - OpenOrder " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }

        // 1 step - загрузка новой версии РКД для отправки на согласование в ТП
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        public ActionResult UploadNewVersionRDK(int? idOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
                ViewBag.idLaslVersion = rKD_Order.RKD_Version.Where(d => d.id_RKD_Order == rKD_Order.id).Count();
                ViewBag.PlanZakaz = rKD_Order.PZ_PlanZakaz.PlanZakaz.ToString();
                ViewBag.PlanZakazName = rKD_Order.PZ_PlanZakaz.Name.ToString();
                ViewBag.Client = rKD_Order.PZ_PlanZakaz.PZ_Client.Name.ToString();
                ViewBag.Inst = rKD_Order.RKD_Institute.name.ToString();
                ViewBag.day = new SelectList(db.RKD_VersionDay.OrderBy(d => d.countDay), "id", "dataName");
                RKD rKD = new RKD(db.RKD_Order.Find(idOrder).id_PZ_PlanZakaz);
                ViewBag.BasicCount = rKD.GetNumberNewVer();
                ViewBag.QuestList = db.RKD_Question.Where(d => d.id_RKD_Order == idOrder & d.complited == false).OrderByDescending(d => d.dateUpload).ToList();
                ViewBag.Despatching = GetListDespathingForOrder(idOrder);
                RKD_AllView rKD_AllView = new RKD_AllView();
                rKD_AllView.ActiveOrder = db.RKD_TaskVersion.Where(d => d.RKD_Task.id_RKD_Order == idOrder & d.RKD_Version.numberVersion1 != 0).ToList();
                var activeQuestion = db.RKD_Question.Where(d => d.complited == false & d.id_RKD_Order == rKD_Order.id).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                var closeQuestion = db.RKD_Question.Where(d => d.complited == true & d.id_RKD_Order == rKD_Order.id).ToList();
                ViewBag.idOrder = idOrder;
                if (activeQuestion.Count != 0)
                    ViewBag.ActiveQuestion = activeQuestion;
                else
                    ViewBag.ActiveQuestion = null;
                if (closeQuestion.Count != 0)
                    ViewBag.CloseQuestion = closeQuestion;
                else
                    ViewBag.CloseQuestion = null;
                ViewBag.PredcwssorOrderLink = GetPredcessorOrderNumForLink(idOrder.Value);
                ViewBag.PoslOrderLink = GetPostOrderNumForLink(idOrder.Value);
                logger.Debug("RKDController - UploadNewVersionRDK " + login.ToString());
                return View(rKD_Order);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - UploadNewVersionRDK " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        [HttpPost]
        public ActionResult UploadNewVersionRDK(RKD_Order rKD_Order, string basicCount, string folderRKD)
        {
            if (folderRKD == null)
            {
                return RedirectToAction("UploadNewVersionRDK", "RKD", new { idOrder = rKD_Order.id });
            }
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.idOrder = rKD_Order.id;
                RKD rKD = new RKD(rKD_Order.id_PZ_PlanZakaz);
                rKD.UploadNewVersionRDK(rKD_Order, basicCount, folderRKD, login);
                logger.Debug("RKDController - UploadNewVersionRDK " + login.ToString());
                return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - UploadNewVersionRDK " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
            }
        }

        // 2 step - Черновой вариант РКД на рссмотрении у ГР
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        public ActionResult SeachTP(int? idOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
                ViewBag.day = new SelectList(db.RKD_VersionDay.OrderBy(d => d.countDay), "id", "dataName");
                ViewBag.idOrder = idOrder;
                ViewBag.Despatching = GetListDespathingForOrder(idOrder);
                ViewBag.PredcwssorOrderLink = GetPredcessorOrderNumForLink(idOrder.Value);
                ViewBag.PoslOrderLink = GetPostOrderNumForLink(idOrder.Value);
                ViewBag.PlanZakaz = rKD_Order.PZ_PlanZakaz.PlanZakaz.ToString();
                ViewBag.Client = rKD_Order.PZ_PlanZakaz.PZ_Client.Name.ToString();
                ViewBag.Inst = rKD_Order.RKD_Institute.name.ToString();
                ViewBag.PlanZakazName = rKD_Order.PZ_PlanZakaz.Name.ToString();

                RKD_AllView rKD_AllView = new RKD_AllView();
                ViewBag.ActiveOrder = db.RKD_TaskVersion.Where(d => d.RKD_Task.id_RKD_Order == idOrder & d.RKD_Version.numberVersion1 != 0).ToList();

                logger.Debug("RKDController - SeachTP " + login.ToString());
                return View(rKD_Order);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - SeachTP " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }

        // 2 step - отправить РКД на доработку 
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        public ActionResult ErrorNewVersionRDK(int id_RKD_Order)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order = db.RKD_Order.Find(id_RKD_Order);
                RKD_Despatching rKD_Despatching = new RKD_Despatching();
                rKD_Despatching.id_RKD_Order = rKD_Order.id;
                logger.Debug("RKDController - ErrorNewVersionRDK " + login.ToString());
                return View(rKD_Despatching);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - ErrorNewVersionRDK " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        // 2 step - отправить РКД на доработку 
        [Authorize(Roles = "Admin, OPTP, OP, Technologist")]
        [HttpPost]
        public ActionResult ErrorNewVersionRDK(RKD_Despatching rKD_Order)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order1 = db.RKD_Order.Find(rKD_Order.id_RKD_Order);
                RKD rKD = new RKD(rKD_Order1.id_PZ_PlanZakaz);
                rKD.ErrorNewVersionRDK(rKD_Order1.id, login, rKD_Order.text);
                ViewBag.QuestList = db.RKD_Question.Where(d => d.id_RKD_Order == rKD_Order.id_RKD_Order & d.complited == false).OrderByDescending(d => d.dateUpload).ToList();
                ViewBag.Despatching = db.RKD_Despatching.Where(d => d.id_RKD_Order == rKD_Order.id_RKD_Order).OrderByDescending(d => d.dateEvent).ToList();
                RKD_AllView rKD_AllView = new RKD_AllView();
                rKD_AllView.ActiveOrder = db.RKD_TaskVersion.Where(d => d.RKD_Task.id_RKD_Order == rKD_Order.id_RKD_Order & d.RKD_Version.numberVersion1 != 0).ToList();
                var activeQuestion = db.RKD_Question.Where(d => d.complited == false & d.id_RKD_Order == rKD_Order.id_RKD_Order).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                var closeQuestion = db.RKD_Question.Where(d => d.complited == true & d.id_RKD_Order == rKD_Order.id_RKD_Order).ToList();
                ViewBag.idOrder = rKD_Order.id_RKD_Order;
                if (activeQuestion.Count != 0)
                    ViewBag.ActiveQuestion = activeQuestion;
                else
                    ViewBag.ActiveQuestion = null;
                if (closeQuestion.Count != 0)
                    ViewBag.CloseQuestion = closeQuestion;
                else
                    ViewBag.CloseQuestion = null;
                return RedirectToAction("Details", "RKD", new { rKD_Order.id });
            }
            catch 
            {
                return RedirectToAction("Details", "RKD", new { rKD_Order.id });
            }
        }

        // 3 step - отправить доработаное РКД в ТП на согласование
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        public ActionResult UploadVersionRDK(int? idOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.idOrder = (int)idOrder;
                RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
                ViewBag.idLaslVersion = rKD_Order.RKD_Version.Where(d => d.id_RKD_Order == rKD_Order.id).Count();
                ViewBag.PlanZakaz = rKD_Order.PZ_PlanZakaz.PlanZakaz.ToString();
                ViewBag.PlanZakazName = rKD_Order.PZ_PlanZakaz.Name.ToString();
                ViewBag.Client = rKD_Order.PZ_PlanZakaz.PZ_Client.Name.ToString();
                ViewBag.Inst = rKD_Order.RKD_Institute.name.ToString();
                ViewBag.day = new SelectList(db.RKD_VersionDay.OrderBy(d => d.countDay), "id", "dataName");
                ViewBag.BasicCount = "V" 
                    + rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().numberVersion1.ToString()
                    +"."
                    + rKD_Order.RKD_Version.Where(d => d.activeVersion == true).First().numberVersion2.ToString();
                ViewBag.QuestList = db.RKD_Question.Where(d => d.id_RKD_Order == idOrder & d.complited == false).OrderByDescending(d => d.dateUpload).ToList();
                ViewBag.Despatching = GetListDespathingForOrder(idOrder);
                var activeQuestion = db.RKD_Question.Where(d => d.complited == false & d.id_RKD_Order == rKD_Order.id).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                var closeQuestion = db.RKD_Question.Where(d => d.complited == true & d.id_RKD_Order == rKD_Order.id).ToList();
                ViewBag.idOrder = idOrder;
                if (activeQuestion.Count != 0)
                    ViewBag.ActiveQuestion = activeQuestion;
                else
                    ViewBag.ActiveQuestion = null;
                if (closeQuestion.Count != 0)
                    ViewBag.CloseQuestion = closeQuestion;
                else
                    ViewBag.CloseQuestion = null;
                ViewBag.PredcwssorOrderLink = GetPredcessorOrderNumForLink(idOrder.Value);
                ViewBag.PoslOrderLink = GetPostOrderNumForLink(idOrder.Value);
                logger.Debug("RKDController - UploadVersionRDK " + login.ToString());
                return View(rKD_Order);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - UploadVersionRDK " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, GR")]
        [HttpPost]
        public ActionResult UploadVersionRDK(RKD_Order rKD_Order, string folderRKD)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                if (folderRKD == null || folderRKD == "")
                {
                    return RedirectToAction("UploadVersionRDK", "RKD", new { idOrder = rKD_Order.id });
                }

                RKD rKD = new RKD(rKD_Order.id_PZ_PlanZakaz);
                rKD.UploadVersionRDK(rKD_Order, folderRKD, login);
                logger.Debug("RKDController - UploadVersionRDK " + login.ToString());
                return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - UploadVersionRDK " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
            }
        }

        // 4 step - отправить РКД Заказчику/ПИ
        [Authorize(Roles = "Admin, OPTP, OP, Technologist")]
        [HttpPost]
        public ActionResult UploadClient(RKD_Order rKD_Order, int day)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD rKD = new RKD(rKD_Order.id_PZ_PlanZakaz);
                rKD.UploadClient(rKD_Order.id, login, day);
                logger.Debug("RKDController - UploadClient " + login.ToString());
                return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - UploadClient " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
            }
        }

        // for step 6-7-8 - Выбираем возможный вариант ответа от Заказчика/ПИ
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        public ActionResult GetInfClient(int? idOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
                ViewBag.idOrder = (int)idOrder;
                ViewBag.idLaslVersion = rKD_Order.RKD_Version.Where(d => d.id_RKD_Order == rKD_Order.id).Count();
                ViewBag.PlanZakaz = rKD_Order.PZ_PlanZakaz.PlanZakaz.ToString();
                ViewBag.PlanZakazName = rKD_Order.PZ_PlanZakaz.Name.ToString();
                ViewBag.Client = rKD_Order.PZ_PlanZakaz.PZ_Client.Name.ToString();
                ViewBag.Inst = rKD_Order.RKD_Institute.name.ToString();
                ViewBag.QuestList = db.RKD_Question.Where(d => d.id_RKD_Order == idOrder & d.complited == false).OrderByDescending(d => d.dateUpload).ToList();
                ViewBag.Despatching = GetListDespathingForOrder(idOrder);
                RKD_AllView rKD_AllView = new RKD_AllView();
                rKD_AllView.ActiveOrder = db.RKD_TaskVersion.Where(d => d.RKD_Task.id_RKD_Order == idOrder & d.RKD_Version.numberVersion1 != 0).ToList();
                var activeQuestion = db.RKD_Question.Where(d => d.complited == false & d.id_RKD_Order == rKD_Order.id).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                var closeQuestion = db.RKD_Question.Where(d => d.complited == true & d.id_RKD_Order == rKD_Order.id).ToList();
                ViewBag.idOrder = idOrder;
                if (activeQuestion.Count != 0)
                    ViewBag.ActiveQuestion = activeQuestion;
                else
                    ViewBag.ActiveQuestion = null;
                if (closeQuestion.Count != 0)
                    ViewBag.CloseQuestion = closeQuestion;
                else
                    ViewBag.CloseQuestion = null;
                ViewBag.PredcwssorOrderLink = GetPredcessorOrderNumForLink(idOrder.Value);
                ViewBag.PoslOrderLink = GetPostOrderNumForLink(idOrder.Value);
                logger.Debug("RKDController - GetInfClient " + login.ToString());
                return View(rKD_AllView);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - GetInfClient " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }

        // 6 step - Добавить дней до получения ответа от Заказчика/ПИ
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        public ActionResult PlusClientDay(int? idOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
                ViewBag.day = new SelectList(db.RKD_VersionDay.OrderBy(d => d.countDay), "id", "dataName");
                logger.Debug("RKDController - PlusClientDay " + login.ToString());
                return View(rKD_Order);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - PlusClientDay " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [Authorize(Roles = "Admin, OPTP, OP, Technologist")]
        [HttpPost]
        public ActionResult PlusClientDay(RKD_Order rKD_Order)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD rKD = new RKD(rKD_Order.id_PZ_PlanZakaz);
                rKD.PlusClientDay(rKD_Order.id, login, rKD_Order.datePlanCritical);
                logger.Debug("RKDController - PlusClientDay " + login.ToString());
                return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - PlusClientDay " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }

        // 7 step - загрузить замечания от Заказчика/ПИ в систему
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        public ActionResult GetErrorList(int? idOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                var orderList = db.RKD_Version.Where(d => d.activeVersion == true).Where(d => d.id_RKD_VersionWork == 5).First().RKD_Order.id;
                RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
                ViewBag.PZ = new SelectList(db.RKD_Version.Where(d => d.activeVersion == true).Where(d => d.id_RKD_VersionWork == 5).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList(), "RKD_Order.id", "RKD_Order.PZ_PlanZakaz.PlanZakaz");
                logger.Debug("RKDController - GetErrorList " + login.ToString());
                return View(rKD_Order);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - GetErrorList " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [Authorize(Roles = "Admin, OPTP, OP, Technologist")]
        [HttpPost]
        public ActionResult GetErrorList(RKD_Order rKD_Order, string folderRKD, string descriptionError, int[] PZ)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                if(PZ.Length == 0)
                    return RedirectToAction("GetErrorList", "RKD", new { idOrder = rKD_Order.id });
                if (descriptionError == null)
                    descriptionError = "";
                if (folderRKD == null || folderRKD == "")
                    return RedirectToAction("GetErrorList", "RKD", new { idOrder = rKD_Order.id });

                foreach (var id_OrderRKD in PZ)
                {
                    rKD_Order = db.RKD_Order.Find(id_OrderRKD);
                    RKD rKD = new RKD(rKD_Order.id_PZ_PlanZakaz);
                    rKD.GetErrorList(id_OrderRKD, login, folderRKD, descriptionError);
                }
                logger.Debug("RKDController - GetErrorList " + login.ToString());
                return RedirectToAction("EditTP", "RKD", new { id = rKD_Order.id });
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - GetErrorList " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("EditTP", "RKD", new { id = rKD_Order.id });
            }
        }

        // 8 step - получено согласование РКД
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        public ActionResult GetGoodList(int? idOrder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.PZ = new SelectList(db.RKD_Version.Where(d => d.activeVersion == true).Where(d => d.id_RKD_VersionWork == 5).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList(), "RKD_Order.id", "RKD_Order.PZ_PlanZakaz.PlanZakaz");
                RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
                logger.Debug("RKDController - GetGoodList " + login.ToString());
                return View(rKD_Order);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - GetGoodList " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [Authorize(Roles = "Admin, OPTP, OP, Technologist")]
        [HttpPost]
        public ActionResult GetGoodList(RKD_Order rKD_Order, string folderRKD, int[] PZ)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                if (PZ.Length == 0)
                    return RedirectToAction("GetGoodList", "RKD", new { idOrder = rKD_Order.id });
                if (folderRKD == null || folderRKD == "")
                    return RedirectToAction("GetGoodList", "RKD", new { idOrder = rKD_Order.id });
                foreach (var id_OrderRKD in PZ)
                {
                    rKD_Order = db.RKD_Order.Find(id_OrderRKD);
                    RKD rKD = new RKD(rKD_Order.id_PZ_PlanZakaz);
                    rKD.GetGoodList(rKD_Order.id, login, folderRKD);
                }
                logger.Debug("RKDController - GetGoodList " + login.ToString());
                return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - GetGoodList " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
            }
        }
        
        //EDIT PARTIAL VIEW
        [Authorize(Roles = "Admin, OPTP, OP")]
        public ActionResult EditInst(int id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order = db.RKD_Order.Find(id);
                ViewBag.id_RKD_Institute = new SelectList(db.RKD_Institute, "id", "name", rKD_Order.id_RKD_Institute);
                logger.Debug("RKDController - EditInst " + login.ToString());
                return PartialView("EditInst", rKD_Order);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - EditInst " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        [HttpPost]
        public ActionResult EditInst(RKD_Order rKD_Order)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.id_RKD_Institute = new SelectList(db.RKD_Institute, "id", "name", rKD_Order.id_RKD_Institute);
                db.Entry(rKD_Order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                logger.Debug("RKDController - EditInst " + login.ToString());
                return RedirectToAction("OpenOrder", "RKD", new { idOrder = rKD_Order.id });
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - EditInst " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }

        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        public ActionResult CreateNewDespatching(int idOrder)
        {
            RKD_Despatching rKD_Despatching = new RKD_Despatching();
            rKD_Despatching.id_RKD_Order = idOrder;

            var tmp = db.RKD_Order
                .Where(d => d.RKD_Version.Where(dz => dz.activeVersion == true).FirstOrDefault().id_RKD_VersionWork != 10)
                .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList();

            ViewBag.PZ = new SelectList(db.RKD_Version.Where(d => d.activeVersion == true).Where(d => d.id_RKD_VersionWork != 10).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList(), "RKD_Order.id", "RKD_Order.PZ_PlanZakaz.PlanZakaz");
            ViewBag.id_AspNetUsers = new SelectList(db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16 || d.Email == "bav@katek.by" || d.Email == "maj@katek.by").Where(d => d.LockoutEnabled == true || d.Email == "yury.melnikau@gmail.com").OrderBy(d => d.CiliricalName), "id", "CiliricalName");
            return PartialView("CreateNewDespatching", rKD_Despatching);
        }
        [Authorize(Roles = "Admin, KBMUser, KBEUser, KBE, KBM, Technologist, OPTP, OP, GR")]
        [HttpPost]
        public ActionResult CreateNewDespatching(RKD_Despatching rKD_Despatching, int[] PZ)
        {
            try
            {
                ViewBag.id_AspNetUsers = new SelectList(db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16 || d.Devision == 4 & d.LockoutEnabled == true).OrderBy(d => d.CiliricalName), "id", "CiliricalName");
                rKD_Despatching.dateEvent = DateTime.Now;
                if (rKD_Despatching.id_AspNetUsers == "3017921b-bfe4-4bee-865d-e7fc407bfce7")
                    rKD_Despatching.id_AspNetUsers = null;
                db.RKD_Despatching.Add(rKD_Despatching);
                db.SaveChanges();
                try
                {
                    if (PZ.Length > 0)
                    {
                        foreach (var VARIABLE in PZ)
                        {
                            RKD_Despatching newDespatching = new RKD_Despatching();
                            newDespatching.id_RKD_Order = VARIABLE;
                            newDespatching.id_AspNetUsers = rKD_Despatching.id_AspNetUsers;
                            newDespatching.dateEvent = rKD_Despatching.dateEvent;
                            newDespatching.text = rKD_Despatching.text;
                            newDespatching.dateTaskFinishDate = rKD_Despatching.dateTaskFinishDate;
                            db.RKD_Despatching.Add(newDespatching);
                            db.SaveChanges();
                        }
                    }
                }
                catch 
                {
                }
                
                return RedirectToAction("OpenOrder", "RKD", new { idOrder = rKD_Despatching.id_RKD_Order });
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - CreateNewDespatching " + ex.Message.ToString());
                return RedirectToAction("OpenOrder", "RKD", new { idOrder = rKD_Despatching.id_RKD_Order });
            }
        }
        
        [Authorize(Roles = "Admin, OPTP, OP, KBM, KBE, Technologist, GR")]
        public ActionResult Edit(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order = new RKD_Order();
                rKD_Order = db.RKD_Order.Find(id);
                ViewBag.idOrder = (int)id;
                ViewBag.idLaslVersion = rKD_Order.RKD_Version.Where(d => d.id_RKD_Order == rKD_Order.id).Count();
                ViewBag.PlanZakaz = rKD_Order.PZ_PlanZakaz.PlanZakaz.ToString();
                ViewBag.PlanZakazName = rKD_Order.PZ_PlanZakaz.Name.ToString();
                ViewBag.Client = rKD_Order.PZ_PlanZakaz.PZ_Client.Name.ToString();
                ViewBag.Inst = rKD_Order.RKD_Institute.name.ToString();
                ViewBag.login = login.ToString();
                ViewBag.QuestList = db.RKD_Question.Where(d => d.id_RKD_Order == id & d.complited == false).OrderByDescending(d => d.dateUpload).ToList();
                ViewBag.Despatching = GetListDespathingForOrder(id);
                RKD_AllView rKD_AllView = new RKD_AllView();
                rKD_AllView.ActiveOrder = db.RKD_TaskVersion.Where(d => d.RKD_Task.id_RKD_Order == id & d.RKD_Version.numberVersion1 != 0).ToList();
                var activeQuestion = db.RKD_Question.Where(d => d.complited == false & d.id_RKD_Order == rKD_Order.id).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                var closeQuestion = db.RKD_Question.Where(d => d.complited == true & d.id_RKD_Order == rKD_Order.id).ToList();
                ViewBag.ActiveQuestion = activeQuestion;
                ViewBag.CloseQuestion = closeQuestion;
                ViewBag.PredcwssorOrderLink = GetPredcessorOrderNumForLink(id.Value);
                ViewBag.PoslOrderLink = GetPostOrderNumForLink(id.Value);
                logger.Debug("RKDController - Edit " + login.ToString());
                return View(rKD_AllView);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - Edit " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        
        [Authorize(Roles = "Admin, OPTP, OP, KBM, KBE, Technologist, GR")]
        public ActionResult EditTP(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order = new RKD_Order();
                rKD_Order = db.RKD_Order.Find(id);
                ViewBag.idOrder = id;
                ViewBag.idLaslVersion = rKD_Order.RKD_Version.Where(d => d.id_RKD_Order == rKD_Order.id).Count();
                ViewBag.PlanZakaz = rKD_Order.PZ_PlanZakaz.PlanZakaz.ToString();
                ViewBag.PlanZakazName = rKD_Order.PZ_PlanZakaz.Name.ToString();
                ViewBag.Client = rKD_Order.PZ_PlanZakaz.PZ_Client.Name.ToString();
                ViewBag.Inst = rKD_Order.RKD_Institute.name.ToString();
                ViewBag.login = login.ToString();
                ViewBag.QuestList = db.RKD_Question.Where(d => d.id_RKD_Order == id & d.complited == false).OrderByDescending(d => d.dateUpload).ToList();
                ViewBag.Despatching = GetListDespathingForOrder(id);
                RKD_AllView rKD_AllView = new RKD_AllView();
                rKD_AllView.ActiveOrder = db.RKD_TaskVersion.Where(d => d.RKD_Task.id_RKD_Order == id & d.RKD_Version.numberVersion1 != 0).ToList();
                var activeQuestion = db.RKD_Question.Where(d => d.complited == false & d.id_RKD_Order == rKD_Order.id).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                var closeQuestion = db.RKD_Question.Where(d => d.complited == true & d.id_RKD_Order == rKD_Order.id).ToList();
                ViewBag.ActiveQuestion = activeQuestion;
                ViewBag.CloseQuestion = closeQuestion;
                logger.Debug("RKDController - EditTP " + login.ToString());
                return View(rKD_AllView);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - EditTP " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        
        [Authorize(Roles = "Admin, OPTP, OP, Technologist")]
        public ActionResult EditTask(int idTask)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(idTask);
                ViewBag.idOrder = rKD_TaskVersion.RKD_Version.id_RKD_Order;
                logger.Debug("RKDController - EditTask " + login.ToString());
                return PartialView("EditTask", rKD_TaskVersion);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - EditTask " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        
        //Получена информация для корректировки статуса задачи
        [Authorize(Roles = "Admin, OPTP, OP, Technologist")]
        public ActionResult EditTaskStatus(int? id_task, int typeTask)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(id_task);
                RKD rKD = new RKD(rKD_TaskVersion.RKD_Version.RKD_Order.id_PZ_PlanZakaz);
                rKD.UpdateRKD_TaskVersionComplited(rKD_TaskVersion.id, login, typeTask);
                
                logger.Debug("RKDController - EditTaskStatus " + login.ToString());
                return RedirectToAction("OpenOrder", "RKD", new { idOrder = rKD_TaskVersion.RKD_Version.RKD_Order.id });
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - EditTaskStatus " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }
        
        public ActionResult PutNewVersion(int idOrder)
        {
            RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
            return View(rKD_Order);
        }
        [HttpPost]
        public ActionResult PutNewVersion(RKD_Order rKD_Order)
        {
            string login = HttpContext.User.Identity.Name;
            RKD rKD = new RKD(rKD_Order.id_PZ_PlanZakaz);
            rKD.LoadNewVersionRKD(rKD_Order, login, rKD_Order.datePlanCritical);
            return RedirectToAction("OpenOrder", "RKD", new { idOrder = rKD_Order.id });
        }
        
        public ActionResult PreSuccessOrder(int idOrder, int typeTask)
        {
            RKD_Order rKD_Order = db.RKD_Order.Find(idOrder);
            string login = HttpContext.User.Identity.Name;
            RKD rKD = new RKD(rKD_Order.id_PZ_PlanZakaz);
            rKD.GetPreGoodList(rKD_Order.id, login, typeTask);
            return RedirectToAction("Details", "RKD", new { id = rKD_Order.id });
        }
        
        //[Authorize(Roles = "Admin, OPTP, OP, KBM, KBE, Technologist, KBMUser, KBEUser")]
        public ActionResult Details(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.linkToExcel = @"http://pserver/RKD/ExportToExcel/" + id.ToString();
                ViewBag.linkToExcelAll = @"http://pserver/RKD/ExportToExcelAll/" + id.ToString();
                //ViewBag.linkToExcel = @"http://localhost:57314/RKD/ExportToExcel/" + id.ToString();
                RKD_Order rKD_Order = new RKD_Order();
                rKD_Order = db.RKD_Order.Find(id);
                ViewBag.idOrder = id;
                ViewBag.idLaslVersion = rKD_Order.RKD_Version.Where(d => d.id_RKD_Order == rKD_Order.id).Count();
                ViewBag.PlanZakaz = rKD_Order.PZ_PlanZakaz.PlanZakaz.ToString();
                ViewBag.PlanZakazName = rKD_Order.PZ_PlanZakaz.Name.ToString();
                ViewBag.Client = rKD_Order.PZ_PlanZakaz.PZ_Client.Name.ToString();
                ViewBag.Inst = rKD_Order.RKD_Institute.name.ToString();
                ViewBag.login = login.ToString();
                ViewBag.QuestList = db.RKD_Question.Where(d => d.id_RKD_Order == id & d.complited == false).OrderByDescending(d => d.dateUpload).ToList();
                ViewBag.Despatching = GetListDespathingForOrder(id);
                ViewBag.PredcwssorOrderLink = GetPredcessorOrderNumForLink(id.Value);
                ViewBag.PoslOrderLink = GetPostOrderNumForLink(id.Value);
                RKD_AllView rKD_AllView = new RKD_AllView();
                rKD_AllView.ActiveOrder = db.RKD_TaskVersion.Where(d => d.RKD_Task.id_RKD_Order == id & d.RKD_Version.numberVersion1 != 0).ToList();
                var activeQuestion = db.RKD_Question.Where(d => d.complited == false & d.id_RKD_Order == rKD_Order.id).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                var closeQuestion = db.RKD_Question.Where(d => d.complited == true & d.id_RKD_Order == rKD_Order.id).ToList();
                ViewBag.ActiveQuestion = activeQuestion;
                ViewBag.CloseQuestion = closeQuestion;
                ViewBag.FailBP = db.RKD_FailBPlan.Where(d => d.ProjectUID == rKD_Order.PZ_PlanZakaz.ProjectUID).ToList();
                logger.Debug("RKDController - Details " + login.ToString());
                return View(rKD_AllView);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - Details " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }

        public ActionResult DetailsClose(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                RKD_Order rKD_Order = new RKD_Order();
                rKD_Order = db.RKD_Order.Find(id);
                ViewBag.idOrder = id;
                ViewBag.idLaslVersion = rKD_Order.RKD_Version.Where(d => d.id_RKD_Order == rKD_Order.id).Count();
                ViewBag.PlanZakaz = rKD_Order.PZ_PlanZakaz.PlanZakaz.ToString();
                ViewBag.PlanZakazName = rKD_Order.PZ_PlanZakaz.Name.ToString();
                ViewBag.Client = rKD_Order.PZ_PlanZakaz.PZ_Client.Name.ToString();
                ViewBag.Inst = rKD_Order.RKD_Institute.name.ToString();
                ViewBag.login = login.ToString();
                ViewBag.QuestList = db.RKD_Question.Where(d => d.id_RKD_Order == id & d.complited == false).OrderByDescending(d => d.dateUpload).ToList();
                ViewBag.Despatching = GetListDespathingForOrder(id);
                ViewBag.PredcwssorOrderLink = GetPredcessorOrderNumForLink(id.Value);
                ViewBag.PoslOrderLink = GetPostOrderNumForLink(id.Value);
                RKD_AllView rKD_AllView = new RKD_AllView();
                rKD_AllView.ActiveOrder = db.RKD_TaskVersion.Where(d => d.RKD_Task.id_RKD_Order == id & d.RKD_Version.numberVersion1 != 0).ToList();
                var activeQuestion = db.RKD_Question.Where(d => d.complited == false & d.id_RKD_Order == rKD_Order.id).OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
                var closeQuestion = db.RKD_Question.Where(d => d.complited == true & d.id_RKD_Order == rKD_Order.id).ToList();
                ViewBag.ActiveQuestion = activeQuestion;
                ViewBag.CloseQuestion = closeQuestion;
                ViewBag.FailBP = db.RKD_FailBPlan.Where(d => d.ProjectUID == rKD_Order.PZ_PlanZakaz.ProjectUID).ToList();
                logger.Debug("RKDController - Details " + login.ToString());
                return View(rKD_AllView);
            }
            catch (Exception ex)
            {
                logger.Error("RKDController - Details " + login.ToString() + " " + ex.Message.ToString());
                return RedirectToAction("Error", "RKD");
            }
        }

        public ActionResult GoToMiting(int idOrder)
        {
            return RedirectToAction("Index", "RKD");
        }

        public void ExportToExcel(int id)
        {
            var collectionData = GetListDespathingForOrder(id).Where(d => d.TypeTask != 100);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            int row = 1;
            ws.Cells[string.Format("A{0}", row)].Value = "Дата/время события";
            ws.Cells[string.Format("B{0}", row)].Value = "№ заказа";
            ws.Cells[string.Format("C{0}", row)].Value = "Событие/описание";
            ws.Cells[string.Format("D{0}", row)].Value = "Срок";
            ws.Cells[string.Format("E{0}", row)].Value = "Исполнитель";
            ws.Cells[string.Format("A{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("B{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("C{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("D{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("E{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            foreach (var data in collectionData)
            {
                row++;
                ws.Cells[string.Format("A{0}", row)].Value = data.DateEvent.ToString();
                ws.Cells[string.Format("B{0}", row)].Value = data.PlanZakazName;
                ws.Cells[string.Format("C{0}", row)].Value = data.TextData;
                ws.Cells[string.Format("D{0}", row)].Value = data.TaskFinishDate;
                ws.Cells[string.Format("E{0}", row)].Value = data.UserID;
                ws.Cells[string.Format("A{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("B{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("C{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("D{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("E{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-daisposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public void ExportToExcelAll(int id)
        {
            var collectionData = GetListDespathingForOrder(id);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            int row = 1;
            ws.Cells[string.Format("A{0}", row)].Value = "Дата/время события";
            ws.Cells[string.Format("B{0}", row)].Value = "№ заказа";
            ws.Cells[string.Format("C{0}", row)].Value = "Событие/описание";
            ws.Cells[string.Format("D{0}", row)].Value = "Срок";
            ws.Cells[string.Format("E{0}", row)].Value = "Исполнитель";
            ws.Cells[string.Format("A{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("B{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("C{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("D{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("E{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            foreach (var data in collectionData)
            {
                row++;
                ws.Cells[string.Format("A{0}", row)].Value = data.DateEvent.ToString();
                ws.Cells[string.Format("B{0}", row)].Value = data.PlanZakazName;
                ws.Cells[string.Format("C{0}", row)].Value = data.TextData;
                ws.Cells[string.Format("D{0}", row)].Value = data.TaskFinishDate;
                ws.Cells[string.Format("E{0}", row)].Value = data.UserID;
                ws.Cells[string.Format("A{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("B{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("C{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("D{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("E{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-daisposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        
        public void ExportToExcelOrders()
        {
            var collectionData = db.RKD_Order.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            int row = 1;
            ws.Cells[string.Format("A{0}", row)].Value = "Заказ №";
            ws.Cells[string.Format("B{0}", row)].Value = "Состояние согласования";
            ws.Cells[string.Format("C{0}", row)].Value = "ГИП КБМ";
            ws.Cells[string.Format("D{0}", row)].Value = "ГИП КБЭ";
            ws.Cells[string.Format("E{0}", row)].Value = "Заказчик";
            ws.Cells[string.Format("E{0}", row)].Value = "Дата открытия";
            ws.Cells[string.Format("F{0}", row)].Value = "Контрактная дата отгрузки";
            ws.Cells[string.Format("G{0}", row)].Value = "Плановая дата отгрузки";
            ws.Cells[string.Format("H{0}", row)].Value = "Текущая версия РКД";

            ws.Cells[string.Format("A{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("B{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("C{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("D{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("E{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("F{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("G{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("H{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            foreach (var data in collectionData)
            {
                row++;
                ws.Cells[string.Format("A{0}", row)].Value = data.PZ_PlanZakaz.PlanZakaz.ToString();
                ws.Cells[string.Format("B{0}", row)].Value = data.RKD_Version.Where(d => d.activeVersion).First().RKD_VersionWork.name.ToString();
                try
                {
                    ws.Cells[string.Format("C{0}", row)].Value = data.RKD_GIP.First().AspNetUsers1.CiliricalName;
                }
                catch
                {
                    ws.Cells[string.Format("C{0}", row)].Value = "";
                }
                try
                {
                    ws.Cells[string.Format("D{0}", row)].Value = data.RKD_GIP.First().AspNetUsers.CiliricalName;
                }
                catch
                {
                    ws.Cells[string.Format("D{0}", row)].Value = "";
                }
                ws.Cells[string.Format("E{0}", row)].Value = data.PZ_PlanZakaz.PZ_Client.NameSort;
                ws.Cells[string.Format("F{0}", row)].Value = data.PZ_PlanZakaz.DateCreate.ToString().Substring(0, 10);
                ws.Cells[string.Format("G{0}", row)].Value = data.PZ_PlanZakaz.dataOtgruzkiBP.ToString().Substring(0, 10);
                ws.Cells[string.Format("H{0}", row)].Value = "V " + data.RKD_Version.Where(d => d.activeVersion).First().numberVersion1.ToString() + "." + data.RKD_Version.Where(d => d.activeVersion).First().numberVersion2.ToString();

                ws.Cells[string.Format("A{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("B{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("C{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("D{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("E{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("F{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("G{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("H{0}", row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-daisposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
    }
}
