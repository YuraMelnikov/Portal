namespace Wiki.Models
{
    public class EmailSuccessRKD : EmailVersion
    {
        public EmailSuccessRKD(RKD_Order rkdOrder, string recipient)
            : base(rkdOrder, recipient)
        {

        }

        protected override void GetSubject()
        {
            subject += GetNumberPlanZakaz() + " - ";
            subject += db.TypeRKD_Mail_Version.Find(8).name.ToString();
        }

        protected override void GetBody(string errorText)
        {
            body += subject + "<br/>";
        }
    }
}