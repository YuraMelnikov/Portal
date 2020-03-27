namespace Wiki.Areas.DashboardD.Models
{
    public class GeneralPercentCustomer
    {
        string customer;
        double percent;

        public string Customer { get => customer; set => customer = value; }
        public double Percent { get => percent; set => percent = value; }
    }
}