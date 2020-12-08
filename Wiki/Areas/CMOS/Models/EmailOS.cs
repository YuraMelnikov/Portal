using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.CMOS.Models
{
    public class EmailOS : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        string subject;
        string body;
        string login;
        List<string> mailToList;
        CMOSOrder order;
        private new readonly PortalKATEKEntities db = new PortalKATEKEntities();

        public EmailOS(CMOSOrder order, string login)
        {
            try
            {
                mailToList = new List<string>();
                this.login = login;
                this.order = db.CMOSOrder.Find(order.id);
                mail.From = new MailAddress(login);
                GetMailPM();
                GetSubject();
                GetBody();
                SendEmail();
                logger.Debug("EmailOS: " + order.id);
            }
            catch (Exception ex)
            {
                logger.Error("EmailOS: " + order.id + " | " + ex);
            }
        }

        bool GetMailPM()
        {
            mailToList.Add("myi@katek.by");
            return true;
        }

        private bool GetSubject()
        {
            subject = "Заказ деталей: " + order.id;
            subject = subject.Replace(Environment.NewLine, "");
            return true;
        }

        private bool GetBody()
        {
            body = "Добрый день!" + "<br/>";
            body += "На заказ изделий из ЛМ № : " + order.id + "<br/>";
            body += "в 1с7 создан документ Поступление ТМЦ: " + order.numberTN + "<br/>";
            body += "Необходимо проконтролировать бюджет." + "<br/>";

            body += "Расчетный вес: " + "<br/>";
            body += "Ставка за кг.: " + order.rate + "<br/>";
            body += "Курс USD/BYN: " + "<br/>";
            body += "Цена, BYN: " + "<br/>";
            body += "Стоимость, BYN: " + "<br/>";
            body += "--------------------------------" + "<br/>";
            body += "Стоимость - Цена: " + "<br/>";
            return true;
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
    }
}