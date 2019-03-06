using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Wiki.Areas.Service.Models
{
    public class Service_ReclamationWhoAddCRUD
    {
        private PortalKATEKEntities _db = new PortalKATEKEntities();

        public List<Service_ReclamationWhoAdd> ListAll()
        {
            return _db.Service_ReclamationWhoAdd.ToList();
        }

        public int Add(Service_ReclamationWhoAdd newTypeReclamation)
        {
            _db.Service_ReclamationWhoAdd.Add(newTypeReclamation);
            _db.SaveChanges();
            return 1;
        }

        public int Update(Service_ReclamationWhoAdd newTypeReclamation)
        {
            _db.Entry(newTypeReclamation).State = EntityState.Modified;
            _db.SaveChanges();
            return 1;
        }
    }
}