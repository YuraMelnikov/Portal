using NLog;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.UsersKO.Controllers
{
    public class UserKOController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            try
            {
                PortalKATEKEntities db = new PortalKATEKEntities();
                string login = HttpContext.User.Identity.Name;
                ViewBag.list = new SelectList(db.ProductionCalendar.OrderBy(d => d.period), "id", "period");
                ViewBag.sections = new SelectList(db.ProductionCalendar.OrderBy(d => d.period), "id", "period");
                logger.Debug("UserKOController Index: " + login);
                return View();
            }
            catch(Exception ex)
            {
                logger.Debug("UserKOController Index: " + ex.Message);
                return View();
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

        [HttpPost]
        public JsonResult List(string id)
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int idS = db.ProductionCalendar.AsNoTracking().First(d => d.period == id).id;
                var query = db.DashboardKO_UsersMonthPlan
                    .AsNoTracking()
                    .Include(d => d.ProductionCalendar)
                    .Include(d => d.AspNetUsers.Devision1)
                    .Where(d => d.id_ProductionCalendar == idS)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getPoint('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    name = dataList.AspNetUsers.CiliricalName,
                    devision = dataList.AspNetUsers.Devision1.name,
                    dataList.ProductionCalendar.period,
                    coefficient = dataList.k
                });
                return Json(new { data, MaxJsonLength = int.MaxValue });
            }
        }

        public JsonResult GetPoint(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKO_UsersMonthPlan
                    .Include(d => d.ProductionCalendar)
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.id == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.AspNetUsers.CiliricalName,
                    dataList.ProductionCalendar.period,
                    ids = dataList.id,
                    ks = dataList.k
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdatePoint(int ids, double ks)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                DashboardKO_UsersMonthPlan dashboardKO_UsersMonthPlan = db.DashboardKO_UsersMonthPlan.Find(ids);
                dashboardKO_UsersMonthPlan.k = ks;
                db.Entry(dashboardKO_UsersMonthPlan).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}