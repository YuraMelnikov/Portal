using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.PZ.Models;

namespace Wiki.Areas.PZ.Controllers
{
    public class OrderController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        public ActionResult Index()
        {
            ViewBag.Manager = new SelectList(db.AspNetUsers.Where(d => d.LockoutEnabled == true).Where(x => x.Devision == 5 || x.CiliricalName == "Антипов Эдуард Валерьевич" || x.CiliricalName == "Брель Андрей Викторович").OrderBy(x => x.CiliricalName), "id", "CiliricalName");
            ViewBag.Client = new SelectList(db.PZ_Client.OrderBy(x => x.NameSort), "id", "NameSort");
            ViewBag.Dostavka = new SelectList(db.PZ_Dostavka.OrderBy(d => d.Name), "id", "Name");
            ViewBag.ProductType = new SelectList(db.PZ_ProductType.OrderBy(d => d.ProductType), "id", "ProductType");
            ViewBag.id_PZ_OperatorDogovora = new SelectList(db.PZ_OperatorDogovora.OrderBy(x => x.name), "id", "name");
            ViewBag.id_PZ_FIO = new SelectList(db.PZ_FIO.OrderBy(x => x.fio), "id", "fio");
            ViewBag.TypeShip = new SelectList(db.PZ_TypeShip.OrderBy(x => x.typeShip), "id", "typeShip");
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

        public JsonResult Add(PZ_PlanZakaz pZ_PlanZakaz, int[] countOrders)
        {
            CorrectPlanZakaz correctPlanZakaz = new CorrectPlanZakaz(pZ_PlanZakaz);
            pZ_PlanZakaz = correctPlanZakaz.PZ_PlanZakaz;
            int count = countOrders[0];
            for (int i = 0; i < count; i++)
            {
                NewPlanZakaz pz = new NewPlanZakaz(pZ_PlanZakaz, true);
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}