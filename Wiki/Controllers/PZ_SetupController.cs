using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class PZ_SetupController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();
        
        public ActionResult Index()
        {
            var pZ_Setup = db.PZ_Setup.Include(p => p.PZ_PlanZakaz).OrderByDescending(p => p.PZ_PlanZakaz.PlanZakaz);
            return View(pZ_Setup.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PZ_Setup pZ_Setup = db.PZ_Setup.Find(id);
            if (pZ_Setup == null)
            {
                return HttpNotFound();
            }
            return View(pZ_Setup);
        }
        
        public ActionResult Create()
        {
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PZ_Setup pZ_Setup)
        {
            if (ModelState.IsValid)
            {
                db.PZ_Setup.Add(pZ_Setup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", pZ_Setup.id_PZ_PlanZakaz);
            return View(pZ_Setup);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PZ_Setup pZ_Setup = db.PZ_Setup.Find(id);
            if (pZ_Setup == null)
            {
                return HttpNotFound();
            }
            ViewBag.userTP = new SelectList(db.AspNetUsers.Where(x => x.Devision == 4).OrderBy(x => x.CiliricalName), "id", "CiliricalName", pZ_Setup.userTP);
            PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(pZ_Setup.id_PZ_PlanZakaz);
            ViewBag.Name = pZ_PlanZakaz.Name.ToString();
            ViewBag.NameTU = pZ_PlanZakaz.nameTU.ToString();
            ViewBag.Zakaz = pZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", pZ_Setup.id_PZ_PlanZakaz);
            return View(pZ_Setup);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PZ_Setup pZ_Setup, bool incorrectName, bool nullTP)
        {
            if (ModelState.IsValid)
            {
                if (nullTP == true)
                    pZ_Setup.userTP = null;
                if (pZ_Setup.PunktDogovoraOSrokahPriemki == null)
                    pZ_Setup.PunktDogovoraOSrokahPriemki = "";
                if (pZ_Setup.UslovieOplatyText == null)
                    pZ_Setup.UslovieOplatyText = "";
                db.Entry(pZ_Setup).State = EntityState.Modified;
                db.SaveChanges();
                try
                {
                    if (pZ_Setup.PunktDogovoraOSrokahPriemki != "")
                    {
                        Debit_WorkBit task = db.Debit_WorkBit.Where(d => d.close == false)
                            .Where(d => d.id_PlanZakaz == pZ_Setup.id_PZ_PlanZakaz)
                            .Where(d => d.id_TaskForPZ == 4)
                            .First();

                        if (task != null)
                        {
                            task = db.Debit_WorkBit.Find(task.id);
                            task.close = true;
                            task.dateClose = DateTime.Now;
                            db.Entry(task).State = EntityState.Modified;
                            db.SaveChanges();
                            EmailModel emailModel = new EmailModel();
                            List<string> list = new List<string>();
                            var debit_EmailList = db.Debit_EmailList.Where(d => d.C4 == true).ToList();
                            foreach (var data1 in debit_EmailList)
                            {
                                list.Add(data1.email);
                            }
                            SendMail(task, " : заполнены договорные условия", list);

                            List<TaskForPZ> dateTaskWork = db.TaskForPZ
                                .Where(w => w.Predecessors == 4)
                                .Where(z => z.id_TypeTaskForPZ == 1)
                                .ToList();
                            foreach (var data in dateTaskWork)
                            {
                                Debit_WorkBit newDebit_WorkBit = new Debit_WorkBit();
                                newDebit_WorkBit.dateCreate = DateTime.Now;
                                newDebit_WorkBit.close = false;
                                newDebit_WorkBit.id_PlanZakaz = task.id_PlanZakaz;
                                newDebit_WorkBit.id_TaskForPZ = (int)data.id;
                                newDebit_WorkBit.datePlanFirst = DateTime.Now.AddDays((double)data.time);
                                newDebit_WorkBit.datePlan = DateTime.Now.AddDays((double)data.time);
                                db.Debit_WorkBit.Add(new Debit_WorkBit()
                                {
                                    close = false,
                                    dateClose = null,
                                    dateCreate = DateTime.Now,
                                    datePlan = newDebit_WorkBit.datePlan,
                                    datePlanFirst = newDebit_WorkBit.datePlanFirst,
                                    id_PlanZakaz = newDebit_WorkBit.id_PlanZakaz,
                                    id_TaskForPZ = newDebit_WorkBit.id_TaskForPZ
                                });
                                db.SaveChanges();
                            }
                        }
                    }
                }
                catch 
                {

                }

                return Redirect("/Debit_WorkBit/Index");
            }
            ViewBag.userTP = new SelectList(db.AspNetUsers.Where(x => x.Devision == 4).OrderBy(x => x.CiliricalName), "id", "CiliricalName", pZ_Setup.userTP);
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", pZ_Setup.id_PZ_PlanZakaz);
            return View(pZ_Setup);
        }
        
        public void SendMail(Debit_WorkBit debit_WorkBit, string text, List<string> list)
        {
            EmailModel emailModel = new EmailModel();
            try
            {
                emailModel.SendEmailList(list, debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString() + text, debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString() + text);
            }
            catch
            {
                emailModel.SendEmailOnePerson("myi@katek.by", "ошибка отправки", debit_WorkBit.id.ToString());
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PZ_Setup pZ_Setup = db.PZ_Setup.Find(id);
            if (pZ_Setup == null)
            {
                return HttpNotFound();
            }
            return View(pZ_Setup);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PZ_Setup pZ_Setup = db.PZ_Setup.Find(id);
            db.PZ_Setup.Remove(pZ_Setup);
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
