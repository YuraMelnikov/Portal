namespace Wiki.Areas.Reclamation.Models
{
    public class TAEdit
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        Reclamation_TechnicalAdvice reclamation_TechnicalAdvice;
        ReclamationViwers reclamation;

        public TAEdit(int id)
        {
            reclamation_TechnicalAdvice = db.Reclamation_TechnicalAdvice.Find(id);
            reclamation = new ReclamationViwers(reclamation_TechnicalAdvice.Reclamation);
        }

        public Reclamation_TechnicalAdvice Reclamation_TechnicalAdvice { get => reclamation_TechnicalAdvice; set => reclamation_TechnicalAdvice = value; }
        public ReclamationViwers Reclamation { get => reclamation; set => reclamation = value; }
    }
}