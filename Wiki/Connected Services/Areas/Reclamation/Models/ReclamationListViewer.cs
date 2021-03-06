﻿using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class ReclamationListViewer
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        ReclamationViwers[] reclamationsListView;

        public ReclamationViwers[] ReclamationsListView { get => reclamationsListView; set => reclamationsListView = value; }

        void InitializationList(int count)
        {
            reclamationsListView = new ReclamationViwers[count];
        }

        public void GetOneReclamation(int id)
        {
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetOneReclamation(id);
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[0]);
                reclamationsListView[i] = reclamation;
            }
        }

        public void GetReclamation()
        {
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

        public void GetReclamation(string login, bool active)
        {
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamation(login, active);
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
            string pzName = db.PZ_PlanZakaz.Find(id_PZ_PlanZakaz).PlanZakaz.ToString();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamationPlanZakaz(id_PZ_PlanZakaz);
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[i], pzName);
                reclamationsListView[i] = reclamation;
            }
        }

        public void GetReclamationPlanZakaz(int id_PZ_PlanZakaz, string login, bool active)
        {
            string pzName = db.PZ_PlanZakaz.Find(id_PZ_PlanZakaz).PlanZakaz.ToString();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamationPlanZakaz(login, active, id_PZ_PlanZakaz);
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[i], pzName);
                reclamationsListView[i] = reclamation;
            }
        }

        public void GetReclamationPlanZakaz(int id_Devision, int id_PZ_PlanZakaz)
        {
            string pzName = db.PZ_PlanZakaz.Find(id_PZ_PlanZakaz).PlanZakaz.ToString();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetReclamationPlanZakaz(id_Devision, id_PZ_PlanZakaz);
            int count = reclamations.Reclamations.Count;
            InitializationList(count);
            for (int i = 0; i < count; i++)
            {
                ReclamationViwers reclamation = new ReclamationViwers(reclamations.Reclamations[i], id_Devision, pzName);
                reclamationsListView[i] = reclamation;
            }
        }
    }
}