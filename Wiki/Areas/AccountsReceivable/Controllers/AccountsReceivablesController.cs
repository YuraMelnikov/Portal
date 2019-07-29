using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Areas.AccountsReceivable.Models;

namespace Wiki.Areas.AccountsReceivable.Controllers
{
    public class AccountsReceivablesController : Controller
    {
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        private HttpPostedFileBase[] fileUploadArray;
        public ActionResult Index()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            ViewBag.userTP = new SelectList(db.AspNetUsers.Where(d => d.Devision == 4).OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            ViewBag.currency = new SelectList(db.PZ_Currency.OrderBy(d => d.Name), "id", "Name");
            ViewBag.orders = new SelectList(db.PZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.ProductType = new SelectList(db.PZ_ProductType.OrderBy(d => d.ProductType), "id", "ProductType");
            return View();
        }

        public JsonResult TEOList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_TEO
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .Include(d => d.PZ_Currency)
                    .Where(d => d.PZ_PlanZakaz.PlanZakaz < 9000)
                    .OrderByDescending(d => d.PZ_PlanZakaz.PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getTEO('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    order = dataList.PZ_PlanZakaz.PlanZakaz,
                    dataList.Rate,
                    dataList.SSM,
                    dataList.SSR,
                    dataList.IzdKom,
                    dataList.IzdPPKredit,
                    dataList.PI,
                    dataList.NOP,
                    dataList.KI_S,
                    dataList.KI_prochee,
                    dataList.OtpuskChena,
                    Currency = dataList.PZ_Currency.Name,
                    dataList.NDS
                });
                return Json(new { data });
            }
        }

