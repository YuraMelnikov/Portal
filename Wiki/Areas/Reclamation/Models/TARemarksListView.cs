using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class TARemarksListView
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        List<TARemarkView> tARemarkViews;

        public List<TARemarkView> TARemarkViews { get => tARemarkViews; set => tARemarkViews = value; }

        public List<TARemarkView> GetActiveTA()
        {
            InitializationList();
            foreach(var data in db.Reclamation_TechnicalAdvice.Where(d => d.Reclamation_TechnicalAdviceProtocolPosition.Count == 0).ToList())
            {
                TARemarkViews.Add(new TARemarkView(data));
            }
            return tARemarkViews;
        }

        public List<TARemarkView> GetAllRemarks()
        {
            InitializationList();
            foreach (var data in db.Reclamation_TechnicalAdvice.ToList())
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

        bool InitializationList()
        {
            tARemarkViews = new List<TARemarkView>();
            return true;
        }
    }
}