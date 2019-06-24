using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.ServiceReclamations.Models;
using System.Data.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using Wiki.Areas.Reclamation.Controllers;

namespace Wiki.Areas.ServiceReclamations.Controllers
{
    public class MarksController : Controller
    {
        readonly JsonSerializerSettings shortSettingLeftRight = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy HH:mm" };

        public ActionResult Index()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            ViewBag.PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.PlanZakaz < 7000).OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_Reclamation_Type = new SelectList(db.Reclamation_Type.Where(d => d.activeService == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_ServiceRemarksCause = new SelectList(db.ServiceRemarksCause.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            string login = HttpContext.User.Identity.Name;
            int devisionUser = 0;
            try
            {
                devisionUser = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            }
            catch
            {

            }
            if (devisionUser == 28)
                ViewBag.userGroupId = 1;
            else
                ViewBag.userGroupId = 0;

            ViewBag.typeRem = new SelectList(db.Reclamation_Type.Where(d => d.activeService == true).OrderBy(d => d.name), "id", "name");
            ViewBag.devRem = new SelectList(db.Devision.Where(d => d.OTK == true).OrderBy(d => d.name), "id", "name");
            ViewBag.pfRem = new SelectList(db.PF.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            return View();
        }

        public JsonResult Add(ServiceRemarks reclamation, int[] pZ_PlanZakaz, int[] id_Reclamation_Type, int[] id_ServiceRemarksCause)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                if (reclamation.description == null)
                    reclamation.description = "";
                if (reclamation.folder == null)
                    reclamation.folder = "";
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string login = HttpContext.User.Identity.Name;
                reclamation.dateTimeCreate = DateTime.Now;
                reclamation.userCreate = db.AspNetUsers.First(d => d.Email == login).Id;
                if (reclamation.description == null)
                    reclamation.description = "";
                if (reclamation.folder == null)
                    reclamation.folder = "";
                db.ServiceRemarks.Add(reclamation);
                db.SaveChanges();
                string directory = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Рекламации_Сервисного_Центра\" + reclamation.id.ToString();
                Directory.CreateDirectory(directory);
                reclamation.folder = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Рекламации_Сервисного_Центра\" + reclamation.id.ToString() + @"\";
                db.Entry(reclamation).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var data in pZ_PlanZakaz)
                {
                    ServiceRemarksPlanZakazs remarkOrder = new ServiceRemarksPlanZakazs();
                    remarkOrder.id_ServiceRemarks = reclamation.id;
                    remarkOrder.id_PZ_PlanZakaz = data;
                    db.ServiceRemarksPlanZakazs.Add(remarkOrder);
                    db.SaveChanges();
                }
                foreach (var data in id_Reclamation_Type)
                {
                    ServiceRemarksTypes remarkOrder = new ServiceRemarksTypes();
                    remarkOrder.id_ServiceRemarks = reclamation.id;
                    remarkOrder.id_Reclamation_Type = data;
                    db.ServiceRemarksTypes.Add(remarkOrder);
                    db.SaveChanges();
                }
                foreach (var data in id_ServiceRemarksCause)
                {
                    ServiceRemarksCauses remarkOrder = new ServiceRemarksCauses();
                    remarkOrder.id_ServiceRemarks = reclamation.id;
                    remarkOrder.id_ServiceRemarksCause = data;
                    db.ServiceRemarksCauses.Add(remarkOrder);
                    db.SaveChanges();
                }
            }
            return Json(reclamation.id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.ServiceRemarks
                    .Include(d => d.ServiceRemarksPlanZakazs)
                    .Include(d => d.ServiceRemarksTypes)
                    .Include(d => d.ServiceRemarksCauses)
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.id == id).ToList();
                var data = query.Select(dataList => new
                {
                    numberReclamation = "Рекламация №: " + dataList.id,
                    dataList.id,
                    pZ_PlanZakaz = GetPlanZakazArray(dataList.ServiceRemarksPlanZakazs.ToList()),
                    id_Reclamation_Type = GetTypesArray(dataList.ServiceRemarksTypes.ToList()),
                    id_ServiceRemarksCause = GetCausesArray(dataList.ServiceRemarksCauses.ToList()),
                    dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settings).Replace(@"""", ""),
                    userCreate = dataList.AspNetUsers.CiliricalName,
                    datePutToService = JsonConvert.SerializeObject(dataList.datePutToService, shortSettingLeftRight).Replace(@"""", ""),
                    dateClose = JsonConvert.SerializeObject(dataList.dateClose, shortSettingLeftRight).Replace(@"""", ""),
                    dataList.folder,
                    dataList.text,
                    dataList.description,
                    answerHistiryText = GetHistoryText(dataList.id)
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        string GetHistoryText(int id)
        {
            string answer = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var listAnswers = db.ServiceRemarksActions
                    .Include(d => d.AspNetUsers)
                    .Where(d => d.id_ServiceRemarks == id)
                    .OrderByDescending(d => d.dateTimeCreate)
                    .ToList();
                foreach (var data in listAnswers)
                {
                    answer += data.dateTimeCreate.ToShortDateString() + " | " + data.AspNetUsers.CiliricalName + " | " + data.text + "\n";
                }
            }
            return answer;
        }

        string[] GetPlanZakazArray(List<ServiceRemarksPlanZakazs> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_PZ_PlanZakaz.ToString();
            }
            return pZ_PlanZakaz;
        }

        string[] GetTypesArray(List<ServiceRemarksTypes> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_Reclamation_Type.ToString();
            }
            return pZ_PlanZakaz;
        }

        string[] GetCausesArray(List<ServiceRemarksCauses> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_ServiceRemarksCause.ToString();
            }
            return pZ_PlanZakaz;
        }

        public JsonResult Update(ServiceRemarks reclamation, int[] pZ_PlanZakaz, int[] id_Reclamation_Type, int[] id_ServiceRemarksCause, string answerText)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                ServiceRemarks beforeUpdateRemark = db.ServiceRemarks.Find(reclamation.id);
                try
                {
                    beforeUpdateRemark.dateClose = reclamation.dateClose;
                }
                catch
                {

                }
                if (reclamation.description == null)
                    reclamation.description = "";
                if (reclamation.folder == null)
                    reclamation.folder = "";
                beforeUpdateRemark.datePutToService = reclamation.datePutToService;
                beforeUpdateRemark.description = reclamation.description;
                beforeUpdateRemark.folder = reclamation.folder;
                beforeUpdateRemark.text = reclamation.text;
                db.Entry(beforeUpdateRemark).State = EntityState.Modified;
                db.SaveChanges();
                if (answerText != "")
                {
                    string login = HttpContext.User.Identity.Name;
                    ServiceRemarksActions serviceRemarksActions = new ServiceRemarksActions();
                    serviceRemarksActions.dateTimeCreate = DateTime.Now;
                    serviceRemarksActions.id_AspNetUsersCreate = db.AspNetUsers.First(d => d.Email == login).Id;
                    serviceRemarksActions.id_ServiceRemarks = beforeUpdateRemark.id;
                    serviceRemarksActions.text = answerText;
                    db.ServiceRemarksActions.Add(serviceRemarksActions);
                    db.SaveChanges();
                }
                UpdatePZList(pZ_PlanZakaz, beforeUpdateRemark.id);
                UpdateTypes(id_Reclamation_Type, beforeUpdateRemark.id);
                UpdateCause(id_ServiceRemarksCause, beforeUpdateRemark.id);
                return Json(beforeUpdateRemark.id, JsonRequestBehavior.AllowGet);
            }
        }

        bool UpdateCause(int[] pZ_PlanZakaz, int id_Reclamation)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var listLastPz = db.ServiceRemarksCauses.Where(d => d.id_ServiceRemarks == id_Reclamation).ToList();
                foreach (var lastReclamationPZ in listLastPz)
                {
                    if (pZ_PlanZakaz.Where(d => d == lastReclamationPZ.id_ServiceRemarksCause).Count() == 0)
                    {
                        db.ServiceRemarksCauses.Remove(lastReclamationPZ);
                        db.SaveChanges();
                    }
                }
                listLastPz = db.ServiceRemarksCauses.Where(d => d.id_ServiceRemarks == id_Reclamation).ToList();
                foreach (var newReclamationPZ in pZ_PlanZakaz)
                {
                    if (listLastPz.Where(d => d.id_ServiceRemarksCause == newReclamationPZ).Count() == 0)
                    {
                        ServiceRemarksCauses reclamation_PZ = new ServiceRemarksCauses
                        {
                            id_ServiceRemarksCause = newReclamationPZ,
                            id_ServiceRemarks = id_Reclamation
                        };
                        db.ServiceRemarksCauses.Add(reclamation_PZ);
                        db.SaveChanges();
                    }
                }
            }
            return true;
        }

        bool UpdateTypes(int[] pZ_PlanZakaz, int id_Reclamation)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var listLastPz = db.ServiceRemarksTypes.Where(d => d.id_ServiceRemarks == id_Reclamation).ToList();
                foreach (var lastReclamationPZ in listLastPz)
                {
                    if (pZ_PlanZakaz.Where(d => d == lastReclamationPZ.id_Reclamation_Type).Count() == 0)
                    {
                        db.ServiceRemarksTypes.Remove(lastReclamationPZ);
                        db.SaveChanges();
                    }
                }
                listLastPz = db.ServiceRemarksTypes.Where(d => d.id_ServiceRemarks == id_Reclamation).ToList();
                foreach (var newReclamationPZ in pZ_PlanZakaz)
                {
                    if (listLastPz.Where(d => d.id_Reclamation_Type == newReclamationPZ).Count() == 0)
                    {
                        ServiceRemarksTypes reclamation_PZ = new ServiceRemarksTypes
                        {
                            id_Reclamation_Type = newReclamationPZ,
                            id_ServiceRemarks = id_Reclamation
                        };
                        db.ServiceRemarksTypes.Add(reclamation_PZ);
                        db.SaveChanges();
                    }
                }
            }
            return true;
        }

        bool UpdatePZList(int[] pZ_PlanZakaz, int id_Reclamation)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var listLastPz = db.ServiceRemarksPlanZakazs.Where(d => d.id_ServiceRemarks == id_Reclamation).ToList();

                foreach (var lastReclamationPZ in listLastPz)
                {
                    if (pZ_PlanZakaz.Where(d => d == lastReclamationPZ.id_PZ_PlanZakaz).Count() == 0)
                    {
                        db.ServiceRemarksPlanZakazs.Remove(lastReclamationPZ);
                        db.SaveChanges();
                    }
                }
                listLastPz = db.ServiceRemarksPlanZakazs.Where(d => d.id_ServiceRemarks == id_Reclamation).ToList();
                foreach (var newReclamationPZ in pZ_PlanZakaz)
                {
                    if (listLastPz.Where(d => d.id_PZ_PlanZakaz == newReclamationPZ).Count() == 0)
                    {
                        ServiceRemarksPlanZakazs reclamation_PZ = new ServiceRemarksPlanZakazs
                        {
                            id_PZ_PlanZakaz = newReclamationPZ,
                            id_ServiceRemarks = id_Reclamation
                        };
                        db.ServiceRemarksPlanZakazs.Add(reclamation_PZ);
                        db.SaveChanges();
                    }
                }
            }
            return true;
        }

        [HttpPost]
        public JsonResult ActiveList()
        {
            var query = new ReclamationsList().GetActive();
            var data = query.Select(dataList => new
            {
                dataList.id,
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                dateGet = JsonConvert.SerializeObject(dataList.datePutToService, shortSetting).Replace(@"""", ""),
                dateClose = JsonConvert.SerializeObject(dataList.dateClose, shortSetting).Replace(@"""", ""),
                folder = GetFolderLink(dataList.folder)
            });
            return Json(new { data });
        }

        string GetFolderLink(string folder)
        {
            string folderLink = "";
            folderLink = @"<a href =" + folder + " target='_explorer.exe'> Папка </a>";
            return folderLink;
        }

        [HttpPost]
        public JsonResult CloseList()
        {
            var query = new ReclamationsList().GetClose();
            var data = query.Select(dataList => new
            {
                dataList.id,
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                dateGet = JsonConvert.SerializeObject(dataList.datePutToService, shortSetting).Replace(@"""", ""),
                dateClose = JsonConvert.SerializeObject(dataList.dateClose, shortSetting).Replace(@"""", ""),
                folder = @"<a href =" + dataList.folder + "> Папка </a>"
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult AllList()
        {
            var query = new ReclamationsList().GetAll();
            var data = query.Select(dataList => new
            {
                dataList.id,
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                dateGet = JsonConvert.SerializeObject(dataList.datePutToService, shortSetting).Replace(@"""", ""),
                dateClose = JsonConvert.SerializeObject(dataList.dateClose, shortSetting).Replace(@"""", ""),
                folder = @"<a href =" + dataList.folder + "> Папка </a>"
            });
            return Json(new { data });
        }

        string GetOrdersName(int id)
        {
            string ordersName = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var dataList = db.ServiceRemarksPlanZakazs
                    .Include(d => d.PZ_PlanZakaz)
                    .Where(d => d.id_ServiceRemarks == id)
                    .ToList();
                foreach (var data in dataList)
                {
                    ordersName += data.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
                }
            }
            return ordersName;
        }

        string GetClient(int id)
        {
            string clientName = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                int idPz = db.ServiceRemarksPlanZakazs
                    .First(d => d.id_ServiceRemarks == id)
                    .id_PZ_PlanZakaz;
                clientName = db.PZ_PlanZakaz.Include(d => d.PZ_Client).First(d => d.Id == idPz).PZ_Client.NameSort;
            }
            return clientName;
        }

        string GetTypes(int id)
        {
            string typesName = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var dataList = db.ServiceRemarksTypes
                    .Include(d => d.Reclamation_Type)
                    .Where(d => d.id_ServiceRemarks == id)
                    .ToList();
                foreach (var data in dataList)
                {
                    typesName += data.Reclamation_Type.name + "; ";
                }
            }
            return typesName;
        }

        string GetCauses(int id)
        {
            string causesName = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var dataList = db.ServiceRemarksCauses
                    .Include(d => d.ServiceRemarksCause)
                    .Where(d => d.id_ServiceRemarks == id)
                    .ToList();
                foreach (var data in dataList)
                {
                    causesName += data.ServiceRemarksCause.name + "; ";
                }
            }
            return causesName;
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

        public JsonResult CreateAnClosePZ(int[] npZ_PlanZakaz)
        {
            var query = new ReclamationsList().GetActive();
            var data = query.Select(dataList => new
            {
                dataList.id,
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getView('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                orders = GetOrdersName(dataList.id),
                client = GetClient(dataList.id),
                dataList.text,
                dataList.description,
                types = GetTypes(dataList.id),
                causes = GetCauses(dataList.id),
                dateOpen = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                dateGet = JsonConvert.SerializeObject(dataList.datePutToService, shortSetting).Replace(@"""", ""),
                dateClose = JsonConvert.SerializeObject(dataList.dateClose, shortSetting).Replace(@"""", ""),
                folder = GetFolderLink(dataList.folder)
            });
            return Json(new { data });
        }

        public ActionResult ToExcel()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            ViewBag.PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.PlanZakaz < 7000).OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
        }
            
        [HttpPost]
        public ActionResult ToExcel(int[] npZ_PlanZakaz)
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Worksheet1");
                var excelWorksheet = excel.Workbook.Worksheets["Worksheet1"];
                List<string[]> headerRow = new List<string[]>()
                {
                  new string[] { "№", "Текст", "Прим.", "История переписк", "Ответственное СП", "Полуфабрикат", "РСАМ", "Автор замечания" }
                };
                int rowNum = 1;
                string headerRange = "A" + rowNum.ToString() + ":" + char.ConvertFromUtf32(headerRow[0].Length + 64) + rowNum.ToString();
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                worksheet.Cells[headerRange].Style.Font.Bold = true;
                worksheet.Cells[headerRange].Style.Font.Size = 14;
                worksheet.Cells[headerRange].Style.Font.Color.SetColor(Color.Blue);
                rowNum++;
                foreach (var data in npZ_PlanZakaz)
                {
                    var listReclamation = db.Reclamation
                        .Include(d => d.Reclamation_PZ)
                        .Include(d => d.AspNetUsers)
                        .Include(d => d.Devision)
                        .Include(d => d.PF)
                        .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                        .Where(d => d.Reclamation_PZ.Where(sd => sd.id_PZ_PlanZakaz == data).Count() > 0)
                        .ToList();
                    foreach (var dataReclamation in listReclamation)
                    {
                        headerRow = new List<string[]>()
                        {
                          new string[] { dataReclamation.id.ToString(), dataReclamation.text, dataReclamation.description, new RemarksController().GetAnswerText(dataReclamation.Reclamation_Answer.ToList()), dataReclamation.Devision.name, dataReclamation.PF.name, dataReclamation.PCAM, dataReclamation.AspNetUsers.CiliricalName }
                        };
                        headerRange = "A" + rowNum.ToString() + ":" + char.ConvertFromUtf32(headerRow[0].Length + 64) + rowNum.ToString();
                        worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                        worksheet.Cells[headerRange].Style.Font.Bold = true;
                        worksheet.Cells[headerRange].Style.Font.Size = 14;
                        worksheet.Cells[headerRange].Style.Font.Color.SetColor(Color.Blue);
                        rowNum++;
                    }
                }
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-daisposition", "attachment: filename=" + "ExcelReport.xlsx");
                Response.BinaryWrite(excel.GetAsByteArray());
                Response.End();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult RemList(int id)
        {
            using (PortalKATEKEntities dbc = new PortalKATEKEntities())
            {
                dbc.Configuration.ProxyCreationEnabled = false;
                dbc.Configuration.LazyLoadingEnabled = false;
                var query = dbc.ServiceRemarksReclamations
                    .AsNoTracking()
                    .Include(d => d.Reclamation.Reclamation_Type)
                    .Include(d => d.Reclamation.PF)
                    .Include(d => d.Reclamation.Devision)
                    .Where(d => d.id_ServiceRemarks == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.Reclamation.id,
                    devision = dataList.Reclamation.Devision.name,
                    dataList.Reclamation.text
                });
                return Json(new { data });
            }
        }

        public JsonResult AddRem(int id, int[] pZ_PlanZakaz, int typeRem, int devRem, string textRem,
            int pfRem, bool technicalAdviceRem)
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                Wiki.Reclamation reclamation = new Wiki.Reclamation
                {
                    id_Reclamation_Type = typeRem,
                    id_DevisionReclamation = devRem,
                    id_Reclamation_CountErrorFinal = 1,
                    id_Reclamation_CountErrorFirst = 1,
                    id_AspNetUsersCreate = db.AspNetUsers.First(d => d.Email == login).Id,
                    id_DevisionCreate = db.AspNetUsers.First(d => d.Email == login).Devision.Value,
                    dateTimeCreate = DateTime.Now,
                    text = textRem,
                    description = "",
                    timeToSearch = 0,
                    timeToEliminate = 0,
                    close = true,
                    gip = false,
                    closeDevision = false,
                    PCAM = "",
                    editManufacturing = false,
                    id_PF = pfRem,
                    technicalAdvice = technicalAdviceRem,
                    closeMKO = false,
                    fixedExpert = false
                };
                db.Reclamation.Add(reclamation);
                db.SaveChanges();
                foreach (var data in pZ_PlanZakaz)
                {
                    Reclamation_PZ reclamation_PZ = new Reclamation_PZ
                    {
                        id_PZ_PlanZakaz = data,
                        id_Reclamation = reclamation.id
                    };
                    db.Reclamation_PZ.Add(reclamation_PZ);
                    db.SaveChanges();
                }
            }
            return Json(id, JsonRequestBehavior.AllowGet);
        }

    }
}