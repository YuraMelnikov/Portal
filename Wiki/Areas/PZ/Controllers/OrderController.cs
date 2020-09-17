using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Wiki.Areas.PZ.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Syncfusion.XlsIO;
using System.Web;

namespace Wiki.Areas.PZ.Controllers
{
    public class OrderController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly JsonSerializerSettings settingsLong = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly string firstPartLinkEditOP = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('";
        readonly string firstPartLinkEditWeight = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetWeightData('";
        readonly string firstPartLinkEditKO = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyKOID('";
        readonly string lastPartEdit = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";

        public ActionResult Index()
        {
            ViewBag.id_Provider = new SelectList(db.Provider.OrderBy(a => a.name), "id", "name");
            ViewBag.Orders = new SelectList(db.PZ_PlanZakaz.Where(d => d.PlanZakaz < 9000).OrderByDescending(x => x.PlanZakaz), "id", "PlanZakaz");
            ViewBag.Manager = new SelectList(db.AspNetUsers.Where(d => d.LockoutEnabled == true).Where(x => x.Devision == 5 || x.CiliricalName == "Антипов Эдуард Валерьевич" || x.CiliricalName == "Брель Андрей Викторович").OrderBy(x => x.CiliricalName), "id", "CiliricalName");
            ViewBag.Client = new SelectList(db.PZ_Client.OrderBy(x => x.NameSort), "id", "NameSort");
            ViewBag.Dostavka = new SelectList(db.PZ_Dostavka.OrderBy(d => d.Name), "id", "Name");
            ViewBag.ProductType = new SelectList(db.PZ_ProductType.OrderBy(d => d.ProductType), "id", "ProductType");
            ViewBag.id_PZ_OperatorDogovora = new SelectList(db.PZ_OperatorDogovora.OrderBy(x => x.name), "id", "name");
            ViewBag.id_PZ_FIO = new SelectList(db.PZ_FIO.OrderBy(x => x.fio), "id", "fio");
            ViewBag.TypeShip = new SelectList(db.PZ_TypeShip.OrderBy(x => x.typeShip), "id", "typeShip");
            try
            {
                string login = HttpContext.User.Identity.Name;
                logger.Debug("PZ_List: " + login);
            }
            catch
            {

            }
            return View();
        }

        public ActionResult Debug()
        {
            int id_PZ = 2728;
            List<TaskForPZ> dateTaskWork = db.TaskForPZ.Where(w => w.step == 1).Where(z => z.id_TypeTaskForPZ == 1).ToList();
            foreach (var data in dateTaskWork)
            {
                Debit_WorkBit newDebit_WorkBit = new Debit_WorkBit();
                newDebit_WorkBit.dateCreate = DateTime.Now;
                newDebit_WorkBit.close = false;
                newDebit_WorkBit.id_PlanZakaz = id_PZ;
                newDebit_WorkBit.id_TaskForPZ = (int)data.id;
                newDebit_WorkBit.datePlanFirst = DateTime.Now.AddDays((double)data.time);
                newDebit_WorkBit.datePlan = DateTime.Now.AddDays((double)data.time);
                if (newDebit_WorkBit.id_TaskForPZ == 1)
                    newDebit_WorkBit.dateClose = DateTime.Now;
                db.Debit_WorkBit.Add(new Debit_WorkBit()
                {
                    close = false,
                    dateCreate = DateTime.Now,
                    datePlan = newDebit_WorkBit.datePlan,
                    datePlanFirst = newDebit_WorkBit.datePlanFirst,
                    id_PlanZakaz = newDebit_WorkBit.id_PlanZakaz,
                    id_TaskForPZ = newDebit_WorkBit.id_TaskForPZ,
                    dateClose = newDebit_WorkBit.dateClose
                });
                db.SaveChanges();
            }
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

        [HttpPost]
        public JsonResult OrdersListInManufacturing()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.PZ_PlanZakaz
                .AsNoTracking()
                .Include(d => d.PZ_ProductType)
                .Include(d => d.AspNetUsers)
                .Include(d => d.PZ_OperatorDogovora)
                .Include(d => d.PZ_FIO)
                .Include(d => d.PZ_Dostavka)
                .Include(d => d.PZ_Client)
                .Include(d => d.Provider)
                .Where(d => d.dataOtgruzkiBP > DateTime.Now)
                .ToList();
            string login = "";
            try
            {
                login = HttpContext.User.Identity.Name;
            }
            catch
            {

            }
            int devision = 0;
            try
            {
                devision = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            }
            catch
            {

            }
            string linkPartOne = "";
            string linkPartTwo = "";
            if (devision == 3 || devision == 15 || devision == 16 || devision == 18 || devision == 12)
            {
                linkPartOne = firstPartLinkEditKO;
                linkPartTwo = lastPartEdit;
            }
            else if (devision == 5 || login == "myi@katek.by")
            {
                linkPartOne = firstPartLinkEditOP;
                linkPartTwo = lastPartEdit;
            }
            else if (login == "bav@katek.by")
            {
                linkPartOne = firstPartLinkEditWeight;
                linkPartTwo = lastPartEdit;
            }
            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                Id = GetLinkForEdit(dataList, linkPartOne, linkPartTwo),
                IdRead = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyReadID('" + dataList.Id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                dataList.PZ_ProductType.ProductType,
                DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                //StatusOrder = "",
                Manager = dataList.AspNetUsers.CiliricalName,
                dataList.Description,
                dataList.MTR,
                dataList.nomenklaturNumber,
                dataList.Name,
                dataList.nameTU,
                dataList.OL,
                provider = dataList.Provider.name,
                dataList.PZ_Client.NameSort,
                dataList.Zapros,
                dataList.massa,
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
                //SF = "",
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settings).Replace(@"""", "")
                //dateDostavki = "",
                //datePriemki = "",
                //dateOplat = ""
            });

            return Json(new { data });
        }

        string GetLinkForEdit(PZ_PlanZakaz pZ_PlanZakaz, string linkPartOne, string linkPartTwo)
        {
            int closeOrder = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == pZ_PlanZakaz.Id).Where(d => d.id_TaskForPZ == 15).Where(d => d.close == true).Count();
            string link = linkPartOne + pZ_PlanZakaz.Id + linkPartTwo;
            if (closeOrder > 0)
                link = "<td><span class=" + '\u0022' + "glyphicon glyphicon-remove-circle" + '\u0022' + "></span></td>";
            if (linkPartOne == "")
                link = "<td></td>";
            return link;
        }

        [HttpPost]
        public JsonResult OrdersList()
        {
            int countLastOrdersView = 250;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.PZ_PlanZakaz
                .AsNoTracking()
                .Include(d => d.PZ_ProductType)
                .Include(d => d.AspNetUsers)
                .Include(d => d.PZ_OperatorDogovora)
                .Include(d => d.PZ_FIO)
                .Include(d => d.PZ_Dostavka)
                .Include(d => d.PZ_Client)
                .Include(d => d.Provider)
                .OrderByDescending(d => d.DateCreate)
                .Take(countLastOrdersView)
                .ToList();
            string login = "";
            try
            {
                login = HttpContext.User.Identity.Name;
            }
            catch
            {

            }
            int devision = 0;
            try
            {
                devision = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            }
            catch
            {

            }
            string linkPartOne = "";
            string linkPartTwo = "";
            if (devision == 3 || devision == 15 || devision == 16 || devision == 18 || devision == 12)
            {
                linkPartOne = firstPartLinkEditKO;
                linkPartTwo = lastPartEdit;
            }
            else if (devision == 5 || login == "myi@katek.by")
            {
                linkPartOne = firstPartLinkEditOP;
                linkPartTwo = lastPartEdit;
            }
            else if (login == "bav@katek.by")
            {
                linkPartOne = firstPartLinkEditWeight;
                linkPartTwo = lastPartEdit;
            }
            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                Id = GetLinkForEdit(dataList, linkPartOne, linkPartTwo),
                IdRead = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyReadID('" + dataList.Id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                dataList.PZ_ProductType.ProductType,
                DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                //StatusOrder = "",
                Manager = dataList.AspNetUsers.CiliricalName,
                dataList.Description,
                dataList.MTR,
                dataList.nomenklaturNumber,
                dataList.Name,
                dataList.nameTU,
                provider = dataList.Provider.name,
                dataList.massa,
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
                //SF = "",
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settings).Replace(@"""", "")
                //dateDostavki = "",
                //datePriemki = "",
                //dateOplat = ""
            });

            return Json(new { data });
        }

        [HttpPost]
        public JsonResult OrdersListLY(int yearCreateOrder)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.PZ_PlanZakaz
                .AsNoTracking()
                .Include(d => d.PZ_ProductType)
                .Include(d => d.AspNetUsers)
                .Include(d => d.PZ_OperatorDogovora)
                .Include(d => d.PZ_FIO)
                .Include(d => d.PZ_Dostavka)
                .Include(d => d.Provider)
                .Include(d => d.PZ_Client)
                .Where(d => d.DateCreate.Year == yearCreateOrder).ToList();
            string login = "";
            try
            {
                login = HttpContext.User.Identity.Name;
            }
            catch
            {

            }
            int devision = 0;
            try
            {
                devision = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            }
            catch
            {

            }
            string linkPartOne = "";
            string linkPartTwo = "";
            if (devision == 3 || devision == 15 || devision == 16 || devision == 18 || devision == 12)
            {
                linkPartOne = firstPartLinkEditKO;
                linkPartTwo = lastPartEdit;
            }
            else if (devision == 5 || login == "myi@katek.by")
            {
                linkPartOne = firstPartLinkEditOP;
                linkPartTwo = lastPartEdit;
            }
            else if (login == "bav@katek.by")
            {
                linkPartOne = firstPartLinkEditWeight;
                linkPartTwo = lastPartEdit;
            }
            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                Id = GetLinkForEdit(dataList, linkPartOne, linkPartTwo),
                IdRead = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyReadID('" + dataList.Id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                dataList.PZ_ProductType.ProductType,
                DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                //StatusOrder = "",
                Manager = dataList.AspNetUsers.CiliricalName,
                dataList.Description,
                dataList.MTR,
                dataList.nomenklaturNumber,
                dataList.Name,
                dataList.nameTU,
                dataList.OL,
                provider = dataList.Provider.name,
                dataList.PZ_Client.NameSort,
                dataList.massa,
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
                //SF = "",
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settings).Replace(@"""", "")
                //dateDostavki = "",
                //datePriemki = "",
                //dateOplat = ""
            });

            return Json(new { data });
        }

        [HttpPost]
        public JsonResult OrdersListALL()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.PZ_PlanZakaz
                .AsNoTracking()
                .Include(d => d.PZ_ProductType)
                .Include(d => d.AspNetUsers)
                .Include(d => d.PZ_OperatorDogovora)
                .Include(d => d.PZ_FIO)
                .Include(d => d.PZ_Dostavka)
                .Include(d => d.PZ_Client)
                .Include(d => d.Provider)
                .ToList();
            string login = "";
            try
            {
                login = HttpContext.User.Identity.Name;
            }
            catch
            {

            }
            int devision = 0;
            try
            {
                devision = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            }
            catch
            {

            }
            string linkPartOne = "";
            string linkPartTwo = "";
            if (devision == 3 || devision == 15 || devision == 16 || devision == 18 || devision == 12)
            {
                linkPartOne = firstPartLinkEditKO;
                linkPartTwo = lastPartEdit;
            }
            else if (devision == 5 || login == "myi@katek.by")
            {
                linkPartOne = firstPartLinkEditOP;
                linkPartTwo = lastPartEdit;
            }
            else if (login == "bav@katek.by")
            {
                linkPartOne = firstPartLinkEditWeight;
                linkPartTwo = lastPartEdit;
            }
            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                Id = GetLinkForEdit(dataList, linkPartOne, linkPartTwo),
                IdRead = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyReadID('" + dataList.Id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                dataList.PZ_ProductType.ProductType,
                DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                //StatusOrder = "",
                Manager = dataList.AspNetUsers.CiliricalName,
                dataList.Description,
                dataList.MTR,
                dataList.nomenklaturNumber,
                dataList.Name,
                dataList.nameTU,
                dataList.OL,
                dataList.PZ_Client.NameSort,
                dataList.Zapros,
                dataList.massa,
                dataList.numZakupki,
                provider = dataList.Provider.name,
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
                //SF = "",
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settings).Replace(@"""", "")
                //dateDostavki = "",
                //datePriemki = "",
                //dateOplat = ""
            });

            return Json(new { data });
        }

