using System.Web.Mvc;
using Wiki.Areas.Reclamation.Models;
using System.Linq;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class RemarksController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            ViewBag.id_Devision = GetIdDevision(login);
            return View();
        }
        
        public JsonResult ActiveReclamation(int id_Devision)
        {
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(id_Devision, false);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult CloseReclamation(int id_Devision)
        {
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(id_Devision, true);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        //public JsonResult NotCountReclamation(int id_Devision)
        //{
        //    ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
        //    reclamationListViewer.GetReclamation(id_Devision, false);
        //    return Json(new { data = reclamationListViewer.ReclamationsListView });
        //}

        public JsonResult PlanZakazDevisionNotSh(int id_Devision)
        {
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(id_Devision, false);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult PlanZakazDevisionSh(int id_Devision)
        {
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(id_Devision, true);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult PlanZakazDevisionAll(int id_Devision)
        {
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(id_Devision);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult PlanZakaz()
        {
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs();
            return Json(new { data = planZakazListViewers.PlanZakazViwers});
        }

        int GetIdDevision(string loginUser)
        {
            int id_Devision = 0;
            try
            {
                id_Devision = db.AspNetUsers.First(d => d.Email == loginUser).Devision.Value;
            }
            catch
            {

            }
            return id_Devision;
        }
    }
}