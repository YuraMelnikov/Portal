using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.ServiceReclamations.Models;
using System.Data.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Wiki.Areas.ServiceReclamations.Controllers
{
    public class MarksController : Controller
    {
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy HH:mm" };

        public ActionResult Index()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                ViewBag.PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
                ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeOTK == true).OrderBy(d => d.name), "id", "name");
                ViewBag.id_ServiceRemarksCause = new SelectList(db.ServiceRemarksCause.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
                string login = HttpContext.User.Identity.Name;
                int devisionUser = 0;
                try
                {
                    devisionUser = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
                }
                catch
                {

                }
                if (devisionUser == 28)
                    ViewBag.userGroupId = 1;
                else
                    ViewBag.userGroupId = 0;
                return View();
            }
        }

        public JsonResult Add(ServiceRemarks reclamation, int[] pZ_PlanZakaz, int[] id_Reclamation_Type, int[] id_ServiceRemarksCause)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string login = HttpContext.User.Identity.Name;
                reclamation.dateTimeCreate = DateTime.Now;
                reclamation.userCreate = db.AspNetUsers.First(d => d.Email == login).Id;
                if (reclamation.description == null)
                    reclamation.description = "";
                if (reclamation.folder == null)
                    reclamation.folder = "";
                db.ServiceRemarks.Add(reclamation);
                db.SaveChanges();
                foreach (var data in pZ_PlanZakaz)
                {
                    ServiceRemarksPlanZakazs remarkOrder = new ServiceRemarksPlanZakazs();
                    remarkOrder.id_ServiceRemarks = reclamation.id;
                    remarkOrder.id_PZ_PlanZakaz = data;
                    db.ServiceRemarksPlanZakazs.Add(remarkOrder);
                    db.SaveChanges();
                }
                foreach (var data in id_Reclamation_Type)
                {
                    ServiceRemarksTypes remarkOrder = new ServiceRemarksTypes();
                    remarkOrder.id_ServiceRemarks = reclamation.id;
                    remarkOrder.id_Reclamation_Type = data;
                    db.ServiceRemarksTypes.Add(remarkOrder);
                    db.SaveChanges();
                }
                foreach (var data in id_ServiceRemarksCause)
                {
                    ServiceRemarksCauses remarkOrder = new ServiceRemarksCauses();
                    remarkOrder.id_ServiceRemarks = reclamation.id;
                    remarkOrder.id_ServiceRemarksCause = data;
                    db.ServiceRemarksCauses.Add(remarkOrder);
                    db.SaveChanges();
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.ServiceRemarks
                    .Include(d => d.ServiceRemarksPlanZakazs)
                    .Include(d => d.ServiceRemarksTypes)
                    .Include(d => d.ServiceRemarksCauses)
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.id == id).ToList();
                var data = query.Select(dataList => new
                {
                    numberReclamation = dataList.id,
                    dataList.id,
                    pZ_PlanZakaz = GetPlanZakazArray(dataList.ServiceRemarksPlanZakazs.ToList()),
                    id_Reclamation_Type = GetTypesArray(dataList.ServiceRemarksTypes.ToList()),
                    id_ServiceRemarksCause = GetCausesArray(dataList.ServiceRemarksCauses.ToList()),
                    dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settings).Replace(@"""", ""),
                    userCreate = dataList.AspNetUsers.CiliricalName,
                    datePutToService = JsonConvert.SerializeObject(dataList.datePutToService, settings).Replace(@"""", ""),
                    dateClose = JsonConvert.SerializeObject(dataList.dateClose, settings).Replace(@"""", ""),
                    dataList.folder,
                    dataList.text,
                    dataList.description,
                    answerHistiryText = GetHistoryText(dataList.id)
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        string GetHistoryText(int id)
        {
            string answer = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var listAnswers = db.ServiceRemarksActions
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.id_ServiceRemarks == id)
                    .ToList();
                foreach (var data in listAnswers)
                {
                    answer += data.dateTimeCreate.ToShortDateString() + "|" + data.AspNetUsers.CiliricalName + " | " + data.text + "</br>";
                }
            }
            return answer;
        }

        string[] GetPlanZakazArray(List<ServiceRemarksPlanZakazs> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_PZ_PlanZakaz.ToString();
            }
            return pZ_PlanZakaz;
        }

        string[] GetTypesArray(List<ServiceRemarksTypes> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_Reclamation_Type.ToString();
            }
            return pZ_PlanZakaz;
        }

        string[] GetCausesArray(List<ServiceRemarksCauses> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_ServiceRemarksCause.ToString();
            }
            return pZ_PlanZakaz;
        }

        public JsonResult Update(ServiceRemarks reclamation, int[] pZ_PlanZakaz, int[] id_Reclamation_Type, int[] id_ServiceRemarksCause, string answerText)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                ServiceRemarks beforeUpdateRemark = db.ServiceRemarks.Find(reclamation.id);
                beforeUpdateRemark.dateClose = reclamation.dateClose;
                beforeUpdateRemark.datePutToService = reclamation.datePutToService;
                if (reclamation.description != null)
                    beforeUpdateRemark.description = reclamation.description = "";
                if (reclamation.folder != null)
                    beforeUpdateRemark.folder = reclamation.folder = "";
                beforeUpdateRemark.text = reclamation.text;
                db.Entry(beforeUpdateRemark).State = EntityState.Modified;
                db.SaveChanges();
                if(answerText != null || answerText != "")
                {
                    string login = HttpContext.User.Identity.Name;
                    ServiceRemarksActions serviceRemarksActions = new ServiceRemarksActions();
                    serviceRemarksActions.dateTimeCreate = DateTime.Now;
                    serviceRemarksActions.id_AspNetUsersCreate = db.AspNetUsers.First(d => d.Email == login).Id;
                    serviceRemarksActions.id_ServiceRemarks = beforeUpdateRemark.id;
                    serviceRemarksActions.text = answerText;
                    db.ServiceRemarksActions.Add(serviceRemarksActions);
                    db.SaveChanges();
                }
            }
            //pZ_PlanZakaz
            //id_Reclamation_Type
            //id_ServiceRemarksCause
            return Json(1, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ActiveList()
        {
            var query = new ReclamationsList().GetActive();
            var data = query.Select(dataList => new
            {
                dataList.id,
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                dateGet = JsonConvert.SerializeObject(dataList.datePutToService, shortSetting).Replace(@"""", ""),
                dateClose = JsonConvert.SerializeObject(dataList.dateClose, shortSetting).Replace(@"""", ""),
                dataList.folder
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult CloseList()
        {
            var query = new ReclamationsList().GetClose();
            var data = query.Select(dataList => new
            {
                dataList.id,
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                dateGet = JsonConvert.SerializeObject(dataList.datePutToService, shortSetting).Replace(@"""", ""),
                dateClose = JsonConvert.SerializeObject(dataList.dateClose, shortSetting).Replace(@"""", ""),
                dataList.folder
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult AllList()
        {
            var query = new ReclamationsList().GetAll();
            var data = query.Select(dataList => new
            {
                dataList.id,
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                dateGet = JsonConvert.SerializeObject(dataList.datePutToService, shortSetting).Replace(@"""", ""),
                dateClose = JsonConvert.SerializeObject(dataList.dateClose, shortSetting).Replace(@"""", ""),
                dataList.folder
            });
            return Json(new { data });
        }

        string GetOrdersName(int id)
        {
            string ordersName = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var dataList = db.ServiceRemarksPlanZakazs
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.id_ServiceRemarks == id)
                    .ToList();
                foreach (var data in dataList)
                {
                    ordersName += data.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
                }
            }
            return ordersName;
        }

        string GetClient(int id)
        {
            string clientName = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int idPz =  db.ServiceRemarksPlanZakazs
                    .First(d => d.id_ServiceRemarks == id)
                    .id_PZ_PlanZakaz;
                clientName = db.PZ_PlanZakaz.Include(d => d.PZ_Client).First(d => d.Id == idPz).PZ_Client.NameSort;
            }
            return clientName;
        }

        string GetTypes(int id)
        {
            string typesName = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var dataList = db.ServiceRemarksTypes
                    .Include(d => d.Reclamation_Type)
                    .Where(d => d.id_ServiceRemarks == id)
                    .ToList();
                foreach (var data in dataList)
                {
                    typesName += data.Reclamation_Type.name + "; ";
                }
            }
            return typesName;
        }

        string GetCauses(int id)
        {
            string causesName = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var dataList = db.ServiceRemarksCauses
                    .Include(d => d.ServiceRemarksCause)
                    .Where(d => d.id_ServiceRemarks == id)
                    .ToList();
                foreach (var data in dataList)
                {
                    causesName += data.ServiceRemarksCause.name + "; ";
                }
            }
            return causesName;
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
    }
}