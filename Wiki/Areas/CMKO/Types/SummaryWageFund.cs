namespace Wiki.Areas.CMKO.Types
{
    public class SummaryWageFund
    {
        int devisionId;
        int plan;
        int fact;

        public int DevisionId { get => devisionId; set => devisionId = value; }
        public int Plan { get => plan; set => plan = value; }
        public int Fact { get => fact; set => fact = value; }
    }
}