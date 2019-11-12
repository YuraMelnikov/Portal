namespace Wiki.Areas.CMKO.Types
{
    public class CoefUserView
    {
        string fullName;
        double data;

        public string FullName { get => fullName; set => fullName = value; }
        public double Data { get => data; set => data = value; }

        public CoefUserView(string fullName, double data)
        {
            this.fullName = fullName;
            this.data = data;
        }
    }
}