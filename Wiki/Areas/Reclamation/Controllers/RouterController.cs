using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class RouterController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            int id_Devision = GetIdDevision(login);
            if(id_Devision == 18 || id_Devision == 12 || id_Devision == 15 || id_Devision == 16 || id_Devision == 3)
                return RedirectToAction("Index", "KO");
            else if (id_Devision == 7 || id_Devision == 25 || id_Devision == 22 || id_Devision == 27 
                || id_Devision == 8 || id_Devision == 20 || id_Devision == 9 || id_Devision == 10)
                return RedirectToAction("Index", "PO");
            else if (id_Devision == 6)
                return RedirectToAction("Index", "OTK");
            else if (id_Devision == 26)
                return RedirectToAction("Index", "Admin");
            else if (id_Devision == 25)
                return RedirectToAction("Index", "PO");
            else 
                return RedirectToAction("Index", "All");
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