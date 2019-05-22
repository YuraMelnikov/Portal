using Wiki.Areas.Reclamation.Models;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System;

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

        [HttpPost]
        public JsonResult CreateNewProtocol()
        {
            DateTime dateClose = DateTime.Now;
            Reclamation_TechnicalAdviceProtocol protocol = new Reclamation_TechnicalAdviceProtocol();
            protocol.date = dateClose;
            protocol.number = db.Reclamation_TechnicalAdviceProtocol.Select(p => p.number).DefaultIfEmpty(0).Max() + 1;
            db.Reclamation_TechnicalAdviceProtocol.Add(protocol);
            db.SaveChanges();
            foreach(var data in db.Reclamation_TechnicalAdvice.Where(d => d.dateTimeClose == null).ToList())
            {
                Reclamation_TechnicalAdvice ta = db.Reclamation_TechnicalAdvice.Find(data.id);
                ta.dateTimeClose = dateClose;
                db.Entry(ta).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Reclamation_TechnicalAdviceProtocolPosition position = new Reclamation_TechnicalAdviceProtocolPosition();
                position.id_Reclamation_TechnicalAdvice = ta.id;
                position.id_Reclamation_TechnicalAdviceProtocol = protocol.id;
                db.Reclamation_TechnicalAdviceProtocolPosition.Add(position);
                db.SaveChanges();
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProtocol(int id)
        {
            var data = new TARemarksListView().GetRemarksProtocol(id);
            return Json(new { data });
        }
    }
}