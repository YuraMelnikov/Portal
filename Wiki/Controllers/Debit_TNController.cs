using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wiki;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class Debit_TNController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();
        
        public ActionResult Index()
        {
            var debit_TN = db.Debit_TN.Include(d => d.Debit_WorkBit);
            return View(debit_TN.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_TN debit_TN = db.Debit_TN.Find(id);
            if (debit_TN == null)
            {
                return HttpNotFound();
            }
            return View(debit_TN);
        }
        
        public ActionResult Create(int? idf, int? idTask)
        {
            ViewBag.Order = db.PZ_PlanZakaz.Find(idf).PlanZakaz.ToString();
            ViewBag.id_DebitTask = idTask;

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "int, id_DebitTask, numberTN, dateTN, numberSF, dateSF, Summa")] Debit_TN debit_TN, int? idTask, int? idf)
        {
            if (ModelState.IsValid)
            {
                if (debit_TN.numberTN == null)
                    debit_TN.numberTN = "";
                if (debit_TN.numberSF == null)
                    debit_TN.numberSF = "";
                Debit_WorkBitController debit_WorkBitController = new Debit_WorkBitController();
                debit_WorkBitController.CloseComplitedTasks(Convert.ToInt32(idf), 11);
                debit_TN.id_DebitTask = Convert.ToInt32(idTask);
                db.Debit_TN.Add(debit_TN);
                db.SaveChanges();


                List<string> list = new List<string>();
                var debit_EmailList = db.Debit_EmailList.Where(d => d.C11 == true).ToList();
                foreach (var data in debit_EmailList)
                {
                    list.Add(data.email);
                }
                SendMail(debit_TN, " : внесены данные о ТН/СФ", list);

                return RedirectToAction("Index", "Debit_WorkBit");
            }

            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id", debit_TN.id_DebitTask);
            return View(debit_TN);
        }

        public void SendMail(Debit_TN debit_TN, string text, List<string> list)
        {
            EmailModel emailModel = new EmailModel();
            try
            {
                emailModel.SendEmailList(list, debit_TN.Debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString() + text, debit_TN.Debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString() + text);
            }
            catch
            {
                emailModel.SendEmailOnePerson("myi@katek.by", "ошибка отправки", debit_TN.@int.ToString());
            }
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_TN debit_TN = db.Debit_TN.Find(id);
            if (debit_TN == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id", debit_TN.id_DebitTask);
            return View(debit_TN);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "int,id_DebitTask,numberTN,dateTN,numberSF,dateSF,Summa")] Debit_TN debit_TN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(debit_TN).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_DebitTask = new SelectList(db.Debit_WorkBit, "id", "id", debit_TN.id_DebitTask);
            return View(debit_TN);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_TN debit_TN = db.Debit_TN.Find(id);
            if (debit_TN == null)
            {
                return HttpNotFound();
            }
            return View(debit_TN);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Debit_TN debit_TN = db.Debit_TN.Find(id);
            db.Debit_TN.Remove(debit_TN);
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
