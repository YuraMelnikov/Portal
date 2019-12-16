namespace Wiki.Areas.CMKO.Types
{
    public class UserResult
    {
        string ciliricName;
        int teach;
        int optimization;
        int speed;
        int bonusQuality;
        int accrued;
        int accruedg;
        int manager;
        int bonusReversed;
        int tax;
        int rate;
        int userAccured;

        public int BonusReversed { get => bonusReversed; set => bonusReversed = value; }
        public int Manager { get => manager; set => manager = value; }
        public int Accruedg { get => accruedg; set => accruedg = value; }
        public int Accrued { get => accrued; set => accrued = value; }
        public int BonusQuality { get => bonusQuality; set => bonusQuality = value; }
        public int Speed { get => speed; set => speed = value; }
        public int Optimization { get => optimization; set => optimization = value; }
        public int Teach { get => teach; set => teach = value; }
        public string CiliricName { get => ciliricName; set => ciliricName = value; }
        public int Tax { get => tax; set => tax = value; }
        public int Rate { get => rate; set => rate = value; }
        public int UserAccured { get => userAccured; set => userAccured = value; }
    }
}