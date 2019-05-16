using System.Collections.Generic;
using System.Linq;

namespace Wiki.Models
{
    public class DevisionsManufacturing
    {
        List<Devision> devisions;
        PortalKATEKEntities db = new PortalKATEKEntities();

        public DevisionsManufacturing()
        {
            devisions = db.Devision.Where(d => d.id == 8 || d.id == 9 || d.id == 10 || d.id == 20 || d.id == 22 || d.id == 27).ToList();
        }

        public List<Devision> Devisions { get => devisions; set => devisions = value; }
    }
}