using Newtonsoft.Json;
using System.Web.Mvc;
using Wiki.Areas.Service.Models;

namespace Wiki.Areas.Service.Controllers
{
    public class Service_ReclamationCounterErrorController : Controller
    {
        private Service_ReclamationCounterErrorCRUD type = new Service_ReclamationCounterErrorCRUD();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var settings = new JsonSerializerSettings { DateFormatString = "dd-MM-yyyy" };
            var list = type.ListAll();
            //list.


            var json = JsonConvert.SerializeObject(type.ListAll(), settings);


            return Json(type.ListAll(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Service_ReclamationCounterError newType)
        {
            return Json(type.Add(newType), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int id)
        {
            Service_ReclamationCounterError typeReclamation = type.ListAll().Find(x => x.id.Equals(id));
            return Json(typeReclamation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Service_ReclamationCounterError updateType)
        {
            return Json(type.Update(updateType), JsonRequestBehavior.AllowGet);
        }
    }
}