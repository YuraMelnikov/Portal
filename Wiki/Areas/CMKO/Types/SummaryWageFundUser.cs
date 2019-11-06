namespace Wiki.Areas.CMKO.Types
{
    public class SummaryWageFundUser : SummaryWageFund
    {
        string fullName;

        public string FullName { get => fullName; set => fullName = value; }
    }
}