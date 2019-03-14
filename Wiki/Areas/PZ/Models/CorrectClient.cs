namespace Wiki.Areas.PZ.Models
{
    public class CorrectClient
    {
        PZ_Client pZ_Client;

        public CorrectClient(PZ_Client pZ_Client)
        {
            this.pZ_Client = pZ_Client;
            if (pZ_Client.GCompany == 0)
                pZ_Client.GCompany = 65;
            if (pZ_Client.DCCompany == 0)
                pZ_Client.DCCompany = 65;
        }

        public PZ_Client PZ_Client { get => pZ_Client; set => pZ_Client = value; }
    }
}