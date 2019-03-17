using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.Deb.Controllers
{
    public class ActiveReportController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly string firstPartLinkEditOP = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('";
        readonly string firstPartLinkEditKO = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyKOID('";
        readonly string lastPartEdit = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            Debit_PeriodReportOprih debit_PeriodReportOprih = db.Debit_PeriodReportOprih.Last();
            var query = db.Debit_DataReportOprih.Where(d => d.id_Debit_PeriodReportOprih == debit_PeriodReportOprih.id).ToList();
            string login = HttpContext.User.Identity.Name;
            int devision = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            string linkPartOne = "";
            string linkPartTwo = "";
            if (devision == 5 || login == "myi@katek.by")
            {
                linkPartOne = firstPartLinkEditOP;
                linkPartTwo = lastPartEdit;
            }

            var data = query.Select(dataList => new
            {
                id = linkPartOne + dataList.id + linkPartTwo,
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

            return Json(new { data });
        }

        [Authorize(Roles = "Admin, OPTP, OP")]
        public JsonResult GetId(int id)
        {
            var query = db.Debit_DataReportOprih.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
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

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, OPTP, OP")]
        public JsonResult Update(Debit_DataReportOprih work, bool oprihClose)
        {



            db.Entry(work).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}