        public JsonResult TasksList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.TaskForPZ.AspNetUsers)
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Where(d => d.close == false && d.TaskForPZ.AspNetUsers.CiliricalName != "")
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "",
                    viewLink = "",
                    order = dataList.PZ_PlanZakaz.PlanZakaz,
                    dataList.TaskForPZ.taskName,
                    client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    user = dataList.TaskForPZ.AspNetUsers.CiliricalName,
                    planDate = JsonConvert.SerializeObject(dataList.datePlan, shortSetting).Replace(@"""", "")
                });
                return Json(new { data });
            }
        }

        public JsonResult TasksCloseList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.TaskForPZ.AspNetUsers)
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Where(d => d.close == true)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "",
                    viewLink = "",
                    order = dataList.PZ_PlanZakaz.PlanZakaz,
                    dataList.TaskForPZ.taskName,
                    client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    user = dataList.TaskForPZ.AspNetUsers.CiliricalName,
                    planDate = JsonConvert.SerializeObject(dataList.datePlan, shortSetting).Replace(@"""", "")
                });
                return Json(new { data });
            }
        }

        public JsonResult MyTasksList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string login = HttpContext.User.Identity.Name;
                if (login == "naa@katek.by" || login == "kns@katek.by" || login == "Drozdov@katek.by" || login == "myi@katek.by")
                {
                    string userId = db.AspNetUsers.First(d => d.Email == login).Id;
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Debit_WorkBit
                        .AsNoTracking()
                        .Include(d => d.TaskForPZ.AspNetUsers)
                        .Include(d => d.PZ_PlanZakaz.PZ_Client)
                        .Where(d => d.close == false && d.TaskForPZ.id_User == userId && d.id_TaskForPZ != 8)
                        .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = GetLinkTaskFin(dataList.id_TaskForPZ),
                        viewLink = "",
                        order = dataList.PZ_PlanZakaz.PlanZakaz,
                        dataList.TaskForPZ.taskName,
                        client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                        user = dataList.TaskForPZ.AspNetUsers.CiliricalName,
                        planDate = JsonConvert.SerializeObject(dataList.datePlan, shortSetting).Replace(@"""", "")
                    });
                    return Json(new { data });
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Debit_WorkBit
                        .AsNoTracking()
                        .Include(d => d.TaskForPZ.AspNetUsers)
                        .Include(d => d.PZ_PlanZakaz.PZ_Client)
                        .Where(d => d.id == 0)
                        .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "",
                        viewLink = "",
                        order = dataList.PZ_PlanZakaz.PlanZakaz,
                        dataList.TaskForPZ.taskName,
                        client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                        user = dataList.TaskForPZ.AspNetUsers.CiliricalName,
                        planDate = JsonConvert.SerializeObject(dataList.datePlan, shortSetting).Replace(@"""", "")
                    });
                    return Json(new { data });
                }
            }
        }

        string GetLinkTaskFin(int idTypeTask)
        {
            if (idTypeTask == 11)
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getTN('" + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else if (idTypeTask == 10)
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getLetter('10" + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else if (idTypeTask == 26)
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getCostSh('" + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "";
        }

        string GetEditLinkMyTask(int id, int id_type)
        {
            string link = "";
            link += "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return";
            link += GetFunctionJSTask(id_type);
            link += "('";
            //link += GetIdForLink(id);
            link += "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
            return link;
        }

        string GetFunctionJSTask(int id_type)
        {
            string functName = "";
            if(id_type == 1 || id_type == 3)
            {
                functName = "getDefault";
            }
            else if (id_type == 2)
            {
                functName = "getTEO";
            }
            else if (id_type == 11)
            {
                functName = "getTN";
            }
            else if (id_type == 4)
            {
                functName = "getSetup";
            }
            else if (id_type == 5 || id_type == 6 || id_type == 7 || id_type == 10 || id_type == 12 || id_type == 25)
            {
                functName = "getLetter";
            }
            else if (id_type == 26)
            {
                functName = "getCostSh";
            }
            return functName;
        }

        public JsonResult ContractList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_Setup
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                    .Where(d => d.PZ_PlanZakaz.DateCreate.Year > 2017)
                    .Where(d => d.PZ_PlanZakaz.Client != 39)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getSetup('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
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

        public JsonResult DebitList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Include(d => d.PZ_PlanZakaz.PZ_Setup)
                    .Where(d => d.id_TaskForPZ == 15 || d.id_TaskForPZ == 38)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    edit = "<a href =" + '\u0022' + "http://pserver/Deb/Upload/NewPlus/" + dataList.id + '\u0022' + " class=" + '\u0022' + "btn-xs btn-primary" + '\u0022' + "role =" + '\u0022' + "button" + '\u0022' + ">Внести</a>",
                    dataList.PZ_PlanZakaz.PlanZakaz,
                    Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                    Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    dataList.PZ_PlanZakaz.PZ_Setup.First().KolVoDneyNaPrijemku,
                    dataList.PZ_PlanZakaz.PZ_Setup.First().PunktDogovoraOSrokahPriemki,
                    dataList.PZ_PlanZakaz.PZ_Setup.First().UslovieOplatyText,
                    status = GetStatusName(dataList)
                });
                return Json(new { data });
            }
        }

        string GetStatusName(Debit_WorkBit debit_WorkBit)
        {
            string statusName = "Не оплачен";
            if (debit_WorkBit.close == true)
                statusName = "Оплачен";
            if (debit_WorkBit.id_TaskForPZ == 38)
                statusName = "Внести предоплату";
            return statusName;
        }

        public JsonResult GetDefault(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .Where(d => d.id == id)
                    .Include(d => d.TaskForPZ)
                    .Include(d => d.PZ_PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    defaultId = dataList.id,
                    defaultTaskName = dataList.TaskForPZ.taskName + " по заказу: " + dataList.PZ_PlanZakaz.PlanZakaz.ToString()
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateDefault(int id, bool checkedDefault)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(id);
                if (checkedDefault == true)
                {
                    debit_WorkBit.close = true;
                    debit_WorkBit.dateClose = DateTime.Now;
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTEO(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_TEO
                    .Where(d => d.id == id)
                    .Include(d => d.PZ_PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    idTEO = dataList.id,
                    dataList.Currency,
                    teoPlanZakaz = "Заказ №: " + dataList.PZ_PlanZakaz.PlanZakaz.ToString(),
                    dataList.Rate,
                    dataList.SSM,
                    dataList.SSR,
                    dataList.IzdKom,
                    dataList.IzdPPKredit,
                    dataList.PI,
                    dataList.NOP,
                    dataList.KI_S,
                    dataList.KI_prochee,
                    dataList.OtpuskChena,
                    dataList.KursValuti,
                    dataList.Id_PlanZakaz,
                    dataList.NDS
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateTEO(PZ_TEO pZ_TEO)
        {
            pZ_TEO.KursValuti = pZ_TEO.KursValuti / 10000;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                db.Entry(pZ_TEO).State = EntityState.Modified;
                db.SaveChanges();
                return Json(4, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSetup(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_Setup
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.id == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    pzSetup = "Заказ №: " + dataList.PZ_PlanZakaz.PlanZakaz.ToString(),
                    idSetup = dataList.id,
                    dataList.id_PZ_PlanZakaz,
                    dataList.KolVoDneyNaPrijemku,
                    dataList.PunktDogovoraOSrokahPriemki,
                    dataList.UslovieOplatyText,
                    dataList.UslovieOplatyInt,
                    dataList.TimeNaRKD,
                    dataList.RassmotrenieRKD,
                    dataList.SrokZamechanieRKD,
                    dataList.userTP
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateSetup(PZ_Setup pZ_Setup)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                db.Entry(pZ_Setup).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLetter(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                if(id == 10)
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var sucursalList = db.Debit_WorkBit
                        .AsNoTracking()
                        .Include(d => d.PZ_PlanZakaz)
                        .Where(d => d.close == false && d.id_TaskForPZ == id)
                        .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz);
                    var data = sucursalList.Select(m => new SelectListItem()
                    {
                        Text = m.PZ_PlanZakaz.PlanZakaz.ToString(),
                        Value = m.PZ_PlanZakaz.Id.ToString(),
                    });
                    return Json(data.ToList(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var sucursalList = db.Debit_WorkBit
                        .AsNoTracking()
                        .Include(d => d.PZ_PlanZakaz)
                        .Where(d => d.close == false && d.id_TaskForPZ == 0)
                        .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz);
                    var data = sucursalList.Select(m => new SelectListItem()
                    {
                        Text = m.PZ_PlanZakaz.PlanZakaz.ToString(),
                        Value = m.PZ_PlanZakaz.Id.ToString(),
                    });
                    return Json(data.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult UpdateLetter(int[] pZ_PlanZakazLetters, HttpPostedFileBase[] ofile1, 
            DateTime datePost, string numPost, DateTime datePrihod)
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                foreach (var pz in pZ_PlanZakazLetters)
                {
                    Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.First(d => d.close == false && d.id_PlanZakaz == pz && d.id_TaskForPZ == 10);
                    debit_WorkBit.close = true;
                    debit_WorkBit.dateClose = DateTime.Now;
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                    PostAlertShip postAlertShip = new PostAlertShip
                    {
                        id_Debit_WorkBit = debit_WorkBit.id,
                        datePost = datePost,
                        numPost = numPost,
                        datePrihod = datePrihod
                    };
                    db.PostAlertShip.Add(postAlertShip);
                    db.SaveChanges();
                    if (ofile1[0] != null)
                    {
                        PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(pz);
                        fileUploadArray = ofile1;
                        CreateFolderAndFileForPreOrder(pZ_PlanZakaz.Folder);
                        string subject = "Получено письмо от экспедитора о поставке заказа: " + pZ_PlanZakaz.PlanZakaz.ToString();
                        new EmailAccountsReceivable(pZ_PlanZakaz.Folder, login, subject);
                    }
                }
                return RedirectToAction("Index");
            }
        }

        private void CreateFolderAndFileForPreOrder(string path)
        {
            string directory = path + @"\08_Сопроводительная документация\";
            Directory.CreateDirectory(directory);
            SaveFileToServer(directory);
        }

        private void SaveFileToServer(string folderAdress)
        {
            for (int i = 0; i < fileUploadArray.Length; i++)
            {
                string fileReplace = Path.GetFileName(fileUploadArray[i].FileName);
                fileReplace = ToSafeFileName(fileReplace);
                var fileName = string.Format("{0}\\{1}", folderAdress, fileReplace);
                fileUploadArray[i].SaveAs(fileName);
            }
        }

        private string ToSafeFileName(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }

        public JsonResult GetTN()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var sucursalList = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.close == false && d.id_TaskForPZ == 11)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.PZ_PlanZakaz.PlanZakaz.ToString(),
                    Value = m.PZ_PlanZakaz.Id.ToString(),
                });
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateTN(int[] pZ_PlanZakazTN, string numberTN, string numberSF,
            string numCMR, DateTime dateTN, DateTime dateSF, DateTime dateCMR)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                foreach (var pz in pZ_PlanZakazTN)
                {
                    Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.First(d => d.close == false && d.id_PlanZakaz == pz && d.id_TaskForPZ == 11);
                    debit_WorkBit.close = true;
                    debit_WorkBit.dateClose = DateTime.Now;
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                    Debit_TN debit_TN = new Debit_TN
                    {
                        id_DebitTask = debit_WorkBit.id,
                        numberTN = numberTN,
                        numberSF = numberSF,
                        dateTN = dateTN,
                        dateSF = dateSF,
                        Summa = 0
                    };
                    db.Debit_TN.Add(debit_TN);
                    db.SaveChanges();
                    debit_WorkBit = db.Debit_WorkBit.First(d => d.close == false && d.id_PlanZakaz == pz && d.id_TaskForPZ == 8);
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                    Debit_CMR debit_CMR = new Debit_CMR
                    {
                        id_DebitTask = debit_WorkBit.id,
                        dateShip = dateCMR,
                        number = numCMR
                    };
                    db.Debit_CMR.Add(debit_CMR);
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCostSh()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var sucursalList = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.close == false && d.id_TaskForPZ == 26)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.PZ_PlanZakaz.PlanZakaz.ToString(),
                    Value = m.PZ_PlanZakaz.Id.ToString(),
                });
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateCostSh(int[] pZ_PlanZakazSF, double transportSum, string numberOrder, double ndsSum, int currency)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                foreach (var pz in pZ_PlanZakazSF)
                {
                    Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.First(d => d.close == false && d.id_PlanZakaz == pz && d.id_TaskForPZ == 26);
                    debit_WorkBit.close = true;
                    debit_WorkBit.dateClose = DateTime.Now;
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                    Debit_IstPost debit_IstPost = new Debit_IstPost
                    {
                        id_DebitTask = debit_WorkBit.id,
                        transportSum = transportSum,
                        numberOrder = numberOrder,
                        ndsSum = ndsSum,
                        currency = currency
                    };
                    db.Debit_IstPost.Add(debit_IstPost);
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult TasksPM()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string login = "gea@katek.by";
                //string login = HttpContext.User.Identity.Name;
                string userId = db.AspNetUsers.First(d => d.Email == login).Id;
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .AsNoTracking()
                    .Where(d => d.close == false && d.TaskForPZ.id_User == userId)
                    .ToList();
                List<int> pzList = new List<int>();
                foreach (var workToDeb in query)
                {
                    if (pzList.IndexOf(workToDeb.id_PlanZakaz) == -1)
                        pzList.Add(workToDeb.id_PlanZakaz);
                }
                var data = pzList.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getPM('" + dataList + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    pmOrderPZName = db.PZ_PlanZakaz.Find(dataList).PlanZakaz.ToString(),
                    powerST = db.PZ_PlanZakaz.Find(dataList).PowerST,
                    vnnn = db.PZ_PlanZakaz.Find(dataList).VN_NN,
                    gbb = db.PZ_PlanZakaz.Find(dataList).Modul,
                    orderRegist = JsonConvert.SerializeObject(db.Debit_WorkBit.First(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 1).dateClose, shortSetting).Replace(@"""", ""),
                    teoRegist = JsonConvert.SerializeObject(db.Debit_WorkBit.First(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 2).dateClose, shortSetting).Replace(@"""", ""),
                    planKBM = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 41).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    planKBE = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 42).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    prototypeKBM = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 43).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    prototypeKBE = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 44).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    prototypeKBMComplited = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 45).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    prototypeKBEComplited = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 46).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    contractComplited = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 4).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    mailManuf = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 40).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    mailShComplited = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 12).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    mailSh = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 5).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    sh = JsonConvert.SerializeObject(db.PZ_PlanZakaz.Find(dataList).dataOtgruzkiBP, shortSetting).Replace(@"""", "")
                }).ToList();
                return Json(new { data });
            }
        }

        public JsonResult GetPM(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                List<int> pzList = new List<int>();
                pzList.Add(id);
                var data = pzList.Select(dataList => new
                {
                    idPZ = id,
                    db.PZ_PlanZakaz.Find(dataList).ProductType,
                    pmOrderPZName = db.PZ_PlanZakaz.Find(dataList).PlanZakaz.ToString(),
                    powerST = db.PZ_PlanZakaz.Find(dataList).PowerST,
                    vnnn = db.PZ_PlanZakaz.Find(dataList).VN_NN,
                    gbb = db.PZ_PlanZakaz.Find(dataList).Modul,
                    orderRegist = JsonConvert.SerializeObject(db.Debit_WorkBit.First(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 1).dateClose, shortSetting).Replace(@"""", ""),
                    teoRegist = JsonConvert.SerializeObject(db.Debit_WorkBit.First(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 2).dateClose, shortSetting).Replace(@"""", ""),
                    planKBM = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 41).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    planKBE = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 42).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    prototypeKBM = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 43).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    prototypeKBE = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 44).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    prototypeKBMComplited = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 45).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    prototypeKBEComplited = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 46).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    contractComplited = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 4).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    mailManuf = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 40).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    mailShComplited = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 12).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                    mailSh = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 5).Select(kv => kv.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", "")
                }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}