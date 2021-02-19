using Newtonsoft.Json;
using NLog;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace Wiki.Areas.CrossTable.Controllers
{
    public class CTOController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };

        public ActionResult Index()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            ViewBag.OrdersList = new SelectList(db.PZ_PlanZakaz
                .Where(d => d.dataOtgruzkiBP.Year == DateTime.Now.Year)
                .OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_PlanZakaz
                    .AsNoTracking()
                    .Include(d => d.PZ_ProductType)
                    .Include(d => d.AspNetUsers)
                    .Include(d => d.PZ_Client)
                    .Include(d => d.CrossOrder)
                    .Where(d => d.CrossOrder.Count() > 0)
                    .ToList();

                var data = query.Select(dataList => new
                {
                    parent = dataList.PlanZakaz,
                    childs = GetChilds(dataList.Id),
                    customer = dataList.PZ_Client.NameSort,
                    name = dataList.Name,
                    dataList.nameTU,
                    manager = dataList.AspNetUsers.CiliricalName,
                    contract = dataList.timeContract,
                    contractDate = JsonConvert.SerializeObject(dataList.timeContractDate, shortSetting).Replace(@"""", ""),
                    spetification = dataList.timeArr,
                    spetificationDate = JsonConvert.SerializeObject(dataList.timeArrDate, shortSetting).Replace(@"""", ""),
                    shDate = JsonConvert.SerializeObject(dataList.dataOtgruzkiBP, shortSetting).Replace(@"""", ""),
                    removeLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return Remove('" + dataList.Id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                });
                return Json(new { data });
            }
        }

        private string GetChilds(int id)
        {
            string res = "";
            using(PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.CrossOrder
                    .AsNoTracking()
                    .Include(a => a.PZ_PlanZakaz1)
                    .Where(a => a.id_OrderParent == id)
                    .ToList();
                foreach (var data in query)
                {
                    res += data.PZ_PlanZakaz1.PlanZakaz.ToString() + " - " + data.PZ_PlanZakaz1.Name + "</br>";
                }
                return res;
            }
        }

        [HttpPost]
        public JsonResult Add(int parent, int[] childs)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    foreach (var ch in childs)
                    {
                        CrossOrder cross = new CrossOrder();
                        cross.id_OrderParent = parent;
                        cross.id_OrderChild = ch;
                        cross.note = "";
                        db.CrossOrder.Add(cross);
                        db.SaveChanges();
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CTO / Add: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Remove(int id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var ord = db.CrossOrder.Find(id);
                    db.CrossOrder.Remove(ord);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CTO / Remove: " + " | " + ex + " | " + id.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}