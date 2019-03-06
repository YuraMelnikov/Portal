namespace Wiki.Models
{
    public class Receivables
    {
        Debit_WorkBit debit_WorkBit;
        tmpRecl recl;

        public Receivables(Debit_WorkBit debit_WorkBit)
        {
            this.debit_WorkBit = debit_WorkBit;
        }

        public Receivables(Debit_WorkBit debit_WorkBit, tmpRecl recl)
        {
            this.debit_WorkBit = debit_WorkBit;
            this.recl = recl;
        }

        public Debit_WorkBit Debit_WorkBit { get => debit_WorkBit; set => debit_WorkBit = value; }
        public tmpRecl Recl { get => recl; set => recl = value; }
    }
}