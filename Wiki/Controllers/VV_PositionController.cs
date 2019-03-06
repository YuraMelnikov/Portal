using Microsoft.Office.Interop.Outlook;
using System;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using Wiki.Models;


namespace Wiki.Controllers
{
    public class VV_PositionController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public Application GetApplicationObject()
        {
            Application application = null;
            if (Process.GetProcessesByName("OUTLOOK").Count() > 0)
            {
                application = Marshal.GetActiveObject("Outlook.Application") as Application;
            }
            else
            {
                application = new Application();
                NameSpace nameSpace = application.GetNamespace("MAPI");
                nameSpace.Logon("", "", Missing.Value, Missing.Value);
                nameSpace = null;
            }
            return application;
        }

        public static void CreateMeetingRequest(string toEmail, string subject, string body, DateTime startDate, DateTime endDate, string location)
        {
            var objOL = new Microsoft.Office.Interop.Outlook.Application();
            //Application objOL = Marshal.GetActiveObject("Outlook.Application") as Application;
            AppointmentItem objAppt = (AppointmentItem)objOL.CreateItem(OlItemType.olAppointmentItem);
            objAppt.Start = startDate;
            objAppt.End = endDate;
            objAppt.Subject = subject;
            objAppt.Body = body;
            objAppt.Location = location;
            objAppt.MeetingStatus = OlMeetingStatus.olMeeting;
            objAppt.RequiredAttendees = toEmail;
            //objAppt.SaveAs("~\\Content\\ICS\\post.ics", _AppointmentItem);
            //objAppt.Display();
            //objAppt.Send();
            objAppt = null;
            objOL = null;                       
            MailMessage mail = new MailMessage();
        }

        public void button1_Click(int id, int idf)
        {
            VV_infPost vV_InfPost = db.VV_infPost.Where(d => d.Id == idf).First();
            string mailSend = "";
            string them = "зак. " + vV_InfPost.PlanZakaz.ToString() + " " + vV_InfPost.NaimenovanieIzdelia;
            string location = vV_InfPost.StantionGruzopoluchatel;
            var vvPisition = db.VV_Position.Where(d => d.id_VVPZ == id).ToList();
            string body =
                "зак. " + vV_InfPost.PlanZakaz.ToString() + " " + vV_InfPost.NaimenovanieIzdelia + "\n\n" +
                "Одно место." + "\n\n";
            foreach(var data in vvPisition)
            {
                if (data.Gabarit == "")
                    break;
                body += data.numMesto.ToString() + "." + data.Gabarit + ", н/у вес брутто " + data.massaBrutto.ToString() + " кг, нетто " + data.massaNetto.ToString() + " кг" + "\n";
            }
            body += "\n";
            body += 
                "Заводской номер: " + vV_InfPost.PlanZakaz.ToString() + "\n" +
                "Грузополучатель: " + vV_InfPost.Gruzopoluchatel + "\n" +
                "Адрес грузополучателя: " + vV_InfPost.PostAdresGruzopoluchatel + "\n" +
                "ИНН грузополучателя: " + vV_InfPost.INNGruzopoluchatel + "\n" +
                "ОКПО грузополучателя: " + vV_InfPost.OKPOGruzopoluchatelya + "\n" +
                "Код грузополучателя: " + vV_InfPost.KodGruzopoluchatela + "\n" +
                "Станция назначения: " + vV_InfPost.StantionGruzopoluchatel + "\n" +
                "Код станции: " + vV_InfPost.KodStanciiGruzopoluchatelya + "\n" +
                "Особые отметки грузоотправителя: " + vV_InfPost.OsobieOtmetkiGruzopoluchatelya + "\n" +
                "Прочее: " + vV_InfPost.DescriptionGruzopoluchatel + "\n\n" +
                "На отгрузку надо заказать машину и кран, 1 платформа.";
            DateTime stratDate = Convert.ToDateTime(vV_InfPost.dateShhip);
            stratDate = DateTime.Parse(stratDate.ToShortDateString() + " 08:00 AM");
            DateTime finishDate = Convert.ToDateTime(vV_InfPost.dateShhip);
            finishDate = DateTime.Parse(finishDate.ToShortDateString() + " 09:00 AM");
            CreateMeetingRequest(mailSend, them, body, stratDate, finishDate, location);
        }

