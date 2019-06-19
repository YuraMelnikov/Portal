using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.ServiceReclamations.Models;
using System.Data.Entity;
using Newtonsoft.Json;

namespace Wiki.Areas.ServiceReclamations.Controllers
{
    public class MarksController : Controller
    {
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };

        public ActionResult Index()
        {
            //ViewBag.PZ_PlanZakaz
            //ViewBag.id_Reclamation_Type
            //ViewBag.id_ServiceRemarksCause
            string login = HttpContext.User.Identity.Name;
            int devisionUser = 0;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    devisionUser = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
                }
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

        //Add
        //Get(int id)
        //Update(????)

        [HttpPost]
        public JsonResult ActiveList()
        {
            var query = new ReclamationsList().GetActive();
            var data = query.Select(dataList => new
            {
                dataList.id,
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateOpen, shortSetting).Replace(@"""", ""),
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
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateOpen, shortSetting).Replace(@"""", ""),
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
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateOpen, shortSetting).Replace(@"""", ""),
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