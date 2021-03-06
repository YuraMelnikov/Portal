﻿using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Areas.AccountsReceivable.Models;
using Wiki.Models;

namespace Wiki.Areas.AccountsReceivable.Controllers
{
    public class AccountsReceivablesController : Controller
    {
        readonly CultureInfo ci = new CultureInfo("en-us");
        private static Logger logger = LogManager.GetCurrentClassLogger();
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly JsonSerializerSettings shortSettingRu = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        private HttpPostedFileBase[] fileUploadArray;
        public ActionResult Index()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            ViewBag.userTP = new SelectList(db.AspNetUsers.Where(d => d.Devision == 4 || d.Devision == 18).OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            ViewBag.currency = new SelectList(db.PZ_Currency.OrderBy(d => d.Name), "id", "Name");
            ViewBag.orders = new SelectList(db.PZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.ProductType = new SelectList(db.PZ_ProductType.OrderBy(d => d.ProductType), "id", "ProductType");

            var pzList = db.PZ_PlanZakaz.Where(d => d.Folder == "").ToList();
            if (pzList.Count > 0)
                GetAdressFolder(pzList);
            try
            {
                string login = HttpContext.User.Identity.Name;
                logger.Debug("AccountsReceivables Index: " + login);
            }
            catch
            {
            }
            return View();
        }

        public void GetAdressFolder(List<PZ_PlanZakaz> pzList)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                foreach(var dataInList in pzList)
                {
                    int substringLan = 0;
                    string adres = db.Folder.Find(1).adres;
                    adres += db.FolderDocument.Find(2).adres;
                    int planZakaz = db.PZ_PlanZakaz.Find(dataInList.Id).PlanZakaz;
                    string[] files = Directory.GetDirectories(adres, "*.*", SearchOption.TopDirectoryOnly);
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            if (Convert.ToInt32(files[i].Substring(files[i].Length - 4)) > planZakaz)
                            {
                                adres = files[i];
                                break;
                            }
                        }
                        catch
                        { }
                    }
                    substringLan = adres.Length + 1;
                    files = null;
                    files = Directory.GetDirectories(adres, "*.*", SearchOption.TopDirectoryOnly);
                    string myLan = "";
                    string myLan2 = "";
                    for (int i = 0; i < files.Length; i++)
                    {
                        myLan = files[i];
                        myLan = files[i].Substring(substringLan, files[i].Length - substringLan);
                        myLan = myLan.Substring(0, 4);
                        try
                        {
                            myLan2 = files[i];
                            myLan2 = files[i].Substring(substringLan, files[i].Length - substringLan);
                            myLan2 = myLan2.Substring(5, 4);
                            int counterStep = Convert.ToInt32(myLan2) - Convert.ToInt32(myLan);
                            for (int j = 0; j <= counterStep; j++)
                            {
                                try
                                {
                                    if (Convert.ToInt32(myLan) + j == planZakaz)
                                    {
                                        adres = files[i];
                                        if (Convert.ToInt32(myLan) + j > planZakaz)
                                        {
                                            adres = files[i - 1];
                                            break;
                                        }
                                        break;
                                    }
                                }
                                catch
                                { }
                            }
                        }
                        catch
                        {
                            try
                            {
                                if (Convert.ToInt32(myLan) == planZakaz)
                                {
                                    adres = files[i];
                                    if (Convert.ToInt32(myLan) > planZakaz)
                                    {
                                        adres = files[i - 1];
                                        break;
                                    }
                                    break;
                                }
                            }
                            catch
                            { }
                        }
                    }
                    PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(dataInList.Id);
                    pZ_PlanZakaz.Folder = adres;
                    db.Entry(pZ_PlanZakaz).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public JsonResult TEOList()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                if (login == "myi@katek.by" || login == "laa@katek.by" || login == "gvi@katek.by")
                {
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
                        dataList.NDS,
                        dataList.SSMProduct,
                        dataList.importDuty,
                        dataList.importDutyPlan
                    });
                    try
                    {
                        logger.Debug("TEOList: " + login);
                    }
                    catch
                    {
                    }
                    return Json(new { data });
                }
                else
                {
                    var query = db.PZ_TEO
                       .AsNoTracking()
                       .Include(d => d.PZ_PlanZakaz)
                       .Include(d => d.PZ_Currency)
                       .Where(d => d.id == 0)
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
                        dataList.NDS,
                        dataList.SSMProduct,
                        dataList.importDuty,
                        dataList.importDutyPlan
                    });
                    try
                    {
                        logger.Debug("TEOList: " + login);
                    }
                    catch
                    {
                    }
                    return Json(new { data });
                }
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
                    try
                    {
                        logger.Debug("MyTasksList: " + login);
                    }
                    catch
                    {
                    }
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
                    UTP = GetUserTP(dataList.userTP),
                    Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                    Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    dataList.KolVoDneyNaPrijemku,
                    dataList.PunktDogovoraOSrokahPriemki,
                    dataList.UslovieOplatyText
                });
                return Json(new { data });
            }
        }

        private string GetUserTP(string id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    return db.AspNetUsers.First(a => a.Id == id).CiliricalName;
                }
                catch
                {
                    return "";
                }
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
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                if (login == "myi@katek.by" || login == "laa@katek.by" || login == "gvi@katek.by")
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Debit_WorkBit
                        .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                        .Include(d => d.PZ_PlanZakaz.PZ_Client)
                        .Include(d => d.PZ_PlanZakaz.PZ_Setup)
                        .Include(d => d.PZ_PlanZakaz.PZ_TEO)
                        .Where(d => d.id_TaskForPZ == 15 && d.PZ_PlanZakaz.dataOtgruzkiBP.Year > 2017)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getDebTask('" + dataList.id_PlanZakaz + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        dataList.PZ_PlanZakaz.PlanZakaz,
                        Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                        Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().KolVoDneyNaPrijemku,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().PunktDogovoraOSrokahPriemki,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().UslovieOplatyText,
                        dateSh = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, shortSetting).Replace(@"""", ""),
                        status = GetStatusName(dataList),
                        sf = GetSF(dataList.id_PlanZakaz),
                        oc = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First(),
                        ocPu = GetocPu(dataList.id_PlanZakaz),
                        otcl = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First() - GetocPu(dataList.id_PlanZakaz)
                    });
                    try
                    {
                        logger.Debug("DebitList: " + login);
                    }
                    catch
                    {
                    }
                    return Json(new { data });
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Debit_WorkBit
                        .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                        .Include(d => d.PZ_PlanZakaz.PZ_Client)
                        .Include(d => d.PZ_PlanZakaz.PZ_Setup)
                        .Include(d => d.PZ_PlanZakaz.PZ_TEO)
                        .Where(d => d.id_TaskForPZ == 0)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getDebTask('" + dataList.id_PlanZakaz + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        dataList.PZ_PlanZakaz.PlanZakaz,
                        Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                        Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().KolVoDneyNaPrijemku,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().PunktDogovoraOSrokahPriemki,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().UslovieOplatyText,
                        dateSh = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, shortSetting).Replace(@"""", ""),
                        status = GetStatusName(dataList),
                        sf = GetSF(dataList.id_PlanZakaz),
                        oc = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First(),
                        ocPu = GetocPu(dataList.id_PlanZakaz),
                        otcl = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First() - GetocPu(dataList.id_PlanZakaz)
                    });
                    try
                    {
                        logger.Debug("DebitList: " + login);
                    }
                    catch
                    {
                    }
                    return Json(new { data });
                }
            }
        }

        public JsonResult DebitActiveList()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                if (login == "myi@katek.by" || login == "laa@katek.by" || login == "gvi@katek.by")
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Debit_WorkBit
                        .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                        .Include(d => d.PZ_PlanZakaz.PZ_Client)
                        .Include(d => d.PZ_PlanZakaz.PZ_Setup)
                        .Include(d => d.PZ_PlanZakaz.PZ_TEO)
                        .Where(d => d.id_TaskForPZ == 15 && d.close == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getDebTask('" + dataList.id_PlanZakaz + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        dataList.PZ_PlanZakaz.PlanZakaz,
                        Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                        Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().KolVoDneyNaPrijemku,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().PunktDogovoraOSrokahPriemki,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().UslovieOplatyText,
                        dateSh = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, shortSetting).Replace(@"""", ""),
                        status = GetStatusName(dataList),
                        sf = GetSF(dataList.id_PlanZakaz),
                        oc = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First(),
                        ocPu = GetocPu(dataList.id_PlanZakaz),
                        otcl = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First() - GetocPu(dataList.id_PlanZakaz)
                    });
                    return Json(new { data });
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Debit_WorkBit
                        .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                        .Include(d => d.PZ_PlanZakaz.PZ_Client)
                        .Include(d => d.PZ_PlanZakaz.PZ_Setup)
                        .Include(d => d.PZ_PlanZakaz.PZ_TEO)
                        .Where(d => d.id_TaskForPZ == 0)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getDebTask('" + dataList.id_PlanZakaz + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        dataList.PZ_PlanZakaz.PlanZakaz,
                        Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                        Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().KolVoDneyNaPrijemku,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().PunktDogovoraOSrokahPriemki,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().UslovieOplatyText,
                        dateSh = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, shortSetting).Replace(@"""", ""),
                        status = GetStatusName(dataList),
                        sf = GetSF(dataList.id_PlanZakaz),
                        oc = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First(),
                        ocPu = GetocPu(dataList.id_PlanZakaz),
                        otcl = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First() - GetocPu(dataList.id_PlanZakaz)
                    });
                    return Json(new { data });
                }
            }
        }

        public JsonResult DebitActiveShList()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                if (login == "myi@katek.by" || login == "laa@katek.by" || login == "gvi@katek.by")
                {
                    DateTime nowDate = DateTime.Now;
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Debit_WorkBit
                        .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                        .Include(d => d.PZ_PlanZakaz.PZ_Client)
                        .Include(d => d.PZ_PlanZakaz.PZ_Setup)
                        .Include(d => d.PZ_PlanZakaz.PZ_TEO)
                        .Where(d => d.id_TaskForPZ == 15 && d.close == false && d.PZ_PlanZakaz.dataOtgruzkiBP < nowDate)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getDebTask('" + dataList.id_PlanZakaz + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        dataList.PZ_PlanZakaz.PlanZakaz,
                        Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                        Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().KolVoDneyNaPrijemku,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().PunktDogovoraOSrokahPriemki,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().UslovieOplatyText,
                        dateSh = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, shortSetting).Replace(@"""", ""),
                        status = GetStatusName(dataList),
                        sf = GetSF(dataList.id_PlanZakaz),
                        oc = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First(),
                        ocPu = GetocPu(dataList.id_PlanZakaz),
                        otcl = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First() - GetocPu(dataList.id_PlanZakaz)
                    });
                    return Json(new { data });
                }
                else
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Debit_WorkBit
                        .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                        .Include(d => d.PZ_PlanZakaz.PZ_Client)
                        .Include(d => d.PZ_PlanZakaz.PZ_Setup)
                        .Include(d => d.PZ_PlanZakaz.PZ_TEO)
                        .Where(d => d.id_TaskForPZ == 0)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getDebTask('" + dataList.id_PlanZakaz + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        dataList.PZ_PlanZakaz.PlanZakaz,
                        Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                        Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().KolVoDneyNaPrijemku,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().PunktDogovoraOSrokahPriemki,
                        dataList.PZ_PlanZakaz.PZ_Setup.First().UslovieOplatyText,
                        dateSh = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, shortSetting).Replace(@"""", ""),
                        status = GetStatusName(dataList),
                        sf = GetSF(dataList.id_PlanZakaz),
                        oc = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First(),
                        ocPu = GetocPu(dataList.id_PlanZakaz),
                        otcl = dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.OtpuskChena).DefaultIfEmpty(0).First() + dataList.PZ_PlanZakaz.PZ_TEO.Select(kv => kv.NDS).DefaultIfEmpty(0).First() - GetocPu(dataList.id_PlanZakaz)
                    });
                    return Json(new { data });
                }
            }
        }

        double GetocPu(int id_PlanZakaz)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    return db.Debit_CostUpdate.Where(d => d.id_PZ_PlanZakaz == id_PlanZakaz).Sum(d => d.cost);
                }
            }
            catch
            {
                return 0.0;
            }
        }

        string GetStatusName(Debit_WorkBit debit_WorkBit)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                if (db.PZ_PlanZakaz.Find(debit_WorkBit.id_PlanZakaz).dataOtgruzkiBP > DateTime.Now)
                    return "Не отгружен";
                else if (db.Debit_WorkBit.First(d => d.id_PlanZakaz == debit_WorkBit.id_PlanZakaz && d.id_TaskForPZ == 15).close == true)
                    return "Оплачен";
                else if (db.Debit_WorkBit.Where(d => d.id_PlanZakaz == debit_WorkBit.id_PlanZakaz && d.id_TaskForPZ == 28).Select(kv => kv.close).DefaultIfEmpty(true).First() == false)
                    return "Не оприходован";
                //else if (db.Debit_WorkBit.First(d => d.id_PlanZakaz == debit_WorkBit.id_PlanZakaz && d.id_TaskForPZ == 15).datePlan < DateTime.Now)
                //{
                //    DateTime dateTime = db.Debit_WorkBit.First(d => d.id_PlanZakaz == debit_WorkBit.id_PlanZakaz && d.id_TaskForPZ == 15).datePlan;
                //    return "ДЗ (прос.)";
                //}
                else
                    return "ДЗ";
            }
        }

        string GetSF(int id_PZ_PlanZakaz)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                try
                {
                    int idTN = db.Debit_WorkBit.First(d => d.id_TaskForPZ == 11 && d.id_PlanZakaz == id_PZ_PlanZakaz).id;
                    return db.Debit_TN.First(d => d.id_DebitTask == idTN).numberTN;
                }
                catch
                {
                    return "";
                }
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
                    dataList.NDS,
                    dataList.SSMProduct,
                    dataList.importDuty,
                    dataList.importDutyPlan
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
                if (pZ_TEO.SSM > 0 && pZ_TEO.Rate > 0)
                {
                    CloseTask(2, pZ_TEO.Id_PlanZakaz, DateTime.Now);
                }
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
                if (pZ_Setup.UslovieOplatyText != null)
                {
                    CloseTask(4, pZ_Setup.id_PZ_PlanZakaz, DateTime.Now);
                }
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
                    if (ofile1[0] != null)
                    {
                        PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(pz);
                        fileUploadArray = ofile1;
                        CreateFolderAndFileForPreOrder(pZ_PlanZakaz.Folder);
                        string subject = "Получено письмо от экспедитора о поставке заказа: " + pZ_PlanZakaz.PlanZakaz.ToString();
                        new EmailAccountsReceivable(pZ_PlanZakaz.Folder, login, subject);
                        PostAlertShip postAlertShip = new PostAlertShip
                        {
                            id_Debit_WorkBit = CloseTask(10, pz, DateTime.Now),
                            datePost = datePost,
                            numPost = numPost,
                            datePrihod = datePrihod
                        };
                        db.PostAlertShip.Add(postAlertShip);
                        db.SaveChanges();
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

        private void CreateFolderAndFileForGraphic(string path)
        {
            string directory = path + @"\03_Переписка\График\";
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
                    Debit_TN debit_TN = new Debit_TN
                    {
                        id_DebitTask = CloseTask(11, pz, DateTime.Now),
                        numberTN = numberTN,
                        numberSF = numberSF,
                        dateTN = dateTN,
                        dateSF = dateSF,
                        Summa = 0
                    };
                    db.Debit_TN.Add(debit_TN);
                    db.SaveChanges();
                    Debit_CMR debit_CMR = new Debit_CMR
                    {
                        id_DebitTask = CloseTask(8, pz, DateTime.Now),
                        dateShip = dateCMR,
                        number = numCMR
                    };
                    ;
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
                    Debit_IstPost debit_IstPost = new Debit_IstPost
                    {
                        id_DebitTask = CloseTask(26, pz, DateTime.Now),
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
                string login = HttpContext.User.Identity.Name;
                string userId = db.AspNetUsers.First(d => d.Id == "f9bdc6ed-ce38-4a15-b983-3d85be1e30b1").Id;
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
                if(login == "myi@katek.by")
                {
                    var data = pzList.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getPM('" + dataList + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        pmOrderPZName = db.PZ_PlanZakaz.Find(dataList).PlanZakaz.ToString(),
                        powerST = db.PZ_PlanZakaz.Find(dataList).PowerST,
                        vnnn = db.PZ_PlanZakaz.Find(dataList).VN_NN,
                        gbb = db.PZ_PlanZakaz.Find(dataList).Modul,
                        orderRegist = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 1).Select(a => a.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
                        teoRegist = JsonConvert.SerializeObject(db.Debit_WorkBit.Where(d => d.id_PlanZakaz == dataList && d.id_TaskForPZ == 2).Select(a => a.dateClose).DefaultIfEmpty(new DateTime(1990, 1, 1)).First(), shortSetting).Replace(@"""", ""),
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
                    try
                    {
                        logger.Debug("TasksPM: " + login);
                    }
                    catch
                    {
                    }
                    return Json(new { data });
                }
                else
                {
                    var data = pzList.Select(dataList => new
                    {
                        editLink = "",
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
                    try
                    {
                        logger.Debug("TasksPM: " + login);
                    }
                    catch
                    {
                    }
                    return Json(new { data });
                }
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

        public JsonResult UpdatePM(int idPZ, int ProductType, string powerST, string vnnn, string gbb,
            DateTime? planKBM, DateTime? planKBE, DateTime? prototypeKBM, DateTime? prototypeKBE,
            DateTime? prototypeKBMComplited, DateTime? prototypeKBEComplited)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(idPZ);
                pZ_PlanZakaz.ProductType = ProductType;
                pZ_PlanZakaz.PowerST = GetCorrectParName(powerST);
                pZ_PlanZakaz.VN_NN = GetCorrectParName(vnnn);
                pZ_PlanZakaz.Modul = GetCorrectParName(gbb);
                db.Entry(pZ_PlanZakaz).State = EntityState.Modified;
                db.SaveChanges();
                if(planKBM != null)
                {
                    CloseTask(41, idPZ, planKBM.Value);
                }
                if (planKBE != null)
                {
                    CloseTask(42, idPZ, planKBE.Value);
                }
                if (prototypeKBM != null)
                {
                    CloseTask(43, idPZ, prototypeKBM.Value);
                }
                if (prototypeKBE != null)
                {
                    CloseTask(44, idPZ, prototypeKBE.Value);
                }
                if (prototypeKBMComplited != null)
                {
                    CloseTask(45, idPZ, prototypeKBMComplited.Value);
                }
                if (prototypeKBEComplited != null)
                {
                    CloseTask(46, idPZ, prototypeKBEComplited.Value);
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        string GetCorrectParName(string name)
        {
            if (name == "")
                return "??";
            return name;
        }

        int CloseTask(int idType, int idPZ, DateTime planDate)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                try
                {
                    Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.First(d => d.close == false && d.id_PlanZakaz == idPZ && d.id_TaskForPZ == idType);
                    debit_WorkBit.close = true;
                    debit_WorkBit.dateClose = DateTime.Now;
                    debit_WorkBit.datePlan = planDate;
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                    var predcessorsList = db.TaskForPZ.Where(d => d.Predecessors == idType);
                    if(predcessorsList.Count() > 0)
                    {
                        foreach (var predcessor in predcessorsList.ToList())
                        {
                            Debit_WorkBit predcessorWork = new Debit_WorkBit
                            {
                                id_TaskForPZ = predcessor.id,
                                dateCreate = DateTime.Now,
                                datePlanFirst = DateTime.Now.AddDays(predcessor.time),
                                close = false,
                                id_PlanZakaz = idPZ,
                                datePlan = DateTime.Now.AddDays(predcessor.time),
                                dateClose = null
                            };
                            db.Debit_WorkBit.Add(predcessorWork);
                            db.SaveChanges();
                        }
                    }
                    return debit_WorkBit.id;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public JsonResult GetLetterPM(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var sucursalList = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.close == false && d.id_TaskForPZ == id)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz);
                if (id == 2019)
                {
                    sucursalList = db.Debit_WorkBit
                        .AsNoTracking()
                        .Include(d => d.PZ_PlanZakaz)
                        .Where(d => d.id_TaskForPZ == 1)
                        .OrderByDescending(d => d.PZ_PlanZakaz.PlanZakaz);
                }
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.PZ_PlanZakaz.PlanZakaz.ToString(),
                    Value = m.PZ_PlanZakaz.Id.ToString(),
                });
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateLetterPM(int[] pZ_PlanZakazLettersPM, HttpPostedFileBase[] ofile1PM,
            DateTime datePost, string numPost, int idTaskPM, DateTime? datePrihod)
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                foreach (var pz in pZ_PlanZakazLettersPM)
                {
                    try
                    {
                        if (idTaskPM == 12)
                        {
                            PostAlertShip postAlertShip = new PostAlertShip
                            {
                                id_Debit_WorkBit = CloseTask(idTaskPM, pz, DateTime.Now),
                                datePost = datePost,
                                numPost = numPost,
                                datePrihod = datePrihod.Value
                            };
                            db.PostAlertShip.Add(postAlertShip);
                            db.SaveChanges();
                        }
                        else
                        {
                            CloseTask(idTaskPM, pz, DateTime.Now);
                        }
                    }
                    catch
                    {

                    }
                    if (idTaskPM == 2019)
                    {
                        MailGraphic mailGraphic = new MailGraphic
                        {
                            id_PZ_PlanZakaz = pz,
                            dateUpload = DateTime.Now,
                            dateMail = datePost,
                            numMail = numPost
                        };
                        db.MailGraphic.Add(mailGraphic);
                        db.SaveChanges();
                    }
                    if (ofile1PM[0] != null)
                    {
                        PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(pz);
                        fileUploadArray = ofile1PM;
                        if(idTaskPM != 2019)
                            CreateFolderAndFileForPreOrder(pZ_PlanZakaz.Folder);
                        else
                            CreateFolderAndFileForGraphic(pZ_PlanZakaz.Folder);
                    }
                }
                return RedirectToAction("Index");
            }
        }

        public JsonResult GetRKD()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var sucursalList = db.PZ_PlanZakaz
                    .AsNoTracking()
                    .Include(d => d.RKD_Order)
                    .Where(d => d.RKD_Order.Count == 0 && d.dataOtgruzkiBP > DateTime.Now)
                    .OrderByDescending(d => d.PlanZakaz);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.PlanZakaz.ToString(),
                    Value = m.Id.ToString(),
                });
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateRKD(int[] pZ_PlanZakazRKD)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                foreach (var pz in pZ_PlanZakazRKD)
                {
                    //RKD rKD = new RKD(pz);
                    //rKD.CreateRKDOrder();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDebTask(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_TEO
                    .Where(d => d.Id_PlanZakaz == id)
                    .Include(d => d.PZ_Currency)
                    .Include(d => d.PZ_PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    textPZ_Deb = "План-заказ №: " + dataList.PZ_PlanZakaz.PlanZakaz.ToString(),
                    id_PlanZakazDeb = dataList.PZ_PlanZakaz.PlanZakaz,
                    rateWhisVAT = (dataList.OtpuskChena + dataList.NDS).Value.ToString("N03", ci),
                    payment = GetocPu(dataList.Id_PlanZakaz).ToString("N03", ci),
                    receivables = (dataList.OtpuskChena + dataList.NDS - GetocPu(dataList.Id_PlanZakaz)).Value.ToString("N03", ci),
                    currencyDeb = dataList.PZ_Currency.Name
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddDebTask(int id_PlanZakazDeb, double paymentCost, DateTime paymentDate)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                Debit_CostUpdate debit_CostUpdate = new Debit_CostUpdate
                {
                    cost = paymentCost,
                    dateCreate = DateTime.Now,
                    dateGetMoney = paymentDate,
                    id_PZ_PlanZakaz = db.PZ_PlanZakaz.First(d => d.PlanZakaz == id_PlanZakazDeb).Id
                };
                db.Debit_CostUpdate.Add(debit_CostUpdate);
                db.SaveChanges();
                ClosePayment(debit_CostUpdate.id_PZ_PlanZakaz);
                logger.Debug("AddDebTask");
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDebOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_CostUpdate
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.PZ_PlanZakaz.PlanZakaz == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return updateDebTask('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return removeDebTask('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                    datePayment = JsonConvert.SerializeObject(dataList.dateGetMoney, shortSetting).Replace(@"""", ""),
                    sumPayment = dataList.cost
                });
                return Json(new { data });
            }
        }

        public JsonResult RemoveDebTask(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                int pz = db.Debit_CostUpdate.Find(id).id_PZ_PlanZakaz;
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                db.Debit_CostUpdate.Remove(db.Debit_CostUpdate.Find(id));
                db.SaveChanges();
                ClosePayment(pz);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRemoveTask()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var sucursalList = db.PZ_PlanZakaz
                    .AsNoTracking()
                    .Where(d => d.dataOtgruzkiBP > DateTime.Now)
                    .OrderByDescending(d => d.PlanZakaz);
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.PlanZakaz.ToString(),
                    Value = m.Id.ToString(),
                });
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateRemoveTask(int[] pZ_PlanZakazRemoveTask)
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                try
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    List<int> idList = new List<int>();
                    foreach (var pz in pZ_PlanZakazRemoveTask)
                    {
                        var listWork = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == pz && d.close == false && d.id_TaskForPZ != 15);
                        foreach (var task in listWork)
                        {
                            idList.Add(task.id);
                        }
                    }
                    foreach (var x in idList)
                    {
                        Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(x);
                        logger.Debug("UpdateRemoveTask: " + x + " | " + login);
                        db.Debit_WorkBit.Remove(debit_WorkBit);
                        db.SaveChanges();
                    }
                    logger.Debug("UpdateRemoveTask");
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    logger.Error("UpdateRemoveTask: " + ex.Message);
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetTtableLetterModal()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.close == false && d.id_TaskForPZ == 10)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    order = dataList.PZ_PlanZakaz.PlanZakaz,
                    tnNum = GetSF(dataList.id_PlanZakaz),
                    cmrNum = GetCMR(dataList.id_PlanZakaz)
                });
                return Json(new { data });
            }
        }

        string GetCMR(int id_PZ_PlanZakaz)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                try
                {
                    int idTN = db.Debit_WorkBit.First(d => d.id_TaskForPZ == 8 && d.id_PlanZakaz == id_PZ_PlanZakaz).id;
                    return db.Debit_CMR.First(d => d.id_DebitTask == idTN).number;
                }
                catch
                {
                    return "";
                }
            }
        }

        public JsonResult UpdateDebTask(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_CostUpdate
                    .Where(d => d.id == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    idPayment = dataList.id,
                    paymentCostUpdate = dataList.cost,
                    paymentDateUpdate = JsonConvert.SerializeObject(dataList.dateGetMoney, shortSettingRu).Replace(@"""", "")
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdatePayment(int idPayment, double paymentCostUpdate, DateTime paymentDateUpdate)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                Debit_CostUpdate debit_CostUpdate = db.Debit_CostUpdate.Find(idPayment);
                debit_CostUpdate.dateGetMoney = paymentDateUpdate;
                debit_CostUpdate.cost = paymentCostUpdate;
                db.Entry(debit_CostUpdate).State = EntityState.Modified;
                db.SaveChanges();
                ClosePayment(debit_CostUpdate.id_PZ_PlanZakaz);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        bool ClosePayment(int pz)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                try
                {
                    double paymant = db.Debit_CostUpdate
                        .Where(d => d.id_PZ_PlanZakaz == pz)
                        .Sum(d => d.cost);
                    double deb = db.PZ_TEO.First(d => d.Id_PlanZakaz == pz).Rate + db.PZ_TEO.First(d => d.Id_PlanZakaz == pz).NDS.Value;
                    if (paymant - deb == 0)
                    {
                        Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.First(d => d.id_PlanZakaz == pz && d.id_TaskForPZ == 15);
                        debit_WorkBit.close = true;
                        debit_WorkBit.dateClose = DateTime.Now;
                        db.Entry(debit_WorkBit).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.First(d => d.id_PlanZakaz == pz && d.id_TaskForPZ == 15);
                        debit_WorkBit.close = false;
                        debit_WorkBit.dateClose = null;
                        db.Entry(debit_WorkBit).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch
                {
                    logger.Error("ClosePayment: " + pz);
                }
            }
            return true;
        }

        public JsonResult FinExpDataList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string login = HttpContext.User.Identity.Name;
                if (login == "kns@katek.by" || login == "myi@katek.by")
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Debit_IstPost
                        .AsNoTracking()
                        .Include(d => d.Debit_WorkBit.PZ_PlanZakaz)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return editFin('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        order = dataList.Debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString(),
                        dataList.transportSum,
                        dataList.numberOrder,
                        dataList.ndsSum
                    });
                    try
                    {
                        logger.Debug("FinExpDataList: " + login);
                    }
                    catch
                    {
                    }
                    return Json(new { data });
                }
                else
                {
                    return null;
                }
            }
        }

        public JsonResult EditFin(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_IstPost
                    .Include(d => d.Debit_WorkBit.PZ_PlanZakaz)
                    .Where(d => d.id == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    finOrder = dataList.Debit_WorkBit.PZ_PlanZakaz.PlanZakaz.ToString(),
                    finid = dataList.id,
                    fintransportSum = dataList.transportSum,
                    finnumberOrder = dataList.numberOrder,
                    finndsSum = dataList.ndsSum,
                    fincurrency = dataList.currency
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateFin(double fintransportSum, string finnumberOrder, double finndsSum, int fincurrency, int finid)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                Debit_IstPost debit_IstPost = db.Debit_IstPost.Include(d => d.Debit_WorkBit.PZ_PlanZakaz).First(d => d.id == finid);
                debit_IstPost.transportSum = fintransportSum;
                debit_IstPost.numberOrder = finnumberOrder;
                debit_IstPost.ndsSum = finndsSum;
                debit_IstPost.currency = fincurrency;
                db.Entry(debit_IstPost).State = EntityState.Modified;
                db.SaveChanges();
                CloseTask(26, debit_IstPost.Debit_WorkBit.PZ_PlanZakaz.Id, DateTime.Now);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOrders1c()
        {
            string login = HttpContext.User.Identity.Name;
            using (ExportImportEntities db = new ExportImportEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var query = db.planZakaz
                   .AsNoTracking()
                   .Where(d => d.updateData == true)
                   .OrderByDescending(d => d.Zakaz)
                   .ToList();
                var data = query.Select(dataList => new
                {
                    edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrder1c('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    orderNumber = dataList.Zakaz,
                    dateSh = dataList.Otgruzka.ToString().Substring(0, 10)
                });
                return Json(new { data });
            }
        }

        public JsonResult GetOrder1c(int id)
        {
            using (ExportImportEntities db = new ExportImportEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var sucursalList = db.planZakaz
                    .AsNoTracking()
                    .Where(d => d.id == id)
                    .OrderByDescending(d => d.Zakaz)
                    .ToList();
                var data = sucursalList.Select(m => new
                {
                    idOrder1c = m.id,
                    orderName1c = m.Zakaz,
                    dateSh1c = JsonConvert.SerializeObject(m.Otgruzka, shortSettingRu).Replace(@"""", "")
                });
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateOrder1c(int idOrder1c, DateTime dateSh1c)
        {
            using (ExportImportEntities db = new ExportImportEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                planZakaz order = db.planZakaz.First(d => d.id == idOrder1c);
                order.Otgruzka = dateSh1c;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}