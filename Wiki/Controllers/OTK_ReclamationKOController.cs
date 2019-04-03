using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class OTK_ReclamationKOController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private PortalKATEKEntities db = new PortalKATEKEntities();

        [Authorize(Roles = "Admin, KBM, KBE, KBMUser, KBEUser")]
        public ActionResult IndexMy()
        {
            string login = HttpContext.User.Identity.Name;
            var user = db.AspNetUsers.Where(d => d.Email == login).First().Devision;
            if (user == 3 || user == 16)
            {
                var oTK_ReclamationKO = db.OTK_ReclamationKO
                    .Where(d => d.AspNetUsers1.Devision == 3 || d.AspNetUsers1.Devision == 16).ToList();
                return View(oTK_ReclamationKO);
            }
            else
            {
                var oTK_ReclamationKO = db.OTK_ReclamationKO
                    .Where(d => d.AspNetUsers1.Devision == 15).ToList();
                return View(oTK_ReclamationKO);
            }
        }
        
        [Authorize(Roles = "Admin, KBM, KBE, Manufacturing")]
        public ActionResult Create()
        {
            string login = HttpContext.User.Identity.Name;
            var user = db.AspNetUsers.Where(d => d.Email == login).First().Devision;
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name");
            ViewBag.typeReclamation = new SelectList(db.OTK_TypeReclamation.Where(d => d.KO == true), "id", "Name");
            ViewBag.order = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now), "Id", "PlanZakaz");
            if (user == 3 || user == 16)
            {
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 16).OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                return View();
            }
            else if (user == 15)
            {
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 15).OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                return View();
            }
            else
            {
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Id == "4f91324a-1918-4e62-b664-d8cd89a19d95" || d.Id == "8363828f-bba2-4a89-8ed8-d7f5623b4fa8").OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                return RedirectToAction("CreatePO");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OTK_ReclamationKO oTK_ReclamationKO)
        {
            if (ModelState.IsValid)
            {
                string login = HttpContext.User.Identity.Name;
                var user = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                if (oTK_ReclamationKO.textReclamation == null)
                    oTK_ReclamationKO.textReclamation = "";
                if (oTK_ReclamationKO.time == null)
                    oTK_ReclamationKO.time = 0;
                oTK_ReclamationKO.textAnswer = "";
                oTK_ReclamationKO.dateUpdate = DateTime.Now;
                oTK_ReclamationKO.userUpdate = "";
                oTK_ReclamationKO.countError = 1;
                oTK_ReclamationKO.userCreate = user;
                oTK_ReclamationKO.aplayResylt = false;
                oTK_ReclamationKO.clouseReclamation = false;
                oTK_ReclamationKO.dateCreate = DateTime.Now;
                db.OTK_ReclamationKO.Add(oTK_ReclamationKO);
                db.SaveChanges();

                PushEmail(oTK_ReclamationKO.id);
                return Redirect("/ReclamationPO/Index");
            }

            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userCreate = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userCreate);
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationKO.countError);
            ViewBag.typeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_ReclamationKO.typeReclamation);
            ViewBag.order = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", oTK_ReclamationKO.order);
            return View(oTK_ReclamationKO);
        }

        public void PushEmail(int reclamationKOId)
        {
            OTK_ReclamationKO oTK_ReclamationKO = db.OTK_ReclamationKO.Include(d => d.OTK_CounterErrorKO).Where(d => d.id == reclamationKOId).First();
            string pz = db.PZ_PlanZakaz.Find(oTK_ReclamationKO.order).PlanZakaz.ToString();
            string us = db.AspNetUsers.Find(oTK_ReclamationKO.userCreate).CiliricalName;
            string usError = db.AspNetUsers.Find(oTK_ReclamationKO.userError).CiliricalName;
            string recipient = db.AspNetUsers.Find(oTK_ReclamationKO.userError).Email;
            string usErrorDevision = db.AspNetUsers.Find(oTK_ReclamationKO.userError).Devision1.name;
            List<string> listPostMail = new List<string>();
            string subject = "Замечание производственного отдела";
            string body = "<html>" +
                "" +
                "" +
                "" +
                "" +
  "<head><style type=text/css>TABLE {width: 700px; border-collapse: collapse; }TD, TH, TR {padding: 0; border: 1px solid black;}TH {background: #b0e0e6;}</style></head>" +

                "<body><table>" +
              "<tr>" +
                    "<td>№ заказа</td>" + "<td>" + oTK_ReclamationKO.PZ_PlanZakaz.PlanZakaz.ToString() + "</td>" +
              "<tr/>" +
                            "<tr>" +
                    "<td>№ рекламации</td>" + "<td>" + oTK_ReclamationKO.id.ToString() + "</td>" +
              "<tr/>" +
              "<tr>" +
                    "<td>Дата и время замечания</td>" + "<td>" + oTK_ReclamationKO.dateCreate.ToString() + "</td>" +
              "<tr/>" +
              "<tr>" +
                    "<td>Кто создал</td>" + "<td>" + us + "</td>" +
              "<tr/>" +
              "<tr>" +
                    "<td>СП</td>" + "<td>" + usErrorDevision + "</td>" +
              "<tr/>" +
              "<tr>" +
                    "<td>Ответственный</td>" + "<td>" + usError + "</td>" +
              "<tr/>" +
              "<tr>" +
                    "<td>Описание</td>" + "<td>" + oTK_ReclamationKO.textReclamation.ToString() + "</td>" +
              "<tr/>" +
              "<tr>" +
                    "<td>Ответ</td>" + "<td>" + oTK_ReclamationKO.textAnswer.ToString() + "</td>" +
              "<tr/>";
            try
            {
                body += "<tr>" +
            "<td>Комментарий руководителя КБ</td>" + "<td>" + oTK_ReclamationKO.descriptionManagerKO.ToString() + "</td>" +
      "<tr/>";
            }
            catch { }
         body += "</table></html></body>";


            var devisionListKBM = db.AspNetUsers.Where(d => d.Devision == 15).ToList().Where(d => d.LockoutEnabled == true).ToList();
            var devisionListKBE = db.AspNetUsers.Where(d => d.Devision == 16 || d.Devision == 3).ToList().Where(d => d.LockoutEnabled == true).ToList();


            if (oTK_ReclamationKO.AspNetUsers.Devision == 15)
            {
                foreach (var data in devisionListKBM)
                {
                    listPostMail.Add(data.Email);
                }
            }
            else
            {
                foreach (var data in devisionListKBE)
                {
                    listPostMail.Add(data.Email);
                }
            }
            try
            {
                listPostMail.Add(oTK_ReclamationKO.AspNetUsers3.Email);

            }
            catch { }
            listPostMail.Add("myi@katek.by");
            listPostMail.Add(db.AspNetUsers.Find(oTK_ReclamationKO.userCreate).Email);

            EmailModel emailModel = new EmailModel();
            emailModel.SendEmailListToHTML(listPostMail, subject, body, recipient);
        }

        [Authorize(Roles = "Admin, Manufacturing")]
        public ActionResult CreatePO()
        {
            ViewBag.typeReclamation = new SelectList(db.OTK_TypeReclamation.Where(d => d.KO == true), "id", "Name");
            ViewBag.order = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Id == "4f91324a-1918-4e62-b664-d8cd89a19d95" || d.Id == "8363828f-bba2-4a89-8ed8-d7f5623b4fa8").OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePO(OTK_ReclamationKO oTK_ReclamationKO)
        {
            if (ModelState.IsValid)
            {
                string login = HttpContext.User.Identity.Name;
                var user = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                var emailRec = db.AspNetUsers.Where(d => d.Email == login).First().Email;
                if (oTK_ReclamationKO.textReclamation == null)
                    oTK_ReclamationKO.textReclamation = "";
                if (oTK_ReclamationKO.time == null)
                    oTK_ReclamationKO.time = 0;
                oTK_ReclamationKO.textAnswer = "";
                oTK_ReclamationKO.dateUpdate = DateTime.Now;
                oTK_ReclamationKO.userUpdate = "";
                oTK_ReclamationKO.countError = 1;
                oTK_ReclamationKO.descriptionManagerKO = "";
                oTK_ReclamationKO.userCreate = user;
                oTK_ReclamationKO.clouseReclamation = false;
                oTK_ReclamationKO.dateCreate = DateTime.Now;
                db.OTK_ReclamationKO.Add(oTK_ReclamationKO);
                db.SaveChanges();
                PushEmail(oTK_ReclamationKO.id, emailRec, false);
                try
                {
                    TaskForPWA pwa = new TaskForPWA();
                    string reclamationNumAndText = oTK_ReclamationKO.id.ToString() + " " + oTK_ReclamationKO.textReclamation;
                    var projectUid = db.PZ_PlanZakaz.Find(oTK_ReclamationKO.order).ProjectUID;
                    if (projectUid != null)
                        pwa.CreateTaskOTK_PO((Guid)projectUid, reclamationNumAndText, login);
                }
                catch (Exception ex)
                {
                    logger.Error("Reclamation PO - " + ex.Message);
                }
                return Redirect("/ReclamationPO/Index");
            }
            return View(oTK_ReclamationKO);
        }
        
        [Authorize(Roles = "Admin, KBM, KBE, KBMUser, KBEUser")]
        public ActionResult Edit(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            var aspUser = db.AspNetUsers.Where(d => d.Email == login).First();
            var user = aspUser.Devision;
            OTK_ReclamationKO oTK_ReclamationKO = db.OTK_ReclamationKO.Find(id);
            if (login == "fvs@katek.by")
            {
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 16 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationKO.userError);
            }
            else if (login == "myi@katek.by")
            {
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 18 || d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationKO.userError);
            }
            else if (login == "nrf@katek.by")
            {
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 18 || d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationKO.userError);
            }
            else if (login == "Kuchynski@katek.by")
            {
                ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 16 || d.Devision == 15 || d.Email == "melnikauyi@gmail.com" || d.Email == "katekproject@gmail.com").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationKO.userError);
            }
            else if (user == 3 || user == 16)
            {
                return RedirectToAction("EditUserKO", "OTK_ReclamationKO", new { id, login });
            }
            else if (user == 15)
            {
                return RedirectToAction("EditUserKO", "OTK_ReclamationKO", new { id, login });
            }
            else if (user == 8 || user == 9 || user == 10 || user == 20 || user == 22)
            {
                //ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Id == "4f91324a-1918-4e62-b664-d8cd89a19d95" || d.Id == "8363828f-bba2-4a89-8ed8-d7f5623b4fa8").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationKO.userError);
                return Redirect("/OTK_ReclamationKO/EditPO" + "/" + id);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (oTK_ReclamationKO == null)
            {
                return HttpNotFound();
            }
            @ViewBag.Textid = oTK_ReclamationKO.id.ToString();
            @ViewBag.TextdateCreate = oTK_ReclamationKO.dateCreate.ToString();
            @ViewBag.TextPlanZakaz = oTK_ReclamationKO.PZ_PlanZakaz.PlanZakaz.ToString();
            @ViewBag.TexttextReclamation = oTK_ReclamationKO.textReclamation.ToString();
            @ViewBag.TextCiliricalName = oTK_ReclamationKO.AspNetUsers3.CiliricalName.ToString();
            ViewBag.userCreate = new SelectList(db.AspNetUsers, "Id", "CiliricalName", oTK_ReclamationKO.userCreate);
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationKO.countError);
            ViewBag.typeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_ReclamationKO.typeReclamation);
            ViewBag.order = new SelectList(db.PZ_PlanZakaz, "Id", "PlanZakaz", oTK_ReclamationKO.order);
            return View(oTK_ReclamationKO);
        }

        [Authorize(Roles = "Admin, KBM, KBE, KBMUser, KBEUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OTK_ReclamationKO oTK_ReclamationKO)
        {
            string login = HttpContext.User.Identity.Name;
            var emailRec = db.AspNetUsers.Where(d => d.Email == login).First().Email;
            if (ModelState.IsValid)
            {
                if (oTK_ReclamationKO.descriptionManagerKO == "")
                    oTK_ReclamationKO.descriptionManagerKO = "-";
                if (oTK_ReclamationKO.descriptionManagerKO == null)
                    oTK_ReclamationKO.descriptionManagerKO = "-";
                if (oTK_ReclamationKO.textAnswer == null)
                    oTK_ReclamationKO.textAnswer = "";
                var user = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                oTK_ReclamationKO.userUpdate = user;
                oTK_ReclamationKO.dateUpdate = DateTime.Now;
                db.Entry(oTK_ReclamationKO).State = EntityState.Modified;
                db.SaveChanges();
                if (oTK_ReclamationKO.descriptionManagerKO != "-")
                    PushEmail(oTK_ReclamationKO.id, emailRec, true);
                return Redirect("/ReclamationPO/Index");
            }
            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userCreate = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userCreate);
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationKO.countError);
            ViewBag.typeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_ReclamationKO.typeReclamation);
            ViewBag.order = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", oTK_ReclamationKO.order);
            return View(oTK_ReclamationKO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Error()
        {
            return View();
        }
        
        public ActionResult RepotrQuart()
        {
            //set user error today quart
            var oTK_ReclamationKOToday = db.OTK_ReclamationKO
                .Include(o => o.OTK_CounterErrorKO)
                .Include(o => o.OTK_TypeReclamation)
                .Include(o => o.PZ_PlanZakaz)
                .Where(o => (o.dateCreate.Month + 2) / 3 == (DateTime.Now.Month + 2) / 3 &
                    o.dateCreate.Year == DateTime.Now.Year)
                .OrderBy(o => o.dateCreate);
            //set list error
            List<OTKReclamationKoReportQuart> quartHuman = new List<OTKReclamationKoReportQuart>();
            double countUserError = 0;
            foreach (var t in db.AspNetUsers)
            {
                if (t.Devision1.name.ToString().Substring(0, 2) == "КБ")
                {
                    foreach (var x in oTK_ReclamationKOToday)
                    {

                        if (t.Id == x.userError & (x.dateCreate.Month + 2) / 3 == (DateTime.Now.Month + 2) / 3 & x.dateCreate.Year == DateTime.Now.Year)
                        {
                            countUserError += x.OTK_CounterErrorKO.count;
                        }
                    }
                    if (countUserError > 0)
                        quartHuman.Add(new OTKReclamationKoReportQuart(t.CiliricalName, countUserError, t.Devision1.name));
                    countUserError = 0;
                }
            }
            var myQH = quartHuman.OrderBy(x => x.countError).OrderBy(t => t.userName).OrderBy(y => y.departament);
            ViewBag.quartHuman = myQH;
            return View(oTK_ReclamationKOToday.ToList());
        }
        
        [Authorize(Roles = "Admin, KBM, KBE, KBMUser, KBEUser")]
        public ActionResult EditPO(int? id)
        {
            OTK_ReclamationKO oTK_ReclamationKO = db.OTK_ReclamationKO.Find(id);

            ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Id == "4f91324a-1918-4e62-b664-d8cd89a19d95" || d.Id == "8363828f-bba2-4a89-8ed8-d7f5623b4fa8").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationKO.userError);


            @ViewBag.Textid = oTK_ReclamationKO.id.ToString();
            @ViewBag.TextdateCreate = oTK_ReclamationKO.dateCreate.ToString();
            @ViewBag.TextPlanZakaz = oTK_ReclamationKO.PZ_PlanZakaz.PlanZakaz.ToString();
            @ViewBag.TexttextReclamation = oTK_ReclamationKO.textReclamation.ToString();
            @ViewBag.TextCiliricalName = oTK_ReclamationKO.AspNetUsers3.CiliricalName.ToString();
            ViewBag.userCreate = new SelectList(db.AspNetUsers, "Id", "CiliricalName", oTK_ReclamationKO.userCreate);
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationKO.countError);
            ViewBag.typeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_ReclamationKO.typeReclamation);
            ViewBag.order = new SelectList(db.PZ_PlanZakaz, "Id", "PlanZakaz", oTK_ReclamationKO.order);
            return View(oTK_ReclamationKO);
        }

        [Authorize(Roles = "Admin, KBM, KBE, KBMUser, KBEUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPO(OTK_ReclamationKO oTK_ReclamationKO)
        {
            if (ModelState.IsValid)
            {
                if (oTK_ReclamationKO.textAnswer == null)
                    oTK_ReclamationKO.textAnswer = "";
                string login = HttpContext.User.Identity.Name;
                var user = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                oTK_ReclamationKO.userUpdate = user;
                oTK_ReclamationKO.dateUpdate = DateTime.Now;
                db.Entry(oTK_ReclamationKO).State = EntityState.Modified;
                db.SaveChanges();
                PushEmail(oTK_ReclamationKO.id);
                return Redirect("/ReclamationPO/Index");
            }
            ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Id == "4f91324a-1918-4e62-b664-d8cd89a19d95" || d.Id == "8363828f-bba2-4a89-8ed8-d7f5623b4fa8").OrderBy(d => d.CiliricalName), "Id", "CiliricalName", oTK_ReclamationKO.userError);
            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userCreate = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userCreate);
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationKO.countError);
            ViewBag.typeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_ReclamationKO.typeReclamation);
            ViewBag.order = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", oTK_ReclamationKO.order);
            return View(oTK_ReclamationKO);
        }

        [Authorize(Roles = "Admin, KBM, KBE, KBMUser, KBEUser")]
        public ActionResult EditUserKO(int id, string login)
        {
            var aspUser = db.AspNetUsers.Where(d => d.Email == login).First();
            var user = aspUser.Devision;
            OTK_ReclamationKO oTK_ReclamationKO = db.OTK_ReclamationKO.Find(id);
            ViewBag.userError = new SelectList(db.AspNetUsers.Where(d => d.Email == login), "Id", "CiliricalName", oTK_ReclamationKO.userError);
            @ViewBag.Textid = oTK_ReclamationKO.id.ToString();
            @ViewBag.TextdateCreate = oTK_ReclamationKO.dateCreate.ToString();
            @ViewBag.TextPlanZakaz = oTK_ReclamationKO.PZ_PlanZakaz.PlanZakaz.ToString();
            @ViewBag.TexttextReclamation = oTK_ReclamationKO.textReclamation.ToString();
            @ViewBag.TextCiliricalName = oTK_ReclamationKO.AspNetUsers3.CiliricalName.ToString();
            ViewBag.userCreate = new SelectList(db.AspNetUsers, "Id", "CiliricalName", oTK_ReclamationKO.userCreate);
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationKO.countError);
            ViewBag.typeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_ReclamationKO.typeReclamation);
            ViewBag.order = new SelectList(db.PZ_PlanZakaz, "Id", "PlanZakaz", oTK_ReclamationKO.order);
            return View(oTK_ReclamationKO);
        }
        [Authorize(Roles = "Admin, KBM, KBE, KBMUser, KBEUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserKO(OTK_ReclamationKO oTK_ReclamationKO)
        {
            if (ModelState.IsValid)
            {
                if (oTK_ReclamationKO.textAnswer == null)
                    oTK_ReclamationKO.textAnswer = "";
                string login = HttpContext.User.Identity.Name;
                var user = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                oTK_ReclamationKO.userUpdate = user;
                oTK_ReclamationKO.dateUpdate = DateTime.Now;
                db.Entry(oTK_ReclamationKO).State = EntityState.Modified;
                db.SaveChanges();
                PushEmail(oTK_ReclamationKO.id);
                return Redirect("/ReclamationPO/Index");
            }
            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userError = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userError);
            ViewBag.userCreate = new SelectList(db.AspNetUsers, "Id", "Email", oTK_ReclamationKO.userCreate);
            ViewBag.countError = new SelectList(db.OTK_CounterErrorKO, "id", "name", oTK_ReclamationKO.countError);
            ViewBag.typeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_ReclamationKO.typeReclamation);
            ViewBag.order = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", oTK_ReclamationKO.order);
            return View(oTK_ReclamationKO);
        }

        public void PushEmail(int reclamationKOId, string rec, bool postUserCreate)
        {
            OTK_ReclamationKO oTK_ReclamationKO = db.OTK_ReclamationKO.Include(d => d.OTK_CounterErrorKO).Where(d => d.id == reclamationKOId).First();
            string pz = db.PZ_PlanZakaz.Find(oTK_ReclamationKO.order).PlanZakaz.ToString();
            string us = db.AspNetUsers.Find(oTK_ReclamationKO.userCreate).CiliricalName;
            string usError = db.AspNetUsers.Find(oTK_ReclamationKO.userError).CiliricalName;
            string usErrorDevision = db.AspNetUsers.Find(oTK_ReclamationKO.userError).Devision1.name;
            List<string> listPostMail = new List<string>();
            string subject = "Замечание производственного отдела";
            string body = "<html>" + "" + "" + "" + "" + "<head><style type=text/css>TABLE {width: 700px; border-collapse: collapse; }TD, TH, TR {padding: 0; border: 1px solid black;}TH {background: #b0e0e6;}</style></head>" + "<body><table>" +
              "<tr>" + "<td> № заказа </td>" + "<td>" + oTK_ReclamationKO.PZ_PlanZakaz.PlanZakaz.ToString() + "</td>" + "<tr/>" +
              "<tr>" + "<td> № рекламации </td>" + "<td>" + oTK_ReclamationKO.id + "</td>" + "<tr/>" +
              "<tr>" + "<td>Дата и время замечания</td>" + "<td>" + oTK_ReclamationKO.dateCreate.ToString() + "</td>" + "<tr/>" +
              "<tr>" + "<td>Кто создал</td>" + "<td>" + us + "</td>" + "<tr/>" +
              "<tr>" + "<td>СП</td>" + "<td>" + usErrorDevision + "</td>" + "<tr/>" +
              "<tr>" + "<td>Ответственный</td>" + "<td>" + usError + "</td>" + "<tr/>" +
              "<tr>" + "<td>Описание</td>" + "<td>" + oTK_ReclamationKO.textReclamation.ToString() + "</td>" + "<tr/>" +
              "<tr>" + "<td>Ответ</td>" + "<td>" + oTK_ReclamationKO.textAnswer.ToString() + "</td>" + "<tr/>";
            try
            {
                body += "<tr>" + "<td>Комментарий руководителя КБ</td>" + "<td>" + oTK_ReclamationKO.descriptionManagerKO.ToString() + "</td>" + "<tr/>";
            }
            catch { }
            body += "</table></html></body>";
            var devisionListKBM = db.AspNetUsers.Where(d => d.Devision == 15).ToList().Where(d => d.LockoutEnabled == true).ToList();
            var devisionListKBE = db.AspNetUsers.Where(d => d.Devision == 16 || d.Devision == 3).ToList().Where(d => d.LockoutEnabled == true).ToList();
            if (oTK_ReclamationKO.AspNetUsers.Devision == 15)
            {
                foreach (var data in devisionListKBM)
                {
                    listPostMail.Add(data.Email);
                }
            }
            else
            {
                foreach (var data in devisionListKBE)
                {
                    listPostMail.Add(data.Email);
                }
            }
            listPostMail.Add("myi@katek.by");
            if(postUserCreate == true)
                listPostMail.Add(db.AspNetUsers.Find(oTK_ReclamationKO.userCreate).Email);
            EmailModel emailModel = new EmailModel();
            emailModel.SendEmailListToHTML(listPostMail, subject, body, rec);
        }
    }
}
