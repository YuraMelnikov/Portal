using Newtonsoft.Json;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.Deb.Controllers
{
    public class DSetupController : Controller
    {
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.dd.MM" };

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var query = db.PZ_Setup
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                    .Where(d => d.PZ_PlanZakaz.DateCreate.Year > 2017)
                    .Where(d => d.PZ_PlanZakaz.Client != 39)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.PZ_PlanZakaz.PlanZakaz,
                    Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                    Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    dataList.KolVoDneyNaPrijemku,
                    dataList.PunktDogovoraOSrokahPriemki,
                    dataList.UslovieOplatyText
                });
                return Json(new { data });
            }
        }
    }
}