namespace Wiki.Models
{
    public class EmailToTPVersion : EmailVersion
    {
        public EmailToTPVersion(RKD_Order rkdOrder, string recipient)
            : base(rkdOrder, recipient)
        {

        }

        protected override void GetSubject()
        {
            subject += GetNumberPlanZakaz() + " - ";
            subject += db.TypeRKD_Mail_Version.Find(3).name.ToString();
        }
    }
}