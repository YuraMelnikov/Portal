using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.ApproveCD.Models
{
    public class EmailApproveCD : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //Stepnumber:
        //toTp - 1
        //toError - 2
        //toComplited - 3

        int stepNumber;
        string subject;
        string body;
        string login;
        string link;
        int approveCDOrdersId;
        List<MailList> mailToList;

        public EmailApproveCD(int approveCDOrdersId, string login, int stepNumber, string link)
        {
            try
            {
                this.link = link;
                this.approveCDOrdersId = approveCDOrdersId;
                this.login = login;
                this.stepNumber = stepNumber;
                mail.From = new MailAddress(login);
                GetMailList();
                GetSubject();
                GetBody();
                SendEmail();
            }
            catch (Exception ex)
            {
                logger.Error("EmailApproveCD: EmailApproveCD: " + approveCDOrdersId + " | Exeption text: " + ex);
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
                subject = "Заказ №: " + db.ApproveCDOrders
                    .Find(approveCDOrdersId).PZ_PlanZakaz.PlanZakaz
                    .ToString();
                if (stepNumber == 1)
                {
                    subject += " - Отправлена черновая версия РКД в ТП";
                }
                if (stepNumber == 2)
                {
                    subject += " - Получен ответ от Заказчика";
                }
                if (stepNumber == 3)
                {
                    subject += " - Получено согласование РКД";
                }
                return true;
            }
        }

        private bool GetBody()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                if (stepNumber < 3)
                    body += subject + @"<br/>" + @"<br/>" + link;
                else
                    body += subject;
                return true;
            }
        }

        bool GetMailList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                mailToList = new List<MailList>();
                mailToList.Add(new MailList { mail = "myi@katek.by"});
                mailToList.Add(new MailList { mail = "bav@katek.by" });
                mailToList.Add(new MailList { mail = "vi@katek.by" });
                mailToList.Add(new MailList { mail = "goa@katek.by" });
                mailToList.Add(new MailList { mail = "maj@katek.by" });
                mailToList.Add(new MailList { mail = "nrf@katek.by" });
                mailToList.Add(new MailList { mail = "Kuchynski@katek.by" });
                mailToList.Add(new MailList { mail = "fvs@katek.by" });
                mailToList.Add(new MailList { mail = "gea@katek.by" });
                string mailToG = "";
                ApproveCDOrders approveCDOrders = db.ApproveCDOrders.Find(approveCDOrdersId);
                try
                {
                    if (approveCDOrders.id_AspNetUsersM != "4f91324a-1918-4e62-b664-d8cd89a19d95")
                    {
                        mailToG = db.AspNetUsers.Find(approveCDOrders.id_AspNetUsersM).Email;
                        if (mailToList.Where(a => a.mail == mailToG).ToList() == null)
                        {
                            mailToList.Add(new MailList { mail = mailToG });
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
                        if (mailToList.Where(a => a.mail == mailToG).ToList() == null)
                        {
                            mailToList.Add(new MailList { mail = mailToG });
                        }
                    }
                }
                catch
                {

                }
                try
                {
                    mailToList.Add(new MailList { mail = db.ApproveCDOrders.Find(approveCDOrdersId).PZ_PlanZakaz.AspNetUsers.Email });
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