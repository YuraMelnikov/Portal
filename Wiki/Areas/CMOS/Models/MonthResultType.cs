namespace Wiki.Areas.CMOS.Models
{
    public class MonthResultType : MonthResult
    {
        public MonthResultType(string period, string customer, string type) : base(period, customer)
        {
            Type = type;
        }
        public string Type { get; set; }
    }
}