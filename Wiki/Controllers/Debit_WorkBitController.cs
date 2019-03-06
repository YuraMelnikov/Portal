using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.IO;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class Debit_WorkBitController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ActionResult Index()
        {
            try
            {
                string login = HttpContext.User.Identity.Name;
                login = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                //var debit_WorkBit = db.Debit_WorkBit.Include(d => d.TaskForPZ).Include(d => d.PZ_PlanZakaz).Where(d => d.close == false).OrderBy(d => d.datePlan);
                var debit_WorkBit = db.Debit_WorkBit.Include(d => d.TaskForPZ).Include(d => d.PZ_PlanZakaz).Where(d => d.close == false).Where(d => d.TaskForPZ.id_User == login).OrderBy(d => d.datePlan);
                return View(debit_WorkBit.ToList());
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult StatusOrders()
        {
                var debit_WorkBit = db.Debit_WorkBit
                                        .Include(d => d.TaskForPZ)
                                        .Include(d => d.PZ_PlanZakaz)
                                        .Where(d => d.close == false)
                                        .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz);
                return View(debit_WorkBit.ToList());
        }

        public ActionResult RemainingList()
        {
            var debit_WorkBit = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 15).OrderBy(d => d.datePlan);
            return View(debit_WorkBit.ToList());
        }

        public ActionResult IndexBrows()
        {
            var debit_WorkBit = db.Debit_WorkBit
                .Include(d => d.TaskForPZ)
                .Include(d => d.PZ_PlanZakaz)
                .Where(d => d.close == false)
                .OrderBy(d => d.datePlan);
            return View(debit_WorkBit.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(id);
            if (debit_WorkBit == null)
            {
                return HttpNotFound();
            }
            return View(debit_WorkBit);
        }
        
        public ActionResult Create()
        {
            ViewBag.id_TaskForPZ = new SelectList(db.TaskForPZ, "id", "taskName");
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_TaskForPZ,dateCreate,datePlan,dateClose,close,id_PlanZakaz,datePlanFirst")] Debit_WorkBit debit_WorkBit)
        {
            if (ModelState.IsValid)
            {
                db.Debit_WorkBit.Add(debit_WorkBit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_TaskForPZ = new SelectList(db.TaskForPZ, "id", "taskName", debit_WorkBit.id_TaskForPZ);
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_WorkBit.id_PlanZakaz);
            return View(debit_WorkBit);
        }
        
        public void UpdateDebitWorkPlan (int pz)
        {
            int count = 0;

            foreach (var date in db.Debit_WorkBit.Where(d => d.close == false))
            {
                if (date.id_PlanZakaz == pz & date.close == false & date.id_TaskForPZ == 6 || date.id_TaskForPZ == 7)
                {
                    count = date.id;
                    break;
                }
            }
            if (count > 0)
            {
                var date = db.Debit_WorkBit.Find(count);
                date.close = true;
                date.dateClose = DateTime.Now;
                db.Entry(date).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public string IfTaskForPZ2(int pz)
        {
            string connectingString = "/Edit/";
            PZ_PlanZakaz planZakaz = db.PZ_PlanZakaz.Find(pz);
            foreach (var data in db.PZ_TEO)
            {
                if (data.Id_PlanZakaz == planZakaz.Id)
                {
                    connectingString += data.id.ToString();
                    break;
                }
            }
            return connectingString;
        }

        public string IfTaskForPZ22(int pz)
        {
            string connectingString = "/Edit/";
            PZ_PlanZakaz planZakaz = db.PZ_PlanZakaz.Find(pz);
            foreach (var data in db.PZ_Packaging)
            {
                if (data.id_PZ_PlanZakaz == planZakaz.Id)
                {
                    connectingString += data.id.ToString();
                    break;
                }
            }
            return connectingString;
        }

        public string IfTaskForPZ4(int pz)
        {
            string connectingString = "/Edit/";
            PZ_PlanZakaz planZakaz = db.PZ_PlanZakaz.Find(pz);
            foreach (var data in db.PZ_Setup)
            {
                if (data.id_PZ_PlanZakaz == planZakaz.Id)
                {
                    connectingString += data.id.ToString();
                    break;
                }
            }
            return connectingString;
        }

        public void SendMail(Debit_WorkBit debit_WorkBit, string text, List<string> list)
        {
            EmailModel emailModel = new EmailModel();
            try
            {
                PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(debit_WorkBit.id_PlanZakaz);
                string textMail = "";
                textMail += pZ_PlanZakaz.PlanZakaz.ToString() + text;

                emailModel.SendEmailList(list, textMail, textMail);
            }
            catch
            {
                emailModel.SendEmailOnePerson("myi@katek.by", "ошибка отправки", debit_WorkBit.id.ToString());
            }
        }

        public ActionResult Edit(int? id, int? idTask)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(id);
            //Заполнить данные ТЭО
            if (idTask == 2)
            {
                string connectingString;
                connectingString = IfTaskForPZ2(debit_WorkBit.id_PlanZakaz);
                return RedirectToRoute(new { controller = "PZ_TEO", action = connectingString });
            }
            //Заполнить договорные условия
            if (idTask == 4)
            {
                string connectingString;
                connectingString = IfTaskForPZ4(debit_WorkBit.id_PlanZakaz);
                return RedirectToRoute(new { controller = "PZ_Setup", action = connectingString });
            }
            //Уведомить Заказчика о готовности к отгрузке (при наличии грузополучателя)
            if (idTask == 5)
            {
                return RedirectToAction("UploadAlertShip", "UploadDocument");
            }
            //Отправить первоначальный график Заказчику + Отправить график Заказчику после согласования РКД
            if (idTask == 6 || idTask == 7)
            {
                return RedirectToAction("UploadGraphic", "UploadDocument");
            }
            //Внести данные о ЖДН/CMR
            if (idTask == 8)
            {
                return RedirectToAction("UploadCMR", "UploadDocument");
            }
            //Получить письмо от экспедитора
            if (idTask == 10)
            {
                return RedirectToAction("UploadAlertShipClose", "UploadDocument");
            }
            //Внести данные по ТН/СФ
            if (idTask == 11)
            {
                return RedirectToAction("Create", "Debit_TN", new { idf = debit_WorkBit.id_PlanZakaz, idTask = debit_WorkBit.id });
            }
            //Отправить письмо-уведомление Заказчику о прибытии заказа на станцию назначения
            if (idTask == 12)
            {
                return RedirectToAction("PostUploadAlertShipClose", "UploadDocument");
            }
            //Подтвердить приход средств по заказу
            if (idTask == 15)
            {
                return RedirectToAction("NewPlus", "Debit_CostUpdate", new { id_PlanZakaz = debit_WorkBit.id_PlanZakaz, idTask = debit_WorkBit.id });
            }
            //Получить согласование отгрузки
            if (idTask == 18)
            {
                return RedirectToAction("UploadMatchingShip", "UploadDocument");
            }
            //Подготовить рассылку письма об отгрузке заказов
            if (idTask == 21)
            {
                VVPZ vVPZ = new VVPZ();
                PZ_PlanZakaz pZ = db.PZ_PlanZakaz.Find(debit_WorkBit.id_PlanZakaz);
                vVPZ.id_PZ_PlanZakaz = pZ.Id;
                vVPZ.name = pZ.PlanZakaz.ToString() + " - " + pZ.Name;
                vVPZ.description = "";
                vVPZ.gruzopoluchatel = pZ.Gruzopoluchatel;
                vVPZ.iNNGruz = pZ.INNGruzopoluchatel;
                vVPZ.kodGruz = pZ.KodGruzopoluchatela;
                vVPZ.kodSt = pZ.KodStanciiGruzopoluchatelya;
                vVPZ.massaBrutto = 0;
                vVPZ.massaNetto = 0;
                vVPZ.oKPOGruz = pZ.OKPOGruzopoluchatelya;
                vVPZ.osobieOtm = pZ.OsobieOtmetkiGruzopoluchatelya;
                vVPZ.prochee = pZ.DescriptionGruzopoluchatel;
                vVPZ.sTNazn = pZ.StantionGruzopoluchatel;
                vVPZ.adresGruzo = pZ.PostAdresGruzopoluchatel;
                vVPZ.dateSh = Convert.ToDateTime(DateTime.Now);
                if (vVPZ.prochee == null)
                    vVPZ.prochee = "";

                if (vVPZ.adresGruzo == null)
                    vVPZ.adresGruzo = "";
                if (vVPZ.description == null)
                    vVPZ.description = "";
                if (vVPZ.gruzopoluchatel == null)
                    vVPZ.gruzopoluchatel = "";
                if (vVPZ.iNNGruz == null)
                    vVPZ.iNNGruz = "";

                if (vVPZ.kodGruz == null)
                    vVPZ.kodGruz = "";
                if (vVPZ.kodSt == null)
                    vVPZ.kodSt = "";
                if (vVPZ.name == null)
                    vVPZ.name = "";
                if (vVPZ.oKPOGruz == null)
                    vVPZ.oKPOGruz = "";

                if (vVPZ.osobieOtm == null)
                    vVPZ.osobieOtm = "";
                if (vVPZ.prochee == null)
                    vVPZ.prochee = "";
                if (vVPZ.sTNazn == null)
                    vVPZ.sTNazn = "";
                db.VVPZ.Add(vVPZ);
                db.SaveChanges();
                for(int i = 0; i < 15; i++)
                {
                    VV_Position vV_Position = new VV_Position();
                    vV_Position.Description = "";
                    vV_Position.Gabarit = "";
                    vV_Position.id_VVPZ = vVPZ.id;
                    vV_Position.massaBrutto = 0;
                    vV_Position.massaNetto = 0;
                    vV_Position.numMesto = i + 1;
                    db.VV_Position.Add(vV_Position);
                    db.SaveChanges();
                }

                return RedirectToAction("List", "VV_Position", new { id = vVPZ.id });
            }
            //Заполнить данные по упаковке
            if (idTask == 22)
            {
                string connectingString;
                connectingString = IfTaskForPZ22(debit_WorkBit.id_PlanZakaz);
                return RedirectToRoute(new { controller = "PZ_Packaging", action = connectingString });
            }
            //Заполнить финансовые данные по экспедитору
            if (idTask == 26)
            {
                return RedirectToAction("Create", "Debit_IstPost", new { idf = debit_WorkBit.id_PlanZakaz, idTask = debit_WorkBit.id });
            }
            //Заполнить отгрузочные данные (по платформе)
            if (idTask == 29)
            {
                id = db.Debit_Platform.Where(d => d.id_PlanZakaz == debit_WorkBit.id_PlanZakaz).First().id;
                return RedirectToAction("Edit", "Debit_Platform", new { id });
            }
            //Отправить Заказчику уведомление о размещении заказа в производство
            if (idTask == 30)
            {
                return RedirectToAction("AlertOpenOrderInManuf", "ReName");
            }
            if (debit_WorkBit == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_TaskForPZ = new SelectList(db.TaskForPZ, "id", "taskName", debit_WorkBit.id_TaskForPZ);
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "PlanZakaz", "PlanZakaz", debit_WorkBit.id_PlanZakaz);
            ViewBag.idTask = idTask;
            PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(debit_WorkBit.id_PlanZakaz);
            ViewBag.PZTitle = pZ_PlanZakaz.PlanZakaz.ToString();

            return View(debit_WorkBit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Debit_WorkBit debit_WorkBit, int? idTask)
        {
            if (ModelState.IsValid)
            {

                //Зарегистрировать в системе ПЗ (Project, 1c, письмо об открытии заказа)
                if (debit_WorkBit.close == true & debit_WorkBit.id_TaskForPZ == 1)
                {
                    CloseComplitedTasks(debit_WorkBit.id_PlanZakaz, 1);
                    List<string> list = new List<string>();
                    var debit_EmailList = db.Debit_EmailList.Where(d => d.C1 == true).ToList();
                    foreach (var data in debit_EmailList)
                    {
                        list.Add(data.email);
                    }
                    SendMail(debit_WorkBit, " : зарегистрирован в информационной системе (1с / Project / Реестр заказов)", list);
                    string adress = db.Folder.Find(1).adres.ToString();
                    adress = GetAdressFolder(debit_WorkBit.id_PlanZakaz);
                    PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(debit_WorkBit.id_PlanZakaz);
                    pZ_PlanZakaz.Folder = adress;
                    db.Entry(pZ_PlanZakaz).State = EntityState.Modified;
                    db.SaveChanges();
                    RKD rKD = new RKD(pZ_PlanZakaz.Id);
                    rKD.CreateTasks();
                    return RedirectToAction("Index");
                }
                //Подготовить вагонную ведомость*
                if (debit_WorkBit.close == true & debit_WorkBit.id_TaskForPZ == 9)
                {
                    CloseComplitedTasks(debit_WorkBit.id_PlanZakaz, 9);
                    return RedirectToAction("Index");
                }

                if (debit_WorkBit.close == true & debit_WorkBit.id_TaskForPZ == 28)
                {
                    CloseComplitedTasks(debit_WorkBit.id_PlanZakaz, 28);
                    return RedirectToAction("Index");
                }
                if (debit_WorkBit.close == true & debit_WorkBit.id_TaskForPZ == 13)
                {
                    CloseComplitedTasks(debit_WorkBit.id_PlanZakaz, 13);
                    List<string> list = new List<string>();
                    var debit_EmailList = db.Debit_EmailList.Where(d => d.C13 == true).ToList();
                    foreach (var data in debit_EmailList)
                    {
                        list.Add(data.email);
                    }
                    SendMail(debit_WorkBit, " : подтверждено оприходование заказа", list);
                    return RedirectToAction("Index");
                }
                if (debit_WorkBit.close == true & debit_WorkBit.id_TaskForPZ == 14)
                {
                    CloseComplitedTasks(debit_WorkBit.id_PlanZakaz, 14);
                    List<string> list = new List<string>();
                    var debit_EmailList = db.Debit_EmailList.Where(d => d.C13 == true).ToList();
                    foreach (var data in debit_EmailList)
                    {
                        list.Add(data.email);
                    }
                    SendMail(debit_WorkBit, " : подтверждено оприходование заказа", list);
                    return RedirectToAction("Index");
                }
                if (debit_WorkBit.close == true & debit_WorkBit.id_TaskForPZ == 25)
                {
                    CloseComplitedTasks(debit_WorkBit.id_PlanZakaz, 25);
                    return RedirectToAction("Index");
                }
                if (debit_WorkBit.close == true)
                {
                    debit_WorkBit.dateClose = DateTime.Now;

                    if (debit_WorkBit.id_TaskForPZ == 1)
                    {
                        string adress = db.Folder.Find(1).adres.ToString();
                        adress = GetAdressFolder(debit_WorkBit.id_PlanZakaz);
                        PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(debit_WorkBit.id_PlanZakaz);
                        pZ_PlanZakaz.Folder = adress;
                        db.Entry(pZ_PlanZakaz).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                db.Entry(debit_WorkBit).State = EntityState.Modified;
                db.SaveChanges();
                int idOrder = debit_WorkBit.id_PlanZakaz;
                if (debit_WorkBit.close == true & debit_WorkBit.id_TaskForPZ == 3)
                {
                    CloseComplitedTasks(debit_WorkBit.id_PlanZakaz, 3);
                    EmailModel emailModel = new EmailModel();
                    List<string> list = new List<string>();
                    var debit_EmailList = db.Debit_EmailList.Where(d => d.C3 == true).ToList();
                    foreach (var data in debit_EmailList)
                    {
                        list.Add(data.email);
                    }
                    SendMail(debit_WorkBit, " : прототипы по производству внесены в Project", list);
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Index");
            }
            ViewBag.id_TaskForPZ = new SelectList(db.TaskForPZ, "id", "taskName", debit_WorkBit.id_TaskForPZ);
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_WorkBit.id_PlanZakaz);
            return View(debit_WorkBit);
        }

        public string GetAdressFolder(int idPz)
        {
            int substringLan = 0;
            string adres = db.Folder.Find(1).adres;
            adres += db.FolderDocument.Find(2).adres;

            int planZakaz = db.PZ_PlanZakaz.Find(idPz).PlanZakaz;
            string[] files = Directory.GetDirectories(adres, "*.*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    if (Convert.ToInt32(files[i].Substring(files[i].Length - 4)) > planZakaz)
                    {
                        adres = files[i];
                        break;
                    }
                }
                catch
                { }
            }
            substringLan = adres.Length + 1;
            files = null;
            files = Directory.GetDirectories(adres, "*.*", SearchOption.TopDirectoryOnly);
            string myLan = "";
            string myLan2 = "";
            for (int i = 0; i < files.Length; i++)
            {
                myLan = files[i];
                myLan = files[i].Substring(substringLan, files[i].Length - substringLan);
                myLan = myLan.Substring(0, 4);
                try
                {
                    myLan2 = files[i];
                    myLan2 = files[i].Substring(substringLan, files[i].Length - substringLan);
                    myLan2 = myLan2.Substring(5, 4);
                    int counterStep = Convert.ToInt32(myLan2) - Convert.ToInt32(myLan);
                    for (int j = 0; j <= counterStep; j++)
                    {
                        try
                        {
                            if (Convert.ToInt32(myLan) + j == planZakaz)
                            {
                                adres = files[i];
                                if (Convert.ToInt32(myLan) + j > planZakaz)
                                {
                                    adres = files[i - 1];
                                    break;
                                }
                                break;
                            }
                        }
                        catch
                        { }
                    }
                }
                catch
                {
                    try
                    {
                        if (Convert.ToInt32(myLan) == planZakaz)
                        {
                            adres = files[i];
                            if (Convert.ToInt32(myLan) > planZakaz)
                            {
                                adres = files[i - 1];
                                break;
                            }
                            break;
                        }
                    }
                    catch
                    { }
                }
            }
            
            return adres;
        }

        public void CreateNewTasks(int predecessors, int idOrder)
        {
            List<TaskForPZ> dateTaskWork = db.TaskForPZ.Where(w => w.Predecessors == predecessors).Where(z => z.id_TypeTaskForPZ == 1).ToList();
            foreach (var data in dateTaskWork)
            {
                Debit_WorkBit newDebit_WorkBit = new Debit_WorkBit();
                newDebit_WorkBit.dateCreate = DateTime.Now;
                newDebit_WorkBit.close = false;
                newDebit_WorkBit.id_PlanZakaz = idOrder;
                newDebit_WorkBit.id_TaskForPZ = (int)data.id;
                newDebit_WorkBit.datePlanFirst = DateTime.Now.AddDays((double)data.time);
                newDebit_WorkBit.datePlan = DateTime.Now.AddDays((double)data.time);
                db.Debit_WorkBit.Add(new Debit_WorkBit()
                {
                    close = false,
                    dateClose = null,
                    dateCreate = DateTime.Now,
                    datePlan = newDebit_WorkBit.datePlan,
                    datePlanFirst = newDebit_WorkBit.datePlanFirst,
                    id_PlanZakaz = newDebit_WorkBit.id_PlanZakaz,
                    id_TaskForPZ = newDebit_WorkBit.id_TaskForPZ
                });
                db.SaveChanges();
            }
        }

        public void CloseComplitedTasks(int idPlanZakaz, int idTask)
        {
            try
            {
                Debit_WorkBit debit_WorkBit = new Debit_WorkBit();
                debit_WorkBit = db.Debit_WorkBit
                    .Where(d => d.id_PlanZakaz == idPlanZakaz)
                    .Where(d => d.id_TaskForPZ == idTask)
                    .Where(d => d.close == false)
                    .First();
                if (debit_WorkBit.close == false)
                {
                    debit_WorkBit.close = true;
                    debit_WorkBit.dateClose = DateTime.Now;
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                }
                CreateNewTasks(idTask, idPlanZakaz);
            }
            catch
            { }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(id);
            if (debit_WorkBit == null)
            {
                return HttpNotFound();
            }
            return View(debit_WorkBit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(id);
            db.Debit_WorkBit.Remove(debit_WorkBit);
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
        
        public ActionResult ReportBasic()
        {
            ViewBag.TaskList = db.Debit_WorkBit.Where(d => d.close == false).ToList();
            return View();
        }
        
        public ActionResult UpdateFolderPZ()
        {
            var listFile = Directory.GetFiles(@"\\192.168.1.30\m$\_ЗАКАЗЫ\", "*",SearchOption.AllDirectories);
            return View();
        }

    }
}