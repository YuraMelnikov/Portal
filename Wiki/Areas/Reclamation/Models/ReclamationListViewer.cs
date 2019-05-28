using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public struct ReclamationListViewer
    {
        PortalKATEKEntities db;
        ReclamationViwers[] reclamationsListView;

        public ReclamationViwers[] ReclamationsListView { get => reclamationsListView; set => reclamationsListView = value; }

        void InitializationList(int count)
        {
            db = new PortalKATEKEntities();
            reclamationsListView = new ReclamationViwers[count];
        }

        public void GetReclamation()
        {
            db = new PortalKATEKEntities();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamation();
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[0]);
                reclamationsListView[i] = reclamation;
            }
        }

        public void GetReclamation(string login)
        {
            db = new PortalKATEKEntities();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamation(login);
            int id_Devision = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[i], id_Devision);
                reclamationsListView[i] = reclamation;
            }
        }

        public void GetReclamation(int id_Devision)
        {
            db = new PortalKATEKEntities();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamation(id_Devision);
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[i], id_Devision);
                reclamationsListView[i] = reclamation;
            }
        }

        public void GetReclamation(int id_Devision, bool active)
        {
            db = new PortalKATEKEntities();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamation(id_Devision, active);
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[i], id_Devision);
                reclamationsListView[i] = reclamation;
            }
        }

        public void GetReclamation(int id_Devision, bool active, string login)
        {
            db = new PortalKATEKEntities();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamation(id_Devision, active, login);
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[i], id_Devision);
                reclamationsListView[i] = reclamation;
            }
        }

        public void GetReclamationPlanZakaz(int id_PZ_PlanZakaz)
        {
            db = new PortalKATEKEntities();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamationPlanZakaz(id_PZ_PlanZakaz);
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[i]);
                reclamationsListView[i] = reclamation;
            }
        }

        public void GetReclamationPlanZakaz(int id_Devision, int id_PZ_PlanZakaz)
        {
            db = new PortalKATEKEntities();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamationPlanZakaz(id_Devision, id_PZ_PlanZakaz);
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[i], id_Devision);
                reclamationsListView[i] = reclamation;
            }
        }
    }
}