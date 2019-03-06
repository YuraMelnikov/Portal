namespace Wiki.Models
{
    public class EmailFirstToTPVersion : EmailVersion
    {
        public EmailFirstToTPVersion(RKD_Order rkdOrder, string recipient)
            : base(rkdOrder, recipient)
        {

        }
    }
}