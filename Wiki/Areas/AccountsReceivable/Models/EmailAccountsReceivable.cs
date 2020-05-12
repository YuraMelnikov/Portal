using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.AccountsReceivable.Models
{
    public class EmailAccountsReceivable : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        List<string> mailToList;

        public EmailAccountsReceivable(string path, string login, string subject)
        {
            try
            {
                mail.From = new MailAddress(login);
                mailToList = new List<string>();
                mailToList.Add("myi@katek.by");
                mailToList.Add("mvv@katek.by");
                mailToList.Add("omo@katek.by");
                mailToList.Add("evy@katek.by");
                foreach (var data in GetFileArray(path))
                {
                    mail.Attachments.Add(new Attachment(data));
                }
                foreach (var dataUser in mailToList)
                {
                    mail.To.Add(new MailAddress(dataUser));
                }
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = subject;
                client.Send(mail);
                mail.Dispose();
                logger.Debug("EmailAccountsReceivable: " + subject);
            }
            catch (Exception ex)
            {
                logger.Error("EmailAccountsReceivable: " + subject + " | " + ex);
            }
        }

        private List<string> GetFileArray(string path)
        {
            var fileList = Directory.GetFiles(path).ToList();
            return fileList;
        }
    }
}