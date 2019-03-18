using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.Deb.Controllers
{
    public class ArchiveReportController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly string firstPartLinkEditOP = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('";
        readonly string lastPartEdit = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";

        [Authorize(Roles = "Admin, OPTP, OP, Fin director")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var query = db.Debit_PeriodReportOprih
                .OrderBy(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                id = firstPartLinkEditOP + dataList.id + lastPartEdit,
                dataList.period,
                dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settings).Replace(@"""", ""),
                dateTimeClose = JsonConvert.SerializeObject(dataList.dateTimeClose, settings).Replace(@"""", ""),
                dataList.close
            });
            return Json(new { data });
        }

        public JsonResult GetId(int id)
        {
            var query = db.Debit_DataReportOprih.Where(d => d.id_Debit_PeriodReportOprih == id).ToList();
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
}