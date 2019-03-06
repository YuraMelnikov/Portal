using System.Web.Mvc;
using Wiki.Areas.Service.Models;

namespace Wiki.Areas.Service.Controllers
{
    public class Service_TypeReclamationController : Controller
    {
        private Service_TypeReclamationCRUD type = new Service_TypeReclamationCRUD();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            return Json(type.ListAll(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Service_TypeReclamation newType)
        {
            return Json(type.Add(newType), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int id)
        {
            Service_TypeReclamation typeReclamation = type.ListAll().Find(x => x.id.Equals(id));
            return Json(typeReclamation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Service_TypeReclamation updateType)
        {
            return Json(type.Update(updateType), JsonRequestBehavior.AllowGet);
        }
    }
}