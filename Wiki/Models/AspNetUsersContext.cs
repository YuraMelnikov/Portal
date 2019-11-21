namespace Wiki.Models
{
    public class AspNetUsersContext
    {
        public string GetCiliricalName(AspNetUsers aspNetUsers)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    return aspNetUsers.CiliricalName;
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}