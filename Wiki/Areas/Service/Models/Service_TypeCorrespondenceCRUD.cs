using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Wiki.Areas.Service.Models
{
    public class Service_TypeCorrespondenceCRUD
    {
        private PortalKATEKEntities _db = new PortalKATEKEntities();

        public List<Service_TypeCorrespondence> ListAll()
        {
            return _db.Service_TypeCorrespondence.ToList();
        }

        public int Add(Service_TypeCorrespondence newTypeReclamation)
        {
            _db.Service_TypeCorrespondence.Add(newTypeReclamation);
            _db.SaveChanges();
            return 1;
        }

        public int Update(Service_TypeCorrespondence newTypeReclamation)
        {
            _db.Entry(newTypeReclamation).State = EntityState.Modified;
            _db.SaveChanges();
            return 1;
        }
    }
}