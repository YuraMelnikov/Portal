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
            return View();
        }

        public ActionResult IndexOrders()
        {
            return View();
        }

        public JsonResult ActiveReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(GetIdDevision(login), false);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult PlanZakazDevisionNotSh()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login), false);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult PlanZakazDevisionSh()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login), true);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult PlanZakazDevisionAll()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login));
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult CloseReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(GetIdDevision(login), true);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
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

        public string RenderUserMenu()
        {
            string login = "Войти";
            try
            {
                if (HttpContext.User.Identity.Name != "")
                    login = HttpContext.User.Identity.Name;
            }
            catch
            {
                login = "Войти";
            }
            return login;
        }
    }
}