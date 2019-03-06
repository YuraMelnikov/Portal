using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class ReclamationPOController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            var oTK_ReclamationKO = db.OTK_ReclamationKO
                .Include(o => o.OTK_CounterErrorKO)
                .Include(o => o.OTK_TypeReclamation)
                .Include(o => o.PZ_PlanZakaz)
                .Where(o => o.descriptionManagerKO == "" || o.descriptionManagerKO == null)
                .OrderByDescending(o => o.dateCreate)
                .ToList();
            var oTK_ReclamationKOClose = db.OTK_ReclamationKO
                .Include(o => o.OTK_CounterErrorKO)
                .Include(o => o.OTK_TypeReclamation)
                .Include(o => o.PZ_PlanZakaz)
                .Where(o => o.descriptionManagerKO != "")
                .Where(o => o.descriptionManagerKO != null)
                .OrderByDescending(o => o.dateCreate)
                .ToList();
            ReclamationPO reclamationPO = new ReclamationPO();
            reclamationPO.ActiveList = oTK_ReclamationKO;
            reclamationPO.DeActiveList = oTK_ReclamationKOClose;

            return View(reclamationPO);
        }
    }
}