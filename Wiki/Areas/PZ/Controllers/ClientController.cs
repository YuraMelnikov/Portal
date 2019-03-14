using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.PZ.Models;

namespace Wiki.Areas.PZ.Controllers
{
    public class ClientController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        public ActionResult Index()
        {
            ViewBag.Client1 = new SelectList(db.PZ_Client.OrderBy(x => x.NameSort), "id", "NameSort");
            ViewBag.Client11 = new SelectList(db.PZ_Client.OrderBy(x => x.NameSort), "id", "NameSort");
            return View();
        }

        [HttpPost]
        public JsonResult ClientsList()
        {
            var query = db.PZ_Client.OrderBy(d => d.NameSort).ToList();
            var data = query.Select(dataList => new
            {
                Id = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                dataList.id,
                dataList.NameSort,
                dataList.INN_UNN,
                dataList.Name
            });

            return Json(new { data });
        }

        public JsonResult Add(PZ_Client client)
        {
            CorrectClient correctClient = new CorrectClient(client);
            client = correctClient.PZ_Client;
            db.PZ_Client.Add(client);
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClient(int Id)
        {
            var query = db.PZ_Client.Where(d => d.id == Id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                dataList.NameSort,
                dataList.INN_UNN,
                dataList.Name,
                dataList.GCompany,
                dataList.DCCompany
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(PZ_Client client)
        {
            PZ_Client editClient = db.PZ_Client.First(d => d.id == client.id);
            if (editClient.NameSort != client.NameSort)
                editClient.NameSort = client.NameSort;
            if (editClient.INN_UNN != client.INN_UNN)
                editClient.INN_UNN = client.INN_UNN;
            if (editClient.Name != client.Name)
                editClient.Name = client.Name;
            if (editClient.GCompany != client.GCompany)
                editClient.GCompany = client.GCompany;
            if (editClient.DCCompany != client.DCCompany)
                editClient.DCCompany = client.DCCompany;
            CorrectClient correctClient = new CorrectClient(editClient);
            editClient = correctClient.PZ_Client;
            db.Entry(editClient).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}