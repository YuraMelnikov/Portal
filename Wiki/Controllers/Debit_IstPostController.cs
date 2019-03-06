using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Wiki.Controllers
{
    public class Debit_IstPostController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();
        
        public ActionResult Index()
        {
            var debit_IstPost = db.Debit_IstPost.Include(d => d.Debit_WorkBit);
            return View(debit_IstPost.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_IstPost debit_IstPost = db.Debit_IstPost.Find(id);
            if (debit_IstPost == null)
            {
                return HttpNotFound();
            }
            return View(debit_IstPost);
        }
        
        public ActionResult Create(int? idf, int? idTask)
        {
            ViewBag.Order = db.PZ_PlanZakaz.Find(idf).PlanZakaz.ToString();
            ViewBag.id_DebitTask = idTask;
            ViewBag.currency = new SelectList(db.PZ_Currency, "id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_DebitTask,transportSum,numberOrder,ndsSum, currency")] Debit_IstPost debit_IstPost,
            int? idTask, int? idf)
        {
            if (ModelState.IsValid)
            {
                if (debit_IstPost.numberOrder == null)
                    debit_IstPost.numberOrder = "";
                Debit_WorkBitController debit_WorkBitController = new Debit_WorkBitController();
                debit_WorkBitController.CloseComplitedTasks(Convert.ToInt32(idf), 26);
                debit_IstPost.id_DebitTask = Convert.ToInt32(idTask);
                db.Debit_IstPost.Add(debit_IstPost);
                db.SaveChanges();
                return RedirectToAction("Index", "Debit_WorkBit");
            }

            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id", debit_IstPost.id_DebitTask);
            return View(debit_IstPost);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_IstPost debit_IstPost = db.Debit_IstPost.Find(id);
            if (debit_IstPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id", debit_IstPost.id_DebitTask);
            return View(debit_IstPost);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_DebitTask,transportSum,numberOrder,ndsSum")] Debit_IstPost debit_IstPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(debit_IstPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id", debit_IstPost.id_DebitTask);
            return View(debit_IstPost);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_IstPost debit_IstPost = db.Debit_IstPost.Find(id);
            if (debit_IstPost == null)
            {
                return HttpNotFound();
            }
            return View(debit_IstPost);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Debit_IstPost debit_IstPost = db.Debit_IstPost.Find(id);
            db.Debit_IstPost.Remove(debit_IstPost);
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
    }
}
