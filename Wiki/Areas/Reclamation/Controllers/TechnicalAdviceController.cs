using Wiki.Areas.Reclamation.Models;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class TechnicalAdviceController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetProtocols()
        {
            var data = new ProtocolTAListView().ProtocolTAViews;
            return Json(new { data });
        }

        public JsonResult GetActiveTA()
        {
            var data = new TARemarksListView().GetActiveTA();
            return Json(new { data });
        }

        public JsonResult GetAllRemarks()
        {
            var data = new TARemarksListView().GetAllRemarks();
            return Json(new { data });
        }

        public JsonResult GetTA(int id)
        {
            List<TAEdit> query = new List<TAEdit>();
            query.Add(new TAEdit(id));
            var data = query.Select(dataList => new
            {
                dataList.Reclamation_TechnicalAdvice.id,
                userUploadReclamation = dataList.Reclamation_TechnicalAdvice.AspNetUsers.CiliricalName,
                dataList.Reclamation_TechnicalAdvice.text,
                dataList.Reclamation_TechnicalAdvice.description,
                orders = dataList.Reclamation.PlanZakaz,
                userCreateReclamation = dataList.Reclamation.UserCreate,
                devisionReclamation = dataList.Reclamation.Devision,
                reclamationText = dataList.Reclamation.Text,
                answerHistiryText = dataList.Reclamation.Answers
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Reclamation_TechnicalAdvice ta)
        {
            Reclamation_TechnicalAdvice technicalAdvice = db.Reclamation_TechnicalAdvice.Find(ta.id);
            if(ta.text != null)
                technicalAdvice.text = ta.text;
            if(ta.description != null)
                technicalAdvice.description = ta.description;
            db.Entry(technicalAdvice).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult DownloadProtocol(int id)
        //{
        //    return Json(new { data });
        //}

        //public JsonResult GetProtocol(int id)
        //{
        //    return Json(new { data });
        //}

        //public JsonResult CreateNewProtocol(int id)
        //{
        //    return Json(new { data });
        //}
    }
}