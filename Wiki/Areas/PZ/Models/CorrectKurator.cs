namespace Wiki.Areas.PZ.Models
{
    public class CorrectKurator
    {
        PZ_FIO kurator;

        public CorrectKurator(PZ_FIO kurator)
        {
            this.kurator = kurator;
            if (kurator.id_PZ_Client == 0)
                kurator.id_PZ_Client = 65;
            if (kurator.i == null)
                kurator.i = "";
            if (kurator.o == null)
                kurator.o = "";
            if (kurator.email == null)
                kurator.email = "";
            if (kurator.phone == null)
                kurator.phone = "";
            if (kurator.position == null)
                kurator.position = "";
        }

        public PZ_FIO Kurator { get => kurator; set => kurator = value; }
    }
}