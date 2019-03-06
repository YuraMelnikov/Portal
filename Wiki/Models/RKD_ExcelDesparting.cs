using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Models
{
    public class RKD_ExcelDesparting
    {
        private int planZakaz;
        private DateTime date;
        private string text;

        public RKD_ExcelDesparting(int planZakaz, DateTime date, string text)
        {
            this.planZakaz = planZakaz;
            this.date = date;
            this.text = text;
        }

        public int PlanZakaz { get => planZakaz; set => planZakaz = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Text { get => text; set => text = value; }
    }
}