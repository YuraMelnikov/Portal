using Newtonsoft.Json;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.Deb.Controllers
{
    public class ArchiveReportController : Controller
    {
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly string firstPartLinkEditOP = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('";
        readonly string lastPartEdit = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";

        [Authorize(Roles = "Admin, OPTP, OP, Fin director")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_PeriodReportOprih
                              .AsNoTracking()
                              .OrderBy(d => d.dateTimeCreate)
                              .ToList();
                var data = query.Select(dataList => new
                {
                    id = firstPartLinkEditOP + dataList.id + lastPartEdit,
                    dataList.period,
                    dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settings).Replace(@"""", ""),
                    dateTimeClose = JsonConvert.SerializeObject(dataList.dateTimeClose, settings).Replace(@"""", ""),
                    countClose = GetCountCloseOrders(dataList.id),
                    countOpen = GetCountNoCloseOrders(dataList.id),
                    dataList.close
                });
                return Json(new { data });
            }
        }

        public JsonResult GetId(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_DataReportOprih
                    .AsNoTracking()
                    .Where(d => d.id_Debit_PeriodReportOprih == id)
                    .Include(d => d.Debit_WorkBit.PZ_PlanZakaz.PZ_Client)
                    .Include(d => d.Debit_WorkBit.PZ_PlanZakaz.AspNetUsers)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.description,
                    dataList.oprihClose,
                    dateOprihPlanFact = JsonConvert.SerializeObject(dataList.dateOprihPlanFact, settings).Replace(@"""", ""),
                    dataList.Debit_WorkBit.PZ_PlanZakaz.PlanZakaz,
                    dataList.Debit_WorkBit.PZ_PlanZakaz.Name,
                    Manager = dataList.Debit_WorkBit.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                    Client = dataList.Debit_WorkBit.PZ_PlanZakaz.PZ_Client.NameSort,
                    dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.Debit_WorkBit.PZ_PlanZakaz.dataOtgruzkiBP, settings).Replace(@"""", ""),
                    DateSupply = JsonConvert.SerializeObject(dataList.Debit_WorkBit.PZ_PlanZakaz.DateSupply, settings).Replace(@"""", ""),
                    dataList.numberSF,
                    dataList.reclamation,
                    openReclamation = JsonConvert.SerializeObject(dataList.reclamationOpen, settings).Replace(@"""", ""),
                    closeReclamation = JsonConvert.SerializeObject(dataList.reclamationClose, settings).Replace(@"""", "")
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        int GetCountCloseOrders(int id_Debit_PeriodReportOprih)
        {
            int count = 0;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                count = db.Debit_DataReportOprih
                    .Where(d => d.id_Debit_PeriodReportOprih == id_Debit_PeriodReportOprih)
                    .Where(d => d.oprihClose == true)
                    .Count();
            }
            return count;
        }

        int GetCountNoCloseOrders(int id_Debit_PeriodReportOprih)
        {
            int count = 0;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                count = db.Debit_DataReportOprih
                    .Where(d => d.id_Debit_PeriodReportOprih == id_Debit_PeriodReportOprih)
                    .Where(d => d.oprihClose == false)
                    .Count();
            }
            return count;
        }
    }
}