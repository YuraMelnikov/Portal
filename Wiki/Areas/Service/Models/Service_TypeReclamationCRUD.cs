using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Wiki.Areas.Service.Models
{
    public class Service_TypeReclamationCRUD
    {
        private PortalKATEKEntities _db = new PortalKATEKEntities();

        public List<Service_TypeReclamation> ListAll()
        {
            return _db.Service_TypeReclamation.ToList();
        }

        public int Add(Service_TypeReclamation newTypeReclamation)
        {
            _db.Service_TypeReclamation.Add(newTypeReclamation);
            _db.SaveChanges();
            return 1;
        }

        public int Update(Service_TypeReclamation newTypeReclamation)
        {
            _db.Entry(newTypeReclamation).State = EntityState.Modified;
            _db.SaveChanges();
            return 1;
        }
    }
}