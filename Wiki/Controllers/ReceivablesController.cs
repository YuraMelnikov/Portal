using System.Linq;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class ReceivablesController : Controller
    {
        public ActionResult Index()
        {
            ReceivablesList list = new ReceivablesList();
            return View(list.GetActiveWork());
        }

        public ActionResult CloseReceivables()
        {
            ReceivablesList list = new ReceivablesList();
            return View(list.GetCloseWork());
        }

    }
}