using System.Net;
using System.Net.Mail;

namespace Wiki.Models
{
    public class EmailClient
    {
        public string mailAdressSender = "myi@katek.by";
        public readonly string smtpServer = "192.168.1.65";
        public readonly int portSmtpServer = 25;
        protected MailMessage mail = new MailMessage();
        protected SmtpClient client = null;
        protected PortalKATEKEntities db = new PortalKATEKEntities();

        public EmailClient()
        {
            client = new SmtpClient(smtpServer, portSmtpServer);
            mail.From = new MailAddress(mailAdressSender);
            client.EnableSsl = false;
        }
    }

    public class GEmailClient
    {
        public string mailAdressSender = "cmokatek@gmail.com";
        public readonly string smtpServer = "smtp.gmail.com";
        public readonly int portSmtpServer = 587;
        protected MailMessage mail = new MailMessage();
        protected SmtpClient client = null;
        protected PortalKATEKEntities db = new PortalKATEKEntities();

        public GEmailClient()
        {
            client = new SmtpClient(smtpServer, portSmtpServer);
            client.Credentials = new NetworkCredential("cmokatek@gmail.com", "3narodowy3");
            mail.From = new MailAddress(mailAdressSender);
            client.EnableSsl = true;
        }
    }
}