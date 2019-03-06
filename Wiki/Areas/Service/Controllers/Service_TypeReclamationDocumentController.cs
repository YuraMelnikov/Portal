using System.Web.Mvc;
using Wiki.Areas.Service.Models;

namespace Wiki.Areas.Service.Controllers
{
    public class Service_TypeReclamationDocumentController : Controller
    {
        private Service_TypeReclamationDocumentCRUD type = new Service_TypeReclamationDocumentCRUD();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            return Json(type.ListAll(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Service_TypeReclamationDocument newType)
        {
            return Json(type.Add(newType), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int id)
        {
            Service_TypeReclamationDocument typeReclamation = type.ListAll().Find(x => x.id.Equals(id));
            return Json(typeReclamation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Service_TypeReclamationDocument updateType)
        {
            return Json(type.Update(updateType), JsonRequestBehavior.AllowGet);
        }
    }
}