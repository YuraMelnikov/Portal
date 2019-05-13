using System.Web.Mvc;
using Wiki.Areas.Reclamation.Models;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class RemarksController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            int id_Devision = db.AspNetUsers.FirstOrDefault(d => d.Email == login).Devision.Value;
            if (login == "fvs@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com")
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "nrf@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com")
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "myi@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com")
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (login == "Kuchynski@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com")
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.CRUDCounter = '2';
            }
            else if (id_Devision == 8 || id_Devision == 9 || id_Devision == 10 || id_Devision == 20 || id_Devision == 22)
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.UserPO
                    .Where(d => d.id_Devision == id_Devision)
                    .OrderBy(d => d.name), "id", "name");
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
            ViewBag.id_DevisionReclamation = new SelectList(db.Devision.Where(d => d.OTK == true)
                .OrderBy(d => d.name), "id", "name");
            ViewBag.id_DevisionReclamationReload = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != id_Devision)
                .OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFirst = new SelectList(db.Reclamation_CountError
                .Where(d => d.active == true)
                .OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFinal = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            DateTime dateTimeSh = DateTime.Now.AddDays(-14);
            ViewBag.PZ = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > dateTimeSh).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_PF = new SelectList(db.PF.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            return View();
        }

        public ActionResult IndexOrders()
        {
            string login = HttpContext.User.Identity.Name;
            int id_Devision = db.AspNetUsers.FirstOrDefault(d => d.Email == login).Devision.Value;

            if (login == "fvs@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com")
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            }
            else if (login == "nrf@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com")
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            }
            else if (login == "myi@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com")
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            }
            else if (login == "Kuchynski@katek.by")
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers
                    .Where(d => d.Devision == 3 || d.Devision == 16 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com")
                    .OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            }
            else if (id_Devision == 8 || id_Devision == 9 || id_Devision == 10 || id_Devision == 20 || id_Devision == 22)
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.UserPO
                    .Where(d => d.id_Devision == id_Devision)
                    .OrderBy(d => d.name), "id", "name");
            }
            else
            {
                ViewBag.id_AspNetUsersError = new SelectList(db.AspNetUsers.Where(d => d.Email == login), "Id", "CiliricalName");
            }
            if (id_Devision == 6)
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
            else
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activePO == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_DevisionReclamation = new SelectList(db.Devision.Where(d => d.OTK == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFirst = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_Reclamation_CountErrorFinal = new SelectList(db.Reclamation_CountError.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.PZ = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now.AddDays(-14)), "Id", "PlanZakaz");
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
            CorrectReclamation correctReclamation = new CorrectReclamation(reclamation, login);
            reclamation = correctReclamation.Reclamation;
            db.Reclamation.Add(reclamation);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Wiki.Reclamation reclamation)
        {
            string login = HttpContext.User.Identity.Name;
            CorrectReclamation correctPlanZakaz = new CorrectReclamation(reclamation);
            reclamation = correctPlanZakaz.Reclamation;
            db.Entry(reclamation).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
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
                dataList.id_AspNetUsersCreate,
                dataList.id_DevisionCreate,
                dataList.text,
                dataList.description,
                dataList.timeToSearch,
                dataList.timeToEliminate,
                dataList.close,
                dataList.gip,
                dataList.closeDevision,
                dataList.PCAM,
                dataList.editManufacturing,
                dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settings).Replace(@"""", "")
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
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
    }
}