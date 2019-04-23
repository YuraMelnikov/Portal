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

        public void GetActiveReclamation(int id_Devision)
        {
            InitializationList();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetActiveReclamation(id_Devision);
            foreach (var data in reclamations.Reclamations)
            {
                ReclamationViwers reclamation = new ReclamationViwers(data);
                ReclamationsListView.Add(reclamation);
            }
        }
    }
}