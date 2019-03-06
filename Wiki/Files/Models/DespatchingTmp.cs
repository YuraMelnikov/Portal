using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Models
{
    public class DespatchingTmp
    {
        private int numberPlanZakaz;
        private string dataText;
        private DateTime date;

        public DespatchingTmp(int numberPlanZakaz, string dataText, DateTime date)
        {
            this.numberPlanZakaz = numberPlanZakaz;
            this.dataText = dataText;
            this.date = date;
        }

        public int NumberPlanZakaz
        {
            get { return numberPlanZakaz; }
        }
        public string DataText
        {
            get { return dataText; }
        }
        public DateTime Date
        {
            get { return date; }
        }
    }
}