using System.Collections.Generic;
using System.Net.Mail;
using System.Web;

namespace Wiki.Models
{
    public class EmailModel
    {
        string mailAdressSender = "myi@katek.by";
        string smtpServer = "192.168.1.3";
        int portSmtpServer = 25;

        public void SendEmailOnePerson(string recipient, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            mail.To.Add(new MailAddress(recipient));
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;
            client.Send(mail);
            mail.Dispose();
        }

        public void SendEmailOnePerson(string recipient, string subject, string body, List <string> attachment)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            mail.To.Add(new MailAddress(recipient));
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;
            foreach (var data in attachment)
            {
                mail.Attachments.Add(new Attachment(data));
            }
            client.Send(mail);
            mail.Dispose();
        }

        public void SendEmail(string[] recipient, string subject, string body, HttpPostedFileBase[] fileUpload)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            for (int i = 0; i < recipient.Length; i++)
            {
                if (recipient[i] != null)
                    mail.To.Add(new MailAddress(recipient[i]));
            }
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;
            foreach (var data in fileUpload)
            {
                mail.Attachments.Add(new Attachment(data.FileName));
            }
            client.Send(mail);
            mail.Dispose();
        }

        public void SendEmail (string[] recipient, string subject, string body, List<string> attachment)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            for(int i = 0; i < recipient.Length; i++)
            {
                if (recipient[i] != null)
                    mail.To.Add(new MailAddress(recipient[i]));
            }
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;
            foreach (var data in attachment)
            {
                mail.Attachments.Add(new Attachment(data));
            }
            client.Send(mail);
            mail.Dispose();
        }

        public void SendEmail(string[] recipient, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            for (int i = 0; i < recipient.Length; i++)
            {
                if (recipient[i] != null)
                    mail.To.Add(new MailAddress(recipient[i]));
            }
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;

            client.Send(mail);
            mail.Dispose();
        }

        public void SendEmailList(List<string> recipient, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            foreach (var data in recipient)
            {
                mail.To.Add(new MailAddress(data));
            }
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;
            client.Send(mail);
            mail.Dispose();
        }

        public void SendEmailListToHTML(List<string> recipient, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            foreach (var data in recipient)
            {
                mail.To.Add(new MailAddress(data));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;
            client.Send(mail);
            mail.Dispose();
        }

        public void SendEmailListHtml(List<string> recipient, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            foreach (var data in recipient)
            {
                mail.To.Add(new MailAddress(data));
            }
            mail.Subject = subject;
            mail.Body = body + "\n";
            mail.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;
            client.Send(mail);
            mail.Dispose();
        }
        
        public void SendEmail(string[] recipient, string subject, string body, HttpPostedFileBase[] fileUpload, string mailAdressSender)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            for (int i = 0; i < recipient.Length; i++)
            {
                if (recipient[i] != null)
                    mail.To.Add(new MailAddress(recipient[i]));
            }
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;
            foreach (var data in fileUpload)
            {
                mail.Attachments.Add(new Attachment(data.FileName));
            }
            client.Send(mail);
            mail.Dispose();
        }

        public void SendEmail(string[] recipient, string subject, string body, List<string> attachment, string mailAdressSender)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            for (int i = 0; i < recipient.Length; i++)
            {
                if (recipient[i] != null)
                    mail.To.Add(new MailAddress(recipient[i]));
            }
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;
            foreach (var data in attachment)
            {
                mail.Attachments.Add(new Attachment(data));
            }
            client.Send(mail);
            mail.Dispose();
        }

        public void SendEmail(string[] recipient, string subject, string body, string mailAdressSender)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailAdressSender);
            for (int i = 0; i < recipient.Length; i++)
            {
                if (recipient[i] != null)
                    mail.To.Add(new MailAddress(recipient[i]));
            }
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient client = new SmtpClient(smtpServer, portSmtpServer);
            client.EnableSsl = false;

            client.Send(mail);
            mail.Dispose();
        }
    }
}