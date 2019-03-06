namespace Wiki.Models
{
    public class TenderOffer
    {
        public TenderOffer()
        {

        }

        public TenderOffer(int id_CMO_Order, int id_CMO_Company, double cost, int duration)
        {
            Id_CMO_Order = id_CMO_Order;
            Id_CMO_Company = id_CMO_Company;
            Cost = cost;
            Duration = duration;
        }

        public int Id_CMO_Order { get; set; }
        public int Id_CMO_Company { get; set; }
        public double Cost { get; set; }
        public int Duration { get; set; }
    }
}