using System.Web.Mvc;
using Wiki.Areas.Reclamation.Models;
using System.Linq;
using Newtonsoft.Json;
using System;
using Wiki.Models;
using System.Collections.Generic;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class RemarksController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            int id_Devision = 0;
            try
            {
                id_Devision = db.AspNetUsers.FirstOrDefault(d => d.Email == login).Devision.Value;
            }
            catch
            {

            }
            ViewBag.DevisionsManufacturing = new SelectList(new DevisionsManufacturing().Devisions.OrderBy(d => d.name), "id", "name");
            if (id_Devision == 16 || id_Devision == 3)
                ViewBag.id_DevisionReclamation = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 3 && d.id != 16).OrderBy(d => d.name), "id", "name");
            else
                ViewBag.id_DevisionReclamation = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != id_Devision).OrderBy(d => d.name), "id", "name");
            if (login == "fvs@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "nrf@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "myi@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "Kuchynski@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (id_Devision == 8 || id_Devision == 9 || id_Devision == 10 || id_Devision == 20 || id_Devision == 22)
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == id_Devision).Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '3';
            }
            else
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers.Where(d => d.Email == login), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '4';
            }
            if (id_Devision == 6)
            {
                ViewBag.CRUDCounter = '1';
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
            }
            else
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activePO == true).OrderBy(d => d.name), "id", "name");

            ViewBag.id_DevisionReclamationReload = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != id_Devision).OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFirst = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFinal = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            DateTime dateTimeSh = DateTime.Now.AddDays(-14);
            ViewBag.PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > dateTimeSh).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_PF = new SelectList(db.PF.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            return View();
        }

        public ActionResult IndexOrders()
        {
            string login = HttpContext.User.Identity.Name;
            int id_Devision = db.AspNetUsers.FirstOrDefault(d => d.Email == login).Devision.Value;
            ViewBag.DevisionsManufacturing = new SelectList(new DevisionsManufacturing().Devisions.OrderBy(d => d.name), "id", "name");
            if (id_Devision == 16 || id_Devision == 3)
                ViewBag.id_DevisionReclamation = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 3 && d.id != 16).OrderBy(d => d.name), "id", "name");
            else
                ViewBag.id_DevisionReclamation = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != id_Devision).OrderBy(d => d.name), "id", "name");
            if (login == "fvs@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "nrf@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "myi@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "Kuchynski@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16)
                    .Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (id_Devision == 8 || id_Devision == 9 || id_Devision == 10 || id_Devision == 20 || id_Devision == 22)
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == id_Devision).Where(d => d.LockoutEnabled == true)
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '3';
            }
            else
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers.Where(d => d.Email == login), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '4';
            }
            if (id_Devision == 6)
            {
                ViewBag.CRUDCounter = '1';
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
            }
            else
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activePO == true).OrderBy(d => d.name), "id", "name");

            ViewBag.id_DevisionReclamationReload = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != id_Devision).OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFirst = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFinal = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            DateTime dateTimeSh = DateTime.Now.AddDays(-14);
            ViewBag.PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > dateTimeSh).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_PF = new SelectList(db.PF.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            return View();
        }

        public JsonResult ActiveReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
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

        public JsonResult PlanZakazDevisionAll()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login));
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult CloseReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(GetIdDevision(login), true);
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
            }
            if(reclamation.technicalAdvice == true)
                UpdateTechnicalAdvice(reclamation.id, aspNetUser.Id);
            UpdateReclamation_PZ(pZ_PlanZakaz, reclamation.id);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReclamation(int id)
        {
            var query = db.Reclamation.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
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
            if(reclamation_Answers.Count > 0)
            {
                foreach(var data in reclamation_Answers.OrderByDescending(d => d.dateTimeCreate))
                {
                    text += data.dateTimeCreate.ToString().Substring(0, 10) + " | " + data.answer + " | " + data.AspNetUsers.CiliricalName + "\n";
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
            if(db.Reclamation_TechnicalAdvice.Where(d => d.id_Reclamation == id_Reclamation).Count() == 0 )
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
    }
}