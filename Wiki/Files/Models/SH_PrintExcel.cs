using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Models
{
    public class SH_PrintExcel
    {
        public string order;
        public string count;
        public string classicCount;
        public string rubka;
        public string razmetka;
        public string probivka;
        public string gibka;
        public string lujenie;
        public string result;
        public string designation;
        public string name;
        public string iA;
        public string sechenie;
        public string razvertka;
        public string eGib;
        public string dForm;
        public string aMaterial;
        public string bSechenie;
        public string cPrimenyaemost;
        public string countHole;

        //public string Order { get; set; }
        //public string Count { get; set; }
        //public string ClassicCount { get; set; }
        //public string Rubka { get; set; }
        //public string Razmetka { get; set; }
        //public string Probivka { get; set; }
        //public string Gibka { get; set; }
        //public string Lujenie { get; set; }
        //public string Result { get; set; }
        //public string Designation { get; set; }
        //public string Name { get; set; }
        //public string IA { get; set; }
        //public string Sechenie { get; set; }
        //public string Razvertka { get; set; }
        //public string EGib { get; set; }
        //public string DForm { get; set; }
        //public string AMaterial { get; set; }
        //public string BSechenie { get; set; }
        //public string CPrimenyaemost { get; set; }
        //public string CountHole { get; set; }

        public SH_PrintExcel (string order, string count, string classicCount, string rubka, string razmetka, string probivka,
                             string gibka, string lujenie, string result, string designation, string name, string iA, string sechenie,
                             string razvertka, string eGib, string dForm, string aMaterial, string bSechenie, string cPrimenyaemost, 
                             string countHole)
        {
            this.order = order;
            this.count = count;
            this.classicCount = classicCount;
            this.rubka = rubka;
            this.razmetka = razmetka;
            this.probivka = probivka;
            this.gibka = gibka;
            this.lujenie = lujenie;
            this.result = result;
            this.designation = designation;
            this.name = name;
            this.iA = iA;
            this.sechenie = sechenie;
            this.razvertka = razvertka;
            this.eGib = eGib;
            this.dForm = dForm;
            this.aMaterial = aMaterial;
            this.bSechenie = bSechenie;
            this.cPrimenyaemost = cPrimenyaemost;
            this.countHole = countHole;
    }
    }
}