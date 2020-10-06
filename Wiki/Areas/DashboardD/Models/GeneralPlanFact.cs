namespace Wiki.Areas.DashboardD.Models
{
    public class GeneralPlanFact
    {
        int year;
        string month;
        double pSSW;
        double fSSW;
        double rSSW;
        double pPK;
        double fPK;
        double rPK;
        double pPI;
        double fPI;
        double rPI;
        double pIK;
        double fIK;
        double rIK;
        double pSSM;
        double fSSM;
        double rSSM;
        double fS1;
        double fS2;
        double pFull;
        double fFull;
        double rFull;
        double fS11;
        double fS12;

        public int Year { get => year; set => year = value; }
        public double PSSW { get => pSSW; set => pSSW = value; }
        public double FSSW { get => fSSW; set => fSSW = value; }
        public double RSSW { get => rSSW; set => rSSW = value; }
        public double PPK { get => pPK; set => pPK = value; }
        public double FPK { get => fPK; set => fPK = value; }
        public double RPK { get => rPK; set => rPK = value; }
        public double PPI { get => pPI; set => pPI = value; }
        public double FPI { get => fPI; set => fPI = value; }
        public double RPI { get => rPI; set => rPI = value; }
        public double PIK { get => pIK; set => pIK = value; }
        public double FIK { get => fIK; set => fIK = value; }
        public double RIK { get => rIK; set => rIK = value; }
        public double PSSM { get => pSSM; set => pSSM = value; }
        public double FSSM { get => fSSM; set => fSSM = value; }
        public double RSSM { get => rSSM; set => rSSM = value; }
        public double FS1 { get => fS1; set => fS1 = value; }
        public double FS11 { get => fS11; set => fS11 = value; }
        public double FS12 { get => fS12; set => fS12 = value; }
        public double FS2 { get => fS2; set => fS2 = value; }
        public double PFull { get => pFull; set => pFull = value; }
        public double FFull { get => fFull; set => fFull = value; }
        public double RFull { get => rFull; set => rFull = value; }
        public string Month { get => month; set => month = value; }
    }
}