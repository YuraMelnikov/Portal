namespace Wiki.Models
{
    public class EmailPutErrorClient : EmailVersion
    {
        public EmailPutErrorClient(RKD_Order rkdOrder, string recipient, string folder)
            : base(rkdOrder, recipient)
        {
            this.folder = folder;
        }

        private readonly string folder;

        protected override void GetSubject()
        {
            subject += GetNumberPlanZakaz() + " - ";
            subject += db.TypeRKD_Mail_Version.Find(7).name.ToString();
        }

        protected override void GetBody(string errorText)
        {
            body += subject + "<br/>";
            body += "Прим.:" + errorText + "<br/>";
            body += "<a href>" + folder + "</a>" + "<br/>";
        }
    }
}