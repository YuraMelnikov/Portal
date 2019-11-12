using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class TARemarksListView
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();
        private List<TARemarkView> tARemarkViews;

        public List<TARemarkView> TARemarkViews { get => tARemarkViews; set => tARemarkViews = value; }

        public List<TARemarkView> GetActiveTA()
        {
            InitializationList();
            foreach (var data in db.Reclamation_TechnicalAdvice.Where(d => d.Reclamation_TechnicalAdviceProtocolPosition.Count == 0).ToList())
            {
                TARemarkViews.Add(new TARemarkView(data));
            }
            return tARemarkViews;
        }

        public List<TARemarkView> GetAllRemarks()
        {
            InitializationList();
            foreach (var data in db.Reclamation_TechnicalAdvice.Where(d => d.dateTimeClose != null).ToList())
            {
                TARemarkViews.Add(new TARemarkView(data));
            }
            return tARemarkViews;
        }

        public List<TARemarkView> GetRemarksOTK()
        {
            InitializationList();
            foreach (var data in db.Reclamation.Where(d => d.fixedExpert == false && d.id_DevisionCreate == 6).ToList())
            {
                TARemarkViews.Add(new TARemarkView(data));
            }
            return tARemarkViews;
        }

        public List<TARemarkView> GetRemarksPO()
        {
            InitializationList();
            foreach (var data in db.Reclamation.Where(d => d.fixedExpert == false && d.id_DevisionCreate != 6).ToList())
            {
                TARemarkViews.Add(new TARemarkView(data));
            }
            return tARemarkViews;
        }

        public List<TARemarkView> GetRemarksNull()
        {
            InitializationList();
            return tARemarkViews;
        }

        public List<TARemarkView> GetRemarksProtocol(int id_protocol)
        {
            InitializationList();
            foreach (var data in db.Reclamation_TechnicalAdvice
                .Where(d => d.Reclamation_TechnicalAdviceProtocolPosition.FirstOrDefault().id_Reclamation_TechnicalAdviceProtocol == id_protocol)
                .ToList())
            {
                TARemarkViews.Add(new TARemarkView(data));
            }
            return tARemarkViews;
        }

        private bool InitializationList()
        {
            tARemarkViews = new List<TARemarkView>();
            return true;
        }

        public List<TARemarkView> GetNoCloseTA()
        {
            InitializationList();
            foreach (var data in db.Reclamation_TechnicalAdvice.Where(d => d.close == false && d.dateTimeClose != null).ToList())
            {
                TARemarkViews.Add(new TARemarkView(data));
            }
            return tARemarkViews;
        }
    }
}