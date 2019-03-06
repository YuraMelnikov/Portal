namespace Wiki.Models
{
    public class ManufacturingHoure
    {
        int sp;
        string date;
        string order;
        string work;
        float houre;
        string user;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public string Order
        {
            get { return order; }
            set { order = value; }
        }

        public string Work
        {
            get { return work; }
            set { work = value; }
        }

        public float Houre
        {
            get { return houre; }
            set { houre = value; }
        }

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public int Sp
        {
            get { return sp; }
            set { sp = value; }
        }
    }
}