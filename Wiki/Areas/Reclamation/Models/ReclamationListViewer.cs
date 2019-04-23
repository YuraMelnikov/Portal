using System.Collections.Generic;

namespace Wiki.Areas.Reclamation.Models
{
    public class ReclamationListViewer
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        List<ReclamationViwers> reclamationsListView = new List<ReclamationViwers>();

        public List<ReclamationViwers> ReclamationsListView { get => reclamationsListView; set => reclamationsListView = value; }

        public void GetActiveReclamationOTK()
        {
            InitializationList();
            ReclamationsList reclamations = new ReclamationsList();
            reclamations.GetActiveReclamationOTK();
            foreach (var data in reclamations.Reclamations)
            {
                ReclamationViwers reclamation = new ReclamationViwers(data);
                ReclamationsListView.Add(reclamation);
            }
        }

        void InitializationList()
        {
            reclamationsListView = new List<ReclamationViwers>();
        }
    }
}