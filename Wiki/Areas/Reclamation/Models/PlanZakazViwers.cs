namespace Wiki.Areas.Reclamation.Models
{
    public struct PlanZakazViwers
    {
        int id_PZ_PlanZakaz;
        int planZakaz;
        string contractName;
        string tuName;
        string client;
        string mtr;
        string ol;
        int reclamationCount;
        int reclamationActive;
        int reclamationNotAnswer;
        int reclamationClose;

        public int Id_PZ_PlanZakaz { get => id_PZ_PlanZakaz; set => id_PZ_PlanZakaz = value; }
        public int PlanZakaz { get => planZakaz; set => planZakaz = value; }
        public string ContractName { get => contractName; set => contractName = value; }
        public string TuName { get => tuName; set => tuName = value; }
        public string Client { get => client; set => client = value; }
        public string Mtr { get => mtr; set => mtr = value; }
        public string Ol { get => ol; set => ol = value; }
        public int ReclamationCount { get => reclamationCount; set => reclamationCount = value; }
        public int ReclamationActive { get => reclamationActive; set => reclamationActive = value; }
        public int ReclamationNotAnswer { get => reclamationNotAnswer; set => reclamationNotAnswer = value; }
        public int ReclamationClose { get => reclamationClose; set => reclamationClose = value; }
    }
}