using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.Cells.Controllers
{
    public class SCellsController : Controller
    {
        public ActionResult Index()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {

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
                var query = db.SCells.AsNoTracking().ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getPoint('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    dataList.sectionIdStart,
                    dataList.name1,
                    dataList.sectionIdFinish,
                    dataList.name2,
                    dataList.distance
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
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                SCells sCells = db.SCells.Find(id);
                sCells.distance = distance;
                db.Entry(sCells).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}