using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Wiki.Controllers
{
    public class Debit_CMRController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        // GET: Debit_CMR
        public ActionResult Index()
        {
            var debit_CMR = db.Debit_CMR.Include(d => d.Debit_WorkBit);
            return View(debit_CMR.ToList());
        }

        // GET: Debit_CMR/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_CMR debit_CMR = db.Debit_CMR.Find(id);
            if (debit_CMR == null)
            {
                return HttpNotFound();
            }
            return View(debit_CMR);
        }

        // GET: Debit_CMR/Create
        public ActionResult Create()
        {
            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id");
            return View();
        }

        // POST: Debit_CMR/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_DebitTask,dateShip,number")] Debit_CMR debit_CMR)
        {
            if (ModelState.IsValid)
            {
                db.Debit_CMR.Add(debit_CMR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id", debit_CMR.id_DebitTask);
            return View(debit_CMR);
        }

        // GET: Debit_CMR/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_CMR debit_CMR = db.Debit_CMR.Find(id);
            if (debit_CMR == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id", debit_CMR.id_DebitTask);
            return View(debit_CMR);
        }

        // POST: Debit_CMR/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_DebitTask,dateShip,number")] Debit_CMR debit_CMR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(debit_CMR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id", debit_CMR.id_DebitTask);
            return View(debit_CMR);
        }

        // GET: Debit_CMR/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_CMR debit_CMR = db.Debit_CMR.Find(id);
            if (debit_CMR == null)
            {
                return HttpNotFound();
            }
            return View(debit_CMR);
        }

        // POST: Debit_CMR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Debit_CMR debit_CMR = db.Debit_CMR.Find(id);
            db.Debit_CMR.Remove(debit_CMR);
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
