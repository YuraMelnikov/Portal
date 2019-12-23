using NLog;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.VerifPlan.Models
{
    public class EmailVerifPlan : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        int stepNumber;
        string subject;
        string body;
        string login;
        List<string> mailToList;
        PlanVerificationItems order;
        private new readonly PortalKATEKEntities db = new PortalKATEKEntities();

        public EmailVerifPlan(PlanVerificationItems order, string login, int stepNumber)
        {
            mailToList = new List<string>();
            try
            {
                this.login = login;
                this.stepNumber = stepNumber;
                this.order = db.PlanVerificationItems.Find(order.id);
                mail.From = new MailAddress(login);
                GetMailListCreate();
                GetSubject();
                GetBody();
                SendEmail();
                logger.Debug("EmailVerifPlan: " + order.id);
            }
            catch
            {
                
            }
        }

        void SendEmail()
        {
            foreach (var dataUser in mailToList)
            {
                mail.To.Add(new MailAddress(dataUser));
            }
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
            mail.Dispose();
        }

        private bool GetSubject()
        {
            subject = "";
            if (stepNumber == 1)
            {
                subject = "Установлен срок передачи изделия на проверку: " + order.PZ_PlanZakaz.PlanZakaz.ToString();
            }
            else if (stepNumber == 2)
            {
                subject = "Изделение передано на проверку: " + order.PZ_PlanZakaz.PlanZakaz.ToString();
            }
            else if (stepNumber == 3)
            {
                subject = "Изделие принято на проверку: " + order.PZ_PlanZakaz.PlanZakaz.ToString();
            }
            else if (stepNumber == 4)
            {
                subject = "Запрос на смену сроков передачи изделия на проверку: " + order.PZ_PlanZakaz.PlanZakaz.ToString();
            }
            else if (stepNumber == 5)
            {
                subject = "Внимание, срыв плановых сроков: " + order.PZ_PlanZakaz.PlanZakaz.ToString();
            }
            subject = subject.Replace(Environment.NewLine, "");
            return true;
        }

        private bool GetBody()
        {
            body = "";
            if (stepNumber == 1)
            {
                body = "Добрый день!" + "<br/>";
                body += "Установлен срок передачи изделия на проверку: " + order.PZ_PlanZakaz.PlanZakaz.ToString() + " | "  + order.planDate.Value.ToShortDateString();
            }
            else if (stepNumber == 2)
            {
                body = "Добрый день!" + "<br/>";
                body += "Изделение передано на проверку: " + order.PZ_PlanZakaz.PlanZakaz.ToString() + " | " + order.factDate.Value.ToShortDateString();
            }
            else if (stepNumber == 3)
            {
                body = "Добрый день!" + "<br/>";
                body += "Изделие принято на проверку: " + order.PZ_PlanZakaz.PlanZakaz.ToString() + " | " + order.appDate.Value.ToShortDateString();
            }
            else if (stepNumber == 4)
            {
                body = "Добрый день!" + "<br/>";
                body += "Запрос на смену сроков передачи изделия на проверку: " + order.PZ_PlanZakaz.PlanZakaz.ToString() + " | " + order.fixedDateForKO.Value.ToShortDateString();
            }
            else if (stepNumber == 5)
            {
                body = "Добрый день!" + "<br/>";
                body += "Внимание, срыв плановых сроков: " + order.PZ_PlanZakaz.PlanZakaz.ToString() + " | " + order.fixedDateForKO.Value.ToShortDateString();
            }
            return true;
        }

        bool GetMailListCreate()
        {
            mailToList.Add("Kuchynski@katek.by");
            mailToList.Add("myi@katek.by");
            mailToList.Add("Medvedev@katek.by");
            mailToList.Add("pev@katek.by");
            mailToList.Add("bav@katek.by");
            return true;
        }
    }
}