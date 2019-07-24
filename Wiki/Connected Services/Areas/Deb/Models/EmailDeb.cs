using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.Deb.Models
{
    public class EmailDeb : EmailClient
    {
        string period;

        public EmailDeb(string period)
        {
            this.period = period;
        }
        
        string GetSubject()
        {
            return "Сверка дат оприходования товара завершена (" + period + ")";
        }

        string GetBody()
        {
            return "Сверка дат оприходования товара завершена (" + period + ")";
        }

        void SetMailAddress()
        {
            mail.To.Add(new MailAddress("laa@katek.by"));
            mail.To.Add(new MailAddress("myi@katek.by"));
            mail.To.Add(new MailAddress("gvi@katek.by"));
        }

        public void SendEmail()
        {
            mail.From = new MailAddress("mvv@katek.by");
            SetMailAddress();
            mail.IsBodyHtml = true;
            mail.Subject = GetSubject();
            mail.Body = GetBody();
            client.Send(mail);
            mail.Dispose();
        }
    }
}