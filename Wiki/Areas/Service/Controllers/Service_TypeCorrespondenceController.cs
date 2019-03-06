using System.Web.Mvc;
using Wiki.Areas.Service.Models;

namespace Wiki.Areas.Service.Controllers
{
    public class Service_TypeCorrespondenceController : Controller
    {
        private Service_TypeCorrespondenceCRUD type = new Service_TypeCorrespondenceCRUD();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            return Json(type.ListAll(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Service_TypeCorrespondence newType)
        {
            return Json(type.Add(newType), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int id)
        {
            Service_TypeCorrespondence typeReclamation = type.ListAll().Find(x => x.id.Equals(id));
            return Json(typeReclamation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Service_TypeCorrespondence updateType)
        {
            return Json(type.Update(updateType), JsonRequestBehavior.AllowGet);
        }
    }
}