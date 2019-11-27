using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.Reclamation.Models;
using System.Data.Entity;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class TechnicalAdviceController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            ViewBag.id_AspNetUserResponsible = new SelectList(db.AspNetUsers.Where(d => d.LockoutEnabled == true).OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
            ViewBag.id_AspNetUserTask = new SelectList(db.AspNetUsers.Where(d => d.LockoutEnabled == true).OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
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

        public JsonResult GetNoCloseTA()
        {
            var data = new TARemarksListView().GetNoCloseTA();
            return Json(new { data });
        }

        public JsonResult GetAllRemarks()
        {
            var data = new TARemarksListView().GetAllRemarks();
            return Json(new { data });
        }

        public JsonResult GetTA(int id)
        {
            JsonSerializerSettings dateFormatForView = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
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
                answerHistiryText = dataList.Reclamation.Answers,
                close = dataList.Reclamation_TechnicalAdvice.close,
                id_AspNetUserResponsible = dataList.Reclamation_TechnicalAdvice.id_AspNetUserResponsible,
                deadline = JsonConvert.SerializeObject(dataList.Reclamation_TechnicalAdvice.deadline, dateFormatForView).Replace(@"""", "")
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "TA")]
        public JsonResult Update(Reclamation_TechnicalAdvice ta)
        {
            Reclamation_TechnicalAdvice technicalAdvice = db.Reclamation_TechnicalAdvice.Find(ta.id);
            if (ta.text != null)
                technicalAdvice.text = ta.text;
            if (ta.description != null)
                technicalAdvice.description = ta.description;
            technicalAdvice.close = ta.close;
            if (ta.id_AspNetUserResponsible != null)
                technicalAdvice.id_AspNetUserResponsible = ta.id_AspNetUserResponsible;
            if (ta.deadline != null)
                technicalAdvice.deadline = ta.deadline;
            db.Entry(technicalAdvice).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "TA")]
        [HttpPost]
        public JsonResult CreateNewProtocol()
        {
            DateTime dateClose = DateTime.Now;
            Reclamation_TechnicalAdviceProtocol protocol = new Reclamation_TechnicalAdviceProtocol();
            protocol.date = dateClose;
            protocol.number = db.Reclamation_TechnicalAdviceProtocol.Select(p => p.number).DefaultIfEmpty(0).Max() + 1;
            db.Reclamation_TechnicalAdviceProtocol.Add(protocol);
            db.SaveChanges();
            foreach (var data in db.Reclamation_TechnicalAdvice.Where(d => d.dateTimeClose == null).ToList())
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

        public JsonResult GetActiveTA(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities)
            {
                var query = db.Reclamation_TechnicalAdviceTasks
                    .AsNoTracking()
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.id_Reclamation_TechnicalAdvice == id)
                    .ToList();
                var data = query.Select(datalist => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('" + datalist.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    idTask = datalist.id,
                    textTask = datalist.textTask,
                    textAnswer = datalist.textAnswer,
                    deadline = datalist.deadline.ToString().Substring(0, 10),
                    dateComplited = GetDateComplited(datalist.dateClose),
                    answers = datalist.textAnswer
                });;

                return Json(new { data });
            }
        }


        string GetDateComplited(DateTime? date)
        {
            try
            {
                return date.ToString().Substring(0, 10);
            }
            catch
            {
                return "";
            }
        }

        public JsonResult GetAdviceTask(int id)
        {
            var query = db.Reclamation_TechnicalAdviceTasks
                .AsNoTracking()
                .Where(d => d.id == id)
                .ToList();

            var data = query.Select(dataList => new
            {
                idAdviceTask = dataList.id,
                adviceUser = dataList.AspNetUsers.CiliricalName,
                adviceDeadline = dataList.deadline.ToString().Substring(0, 10),
                reclamationText = dataList.textTask,
                adviceAnswerTask = dataList.textAnswer,
                dateComplitedTask = GetDateComplited(dataList.dateClose)
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAdviceTask(int idAdviceTask, DateTime adviceDeadline, string textTask, 
            DateTime? dateComplitedTask, string textAnswer)
        {
            Reclamation_TechnicalAdviceTasks reclamation_TechnicalAdviceTasks = db.Reclamation_TechnicalAdviceTasks.Find(idAdviceTask);
            if (adviceDeadline != reclamation_TechnicalAdviceTasks.deadline)
                reclamation_TechnicalAdviceTasks.deadline = adviceDeadline;
            if (textTask != reclamation_TechnicalAdviceTasks.textTask)
                reclamation_TechnicalAdviceTasks.textTask = textTask;
            if (dateComplitedTask != reclamation_TechnicalAdviceTasks.dateClose)
                reclamation_TechnicalAdviceTasks.dateClose = dateComplitedTask;
            if (textAnswer != reclamation_TechnicalAdviceTasks.textAnswer)
                reclamation_TechnicalAdviceTasks.textAnswer = textAnswer;
            db.Entry(reclamation_TechnicalAdviceTasks).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}