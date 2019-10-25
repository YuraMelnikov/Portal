using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.CMKO.Controllers
{
    public class CMKController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        public ActionResult Index()
        {
            ViewBag.period = new SelectList(db.CMKO_PeriodResult
                .Where(d => d.close == false)
                .OrderByDescending(x => x.period), "period", "period");
            ViewBag.autor = new SelectList(db.AspNetUsers
                .Where(d => d.LockoutEnabled == true)
                .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
            return View();
        }

        //GetOptimizationList
        //AddOptimization
        //GetOptimization
        //RemoveOptimization
        //UpdateOptimization
    }
}