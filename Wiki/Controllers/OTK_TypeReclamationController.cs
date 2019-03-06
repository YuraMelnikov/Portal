using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Wiki.Controllers
{
    public class OTK_TypeReclamationController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            return View(db.OTK_TypeReclamation.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_TypeReclamation oTK_TypeReclamation = db.OTK_TypeReclamation.Find(id);
            if (oTK_TypeReclamation == null)
            {
                return HttpNotFound();
            }
            return View(oTK_TypeReclamation);
        }

        [Authorize(Roles = "Admin, OTK")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Name")] OTK_TypeReclamation oTK_TypeReclamation)
        {
            if (ModelState.IsValid)
            {
                db.OTK_TypeReclamation.Add(oTK_TypeReclamation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oTK_TypeReclamation);
        }

        [Authorize(Roles = "Admin, OTK")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_TypeReclamation oTK_TypeReclamation = db.OTK_TypeReclamation.Find(id);
            if (oTK_TypeReclamation == null)
            {
                return HttpNotFound();
            }
            return View(oTK_TypeReclamation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name")] OTK_TypeReclamation oTK_TypeReclamation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oTK_TypeReclamation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oTK_TypeReclamation);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_TypeReclamation oTK_TypeReclamation = db.OTK_TypeReclamation.Find(id);
            if (oTK_TypeReclamation == null)
            {
                return HttpNotFound();
            }
            return View(oTK_TypeReclamation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OTK_TypeReclamation oTK_TypeReclamation = db.OTK_TypeReclamation.Find(id);
            db.OTK_TypeReclamation.Remove(oTK_TypeReclamation);
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
