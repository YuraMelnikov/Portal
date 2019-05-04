using System.Web.Mvc;
using Wiki.Areas.Reclamation.Models;
using System.Linq;
using Newtonsoft.Json;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class RemarksController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

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

        public ActionResult IndexOrders()
        {
            return View();
        }

        public JsonResult ActiveReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(GetIdDevision(login), false);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }

        public JsonResult PlanZakazDevisionNotSh()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login), false);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult PlanZakazDevisionSh()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login), true);
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult PlanZakazDevisionAll()
        {
            string login = HttpContext.User.Identity.Name;
            PlanZakazListViewers planZakazListViewers = new PlanZakazListViewers();
            planZakazListViewers.GetPlanZakazs(GetIdDevision(login));
            return Json(new { data = planZakazListViewers.PlanZakazViwers });
        }

        public JsonResult CloseReclamation()
        {
            string login = HttpContext.User.Identity.Name;
            ReclamationListViewer reclamationListViewer = new ReclamationListViewer();
            reclamationListViewer.GetReclamation(GetIdDevision(login), true);
            return Json(new { data = reclamationListViewer.ReclamationsListView });
        }
        
        public JsonResult Add(Wiki.Reclamation reclamation)
        {
            string login = HttpContext.User.Identity.Name;
            CorrectReclamation correctReclamation = new CorrectReclamation(reclamation, login);
            reclamation = correctReclamation.Reclamation;
            db.Reclamation.Add(reclamation);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Wiki.Reclamation reclamation)
        {
            string login = HttpContext.User.Identity.Name;
            CorrectReclamation correctPlanZakaz = new CorrectReclamation(reclamation);
            reclamation = correctPlanZakaz.Reclamation;
            db.Entry(reclamation).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReclamation(int id)
        {
            var query = db.Reclamation.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                dataList.id_Reclamation_Type,
                dataList.id_DevisionReclamation,
                dataList.id_Reclamation_CountErrorFirst,
                dataList.id_Reclamation_CountErrorFinal,
                dataList.id_AspNetUsersCreate,
                dataList.id_DevisionCreate,
                dataList.text,
                dataList.description,
                dataList.timeToSearch,
                dataList.timeToEliminate,
                dataList.close,
                dataList.gip,
                dataList.closeDevision,
                dataList.PCAM,
                dataList.editManufacturing,
                dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settings).Replace(@"""", "")
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }
        
        int GetIdDevision(string loginUser)
        {
            int id_Devision = 0;
            try
            {
                id_Devision = db.AspNetUsers.First(d => d.Email == loginUser).Devision.Value;
            }
            catch
            {

            }
            return id_Devision;
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