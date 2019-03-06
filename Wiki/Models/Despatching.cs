using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Models
{
    public class Despatching
    {
        private int numberPlanZakaz;
        private string dataText;

        public Despatching (int numberPlanZakaz, string dataText)
        {
            this.numberPlanZakaz = numberPlanZakaz;
            this.dataText = dataText;
        }

        public int NumberPlanZakaz
        {
            get { return numberPlanZakaz; }
        }
        public string DataText
        {
            get { return dataText; }
        }
    }
}