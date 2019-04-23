using System.Collections.Generic;

namespace Wiki.Areas.Reclamation.Models
{
    public class ReclamationListViewer
    {
        readonly PortalKATEKEntities db = new PortalKATEKEntities();
        List<ReclamationViwers> reclamationsListView = new List<ReclamationViwers>();

        public List<ReclamationViwers> ReclamationsListView { get => reclamationsListView; set => reclamationsListView = value; }

        void InitializationList()
        {
            reclamationsListView = new List<ReclamationViwers>();
        }

        public void GetReclamation(int id_Devision, bool active)
        {
            InitializationList();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamation(id_Devision, active);
            foreach (var data in reclamations.Reclamations)
            {
                ReclamationViwers reclamation = new ReclamationViwers(data, id_Devision);
                ReclamationsListView.Add(reclamation);
            }
        }
        
        public void GetReclamationPlanZakaz(int id_PZ_PlanZakaz)
        {
            InitializationList();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamationPlanZakaz(id_PZ_PlanZakaz);
            foreach (var data in reclamations.Reclamations)
            {
                ReclamationViwers reclamation = new ReclamationViwers(data);
                ReclamationsListView.Add(reclamation);
            }
        }

        public void GetReclamationPlanZakaz(int id_Devision, int id_PZ_PlanZakaz)
        {
            InitializationList();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamationPlanZakaz(id_Devision, id_PZ_PlanZakaz);
            foreach (var data in reclamations.Reclamations)
            {
                ReclamationViwers reclamation = new ReclamationViwers(data, id_Devision);
                ReclamationsListView.Add(reclamation);
            }
        }
        
        public void GetReclamationPlanZakaz(int id_Devision, bool active, int id_PZ_PlanZakaz)
        {
            InitializationList();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamationPlanZakaz(id_Devision, active, id_PZ_PlanZakaz);
            foreach (var data in reclamations.Reclamations)
            {
                ReclamationViwers reclamation = new ReclamationViwers(data, id_Devision);
                ReclamationsListView.Add(reclamation);
            }
        }
    }
}