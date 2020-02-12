using System;

namespace Wiki.Areas.VisualizationBP.Models
{
    public class CMOOrder
    {
        string position;
        string customer;
        DateTime? dateOpen;
        DateTime? criticalDate;
        DateTime? dateComplited;
        string orderNumber;
        string state;

        public CMOOrder(string position, string customer, DateTime? dateOpen, DateTime? criticalDate, DateTime? dateComplited, string orderNumber, string state)
        {
            Position = position;
            Customer = customer;
            DateOpen = dateOpen;
            CriticalDate = criticalDate;
            DateComplited = dateComplited;
            OrderNumber = orderNumber;
            State = state;
        }

        public string Position { get => position; set => position = value; }
        public string Customer { get => customer; set => customer = value; }
        public DateTime? DateOpen { get => dateOpen; set => dateOpen = value; }
        public DateTime? CriticalDate { get => criticalDate; set => criticalDate = value; }
        public DateTime? DateComplited { get => dateComplited; set => dateComplited = value; }
        public string OrderNumber { get => orderNumber; set => orderNumber = value; }
        public string State { get => state; set => state = value; }
    }
}