using NLog;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.Cells.Controllers
{
    public class SCellsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                logger.Debug("SCells ActionResult: " + login);
            }
            catch
            {
                logger.Debug("SCells ActionResult: ");
            }
            return View();
        }

        public JsonResult List()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.SCells.AsNoTracking().Take(10).ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getPoint('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    dataList.sectionIdStart,
                    dataList.name1,
                    dataList.sectionIdFinish,
                    dataList.name2,
                    distance = dataList.distance
                });
                return Json(new { data });
            }
        }

        public JsonResult GetPoint(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.SCells.Where(d => d.id == id).ToList();
                var data = query.Select(dataList => new
                {
                    dataList.id,
                    dataList.sectionIdStart,
                    dataList.name1,
                    dataList.sectionIdFinish,
                    dataList.name2,
                    dataList.distance
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdatePoint(int id, double distance)
        {
            string login = HttpContext.User.Identity.Name;
            if(login != "myi@katek.by")
                return Json(0, JsonRequestBehavior.AllowGet);
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                SCells sCells = db.SCells.First(d => d.id == id);
                sCells.distance = distance;
                db.Entry(sCells).State = EntityState.Modified;
                db.SaveChanges();
                SCells sCells1 = db.SCells.First(d => d.sectionIdFinish == sCells.sectionIdStart);
                sCells1.distance = distance;
                db.Entry(sCells1).State = EntityState.Modified;
                db.SaveChanges();
                logger.Debug("SCells UpdatePoint: " + login + " | " + id.ToString());
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