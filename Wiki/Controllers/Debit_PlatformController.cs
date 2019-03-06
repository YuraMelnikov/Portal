using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Wiki.Controllers
{
    public class Debit_PlatformController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();
        
        public ActionResult Index()
        {
            return View(db.Debit_Platform.ToList().OrderBy(d => d.PZ_PlanZakaz.PlanZakaz));
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_Platform debit_Platform = db.Debit_Platform.Find(id);
            if (debit_Platform == null)
            {
                return HttpNotFound();
            }
            return View(debit_Platform);
        }
        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_PlanZakaz,massa,countPlatform,numPlatform,numPlomb,gabar")] Debit_Platform debit_Platform)
        {
            if (ModelState.IsValid)
            {
                db.Debit_Platform.Add(debit_Platform);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(debit_Platform);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_Platform debit_Platform = db.Debit_Platform.Find(id);
            if (debit_Platform == null)
            {
                return HttpNotFound();
            }
            return View(debit_Platform);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Debit_Platform debit_Platform)
        {
            if (ModelState.IsValid)
            {
                if (debit_Platform.massa == null)
                    debit_Platform.massa = 0;
                if (debit_Platform.numPlatform == null)
                    debit_Platform.numPlatform = "";
                if (debit_Platform.numPlomb == null)
                    debit_Platform.numPlomb = "";
                if (debit_Platform.gabar == null)
                    debit_Platform.gabar = "";
                if (debit_Platform.countPlatform == null)
                    debit_Platform.countPlatform = 0;
                db.Entry(debit_Platform).State = EntityState.Modified;
                db.SaveChanges();

                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_TaskForPZ == 29 & d.id_PlanZakaz == debit_Platform.id_PlanZakaz).First();
                debit_WorkBit.close = true;
                debit_WorkBit.dateClose = DateTime.Now;
                db.Entry(debit_WorkBit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Debit_WorkBit");
            }
            return View(debit_Platform);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_Platform debit_Platform = db.Debit_Platform.Find(id);
            if (debit_Platform == null)
            {
                return HttpNotFound();
            }
            return View(debit_Platform);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Debit_Platform debit_Platform = db.Debit_Platform.Find(id);
            db.Debit_Platform.Remove(debit_Platform);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult Debit(int? idPlanZakaz, int idTask)
        {
            Debit_Platform debit_Platform = db.Debit_Platform.Where(d => d.id_PlanZakaz == idPlanZakaz).First();
            if (debit_Platform == null)
            {
                return HttpNotFound();
            }
            return View(debit_Platform);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Debit([Bind(Include = "id,id_PlanZakaz,massa,countPlatform,numPlatform,numPlomb,gabar")] Debit_Platform debit_Platform, int? idTask)
        {
            if (ModelState.IsValid)
            {
                if (debit_Platform.massa == null)
                    debit_Platform.massa = 0;
                if (debit_Platform.numPlatform == null)
                    debit_Platform.numPlatform = "";
                if (debit_Platform.numPlomb == null)
                    debit_Platform.numPlomb = "";
                if (debit_Platform.gabar == null)
                    debit_Platform.gabar = "";
                if (debit_Platform.countPlatform == null)
                    debit_Platform.countPlatform = 0;
                db.Entry(debit_Platform).State = EntityState.Modified;
                db.SaveChanges();
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(idTask);
                debit_WorkBit.close = true;
                debit_WorkBit.dateClose = DateTime.Now;
                db.Entry(debit_WorkBit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Debit_WorkBit");
            }
            return RedirectToAction("Index", "Debit_WorkBit");
        }



        //public void SendMail(Debit_WorkBit debit_WorkBit)
        //{
        //    List<string> list = new List<string>();
        //    var debit_EmailList = db.Debit_EmailList.Where(d => d.C1 == true).ToList();
        //    foreach (var data in debit_EmailList)
        //    {
        //        list.Add(data.email);
        //    }


        //    EmailModel emailModel = new EmailModel();
        //    try
        //    {
        //        PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(debit_WorkBit.id_PlanZakaz);
        //        string textMail = "";
        //        textMail += pZ_PlanZakaz.PlanZakaz.ToString() + text;

        //        emailModel.SendEmailList(list, textMail, textMail);
        //    }
        //    catch
        //    {
        //        emailModel.SendEmailOnePerson("myi@katek.by", "ошибка отправки", debit_WorkBit.id.ToString());
        //    }
        //}
    }
}
