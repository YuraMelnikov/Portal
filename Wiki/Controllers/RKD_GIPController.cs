using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Wiki.Controllers
{
    public class RKD_GIPController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        // GET: RKD_GIP
        public ActionResult Index()
        {
            var rKD_GIP = db.RKD_GIP.OrderBy(d => d.RKD_Order.PZ_PlanZakaz.PlanZakaz).ToList();
            return View(rKD_GIP);
        }

        // GET: RKD_GIP/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RKD_GIP rKD_GIP = db.RKD_GIP.Find(id);
            if (rKD_GIP == null)
            {
                return HttpNotFound();
            }
            return View(rKD_GIP);
        }

        // GET: RKD_GIP/Create
        public ActionResult Create()
        {
            ViewBag.id_UserKBE = new SelectList(db.AspNetUsers.OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            ViewBag.id_UserKBM = new SelectList(db.AspNetUsers.OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            ViewBag.id_RKD_Order = new SelectList(db.RKD_Order, "id", "id");
            return View();
        }

        // POST: RKD_GIP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_UserKBM,id_UserKBE,id_RKD_Order")] RKD_GIP rKD_GIP)
        {
            if (ModelState.IsValid)
            {
                db.RKD_GIP.Add(rKD_GIP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_UserKBE = new SelectList(db.AspNetUsers.OrderBy(d => d.CiliricalName), "Id", "CiliricalName", rKD_GIP.id_UserKBE);
            ViewBag.id_UserKBM = new SelectList(db.AspNetUsers.OrderBy(d => d.CiliricalName), "Id", "CiliricalName", rKD_GIP.id_UserKBM);
            ViewBag.id_RKD_Order = new SelectList(db.RKD_Order, "id", "id", rKD_GIP.id_RKD_Order);
            return View(rKD_GIP);
        }

        [Authorize(Roles = "Admin, KBE, KBM")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RKD_GIP rKD_GIP = db.RKD_GIP.Find(id);
            if (rKD_GIP == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_UserKBE = new SelectList(db.AspNetUsers.Where(d => d.Devision == 16 || d.Devision == 3).OrderBy(d => d.CiliricalName), "Id", "CiliricalName", rKD_GIP.id_UserKBE);
            ViewBag.id_UserKBM = new SelectList(db.AspNetUsers.Where(d => d.Devision == 15).OrderBy(d => d.CiliricalName), "Id", "CiliricalName", rKD_GIP.id_UserKBM);
            ViewBag.id_RKD_Order = new SelectList(db.RKD_Order, "id", "id", rKD_GIP.id_RKD_Order);
            return View(rKD_GIP);
        }

        // POST: RKD_GIP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_UserKBM,id_UserKBE,id_RKD_Order")] RKD_GIP rKD_GIP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rKD_GIP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_UserKBE = new SelectList(db.AspNetUsers.OrderBy(d => d.CiliricalName), "Id", "CiliricalName", rKD_GIP.id_UserKBE);
            ViewBag.id_UserKBM = new SelectList(db.AspNetUsers.OrderBy(d => d.CiliricalName), "Id", "CiliricalName", rKD_GIP.id_UserKBM);
            ViewBag.id_RKD_Order = new SelectList(db.RKD_Order, "id", "id", rKD_GIP.id_RKD_Order);
            return View(rKD_GIP);
        }

        // GET: RKD_GIP/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RKD_GIP rKD_GIP = db.RKD_GIP.Find(id);
            if (rKD_GIP == null)
            {
                return HttpNotFound();
            }
            return View(rKD_GIP);
        }

        // POST: RKD_GIP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RKD_GIP rKD_GIP = db.RKD_GIP.Find(id);
            db.RKD_GIP.Remove(rKD_GIP);
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
