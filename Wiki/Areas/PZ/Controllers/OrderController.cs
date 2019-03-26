using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.PZ.Models;


namespace Wiki.Areas.PZ.Controllers
{
    public class OrderController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly string firstPartLinkEditOP = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('";
        readonly string firstPartLinkEditKO = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyKOID('";
        readonly string lastPartEdit = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";

        public ActionResult Index()
        {
            ViewBag.Orders = new SelectList(db.PZ_PlanZakaz.Where(d => d.PlanZakaz < 9000).OrderByDescending(x => x.PlanZakaz), "id", "PlanZakaz");
            ViewBag.Manager = new SelectList(db.AspNetUsers.Where(d => d.LockoutEnabled == true).Where(x => x.Devision == 5 || x.CiliricalName == "Антипов Эдуард Валерьевич" || x.CiliricalName == "Брель Андрей Викторович").OrderBy(x => x.CiliricalName), "id", "CiliricalName");
            ViewBag.Client = new SelectList(db.PZ_Client.OrderBy(x => x.NameSort), "id", "NameSort");
            ViewBag.Dostavka = new SelectList(db.PZ_Dostavka.OrderBy(d => d.Name), "id", "Name");
            ViewBag.ProductType = new SelectList(db.PZ_ProductType.OrderBy(d => d.ProductType), "id", "ProductType");
            ViewBag.id_PZ_OperatorDogovora = new SelectList(db.PZ_OperatorDogovora.OrderBy(x => x.name), "id", "name");
            ViewBag.id_PZ_FIO = new SelectList(db.PZ_FIO.OrderBy(x => x.fio), "id", "fio");
            ViewBag.TypeShip = new SelectList(db.PZ_TypeShip.OrderBy(x => x.typeShip), "id", "typeShip");
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
            var query = db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).ToList();
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
            if (devision == 3 || devision == 15 || devision == 16)
            {
                linkPartOne = firstPartLinkEditKO;
                linkPartTwo = lastPartEdit;
            }
            else if (devision == 5 || login == "myi@katek.by" || login == "gea@katek.by")
            {
                linkPartOne = firstPartLinkEditOP;
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
                Manager = ExctraxtIni(dataList.AspNetUsers.CiliricalName),
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
            int countLastOrdersView = 200;
            var query = db.PZ_PlanZakaz.OrderByDescending(d => d.DateCreate).Take(countLastOrdersView).ToList();
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
            if (devision == 3 || devision == 15 || devision == 16)
            {
                linkPartOne = firstPartLinkEditKO;
                linkPartTwo = lastPartEdit;
            }
            else if (devision == 5 || login == "myi@katek.by" || login == "gea@katek.by")
            {
                linkPartOne = firstPartLinkEditOP;
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
                Manager = ExctraxtIni(dataList.AspNetUsers.CiliricalName),
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
            var query = db.PZ_PlanZakaz.Where(d => d.DateCreate.Year == yearCreateOrder).ToList();
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
            if (devision == 3 || devision == 15 || devision == 16)
            {
                linkPartOne = firstPartLinkEditKO;
                linkPartTwo = lastPartEdit;
            }
            else if (devision == 5 || login == "myi@katek.by" || login == "gea@katek.by")
            {
                linkPartOne = firstPartLinkEditOP;
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
                Manager = ExctraxtIni(dataList.AspNetUsers.CiliricalName),
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
            var query = db.PZ_PlanZakaz.ToList();
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
            if (devision == 3 || devision == 15 || devision == 16)
            {
                linkPartOne = firstPartLinkEditKO;
                linkPartTwo = lastPartEdit;
            }
            else if (devision == 5 || login == "myi@katek.by" || login == "gea@katek.by")
            {
                linkPartOne = firstPartLinkEditOP;
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
                Manager = ExctraxtIni(dataList.AspNetUsers.CiliricalName),
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
                //SF = "",
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, settings).Replace(@"""", "")
                //dateDostavki = "",
                //datePriemki = "",
                //dateOplat = ""
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

        public JsonResult GetOrder(int Id)
        {
            var query = db.PZ_PlanZakaz.Where(d => d.Id == Id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.PlanZakaz,
                DateCreate = JsonConvert.SerializeObject(dataList.DateCreate, settings).Replace(@"""", ""),
                dataList.Manager,
                dataList.Client,
                dataList.id_PZ_OperatorDogovora,
                dataList.id_PZ_FIO,
                dataList.Name,
                dataList.Description,
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
            db.Entry(editPZ).State = System.Data.Entity.EntityState.Modified;
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
                if (pZ_PlanZakaz.Client != 0)
                    editPZ.Client = pZ_PlanZakaz.Client;
                if (pZ_PlanZakaz.id_PZ_OperatorDogovora != 0)
                    editPZ.id_PZ_OperatorDogovora = pZ_PlanZakaz.id_PZ_OperatorDogovora;
                if (pZ_PlanZakaz.id_PZ_FIO != 0)
                    editPZ.id_PZ_FIO = pZ_PlanZakaz.id_PZ_FIO;
                if (pZ_PlanZakaz.Name != null)
                {
                    if (editPZ.Name != pZ_PlanZakaz.Name)
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
                db.Entry(editPZ).State = System.Data.Entity.EntityState.Modified;
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
                dataList.nameTU,
                dataList.Id
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateKO(PZ_PlanZakaz pZ_PlanZakaz)
        {
            string login = HttpContext.User.Identity.Name;
            PZ_PlanZakaz editPZ = db.PZ_PlanZakaz.First(d => d.PlanZakaz == pZ_PlanZakaz.PlanZakaz);
            if (editPZ.nameTU != pZ_PlanZakaz.nameTU)
            {
                EmailRename emailRename = new EmailRename(editPZ.PlanZakaz.ToString(), editPZ.nameTU, pZ_PlanZakaz.nameTU, login, true);
                emailRename.SendEmail();
                editPZ.nameTU = pZ_PlanZakaz.nameTU;
            }
            CorrectPlanZakaz correctPlanZakaz = new CorrectPlanZakaz(editPZ);
            editPZ = correctPlanZakaz.PZ_PlanZakaz;
            db.Entry(editPZ).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult TableOrders(int[] Id)
        {
            string part = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Таблички\" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + "_" + DateTime.Now.Minute + DateTime.Now.Second + "_" + "(";
            foreach (var data in Id)
            {
                part += db.PZ_PlanZakaz.Find(data).PlanZakaz.ToString() + ", ";
            }
            part += ").docx";
            GeneratedTablesOreder generatedTablesOreder = new GeneratedTablesOreder(Id);
            generatedTablesOreder.CreatePackage(part, Id.Length);

            return Json(1, JsonRequestBehavior.AllowGet);
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
    }
}