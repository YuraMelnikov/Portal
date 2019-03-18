using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Wiki.Controllers
{
    public class Debit_CostUpdateController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            var debit_CostUpdate = db.Debit_CostUpdate.Include(d => d.PZ_PlanZakaz);
            return View(debit_CostUpdate.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_CostUpdate debit_CostUpdate = db.Debit_CostUpdate.Find(id);
            if (debit_CostUpdate == null)
            {
                return HttpNotFound();
            }
            return View(debit_CostUpdate);
        }
        
        public ActionResult Create()
        {
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_PZ_PlanZakaz,cost,dateCreate")] Debit_CostUpdate debit_CostUpdate)
        {
            if (ModelState.IsValid)
            {
                db.Debit_CostUpdate.Add(debit_CostUpdate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_CostUpdate.id_PZ_PlanZakaz);
            return View(debit_CostUpdate);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_CostUpdate debit_CostUpdate = db.Debit_CostUpdate.Find(id);
            if (debit_CostUpdate == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_CostUpdate.id_PZ_PlanZakaz);
            return View(debit_CostUpdate);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_PZ_PlanZakaz,cost,dateCreate")] Debit_CostUpdate debit_CostUpdate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(debit_CostUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NewPlus", "Debit_CostUpdate", new { id_PlanZakaz = debit_CostUpdate.id_PZ_PlanZakaz, idTask = 15 });
            }
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_CostUpdate.id_PZ_PlanZakaz);
            return View(debit_CostUpdate);
        }

        //[HttpGet]
        public ActionResult EditPartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_CostUpdate debit_CostUpdate = db.Debit_CostUpdate.Find(id);
            if (debit_CostUpdate == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_CostUpdate.id_PZ_PlanZakaz);
            return PartialView(debit_CostUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartial([Bind(Include = "id,id_PZ_PlanZakaz,cost,dateCreate")] Debit_CostUpdate debit_CostUpdate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(debit_CostUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NewPlus", "Debit_CostUpdate", new { id_PlanZakaz = debit_CostUpdate.id_PZ_PlanZakaz, idTask = 15 });
            }
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_CostUpdate.id_PZ_PlanZakaz);
            return View(debit_CostUpdate);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_CostUpdate debit_CostUpdate = db.Debit_CostUpdate.Find(id);
            if (debit_CostUpdate == null)
            {
                return HttpNotFound();
            }
            return View(debit_CostUpdate);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Debit_CostUpdate debit_CostUpdate = db.Debit_CostUpdate.Find(id);
            db.Debit_CostUpdate.Remove(debit_CostUpdate);
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

        public ActionResult NewPlus(int id_PlanZakaz, int idTask)
        {
            double getCost = 0;
            ViewBag.PlanZakaz = db.PZ_PlanZakaz.Find(id_PlanZakaz).PlanZakaz.ToString();
            ViewBag.idPlanZakaz = db.PZ_PlanZakaz.Find(id_PlanZakaz).Id;
            System.Globalization.NumberFormatInfo numForInf = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            ViewBag.Cost = db.PZ_TEO.Where(d => d.Id_PlanZakaz == id_PlanZakaz).First().OtpuskChena.ToString("N", numForInf);
            ViewBag.Curency = db.PZ_TEO.Where(d => d.Id_PlanZakaz == id_PlanZakaz).First().PZ_Currency.Name.ToString();
            var listCost = db.Debit_CostUpdate.Where(d => d.id_PZ_PlanZakaz == id_PlanZakaz).ToList();
            foreach(var data in listCost)
            {
                getCost += data.cost;
            }
            ViewBag.GetCost = getCost.ToString("N", numForInf);


            PZ_TEO pZ_TEO = db.PZ_TEO.Where(d => d.Id_PlanZakaz == id_PlanZakaz).First();

            double nds = 0;
            double ndsCost = pZ_TEO.OtpuskChena;
            if (pZ_TEO.NDS != null)
            {
                ndsCost += (double)pZ_TEO.NDS;
                nds += (double)pZ_TEO.NDS;
            }


            ViewBag.NDS = nds.ToString("N", numForInf);
            ViewBag.CostNDS = ndsCost.ToString("N", numForInf);


            ViewBag.PostCost = (ndsCost - getCost).ToString("N", numForInf);
            ViewBag.listCost = listCost.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPlus([Bind(Include = "id,id_PZ_PlanZakaz,cost,dateCreate")] Debit_CostUpdate debit_CostUpdate, int? id_PlanZakaz)
        {
            if (ModelState.IsValid)
            {
                PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(id_PlanZakaz);
                debit_CostUpdate.id_PZ_PlanZakaz = pZ_PlanZakaz.Id;
                debit_CostUpdate.dateCreate = DateTime.Now;
                db.Debit_CostUpdate.Add(debit_CostUpdate);
                db.SaveChanges();
                PZ_TEO pZ_TEO = db.PZ_TEO.Where(d => d.Id_PlanZakaz == id_PlanZakaz).First();
                double costCorrect = (double)pZ_TEO.NDS + (double)pZ_TEO.OtpuskChena;
                double costNow = 0;
                foreach(var data in db.Debit_CostUpdate.Where(d => d.id_PZ_PlanZakaz == id_PlanZakaz))
                {
                    costNow += data.cost;
                }

                if(costCorrect - costNow == 0)
                {
                    Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == id_PlanZakaz & d.id_TaskForPZ == 15).First();
                    debit_WorkBit.close = true;
                    debit_WorkBit.dateClose = DateTime.Now;
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Debit_WorkBit");
            }

            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_CostUpdate.id_PZ_PlanZakaz);
            return View(debit_CostUpdate);
        }
    }
}
