using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;
using System.Data.Entity;

namespace Wiki.Areas.ApproveCD.Models
{
    public class EmailApproveCDQue : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        string subject;
        string body;
        string login;
        int queId;
        List<MailList> mailToList;

        public EmailApproveCDQue(int queId, string login)
        {
            try
            {
                this.login = login;
                this.queId = queId;
                mail.From = new MailAddress(login);
                GetMailList();
                GetSubject();
                GetBody();
                SendEmail();
            }
            catch (Exception ex)
            {
                logger.Error("EmailApproveCDQue: EmailApproveCDQue: " + queId + " | Exeption text: " + ex);
            }
        }

        void SendEmail()
        {
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
            mail.Dispose();
        }

        private bool GetSubject()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                subject = "Вопрос по РКД на Заказ №: " + db.ApproveCDQuestions
                    .Find(queId).ApproveCDOrders.PZ_PlanZakaz.PlanZakaz.ToString();
                return true;
            }
        }

        private bool GetBody()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                body = subject + @"<br/>";
                body += "Текст вопроса: " + db.ApproveCDQuestions.Find(queId).textQuestion + @"<br/>" + @"<br/>";
                body += "История переписки: " + @"<br/>";
                foreach (var dataInQueList in db.ApproveCDQuestionCorr.Include(a => a.AspNetUsers).Where(a => a.id_ApproveCDQuestions == queId).ToList())
                {
                    body += dataInQueList.datetimeCreate + " | " + dataInQueList.textData + " | " + dataInQueList.AspNetUsers.CiliricalName + @"<br/>";
                }
                if(db.ApproveCDQuestions.Find(queId).active == true)
                {
                    body += @"<br/>" + @"<br/>" + "Вопрос закрыт";
                }

                return true;
            }
        }

        bool GetMailList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                int approveCDOrdersId = db.ApproveCDQuestions.Find(queId).id_ApproveCDOrders;
                mailToList = new List<MailList>();
                mailToList.Add(new MailList { mail = "myi@katek.by" });
                mailToList.Add(new MailList { mail = "bav@katek.by" });
                mailToList.Add(new MailList { mail = "vi@katek.by" });
                mailToList.Add(new MailList { mail = "goa@katek.by" });
                mailToList.Add(new MailList { mail = "nrf@katek.by" });
                mailToList.Add(new MailList { mail = "fvs@katek.by" });
                string mailToG = "";
                ApproveCDOrders approveCDOrders = db.ApproveCDOrders.Find(approveCDOrdersId);
                PZ_PlanZakaz pz = db.PZ_PlanZakaz.Find(approveCDOrders.id_PZ_PlanZakaz);
                mailToList.Add(pz.Manager == "a94f65df-4580-4729-9541-446ebee13c1e"
                    ? new MailList {mail = "cyv@katek.by"}
                    : new MailList {mail = "maj@katek.by"});
                try
                {
                    if (approveCDOrders.id_AspNetUsersM != "8363828f-bba2-4a89-8ed8-d7f5623b4fa8")
                    {
                        mailToG = db.AspNetUsers.Find(approveCDOrders.id_AspNetUsersM).Email;
                        if (mailToList.Where(a => a.mail == mailToG).ToList().Count == 0)
                        {
                            MailList mailList = new MailList();
                            mailList.mail = mailToG;
                            mailToList.Add(mailList);
                        }
                    }
                }
                catch
                {

                }
                try
                {
                    if (approveCDOrders.id_AspNetUsersE != "8363828f-bba2-4a89-8ed8-d7f5623b4fa8")
                    {
                        mailToG = db.AspNetUsers.Find(approveCDOrders.id_AspNetUsersE).Email;
                        if (mailToList.Where(a => a.mail == mailToG).ToList().Count == 0)
                        {
                            MailList mailList = new MailList();
                            mailList.mail = mailToG;
                            mailToList.Add(mailList);
                        }
                    }
                }
                catch
                {

                }
                try
                {
                    MailList mailList = new MailList();
                    mailList.mail = db.ApproveCDOrders.Find(approveCDOrdersId).PZ_PlanZakaz.AspNetUsers.Email;
                    mailToList.Add(mailList);
                }
                catch
                {

                }
                foreach (var dataList in mailToList)
                {
                    mail.To.Add(new MailAddress(dataList.mail));
                }
                return true;
            }
        }
    }
}