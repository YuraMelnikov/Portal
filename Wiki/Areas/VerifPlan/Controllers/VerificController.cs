using Newtonsoft.Json;
using NLog;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.VerifPlan.Models;
using Wiki.Models;

namespace Wiki.Areas.VerifPlan.Controllers
{
    public class VerificController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View();
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

        [HttpPost]
        public JsonResult ListActive()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PlanVerificationItems
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    id_PZ_PlanZakaz = dataList.PZ_PlanZakaz.PlanZakaz.ToString(),
                    planDate = JsonConvert.SerializeObject(dataList.planDate, settings).Replace(@"""", ""),
                    factDate = JsonConvert.SerializeObject(dataList.factDate, settings).Replace(@"""", ""),
                    appDate = JsonConvert.SerializeObject(dataList.appDate, settings).Replace(@"""", ""),
                    dataList.planDescription,
                    dataList.factDescription,
                    dataList.appDescription,
                    fixedDateForKO = JsonConvert.SerializeObject(dataList.fixedDateForKO, settings).Replace(@"""", ""),
                    verificationDateInPrj = JsonConvert.SerializeObject(dataList.verificationDateInPrj, settings).Replace(@"""", ""),
                    state = GetState(dataList)
                }); ;
                return Json(new { data });
            }
        }

        [HttpPost]
        public JsonResult ListClose()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PlanVerificationItems
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP < DateTime.Now)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    id_PZ_PlanZakaz = dataList.PZ_PlanZakaz.PlanZakaz.ToString(),
                    planDate = JsonConvert.SerializeObject(dataList.planDate, settings).Replace(@"""", ""),
                    factDate = JsonConvert.SerializeObject(dataList.factDate, settings).Replace(@"""", ""),
                    appDate = JsonConvert.SerializeObject(dataList.appDate, settings).Replace(@"""", ""),
                    dataList.planDescription,
                    dataList.factDescription,
                    dataList.appDescription,
                    fixedDateForKO = JsonConvert.SerializeObject(dataList.fixedDateForKO, settings).Replace(@"""", ""),
                    verificationDateInPrj = JsonConvert.SerializeObject(dataList.verificationDateInPrj, settings).Replace(@"""", ""),
                    state = GetState(dataList)
                }); ;
                return Json(new { data });
            }
        }

        string GetState(PlanVerificationItems planVerificationItems)
        {
            string state = "";
            if (planVerificationItems.appDate != null)
            {
                state = "Принят ОТК";
            }
            else if (planVerificationItems.factDate != null)
            {
                state = "Сдан ПО";
            }
            else if (planVerificationItems.planDate != null)
            {
                state = "Срок зафиксирован";
            }
            else if (planVerificationItems.@fixed == true)
            {
                state = "Срок не зафиксирован";
            }
            else
            {
                state = "Срок не зафиксирован";
            }
            return state;
        }

        public JsonResult Get(int id)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PlanVerificationItems
                    .AsNoTracking()
                    .Where(d => d.id == id)
                    .Include(d => d.PZ_PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.id,
                    id_PZ_PlanZakaz = dataList.PZ_PlanZakaz.PlanZakaz.ToString(),
                    dataList.@fixed,
                    state = GetState(dataList),
                    fixetFirstDate = JsonConvert.SerializeObject(dataList.fixetFirstDate, settings).Replace(@"""", ""),
                    planDate = JsonConvert.SerializeObject(dataList.planDate, settings).Replace(@"""", ""),
                    dataList.planDescription,
                    factDate = JsonConvert.SerializeObject(dataList.factDate, settings).Replace(@"""", ""),
                    dataList.factDescription,
                    appDate = JsonConvert.SerializeObject(dataList.appDate, settings).Replace(@"""", ""),
                    dataList.appDescription,
                    verificationDateInPrj = JsonConvert.SerializeObject(dataList.verificationDateInPrj, settings).Replace(@"""", ""),
                    fixedDateForKO = JsonConvert.SerializeObject(dataList.fixedDateForKO, settings).Replace(@"""", ""),
                    dateSh = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, settings).Replace(@"""", "")
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUserGroup()
        {
            int numberUserGroup = 0;
            string login = HttpContext.User.Identity.Name;
            if (login == "bav@katek.by")
                numberUserGroup = 2;
            else if (login == "Medvedev@katek.by" || login == "myi@katek.by")
                numberUserGroup = 3;
            else if (login == "pev@katek.by")
                numberUserGroup = 1;
            return Json(numberUserGroup, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTE(int id, DateTime? planDate, string planDescription)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string login = HttpContext.User.Identity.Name;
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                PlanVerificationItems planVerificationItems = db.PlanVerificationItems.Find(id);
                if (planDate != null)
                {
                    planVerificationItems.planDate = planDate.Value;
                    planVerificationItems.@fixed = true;
                }
                if (planDescription != null)
                    planVerificationItems.planDescription = planDescription;
                db.Entry(planVerificationItems).State = EntityState.Modified;
                db.SaveChanges();
                EmailVerifPlan dataMail = new EmailVerifPlan(planVerificationItems, login, 1);

                ProjectServer_UpdateMustStartOnCRUD projectServer_UpdateMustStartOnCRUD = new ProjectServer_UpdateMustStartOnCRUD(planVerificationItems.id_PZ_PlanZakaz, "ПП", planVerificationItems.planDate.Value);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateOTK(int id, DateTime? appDate, string appDescription)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string login = HttpContext.User.Identity.Name;
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                PlanVerificationItems planVerificationItems = db.PlanVerificationItems.Find(id);
                if (appDate != null)
                    planVerificationItems.appDate = appDate.Value;
                if (appDescription != null)
                    planVerificationItems.appDescription = appDescription;
                db.Entry(planVerificationItems).State = EntityState.Modified;
                db.SaveChanges();
                EmailVerifPlan dataMail = new EmailVerifPlan(planVerificationItems, login, 3);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateTM(int id, DateTime? factDate, string factDescription, DateTime? fixedDateForKO)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string login = HttpContext.User.Identity.Name;
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                PlanVerificationItems planVerificationItems = db.PlanVerificationItems.Find(id);
                if (factDate != null)
                    planVerificationItems.factDate = factDate.Value;
                if (factDescription != null)
                    planVerificationItems.factDescription = factDescription;
                if (fixedDateForKO != null)
                    planVerificationItems.fixedDateForKO = fixedDateForKO.Value;
                db.Entry(planVerificationItems).State = EntityState.Modified;
                db.SaveChanges();
                EmailVerifPlan dataMail = new EmailVerifPlan(planVerificationItems, login, 2);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }


    }
}