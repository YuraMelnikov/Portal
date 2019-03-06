using System.Linq;

namespace Wiki.Models
{
    public class RenderUserMenu
    {
        string userName = "Необходимо войти под учетной записью";
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public RenderUserMenu(string identitiUserName)
        {
            if(identitiUserName != null || identitiUserName != "")
                userName = db.AspNetUsers.First(d => d.Email == identitiUserName).CiliricalName;
        }

        public string UserName { get; }
    }
}