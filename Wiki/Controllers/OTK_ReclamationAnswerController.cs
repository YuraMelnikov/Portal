using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    [Authorize(Roles = "Admin, Technologist, KBE, KBMUser, KBEUser, OTK, KBM, OS, Manufacturing")]
    public class OTK_ReclamationAnswerController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            var oTK_ReclamationAnswer = db.OTK_ReclamationAnswer.Include(o => o.Devision).Include(o => o.OTK_Reclamation);
            return View(oTK_ReclamationAnswer.ToList());
        }

        public ActionResult IndexSP(int? myDevision)
        {
            var oTK_ReclamationAnswer = new List<OTK_ReclamationAnswer>();


            if (myDevision == 6 || myDevision == 13)
            {
                oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                    .Where(c => c.OTK_Reclamation.Complited == false)
                    .OrderByDescending(z => z.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz).ToList();
            }
            else if (myDevision == 8)
            {
                oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                    .Where(x => x.Devision == 8 || x.Devision == 20)
                    .Where(c => c.OTK_Reclamation.Complited == false)
                    .OrderByDescending(z => z.DateCreate).ToList();
            }
            else if (myDevision == 10)
            {
                oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                    .Where(x => x.Devision == 10 || x.Devision == 7)
                    .Where(c => c.OTK_Reclamation.Complited == false)
                    .OrderByDescending(z => z.DateCreate).ToList();
            }
            else
            {
                oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                    .Where(x => x.Devision == myDevision)
                    .Where(c => c.OTK_Reclamation.Complited == false)
                    .OrderBy(z => z.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz).ToList();
            }

            return View(oTK_ReclamationAnswer.ToList());
        }

        public ActionResult IndexAll()
        {
            var oTK_ReclamationAnswer = db.OTK_ReclamationAnswer
                .Where(c => c.OTK_Reclamation.Complited == false)
                .OrderBy(z => z.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz);

            return View(oTK_ReclamationAnswer.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_ReclamationAnswer oTK_ReclamationAnswer = db.OTK_ReclamationAnswer.Find(id);
            if (oTK_ReclamationAnswer == null)
            {
                return HttpNotFound();
            }
            return View(oTK_ReclamationAnswer);
        }

        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad")]
        public ActionResult EditKO(int? idRA)
        {
            OTK_ReclamationAnswer oTK_ReclamationAnswer = db.OTK_ReclamationAnswer.Find(idRA);
            string login = HttpContext.User.Identity.Name;
            var user = db.AspNetUsers.Where(d => d.Email == login).First().Devision;
            if (user == 3 || user == 16)
            {
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 16 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);
            }
            else if (user == 15)
            {
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);
            }
            if(login == "Kuchynski@katek.by")
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 16 || d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationAnswer.countError);
            ViewBag.ReclamationId = new SelectList(db.OTK_Reclamation, "Id", "ReclamationText", oTK_ReclamationAnswer.ReclamationId);
            ViewBag.NumberOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.NameOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.Name.ToString();
            ViewBag.TextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();

            ViewBag.Textid = oTK_ReclamationAnswer.OTK_Reclamation.Id.ToString();
            ViewBag.TextdateCreate = oTK_ReclamationAnswer.OTK_Reclamation.DateTimeCreate.ToString();
            ViewBag.TextPlanZakaz = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.TexttextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();
            ViewBag.TextCiliricalName = oTK_ReclamationAnswer.OTK_Reclamation.AspNetUsers.CiliricalName.ToString();
           
            return View(oTK_ReclamationAnswer);
        }
        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKO(OTK_ReclamationAnswer oTK_ReclamationAnswer)
        {
            
            string login = HttpContext.User.Identity.Name;
            login = db.AspNetUsers.Where(d => d.Email == login).First().Id;
            oTK_ReclamationAnswer.UserUpdate = login;
            oTK_ReclamationAnswer.Completed = true;
            Devision devision = db.Devision.Find(oTK_ReclamationAnswer.Devision);
            oTK_ReclamationAnswer.DateUpdate = DateTime.Now;
            if (oTK_ReclamationAnswer.Text == null)
                oTK_ReclamationAnswer.Text = "";
            db.Entry(oTK_ReclamationAnswer).State = EntityState.Modified;
            db.SaveChanges();
            if(oTK_ReclamationAnswer.Completed == true)
            {
                OTK_Reclamation oTK_Reclamation = db.OTK_Reclamation.Find(oTK_ReclamationAnswer.ReclamationId);
                oTK_Reclamation.AnswerDate = DateTime.Now;
                oTK_Reclamation.Devision = devision.id;
                string devisionName = db.Devision.Find(oTK_ReclamationAnswer.Devision).name;
                oTK_Reclamation.AnswerText += "\n" + devisionName + ": " + oTK_ReclamationAnswer.Text;
                oTK_Reclamation.AnswerUser = oTK_ReclamationAnswer.UserUpdate;
                db.Entry(oTK_Reclamation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "ReclamationOTK");
            }
            ViewBag.Error = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationAnswer.countError);
            ViewBag.NumberOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.NameOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.Name.ToString();
            ViewBag.TextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();
            return RedirectToAction("Index", "ReclamationOTK");
        }

        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_ReclamationAnswer oTK_ReclamationAnswer = db.OTK_ReclamationAnswer.Find(id);

            if (oTK_ReclamationAnswer.Devision == 3 || oTK_ReclamationAnswer.Devision == 15 || oTK_ReclamationAnswer.Devision == 16)
            {
                return RedirectToAction("EditKO", "OTK_ReclamationAnswer", new { idRA = oTK_ReclamationAnswer.id});
            }
            if (oTK_ReclamationAnswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReclamationId = new SelectList(db.OTK_Reclamation, "Id", "ReclamationText", oTK_ReclamationAnswer.ReclamationId);
            ViewBag.NumberOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.NameOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.Name.ToString();
            ViewBag.TextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();
            return View(oTK_ReclamationAnswer);
        }
        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OTK_ReclamationAnswer oTK_ReclamationAnswer)
        {
            string login = HttpContext.User.Identity.Name;
            login = db.AspNetUsers.Where(d => d.Email == login).First().Id;
            oTK_ReclamationAnswer.Completed = true;
            oTK_ReclamationAnswer.UserUpdate = login;
            oTK_ReclamationAnswer.DateUpdate = DateTime.Now;
            if (oTK_ReclamationAnswer.Text == null)
                oTK_ReclamationAnswer.Text = "";
            db.Entry(oTK_ReclamationAnswer).State = EntityState.Modified;
            db.SaveChanges();
            if (oTK_ReclamationAnswer.Completed == true)
            {
                OTK_Reclamation oTK_Reclamation = db.OTK_Reclamation.Find(oTK_ReclamationAnswer.ReclamationId);
                oTK_Reclamation.AnswerDate = DateTime.Now;
                string devisionName = db.Devision.Find(oTK_ReclamationAnswer.Devision).name;
                oTK_Reclamation.AnswerText += "\n" + devisionName + ": " +  oTK_ReclamationAnswer.Text;
                oTK_Reclamation.AnswerUser = oTK_ReclamationAnswer.UserUpdate;
                db.Entry(oTK_Reclamation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "ReclamationOTK");
            }
            ViewBag.Error = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationAnswer.countError);
            ViewBag.NumberOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.NameOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.Name.ToString();
            ViewBag.TextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();
            return RedirectToAction("Index", "ReclamationOTK");
        }

        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad, KBMUser, KBEUser")]
        public ActionResult EditKOPartial(int? id)
        {
            OTK_ReclamationAnswer oTK_ReclamationAnswer = db.OTK_ReclamationAnswer.Find(id);
            string login = HttpContext.User.Identity.Name;
            var user = db.AspNetUsers.Where(d => d.Email == login).First().Devision;
            if (login == "fvs@katek.by")
            {
                ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationAnswer.countError);
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 16 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);
                ViewBag.reloadError = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 3 && d.id != 16).OrderBy(d => d.name), "id", "name");
            }
            else if (login == "nrf@katek.by")
            {
                ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationAnswer.countError);
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);
                ViewBag.reloadError = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 15).OrderBy(d => d.name), "id", "name");
            }
            //else if (login == "myi@katek.by")
            //{
            //    ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationAnswer.countError);
            //    ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);
            //    ViewBag.reloadError = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 15).OrderBy(d => d.name), "id", "name");
            //}
            else if (login == "Kuchynski@katek.by")
            {
                ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationAnswer.countError);
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 16 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);
                ViewBag.reloadError = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 3 && d.id != 16).OrderBy(d => d.name), "id", "name");
            }
            else if (user == 3 || user == 16)
            {
                ViewBag.countError = new SelectList(db.OTK_CounterErrorKO.Where(d => d.id == 1), "id", "name", oTK_ReclamationAnswer.countError);
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Email == login), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);
                ViewBag.reloadError = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 3 && d.id != 16).OrderBy(d => d.name), "id", "name");
            }
            else if (user == 15)
            {
                ViewBag.countError = new SelectList(db.OTK_CounterErrorKO.Where(d => d.id == 1), "id", "name", oTK_ReclamationAnswer.countError);
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Email == login), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);
                ViewBag.reloadError = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 3 && d.id != 16).OrderBy(d => d.name), "id", "name");
            }


            ViewBag.ReclamationId = new SelectList(db.OTK_Reclamation, "Id", "ReclamationText", oTK_ReclamationAnswer.ReclamationId);
            ViewBag.NumberOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.NameOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.Name.ToString();
            ViewBag.TextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();
            ViewBag.Textid = oTK_ReclamationAnswer.OTK_Reclamation.Id.ToString();
            ViewBag.TextdateCreate = oTK_ReclamationAnswer.OTK_Reclamation.DateTimeCreate.ToString();
            ViewBag.TextPlanZakaz = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.TexttextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();
            ViewBag.TextCiliricalName = oTK_ReclamationAnswer.OTK_Reclamation.AspNetUsers.CiliricalName.ToString();

            return View(oTK_ReclamationAnswer);
        }
        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad, KBMUser, KBEUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKOPartial(OTK_ReclamationAnswer oTkReclamationAnswer, string reloadError, bool newDevision)
        {
            string login = HttpContext.User.Identity.Name;
            login = db.AspNetUsers.First(d => d.Email == login).Id;
            string recipient = db.AspNetUsers.First(d => d.Id == login).Email;
            string loginEmail = db.AspNetUsers.First(d => d.Id == login).Email;
            oTkReclamationAnswer.UserUpdate = login;
            oTkReclamationAnswer.Completed = true;
            var user = db.AspNetUsers.Where(d => d.Id == login).First().Devision;
            if (loginEmail == "fvs@katek.by")
            {
                if (oTkReclamationAnswer.descriptionManagerKO == null)
                    oTkReclamationAnswer.descriptionManagerKO = "-";
                if (oTkReclamationAnswer.descriptionManagerKO == "")
                    oTkReclamationAnswer.descriptionManagerKO = "-";
            }
            else if (loginEmail == "nrf@katek.by")
            {
                if (oTkReclamationAnswer.descriptionManagerKO == null)
                    oTkReclamationAnswer.descriptionManagerKO = "-";
                if (oTkReclamationAnswer.descriptionManagerKO == "")
                    oTkReclamationAnswer.descriptionManagerKO = "-";
            }
            //else if (loginEmail == "myi@katek.by")
            //{
            //    if (oTkReclamationAnswer.descriptionManagerKO == null)
            //        oTkReclamationAnswer.descriptionManagerKO = "-";
            //    if (oTkReclamationAnswer.descriptionManagerKO == "")
            //        oTkReclamationAnswer.descriptionManagerKO = "-";
            //}
            else if (loginEmail == "Kuchynski@katek.by")
            {
                if (oTkReclamationAnswer.descriptionManagerKO == null)
                    oTkReclamationAnswer.descriptionManagerKO = "-";
                if (oTkReclamationAnswer.descriptionManagerKO == "")
                    oTkReclamationAnswer.descriptionManagerKO = "-";
            }
            else if (user == 3 || user == 16)
            {
                oTkReclamationAnswer.descriptionManagerKO =
                    db.OTK_ReclamationAnswer.Find(oTkReclamationAnswer.id).descriptionManagerKO;
                oTkReclamationAnswer.countError =
                    db.OTK_ReclamationAnswer.Find(oTkReclamationAnswer.id).countError;
            }
            else if (user == 15)
            {
                oTkReclamationAnswer.descriptionManagerKO =
                    db.OTK_ReclamationAnswer.Find(oTkReclamationAnswer.id).descriptionManagerKO;
                oTkReclamationAnswer.countError =
                    db.OTK_ReclamationAnswer.Find(oTkReclamationAnswer.id).countError;
            }
            Devision devision = db.Devision.Find(oTkReclamationAnswer.Devision);
            oTkReclamationAnswer.DateUpdate = DateTime.Now;
            if (oTkReclamationAnswer.Text == null)
                oTkReclamationAnswer.Text = "";
            OTK_Reclamation oTkReclamation = db.OTK_Reclamation.Find(oTkReclamationAnswer.ReclamationId);
            oTkReclamation.AnswerDate = DateTime.Now;
            string devisionName = db.Devision.Find(oTkReclamationAnswer.Devision)?.name;
            if (newDevision == false)
            {
                if (devision != null) oTkReclamation.Devision = devision.id;
                oTkReclamation.AnswerText += "\n" + devisionName + ": " + oTkReclamationAnswer.Text;
            }
            else
            {
                if (oTkReclamationAnswer.Text == null || oTkReclamationAnswer.Text == "")
                    oTkReclamationAnswer.Text = "замечание перенаправлено";
                oTkReclamationAnswer.countError = 4;
                oTkReclamation.Devision = Convert.ToInt32(reloadError);
                oTkReclamation.AnswerText += "\n" + devisionName + ": " + " замечание перенаправлено";
                OTK_ReclamationAnswer newAnswer = new OTK_ReclamationAnswer();
                newAnswer.ReclamationId = oTkReclamationAnswer.ReclamationId;
                newAnswer.Text = "";
                newAnswer.Devision = Convert.ToInt32(reloadError);
                newAnswer.User = oTkReclamationAnswer.User;
                newAnswer.DateCreate = DateTime.Now;
                newAnswer.Completed = false;
                db.OTK_ReclamationAnswer.Add(newAnswer);
                db.SaveChanges();
            }

            db.Entry(oTkReclamation).State = EntityState.Modified;
            db.SaveChanges();
            db.OTK_ReclamationAnswer.AsNoTracking();
            oTkReclamation.AnswerUser = oTkReclamationAnswer.UserUpdate;
            db.Set<OTK_ReclamationAnswer>().AddOrUpdate(oTkReclamationAnswer);
            db.SaveChanges();
            if(oTkReclamationAnswer.descriptionManagerKO != "-")
                PushEmail(oTkReclamationAnswer.id, db.AspNetUsers.First(d => d.Id == oTkReclamationAnswer.userError).Email, recipient);
            return RedirectToAction("Index", "ReclamationOTK");
        }

        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad, KBMUser, KBEUser")]
        public ActionResult EditPartial(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int dev = (int)db.AspNetUsers.First(f => f.Email == login).Devision;
            OTK_ReclamationAnswer oTK_ReclamationAnswer = db.OTK_ReclamationAnswer.Find(id);
            ViewBag.reloadError = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != dev).OrderBy(d => d.name), "id", "name");
            ViewBag.userPOError = new SelectList(db.UserPO.Where(d => d.id_Devision == dev).Where(d => d.active == true).OrderBy(d => d.name), "id", "name", oTK_ReclamationAnswer.userPO);
            ViewBag.ReclamationId = new SelectList(db.OTK_Reclamation, "Id", "ReclamationText", oTK_ReclamationAnswer.ReclamationId);
            ViewBag.NumberOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.NameOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.Name.ToString();
            ViewBag.TextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();
            return View(oTK_ReclamationAnswer);
        }
        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartial(OTK_ReclamationAnswer oTkReclamationAnswer, string reloadError, bool newDevision, int userPOError)
        {
            string login = HttpContext.User.Identity.Name;
            Devision devision = db.Devision.Find(oTkReclamationAnswer.Devision);
            OTK_Reclamation oTkReclamation = db.OTK_Reclamation.Find(oTkReclamationAnswer.ReclamationId);
            oTkReclamationAnswer.Completed = true;
            oTkReclamationAnswer.userPO = userPOError;
            oTkReclamationAnswer.UserUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            oTkReclamationAnswer.DateUpdate = DateTime.Now;
            if (oTkReclamationAnswer.Text == null)
                oTkReclamationAnswer.Text = "";
            oTkReclamation.AnswerDate = DateTime.Now;
            string devisionName = db.Devision.Find(oTkReclamationAnswer.Devision).name;
            if (newDevision == false)
            {
                if (devision != null) oTkReclamation.Devision = devision.id;
                oTkReclamation.AnswerText += "\n" + devisionName + ": " + oTkReclamationAnswer.Text;
            }
            else
            {
                if (oTkReclamationAnswer.Text == null || oTkReclamationAnswer.Text == "")
                    oTkReclamationAnswer.Text = "замечание перенаправлено";
                oTkReclamationAnswer.countError = 4;
                oTkReclamation.Devision = Convert.ToInt32(reloadError);
                oTkReclamation.AnswerText += "\n" + devisionName + ": " + " замечание перенаправлено";
                oTkReclamation.Description += "\n" + devisionName + ": " + " замечание перенаправлено";
                OTK_ReclamationAnswer newAnswer = new OTK_ReclamationAnswer();
                newAnswer.ReclamationId = oTkReclamationAnswer.ReclamationId;
                newAnswer.Text = "";
                newAnswer.Devision = Convert.ToInt32(reloadError);
                newAnswer.User = oTkReclamationAnswer.User;
                newAnswer.DateCreate = DateTime.Now;
                newAnswer.Completed = false;
                db.OTK_ReclamationAnswer.Add(newAnswer);
                db.SaveChanges();
                PushEmailReload(newAnswer, login);
            }
            oTkReclamation.AnswerUser = oTkReclamationAnswer.UserUpdate;
            db.Entry(oTkReclamation).State = EntityState.Modified;
            db.SaveChanges();
            db.Entry(oTkReclamationAnswer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "ReclamationOTK");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad, KBMUser, KBEUser")]
        public ActionResult EditUserKOPartial(int id, string login)
        {
            OTK_ReclamationAnswer oTK_ReclamationAnswer = db.OTK_ReclamationAnswer.Find(id);
            var user = db.AspNetUsers.Where(d => d.Email == login).First().Devision;

            ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Email == login), "Id", "CiliricalName", oTK_ReclamationAnswer.userError);

            if (user == 3 || user == 16)
            {
                ViewBag.reloadError = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 3 && d.id != 16).OrderBy(d => d.name), "id", "name");
            }
            else
            {
                ViewBag.reloadError = new SelectList(db.Devision.Where(d => d.OTK == true && d.id != 15).OrderBy(d => d.name), "id", "name");
            }
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationAnswer.countError);
            ViewBag.ReclamationId = new SelectList(db.OTK_Reclamation, "Id", "ReclamationText", oTK_ReclamationAnswer.ReclamationId);
            ViewBag.NumberOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.NameOrder = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.Name.ToString();
            ViewBag.TextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();
            ViewBag.Textid = oTK_ReclamationAnswer.OTK_Reclamation.Id.ToString();
            ViewBag.TextdateCreate = oTK_ReclamationAnswer.OTK_Reclamation.DateTimeCreate.ToString();
            ViewBag.TextPlanZakaz = oTK_ReclamationAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.TexttextReclamation = oTK_ReclamationAnswer.OTK_Reclamation.ReclamationText.ToString();
            ViewBag.TextCiliricalName = oTK_ReclamationAnswer.OTK_Reclamation.AspNetUsers.CiliricalName.ToString();

            return View(oTK_ReclamationAnswer);
        }
        [Authorize(Roles = "KBE, Admin, KBM, OS, Manufacturing, Sklad, KBMUser, KBEUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserKOPartial(OTK_ReclamationAnswer oTkReclamationAnswer, string reloadError, bool newDevision)
        {
            string login = HttpContext.User.Identity.Name;
            login = db.AspNetUsers.First(d => d.Email == login).Id;
            string recipient = db.AspNetUsers.First(d => d.Email == login).Email;
            oTkReclamationAnswer.UserUpdate = login;
            oTkReclamationAnswer.Completed = true;
            Devision devision = db.Devision.Find(oTkReclamationAnswer.Devision);
            oTkReclamationAnswer.DateUpdate = DateTime.Now;
            if (oTkReclamationAnswer.Text == null)
                oTkReclamationAnswer.Text = "";
            OTK_Reclamation oTkReclamation = db.OTK_Reclamation.Find(oTkReclamationAnswer.ReclamationId);
            oTkReclamation.AnswerDate = DateTime.Now;
            string devisionName = db.Devision.Find(oTkReclamationAnswer.Devision)?.name;
            if (newDevision == false)
            {
                if (devision != null) oTkReclamation.Devision = devision.id;
                oTkReclamation.AnswerText += "\n" + devisionName + ": " + oTkReclamationAnswer.Text;
            }
            else
            {
                if (oTkReclamationAnswer.Text == null || oTkReclamationAnswer.Text == "")
                    oTkReclamationAnswer.Text = "замечание перенаправлено";
                oTkReclamationAnswer.countError = 4;
                oTkReclamation.Devision = Convert.ToInt32(reloadError);
                oTkReclamation.AnswerText += "\n" + devisionName + ": " + " замечание перенаправлено";
                OTK_ReclamationAnswer newAnswer = new OTK_ReclamationAnswer();
                newAnswer.ReclamationId = oTkReclamationAnswer.ReclamationId;
                newAnswer.Text = "";
                newAnswer.Devision = Convert.ToInt32(reloadError);
                newAnswer.User = oTkReclamationAnswer.User;
                newAnswer.DateCreate = DateTime.Now;
                newAnswer.Completed = false;
                db.OTK_ReclamationAnswer.Add(newAnswer);
                db.SaveChanges();
            }
            oTkReclamation.AnswerUser = oTkReclamationAnswer.UserUpdate;
            db.Entry(oTkReclamation).State = EntityState.Modified;
            db.SaveChanges();
            db.Entry(oTkReclamationAnswer).State = EntityState.Modified;
            db.SaveChanges();
            PushEmail(oTkReclamationAnswer.id, login, recipient);
            return RedirectToAction("Index", "ReclamationOTK");
        }

        public void PushEmail(int reclamationId, string userError, string recipient)
        {
            OTK_ReclamationAnswer oTK_Reclamation = db.OTK_ReclamationAnswer.First(d => d.id == reclamationId);
            string pz = db.PZ_PlanZakaz.Find(oTK_Reclamation.OTK_Reclamation.OTK_ChaeckList.Order).PlanZakaz.ToString();
            string us = db.AspNetUsers.Find(oTK_Reclamation.OTK_Reclamation.AspNetUsers.Id).CiliricalName;
            string usError = db.AspNetUsers.Where(d => d.Email == userError).First().CiliricalName;
            string usErrorDevision = db.AspNetUsers.Where(d => d.Email == userError).First().Devision1.name;
            List<string> listPostMail = new List<string>();
            string subject = "Ответ на рекламацию ЛЭФИ";
            string body = "<html>" + "" + "" + "" +
                "" + "<head><style type=text/css>TABLE {width: 700px; border-collapse: collapse; }TD, TH, TR {padding: 0; border: 1px solid black;}TH {background: #b0e0e6;}</style></head>" +
                "<body><table>" + 
                "<tr>" + "<td> № заказа </td>" + "<td>" + pz + "</td>" + "<tr/>" +
                "<tr>" + "<td> № рекламации </td>" + "<td>" + oTK_Reclamation.OTK_Reclamation.Id + "</td>" + "<tr/>" +
                "<tr>" +"<td>Дата и время замечания</td>" + "<td>" + oTK_Reclamation.OTK_Reclamation.DateTimeCreate.ToString() + "</td>" + "<tr/>" + 
                "<tr>" + "<td>Кто создал</td>" + "<td>" + us + "</td>" + "<tr/>" + 
                "<tr>" + "<td> СП </td>" + "<td>" + usErrorDevision + "</td>" + "<tr/>" + 
                "<tr>" + "<td>Ответственный</td>" + "<td>" + usError + "</td>" + "<tr/>" +
                "<tr>" + "<td>Описание</td>" + "<td>" + oTK_Reclamation.OTK_Reclamation.ReclamationText.ToString() + "</td>" + "<tr/>" + 
                "<tr>" + "<td>Ответ</td>" + "<td>" + oTK_Reclamation.Text.ToString() + "</td>" + "<tr/>";
            try
            {
                body += "<tr>" + "<td>Комментарий руководителя КБ</td>" + "<td>" + oTK_Reclamation.descriptionManagerKO.ToString() + "</td>" + "<tr/>";
            }
            catch
            {
                
            }
            body += "</table></html></body>";
            if (db.AspNetUsers.Where(d => d.Email == userError).First().Devision == 15)
            {
                var devisionListKBM = db.AspNetUsers.Where(d => d.Devision == 15).ToList().Where(d => d.LockoutEnabled == true).ToList();
                foreach (var data in devisionListKBM)
                {
                    listPostMail.Add(data.Email);
                }
            }
            else
            {
                var devisionListKBE = db.AspNetUsers.Where(d => d.Devision == 16 || d.Devision == 3).ToList().Where(d => d.LockoutEnabled == true).ToList();
                foreach (var data in devisionListKBE)
                {
                    listPostMail.Add(data.Email);
                }
            }
            listPostMail.Add("myi@katek.by");
            EmailModel emailModel = new EmailModel();
            emailModel.SendEmailListToHTML(listPostMail, subject, body, recipient);
        }

        public void PushEmailReload(OTK_ReclamationAnswer newAnswer, string login)
        {
            List<string> listPostMail = new List<string>();
            string subject = "Перенаправлена рекламация ЛЭФИ";
            string body = "<html>" + "" + "" + "" +
                "" + "<head><style type=text/css>TABLE {width: 700px; border-collapse: collapse; }TD, TH, TR {padding: 0; border: 1px solid black;}TH {background: #b0e0e6;}</style></head>" +
                "<body><table>" +
                "<tr>" + "<td> № заказа </td>" + "<td>" + newAnswer.OTK_Reclamation.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString() + "</td>" + "<tr/>" +
                "<tr>" + "<td> № рекламации </td>" + "<td>" + newAnswer.OTK_Reclamation.Id + "</td>" + "<tr/>" +
                "<tr>" + "<td>Дата и время замечания</td>" + "<td>" + newAnswer.OTK_Reclamation.DateTimeCreate.ToString() + "</td>" + "<tr/>" +
                "<tr>" + "<td>Кто создал</td>" + "<td>" + db.AspNetUsers.Find(newAnswer.OTK_Reclamation.AspNetUsers.Id).CiliricalName + "</td>" + "<tr/>" +
                "<tr>" + "<td>Описание</td>" + "<td>" + newAnswer.OTK_Reclamation.ReclamationText.ToString() + "</td>" + "<tr/>" +
                "<tr>" + "<td>Прим.</td>" + "<td>" + newAnswer.OTK_Reclamation.Description.ToString() + "</td>" + "<tr/>" +
                "<tr>" + "<td>Ответ</td>" + "<td>" + newAnswer.Text.ToString() + "</td>" + "<tr/>";
            body += "</table></html></body>";
            if(newAnswer.Devision == 16)
            {
                foreach (var data in db.AspNetUsers.Where(d => d.Devision == 3).Where(d => d.LockoutEnabled == true).ToList())
                {
                    listPostMail.Add(data.Email);
                }
            }
            foreach (var data in db.AspNetUsers.Where(d => d.Devision == newAnswer.Devision).Where(d => d.LockoutEnabled == true).ToList())
            {
                listPostMail.Add(data.Email);
            }
            listPostMail.Add("myi@katek.by");
            EmailModel emailModel = new EmailModel();
            emailModel.SendEmailListToHTML(listPostMail, subject, body, login);
        }
    }
}
