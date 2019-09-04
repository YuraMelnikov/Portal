using System.Web.Mvc;
using System.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Wiki.Areas.DashboardKO.Controllers
{
    public class ReportPageController : Controller
    {
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        JavaScriptSerializer js = new JavaScriptSerializer();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUsersQuaResult()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.DashboardKOQuaHumen
                    .AsNoTracking()
                    .ToList();
                int maxCounterValue = query.Count();
                Models.UserResult[] data = new Models.UserResult[maxCounterValue];
                for(int i = 0; i < maxCounterValue; i++)
                {
                    data[i].userName = query[i].user;
                    data[i].count = query[i].normHoure;
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}