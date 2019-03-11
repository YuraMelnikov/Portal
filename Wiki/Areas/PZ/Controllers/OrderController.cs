using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.PZ.Controllers
{
    public class OrderController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult OrdersList()
        {
            int countLastOrdersView = 200;
            var query = db.PZ_PlanZakaz.OrderByDescending(d => d.DateCreate).Take(countLastOrdersView).ToList();

            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                dataList.PZ_ProductType.ProductType,
                DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                StatusOrder = "",
                Manager = dataList.AspNetUsers.CiliricalName,
                dataList.Description,
                dataList.MTR,
                dataList.nomenklaturNumber,
                dataList.Name,
                dataList.nameTU,
                dataList.OL,
                dataList.PZ_Client.NameSort,
                dataList.Zapros,
                dataList.numZakupki,
                dataList.numLota,
                dataList.timeContract,
                timeContractDate = JsonConvert.SerializeObject(dataList.timeContractDate, settings).Replace(@"""", ""),
                dataList.timeArr,
                timeArrDate = JsonConvert.SerializeObject(dataList.timeArrDate, settings).Replace(@"""", ""),
                DateShipping = JsonConvert.SerializeObject(dataList.DateShipping, settings).Replace(@"""", ""),
                DateSupply = JsonConvert.SerializeObject(dataList.DateSupply, settings).Replace(@"""", ""),
                OperatorDogovora = dataList.PZ_OperatorDogovora.name,
                KuratorDogovora = dataList.PZ_FIO.fio,
                typeDostavka = dataList.PZ_Dostavka.Name,
                dataList.Cost,
                dataList.costSMR,
                dataList.costPNR,
                SF = "",
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settings).Replace(@"""", ""),
                dateDostavki = "",
                datePriemki = "",
                dateOplat = ""
            });

            return Json(new { data });
        }

        [HttpPost]
        public JsonResult OrdersListLY(int yearCreateOrder)
        {
            var query = db.PZ_PlanZakaz.Where(d => d.DateCreate.Year == yearCreateOrder).ToList();
            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                dataList.PZ_ProductType.ProductType,
                DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                StatusOrder = "",
                Manager = dataList.AspNetUsers.CiliricalName,
                dataList.Description,
                dataList.MTR,
                dataList.nomenklaturNumber,
                dataList.Name,
                dataList.nameTU,
                dataList.OL,
                dataList.PZ_Client.NameSort,
                dataList.Zapros,
                dataList.numZakupki,
                dataList.numLota,
                dataList.timeContract,
                timeContractDate = JsonConvert.SerializeObject(dataList.timeContractDate, settings).Replace(@"""", ""),
                dataList.timeArr,
                timeArrDate = JsonConvert.SerializeObject(dataList.timeArrDate, settings).Replace(@"""", ""),
                DateShipping = JsonConvert.SerializeObject(dataList.DateShipping, settings).Replace(@"""", ""),
                DateSupply = JsonConvert.SerializeObject(dataList.DateSupply, settings).Replace(@"""", ""),
                OperatorDogovora = dataList.PZ_OperatorDogovora.name,
                KuratorDogovora = dataList.PZ_FIO.fio,
                typeDostavka = dataList.PZ_Dostavka.Name,
                dataList.Cost,
                dataList.costSMR,
                dataList.costPNR,
                SF = "",
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settings).Replace(@"""", ""),
                dateDostavki = "",
                datePriemki = "",
                dateOplat = ""
            });

            return Json(new { data });
        }

        [HttpPost]
        public JsonResult OrdersListALL()
        {
            var query = db.PZ_PlanZakaz.ToList();
            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                dataList.PZ_ProductType.ProductType,
                DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                StatusOrder = "",
                Manager = dataList.AspNetUsers.CiliricalName,
                dataList.Description,
                dataList.MTR,
                dataList.nomenklaturNumber,
                dataList.Name,
                dataList.nameTU,
                dataList.OL,
                dataList.PZ_Client.NameSort,
                dataList.Zapros,
                dataList.numZakupki,
                dataList.numLota,
                dataList.timeContract,
                timeContractDate = JsonConvert.SerializeObject(dataList.timeContractDate, settings).Replace(@"""", ""),
                dataList.timeArr,
                timeArrDate = JsonConvert.SerializeObject(dataList.timeArrDate, settings).Replace(@"""", ""),
                DateShipping = JsonConvert.SerializeObject(dataList.DateShipping, settings).Replace(@"""", ""),
                DateSupply = JsonConvert.SerializeObject(dataList.DateSupply, settings).Replace(@"""", ""),
                OperatorDogovora = dataList.PZ_OperatorDogovora.name,
                KuratorDogovora = dataList.PZ_FIO.fio,
                typeDostavka = dataList.PZ_Dostavka.Name,
                dataList.Cost,
                dataList.costSMR,
                dataList.costPNR,
                SF = "",
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settings).Replace(@"""", ""),
                dateDostavki = "",
                datePriemki = "",
                dateOplat = ""
            });

            return Json(new { data });
        }
    }
}