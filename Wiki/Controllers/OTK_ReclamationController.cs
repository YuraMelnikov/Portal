using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class OTK_ReclamationController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();
        OTK_Reclamation empDB = new OTK_Reclamation();
        
        public ActionResult Index()
        {
            var oTK_Reclamation = db.OTK_Reclamation.Include(o => o.OTK_ChaeckList).Include(o => o.OTK_TypeReclamation).Include(o => o.AspNetUsers);
            return View(oTK_Reclamation.ToList());
        }

        public ActionResult ComplitedAllTask(int? idf)
        {
            foreach (var data in db.OTK_Reclamation.Where(d => d.CheckList == idf).ToList())
            {
                data.Complited = true;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("IndexCheckList", new { id = idf });
        }

        public ActionResult IndexCheckList(int? id)
        {
            var oTK_Reclamation = db.OTK_Reclamation
                .Include(o => o.OTK_ChaeckList)
                .Include(o => o.OTK_TypeReclamation)
                .Include(o => o.AspNetUsers)
                .Where(o => o.CheckList == id)
                .OrderBy(o => o.Complited)
                .ToList();
            OTK_ChaeckList oTK_ChaeckList = db.OTK_ChaeckList.Find(Convert.ToInt32(id));
            ViewBag.CheckList = oTK_ChaeckList;
            return View(oTK_Reclamation);
        }

        public ActionResult ListActiveWork()
        {
            var oTK_Reclamation = db.OTK_Reclamation
                .Where(o => o.Complited == false)
                .Where(o => o.OTK_ChaeckList.PZ_PlanZakaz.dataOtgruzkiBP > DateTime.Now)
                .OrderBy(o => o.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz)
                .ToList();
            return View(oTK_Reclamation);
        }

        public ActionResult ListDeactiveWork()
        {
            var oTK_Reclamation = db.OTK_Reclamation
                .Where(o => o.Complited == false)
                .Where(o => o.OTK_ChaeckList.PZ_PlanZakaz.dataOtgruzkiBP < DateTime.Now)
                .OrderBy(o => o.OTK_ChaeckList.PZ_PlanZakaz.PlanZakaz)
                .ToList();
            return View(oTK_Reclamation);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_Reclamation oTK_Reclamation = db.OTK_Reclamation.Find(id);
            if (oTK_Reclamation == null)
            {
                return HttpNotFound();
            }
            return View(oTK_Reclamation);
        }

        [Authorize(Roles = "Admin, OTK")]
        public ActionResult Create(int? idf)
        {
            OTK_ChaeckList oTK_ChaeckList = db.OTK_ChaeckList.Find(idf);
            ViewBag.CheckList = oTK_ChaeckList.id.ToString();
            ViewBag.CheckList = oTK_ChaeckList;
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name");
            ViewBag.User = new SelectList(db.AspNetUsers, "Id", "CiliricalName");
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name");

            return View();
        }

        [Authorize(Roles = "Admin, OTK")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OTK_Reclamation oTK_Reclamation, int? idf, OTK_ReclamationAnswer oTK_Answer, int Devision)
        {
            OTK_ChaeckList oTK_ChaeckList = db.OTK_ChaeckList.Find(Convert.ToInt32(idf));

            oTK_Reclamation.CheckList = oTK_ChaeckList.id;
            if (oTK_Reclamation.ReclamationText == null)
                oTK_Reclamation.ReclamationText = "";
            if (oTK_Reclamation.Description == null)
                oTK_Reclamation.Description = "";
            oTK_Reclamation.DateTimeCreate = DateTime.Now;
            var user = db.AspNetUsers;
            string login = HttpContext.User.Identity.Name;
            foreach (var x in user)
            {
                if (login == x.UserName.ToString())
                {
                    oTK_Reclamation.User = x.Id;
                    break;
                }
            }
            oTK_Reclamation.LaborCostManufacturing = 0;
            if (oTK_Reclamation.AnswerText == null)
                oTK_Reclamation.AnswerText = "";
            ViewBag.CheckList = oTK_ChaeckList;
            oTK_Reclamation.mailPost = false;
            db.OTK_Reclamation.Add(oTK_Reclamation);
            db.SaveChanges();
            
            oTK_Answer.ReclamationId = oTK_Reclamation.Id;
            oTK_Answer.Text = "";
            oTK_Answer.User = oTK_Reclamation.User;
            oTK_Answer.Devision =  (int)oTK_Reclamation.Devision;
            oTK_Answer.DateCreate = DateTime.Now;
            oTK_Answer.Completed = false;
            oTK_Answer.DateUpdate = DateTime.Now;
            db.OTK_ReclamationAnswer.Add(oTK_Answer);
            db.SaveChanges();
            return RedirectToAction("IndexCheckList", new { oTK_ChaeckList.id });
        }
        
        [Authorize(Roles = "Admin, OTK")]
        public ActionResult PostMail(int? idf)
        {
            string login = HttpContext.User.Identity.Name;
            EmailModel emailModel = new EmailModel();
            
            OTK_ChaeckList oTK_ChaeckList = db.OTK_ChaeckList.Find(idf);
            PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(oTK_ChaeckList.Order);
            var reclamationList = db.OTK_Reclamation
                .Where(d => d.CheckList == idf)
                .Where(d => d.Complited == false)
                //.Where(d => d.mailPost == false)
                .OrderBy(d => d.Devision)
                .ToList();
            
            List<int> devisOrder = new List<int>();
            foreach (var data in reclamationList)
            {
                if (Convert.ToInt32(data.Devision) == 7)
                {

                }
                else
                {
                    devisOrder.Add(Convert.ToInt32(data.Devision));
                    if (Convert.ToInt32(data.Devision) == 16)
                        devisOrder.Add(3);
                    if (Convert.ToInt32(data.Devision) == 3)
                        devisOrder.Add(16);
                }
            }

            List<int> distinct = devisOrder.Distinct().ToList();

            string textSubject = pZ_PlanZakaz.PlanZakaz.ToString() + " : подготовлен чек-лист";

            string textBody = "<html><head>" + "<style>" + "TABLE {width=700px;" + "border-collapse: collapse; " + "}"  + "TD, TH {" +
                              "padding: 0px; " + "border: 1px solid black; " + "}" + "}" + "</style>" + "</head><body>";
            
            foreach (var devision in distinct)
            {
                string bodyMess = textBody;
                if (reclamationList.Where(d => d.Devision == devision).Count() > 0)
                {
                    foreach (var reclamation in reclamationList.Where(d => d.Devision == devision))
                    {
                        string countError = "НД";
                        try
                        {
                            countError = reclamation.OTK_ReclamationAnswer.First().OTK_CounterErrorKO.name.ToString();
                        }
                        catch
                        {

                        }
                        string userError = "НД";
                        try
                        {
                            userError = reclamation.AspNetUsers.CiliricalName.ToString();
                        }
                        catch
                        {

                        }
                        bodyMess +=
                            "<table style=border: 1px>" + "<tbody>" +
                              "<tr>" +
                                    "<td>Дата замечания</td>" + "<td>" + reclamation.OTK_ReclamationAnswer.First().DateCreate.ToString() + "</td>" +
                              "<tr/>" +
                              "<tr>" +
                                    "<td>Кто создал</td>" + "<td>" + reclamation.AspNetUsers.CiliricalName.ToString() + "</td>" +
                              "<tr/>" +
                              "<tr>" +
                                    "<td>СП</td>" + "<td>" + reclamation.Devision1.name.ToString() + "</td>" +
                              "<tr/>" +
                              "<tr>" +
                                    "<td>Ответственный</td>" + "<td>" + userError + "</td>" +
                              "<tr/>" +
                              "<tr>" +
                                    "<td>Степень ошибки</td>" + "<td>" + countError + "</td>" +
                              "<tr/>" +
                              "<tr>" +
                                    "<td>Описание</td>" + "<td>" + reclamation.ReclamationText.ToString() + "</td>" +
                              "<tr/>" +
                              "<tr>" +
                                    "<td>Прим.</td>" + "<td>" + reclamation.Description.ToString() + "</td>" +
                              "<tr/>" +
                              "<tr>" +
                                    "<td>Ответ</td>" + "<td>" + reclamation.OTK_ReclamationAnswer.First().Text.ToString() + "</td>" +
                              "<tr/>" +
                              "</tbody>" +
                          "</table>";
                        bodyMess += "<br/>";
                    }
                    bodyMess += "</body></html>";
                    List<string> listMail = new List<string>();
                    foreach (var data in db.AspNetUsers.Where(d => d.Devision == devision))
                    {
                        if (data.LockoutEnabled == true)
                            listMail.Add(data.Email);
                    }
                    foreach (var data in db.AspNetUsers.Where(d => d.Devision == 6))
                    {
                        if (data.LockoutEnabled == true)
                            listMail.Add(data.Email);
                    }
                    listMail.Add("myi@katek.by");
                    listMail.Add("bav@katek.by");
                    listMail.Add("Antipov@katek.by");
                    emailModel.SendEmailListToHTML(listMail, textSubject, bodyMess, login);
                }
                
            }

            foreach (var VARIABLE in reclamationList)
            {
                VARIABLE.mailPost = true;
                db.Entry(VARIABLE).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return RedirectToAction("IndexCheckList", new { id = idf });
        }

        [Authorize(Roles = "Admin, OTK")]
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_Reclamation oTK_Reclamation = db.OTK_Reclamation.Find(Id);
            if (oTK_Reclamation == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_Reclamation.TypeReclamation);
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name", oTK_Reclamation.Devision);

            return View(oTK_Reclamation);
        }
        [Authorize(Roles = "Admin, OTK")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OTK_Reclamation oTK_Reclamation, OTK_ReclamationAnswer oTK_Answer)
        {
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers;
                string login = HttpContext.User.Identity.Name;
                foreach (var x in user)
                {
                    if (login == x.UserName.ToString())
                    {
                        oTK_Reclamation.User = x.Id;
                        break;
                    }
                }
                if (oTK_Reclamation.Description == null)
                    oTK_Reclamation.Description = "";
                if (oTK_Reclamation.ReclamationText == null)
                    oTK_Reclamation.ReclamationText = "";
                if (oTK_Reclamation.AnswerText == null)
                    oTK_Reclamation.AnswerText = "";
                oTK_Reclamation.LaborCostManufacturing = 0;
                db.Entry(oTK_Reclamation).State = EntityState.Modified;
                db.SaveChanges();

                var answerList = db.OTK_ReclamationAnswer.Where(d => d.ReclamationId == oTK_Reclamation.Id & d.Devision == oTK_Reclamation.Devision).ToList();

                if (answerList.Count == 0)
                {
                    OTK_ReclamationAnswer oTK_ReclamationAnswer = new OTK_ReclamationAnswer();
                    oTK_ReclamationAnswer.Completed = false;
                    oTK_ReclamationAnswer.countError = 1;
                    oTK_ReclamationAnswer.DateCreate = DateTime.Now;
                    oTK_ReclamationAnswer.DateUpdate = DateTime.Now;
                    oTK_ReclamationAnswer.Devision = Convert.ToInt32(oTK_Reclamation.Devision);
                    oTK_ReclamationAnswer.ReclamationId = Convert.ToInt32(oTK_Reclamation.Id);
                    oTK_ReclamationAnswer.Text = "";
                    oTK_ReclamationAnswer.User = oTK_Reclamation.User;
                    db.OTK_ReclamationAnswer.Add(oTK_ReclamationAnswer);
                    db.SaveChanges();
                }

                var answerDelList = db.OTK_ReclamationAnswer.Where(d => d.ReclamationId == oTK_Reclamation.Id & d.Devision != oTK_Reclamation.Devision & d.Text == "");
                try
                {
                    foreach (var data in answerDelList)
                    {
                        db.OTK_ReclamationAnswer.Remove(db.OTK_ReclamationAnswer.Find(data.id));
                        db.SaveChanges();
                    }
                }
                catch
                {

                }


                return RedirectToAction("IndexCheckList", new { id = oTK_Reclamation.CheckList });
            }
            ViewBag.CheckList = new SelectList(db.OTK_ChaeckList, "id", "UserCreate", oTK_Reclamation.CheckList);
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_Reclamation.TypeReclamation);
            ViewBag.User = new SelectList(db.AspNetUsers, "Id", "Email", oTK_Reclamation.User);
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name", oTK_Reclamation.Devision);
            return View(oTK_Reclamation);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_Reclamation oTK_Reclamation = db.OTK_Reclamation.Find(id);
            if (oTK_Reclamation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CheckList = new SelectList(db.OTK_ChaeckList, "id", "id", oTK_Reclamation.CheckList);
            return View(oTK_Reclamation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int remuveReclamation = 0;
            OTK_Reclamation oTK_Reclamation = db.OTK_Reclamation.Find(id);
            int idf = Convert.ToInt32(oTK_Reclamation.CheckList);
            remuveReclamation = oTK_Reclamation.Id;
            var listAnswer = db.OTK_ReclamationAnswer
                    .Where(d => d.ReclamationId == remuveReclamation)
                    .ToList();
            if (listAnswer != null)
            {
                foreach (var data in listAnswer)
                {
                    OTK_ReclamationAnswer oTK_ReclamationAnswer = db.OTK_ReclamationAnswer.Find(data.id);
                    db.OTK_ReclamationAnswer.Remove(oTK_ReclamationAnswer);
                    db.SaveChanges();
                }
            }
            db.OTK_Reclamation.Remove(oTK_Reclamation);
            db.SaveChanges();

            return RedirectToAction("IndexCheckList", new { id = idf });
        }

        [Authorize(Roles = "Admin, OTK")]
        public ActionResult EditPartial(int? Id)
        {
            OTK_Reclamation oTK_Reclamation = db.OTK_Reclamation.Find(Id);
            if (oTK_Reclamation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CL = oTK_Reclamation.CheckList;
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_Reclamation.TypeReclamation);
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name", oTK_Reclamation.Devision);
            return View(oTK_Reclamation);
        }
        [Authorize(Roles = "Admin, OTK")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartial(OTK_Reclamation oTK_Reclamation, OTK_ReclamationAnswer oTK_Answer)
        {
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers;
                string login = HttpContext.User.Identity.Name;
                foreach (var x in user)
                {
                    if (login == x.UserName.ToString())
                    {
                        oTK_Reclamation.User = x.Id;
                        break;
                    }
                }
                if (oTK_Reclamation.Description == null)
                    oTK_Reclamation.Description = "";
                if (oTK_Reclamation.ReclamationText == null)
                    oTK_Reclamation.ReclamationText = "";
                if (oTK_Reclamation.AnswerText == null)
                    oTK_Reclamation.AnswerText = "";
                oTK_Reclamation.LaborCostManufacturing = 0;
                db.Entry(oTK_Reclamation).State = EntityState.Modified;
                db.SaveChanges();
                var answerList = db.OTK_ReclamationAnswer.Where(d => d.ReclamationId == oTK_Reclamation.Id & d.Devision == oTK_Reclamation.Devision).ToList();
                if (answerList.Count == 0)
                {
                    OTK_ReclamationAnswer oTK_ReclamationAnswer = new OTK_ReclamationAnswer();
                    oTK_ReclamationAnswer.Completed = false;
                    oTK_ReclamationAnswer.countError = 1;
                    oTK_ReclamationAnswer.DateCreate = DateTime.Now;
                    oTK_ReclamationAnswer.DateUpdate = DateTime.Now;
                    oTK_ReclamationAnswer.Devision = Convert.ToInt32(oTK_Reclamation.Devision);
                    oTK_ReclamationAnswer.ReclamationId = Convert.ToInt32(oTK_Reclamation.Id);
                    oTK_ReclamationAnswer.Text = "";
                    oTK_ReclamationAnswer.User = oTK_Reclamation.User;
                    db.OTK_ReclamationAnswer.Add(oTK_ReclamationAnswer);
                    db.SaveChanges();
                }
                var answerDelList = db.OTK_ReclamationAnswer.Where(d => d.ReclamationId == oTK_Reclamation.Id & d.Devision != oTK_Reclamation.Devision & d.Text == "");
                try
                {
                    foreach (var data in answerDelList)
                    {
                        db.OTK_ReclamationAnswer.Remove(db.OTK_ReclamationAnswer.Find(data.id));
                        db.SaveChanges();
                    }
                }
                catch
                {

                }
                return RedirectToAction("IndexCheckList", new { id = oTK_Reclamation.CheckList });
            }
            ViewBag.CL = oTK_Reclamation.CheckList;
            ViewBag.CheckList = new SelectList(db.OTK_ChaeckList, "id", "UserCreate", oTK_Reclamation.CheckList);
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_Reclamation.TypeReclamation);
            ViewBag.User = new SelectList(db.AspNetUsers, "Id", "Email", oTK_Reclamation.User);
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name", oTK_Reclamation.Devision);
            return View(oTK_Reclamation);
        }

        [Authorize(Roles = "Admin, OTK")]
        public ActionResult CreatePartial(int? idf)
        {
            OTK_ChaeckList oTK_ChaeckList = db.OTK_ChaeckList.Find(idf);
            ViewBag.CheckList = oTK_ChaeckList.id.ToString();
            ViewBag.CheckList = oTK_ChaeckList;
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name");
            ViewBag.User = new SelectList(db.AspNetUsers, "Id", "CiliricalName");
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name");

            return View();
        }

        [Authorize(Roles = "Admin, OTK")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePartial(OTK_Reclamation oTK_Reclamation, int? idf, OTK_ReclamationAnswer oTK_Answer, int Devision)
        {
            OTK_ChaeckList oTK_ChaeckList = db.OTK_ChaeckList.Find(Convert.ToInt32(idf));

            oTK_Reclamation.CheckList = oTK_ChaeckList.id;
            if (oTK_Reclamation.ReclamationText == null)
                oTK_Reclamation.ReclamationText = "";
            if (oTK_Reclamation.Description == null)
                oTK_Reclamation.Description = "";
            oTK_Reclamation.DateTimeCreate = DateTime.Now;
            var user = db.AspNetUsers;
            string login = HttpContext.User.Identity.Name;
            foreach (var x in user)
            {
                if (login == x.UserName.ToString())
                {
                    oTK_Reclamation.User = x.Id;
                    break;
                }
            }
            oTK_Reclamation.AnswerDate = DateTime.Now;
            oTK_Reclamation.LaborCostManufacturing = 0;
            if (oTK_Reclamation.AnswerText == null)
                oTK_Reclamation.AnswerText = "";
            ViewBag.CheckList = oTK_ChaeckList;
            db.OTK_Reclamation.Add(oTK_Reclamation);
            db.SaveChanges();

            oTK_Answer.ReclamationId = oTK_Reclamation.Id;
            oTK_Answer.Text = "";
            oTK_Answer.User = oTK_Reclamation.User;
            oTK_Answer.Devision = (int)oTK_Reclamation.Devision;
            oTK_Answer.DateCreate = DateTime.Now;
            oTK_Answer.Completed = false;
            oTK_Answer.DateUpdate = DateTime.Now;
            db.OTK_ReclamationAnswer.Add(oTK_Answer);
            db.SaveChanges();
            return RedirectToAction("IndexCheckList", new { oTK_ChaeckList.id });
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        [Authorize(Roles = "Admin, OTK")]
        public ActionResult EditPartialActive(int? Id)
        {
            OTK_Reclamation oTK_Reclamation = db.OTK_Reclamation.Find(Id);
            if (oTK_Reclamation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CL = oTK_Reclamation.CheckList;
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_Reclamation.TypeReclamation);
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name", oTK_Reclamation.Devision);
            return View(oTK_Reclamation);
        }
        [Authorize(Roles = "Admin, OTK")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartialActive(OTK_Reclamation oTK_Reclamation, OTK_ReclamationAnswer oTK_Answer)
        {
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers;
                string login = HttpContext.User.Identity.Name;
                foreach (var x in user)
                {
                    if (login == x.UserName.ToString())
                    {
                        oTK_Reclamation.User = x.Id;
                        break;
                    }
                }
                if (oTK_Reclamation.Description == null)
                    oTK_Reclamation.Description = "";
                if (oTK_Reclamation.ReclamationText == null)
                    oTK_Reclamation.ReclamationText = "";
                if (oTK_Reclamation.AnswerText == null)
                    oTK_Reclamation.AnswerText = "";
                oTK_Reclamation.LaborCostManufacturing = 0;
                db.Entry(oTK_Reclamation).State = EntityState.Modified;
                db.SaveChanges();
                var answerList = db.OTK_ReclamationAnswer.Where(d => d.ReclamationId == oTK_Reclamation.Id & d.Devision == oTK_Reclamation.Devision).ToList();
                if (answerList.Count == 0)
                {
                    OTK_ReclamationAnswer oTK_ReclamationAnswer = new OTK_ReclamationAnswer();
                    oTK_ReclamationAnswer.Completed = false;
                    oTK_ReclamationAnswer.countError = 1;
                    oTK_ReclamationAnswer.DateCreate = DateTime.Now;
                    oTK_ReclamationAnswer.DateUpdate = DateTime.Now;
                    oTK_ReclamationAnswer.Devision = Convert.ToInt32(oTK_Reclamation.Devision);
                    oTK_ReclamationAnswer.ReclamationId = Convert.ToInt32(oTK_Reclamation.Id);
                    oTK_ReclamationAnswer.Text = "";
                    oTK_ReclamationAnswer.User = oTK_Reclamation.User;
                    db.OTK_ReclamationAnswer.Add(oTK_ReclamationAnswer);
                    db.SaveChanges();
                }
                var answerDelList = db.OTK_ReclamationAnswer.Where(d => d.ReclamationId == oTK_Reclamation.Id & d.Devision != oTK_Reclamation.Devision & d.Text == "");
                try
                {
                    foreach (var data in answerDelList)
                    {
                        db.OTK_ReclamationAnswer.Remove(db.OTK_ReclamationAnswer.Find(data.id));
                        db.SaveChanges();
                    }
                }
                catch
                {

                }
                return RedirectToAction("ListActiveWork");
            }
            ViewBag.CL = oTK_Reclamation.CheckList;
            ViewBag.CheckList = new SelectList(db.OTK_ChaeckList, "id", "UserCreate", oTK_Reclamation.CheckList);
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_Reclamation.TypeReclamation);
            ViewBag.User = new SelectList(db.AspNetUsers, "Id", "Email", oTK_Reclamation.User);
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name", oTK_Reclamation.Devision);
            return View(oTK_Reclamation);
        }
        
        [Authorize(Roles = "Admin, OTK")]
        public ActionResult EditPartialDeActive(int? Id)
        {
            OTK_Reclamation oTK_Reclamation = db.OTK_Reclamation.Find(Id);
            if (oTK_Reclamation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CL = oTK_Reclamation.CheckList;
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_Reclamation.TypeReclamation);
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name", oTK_Reclamation.Devision);
            return View(oTK_Reclamation);
        }
        [Authorize(Roles = "Admin, OTK")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartialDeActive(OTK_Reclamation oTK_Reclamation, OTK_ReclamationAnswer oTK_Answer)
        {
            if (ModelState.IsValid)
            {
                var user = db.AspNetUsers;
                string login = HttpContext.User.Identity.Name;
                foreach (var x in user)
                {
                    if (login == x.UserName.ToString())
                    {
                        oTK_Reclamation.User = x.Id;
                        break;
                    }
                }
                if (oTK_Reclamation.Description == null)
                    oTK_Reclamation.Description = "";
                if (oTK_Reclamation.ReclamationText == null)
                    oTK_Reclamation.ReclamationText = "";
                if (oTK_Reclamation.AnswerText == null)
                    oTK_Reclamation.AnswerText = "";
                oTK_Reclamation.LaborCostManufacturing = 0;
                db.Entry(oTK_Reclamation).State = EntityState.Modified;
                db.SaveChanges();
                var answerList = db.OTK_ReclamationAnswer.Where(d => d.ReclamationId == oTK_Reclamation.Id & d.Devision == oTK_Reclamation.Devision).ToList();
                if (answerList.Count == 0)
                {
                    OTK_ReclamationAnswer oTK_ReclamationAnswer = new OTK_ReclamationAnswer();
                    oTK_ReclamationAnswer.Completed = false;
                    oTK_ReclamationAnswer.countError = 1;
                    oTK_ReclamationAnswer.DateCreate = DateTime.Now;
                    oTK_ReclamationAnswer.DateUpdate = DateTime.Now;
                    oTK_ReclamationAnswer.Devision = Convert.ToInt32(oTK_Reclamation.Devision);
                    oTK_ReclamationAnswer.ReclamationId = Convert.ToInt32(oTK_Reclamation.Id);
                    oTK_ReclamationAnswer.Text = "";
                    oTK_ReclamationAnswer.User = oTK_Reclamation.User;
                    db.OTK_ReclamationAnswer.Add(oTK_ReclamationAnswer);
                    db.SaveChanges();
                }
                var answerDelList = db.OTK_ReclamationAnswer.Where(d => d.ReclamationId == oTK_Reclamation.Id & d.Devision != oTK_Reclamation.Devision & d.Text == "");
                try
                {
                    foreach (var data in answerDelList)
                    {
                        db.OTK_ReclamationAnswer.Remove(db.OTK_ReclamationAnswer.Find(data.id));
                        db.SaveChanges();
                    }
                }
                catch
                {

                }
                return RedirectToAction("ListDeactiveWork");
            }
            ViewBag.CL = oTK_Reclamation.CheckList;
            ViewBag.CheckList = new SelectList(db.OTK_ChaeckList, "id", "UserCreate", oTK_Reclamation.CheckList);
            ViewBag.TypeReclamation = new SelectList(db.OTK_TypeReclamation, "id", "Name", oTK_Reclamation.TypeReclamation);
            ViewBag.User = new SelectList(db.AspNetUsers, "Id", "Email", oTK_Reclamation.User);
            ViewBag.Devision = new SelectList(db.Devision.Where(z => z.OTK == true).OrderBy(x => x.name), "id", "name", oTK_Reclamation.Devision);
            return View(oTK_Reclamation);
        }
    }
}
