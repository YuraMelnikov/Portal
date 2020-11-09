namespace Wiki.Areas.CMOS.Models
{
    public class MonthResult
    {
        public string Period { get; set; }
        public string Customer { get; set; }
        public double Weight { get; set; }
        public double Cost { get; set; }

        public MonthResult(string period, string customer)
        {
            Period = period;
            Weight = 0.0;
            Cost = 0.0;
            Customer = customer;
        }
    }
}