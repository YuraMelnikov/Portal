using System.Web.Mvc;
using Wiki.Areas.Service.Models;

namespace Wiki.Areas.Service.Controllers
{
    public class Service_ReclamationWhoAddController : Controller
    {
        private Service_ReclamationWhoAddCRUD type = new Service_ReclamationWhoAddCRUD();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            return Json(type.ListAll(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Service_ReclamationWhoAdd newType)
        {
            return Json(type.Add(newType), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int id)
        {
            Service_ReclamationWhoAdd typeReclamation = type.ListAll().Find(x => x.id.Equals(id));
            return Json(typeReclamation, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Service_ReclamationWhoAdd updateType)
        {
            return Json(type.Update(updateType), JsonRequestBehavior.AllowGet);
        }
    }
}