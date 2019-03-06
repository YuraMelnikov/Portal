using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Wiki.Controllers
{
    public class OTK_CounterErrorKOController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            return View(db.OTK_CounterErrorKO.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_CounterErrorKO oTK_CounterErrorKO = db.OTK_CounterErrorKO.Find(id);
            if (oTK_CounterErrorKO == null)
            {
                return HttpNotFound();
            }
            return View(oTK_CounterErrorKO);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,count")] OTK_CounterErrorKO oTK_CounterErrorKO)
        {
            if (ModelState.IsValid)
            {
                db.OTK_CounterErrorKO.Add(oTK_CounterErrorKO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oTK_CounterErrorKO);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_CounterErrorKO oTK_CounterErrorKO = db.OTK_CounterErrorKO.Find(id);
            if (oTK_CounterErrorKO == null)
            {
                return HttpNotFound();
            }
            return View(oTK_CounterErrorKO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,count")] OTK_CounterErrorKO oTK_CounterErrorKO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oTK_CounterErrorKO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oTK_CounterErrorKO);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_CounterErrorKO oTK_CounterErrorKO = db.OTK_CounterErrorKO.Find(id);
            if (oTK_CounterErrorKO == null)
            {
                return HttpNotFound();
            }
            return View(oTK_CounterErrorKO);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OTK_CounterErrorKO oTK_CounterErrorKO = db.OTK_CounterErrorKO.Find(id);
            db.OTK_CounterErrorKO.Remove(oTK_CounterErrorKO);
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
