using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.NotesOrders.Controllers
{
    public class NoteOrderController : Controller
    {
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly JsonSerializerSettings settingsLong = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        public ActionResult Index()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                ViewBag.pz = new SelectList(db.PZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
                return View();
            }
        }

        [HttpPost]
        public JsonResult ReportTable()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_PlanZakaz
                    .Include(d => d.PZ_PZNotes)
                    .Include(d => d.AspNetUsers)
                    .Include(d => d.Client)
                    .Where(d => d.PZ_PZNotes.Count() > 0)
                    .OrderByDescending(d => d.PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    link = "",
                    order = dataList.PlanZakaz,
                    Manager = dataList.AspNetUsers.CiliricalName,
                    dataList.Name,
                    dataList.nameTU,
                    client = dataList.PZ_Client.NameSort,
                    dataList.MTR,
                    dataList.Zapros
                });
                return Json(new { data });
            }
        }

        public JsonResult Get(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_PlanZakaz
                    .Where(d => d.Id == id)
                    .Include(d => d.PZ_PZNotes)
                    .Include(d => d.AspNetUsers)
                    .Include(d => d.Client)
                    .Include(d => d.PZ_ProductType)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.PlanZakaz,
                    DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                    dataList.AspNetUsers.CiliricalName,
                    dataList.PZ_Client.NameSort,
                    dataList.Name,
                    dataList.nameTU,
                    dataList.Description,
                    dataList.MTR,
                    dataList.PZ_ProductType.ProductType,
                    dataList.OL,
                    dataList.Zapros,
                    dataList.Modul
                });

                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemOrder(int Id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_Notes
                    .AsNoTracking()
                    .Include(d => d.PZ_PZNotes.Select(c => c.PZ_PlanZakaz))
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.PZ_PZNotes.Where(c => c.id_PZ_PlanZakaz == Id).Count() > 0)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    remCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settingsLong).Replace(@"""", ""),
                    remNote = dataList.note,
                    remUser = dataList.AspNetUsers.CiliricalName
                });
                return Json(new { data });
            }
        }

        public JsonResult AddRem(int[] pz, string textRem)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                PZ_Notes pZ_Notes = new PZ_Notes
                {
                    note = textRem,
                    dateTimeCreate = DateTime.Now,
                    id_AspNetUsersCreate = db.AspNetUsers.First(d => d.Email == HttpContext.User.Identity.Name).Id
                };
                db.PZ_Notes.Add(pZ_Notes);
                db.SaveChanges();
                foreach(var data in pz)
                {
                    PZ_PZNotes pZ_PZNotes = new PZ_PZNotes();
                    pZ_PZNotes.id_PZ_Notes = pZ_Notes.id;
                    pZ_PZNotes.id_PZ_PlanZakaz = data;
                    db.PZ_PZNotes.Add(pZ_PZNotes);
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
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
    }
}