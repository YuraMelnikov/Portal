namespace Wiki.Models
{
    public class OTKReclamationKoReportQuart
    {
        public string userName;
        public double countError;
        public string departament;

        public OTKReclamationKoReportQuart(string userName, double countError, string departament)
        {
            this.userName = userName;
            this.countError = countError;
            this.departament = departament;
        }
    }

}