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
    public class VVPZsController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();
        
        public ActionResult Index()
        {
            var vVPZ = db.VVPZ.Include(v => v.PZ_PlanZakaz).OrderByDescending(v => v.dateSh);
            return View(vVPZ.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VVPZ vVPZ = db.VVPZ.Find(id);
            if (vVPZ == null)
            {
                return HttpNotFound();
            }
            return View(vVPZ);
        }

        public ActionResult Create()
        {
            int[] array = new int[100];
            int step = 0;
            foreach (var data in db.Debit_WorkBit.Where(d => d.id_TaskForPZ == 21).Where(d => d.close == false))
            {
                array[step] = data.id_PlanZakaz;
            }
            List<PZ_PlanZakaz> listPZ = new List<PZ_PlanZakaz>();
            foreach (var data in array)
            {
                PZ_PlanZakaz planZakaz = db.PZ_PlanZakaz.Find(data);
                listPZ.Add(planZakaz);
            }
            ViewBag.id_PZ_PlanZakaz = new SelectList(listPZ, "Id", "PlanZakaz");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_PZ_PlanZakaz,name,description,massaBrutto,massaNetto,gruzopoluchatel,adresGruzo,iNNGruz,oKPOGruz,kodGruz,sTNazn,kodSt,osobieOtm,prochee")] VVPZ vVPZ)
        {
            if (ModelState.IsValid)
            {
                db.VVPZ.Add(vVPZ);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", vVPZ.id_PZ_PlanZakaz);
            return View(vVPZ);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VVPZ vVPZ = db.VVPZ.Find(id);
            if (vVPZ == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", vVPZ.id_PZ_PlanZakaz);
            return View(vVPZ);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_PZ_PlanZakaz,name,description,massaBrutto,massaNetto,gruzopoluchatel,adresGruzo,iNNGruz,oKPOGruz,kodGruz,sTNazn,kodSt,osobieOtm,prochee")] VVPZ vVPZ)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vVPZ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", vVPZ.id_PZ_PlanZakaz);
            return View(vVPZ);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VVPZ vVPZ = db.VVPZ.Find(id);
            if (vVPZ == null)
            {
                return HttpNotFound();
            }
            return View(vVPZ);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VVPZ vVPZ = db.VVPZ.Find(id);
            db.VVPZ.Remove(vVPZ);
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
