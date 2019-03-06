using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class PZ_TEOController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        [Authorize(Roles = "Admin, Fin director")]
        public ActionResult Index()
        {
            var pZ_TEO = db.PZ_TEO.OrderByDescending(p => p.PZ_PlanZakaz.PlanZakaz);
            return View(pZ_TEO.ToList());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PZ_TEO pZ_TEO = db.PZ_TEO.Find(id);
            if (pZ_TEO == null)
            {
                return HttpNotFound();
            }
            ViewBag.Currency = new SelectList(db.PZ_Currency, "id", "Name", pZ_TEO.Currency);
            ViewBag.PZ = pZ_TEO.PZ_PlanZakaz.PlanZakaz.ToString();
            return View(pZ_TEO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PZ_TEO pZ_TEO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pZ_TEO).State = EntityState.Modified;
                db.SaveChanges();
                if(pZ_TEO.KursValuti > 0)
                {
                    try
                    {
                        Debit_WorkBit task = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_PlanZakaz == pZ_TEO.Id_PlanZakaz).Where(d => d.id_TaskForPZ == 2).First();
                        if (task != null)
                        {
                            task = db.Debit_WorkBit.Find(task.id);
                            task.close = true;
                            task.dateClose = DateTime.Now;
                            db.Entry(task).State = EntityState.Modified;
                            db.SaveChanges();
                            List<string> list = new List<string>();
                            var debit_EmailList = db.Debit_EmailList.Where(d => d.C2 == true).ToList();
                            foreach (var data in debit_EmailList)
                            {
                                list.Add(data.email);
                            }
                            SendMail(task, " : ТЭО заказа зафиксировано (внесена предрасчетная С/С материалов в Project)", list);
                        }
                        else
                        {
                            return Redirect("/PZ_TEO/Index");
                        }
                    }
                    catch
                    {
                        return Redirect("/PZ_TEO/Index");
                    }
                }
                return Redirect("/PZ_TEO/Index");
            }
            ViewBag.Currency = new SelectList(db.PZ_Currency, "id", "Name", pZ_TEO.Currency);
            return Redirect("/PZ_TEO/Index");
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
    }
}
