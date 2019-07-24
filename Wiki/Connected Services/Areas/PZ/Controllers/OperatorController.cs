using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.PZ.Controllers
{
    public class OperatorController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        [Authorize(Roles = "Admin, OPTP, OP")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult OperatorList()
        {
            var query = db.PZ_OperatorDogovora.OrderBy(d => d.name).ToList();
            var data = query.Select(dataList => new
            {
                Id = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                dataList.name
            });

            return Json(new { data });
        }

        public JsonResult Add(PZ_OperatorDogovora operatorDogovora)
        {
            db.PZ_OperatorDogovora.Add(operatorDogovora);
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOperator(int Id)
        {
            var query = db.PZ_OperatorDogovora.Where(d => d.id == Id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                dataList.name
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(PZ_OperatorDogovora operatorDogovora)
        {
            db.Entry(operatorDogovora).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}