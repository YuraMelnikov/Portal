namespace Wiki.Models
{
    public class EmailRevisionToKO : EmailVersion
    {
        public EmailRevisionToKO(RKD_Order rkdOrder, string recipient)
            : base(rkdOrder, recipient)
        {
            
        }

        protected override void GetSubject()
        {
            subject += GetNumberPlanZakaz() + " - ";
            subject += db.TypeRKD_Mail_Version.Find(2).name.ToString();
        }

        protected override void GetBody(string errorText)
        {
            body = subject + "<br/>" + "Прим.:" + errorText;
        }
    }
}