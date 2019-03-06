using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class UploadDocumentController : Controller
    {
        string smtpServer = "192.168.1.65";
        int portSmtpServer = 25;
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult UploadGraphic()
        {
            List<PZ_PlanZakaz> listPZ_PlanZakaz = new List<PZ_PlanZakaz>();
            var listDebitWork = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 6 || d.id_TaskForPZ == 7).ToList();
            foreach (var data in listDebitWork)
            {
                listPZ_PlanZakaz.Add(db.PZ_PlanZakaz.Find(data.id_PlanZakaz));
            }
            ViewBag.id_PlanZakaz = new SelectList(listPZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");

            return View();
        }
        [HttpPost]
        public ActionResult UploadGraphic(HttpPostedFileBase upload, int[] PlanZakaz)
        {
            foreach (var data in PlanZakaz)
            {
                var pZ_PlanZakaz = db.PZ_PlanZakaz.Find(data);
                string adres = pZ_PlanZakaz.Folder.ToString();
                adres += db.FolderDocument.Find(1).adres;
                string fileName = pZ_PlanZakaz.PlanZakaz.ToString() + "_" + Path.GetFileName(upload.FileName);
                upload.SaveAs(Path.Combine(@adres, fileName));
                try
                {
                    Debit_WorkBit debit_WorkBit = new Debit_WorkBit();
                    debit_WorkBit = db.Debit_WorkBit
                        .Where(d => d.id_PlanZakaz == data)
                        .Where(d => d.id_TaskForPZ == 6)
                        .Where(d => d.close == false)
                        .First();
                    if (debit_WorkBit.close == false)
                    {
                        debit_WorkBit.close = true;
                        debit_WorkBit.dateClose = DateTime.Now;
                        db.Entry(debit_WorkBit).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch
                {

                }
                try
                {
                    Debit_WorkBit debit_WorkBit = new Debit_WorkBit();
                    debit_WorkBit = db.Debit_WorkBit
                        .Where(d => d.id_PlanZakaz == data)
                        .Where(d => d.id_TaskForPZ == 7)
                        .Where(d => d.close == false)
                        .First();
                    if (debit_WorkBit.close == false)
                    {
                        debit_WorkBit.close = true;
                        debit_WorkBit.dateClose = DateTime.Now;
                        db.Entry(debit_WorkBit).State = EntityState.Modified;
                        db.SaveChanges();

                        Debit_WorkBit newDebit_WorkBit = new Debit_WorkBit();
                        newDebit_WorkBit.dateCreate = DateTime.Now;
                        newDebit_WorkBit.close = false;
                        newDebit_WorkBit.id_PlanZakaz = debit_WorkBit.id_PlanZakaz;
                        newDebit_WorkBit.id_TaskForPZ = 25;



                        newDebit_WorkBit.datePlanFirst = DateTime.Now;
                        newDebit_WorkBit.datePlan = DateTime.Now;
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
                catch
                { }
            }
            return RedirectToAction("Index", "Debit_WorkBit");
        }
        
        public ActionResult UploadAlertShip()
        {
            List<PZ_PlanZakaz> listPZ_PlanZakaz = new List<PZ_PlanZakaz>();
            var listDebitWork = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 5).ToList();
            foreach (var data in listDebitWork)
            {
                listPZ_PlanZakaz.Add(db.PZ_PlanZakaz.Find(data.id_PlanZakaz));
            }
            ViewBag.id_PlanZakaz = new SelectList(listPZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
        }
        [HttpPost]
        public ActionResult UploadAlertShip(HttpPostedFileBase upload, int[] PlanZakaz, string myClientConArr, DateTime DateSupply)
        {
            foreach (var data in PlanZakaz)
            {
                var pZ_PlanZakaz = db.PZ_PlanZakaz.Find(data);
                string adres = pZ_PlanZakaz.Folder.ToString();
                adres += db.FolderDocument.Find(3).adres;
                string fileName = pZ_PlanZakaz.PlanZakaz.ToString() + "__" + Path.GetFileName(upload.FileName);
                upload.SaveAs(Path.Combine(@adres, fileName));

                Debit_WorkBitController firstDebit_WorkBit = new Debit_WorkBitController();
                firstDebit_WorkBit.CloseComplitedTasks(pZ_PlanZakaz.Id, 5);

                PostAlertShip postAlertShip = new PostAlertShip();
                postAlertShip.datePost = DateSupply;
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == pZ_PlanZakaz.Id).Where(d => d.id_TaskForPZ ==5).First();
                postAlertShip.id_Debit_WorkBit = debit_WorkBit.id;
                postAlertShip.numPost = myClientConArr;
                db.PostAlertShip.Add(postAlertShip);
                db.SaveChanges();
                List<string> list = new List<string>();
                var debit_EmailList = db.Debit_EmailList.Where(d => d.C5 == true).ToList();
                foreach (var data1 in debit_EmailList)
                {
                    list.Add(data1.email);
                }
                SendMail(debit_WorkBit, " : отправлено уведомление о готовности к отгрузке\n" + adres, list);
            }

            return RedirectToAction("Index", "Debit_WorkBit");
        }

        public ActionResult UploadMatchingShip()
        {
            List<PZ_PlanZakaz> listPZ_PlanZakaz = new List<PZ_PlanZakaz>();
            var listDebitWork = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 18).ToList();
            foreach (var data in listDebitWork)
            {
                listPZ_PlanZakaz.Add(db.PZ_PlanZakaz.Find(data.id_PlanZakaz));
            }
            ViewBag.id_PlanZakaz = new SelectList(listPZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.tipe = new SelectList(db.Debit_MatchingType, "name", "name");
            return View();
        }
        [HttpPost]
        public ActionResult UploadMatchingShip(int[] PlanZakaz, string tipe)
        {
            foreach (var data in PlanZakaz)
            {
                var pZ_PlanZakaz = db.PZ_PlanZakaz.Find(data);

                //string adres = pZ_PlanZakaz.Folder.ToString();
                //adres += db.FolderDocument.Find(3).adres;
                //string fileName = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_"
                //        + DateTime.Now.Day.ToString() + "_" + pZ_PlanZakaz.PlanZakaz.ToString() + "__" + Path.GetFileName(upload.FileName);
                //upload.SaveAs(Path.Combine(@adres, fileName));


                Debit_MatchingType debit_MatchingType = db.Debit_MatchingType.Where(d => d.name == tipe).First();
                Debit_WorkBitController firstDebit_WorkBit = new Debit_WorkBitController();
                firstDebit_WorkBit.CloseComplitedTasks(pZ_PlanZakaz.Id, 18);
                PostMatching postMatching = new PostMatching();
                postMatching.id_MatchingType = debit_MatchingType.id;
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == pZ_PlanZakaz.Id).Where(d => d.id_TaskForPZ == 18).First();
                postMatching.id_Debit_WorkBit = debit_WorkBit.id;
                db.PostMatching.Add(postMatching);
                db.SaveChanges();

                //List<string> list = new List<string>();
                //var debit_EmailList = db.Debit_EmailList.Where(d => d.C18 == true).ToList();
                //foreach (var data1 in debit_EmailList)
                //{
                //    list.Add(data1.email);
                //}
                //list.Add(pZ_PlanZakaz.AspNetUsers.Email);
                //SendMail(pZ_PlanZakaz.PlanZakaz.ToString(), " : разрешение на отгрузку" + "\n\n" + postMatching.Debit_MatchingType.name, list);
            }
            return RedirectToAction("Index", "Debit_WorkBit");
        }

        public ActionResult UploadCMR()
        {
            List<PZ_PlanZakaz> listPZ_PlanZakaz = new List<PZ_PlanZakaz>();
            var listDebitWork = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 8).ToList();
            foreach(var data in listDebitWork)
            {
                listPZ_PlanZakaz.Add(db.PZ_PlanZakaz.Find(data.id_PlanZakaz));
            }
            ViewBag.id_PlanZakaz = new SelectList(listPZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
        }
        [HttpPost]
        public ActionResult UploadCMR(int[] PlanZakaz, string myClientCon, string DateSupply)
        {
            foreach (var data in PlanZakaz)
            {
                var pZ_PlanZakaz = db.PZ_PlanZakaz.Find(data);
                //string adres = pZ_PlanZakaz.Folder.ToString();
                //adres += db.FolderDocument.Find(4).adres;
                //string fileName = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_"
                //        + DateTime.Now.Day.ToString() + "_" + pZ_PlanZakaz.PlanZakaz.ToString() + "__" + Path.GetFileName(upload.FileName);
                //upload.SaveAs(Path.Combine(@adres, fileName));
                Debit_WorkBitController firstDebit_WorkBit = new Debit_WorkBitController();
                firstDebit_WorkBit.CloseComplitedTasks(pZ_PlanZakaz.Id, 8);
                Debit_CMR debit_CMR = new Debit_CMR();
                debit_CMR.dateShip = Convert.ToDateTime(DateSupply);
                debit_CMR.id_DebitTask = db.Debit_WorkBit.Where(d => d.id_TaskForPZ == 8).Where(d => d.id_PlanZakaz == pZ_PlanZakaz.Id).First().id;
                debit_CMR.number = myClientCon;
                db.Debit_CMR.Add(debit_CMR);
                db.SaveChanges();

                List<string> list = new List<string>();
                var debit_EmailList = db.Debit_EmailList.Where(d => d.C8 == true).ToList();
                foreach (var data1 in debit_EmailList)
                {
                    list.Add(data1.email);
                }
                SendMail(pZ_PlanZakaz.PlanZakaz.ToString(), " : внесены данные о ЖДН/CMR\n" + pZ_PlanZakaz.Folder, list);
            }
            return RedirectToAction("Index", "Debit_WorkBit");
        }
        
        public ActionResult UploadDocument(int? idTask, int? idFolder)
        {
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.idTask = idTask;
            ViewBag.idFolder = idFolder;
            return View();
        }
        [HttpPost]
        public ActionResult UploadDocument(HttpPostedFileBase upload, int[] PlanZakaz, string myClientConArr, DateTime DateSupply, int? idTask, int? idFolder)
        {
            foreach (var data in PlanZakaz)
            { 
                var pZ_PlanZakaz = db.PZ_PlanZakaz.Find(data);
                string adres = pZ_PlanZakaz.Folder.ToString();
                adres += db.FolderDocument.Find(idFolder).adres;
                string fileName = pZ_PlanZakaz.PlanZakaz.ToString() + "__" + Path.GetFileName(upload.FileName);
                upload.SaveAs(Path.Combine(@adres, fileName));
                Debit_WorkBitController firstDebit_WorkBit = new Debit_WorkBitController();
                firstDebit_WorkBit.CloseComplitedTasks(pZ_PlanZakaz.Id, Convert.ToInt32(idTask));
            }

            return RedirectToAction("Index", "Debit_WorkBit");
        }
        
        public ActionResult UploadAlertShipClose()
        {
            List<PZ_PlanZakaz> listPZ_PlanZakaz = new List<PZ_PlanZakaz>();
            var listDebitWork = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 10).ToList();
            foreach (var data in listDebitWork)
            {
                listPZ_PlanZakaz.Add(db.PZ_PlanZakaz.Find(data.id_PlanZakaz));
            }

            ViewBag.id_PlanZakaz = new SelectList(listPZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
        }
        [HttpPost]
        public ActionResult UploadAlertShipClose(HttpPostedFileBase upload, int[] PlanZakaz, string myClientConArr, DateTime DateSupply)
        {
            foreach (var data in PlanZakaz)
            {
                var pZ_PlanZakaz = db.PZ_PlanZakaz.Find(data);
                Debit_WorkBitController firstDebit_WorkBit = new Debit_WorkBitController();
                firstDebit_WorkBit.CloseComplitedTasks(pZ_PlanZakaz.Id, 10);
                PostAlertShip postAlertShip = new PostAlertShip();
                postAlertShip.datePost = DateSupply;
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == pZ_PlanZakaz.Id).Where(d => d.id_TaskForPZ == 10).First();
                postAlertShip.id_Debit_WorkBit = debit_WorkBit.id;
                postAlertShip.numPost = myClientConArr;
                db.PostAlertShip.Add(postAlertShip);
                db.SaveChanges();
                List<string> list = new List<string>();
                list.Add(pZ_PlanZakaz.AspNetUsers.Email);
                list.Add("myi@katek.by");
                list.Add("mvv@katek.by");
                list.Add("gea@katek.by");
                list.Add("maa@katek.by");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("myi@katek.by");
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] != null)
                        mail.To.Add(new MailAddress(list[i]));
                }
                mail.Subject = pZ_PlanZakaz.PlanZakaz.ToString() +  " получено письмо от экспедитора о поставке товара на станцию назначения";
                mail.Body = pZ_PlanZakaz.PlanZakaz.ToString() + " получено письмо от экспедитора о поставке товара на станцию назначения";
                SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
                client.EnableSsl = false;
                string adres = pZ_PlanZakaz.Folder.ToString();
                adres += db.FolderDocument.Find(4).adres;
                string fileName = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_"
                                  + DateTime.Now.Day.ToString() + "_" + pZ_PlanZakaz.PlanZakaz.ToString() + "__" + Path.GetFileName(upload.FileName);
                upload.SaveAs(Path.Combine(adres, fileName));
                mail.Attachments.Add(new Attachment(Path.Combine(adres, fileName)));
                client.Send(mail);
                mail.Dispose();
            }
            return RedirectToAction("Index", "Debit_WorkBit");
        }
        
        public ActionResult PostUploadAlertShipClose()
        {
            List<PZ_PlanZakaz> listPZ_PlanZakaz = new List<PZ_PlanZakaz>();
            var listDebitWork = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 12).ToList();
            foreach (var data in listDebitWork)
            {
                listPZ_PlanZakaz.Add(db.PZ_PlanZakaz.Find(data.id_PlanZakaz));
            }
            ViewBag.id_PlanZakaz = new SelectList(listPZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
        }
        [HttpPost]
        public ActionResult PostUploadAlertShipClose(HttpPostedFileBase upload, int[] PlanZakaz, string myClientConArr, DateTime DateSupply)
        {
            foreach (var data in PlanZakaz)
            {
                var pZ_PlanZakaz = db.PZ_PlanZakaz.Find(data);
                string adres = pZ_PlanZakaz.Folder.ToString();
                adres += db.FolderDocument.Find(4).adres;
                string fileName = pZ_PlanZakaz.PlanZakaz.ToString() + "__" + Path.GetFileName(upload.FileName);
                upload.SaveAs(Path.Combine(@adres, fileName));
                Debit_WorkBitController firstDebit_WorkBit = new Debit_WorkBitController();
                firstDebit_WorkBit.CloseComplitedTasks(pZ_PlanZakaz.Id, 12);
                PostAlertShip postAlertShip = new PostAlertShip();
                postAlertShip.datePost = DateSupply;
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == pZ_PlanZakaz.Id).Where(d => d.id_TaskForPZ == 12).First();
                postAlertShip.id_Debit_WorkBit = debit_WorkBit.id;
                postAlertShip.numPost = myClientConArr;
                db.PostAlertShip.Add(postAlertShip);
                db.SaveChanges();
                List<string> list = new List<string>();
                var debit_EmailList = db.Debit_EmailList.Where(d => d.C12 == true).ToList();
                foreach (var data1 in debit_EmailList)
                {
                    list.Add(data1.email);
                }
                list.Add(pZ_PlanZakaz.AspNetUsers.Email);
                SendMail(pZ_PlanZakaz.PlanZakaz.ToString(), " : отправлено письмо-уведомление Заказчику о прибытии заказа на станцию назначения", list);
            }

            return RedirectToAction("Index", "Debit_WorkBit");
        }

        public ActionResult AlertOpenOrderInManuf()
        {
            List<PZ_PlanZakaz> listPZ_PlanZakaz = new List<PZ_PlanZakaz>();
            var listDebitWork = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 6 || d.id_TaskForPZ == 7).ToList();
            foreach (var data in listDebitWork)
            {
                listPZ_PlanZakaz.Add(db.PZ_PlanZakaz.Find(data.id_PlanZakaz));
            }
            ViewBag.id_PlanZakaz = new SelectList(listPZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");

            return View();
        }
        [HttpPost]
        public ActionResult AlertOpenOrderInManuf(HttpPostedFileBase upload, int[] PlanZakaz)
        {
            foreach (var data in PlanZakaz)
            {
                var pZ_PlanZakaz = db.PZ_PlanZakaz.Find(data);
                string adres = pZ_PlanZakaz.Folder.ToString();
                adres += db.FolderDocument.Find(6).adres;
                string fileName = pZ_PlanZakaz.PlanZakaz.ToString() + "_" + Path.GetFileName(upload.FileName);
                upload.SaveAs(Path.Combine(@adres, fileName));
                try
                {
                    Debit_WorkBit debit_WorkBit = new Debit_WorkBit();
                    debit_WorkBit = db.Debit_WorkBit
                        .Where(d => d.id_PlanZakaz == data)
                        .Where(d => d.id_TaskForPZ == 30)
                        .Where(d => d.close == false)
                        .First();
                    if (debit_WorkBit.close == false)
                    {
                        debit_WorkBit.close = true;
                        debit_WorkBit.dateClose = DateTime.Now;
                        db.Entry(debit_WorkBit).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch
                {

                }
            }
            return RedirectToAction("Index", "Debit_WorkBit");
        }

        public void SendMail(Debit_WorkBit debit_WorkBit, string text, List<string> list)
        {
            EmailModel emailModel = new EmailModel();
            try
            {
                emailModel.SendEmail(list.ToArray(), debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString() + text, debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString() + text);
            }
            catch
            {
                emailModel.SendEmailOnePerson("myi@katek.by", "ошибка отправки", debit_WorkBit.id.ToString());
            }
        }
        public void SendMail(HttpPostedFileBase upload, Debit_WorkBit debit_WorkBit, string text, List<string> list)
        {
            EmailModel emailModel = new EmailModel();
            try
            {
                emailModel.SendEmail(list.ToArray(), debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString() + text, debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString() + text, upload);
            }
            catch
            {
                emailModel.SendEmailOnePerson("myi@katek.by", "ошибка отправки", debit_WorkBit.id.ToString());
            }
        }
        public void SendMail(string debit_CMR, string text, List<string> list)
        {
            EmailModel emailModel = new EmailModel();
            try
            {
                emailModel.SendEmailList(list, debit_CMR + text, debit_CMR + text);
            }
            catch
            {
                emailModel.SendEmailOnePerson("myi@katek.by", "ошибка отправки", "Заказ " + debit_CMR + "text");
            }
        }
    }
}