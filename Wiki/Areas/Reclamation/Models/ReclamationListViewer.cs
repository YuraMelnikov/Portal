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
    }
}