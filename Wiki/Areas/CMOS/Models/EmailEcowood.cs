using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.CMOS.Models
{
    public class EmailEcowood : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        string subject;
        string body;
        string login;
        List<string> mailToList;
        CMOSOrder order;
        private new readonly PortalKATEKEntities db = new PortalKATEKEntities();

        public EmailEcowood(CMOSOrder order, string login)
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
                logger.Debug("EmailEcowood: " + order.id);
            }
            catch (Exception ex)
            {
                logger.Error("EmailEcowood: " + order.id + " | " + ex);
            }
        }

        bool GetMailPM()
        {
            mailToList.Add("sim@katek.by");
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
            body += "Необходимо распечатать этикетки" + "<br/>";
            return true;
        }

        void SendEmail()
        {
            foreach (var data in GetFileArray())
            {
                mail.Attachments.Add(new Attachment(data));
            }
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

        private List<string> GetFileArray()
        {
            return Directory.GetFiles(@"\\192.168.1.30\m$\_ЗАКАЗЫ\CMOS\Ecowood\" + order.id.ToString() + @"\").ToList();
        }
    }
}