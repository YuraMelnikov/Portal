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
        readonly JsonSerializerSettings settingsLong = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd HH:mm" };
        PortalKATEKEntities dbc = new PortalKATEKEntities();
        public ActionResult Index()
        {
            ViewBag.pz = new SelectList(dbc.PZ_PlanZakaz.Where(d => d.PlanZakaz < 9000).OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
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
                    .Include(d => d.PZ_Client)
                    .Where(d => d.PZ_PZNotes.Count() > 0)
                    .OrderByDescending(d => d.PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    link = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.Id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    order = dataList.PlanZakaz,
                    Manager = dataList.AspNetUsers.CiliricalName,
                    dataList.Name,
                    dataList.nameTU,
                    Client = dataList.PZ_Client.NameSort,
                    dataList.MTR,
                    dataList.Zapros,
                    dateSh = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settingsLong).Replace(@"""", ""),
                    dataList.Id
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
                    .Include(d => d.PZ_Client)
                    .Include(d => d.PZ_ProductType)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.Id,
                    dataList.PlanZakaz,
                    DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                    Manager = dataList.AspNetUsers.CiliricalName,
                    Client = dataList.PZ_Client.NameSort,
                    dataList.Name,
                    dataList.Description,
                    dataList.MTR,
                    dataList.Cost,
                    dataList.Zapros,
                    dataList.Modul,
                    dataList.PZ_ProductType.ProductType,
                    dataList.OL
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

        public JsonResult AddRem(int[] pz, string mText)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                PZ_Notes pZ_Notes = new PZ_Notes
                {
                    note = mText,
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


        public JsonResult AddRemToOrder(PZ_PlanZakaz pZ_PlanZakaz, string textRem)
        {
            using(PortalKATEKEntities db = new PortalKATEKEntities())
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
                PZ_PZNotes pZ_PZNotes = new PZ_PZNotes();
                pZ_PZNotes.id_PZ_Notes = pZ_Notes.id;
                pZ_PZNotes.id_PZ_PlanZakaz = pZ_PlanZakaz.Id;
                db.PZ_PZNotes.Add(pZ_PZNotes);
                db.SaveChanges();
                return Json(pZ_PlanZakaz.Id, JsonRequestBehavior.AllowGet);
            }
        }
    }
}