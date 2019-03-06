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
}