using System.Collections.Generic;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Areas.Service.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        public string RenderUserMenu()
        {
            string login = "Войти";
            try
            {
                if(HttpContext.User.Identity.Name != "")
                    login = HttpContext.User.Identity.Name;
            }
            catch
            {
                login = "Войти";
            }
            return login;
        }

        public ActionResult IndexTest()
        {
            List<string> list = new List<string>();
            list.Add("myi@katek.by");
            string text = @"<a href='\\192.168.1.30\\m$\\_ЗАКАЗЫ\\1401 - 1500\\1455_ГПН - Мессояха'> Скачать файл</a>";


            SendMail(text, list);
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public void SendMail(string text, List<string> list)
        {
            EmailModel emailModel = new EmailModel();
            try
            {
                emailModel.SendEmailListHtml(list, text, text);

            }
            catch
            {
                emailModel.SendEmailOnePerson("myi@katek.by", "ошибка отправки", "ошибка отправки IndexTest");
            }
        }
    }
}