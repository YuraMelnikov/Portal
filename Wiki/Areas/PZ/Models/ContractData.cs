using System;

namespace Wiki.Areas.PZ.Models
{
    public class ContractData
    {
        private string client;
        private string number;
        private string numberCh;
        private DateTime date;
        private DateTime? dateCh;
        private Order[] orders;
        private DateTime orderBy;

        public ContractData(string client, string number, string numberCh, DateTime date, DateTime? dateCh)
        {
            Client = client;
            Number = number;
            NumberCh = numberCh;
            Date = date;
            DateCh = dateCh;
            if (dateCh != null)
            {
                OrderBy = dateCh.Value;
            }
            else
            {
                OrderBy = date;
            }
        }

        public string Client { get => client; set => client = value; }
        public string Number { get => number; set => number = value; }
        public string NumberCh { get => numberCh; set => numberCh = value; }
        public DateTime Date { get => date; set => date = value; }
        public DateTime? DateCh { get => dateCh; set => dateCh = value; }
        public Order[] Orders { get => orders; set => orders = value; }
        public DateTime OrderBy { get => orderBy; set => orderBy = value; }
    }
}