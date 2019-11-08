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

        public int BonusReversed { get => bonusReversed; set => bonusReversed = value; }
        public int Manager { get => manager; set => manager = value; }
        public int Accruedg { get => accruedg; set => accruedg = value; }
        public int Accrued { get => accrued; set => accrued = value; }
        public int BonusQuality { get => bonusQuality; set => bonusQuality = value; }
        public int Speed { get => speed; set => speed = value; }
        public int Optimization { get => optimization; set => optimization = value; }
        public int Teach { get => teach; set => teach = value; }
        public string CiliricName { get => ciliricName; set => ciliricName = value; }
    }
}