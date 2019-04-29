using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class PlanZakazViwers
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
        int reclamationClose;
        string openLinkJS;

        public int Id_PZ_PlanZakaz { get => id_PZ_PlanZakaz; set => id_PZ_PlanZakaz = value; }
        public int PlanZakaz { get => planZakaz; set => planZakaz = value; }
        public string ContractName { get => contractName; set => contractName = value; }
        public string TuName { get => tuName; set => tuName = value; }
        public string Client { get => client; set => client = value; }
        public string Mtr { get => mtr; set => mtr = value; }
        public string Ol { get => ol; set => ol = value; }
        public int ReclamationCount { get => reclamationCount; set => reclamationCount = value; }
        public int ReclamationActive { get => reclamationActive; set => reclamationActive = value; }
        public int ReclamationClose { get => reclamationClose; set => reclamationClose = value; }
        public string OpenLinkJS { get => openLinkJS; set => openLinkJS = value; }

        public PlanZakazViwers(PZ_PlanZakaz pZ_PlanZakaz)
        {
            Id_PZ_PlanZakaz = pZ_PlanZakaz.Id;
            PlanZakaz = pZ_PlanZakaz.PlanZakaz;
            ContractName = pZ_PlanZakaz.Name;
            TuName = pZ_PlanZakaz.nameTU;
            Client = pZ_PlanZakaz.PZ_Client.NameSort;
            Mtr = pZ_PlanZakaz.MTR;
            Ol = pZ_PlanZakaz.OL;
            ReclamationCount = pZ_PlanZakaz.Reclamation_PZ.Count;
            ReclamationActive = pZ_PlanZakaz.Reclamation_PZ.Where(d => d.Reclamation.close == false).Count();
            ReclamationClose = pZ_PlanZakaz.Reclamation_PZ.Where(d => d.Reclamation.close == true).Count();
            OpenLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getID('" + pZ_PlanZakaz.Id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
        }

        public PlanZakazViwers(PZ_PlanZakaz pZ_PlanZakaz, int id_Devision)
        {
            Id_PZ_PlanZakaz = pZ_PlanZakaz.Id;
            PlanZakaz = pZ_PlanZakaz.PlanZakaz;
            ContractName = pZ_PlanZakaz.Name;
            TuName = pZ_PlanZakaz.nameTU;
            Client = pZ_PlanZakaz.PZ_Client.NameSort;
            Mtr = pZ_PlanZakaz.MTR;
            Ol = pZ_PlanZakaz.OL;
            ReclamationCount = GetReclamationCount(pZ_PlanZakaz, id_Devision);
            ReclamationActive = GetReclamationActive(pZ_PlanZakaz, id_Devision, false);
            ReclamationClose = GetReclamationActive(pZ_PlanZakaz, id_Devision, true);
        }

        int GetReclamationCount(PZ_PlanZakaz pZ_PlanZakaz, int id_Devision)
        {
            int count = 0;
            if (id_Devision == 6)
                count = pZ_PlanZakaz.Reclamation_PZ.Where(d => d.Reclamation.id_DevisionCreate == 6).Count();
            else if (id_Devision == 3 || id_Devision == 16)
                count = pZ_PlanZakaz.Reclamation_PZ.Where(d => d.Reclamation.id_DevisionReclamation == 3 || d.Reclamation.id_DevisionReclamation == 16).Count();
            else if (id_Devision == 0)
                count = pZ_PlanZakaz.Reclamation_PZ.Count();
            else
                count = pZ_PlanZakaz.Reclamation_PZ.Where(d => d.Reclamation.id_DevisionReclamation == id_Devision).Count();
            return count;
        }

        int GetReclamationActive(PZ_PlanZakaz pZ_PlanZakaz, int id_Devision, bool close)
        {
            int count = 0;
            if (id_Devision == 6)
                count = pZ_PlanZakaz.Reclamation_PZ
                    .Where(d => d.Reclamation.id_DevisionCreate == 6)
                    .Where(d => d.Reclamation.close == close)
                    .Count();
            else if (id_Devision == 3 || id_Devision == 16)
                count = pZ_PlanZakaz.Reclamation_PZ
                    .Where(d => d.Reclamation.id_DevisionReclamation == 3 || d.Reclamation.id_DevisionReclamation == 16)
                    .Where(d => d.Reclamation.close == close)
                    .Count();
            else if (id_Devision == 0)
                count = pZ_PlanZakaz.Reclamation_PZ
                    .Where(d => d.Reclamation.close == close)
                    .Count();
            else
                count = pZ_PlanZakaz.Reclamation_PZ
                    .Where(d => d.Reclamation.id_DevisionReclamation == id_Devision)
                    .Where(d => d.Reclamation.close == close)
                    .Count();
            return count;
        }
    }
}