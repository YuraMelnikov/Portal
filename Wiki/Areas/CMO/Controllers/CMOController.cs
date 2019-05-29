using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wiki.Areas.CMO.Controllers
{
    public class CMOController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            int devisionUser = db.AspNetUsers.First(d => d.Email == login).Devision.Value;

            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_CMO_TypeProduct = new SelectList(db.CMO_TypeProduct.Where(d => d.active == true), "id", "name");
            if (devisionUser == 7)
                ViewBag.userGroupId = 1;
            else if (login == "nrf@katek.by")
                ViewBag.userGroupId = 2;
            else if (devisionUser == 13)
                ViewBag.userGroupId = 3;
            else if (devisionUser == 18 || devisionUser == 15)
                ViewBag.userGroupId = 4;
            else
                ViewBag.userGroupId = 5;


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
    }
}