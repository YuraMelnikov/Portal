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
            SCellsEntities db = new SCellsEntities();
            string login = HttpContext.User.Identity.Name;
            ViewBag.list = new SelectList(db.Section.OrderBy(d => d.name), "idS", "name");
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

        public JsonResult List(int id)
        {
            string login = HttpContext.User.Identity.Name;
            using (SCellsEntities db = new SCellsEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.SectionMap
                    .AsNoTracking()
                    .Include(d => d.Section)
                    .Include(d => d.Section1)
                    .Where(d => d.sectionIdStart == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getPoint('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    dataList.sectionIdStart,
                    name1 = dataList.Section.name,
                    dataList.sectionIdFinish,
                    name2 = dataList.Section1.name,
                    dataList.distance
                });

                return Json(new { data, MaxJsonLength = int.MaxValue });
            }
        }

        public JsonResult GetPoint(int id)
        {
            using (SCellsEntities db = new SCellsEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.SectionMap
                    .Include(d => d.Section)
                    .Include(d => d.Section1)
                    .Where(d => d.id == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.id,
                    dataList.sectionIdStart,
                    name1 = dataList.Section.name,
                    dataList.sectionIdFinish,
                    name2 = dataList.Section1.name,
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
            using (SCellsEntities db = new SCellsEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                SectionMap sCells = db.SectionMap.Find(id);
                sCells.distance = distance;
                db.Entry(sCells).State = EntityState.Modified;
                db.SaveChanges();
                SectionMap sCells1 = db.SectionMap.First(d => d.sectionIdFinish == sCells.sectionIdStart);
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