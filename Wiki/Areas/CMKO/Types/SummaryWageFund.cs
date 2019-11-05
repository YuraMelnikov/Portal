namespace Wiki.Areas.CMKO.Types
{
    public class SummaryWageFund
    {
        int[] devisionId;
        int[] plan;
        int[] fact;

        public int[] DevisionId { get => devisionId; set => devisionId = value; }
        public int[] Plan { get => plan; set => plan = value; }
        public int[] Fact { get => fact; set => fact = value; }

        public SummaryWageFund()
        {
            devisionId = new int[3];
            for(int i = 0; i < 3; i++)
            {
                devisionId[i] = new int();
            }
            plan = new int[3];
            for (int i = 0; i < 3; i++)
            {
                plan[i] = new int();
            }
            fact = new int[3];
            for (int i = 0; i < 3; i++)
            {
                fact[i] = new int();
            }
        }
    }
}