        [HttpPost]
        public JsonResult OrdersListForOpenedOrder()
        {
            DateTime controlDate = DateTime.Now.AddDays(-7);
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.PZ_PlanZakaz
                .AsNoTracking()
                .Include(d => d.PZ_ProductType)
                .Include(d => d.AspNetUsers)
                .Include(d => d.PZ_OperatorDogovora)
                .Include(d => d.PZ_FIO)
                .Include(d => d.PZ_Dostavka)
                .Include(d => d.PZ_Client)
                .Where(d => d.DateCreate > controlDate)
                .ToList();

            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                dataList.Name,
                dataList.PZ_Client.NameSort,
                DateSupply = JsonConvert.SerializeObject(dataList.DateSupply, settings).Replace(@"""", ""),
                ol = "+",
                tp = "+",
                tCost = "+",
                ett = "-",
                managet = dataList.AspNetUsers.CiliricalName,
                dataList.Zapros,
                sd = dataList.PZ_Dostavka.Name,
                dataList.Description
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
                _ = new NewPlanZakaz(pZ_PlanZakaz, true);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrder(int Id)
        {
            var query = db.PZ_PlanZakaz.Where(d => d.Id == Id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                dataList.Manager,
                dataList.id_Provider,
                dataList.Client,
                dataList.id_PZ_OperatorDogovora,
                dataList.id_PZ_FIO,
                dataList.Name,
                dataList.Description,
                dataList.massa,
                cgm = dataList.coefM,
                cge = dataList.coefE,
                dataList.MTR,
                dataList.nomenklaturNumber,
                dataList.timeContract,
                timeContractDate = JsonConvert.SerializeObject(dataList.timeContractDate, settings).Replace(@"""", ""),
                dataList.timeArr,
                timeArrDate = JsonConvert.SerializeObject(dataList.timeArrDate, settings).Replace(@"""", ""),
                DateShipping = JsonConvert.SerializeObject(dataList.DateShipping, settings).Replace(@"""", ""),
                DateSupply = JsonConvert.SerializeObject(dataList.DateSupply, settings).Replace(@"""", ""),
                dataList.Dostavka,
                dataList.Cost,
                dataList.costSMR,
                dataList.costPNR,
                dataList.ProductType,
                dataList.OL,
                dataList.Zapros,
                dataList.numZakupki,
                dataList.numLota,
                dataList.TypeShip,
                criticalDateShip = JsonConvert.SerializeObject(dataList.criticalDateShip, settings).Replace(@"""", ""),
                dataList.PowerST,
                dataList.VN_NN,
                dataList.objectOfExploitation,
                dataList.counterText,
                dataList.Modul,
                dataList.Gruzopoluchatel,
                dataList.PostAdresGruzopoluchatel,
                dataList.INNGruzopoluchatel,
                dataList.OKPOGruzopoluchatelya,
                dataList.KodGruzopoluchatela,
                dataList.StantionGruzopoluchatel,
                dataList.KodStanciiGruzopoluchatelya,
                dataList.OsobieOtmetkiGruzopoluchatelya,
                dataList.DescriptionGruzopoluchatel
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(PZ_PlanZakaz pZ_PlanZakaz)
        {
            string login = HttpContext.User.Identity.Name;
            PZ_PlanZakaz editPZ = db.PZ_PlanZakaz.First(d => d.PlanZakaz == pZ_PlanZakaz.PlanZakaz);
            if (editPZ.Manager != pZ_PlanZakaz.Manager)
                editPZ.Manager = pZ_PlanZakaz.Manager;
            if (editPZ.id_Provider != pZ_PlanZakaz.id_Provider)
                editPZ.id_Provider = pZ_PlanZakaz.id_Provider;
            if (editPZ.Client != pZ_PlanZakaz.Client)
                editPZ.Client = pZ_PlanZakaz.Client;
            if (editPZ.id_PZ_OperatorDogovora != pZ_PlanZakaz.id_PZ_OperatorDogovora)
                editPZ.id_PZ_OperatorDogovora = pZ_PlanZakaz.id_PZ_OperatorDogovora;
            if (editPZ.id_PZ_FIO != pZ_PlanZakaz.id_PZ_FIO)
                editPZ.id_PZ_FIO = pZ_PlanZakaz.id_PZ_FIO;
            if (editPZ.Name != pZ_PlanZakaz.Name)
            {
                EmailRename emailRename = new EmailRename(editPZ.PlanZakaz.ToString(), editPZ.Name, pZ_PlanZakaz.Name, login, false);
                emailRename.SendEmail();
                editPZ.Name = pZ_PlanZakaz.Name;
            }
            if (editPZ.Description != pZ_PlanZakaz.Description)
                editPZ.Description = pZ_PlanZakaz.Description;
            if (editPZ.MTR != pZ_PlanZakaz.MTR)
                editPZ.MTR = pZ_PlanZakaz.MTR;
            if (editPZ.nomenklaturNumber != pZ_PlanZakaz.nomenklaturNumber)
                editPZ.nomenklaturNumber = pZ_PlanZakaz.nomenklaturNumber;
            if (editPZ.timeContract != pZ_PlanZakaz.timeContract)
                editPZ.timeContract = pZ_PlanZakaz.timeContract;
            if (editPZ.timeContractDate != pZ_PlanZakaz.timeContractDate)
                editPZ.timeContractDate = pZ_PlanZakaz.timeContractDate;
            if (editPZ.timeArr != pZ_PlanZakaz.timeArr)
                editPZ.timeArr = pZ_PlanZakaz.timeArr;
            if (editPZ.timeArrDate != pZ_PlanZakaz.timeArrDate)
                editPZ.timeArrDate = pZ_PlanZakaz.timeArrDate;
            if (editPZ.DateShipping != pZ_PlanZakaz.DateShipping)
                editPZ.DateShipping = pZ_PlanZakaz.DateShipping;
            if (editPZ.DateSupply != pZ_PlanZakaz.DateSupply)
                editPZ.DateSupply = pZ_PlanZakaz.DateSupply;
            if (editPZ.Dostavka != pZ_PlanZakaz.Dostavka)
                editPZ.Dostavka = pZ_PlanZakaz.Dostavka;
            if (editPZ.Cost != pZ_PlanZakaz.Cost)
                editPZ.Cost = pZ_PlanZakaz.Cost;
            if (editPZ.costSMR != pZ_PlanZakaz.costSMR)
                editPZ.costSMR = pZ_PlanZakaz.costSMR;
            if (editPZ.costPNR != pZ_PlanZakaz.costPNR)
                editPZ.costPNR = pZ_PlanZakaz.costPNR;
            if (editPZ.ProductType != pZ_PlanZakaz.ProductType)
                editPZ.ProductType = pZ_PlanZakaz.ProductType;
            if (editPZ.OL != pZ_PlanZakaz.OL)
                editPZ.OL = pZ_PlanZakaz.OL;
            if (editPZ.Zapros != pZ_PlanZakaz.Zapros)
                editPZ.Zapros = pZ_PlanZakaz.Zapros;
            if (editPZ.numZakupki != pZ_PlanZakaz.numZakupki)
                editPZ.numZakupki = pZ_PlanZakaz.numZakupki;
            if (editPZ.numLota != pZ_PlanZakaz.numLota)
                editPZ.numLota = pZ_PlanZakaz.numLota;
            if (editPZ.TypeShip != pZ_PlanZakaz.TypeShip)
                editPZ.TypeShip = pZ_PlanZakaz.TypeShip;
            if (editPZ.criticalDateShip != pZ_PlanZakaz.criticalDateShip)
                editPZ.criticalDateShip = pZ_PlanZakaz.criticalDateShip;
            if (editPZ.PowerST != pZ_PlanZakaz.PowerST)
                editPZ.PowerST = pZ_PlanZakaz.PowerST;
            if (editPZ.VN_NN != pZ_PlanZakaz.VN_NN)
                editPZ.VN_NN = pZ_PlanZakaz.VN_NN;
            if (editPZ.objectOfExploitation != pZ_PlanZakaz.objectOfExploitation)
                editPZ.objectOfExploitation = pZ_PlanZakaz.objectOfExploitation;
            if (editPZ.counterText != pZ_PlanZakaz.counterText)
                editPZ.counterText = pZ_PlanZakaz.counterText;
            if (editPZ.Modul != pZ_PlanZakaz.Modul)
                editPZ.Modul = pZ_PlanZakaz.Modul;
            if (editPZ.Gruzopoluchatel != pZ_PlanZakaz.Gruzopoluchatel)
                editPZ.Gruzopoluchatel = pZ_PlanZakaz.Gruzopoluchatel;
            if (editPZ.PostAdresGruzopoluchatel != pZ_PlanZakaz.PostAdresGruzopoluchatel)
                editPZ.PostAdresGruzopoluchatel = pZ_PlanZakaz.PostAdresGruzopoluchatel;
            if (editPZ.INNGruzopoluchatel != pZ_PlanZakaz.INNGruzopoluchatel)
                editPZ.INNGruzopoluchatel = pZ_PlanZakaz.INNGruzopoluchatel;
            if (editPZ.OKPOGruzopoluchatelya != pZ_PlanZakaz.OKPOGruzopoluchatelya)
                editPZ.OKPOGruzopoluchatelya = pZ_PlanZakaz.OKPOGruzopoluchatelya;
            if (editPZ.KodGruzopoluchatela != pZ_PlanZakaz.KodGruzopoluchatela)
                editPZ.KodGruzopoluchatela = pZ_PlanZakaz.KodGruzopoluchatela;
            if (editPZ.StantionGruzopoluchatel != pZ_PlanZakaz.StantionGruzopoluchatel)
                editPZ.StantionGruzopoluchatel = pZ_PlanZakaz.StantionGruzopoluchatel;
            if (editPZ.KodStanciiGruzopoluchatelya != pZ_PlanZakaz.KodStanciiGruzopoluchatelya)
                editPZ.KodStanciiGruzopoluchatelya = pZ_PlanZakaz.KodStanciiGruzopoluchatelya;
            if (editPZ.OsobieOtmetkiGruzopoluchatelya != pZ_PlanZakaz.OsobieOtmetkiGruzopoluchatelya)
                editPZ.OsobieOtmetkiGruzopoluchatelya = pZ_PlanZakaz.OsobieOtmetkiGruzopoluchatelya;
            if (editPZ.DescriptionGruzopoluchatel != pZ_PlanZakaz.DescriptionGruzopoluchatel)
                editPZ.DescriptionGruzopoluchatel = pZ_PlanZakaz.DescriptionGruzopoluchatel;
            CorrectPlanZakaz correctPlanZakaz = new CorrectPlanZakaz(editPZ);
            editPZ = correctPlanZakaz.PZ_PlanZakaz;
            db.Entry(editPZ).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateOrders(PZ_PlanZakaz pZ_PlanZakaz, int[] Orders)
        {
            string login = HttpContext.User.Identity.Name;
            if (Orders == null)
                return Json(1, JsonRequestBehavior.AllowGet);
            for (int i = 0; i < Orders.Length; i++)
            {
                PZ_PlanZakaz editPZ = db.PZ_PlanZakaz.Find(Orders[i]);
                if (pZ_PlanZakaz.Manager != null)
                    editPZ.Manager = pZ_PlanZakaz.Manager;
                editPZ.id_Provider = pZ_PlanZakaz.id_Provider;
                if (pZ_PlanZakaz.Client != 0)
                    editPZ.Client = pZ_PlanZakaz.Client;
                if (pZ_PlanZakaz.id_PZ_OperatorDogovora != 0)
                    editPZ.id_PZ_OperatorDogovora = pZ_PlanZakaz.id_PZ_OperatorDogovora;
                if (pZ_PlanZakaz.id_PZ_FIO != 0)
                    editPZ.id_PZ_FIO = pZ_PlanZakaz.id_PZ_FIO;
                if (pZ_PlanZakaz.Name != null)
                {
                    if (editPZ.Name.Replace(" ", "") != pZ_PlanZakaz.Name.Replace(" ", ""))
                    {
                        EmailRename emailRename = new EmailRename(editPZ.PlanZakaz.ToString(), editPZ.Name, pZ_PlanZakaz.Name, login, false);
                        emailRename.SendEmail();
                        editPZ.Name = pZ_PlanZakaz.Name;
                    }
                }
                if (pZ_PlanZakaz.Description != null)
                    editPZ.Description = pZ_PlanZakaz.Description;
                if (pZ_PlanZakaz.MTR != null)
                    editPZ.MTR = pZ_PlanZakaz.MTR;
                if (pZ_PlanZakaz.nomenklaturNumber != null)
                    editPZ.nomenklaturNumber = pZ_PlanZakaz.nomenklaturNumber;
                if (pZ_PlanZakaz.timeContract != null)
                    editPZ.timeContract = pZ_PlanZakaz.timeContract;
                if (pZ_PlanZakaz.timeContractDate != null && pZ_PlanZakaz.timeContractDate.Value.Year > 2000)
                    editPZ.timeContractDate = pZ_PlanZakaz.timeContractDate;
                if (pZ_PlanZakaz.timeArr != null)
                    editPZ.timeArr = pZ_PlanZakaz.timeArr;
                if (pZ_PlanZakaz.timeArrDate != null && pZ_PlanZakaz.timeArrDate.Value.Year > 2000)
                    editPZ.timeArrDate = pZ_PlanZakaz.timeArrDate;
                if (pZ_PlanZakaz.DateShipping != null && pZ_PlanZakaz.DateShipping.Year > 2000)
                    editPZ.DateShipping = pZ_PlanZakaz.DateShipping;
                if (pZ_PlanZakaz.DateSupply != null && pZ_PlanZakaz.DateSupply.Year > 2000)
                    editPZ.DateSupply = pZ_PlanZakaz.DateSupply;
                if (pZ_PlanZakaz.Dostavka != 0)
                    editPZ.Dostavka = pZ_PlanZakaz.Dostavka;
                if (pZ_PlanZakaz.Cost != 0)
                    editPZ.Cost = pZ_PlanZakaz.Cost;
                if (pZ_PlanZakaz.costSMR != 0)
                    editPZ.costSMR = pZ_PlanZakaz.costSMR;
                if (pZ_PlanZakaz.costPNR != 0)
                    editPZ.costPNR = pZ_PlanZakaz.costPNR;
                if (pZ_PlanZakaz.ProductType != 0)
                    editPZ.ProductType = pZ_PlanZakaz.ProductType;
                if (pZ_PlanZakaz.OL != null)
                    editPZ.OL = pZ_PlanZakaz.OL;
                if (pZ_PlanZakaz.Zapros != 0)
                    editPZ.Zapros = pZ_PlanZakaz.Zapros;
                if (pZ_PlanZakaz.numZakupki != null)
                    editPZ.numZakupki = pZ_PlanZakaz.numZakupki;
                if (pZ_PlanZakaz.numLota != null)
                    editPZ.numLota = pZ_PlanZakaz.numLota;
                if (pZ_PlanZakaz.TypeShip != 0)
                    editPZ.TypeShip = pZ_PlanZakaz.TypeShip;
                if (pZ_PlanZakaz.criticalDateShip != null && pZ_PlanZakaz.criticalDateShip.Value.Year > 2000)
                    editPZ.criticalDateShip = pZ_PlanZakaz.criticalDateShip;
                if (pZ_PlanZakaz.PowerST != null)
                    editPZ.PowerST = pZ_PlanZakaz.PowerST;
                if (pZ_PlanZakaz.VN_NN != null)
                    editPZ.VN_NN = pZ_PlanZakaz.VN_NN;
                if (pZ_PlanZakaz.objectOfExploitation != null)
                    editPZ.objectOfExploitation = pZ_PlanZakaz.objectOfExploitation;
                if (pZ_PlanZakaz.counterText != null)
                    editPZ.counterText = pZ_PlanZakaz.counterText;
                if (pZ_PlanZakaz.Modul != null)
                    editPZ.Modul = pZ_PlanZakaz.Modul;
                if (pZ_PlanZakaz.Gruzopoluchatel != null)
                    editPZ.Gruzopoluchatel = pZ_PlanZakaz.Gruzopoluchatel;
                if (pZ_PlanZakaz.PostAdresGruzopoluchatel != null)
                    editPZ.PostAdresGruzopoluchatel = pZ_PlanZakaz.PostAdresGruzopoluchatel;
                if (pZ_PlanZakaz.INNGruzopoluchatel != null)
                    editPZ.INNGruzopoluchatel = pZ_PlanZakaz.INNGruzopoluchatel;
                if (pZ_PlanZakaz.OKPOGruzopoluchatelya != null)
                    editPZ.OKPOGruzopoluchatelya = pZ_PlanZakaz.OKPOGruzopoluchatelya;
                if (pZ_PlanZakaz.KodGruzopoluchatela != null)
                    editPZ.KodGruzopoluchatela = pZ_PlanZakaz.KodGruzopoluchatela;
                if (pZ_PlanZakaz.StantionGruzopoluchatel != null)
                    editPZ.StantionGruzopoluchatel = pZ_PlanZakaz.StantionGruzopoluchatel;
                if (pZ_PlanZakaz.KodStanciiGruzopoluchatelya != null)
                    editPZ.KodStanciiGruzopoluchatelya = pZ_PlanZakaz.KodStanciiGruzopoluchatelya;
                if (pZ_PlanZakaz.OsobieOtmetkiGruzopoluchatelya != null)
                    editPZ.OsobieOtmetkiGruzopoluchatelya = pZ_PlanZakaz.OsobieOtmetkiGruzopoluchatelya;
                if (pZ_PlanZakaz.DescriptionGruzopoluchatel != null)
                    editPZ.DescriptionGruzopoluchatel = pZ_PlanZakaz.DescriptionGruzopoluchatel;
                CorrectPlanZakaz correctPlanZakaz = new CorrectPlanZakaz(editPZ);
                editPZ = correctPlanZakaz.PZ_PlanZakaz;
                db.Entry(editPZ).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetKOOrder(int Id)
        {
            var query = db.PZ_PlanZakaz.Where(d => d.Id == Id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                dataList.ProductType,
                dataList.massa,
                cgm = dataList.coefM,
                cge = dataList.coefE,
                dataList.nameTU,
                dataList.Id
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateKO(PZ_PlanZakaz pZ_PlanZakaz, double cgm, double cge)
        {
            string login = HttpContext.User.Identity.Name;
            PZ_PlanZakaz editPZ = db.PZ_PlanZakaz.First(d => d.PlanZakaz == pZ_PlanZakaz.PlanZakaz);
            if (editPZ.nameTU != pZ_PlanZakaz.nameTU)
            {
                EmailRename emailRename = new EmailRename(editPZ.PlanZakaz.ToString(), editPZ.nameTU, pZ_PlanZakaz.nameTU, login, true);
                emailRename.SendEmail();
                editPZ.nameTU = pZ_PlanZakaz.nameTU;
                editPZ.ProductType = pZ_PlanZakaz.ProductType;
            }
            if (editPZ.ProductType != pZ_PlanZakaz.ProductType)
            {
                editPZ.ProductType = pZ_PlanZakaz.ProductType;
            }
            editPZ.coefM = cgm;
            editPZ.coefE = cge;
            if (editPZ.massa != pZ_PlanZakaz.massa)
            {
                try
                {
                    EmailRename emailRename = new EmailRename(editPZ.PlanZakaz.ToString(), editPZ.massa, pZ_PlanZakaz.massa, login, false);
                    emailRename.SendEmailMassa();
                }
                catch
                {

                }
                editPZ.massa = pZ_PlanZakaz.massa;
                try
                {
                    using (ExportImportEntities dbc = new ExportImportEntities())
                    {
                        var pzImport = dbc.planZakaz.First(d => d.Zakaz == editPZ.PlanZakaz);
                        pzImport.weight = editPZ.massa;
                        dbc.Entry(pzImport).State = EntityState.Modified;
                        dbc.SaveChanges();
                    }
                }
                catch
                {

                }
            }
            CorrectPlanZakaz correctPlanZakaz = new CorrectPlanZakaz(editPZ);
            editPZ = correctPlanZakaz.PZ_PlanZakaz;
            db.Entry(editPZ).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TableOrders(int[] Id)
        {
            string part = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Таблички\" + DateTime.Now.ToShortDateString() + ".txt";
            string[] body = GetFileBodyCRD(Id);
            System.IO.File.WriteAllLines(part, body);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        private string[] GetFileBodyCRD(int[] Id)
        {
            int sizeArray = Id.Count() * 7 + 3;
            int stepArray = 3;
            int counterStep = 0;
            string[] body = new string[sizeArray];
            body[0] = "sizeArray = " + Id.Count().ToString() + "\n";
            body[1] += "Dim cardArray(" + (Id.Count() - 1).ToString() + ") As CardClass";
            body[2] += "counterStep = 0" + "\n";
            foreach (var data in Id)
            {
                var order = db.PZ_PlanZakaz.Find(data);
                body[stepArray] += "Set cardArray(" + counterStep.ToString() + ") = New CardClass";
                stepArray++;
                if (order.massa < 1000)
                {
                    body[stepArray] += "cardArray(" + counterStep.ToString() + ").Weight = " + @"""" + "          кг." + @"""";
                    stepArray++;
                }
                else
                {
                    body[stepArray] += "cardArray(" + counterStep.ToString() + ").Weight = " + @"""" + order.massa.ToString() + " кг." + @"""";
                    stepArray++;
                }
                body[stepArray] += "cardArray(" + counterStep.ToString() + ").name = " + @"""" + order.Name + @"""";
                stepArray++;
                body[stepArray] += "cardArray(" + counterStep.ToString() + ").NameT = " + @"""" + order.nameTU + @"""";
                stepArray++;
                body[stepArray] += "cardArray(" + counterStep.ToString() + ").Num = " + @"""" + order.PlanZakaz.ToString() + @"""";
                stepArray++;
                body[stepArray] += "cardArray(" + counterStep.ToString() + ").TU = " + @"""" + order.PZ_ProductType.tu + @"""";
                stepArray++;
                body[stepArray] += "cardArray(" + counterStep.ToString() + ").Year = " + @"""" + DateTime.Now.Year.ToString() + @"""";
                stepArray++;
                counterStep++;
            }
            return body;
        }

        private string ExctraxtIni(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return string.Empty;
            string[] results = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            switch (results.Length)
            {
                case 1:
                    return results[0];
                case 2:
                    return string.Format("{0} {1}.", results[0], results[1].Substring(0, 1));
                default:
                    return string.Format("{0} {1}. {2}", results[0], results[1].Substring(0, 1), results[2].Substring(0, 1) + ".");
            }
        }

        PZ_PlanZakaz GetUpdateGP(PZ_PlanZakaz queryFirst, PZ_PlanZakaz pZ_PlanZakaz)
        {
            queryFirst.PostAdresGruzopoluchatel = pZ_PlanZakaz.PostAdresGruzopoluchatel;
            queryFirst.INNGruzopoluchatel = pZ_PlanZakaz.INNGruzopoluchatel;
            queryFirst.OKPOGruzopoluchatelya = pZ_PlanZakaz.OKPOGruzopoluchatelya;
            queryFirst.KodGruzopoluchatela = pZ_PlanZakaz.KodGruzopoluchatela;
            queryFirst.StantionGruzopoluchatel = pZ_PlanZakaz.StantionGruzopoluchatel;
            queryFirst.KodStanciiGruzopoluchatelya = pZ_PlanZakaz.KodGruzopoluchatela;
            queryFirst.OsobieOtmetkiGruzopoluchatelya = pZ_PlanZakaz.OsobieOtmetkiGruzopoluchatelya;
            queryFirst.DescriptionGruzopoluchatel = pZ_PlanZakaz.DescriptionGruzopoluchatel;

            return queryFirst;
        }

        public JsonResult GetGP(string Id)
        {
            List<PZ_PlanZakaz> query = new List<PZ_PlanZakaz>();
            PZ_PlanZakaz pZ_PlanZakaz = new PZ_PlanZakaz();
            try
            {
                pZ_PlanZakaz = db.PZ_PlanZakaz.Where(d => d.Gruzopoluchatel.Replace(" ", "").Replace("\"", "") == Id).OrderByDescending(d => d.dataOtgruzkiBP).First();
            }
            catch
            {

            }
            query.Add(pZ_PlanZakaz);
            var data = query.Select(dataList => new
            {
                dataList.PostAdresGruzopoluchatel,
                dataList.INNGruzopoluchatel,
                dataList.OKPOGruzopoluchatelya,
                dataList.KodGruzopoluchatela,
                dataList.StantionGruzopoluchatel,
                dataList.KodStanciiGruzopoluchatelya,
                dataList.OsobieOtmetkiGruzopoluchatelya,
                dataList.DescriptionGruzopoluchatel
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public string RemoveDiacritics(string s)
        {
            IEnumerable<char> chars = s.Normalize(NormalizationForm.FormD);
            return new string(chars.Where(c => c < 0x7f && !char.IsControl(c)).ToArray());
        }

        public JsonResult GetRemOrder(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.PZ_Notes
                .AsNoTracking()
                .Include(d => d.PZ_PZNotes.Select(c => c.PZ_PlanZakaz))
                .Include(d => d.AspNetUsers)
                .Where(d => d.PZ_PZNotes.Where(c => c.id_PZ_PlanZakaz == Id).Count() > 0)
                .ToList();
            var data = query.Select(dataList => new
            {
                remCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settingsLong).Replace(@"""", ""),
                remNote = dataList.note,
                remUser = dataList.AspNetUsers.CiliricalName
            });
            return Json(new { data });
        }

        public JsonResult AddRem(PZ_PlanZakaz pZ_PlanZakaz, string textRem)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            int idPZ = db.PZ_PlanZakaz.First(d => d.PlanZakaz == pZ_PlanZakaz.Id).Id;
            PZ_Notes pZ_Notes = new PZ_Notes
            {
                note = textRem,
                dateTimeCreate = DateTime.Now,
                id_AspNetUsersCreate = db.AspNetUsers
                                            .First(d => d.Email == HttpContext.User.Identity.Name)
                                            .Id
            };
            db.PZ_Notes.Add(pZ_Notes);
            db.SaveChanges();
            PZ_PZNotes pZ_PZNotes = new PZ_PZNotes();
            pZ_PZNotes.id_PZ_Notes = pZ_Notes.id;
            pZ_PZNotes.id_PZ_PlanZakaz = idPZ;
            db.PZ_PZNotes.Add(pZ_PZNotes);
            db.SaveChanges();
            return Json(idPZ, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWeightData(int Id)
        {
            using (PortalKATEKEntities dbc = new PortalKATEKEntities())
            {
                var query = db.PZ_PlanZakaz
                    .AsNoTracking()
                    .Where(d => d.Id == Id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    idWeight = dataList.Id,
                    orderNumberWeight = dataList.PlanZakaz,
                    massaWeight = dataList.massa
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateWeightData(int idWeight, double massaWeight)
        {
            string login = HttpContext.User.Identity.Name;
            PZ_PlanZakaz editPZ = db.PZ_PlanZakaz.First(d => d.Id == idWeight);
            if (editPZ.massa != massaWeight)
            {
                try
                {
                    EmailRename emailRename = new EmailRename(editPZ.PlanZakaz.ToString(), editPZ.massa, massaWeight, login, false);
                    emailRename.SendEmailMassa();
                }
                catch
                {

                }
                editPZ.massa = massaWeight;
                try
                {
                    using (ExportImportEntities dbc = new ExportImportEntities())
                    {
                        var pzImport = dbc.planZakaz.First(d => d.Zakaz == editPZ.PlanZakaz);
                        pzImport.weight = editPZ.massa;
                        dbc.Entry(pzImport).State = EntityState.Modified;
                        dbc.SaveChanges();
                    }
                }
                catch
                {

                }
            }
            CorrectPlanZakaz correctPlanZakaz = new CorrectPlanZakaz(editPZ);
            editPZ = correctPlanZakaz.PZ_PlanZakaz;
            db.Entry(editPZ).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }


        public void GetReferenceList()
        {
            string leg1 = "№п/п";
            string leg2 = "Сроки выполнения (год и месяц начала выполнения - год и месяц фактического или планируемого окончания выполнения)";
            string leg3 = "Заказчик (наименование, адрес, контактное лицо с указанием должности, контактные телефоны)";
            string leg4 = "Описание договора (объем и состав поставок, работ (услуг), описание основных условий договора)";
            string leg5 = "Сумма договора, рублей";
            string leg6 = "Сведения о рекламациях по перечисленным договорам, процент завершенности выполнения";
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            int thisYear = DateTime.Now.Year - 3;
            List<ContractData> contractDatas = new List<ContractData>();
            var ordersList = db.PZ_PlanZakaz.AsNoTracking()
                .Include(d => d.PZ_Client)
                .Where(d => d.timeContractDate.Value.Year >= thisYear)
                .ToList();
            List<CorrectOrderList> correctSortList = new List<CorrectOrderList>();
            foreach (var data in ordersList)
            {
                correctSortList.Add(new CorrectOrderList(data));
            }
            correctSortList = correctSortList.Where(d => d.SortDate.Year >= thisYear).OrderBy(d => d.SortDate).ToList();
            foreach (var order in correctSortList)
            {
                int countElements = contractDatas.Where(d => d.Client == order.PZ.PZ_Client.Name && d.Number == order.PZ.timeContract && d.NumberCh == order.PZ.timeArr).Count();
                if (countElements == 0)
                {
                    try
                    {
                        ContractData contract = new ContractData(order.PZ.PZ_Client.Name, order.PZ.timeContract, order.PZ.timeArr, order.PZ.timeContractDate.Value, order.PZ.timeArrDate);
                        var thisOrdersList = db.PZ_PlanZakaz.AsNoTracking()
                            .Include(a => a.PZ_TEO.Select(b => b.PZ_Currency))
                            .Include(a => a.PZ_Client)
                            .Where(a => a.PZ_Client.Name == contract.Client && a.timeContract == contract.Number && a.timeArr == contract.NumberCh)
                            .Where(a => a.PZ_TEO.Max(d => d.OtpuskChena) > 0)
                            .ToList();
                        contract.Orders = new Order[thisOrdersList.Count];
                        int i = 0;
                        foreach (var thisOrder in thisOrdersList)
                        {
                            contract.Orders[i] = new Order(thisOrder.Name, thisOrder.DateCreate, thisOrder.dataOtgruzkiBP, thisOrder.PZ_TEO.First().OtpuskChena, thisOrder.PZ_TEO.First().PZ_Currency.Name);
                            i++;
                        }
                        contractDatas.Add(contract);
                    }
                    catch
                    {

                    }
                }
            }
            contractDatas = contractDatas.OrderBy(d => d.OrderBy.Year).ToList();
            ExcelPackage pck = new ExcelPackage();
            int rowNumber = 2;
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(thisYear.ToString());
            ws.Cells[string.Format("A{0}", rowNumber)].Value = leg1;
            ws.Cells[string.Format("A{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("A{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("B{0}", rowNumber)].Value = leg2;
            ws.Cells[string.Format("B{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("B{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("C{0}", rowNumber)].Value = leg3;
            ws.Cells[string.Format("C{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("C{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("D{0}", rowNumber)].Value = leg4;
            ws.Cells[string.Format("D{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("D{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("E{0}", rowNumber)].Value = leg5;
            ws.Cells[string.Format("E{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("E{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("F{0}", rowNumber)].Value = leg6;
            ws.Cells[string.Format("F{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("F{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            rowNumber = 3;
            string lastSheetName = thisYear.ToString();
            int contractNum = 1;
            foreach (var data in contractDatas)
            {
                ws.Column(1).Width = 6;
                ws.Column(2).Width = 16;
                ws.Column(3).Width = 75;
                ws.Column(4).Width = 75;
                ws.Column(5).Width = 14;
                ws.Column(6).Width = 14;

                ws.Column(1).Style.WrapText = true;
                ws.Column(2).Style.WrapText = true;
                ws.Column(3).Style.WrapText = true;
                ws.Column(4).Style.WrapText = true;
                ws.Column(5).Style.WrapText = true;
                ws.Column(6).Style.WrapText = true;

                ws.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                if (data.Orders.Length != 0)
                {
                    if (lastSheetName != data.OrderBy.Year.ToString())
                    {
                        rowNumber = 2;
                        ws = pck.Workbook.Worksheets.Add(data.OrderBy.Year.ToString());
                        ws.Cells[string.Format("A{0}", rowNumber)].Value = leg1;
                        ws.Cells[string.Format("A{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("A{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells[string.Format("B{0}", rowNumber)].Value = leg2;
                        ws.Cells[string.Format("B{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("B{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells[string.Format("C{0}", rowNumber)].Value = leg3;
                        ws.Cells[string.Format("C{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("C{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells[string.Format("D{0}", rowNumber)].Value = leg4;
                        ws.Cells[string.Format("D{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("D{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells[string.Format("E{0}", rowNumber)].Value = leg5;
                        ws.Cells[string.Format("E{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("E{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        ws.Cells[string.Format("F{0}", rowNumber)].Value = leg6;
                        ws.Cells[string.Format("F{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("F{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        rowNumber = 3;
                        lastSheetName = data.OrderBy.Year.ToString();
                        contractNum = 1;
                    }

                    ws.Cells[string.Format("A{0}", rowNumber)].Value = contractNum.ToString() + ".";
                    ws.Cells[string.Format("A{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("A{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[rowNumber, 2, rowNumber, 6].Merge = true;

                    ws.Cells[string.Format("B{0}", rowNumber)].Value = GetContractName(data);
                    ws.Cells[string.Format("B{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("B{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells[string.Format("C{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("D{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("E{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[string.Format("F{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    rowNumber++;
                    foreach (var data1 in data.Orders)
                    {
                        ws.Cells[string.Format("A{0}", rowNumber)].Value = "";
                        ws.Cells[string.Format("A{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("A{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("B{0}", rowNumber)].Value = data1.Start.Year + "." + data1.Start.Month + " - " + data1.Finish.Value.Year + "." + data1.Finish.Value.Month;
                        ws.Cells[string.Format("B{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("B{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells[string.Format("C{0}", rowNumber)].Value = data.Client;
                        ws.Cells[string.Format("C{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("C{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[string.Format("D{0}", rowNumber)].Value = data1.Name;
                        ws.Cells[string.Format("D{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("D{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        ws.Cells[string.Format("E{0}", rowNumber)].Value = data1.Sale;
                        ws.Cells[string.Format("E{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("E{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        if (data1.Curency != "RUB")
                        {
                            ws.Cells[string.Format("E{0}", rowNumber)].Style.Fill.PatternType = ExcelFillStyle.Gray0625;
                        }
                        ws.Cells[string.Format("F{0}", rowNumber)].Value = "";
                        ws.Cells[string.Format("F{0}", rowNumber)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[string.Format("F{0}", rowNumber)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        rowNumber++;
                    }
                    contractNum++;
                }
            }
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-daisposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public ActionResult GetRefT2()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var ordersList = db.PZ_PlanZakaz
                    .AsNoTracking()
                    .Include(a => a.PZ_Client)
                    .Include(a => a.Provider)
                    .Include(a => a.PZ_FIO)
                    .Include(a => a.PZ_TEO.Select(b => b.PZ_Currency))
                    .OrderBy(a => a.DateCreate)
                    .Where(a => a.Client != 39)
                    .Where(a => a.PlanZakaz != 1)
                    .Where(a => a.DateCreate.Year > 2014)
                    .ToList();
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    worksheet["A1"].ColumnWidth = 6.0;
                    worksheet["B1"].ColumnWidth = 35.0;
                    worksheet["C1"].ColumnWidth = 60.0;
                    worksheet["D1"].ColumnWidth = 45.0;
                    worksheet["E1"].ColumnWidth = 50.0;
                    worksheet["F1"].ColumnWidth = 20.0;
                    worksheet["G1"].ColumnWidth = 20.0;
                    worksheet["H1"].ColumnWidth = 10.0;

                    worksheet["A1"].Text = "№ п/п";
                    worksheet["B1"].Text = "№ договора, дата";
                    worksheet["C1"].Text = "Тип оборудования";
                    worksheet["D1"].Text = "Заказчик";
                    worksheet["E1"].Text = "Куратор договора (ФИО), контактный телефон (Заказчика)";
                    worksheet["F1"].Text = "Сумма договора";
                    worksheet["G1"].Text = "Поставщик";
                    worksheet["H1"].Text = "Дата регистрации";

                    worksheet.Range["A1:H1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range["A1:H1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet.Range["A1:H1"].WrapText = true;

                    int rowNum = 2;
                    string fio = "";
                    foreach (var order in ordersList)
                    {
                        worksheet.Range[rowNum, 1].Text = (rowNum - 1).ToString();
                        worksheet.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 1].WrapText = true;

                        try
                        {
                            worksheet.Range[rowNum, 2].Text = order.timeContract + " от " + order.timeContractDate.Value.ToShortDateString();
                            worksheet.Range[rowNum, 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            worksheet.Range[rowNum, 2].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheet.Range[rowNum, 2].WrapText = true;
                        }
                        catch
                        {
                            worksheet.Range[rowNum, 2].Text = "";
                            worksheet.Range[rowNum, 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            worksheet.Range[rowNum, 2].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheet.Range[rowNum, 2].WrapText = true;
                        }

                        worksheet.Range[rowNum, 3].Text = order.Name;
                        worksheet.Range[rowNum, 3].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 3].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 3].WrapText = true;

                        worksheet.Range[rowNum, 4].Text = order.PZ_Client.Name;
                        worksheet.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 4].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 4].WrapText = true;

                        fio = order.PZ_FIO.fio + " " + order.PZ_FIO.i + " " + order.PZ_FIO.o + ", " + order.PZ_FIO.email + ", " + order.PZ_FIO.phone + ", " + order.PZ_FIO.position;
                        if (fio == "- - -, -, -, -")
                            worksheet.Range[rowNum, 5].Text = "";
                        else
                            worksheet.Range[rowNum, 5].Text = order.PZ_FIO.fio + " " + order.PZ_FIO.i + " " + order.PZ_FIO.o + ", " + order.PZ_FIO.email + ", " + order.PZ_FIO.phone + ", " + order.PZ_FIO.position;
                        worksheet.Range[rowNum, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 5].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 5].WrapText = true;

                        worksheet.Range[rowNum, 6].Number = order.PZ_TEO.First().OtpuskChena;
                        worksheet.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 6].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 6].WrapText = true;

                        worksheet.Range[rowNum, 7].Text = order.Provider.name;
                        worksheet.Range[rowNum, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 7].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 7].WrapText = true;

                        worksheet.Range[rowNum, 8].Text = order.DateCreate.ToShortDateString();
                        worksheet.Range[rowNum, 8].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 8].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 8].WrapText = true;

                        rowNum++;
                    }
                    HttpResponse response = HttpContext.ApplicationInstance.Response;
                    try
                    {
                        workbook.SaveAs("Референс.xlsx", HttpContext.ApplicationInstance.Response, ExcelDownloadType.Open);
                    }
                    catch
                    {
                    }
                }
            }
            return View();
        }

        string GetContractName(ContractData data)
        {
            if (data.NumberCh != "")
                try
                {
                    return "Договор №: " + data.Number + " от " + data.Date.ToShortDateString() + ". Приложение №: " + data.NumberCh + " от " + data.DateCh.Value.ToShortDateString();
                }
                catch
                {
                    return "Договор №: " + data.Number + " от " + data.Date.ToShortDateString();
                }
            else
                return "Договор №: " + data.Number + " от " + data.Date.ToShortDateString();
        }
    }
}