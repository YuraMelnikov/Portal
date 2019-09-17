using System.Web.Mvc;

namespace Wiki.Areas.DashboardTV.Controllers
{
    public class DashboardTVController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPeriodReport()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                //int monthPlan = 


            }




            //var query = db.PZ_PlanZakaz
            //    .AsNoTracking()
            //    .Include(d => d.PZ_ProductType)
            //    .Include(d => d.AspNetUsers)
            //    .Include(d => d.PZ_OperatorDogovora)
            //    .Include(d => d.PZ_FIO)
            //    .Include(d => d.PZ_Dostavka)
            //    .Include(d => d.PZ_Client)
            //    .Where(d => d.dataOtgruzkiBP > DateTime.Now)
            //    .ToList();
            //string login = "";
            //try
            //{
            //    login = HttpContext.User.Identity.Name;
            //}
            //catch
            //{

            //}
            //int devision = 0;
            //try
            //{
            //    devision = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            //}
            //catch
            //{

            //}
            //string linkPartOne = "";
            //string linkPartTwo = "";
            //if (devision == 3 || devision == 15 || devision == 16 || devision == 18 || devision == 12)
            //{
            //    linkPartOne = firstPartLinkEditKO;
            //    linkPartTwo = lastPartEdit;
            //}
            //else if (devision == 5 || login == "myi@katek.by" || login == "gea@katek.by")
            //{
            //    linkPartOne = firstPartLinkEditOP;
            //    linkPartTwo = lastPartEdit;
            //}
            //var data = query.Select(dataList => new
            //{
            //    dataList.PlanZakaz,
            //    Id = GetLinkForEdit(dataList, linkPartOne, linkPartTwo),
            //    IdRead = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyReadID('" + dataList.Id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
            //    dataList.PZ_ProductType.ProductType,
            //    DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
            //    //StatusOrder = "",
            //    Manager = dataList.AspNetUsers.CiliricalName,
            //    dataList.Description,
            //    dataList.MTR,
            //    dataList.nomenklaturNumber,
            //    dataList.Name,
            //    dataList.nameTU,
            //    dataList.OL,
            //    dataList.PZ_Client.NameSort,
            //    dataList.Zapros,
            //    dataList.numZakupki,
            //    dataList.numLota,
            //    dataList.timeContract,
            //    timeContractDate = JsonConvert.SerializeObject(dataList.timeContractDate, settings).Replace(@"""", ""),
            //    dataList.timeArr,
            //    timeArrDate = JsonConvert.SerializeObject(dataList.timeArrDate, settings).Replace(@"""", ""),
            //    DateShipping = JsonConvert.SerializeObject(dataList.DateShipping, settings).Replace(@"""", ""),
            //    DateSupply = JsonConvert.SerializeObject(dataList.DateSupply, settings).Replace(@"""", ""),
            //    OperatorDogovora = dataList.PZ_OperatorDogovora.name,
            //    KuratorDogovora = dataList.PZ_FIO.fio,
            //    typeDostavka = dataList.PZ_Dostavka.Name,
            //    dataList.Cost,
            //    dataList.costSMR,
            //    dataList.costPNR,
            //    //SF = "",
            //    dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settings).Replace(@"""", "")
            //    //dateDostavki = "",
            //    //datePriemki = "",
            //    //dateOplat = ""
            //});

            return Json(new { data });
        }
    }
}