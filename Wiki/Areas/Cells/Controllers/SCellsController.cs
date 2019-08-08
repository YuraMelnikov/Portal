using NLog;
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
            ViewBag.sections = new SelectList(db.Section.OrderBy(d => d.name), "idS", "name");
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

        public ActionResult Debug()
        {


            return View();
        }

        public JsonResult List(string id)
        {
            string login = HttpContext.User.Identity.Name;
            using (SCellsEntities db = new SCellsEntities())
            {
                int idS = db.Section.First(d => d.name == id).idS;
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.SectionMap
                    .AsNoTracking()
                    .Include(d => d.Section)
                    .Include(d => d.Section1)
                    .Where(d => d.sectionIdStart == idS)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getPoint('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",

                    dataList.sectionIdStart,
                    name1 = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getPoint('" + dataList.id + "')" + '\u0022' + ">" + dataList.Section.name + "</a></td>",



                    dataList.sectionIdFinish,
                    name2 = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getPoint('" + dataList.id + "')" + '\u0022' + ">" + dataList.Section1.name + "</a></td>",
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
            UpdateDistanceSelections(id, distance);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePointsCells(int[] sectionsChosen, double distanceSelections)
        {
            using (SCellsEntities db = new SCellsEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                for (int i = 0; i < sectionsChosen.Length; i++)
                {
                    double distanceFinal = 0;
                    for (int j = i + 1; j < sectionsChosen.Length; j++)
                    {
                        int predcessor = sectionsChosen[i];
                        int finalId = sectionsChosen[j];
                        distanceFinal += distanceSelections;
                        SectionMap sCells = db.SectionMap.First(d => d.sectionIdStart == predcessor && d.sectionIdFinish == finalId);
                        sCells.distance = distanceFinal;
                        db.Entry(sCells).State = EntityState.Modified;
                        db.SaveChanges();
                        SectionMap sCells1 = db.SectionMap.First(d => d.sectionIdFinish == sCells.sectionIdStart && d.sectionIdStart == sCells.sectionIdFinish);
                        sCells1.distance = distanceFinal;
                        db.Entry(sCells1).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            logger.Debug("SCells UpdatePointsCells");
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        bool UpdateDistanceSelections (int id, double distance)
        {
            using (SCellsEntities db = new SCellsEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                SectionMap sCells = db.SectionMap.Find(id);
                sCells.distance = distance;
                db.Entry(sCells).State = EntityState.Modified;
                db.SaveChanges();
                SectionMap sCells1 = db.SectionMap.First(d => d.sectionIdFinish == sCells.sectionIdStart && d.sectionIdStart == sCells.sectionIdFinish);
                sCells1.distance = distance;
                db.Entry(sCells1).State = EntityState.Modified;
                db.SaveChanges();
                logger.Debug("SCells UpdateDistance: " + id.ToString());
            }
            return true;
        }

        public JsonResult GetRemainingRows()
        {
            using (SCellsEntities db = new SCellsEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                return Json(db.SectionMap.Where(d => d.distance == 0).Count() / 2, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRow(string id)
        {
            id = id.Substring(0, 3);
            using (SCellsEntities db = new SCellsEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var sucursalList = db.Section.Where(d => d.name.Substring(0, 3) == id).OrderBy(d => d.name).ToList();
                int[] data = new int[sucursalList.Count];
                for (int i = 0; i < sucursalList.Count; i++)
                {
                    data[i] = sucursalList[i].idS;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}