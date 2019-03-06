using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wiki;

namespace Wiki.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class DevisionsController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index(string SortOrder)
        {
            ViewBag.FirstName = String.IsNullOrEmpty(SortOrder);
            var emp = from s in db.Devision
                      select s;
            emp = emp.OrderBy(s => s.name);


            return View(emp.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devision devision = db.Devision.Find(id);
            if (devision == null)
            {
                return HttpNotFound();
            }
            return View(devision);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] Devision devision)
        {
            if (ModelState.IsValid)
            {
                db.Devision.Add(devision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(devision);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devision devision = db.Devision.Find(id);
            if (devision == null)
            {
                return HttpNotFound();
            }
            return View(devision);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] Devision devision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(devision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(devision);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devision devision = db.Devision.Find(id);
            if (devision == null)
            {
                return HttpNotFound();
            }
            return View(devision);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Devision devision = db.Devision.Find(id);
            db.Devision.Remove(devision);
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
