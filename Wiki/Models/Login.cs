using System.Linq;

namespace Wiki.Models
{
    public struct Login
    {
        PortalKATEKEntities db;

        public string LoginEmailToId(string login)
        {
            try
            {
                Initialization();
                return db.AspNetUsers.First(d => d.Email == login).Id;
            }
            catch
            {
                return "";
            }
        }

        public string LoginEmailToCiliricalName(string login)
        {
            try
            {
                Initialization();
                return db.AspNetUsers.First(d => d.Email == login).CiliricalName;
            }
            catch
            {
                return "";
            }
        }

        void Initialization()
        {
            db = new PortalKATEKEntities();
        }
    }
}