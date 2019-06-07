﻿using System.Web.Mvc;
using Wiki.Areas.Reclamation.Models;
using System.Linq;
using Newtonsoft.Json;
using System;
using Wiki.Models;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class RemarksController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy HH:mm" };

        public ActionResult Index()
        {
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
            if (login == "fvs@katek.by")
            {
                ViewBag.ButtonAddActivation = 1;
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "nrf@katek.by")
            {
                ViewBag.ButtonAddActivation = 1;
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "Kuchynski@katek.by")
            {
                ViewBag.ButtonAddActivation = 1;
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (id_Devision == 8 || id_Devision == 9 || id_Devision == 10 || id_Devision == 20 || id_Devision == 22)
            {
                List<Devision> devisions = db.Devision.Where(d => d.id == 15 || d.id == 16).ToList();
                foreach(var data in devisions)
                {
                    if (data.id == 16)
                        data.name = "КБЭ";
                }
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
            if (id_Devision == 6)
            {
                List<Devision> devisions = db.Devision.Where(d => d.OTK == true && d.id != 6).ToList();
                foreach (var data in devisions)
                {
                    if (data.id == 16)
                        data.name = "КБЭ";
                }
                ViewBag.id_DevisionReclamation = new SelectList(devisions.OrderBy(d => d.name), "id", "name");
                ViewBag.ButtonAddActivation = 1;
                ViewBag.CRUDCounter = '1';
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
            }
            else if(id_Devision == 3 || id_Devision == 16 || id_Devision == 15)
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activePO == true).OrderBy(d => d.name), "id", "name");
            else
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
            List<Devision> devisionsReload = db.Devision.Where(d => d.OTK == true && d.id != id_Devision).ToList();
            foreach (var data in devisionsReload)
            {
                if (data.id == 16)
                    data.name = "КБЭ";
            }
            ViewBag.id_DevisionReclamationReload = new SelectList(devisionsReload.OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFirst = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFinal = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            DateTime dateTimeSh = DateTime.Now.AddDays(-30);
            ViewBag.PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > dateTimeSh).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_PF = new SelectList(db.PF.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            return View();
        }

        [HttpPost]
        public JsonResult ReclamationsPlanZakaz(int id)
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamationPlanZakaz(GetIdDevision(login), id);
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
            if (login == "nrf@katek.by" || login == "fvs@katek.by" || login == "Kuchynski@katek.by" || login == "myi@katek.by")
                reclamationListViewer.GetReclamation(GetIdDevision(login), false, login);
            else
                reclamationListViewer.GetReclamation(GetIdDevision(login), false);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult PlanZakazDevisionNotSh()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login), false);
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
            reclamationListViewer.GetReclamation(GetIdDevision(login));
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult CloseReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(GetIdDevision(login), true);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult MyReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(login);
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
                if(reclamation.editManufacturing == true)
                {
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

        //DeleteOrder
        public JsonResult DeleteOrder(Wiki.Reclamation reclamation)
        {
            string login = HttpContext.User.Identity.Name;
            Wiki.Reclamation order = db.Reclamation.Find(reclamation.id);
            _ = new EmailReclamation(order, login, 4);
            db.Reclamation.Remove(order);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Wiki.Reclamation reclamation, int[] pZ_PlanZakaz, string answerText, bool? reload, int? reloadDevision, bool? trash)
        {
            string login = HttpContext.User.Identity.Name;
            AspNetUsers aspNetUser = db.AspNetUsers.First(d => d.Email == login);
            CreateReclamation correctPlanZakaz = new CreateReclamation(reclamation, login, reload, reloadDevision);
            reclamation = correctPlanZakaz.Reclamation;
            db.Entry(reclamation).State = System.Data.Entity.EntityState.Modified;
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
                if(reclamation.close != true)
                {
                    reclamation.closeDevision = false;
                    db.Entry(reclamation).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            if (aspNetUser.Devision.Value != 6 && answerText != "" && answerText != null)
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
                if(reload != true)
                {
                    if(answerText != "-")
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
                    answer = "Рекламация перенаправлена на " + db.Devision.Find(reloadDevision).name,
                    dateTimeCreate = DateTime.Now,
                    id_AspNetUsersCreate = aspNetUser.Id,
                    id_Reclamation = reclamation.id,
                    trash = trash.Value
                };
                db.Reclamation_Answer.Add(reclamation_Answer);
                db.SaveChanges();
                EmailReclamation emailReclamation = new EmailReclamation(reclamation, login, 2);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReclamation(int id)
        {
            var query = db.Reclamation.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                dataList.fixedExpert,
                dataList.id_Reclamation_Type,
                dataList.id_DevisionReclamation,
                dataList.id_Reclamation_CountErrorFirst,
                dataList.id_Reclamation_CountErrorFinal,
                id_AspNetUsersCreate = dataList.AspNetUsers.CiliricalName,
                dataList.id_DevisionCreate,
                dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settings).Replace(@"""", ""),
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

        string GetAnswerText(List<Reclamation_Answer> reclamation_Answers)
        {
            string text = "";
            if (reclamation_Answers.Count > 0)
            {
                foreach (var data in reclamation_Answers.OrderByDescending(d => d.dateTimeCreate))
                {
                    text += data.dateTimeCreate.ToString().Substring(0, 5) + " | " + data.answer + " | " + data.AspNetUsers.CiliricalName + "\n";
                }
            }
            return text;
        }

        string[] GetPlanZakazArray(List<Reclamation_PZ> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_PZ_PlanZakaz.ToString();
            }
            return pZ_PlanZakaz;
        }

        int GetIdDevision(string loginUser)
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

        bool CreateReclamation_PZ(int[] pZ_PlanZakaz, int id_Reclamation)
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

        bool CreateTechnicalAdvice(int id_Reclamation, string id_AspNetUser)
        {
            Reclamation_TechnicalAdvice technicalAdvice = new Reclamation_TechnicalAdvice
            {
                description = "",
                id_AspNetUsersCreate = id_AspNetUser,
                id_Reclamation = id_Reclamation,
                dateTimeCreate = DateTime.Now,
                text = ""
            };
            db.Reclamation_TechnicalAdvice.Add(technicalAdvice);
            db.SaveChanges();
            return true;
        }

        bool UpdateTechnicalAdvice(int id_Reclamation, string aspNetUser)
        {
            if (db.Reclamation_TechnicalAdvice.Where(d => d.id_Reclamation == id_Reclamation).Count() == 0)
            {
                Reclamation_TechnicalAdvice technicalAdvice = new Reclamation_TechnicalAdvice
                {
                    description = "",
                    id_AspNetUsersCreate = aspNetUser,
                    id_Reclamation = id_Reclamation,
                    dateTimeCreate = DateTime.Now,
                    text = ""
                };
                db.Reclamation_TechnicalAdvice.Add(technicalAdvice);
                db.SaveChanges();
            }
            return true;
        }

        bool UpdateReclamation_PZ(int[] pZ_PlanZakaz, int id_Reclamation)
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
            if(HttpContext.User.Identity.Name == "pev@katek.by")
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
            if (HttpContext.User.Identity.Name == "antipov@katek.by")
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
            if(HttpContext.User.Identity.Name == "pev@katek.by")
            {
                foreach(Wiki.Reclamation data in db.Reclamation.Where(d => d.fixedExpert == false && d.id_DevisionCreate == 6))
                {
                    Wiki.Reclamation reclamation = data;
                    reclamation.fixedExpert = true;
                    db.Entry(reclamation).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return GetRemarksOTK();
            }
            else if(HttpContext.User.Identity.Name == "antipov@katek.by")
            {
                foreach (Wiki.Reclamation data in db.Reclamation.Where(d => d.fixedExpert == false && d.id_DevisionCreate != 6))
                {
                    Wiki.Reclamation reclamation = data;
                    reclamation.fixedExpert = true;
                    db.Entry(reclamation).State = System.Data.Entity.EntityState.Modified;
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
            string login = HttpContext.User.Identity.Name;
            AspNetUsers user = db.AspNetUsers.First(d => d.Email == login);
            List<Wiki.Reclamation> list = new List<Wiki.Reclamation>();
            if (reclamation.close == true)
            {
                list = db.Reclamation.ToList();
            }
            else if (reclamation.closeDevision == true)
            {
                list = db.Reclamation.Where(d => d.id_DevisionReclamation == user.Devision).ToList();
            }
            else if (reclamation.closeMKO == true)
            {
                list = db.Reclamation.Where(d => d.id_AspNetUsersCreate == user.Id).ToList();
            }
            else if (reclamation.gip == true)
            {
                list = db.Reclamation.Where(d => d.id_AspNetUsersError == user.Id).ToList();
            }
            else
            {

            }
            if (list.Count != 0)
            {
                List<Wiki.Models.ExcelRow> excelRows = new List<Wiki.Models.ExcelRow>();
                Wiki.Models.ExcelRow excelRow = new Wiki.Models.ExcelRow("№", "План-Заказ/ы №№:", "Автор рекламации", "Создана", "Ответственное СП", "Ответственный сотрудник", "Критерий ошибки",
                    "Критерий ошибки (утв.)", "Поиск (ч.)", "Устранение (ч.)", "Текст рекламации", "Прим.", "Полуфабрикат", "РСАМ", "История переписки", "На техсовет", "ГИП",
                    "", "", "", "", "", "", "", "", "", 17);
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
                    string userError = "";
                    try
                    {
                        userError = data.AspNetUsers1.CiliricalName;
                    }
                    catch
                    {

                    }
                    Wiki.Models.ExcelRow excelRow1 = new Wiki.Models.ExcelRow(data.id.ToString(), ordersName, data.AspNetUsers.CiliricalName, data.dateTimeCreate.ToString().Substring(0, 10),
                            data.Devision1.name, userError, data.Reclamation_CountError.name, data.Reclamation_CountError1.name,
                            data.timeToSearch.ToString(), data.timeToEliminate.ToString(), data.text, data.description, data.PF.name, data.PCAM, history, data.technicalAdvice.ToString(),
                            data.gip.ToString(), "", "", "", "", "", "", "", "", "", 17);
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
    }
    
}