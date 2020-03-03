using System;

namespace Wiki.Areas.PZ.Models
{
    public class Order
    {
        private string name;
        private DateTime start;
        private DateTime? finish;
        private double sale;
        private string curency;

        public Order(string name, DateTime start, DateTime? finish, double sale, string curency)
        {
            Name = name;
            Start = start;
            Finish = finish;
            Sale = sale;
            Curency = curency;
        }

        public string Name { get => name; set => name = value; }
        public DateTime Start { get => start; set => start = value; }
        public DateTime? Finish { get => finish; set => finish = value; }
        public double Sale { get => sale; set => sale = value; }
        public string Curency { get => curency; set => curency = value; }
    }
}