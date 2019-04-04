using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class ReclamationOTKController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad, KBMUser, KBEUser, Service, OTK")]
        public ActionResult Index()
        {
            ReclamationOTK reclamationOTK = new ReclamationOTK();
            string login = HttpContext.User.Identity.Name;
            int devisionId = (int)db.AspNetUsers.Where(d => d.Email == login).First().Devision;
            if(devisionId == 3 || devisionId == 15 || devisionId == 16)
            {
                var oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                    .Include(o => o.OTK_Reclamation)
                    .Include(o => o.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz)
                    .Where(o => o.Devision == 3 || o.Devision == 15 || o.Devision == 16)
                    .Where(o => o.descriptionManagerKO == "" || o.descriptionManagerKO == null)
                    .OrderByDescending(o => o.DateCreate)
                    .ToList();

                var oTK_ReclamationAnswerClose = db.OTK_ReclamationAnswer
                    .Include(o => o.OTK_Reclamation)
                    .Where(o => o.Devision == 3 || o.Devision == 15 || o.Devision == 16)
                    .Where(o => o.descriptionManagerKO != "")
                    .Where(o => o.descriptionManagerKO != null)
                    .OrderByDescending(o => o.DateCreate)
                    .ToList();
                reclamationOTK.ActiveList = oTK_ReclamationAnswer;
                reclamationOTK.DeActiveList = oTK_ReclamationAnswerClose;
            }
            else if (devisionId == 8)
            {
                var oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                    .Include(o => o.OTK_Reclamation)
                    .Include(o => o.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz)
                    .Where(o => o.Devision == devisionId || o.Devision == 20)
                    .Where(o => o.Text == "")
                    .OrderByDescending(o => o.DateCreate)
                    .ToList();

                var oTK_ReclamationAnswerClose = db.OTK_ReclamationAnswer
                    .Include(o => o.OTK_Reclamation)
                    .Where(o => o.Devision == devisionId)
                    .Where(o => o.Text != "")
                    .OrderByDescending(o => o.DateCreate)
                    .ToList();
                reclamationOTK.ActiveList = oTK_ReclamationAnswer;
                reclamationOTK.DeActiveList = oTK_ReclamationAnswerClose;
            }
            else if (devisionId == 10)
            {
                var oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                    .Include(o => o.OTK_Reclamation)
                    .Include(o => o.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz)
                    .Where(o => o.Devision == devisionId || o.Devision == 7)
                    .Where(o => o.Text == "")
                    .OrderByDescending(o => o.DateCreate)
                    .ToList();

                var oTK_ReclamationAnswerClose = db.OTK_ReclamationAnswer
                    .Include(o => o.OTK_Reclamation)
                    .Where(o => o.Devision == devisionId)
                    .Where(o => o.Text != "")
                    .OrderByDescending(o => o.DateCreate)
                    .ToList();
                reclamationOTK.ActiveList = oTK_ReclamationAnswer;
                reclamationOTK.DeActiveList = oTK_ReclamationAnswerClose;
            }
            else 
            {
                var oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                    .Include(o => o.OTK_Reclamation)
                    .Include(o => o.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz)
                    .Where(o => o.Devision == devisionId)
                    .Where(o => o.Text == "")
                    .OrderByDescending(o => o.DateCreate)
                    .ToList();

                var oTK_ReclamationAnswerClose = db.OTK_ReclamationAnswer
                    .Include(o => o.OTK_Reclamation)
                    .Where(o => o.Devision == devisionId)
                    .Where(o => o.Text != "")
                    .OrderByDescending(o => o.DateCreate)
                    .ToList();
                reclamationOTK.ActiveList = oTK_ReclamationAnswer;
                reclamationOTK.DeActiveList = oTK_ReclamationAnswerClose;
            }
            return View(reclamationOTK);
        }
        
        public ActionResult ChackLists()
        {
            var oTK_ChaeckList = db.OTK_ChaeckList.OrderByDescending(t => t.PZ_PlanZakaz.PlanZakaz);
            return View(oTK_ChaeckList.ToList());
        }

        public ActionResult IndexCheckList(int? id)
        {
            var oTK_Reclamation = db.OTK_Reclamation
                .Where(o => o.CheckList == id)
                .OrderBy(o => o.Complited)
                .ToList();
            OTK_ChaeckList oTK_ChaeckList = db.OTK_ChaeckList.Find(Convert.ToInt32(id));
            ViewBag.CheckList = oTK_ChaeckList;
            return View(oTK_Reclamation);
        }
        
        public ActionResult ActiveReclamation()
        {
            var oTK_ReclamationAnswer = new List<OTK_ReclamationAnswer>();
            oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                    .Where(c => c.OTK_Reclamation.Complited == false)
                    .OrderByDescending(z => z.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz).ToList();
            return View(oTK_ReclamationAnswer.ToList());
        }
    }
}