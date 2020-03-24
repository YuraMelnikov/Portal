namespace Wiki.Areas.DashboardD.Models
{
    public class GeneralPlanFact
    {
        int year;
        string month;
        int pSSW;
        int fSSW;
        int rSSW;
        int pPK;
        int fPK;
        int rPK;
        int pPI;
        int fPI;
        int rPI;
        int pIK;
        int fIK;
        int rIK;
        int pSSM;
        int fSSM;
        int rSSM;
        int fS1;
        int fS2;
        int pFull;
        int fFull;
        int rFull;

        public int Year { get => year; set => year = value; }
        public int PSSW { get => pSSW; set => pSSW = value; }
        public int FSSW { get => fSSW; set => fSSW = value; }
        public int RSSW { get => rSSW; set => rSSW = value; }
        public int PPK { get => pPK; set => pPK = value; }
        public int FPK { get => fPK; set => fPK = value; }
        public int RPK { get => rPK; set => rPK = value; }
        public int PPI { get => pPI; set => pPI = value; }
        public int FPI { get => fPI; set => fPI = value; }
        public int RPI { get => rPI; set => rPI = value; }
        public int PIK { get => pIK; set => pIK = value; }
        public int FIK { get => fIK; set => fIK = value; }
        public int RIK { get => rIK; set => rIK = value; }
        public int PSSM { get => pSSM; set => pSSM = value; }
        public int FSSM { get => fSSM; set => fSSM = value; }
        public int RSSM { get => rSSM; set => rSSM = value; }
        public int FS1 { get => fS1; set => fS1 = value; }
        public int FS2 { get => fS2; set => fS2 = value; }
        public int PFull { get => pFull; set => pFull = value; }
        public int FFull { get => fFull; set => fFull = value; }
        public int RFull { get => rFull; set => rFull = value; }
        public string Month { get => month; set => month = value; }
    }
}