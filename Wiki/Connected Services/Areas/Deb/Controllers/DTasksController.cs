using Newtonsoft.Json;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.Deb.Controllers
{
    public class DTasksController : Controller
    {
        [Authorize(Roles = "Admin, OPTP, OP, Fin director")]
        public ActionResult Index()
        {
            return View();
        }

        public string RenderUserMenu()
        {
            string login = "Войти";
            try
            {
                if (HttpContext.User.Identity.Name != "")
                    login = HttpContext.User.Identity.Name;
            }
            catch
            {
                login = "Войти";
            }
            return login;
        }

        public JsonResult List()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                    .Where(d => d.close == false)
                    .Where(d => d.id_TaskForPZ == 28)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.PZ_PlanZakaz.PlanZakaz,
                    dataList.PZ_PlanZakaz.Name,
                    Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                    Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, settings).Replace(@"""", ""),
                    DateSupply = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.DateSupply, settings).Replace(@"""", "")
                });
                return Json(new { data });
            }
        }
    }
}