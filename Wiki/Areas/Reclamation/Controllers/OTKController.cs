using System.Web.Mvc;
using Wiki.Areas.Reclamation.Models;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class OTKController : Controller
    {
        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            ViewBag.id_Devision = GetIdDevision(login);
            return View();
        }
        
        public JsonResult ActiveReclamation(int id_Devision)
        {
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetActiveReclamation(id_Devision);

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
    }
}