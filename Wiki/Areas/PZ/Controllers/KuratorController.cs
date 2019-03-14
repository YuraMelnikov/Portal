using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.PZ.Models;

namespace Wiki.Areas.PZ.Controllers
{
    public class KuratorController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        public ActionResult Index()
        {
            ViewBag.id_PZ_Client = new SelectList(db.PZ_Client.OrderBy(x => x.NameSort), "id", "NameSort");
            return View();
        }

        [HttpPost]
        public JsonResult List()
        {
            var query = db.PZ_FIO.OrderBy(d => d.fio).ToList();
            var data = query.Select(dataList => new
            {
                Id = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                dataList.id,
                dataList.fio,
                dataList.i,
                dataList.o,
                id_PZ_Client = dataList.PZ_Client.NameSort,
                dataList.email,
                dataList.phone,
                dataList.position
            });

            return Json(new { data });
        }

        public JsonResult Add(PZ_FIO kurator)
        {
            CorrectKurator correctKurator = new CorrectKurator(kurator);
            kurator = correctKurator.Kurator;
            db.PZ_FIO.Add(kurator);
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetId(int Id)
        {
            var query = db.PZ_FIO.Where(d => d.id == Id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                dataList.fio,
                dataList.i,
                dataList.o,
                dataList.id_PZ_Client,
                dataList.email,
                dataList.phone,
                dataList.position
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(PZ_FIO kurator)
        {
            PZ_FIO editClient = db.PZ_FIO.First(d => d.id == kurator.id);
            editClient.fio = kurator.fio;
            editClient.i = kurator.i;
            editClient.o = kurator.o;
            editClient.id_PZ_Client = kurator.id_PZ_Client;
            editClient.email = kurator.email;
            editClient.phone = kurator.phone;
            editClient.position = kurator.position;
            CorrectKurator correctKurator = new CorrectKurator(editClient);
            editClient = correctKurator.Kurator;
            db.Entry(editClient).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}