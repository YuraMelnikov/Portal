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
                ViewBag.list = new SelectList(db.ProductionCalendar.OrderBy(d => d.period), "idS", "name");
                ViewBag.sections = new SelectList(db.ProductionCalendar.OrderBy(d => d.period), "idS", "name");
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

        public JsonResult Get(int id)
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

        //public JsonResult UpdatePoint(int id, double distance)
        //{
        //    UpdateDistanceSelections(id, distance);
        //    return Json(1, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult UpdatePointsCells(int[] sectionsChosen, double distanceSelections)
        //{
        //    using (SCellsEntities db = new SCellsEntities())
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;
        //        db.Configuration.LazyLoadingEnabled = false;
        //        for (int i = 0; i < sectionsChosen.Length; i++)
        //        {
        //            double distanceFinal = 0;
        //            for (int j = i + 1; j < sectionsChosen.Length; j++)
        //            {
        //                int predcessor = sectionsChosen[i];
        //                int finalId = sectionsChosen[j];
        //                distanceFinal += distanceSelections;
        //                SectionMap sCells = db.SectionMap.First(d => d.sectionIdStart == predcessor && d.sectionIdFinish == finalId);
        //                sCells.distance = distanceFinal;
        //                db.Entry(sCells).State = EntityState.Modified;
        //                db.SaveChanges();
        //                SectionMap sCells1 = db.SectionMap.First(d => d.sectionIdFinish == sCells.sectionIdStart && d.sectionIdStart == sCells.sectionIdFinish);
        //                sCells1.distance = distanceFinal;
        //                db.Entry(sCells1).State = EntityState.Modified;
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    logger.Debug("SCells UpdatePointsCells");
        //    return Json(1, JsonRequestBehavior.AllowGet);
        //}

        //bool UpdateDistanceSelections(int id, double distance)
        //{
        //    using (SCellsEntities db = new SCellsEntities())
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;
        //        db.Configuration.LazyLoadingEnabled = false;
        //        SectionMap sCells = db.SectionMap.Find(id);
        //        sCells.distance = distance;
        //        db.Entry(sCells).State = EntityState.Modified;
        //        db.SaveChanges();
        //        SectionMap sCells1 = db.SectionMap.First(d => d.sectionIdFinish == sCells.sectionIdStart && d.sectionIdStart == sCells.sectionIdFinish);
        //        sCells1.distance = distance;
        //        db.Entry(sCells1).State = EntityState.Modified;
        //        db.SaveChanges();
        //        logger.Debug("SCells UpdateDistance: " + id.ToString());
        //    }
        //    return true;
        //}

        //public JsonResult GetRemainingRows()
        //{
        //    using (SCellsEntities db = new SCellsEntities())
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;
        //        db.Configuration.LazyLoadingEnabled = false;
        //        return Json(db.SectionMap.Count(d => d.distance == 0) / 2, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult GetRow(string id)
        //{
        //    id = id.Substring(0, 3);
        //    using (SCellsEntities db = new SCellsEntities())
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;
        //        db.Configuration.LazyLoadingEnabled = false;
        //        var sucursalList = db.Section.Where(d => d.name.Substring(0, 3) == id).OrderBy(d => d.name).ToList();
        //        int[] data = new int[sucursalList.Count];
        //        for (int i = 0; i < sucursalList.Count; i++)
        //        {
        //            data[i] = sucursalList[i].idS;
        //        }
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}