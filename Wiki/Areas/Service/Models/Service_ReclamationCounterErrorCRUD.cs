using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Wiki.Areas.Service.Models
{
    public class Service_ReclamationCounterErrorCRUD
    {
        private PortalKATEKEntities _db = new PortalKATEKEntities();

        public List<Service_ReclamationCounterError> ListAll()
        {
            return _db.Service_ReclamationCounterError.ToList();
        }

        public int Add(Service_ReclamationCounterError newTypeReclamation)
        {
            _db.Service_ReclamationCounterError.Add(newTypeReclamation);
            _db.SaveChanges();
            return 1;
        }

        public int Update(Service_ReclamationCounterError newTypeReclamation)
        {
            _db.Entry(newTypeReclamation).State = EntityState.Modified;
            _db.SaveChanges();
            return 1;
        }
    }
}