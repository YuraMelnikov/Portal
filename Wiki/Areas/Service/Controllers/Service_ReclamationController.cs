using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.Service.Models;

namespace Wiki.Areas.Service.Controllers
{
    public class Service_ReclamationController : Controller
    {
        private Service_ReclamationCRUD type = new Service_ReclamationCRUD();
        JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        public ActionResult Index()
        {
            ViewBag.PZ_PlanZakaz = new SelectList(type.Db.PZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.Service_TypeReclamation = new SelectList(type.Db.Service_TypeReclamation.OrderBy(d => d.name), "id", "name");
            ViewBag.id_Service_ReclamationWhoAdd = new SelectList(type.Db.Service_ReclamationWhoAdd.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_Service_TypeDocument = new SelectList(type.Db.Service_TypeReclamationDocument.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            return View();
        }

        public JsonResult List()
        {
            IList<Service_Reclamation> list = type.ListAll();
            var collection = list.Select(reclamation => new
            {
                id = reclamation.id,
                description = reclamation.description,
                dateAdd = JsonConvert.SerializeObject(reclamation.dateAdd, settings).Replace(@"""", ""),
                pz_Names = from tag in reclamation.Service_ReclamationPZ group tag by tag.PZ_PlanZakaz.PlanZakaz into words select new { name = words.Key, weight = words.Count() }
            });

            return Json(collection, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Service_Reclamation reclamation, int[] service_TypeReclamation, int[] pZ_PlanZakaz)
        {
            return Json(type.Add(reclamation, service_TypeReclamation, pZ_PlanZakaz), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int id)
        {
            Service_Reclamation reclamation = type.GetListReclamationBeforeSerilise().Find(x => x.id.Equals(id));
            Service_ReclamationU json = new Service_ReclamationU(reclamation);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Service_Reclamation reclamation, int[] service_TypeReclamation, int[] pZ_PlanZakaz)
        {
            return Json(type.Update(reclamation, service_TypeReclamation, pZ_PlanZakaz), JsonRequestBehavior.AllowGet);
        }
    }
}