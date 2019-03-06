using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Wiki.Areas.Service.Models
{
    public class Service_TypeReclamationDocumentCRUD
    {
        private PortalKATEKEntities _db = new PortalKATEKEntities();

        public List<Service_TypeReclamationDocument> ListAll()
        {
            return _db.Service_TypeReclamationDocument.ToList();
        }

        public int Add(Service_TypeReclamationDocument newTypeReclamation)
        {
            _db.Service_TypeReclamationDocument.Add(newTypeReclamation);
            _db.SaveChanges();
            return 1;
        }

        public int Update(Service_TypeReclamationDocument newTypeReclamation)
        {
            _db.Entry(newTypeReclamation).State = EntityState.Modified;
            _db.SaveChanges();
            return 1;
        }
    }
}