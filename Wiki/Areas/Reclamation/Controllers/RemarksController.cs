﻿using Newtonsoft.Json;
using NLog;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using Wiki.Areas.Reclamation.Models;
using Wiki.Models;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class RemarksController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private PortalKATEKEntities db = new PortalKATEKEntities();
        private readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        private readonly JsonSerializerSettings longSettingYY = new JsonSerializerSettings { DateFormatString = "dd.MM.yy HH:mm" };

        public ActionResult Debug()
        {
            ProjectServer projectServer = new ProjectServer();
            projectServer.CreateTasks();
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.id_ReclamationTypeKB = new SelectList(db.ReclamationTypeKB.Where(d => d.id != 26).OrderBy(d => d.name), "id", "name");
            @ViewBag.idPZ = 0;
            string login = HttpContext.User.Identity.Name;
            ViewBag.id_DevisionReclamation = new SelectList(db.Devision.Where(d => d.id == 0).OrderBy(d => d.name), "id", "name");
            ViewBag.ButtonAddActivation = 0;
            int id_Devision = 0;
            ViewBag.Devision = id_Devision;
            try
            {
                id_Devision = db.AspNetUsers.FirstOrDefault(d => d.Email == login).Devision.Value;
            }
            catch
            {
            }
            ViewBag.DevisionsManufacturing = new SelectList(new DevisionsManufacturing().Devisions.OrderBy(d => d.name), "id", "name");
            if (login == "fvs@katek.by" || login == "myi@katek.by")
            {
                List<Devision> devisions = db.Devision.Where(d => d.OTK == true).ToList();
                ViewBag.id_DevisionReclamation = new SelectList(devisions.OrderBy(d => d.name), "id", "name");
                ViewBag.ButtonAddActivation = 1;
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "nrf@katek.by")
            {
                List<Devision> devisions = db.Devision.Where(d => d.OTK == true).ToList();
                ViewBag.id_DevisionReclamation = new SelectList(devisions.OrderBy(d => d.name), "id", "name");
                ViewBag.ButtonAddActivation = 1;
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "vi@katek.by")
            {
                List<Devision> devisions = db.Devision.Where(d => d.OTK == true).ToList();
                ViewBag.id_DevisionReclamation = new SelectList(devisions.OrderBy(d => d.name), "id", "name");
                ViewBag.ButtonAddActivation = 1;
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "bav@katek.by")
            {
                List<Devision> devisions = db.Devision.Where(d => d.OTK == true).ToList();
                ViewBag.id_DevisionReclamation = new SelectList(devisions.OrderBy(d => d.name), "id", "name");
                ViewBag.ButtonAddActivation = 1;
                ViewBag.CRUDCounter = '1';
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 0)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            }
            else if (id_Devision == 8 || id_Devision == 9 || id_Devision == 10 || id_Devision == 20 || id_Devision == 22 || id_Devision == 5)
            {
                List<Devision> devisions = db.Devision.Where(d => d.id == 15 || d.id == 16 || d.id == 12 || d.id == 18).ToList();
                ViewBag.id_DevisionReclamation = new SelectList(devisions.OrderBy(d => d.name), "id", "name");
                ViewBag.ButtonAddActivation = 1;
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == id_Devision)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '3';
                ViewBag.DevisionsManufacturing = new SelectList(db.Devision.Where(d => d.id == id_Devision), "id", "name");
            }
            else
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers.Where(d => d.Email == login), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '4';
            }
            if (id_Devision == 9 || id_Devision == 22)
                ViewBag.DevisionsManufacturing = new SelectList(db.Devision.Where(d => d.id == 9 || d.id == 22), "id", "name");
            if (id_Devision == 8 || id_Devision == 20)
                ViewBag.DevisionsManufacturing = new SelectList(db.Devision.Where(d => d.id == 8 || d.id == 20), "id", "name");
            if (id_Devision == 6)
            {
                List<Devision> devisions = db.Devision.Where(d => d.OTK == true).ToList();
                ViewBag.id_DevisionReclamation = new SelectList(devisions.OrderBy(d => d.name), "id", "name");
                ViewBag.ButtonAddActivation = 1;
                ViewBag.CRUDCounter = '1';
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
            }
            else if (id_Devision == 16 || id_Devision == 15)
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activePO == true).OrderBy(d => d.name), "id", "name");
            else
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
            List<Devision> devisionsReload = db.Devision.Where(d => d.OTK == true && d.id != id_Devision).ToList();
            if (id_Devision == 6)
                devisionsReload = db.Devision.Where(d => d.OTK == true).ToList();
            ViewBag.id_DevisionReclamationReload = new SelectList(devisionsReload.OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFirst = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFinal = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            DateTime dateTimeSh = DateTime.Now.AddDays(-30);
            ViewBag.PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.PlanZakaz < 9000).OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_PF = new SelectList(db.PF.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            if (login == "pev@katek.by" || login == "myi@katek.by" || login == "omo@katek.by")
            {
                ViewBag.ClosePZReclamation = 1;
            }
            else
            {
                ViewBag.ClosePZReclamation = 0;
            }
            if (id_Devision == 28)
            {
                List<Devision> devisions = db.Devision.Where(d => d.OTK == true).ToList();
                ViewBag.id_DevisionReclamation = new SelectList(devisions.OrderBy(d => d.name), "id", "name");
                ViewBag.ButtonAddActivation = 1;
                ViewBag.CRUDCounter = '1';
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
                ViewBag.PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz.OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            }
            logger.Debug("Reclamation: " + login);
            return View();
        }

        [HttpPost]
        public JsonResult ReclamationsPlanZakaz(int id)
        {
            @ViewBag.idPZ = id;
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamationPlanZakaz(GetIdDevision(login), id);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        [HttpPost]
        public JsonResult ReclamationsPlanZakazMy(int id)
        {
            @ViewBag.idPZ = id;
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamationPlanZakaz(id, login, false);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        [HttpPost]
        public JsonResult ChackList(int id)
        {
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamationPlanZakaz(id);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult PlanZakazDevisionAll()
        {
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs();
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult ActiveReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            if (login == "nrf@katek.by" || login == "fvs@katek.by" || login == "vi@katek.by" || login == "myi@katek.by")
                reclamationListViewer.GetReclamation(GetIdDevision(login), false, login);
            else
                reclamationListViewer.GetReclamation(GetIdDevision(login), false);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult ActiveReclamationMy()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(login, false);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult PlanZakazDevisionNotSh()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login), false);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult PlanZakazDevisionNotShMy()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(db.AspNetUsers.First(d => d.Email == login).Id);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult PlanZakazDevisionSh()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login), true);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult AllReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            if (login == "nrf@katek.by" || login == "myi@katek.by")
                reclamationListViewer.GetReclamation(0, 0);
            else
                reclamationListViewer.GetReclamation(GetIdDevision(login));
            return Json(new { data = reclamationListViewer.ReclamationsListView.OrderByDescending(d => d.Id_Reclamation).Take(1500) });
        }

        public JsonResult CloseReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            if (login == "nrf@katek.by" || login == "myi@katek.by")
                reclamationListViewer.GetReclamation(GetIdDevision(login), true, login);
            else
                reclamationListViewer.GetReclamation(GetIdDevision(login), true);
            return Json(new { data = reclamationListViewer.ReclamationsListView.OrderByDescending(d => d.Id_Reclamation).Take(1500) });
        }

        public JsonResult MyReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(login, true);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult EditManufList()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation();
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult Add(Wiki.Reclamation reclamation, int[] pZ_PlanZakaz)
        {
            string login = HttpContext.User.Identity.Name;
            reclamation.dateTimeCreate = DateTime.Now;
            CreateReclamation correctReclamation = new CreateReclamation(reclamation, login);
            reclamation = correctReclamation.Reclamation;
            db.Reclamation.Add(reclamation);
            db.SaveChanges();
            CreateReclamation_PZ(pZ_PlanZakaz, reclamation.id);
            if (reclamation.technicalAdvice == true)
                CreateTechnicalAdvice(reclamation.id, reclamation.id_AspNetUsersCreate);
            int id_Devision = reclamation.id_DevisionCreate;
            if (id_Devision == 8 || id_Devision == 9 || id_Devision == 10 || id_Devision == 20 || id_Devision == 22)
            {
                if (reclamation.editManufacturing == true)
                {
                    login = db.AspNetUsers.First(d => d.Devision == reclamation.editManufacturingIdDevision && d.ResourceUID != null).Email;
                    try
                    {
                        TaskForPWA pwa = new TaskForPWA();
                        string reclamationNumAndText = reclamation.id.ToString() + " " + reclamation.text;
                        var projects = reclamation.Reclamation_PZ.ToList();
                        foreach (var data in projects)
                        {
                            var projectUid = db.PZ_PlanZakaz.Find(data.id_PZ_PlanZakaz).ProjectUID;
                            if (projectUid != null)
                                pwa.CreateTaskOTK_PO((Guid)projectUid, reclamationNumAndText, login);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            EmailReclamation emailReclamation = new EmailReclamation(reclamation, login, 1);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteOrder(Wiki.Reclamation reclamation)
        {
            string login = HttpContext.User.Identity.Name;
            Wiki.Reclamation order = db.Reclamation.Find(reclamation.id);
            _ = new EmailReclamation(order, login, 4);

            foreach (var data in db.Reclamation_PZ.Where(d => d.id_Reclamation == reclamation.id))
            {
                db.Reclamation_PZ.Remove(data);
            }
            db.Reclamation.Remove(order);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Wiki.Reclamation reclamation, int[] pZ_PlanZakaz, string answerText,
            bool? reload, int? reloadDevision, bool? trash)
        {
            string login = HttpContext.User.Identity.Name;
            AspNetUsers aspNetUser = db.AspNetUsers.First(d => d.Email == login);
            if (aspNetUser.Devision == 6)
            {
                if (reclamation.close == true)
                {
                    string textAnswer = "";
                    textAnswer = "Замечание закрыто ОТК";
                    Reclamation_Answer reclamation_Answer = new Reclamation_Answer
                    {
                        answer = textAnswer,
                        dateTimeCreate = DateTime.Now,
                        id_AspNetUsersCreate = aspNetUser.Id,
                        id_Reclamation = reclamation.id,
                        trash = false
                    };
                    db.Reclamation_Answer.Add(reclamation_Answer);
                    db.SaveChanges();
                }
            }
            CreateReclamation correctPlanZakaz = new CreateReclamation(reclamation, login, reload, reloadDevision);
            reclamation = correctPlanZakaz.Reclamation;
            db.Entry(reclamation).State = EntityState.Modified;
            db.SaveChanges();
            if (aspNetUser.Devision.Value == 6 && answerText != "" && answerText != null)
            {
                Reclamation_Answer reclamation_Answer = new Reclamation_Answer
                {
                    answer = answerText,
                    dateTimeCreate = DateTime.Now,
                    id_AspNetUsersCreate = aspNetUser.Id,
                    id_Reclamation = reclamation.id,
                    trash = trash.Value
                };
                db.Reclamation_Answer.Add(reclamation_Answer);
                db.SaveChanges();
                if (reclamation.close != true)
                {
                    reclamation.closeDevision = false;
                    db.Entry(reclamation).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else if (aspNetUser.Devision.Value == 28 && answerText != "" && answerText != null)
            {
                Reclamation_Answer reclamation_Answer = new Reclamation_Answer
                {
                    answer = answerText,
                    dateTimeCreate = DateTime.Now,
                    id_AspNetUsersCreate = aspNetUser.Id,
                    id_Reclamation = reclamation.id,
                    trash = trash.Value
                };
                db.Reclamation_Answer.Add(reclamation_Answer);
                db.SaveChanges();
            }
            else if (aspNetUser.Devision.Value != 6 && aspNetUser.Devision.Value != 28 && answerText != "" && answerText != null)
            {
                Reclamation_Answer reclamation_Answer = new Reclamation_Answer
                {
                    answer = answerText,
                    dateTimeCreate = DateTime.Now,
                    id_AspNetUsersCreate = aspNetUser.Id,
                    id_Reclamation = reclamation.id,
                    trash = trash.Value
                };
                db.Reclamation_Answer.Add(reclamation_Answer);
                db.SaveChanges();
                if (reload != true)
                {
                    if (answerText != "-")
                    {
                        EmailReclamation emailReclamation = new EmailReclamation(reclamation, login, 3);
                    }
                }
            }
            if (reclamation.technicalAdvice == true)
                UpdateTechnicalAdvice(reclamation.id, aspNetUser.Id);
            UpdateReclamation_PZ(pZ_PlanZakaz, reclamation.id);
            if (reload == true)
            {
                Reclamation_Answer reclamation_Answer = new Reclamation_Answer
                {
                    answer = "Замечание перенаправлено на " + db.Devision.Find(reloadDevision).name,
                    dateTimeCreate = DateTime.Now,
                    id_AspNetUsersCreate = aspNetUser.Id,
                    id_Reclamation = reclamation.id,
                    trash = trash.Value
                };
                db.Reclamation_Answer.Add(reclamation_Answer);
                db.SaveChanges();
                EmailReclamation emailReclamation = new EmailReclamation(reclamation, login, 2);
            }
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetOneReclamation(reclamation.id);
            try
            {
                if (login == "fvs@katek.by" || login == "nrf@katek.by" || login == "vi@katek.by")
                {
                    if (reclamation.id_DevisionReclamation == 15 || reclamation.id_DevisionReclamation == 16 || reclamation.id_DevisionReclamation == 12 || reclamation.id_DevisionReclamation == 18)
                    {
                        EmailReclamation emailReclamation = new EmailReclamation(reclamation, login, 5);
                    }
                }
            }
            catch
            {

            }
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult GetReclamation(int id)
        {
            var query = db.Reclamation.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id_ReclamationTypeKB,
                dataList.id,
                dataList.fixedExpert,
                dataList.id_Reclamation_Type,
                dataList.id_DevisionReclamation,
                dataList.id_Reclamation_CountErrorFirst,
                dataList.id_Reclamation_CountErrorFinal,
                id_AspNetUsersCreate = dataList.AspNetUsers.CiliricalName,
                dataList.id_DevisionCreate,
                dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, longSettingYY).Replace(@"""", ""),
                dataList.text,
                dataList.description,
                dataList.timeToSearch,
                dataList.timeToEliminate,
                dataList.close,
                dataList.gip,
                dataList.closeDevision,
                dataList.PCAM,
                dataList.editManufacturing,
                dataList.editManufacturingIdDevision,
                dataList.id_PF,
                dataList.technicalAdvice,
                dataList.id_AspNetUsersError,
                pZ_PlanZakaz = GetPlanZakazArray(dataList.Reclamation_PZ.ToList()),
                answerHistiryText = GetAnswerText(dataList.Reclamation_Answer.ToList())
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public string GetAnswerText(List<Reclamation_Answer> reclamation_Answers)
        {
            string text = "";
            if (reclamation_Answers.Count > 0)
            {
                foreach (var data in reclamation_Answers.OrderByDescending(d => d.dateTimeCreate))
                {
                    text += data.dateTimeCreate.ToString().Substring(0, 5) + " | " + data.AspNetUsers.CiliricalName + " | " + data.answer + "\n";
                }
            }
            return text;
        }

        private string[] GetPlanZakazArray(List<Reclamation_PZ> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_PZ_PlanZakaz.ToString();
            }
            return pZ_PlanZakaz;
        }

        private int GetIdDevision(string loginUser)
        {
            int id_Devision = 0;
            try
            {
                id_Devision = db.AspNetUsers.First(d => d.Email == loginUser).Devision.Value;
            }
            catch
            {
            }
            return id_Devision;
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

        private bool CreateReclamation_PZ(int[] pZ_PlanZakaz, int id_Reclamation)
        {
            foreach (var pz in pZ_PlanZakaz)
            {
                Reclamation_PZ reclamation_PZ = new Reclamation_PZ
                {
                    id_PZ_PlanZakaz = pz,
                    id_Reclamation = id_Reclamation
                };
                db.Reclamation_PZ.Add(reclamation_PZ);
                db.SaveChanges();
            }
            return true;
        }

        private bool CreateTechnicalAdvice(int id_Reclamation, string id_AspNetUser)
        {
            Reclamation_TechnicalAdvice technicalAdvice = new Reclamation_TechnicalAdvice
            {
                description = "",
                id_AspNetUsersCreate = id_AspNetUser,
                id_Reclamation = id_Reclamation,
                dateTimeCreate = DateTime.Now,
                close = false,
                text = ""
            };
            db.Reclamation_TechnicalAdvice.Add(technicalAdvice);
            db.SaveChanges();
            return true;
        }

        private bool UpdateTechnicalAdvice(int id_Reclamation, string aspNetUser)
        {
            if (db.Reclamation_TechnicalAdvice.Where(d => d.id_Reclamation == id_Reclamation).Count() == 0)
            {
                Reclamation_TechnicalAdvice technicalAdvice = new Reclamation_TechnicalAdvice
                {
                    description = "",
                    id_AspNetUsersCreate = aspNetUser,
                    id_Reclamation = id_Reclamation,
                    dateTimeCreate = DateTime.Now,
                    close = false,
                    text = ""
                };
                db.Reclamation_TechnicalAdvice.Add(technicalAdvice);
                db.SaveChanges();
            }
            return true;
        }

        private bool UpdateReclamation_PZ(int[] pZ_PlanZakaz, int id_Reclamation)
        {
            var listLastPz = db.Reclamation_PZ.Where(d => d.id_Reclamation == id_Reclamation).ToList();
            foreach (var lastReclamationPZ in listLastPz)
            {
                if (pZ_PlanZakaz.Where(d => d == lastReclamationPZ.id_PZ_PlanZakaz).Count() == 0)
                {
                    db.Reclamation_PZ.Remove(lastReclamationPZ);
                    db.SaveChanges();
                }
            }
            listLastPz = db.Reclamation_PZ.Where(d => d.id_Reclamation == id_Reclamation).ToList();
            foreach (var newReclamationPZ in pZ_PlanZakaz)
            {
                if (listLastPz.Where(d => d.id_PZ_PlanZakaz == newReclamationPZ).Count() == 0)
                {
                    Reclamation_PZ reclamation_PZ = new Reclamation_PZ
                    {
                        id_PZ_PlanZakaz = newReclamationPZ,
                        id_Reclamation = id_Reclamation
                    };
                    db.Reclamation_PZ.Add(reclamation_PZ);
                    db.SaveChanges();
                }
            }
            return true;
        }

        public JsonResult GetRemarksOTK()
        {
            string login = HttpContext.User.Identity.Name;
            if (login == "pev@katek.by" || login == "omo@katek.by")
            {
                var data = new TARemarksListView().GetRemarksOTK();
                return Json(new { data });
            }
            else
            {
                var data = new TARemarksListView().GetRemarksNull();
                return Json(new { data });
            }
        }

        public JsonResult GetRemarksPO()
        {
            if (HttpContext.User.Identity.Name == "bav@katek.by" || HttpContext.User.Identity.Name == "myi@katek.by")
            {
                var data = new TARemarksListView().GetRemarksPO();
                return Json(new { data });
            }
            else
            {
                var data = new TARemarksListView().GetRemarksNull();
                return Json(new { data });
            }
        }

        public JsonResult ExpertComplitedAll()
        {
            string login = HttpContext.User.Identity.Name;
            if (login == "pev@katek.by" || login == "omo@katek.by")
            {
                foreach (Wiki.Reclamation data in db.Reclamation.Where(d => d.fixedExpert == false && d.id_DevisionCreate == 6))
                {
                    Wiki.Reclamation reclamation = data;
                    reclamation.fixedExpert = true;
                    db.Entry(reclamation).State = EntityState.Modified;
                }
                db.SaveChanges();
                return GetRemarksOTK();
            }
            else if (HttpContext.User.Identity.Name == "bav@katek.by")
            {
                foreach (Wiki.Reclamation data in db.Reclamation.Where(d => d.fixedExpert == false && d.id_DevisionCreate != 6))
                {
                    Wiki.Reclamation reclamation = data;
                    reclamation.fixedExpert = true;
                    db.Entry(reclamation).State = EntityState.Modified;
                }
                db.SaveChanges();
                return GetRemarksPO();
            }
            else
            {
                return GetRemarksPO();
            }
        }

        public void CreateExcelReport(Wiki.Reclamation reclamation)
        {
            PortalKATEKEntities dbc = new PortalKATEKEntities();
            dbc.Configuration.ProxyCreationEnabled = false;
            dbc.Configuration.LazyLoadingEnabled = false;
            string login = HttpContext.User.Identity.Name;
            AspNetUsers user = db.AspNetUsers.First(d => d.Email == login);
            List<Wiki.Reclamation> list = new List<Wiki.Reclamation>();
            if (reclamation.close == true)
            {
                list = dbc.Reclamation
                    .Include(d => d.Reclamation_PZ.Select(c => c.PZ_PlanZakaz))
                    .Include(d => d.AspNetUsers)
                    .Include(d => d.AspNetUsers1)
                    .Include(d => d.Devision)
                    .Include(d => d.Reclamation_Answer.Select(cd => cd.AspNetUsers))
                    .Include(d => d.Reclamation_CountError)
                    .Include(d => d.Reclamation_CountError1)
                    .Include(d => d.PF)
                    .ToList();
            }
            else if (reclamation.closeDevision == true)
            {
                list = dbc.Reclamation.Where(d => d.id_DevisionReclamation == user.Devision)
                    .Include(d => d.Reclamation_PZ.Select(c => c.PZ_PlanZakaz))
                    .Include(d => d.AspNetUsers)
                    .Include(d => d.Reclamation_Answer.Select(cd => cd.AspNetUsers))
                    .Include(d => d.AspNetUsers1)
                    .Include(d => d.Devision)
                    .Include(d => d.Reclamation_CountError)
                    .Include(d => d.Reclamation_CountError1)
                    .Include(d => d.PF)
                    .ToList();
            }
            else if (reclamation.closeMKO == true)
            {
                list = dbc.Reclamation.Where(d => d.id_AspNetUsersCreate == user.Id)
                    .Include(d => d.Reclamation_PZ.Select(c => c.PZ_PlanZakaz))
                    .Include(d => d.AspNetUsers)
                    .Include(d => d.AspNetUsers1)
                    .Include(d => d.Reclamation_Answer.Select(cd => cd.AspNetUsers))
                    .Include(d => d.Devision)
                    .Include(d => d.Reclamation_CountError)
                    .Include(d => d.Reclamation_CountError1)
                    .Include(d => d.PF)
                    .ToList();
            }
            else if (reclamation.gip == true)
            {
                list = dbc.Reclamation.Where(d => d.id_AspNetUsersError == user.Id)
                    .Include(d => d.Reclamation_PZ.Select(c => c.PZ_PlanZakaz))
                    .Include(d => d.AspNetUsers)
                    .Include(d => d.AspNetUsers1)
                    .Include(d => d.Reclamation_Answer.Select(cd => cd.AspNetUsers))
                    .Include(d => d.Devision)
                    .Include(d => d.Reclamation_CountError)
                    .Include(d => d.Reclamation_CountError1)
                    .Include(d => d.PF)
                    .ToList();
            }
            else
            {
            }
            if (list.Count != 0)
            {
                List<Wiki.Models.ExcelRow> excelRows = new List<Wiki.Models.ExcelRow>();
                Wiki.Models.ExcelRow excelRow = new Wiki.Models.ExcelRow("№", "План-Заказ/ы №№:", "Автор замечания", "Создана", "Ответственное СП", "Ответственный сотрудник", "Критерий ошибки",
                    "Критерий ошибки (утв.)", "Поиск (ч.)", "Устранение (ч.)", "Текст замечания", "Прим.", "Полуфабрикат", "РСАМ", "История переписки", "На техсовет", "ГИП",
                    "Шум", "", "", "", "", "", "", "", "", 18);
                excelRows.Add(excelRow);
                foreach (var data in list)
                {
                    string ordersName = "";
                    foreach (var dataOrders in data.Reclamation_PZ)
                    {
                        ordersName += dataOrders.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
                    }
                    string history = "";
                    foreach (var dataAnswer in data.Reclamation_Answer)
                    {
                        history += dataAnswer.AspNetUsers.CiliricalName + " | " + dataAnswer.answer + "\n";
                    }
                    string trash = "Нет";
                    foreach (var dataAnswer in data.Reclamation_Answer)
                    {
                        if (dataAnswer.trash == true)
                            trash = "Да";
                    }
                    string userError = "";
                    try
                    {
                        userError = data.AspNetUsers1.CiliricalName;
                    }
                    catch
                    {
                        userError = "";
                    }
                    Wiki.Models.ExcelRow excelRow1 = new Wiki.Models.ExcelRow(data.id.ToString(), ordersName, data.AspNetUsers.CiliricalName, data.dateTimeCreate.ToString().Substring(0, 10),
                            data.Devision.name, userError, data.Reclamation_CountError.name, data.Reclamation_CountError1.name,
                            data.timeToSearch.ToString(), data.timeToEliminate.ToString(), data.text, data.description, data.PF.name, data.PCAM, history, data.technicalAdvice.ToString(),
                            data.gip.ToString(), trash, "", "", "", "", "", "", "", "", 17);
                    excelRows.Add(excelRow1);
                }
                Wiki.Models.ExcelColumn excelColumnIndex = new Wiki.Models.ExcelColumn();
                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                int row = 1;
                int step = 0;
                int countColumn = excelRows[0].CountData;
                foreach (var data in excelRows)
                {
                    for (int i = 0; i < countColumn; i++)
                    {
                        ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], row)].Value = data.GetData(step);
                        ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        step++;
                    }
                    row++;
                    step = 0;
                }
                for (int i = 0; i < countColumn; i++)
                {
                    ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], 1)].AutoFitColumns();
                    ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], 1)].AutoFilter = true;
                }
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-daisposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
        }

        public JsonResult GetDevisionList(string id)
        {
            if (id == "Дефектные комплектующие")
            {
                var sucursalList = db.Devision.Where(d => d.id == 7 || d.id == 9).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Документация на иностранном языке")
            {
                var sucursalList = db.Devision.Where(d => d.id == 7).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Оборудование не опробавано")
            {
                var sucursalList = db.Devision.Where(d => d.id == 6).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Отсутствие документов")
            {
                var sucursalList = db.Devision.Where(d => d.id == 7).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Отсутствие маркировки")
            {
                var sucursalList = db.Devision.Where(d => d.id == 8 || d.id == 20 || d.id == 9 || d.id == 10 || d.id == 27).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Дефицит")
            {
                var sucursalList = db.Devision.Where(d => d.id == 7 || d.id == 9).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Отсутствие/повреждение ЛКП")
            {
                var sucursalList = db.Devision.Where(d => d.id == 8 || d.id == 20 || d.id == 9 || d.id == 10 || d.id == 27).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Ошибка коммутации")
            {
                var sucursalList = db.Devision.Where(d => d.id == 10).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Ошибка маркировки")
            {
                var sucursalList = db.Devision.Where(d => d.id == 8 || d.id == 20 || d.id == 9 || d.id == 10 || d.id == 27 || d.id == 16).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Ошибка монтажа")
            {
                var sucursalList = db.Devision.Where(d => d.id == 8 || d.id == 20 || d.id == 9 || d.id == 10).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Ошибка проектирования")
            {
                var sucursalList = db.Devision.Where(d => d.id == 15 || d.id == 16 || d.id == 18 || d.id == 12).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Ошибка сборки")
            {
                var sucursalList = db.Devision.Where(d => d.id == 8 || d.id == 20 || d.id == 9 || d.id == 10).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Ошибка программирования")
            {
                var sucursalList = db.Devision.Where(d => d.id == 16).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Производственный мусор")
            {
                var sucursalList = db.Devision.Where(d => d.id == 8 || d.id == 20 || d.id == 9 || d.id == 10 || d.id == 27).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Не заложено в 1с")
            {
                var sucursalList = db.Devision.Where(d => d.id == 15 || d.id == 16 || d.id == 18 || d.id == 12).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Несоответствие 1с схемам")
            {
                var sucursalList = db.Devision.Where(d => d.id == 15 || d.id == 16 || d.id == 18 || d.id == 12).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Несоответствие КД - ТНПА")
            {
                var sucursalList = db.Devision.Where(d => d.id == 15 || d.id == 16 || d.id == 18 || d.id == 12).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (id == "Несоответствие РКД")
            {
                var sucursalList = db.Devision.Where(d => d.id == 15 || d.id == 16 || d.id == 18 || d.id == 12).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var sucursalList = db.Devision.Where(d => d.OTK == true).OrderBy(d => d.name);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.name,
                    Value = m.id.ToString(),
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdatePZList(int id)
        {
            DateTime dateDeactiveOTK = DateTime.Now.AddDays(-15);
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var sucursalList = db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > dateDeactiveOTK).ToList();
            foreach (var dataList in db.Reclamation_PZ.Include(s => s.PZ_PlanZakaz).Where(d => d.id == id && d.PZ_PlanZakaz.dataOtgruzkiBP < dateDeactiveOTK).ToList())
            {
                sucursalList.Add(dataList.PZ_PlanZakaz);
            }
            var data = sucursalList.Select(m => new SelectListItem()
            {
                Text = m.PlanZakaz.ToString(),
                Value = m.Id.ToString(),
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TableNoCloseOrders()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.Reclamation_CloseOrder.AsNoTracking()
                .Where(d => d.close == false)
                .Include(d => d.PZ_PlanZakaz)
                .OrderBy(d => d.PZ_PlanZakaz.dataOtgruzkiBP)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return reclamationsPlanZakaz('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                order = dataList.PZ_PlanZakaz.PlanZakaz,
                dateClosePlan = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, shortSetting).Replace(@"""", "")
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult CloseOrderOTK(int id)
        {
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public void ExcelOrder(int idOrderUploadExcel)
        {
            PortalKATEKEntities dbc = new PortalKATEKEntities();
            dbc.Configuration.ProxyCreationEnabled = false;
            dbc.Configuration.LazyLoadingEnabled = false;
            List<Wiki.Reclamation> list = new List<Wiki.Reclamation>();
            list = dbc.Reclamation
                .Include(d => d.Reclamation_PZ.Select(c => c.PZ_PlanZakaz))
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Devision)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .Include(d => d.PF)
                .Include(d => d.Reclamation_Answer.Select(c => c.AspNetUsers))
                .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == idOrderUploadExcel).Count() > 0 && d.close == false)
                .ToList();
            if (list.Count != 0)
            {
                List<Wiki.Models.ExcelRow> excelRows = new List<Wiki.Models.ExcelRow>();
                Wiki.Models.ExcelRow excelRow = new Wiki.Models.ExcelRow("№", "План-Заказ/ы №№:", "Автор замечания", "Создана", "Ответственное СП", "Ответственный сотрудник", "Критерий ошибки",
                    "Критерий ошибки (утв.)", "Поиск (ч.)", "Устранение (ч.)", "Текст замечания", "Прим.", "Полуфабрикат", "РСАМ", "История переписки", "На техсовет", "ГИП",
                    "Шум", "", "", "", "", "", "", "", "", 18);
                excelRows.Add(excelRow);
                foreach (var data in list)
                {
                    string ordersName = "";
                    foreach (var dataOrders in data.Reclamation_PZ)
                    {
                        ordersName += dataOrders.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
                    }
                    string history = "";
                    foreach (var dataAnswer in data.Reclamation_Answer)
                    {
                        history += dataAnswer.AspNetUsers.CiliricalName + " | " + dataAnswer.answer + "\n";
                    }
                    string trash = "Нет";
                    foreach (var dataAnswer in data.Reclamation_Answer)
                    {
                        if (dataAnswer.trash == true)
                            trash = "Да";
                    }
                    string userError = "";
                    try
                    {
                        userError = data.AspNetUsers1.CiliricalName;
                    }
                    catch
                    {
                        userError = "";
                    }
                    Wiki.Models.ExcelRow excelRow1 = new Wiki.Models.ExcelRow(data.id.ToString(), ordersName, data.AspNetUsers.CiliricalName, data.dateTimeCreate.ToString().Substring(0, 10),
                            data.Devision.name, userError, data.Reclamation_CountError.name, data.Reclamation_CountError1.name,
                            data.timeToSearch.ToString(), data.timeToEliminate.ToString(), data.text, data.description, data.PF.name, data.PCAM, history, data.technicalAdvice.ToString(),
                            data.gip.ToString(), trash, "", "", "", "", "", "", "", "", 17);
                    excelRows.Add(excelRow1);
                }
                Wiki.Models.ExcelColumn excelColumnIndex = new Wiki.Models.ExcelColumn();
                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                int row = 1;
                int step = 0;
                int countColumn = excelRows[0].CountData;
                foreach (var data in excelRows)
                {
                    for (int i = 0; i < countColumn; i++)
                    {
                        ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], row)].Value = data.GetData(step);
                        ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        step++;
                    }
                    row++;
                    step = 0;
                }
                for (int i = 0; i < countColumn; i++)
                {
                    ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], 1)].AutoFitColumns();
                    ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], 1)].AutoFilter = true;
                }
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-daisposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
        }

        public ActionResult GetShortReport()
        {
            string login = HttpContext.User.Identity.Name;
            int devision = GetDevisionId(login);
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var reclamationsList = db.Reclamation
                    .AsNoTracking()
                    .Include(a => a.Reclamation_PZ.Select(b => b.PZ_PlanZakaz))
                    .Include(a => a.PF)
                    .Include(a => a.AspNetUsers) //create
                    .Include(a => a.Reclamation_Answer.Select(b => b.AspNetUsers))
                    .Where(a => a.id_DevisionReclamation == devision)
                    .ToList();
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    worksheet["A1"].ColumnWidth = 6.0;
                    worksheet["B1"].ColumnWidth = 15.0;
                    worksheet["C1"].ColumnWidth = 100.0;
                    worksheet["D1"].ColumnWidth = 44.0;
                    worksheet["E1"].ColumnWidth = 35.0;
                    worksheet["F1"].ColumnWidth = 10.0;
                    worksheet.Range["A1:F1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["A1"].Text = "№ п/п";
                    worksheet["B1"].Text = "План-заказ/ы";
                    worksheet["C1"].Text = "Текст";
                    worksheet["D1"].Text = "Полуфабрикат";
                    worksheet["E1"].Text = "Автор";
                    worksheet["F1"].Text = "Статус";
                    int rowNum = 2;
                    foreach (var prd in reclamationsList)
                    {
                        worksheet.Range[rowNum, 1].Text = prd.id.ToString();
                        worksheet.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 1].WrapText = true;
                        worksheet.Range[rowNum, 2].Text = GetOrdersName(prd.id);
                        worksheet.Range[rowNum, 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 2].WrapText = true;
                        worksheet.Range[rowNum, 2].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 3].Text = prd.text;
                        worksheet.Range[rowNum, 3].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 3].WrapText = true;
                        worksheet.Range[rowNum, 3].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 4].Text = prd.PF.name;
                        worksheet.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 4].WrapText = true;
                        worksheet.Range[rowNum, 4].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 5].Text = prd.AspNetUsers.CiliricalName;
                        worksheet.Range[rowNum, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 5].WrapText = true;
                        worksheet.Range[rowNum, 5].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        if (prd.close == true)
                            worksheet.Range[rowNum, 6].Text = "Закрыта";
                        else
                            worksheet.Range[rowNum, 6].Text = "Активная";
                        worksheet.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        rowNum++;
                    }
                    HttpResponse response = HttpContext.ApplicationInstance.Response;
                    try
                    {
                        workbook.SaveAs("Замечания.xlsx", HttpContext.ApplicationInstance.Response, ExcelDownloadType.Open);
                    }
                    catch
                    {
                    }
                }
            }
            return View();
        }

        private string GetOrdersName(int id)
        {
            string result = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var pz = db.Reclamation_PZ
                    .AsNoTracking()
                    .Include(a => a.PZ_PlanZakaz)
                    .Where(a => a.id_Reclamation == id)
                    .ToList();
                foreach (var data in pz)
                {
                    result += data.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
                }
                return result;
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
                }
                catch
                {

                }
            }
            return devision;
        }
    }
}