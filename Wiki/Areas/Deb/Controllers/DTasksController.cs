using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.Deb.Controllers
{
    public class DTasksController : Controller
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
            var query = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 28).ToList();

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
                //id = linkPartOne + dataList.id + linkPartTwo,
                dataList.PZ_PlanZakaz.PlanZakaz,
                dataList.PZ_PlanZakaz.Name,
                Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, settings).Replace(@"""", ""),
                numberSF = GetNumberSF(dataList),
                datePrihod = GetDatePrihod(dataList),
                DateSupply = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.DateSupply, settings).Replace(@"""", ""),
                idReclamation = GetIdReclamation(dataList),
                openReclamation = GetOpenReclamation(dataList),
                closeReclamation = GetCloseReclamation(dataList)
            });

            return Json(new { data });
        }

        [Authorize(Roles = "Admin, OPTP, OP")]
        public JsonResult GetId(int id)
        {
            var query = db.Debit_WorkBit.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                dataList.PZ_PlanZakaz.PlanZakaz,
                dataList.PZ_PlanZakaz.Name,
                Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, settings).Replace(@"""", ""),
                numberSF = GetNumberSF(dataList),
                datePrihod = GetDatePrihod(dataList),
                DateSupply = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.DateSupply, settings).Replace(@"""", ""),
                idReclamation = GetIdReclamation(dataList),
                openReclamation = GetOpenReclamation(dataList),
                closeReclamation = GetCloseReclamation(dataList),
                closeId = dataList.close,
                dateClose = JsonConvert.SerializeObject(dataList.dateClose, settings).Replace(@"""", "")
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, OPTP, OP")]
        public JsonResult Update(Debit_WorkBit work, bool closeId)
        {



            db.Entry(work).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        string GetNumberSF(Debit_WorkBit debit_WorkBit)
        {
            try
            {
                return debit_WorkBit.Debit_TN.First().numberSF;
            }
            catch
            {
                return "";
            }
        }

        string GetDatePrihod(Debit_WorkBit debit_WorkBit)
        {
            try
            {
                DateTime dateTime = debit_WorkBit.PostAlertShip.First().datePrihod;
                return dateTime.Day.ToString() + "." + dateTime.Month.ToString() + "." + dateTime.Year;
            }
            catch
            {
                return "";
            }
        }

        string GetOpenReclamation(Debit_WorkBit debit_WorkBit)
        {
            try
            {
                DateTime dateTime = debit_WorkBit.PZ_PlanZakaz.DebitReclamation.First().dateOpen;
                return dateTime.Day.ToString() + "." + dateTime.Month.ToString() + "." + dateTime.Year;
            }
            catch
            {
                return "";
            }
        }

        string GetCloseReclamation(Debit_WorkBit debit_WorkBit)
        {
            try
            {
                DateTime dateTime = debit_WorkBit.PZ_PlanZakaz.DebitReclamation.First().dateClose.Value;
                return dateTime.Day.ToString() + "." + dateTime.Month.ToString() + "." + dateTime.Year;
            }
            catch
            {
                return "";
            }
        }

        string GetIdReclamation(Debit_WorkBit debit_WorkBit)
        {
            try
            {
                var tmp = debit_WorkBit.PZ_PlanZakaz.DebitReclamation.First().id;
                return "+";
            }
            catch
            {
                return "";
            }
        }
    }
}