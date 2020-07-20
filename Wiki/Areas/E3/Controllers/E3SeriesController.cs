using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.E3.Controllers
{
    public class E3SeriesController : Controller
    {
        public ActionResult Index()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            string login = HttpContext.User.Identity.Name;
            int devisionUser = GetDevision(login);
            if (login == "myi@katek.by" || login == "dkv@katek.by")
                ViewBag.userGroupId = 1;
            else if (devisionUser == 16)
                ViewBag.userGroupId = 2;
            else
                ViewBag.userGroupId = 0;
            return View();
        }

        public JsonResult GetTableModelData()
        {
            //try
            //{
                using (E3_Components_SymbolsEntities db = new E3_Components_SymbolsEntities())
                {
                    //DateTime controlDate = DateTime.Now.AddDays(-90);
                    //db.Configuration.ProxyCreationEnabled = false;
                    //db.Configuration.LazyLoadingEnabled = false;
                    //var query = db.CMOSOrder
                    //    .AsNoTracking()
                    //    .Include(a => a.CMO_Company)
                    //    .Include(a => a.CMOSPositionOrder)
                    //    .Where(a => a.dateTimeCreate > controlDate && a.remove == false)
                    //    .ToList();
                    //var data = query.Select(dataList => new
                    //{
                    //    dataList.id,
                    //    positions = GetPositionsNamePreOrder(dataList.id),
                    //    customer = dataList.CMO_Company.name,
                    //    state = GetStateOrder(dataList.id),
                    //    startDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                    //    finishDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                    //    dataList.cost,
                    //    dataList.factCost,
                    //    folder = @"<a href =" + dataList.folder + "> Папка </a>",
                    //    tnNumber = dataList.numberTN,
                    //    summaryWeight = Math.Round(dataList.CMOSPositionOrder.Sum(a => a.summaryWeight), 2),
                    //    posList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                    //    dataList.rate
                    //});
                    //logger.Debug("CMOSSController / GetTableOrders");
                    //return Json(new { data });
                    return Json(0);
                }
            //}
            //catch (Exception ex)
            //{
            //    logger.Error("CMOSSController / GetTableOrders: " + " | " + ex);
            //    return Json(0, JsonRequestBehavior.AllowGet);
            //}
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

        private int GetDevision(string login)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    return db.AspNetUsers.First(d => d.Email == login).Devision.Value;
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}