        public ActionResult SendMail(int id, int idf)
        {
            VV_infPost vV_InfPost = db.VV_infPost.Where(d => d.Id == idf).First();
            string them = "зак. " + vV_InfPost.PlanZakaz.ToString() + " " + vV_InfPost.NaimenovanieIzdelia;
            string location = vV_InfPost.StantionGruzopoluchatel;
            var vvPisition = db.VV_Position.Where(d => d.id_VVPZ == id).ToList();
            string body =
                "зак. " + vV_InfPost.PlanZakaz.ToString() + " " + vV_InfPost.NaimenovanieIzdelia + "\n\n" +
                "Одно место." + "\n\n";
            foreach (var data in vvPisition)
            {
                if (data.Gabarit == "")
                    break;
                body += data.numMesto.ToString() + "." + data.Gabarit + ", н/у вес брутто " + data.massaBrutto.ToString() + " кг, нетто " + data.massaNetto.ToString() + " кг" + "\n";
            }
            string dataDesc = "";
            if (vV_InfPost.DescriptionGruzopoluchatel != null)
                dataDesc = vV_InfPost.DescriptionGruzopoluchatel.ToString();
            body += "\n";

            string[] recipient = { "myi@katek.by" };
            EmailModel emailModel = new EmailModel();
            emailModel.SendEmail(recipient, them, body);
            Debit_WorkBitController debit_WorkBitController = new Debit_WorkBitController();
            debit_WorkBitController.CloseComplitedTasks(vV_InfPost.Id, 21);

            return Redirect("/Debit_WorkBit/Index");
        }

        public ActionResult List(int? id)
        {
            var list = db.VV_Position.Where(d => d.id_VVPZ == id).ToList();
            VVPZ vVPZ = db.VVPZ.Find(list[0].id_VVPZ);
            id = vVPZ.id_PZ_PlanZakaz;
            PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(id);
            ViewBag.idPlanZakaz = pZ_PlanZakaz.Id;
            ViewBag.NumOrder = pZ_PlanZakaz.PlanZakaz.ToString();
            ViewBag.name = vVPZ.name.ToString();
            ViewBag.description = vVPZ.description.ToString();
            ViewBag.massaBrutto = vVPZ.massaBrutto.ToString();
            ViewBag.massaNetto = vVPZ.massaNetto.ToString();
            ViewBag.gruzopoluchatel = vVPZ.gruzopoluchatel.ToString();
            ViewBag.adresGruzo = vVPZ.adresGruzo.ToString();
            ViewBag.iNNGruz = vVPZ.iNNGruz.ToString();
            ViewBag.oKPOGruz = vVPZ.oKPOGruz.ToString();
            ViewBag.kodGruz = vVPZ.kodGruz.ToString();
            ViewBag.sTNazn = vVPZ.sTNazn.ToString();
            ViewBag.kodSt = vVPZ.kodSt.ToString();
            ViewBag.osobieOtm = vVPZ.osobieOtm.ToString();
            ViewBag.prochee = vVPZ.prochee.ToString();
            ViewBag.dateSh = pZ_PlanZakaz.DateShipping.ToString().Substring(0, 10);
            ViewBag.IdEmail = vVPZ.id;
            return View(list);
        }

        public ActionResult Index()
        {
            var vV_Position = db.VV_Position.Include(v => v.VVPZ);
            return View(vV_Position.ToList());
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VV_Position vV_Position = db.VV_Position.Find(id);
            if (vV_Position == null)
            {
                return HttpNotFound();
            }
            return View(vV_Position);
        }
        
        public ActionResult Create()
        {
            ViewBag.id_VVPZ = new SelectList(db.VVPZ, "id", "name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_VVPZ,numMesto,Gabarit,Description,massaBrutto,massaNetto")] VV_Position vV_Position)
        {
            if (ModelState.IsValid)
            {
                db.VV_Position.Add(vV_Position);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_VVPZ = new SelectList(db.VVPZ, "id", "name", vV_Position.id_VVPZ);
            return View(vV_Position);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VV_Position vV_Position = db.VV_Position.Find(id);
            if (vV_Position == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_VVPZ = new SelectList(db.VVPZ, "id", "name", vV_Position.id_VVPZ);
            return View(vV_Position);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_VVPZ,numMesto,Gabarit,Description,massaBrutto,massaNetto")] VV_Position vV_Position)
        {
            if (ModelState.IsValid)
            {
                if (vV_Position.Description == null)
                    vV_Position.Description = "";
                if (vV_Position.Gabarit == null)
                    vV_Position.Gabarit = "";
                db.Entry(vV_Position).State = EntityState.Modified;
                db.SaveChanges();
                var data = db.VV_Position.Where(d => d.id_VVPZ == vV_Position.id_VVPZ).ToList();
                int brutto = 0;
                int netto = 0;
                foreach(var bin in data)
                {
                    brutto += bin.massaBrutto;
                    netto += bin.massaNetto;
                }
                VVPZ vPZ = db.VVPZ.Find(vV_Position.id_VVPZ);
                vPZ.massaNetto = netto;
                vPZ.massaBrutto = brutto;
                db.Entry(vPZ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List", new { id = vV_Position.id_VVPZ });
            }
            ViewBag.id_VVPZ = new SelectList(db.VVPZ, "id", "name", vV_Position.id_VVPZ);
            return View(vV_Position);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VV_Position vV_Position = db.VV_Position.Find(id);
            if (vV_Position == null)
            {
                return HttpNotFound();
            }
            return View(vV_Position);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VV_Position vV_Position = db.VV_Position.Find(id);
            db.VV_Position.Remove(vV_Position);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}