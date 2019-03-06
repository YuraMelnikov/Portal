using System;
using System.Linq;

namespace Wiki.Areas.Service.Models
{
    public class ViewReclamationAll
    {
        private int id;
        private string pzs;
        private DateTime dateAdd;
        private string description;

        public ViewReclamationAll()
        {

        }

        public ViewReclamationAll(Service_Reclamation reclamation)
        {
            this.id = reclamation.id;
            this.dateAdd = reclamation.dateAdd;
            this.description = reclamation.description;
            this.pzs = "";
            foreach (var data in reclamation.Service_ReclamationPZ.ToList())
            {
                this.pzs += data.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
        }
    }
}