using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.E3.Models
{
    public class EmailE3 : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        string subject;
        string body;
        string login;
        List<string> mailToList;

        private new readonly PortalKATEKEntities db = new PortalKATEKEntities();

        public EmailE3(string login, string entry)
        {
            mailToList = new List<string>();
            try
            {
                this.subject = entry;
                this.body = entry;
                this.login = login;
                mail.From = new MailAddress(login);
                GetMailList();
                SendEmail();
                logger.Debug("EmailE3:" + entry);
            }
            catch (Exception ex)
            {
                logger.Error("EmailE3: " + entry + " | " + ex);
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

        bool GetMailList()
        {
            var loginList = db.AspNetUsers.Where(a => a.Devision == 16 && a.LockoutEnabled == true).ToList();
            //foreach (var user in loginList)
            //{
            //    mailToList.Add(user.Email);
            //}
            mailToList.Add("dkv@katek.by");
            mailToList.Add("myi@katek.by");
            return true;
        }
    }
}