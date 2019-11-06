using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System;
using Newtonsoft.Json;
using Wiki.Areas.CMKO.Types;

namespace Wiki.Areas.CMKO.Controllers
{
    public class CMKController : Controller
    {
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly JsonSerializerSettings settingsDNY = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        PortalKATEKEntities db = new PortalKATEKEntities();
        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            ViewBag.periodTeach = new SelectList(db.CMKO_PeriodResult
                                                       .Where(d => d.close == false)
                                                       .OrderByDescending(d => d.period), "period", "period");
            ViewBag.categoryUser = new SelectList(db.CMKO_TaxCatigories.OrderBy(d => d.catigoriesName), "id", "catigoriesName");
            ViewBag.period = new SelectList(db.CMKO_PeriodResult.Where(d => d.close == false).OrderByDescending(x => x.period), "period", "period");
            if (login == "myi@katek.by")
            {
                 ViewBag.autor = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO != null)
                    .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO == null)
                    .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");

            }
            else if (login == "Kuchynski@katek.by")
            {
                ViewBag.autor = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO != null)
                    .Where(d => d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO == null)
                    .Where(d => d.Devision == 16)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
            }
            else if (login == "nrf@katek.by")
            {
                ViewBag.autor = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.Devision == 15)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO != null)
                    .Where(d => d.Devision == 15)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO == null)
                    .Where(d => d.Devision == 15)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
            }
            else if (login == "fvs@katek.by")
            {
                ViewBag.autor = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.Devision == 3)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO != null)
                    .Where(d => d.Devision == 3)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers
                    .Where(d => d.LockoutEnabled == true)
                    .Where(d => d.dateToCMKO == null)
                    .Where(d => d.Devision == 3)
                    .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
            }
            else 
            {
                ViewBag.autor = new SelectList(db.AspNetUsers.Where(d => d.Id == ""), "Id", "CiliricalName");
                ViewBag.teacherTeach = new SelectList(db.AspNetUsers.Where(d => d.Id == ""), "Id", "CiliricalName");
                ViewBag.studentTeach = new SelectList(db.AspNetUsers.Where(d => d.Id == ""), "Id", "CiliricalName");
                ViewBag.periodTeach = new SelectList(db.CMKO_PeriodResult.Where(d => d.period == ""), "period", "period");
                ViewBag.categoryUser = new SelectList(db.CMKO_TaxCatigories.Where(d => d.id == 0), "id", "catigoriesName");
                ViewBag.period = new SelectList(db.CMKO_PeriodResult.Where(d => d.period == ""), "period", "period");
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetOptimizationList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Optimization
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.CMKO_PeriodResult)
                .Include(d => d.AspNetUsers2)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkOptimization(login, dataList.id),
                dateCreate = dataList.datetimeCreate.ToShortDateString() + " " + dataList.datetimeCreate.ToShortTimeString(),
                userCreate = dataList.AspNetUsers.CiliricalName,
                autor = dataList.AspNetUsers2.CiliricalName,
                textData = dataList.description,
                dataList.CMKO_PeriodResult.period
            });
            return Json(new { data });
        }

        string GetEditLinkOptimization(string login, int id)
        {
            if (login == "myi@katek.by" || login == "Kuchynski@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOptimization('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        public JsonResult AddOptimization(CMKO_Optimization optimization)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            optimization.id_AspNetUsersCreate = db.AspNetUsers.First(d => d.Email == login).Id;
            optimization.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            optimization.datetimeCreate = DateTime.Now;
            optimization.dateTimeUpdate = DateTime.Now;
            optimization.histiryEdit = "";
            db.CMKO_Optimization.Add(optimization);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOptimization(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Optimization
                .AsNoTracking()
                .Where(d => d.id == id)
                .Include(d => d.CMKO_PeriodResult)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers2)
                .ToList();
            var data = query.Select(dataList => new
            {
                idOptimization = dataList.id,
                dateCreate = dataList.datetimeCreate.ToShortDateString() + " " + dataList.datetimeCreate.ToShortTimeString(),
                userCreate = dataList.AspNetUsers.CiliricalName,
                autor = dataList.id_AspNetUsersIdea,
                textData = dataList.description,
                dataList.CMKO_PeriodResult.period
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateOptimization(CMKO_Optimization optimization)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Optimization updateOptimization = db.CMKO_Optimization.First(d => d.id == optimization.id);
            updateOptimization.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            updateOptimization.dateTimeUpdate = DateTime.Now;
            if (updateOptimization.description != optimization.description)
                updateOptimization.description = optimization.description;
            if (updateOptimization.id_AspNetUsersIdea != optimization.id_AspNetUsersIdea)
                updateOptimization.id_AspNetUsersIdea = optimization.id_AspNetUsersIdea;
            if (updateOptimization.id_CMKO_PeriodResult != optimization.id_CMKO_PeriodResult)
                updateOptimization.id_CMKO_PeriodResult = optimization.id_CMKO_PeriodResult;
            db.Entry(updateOptimization).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveOptimization(CMKO_Optimization optimization)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Optimization updateOptimization = db.CMKO_Optimization.First(d => d.id == optimization.id);
            db.Entry(updateOptimization).State = EntityState.Deleted;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTeachList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Teach
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.AspNetUsers2)
                .Include(d => d.CMKO_PeriodResult)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkTeach(login, dataList.id),
                dateCreate = dataList.datetimeCreate.ToShortDateString() + " " + dataList.datetimeCreate.ToShortTimeString(),
                userCreate = dataList.AspNetUsers2.CiliricalName,
                teacher = dataList.AspNetUsers.CiliricalName,
                student = dataList.AspNetUsers1.CiliricalName,
                dataList.description,
                dataList.CMKO_PeriodResult.period
            });
            return Json(new { data });
        }

        string GetEditLinkTeach(string login, int id)
        {
            if (login == "myi@katek.by" || login == "Kuchynski@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetTeach('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        public JsonResult AddTeach(CMKO_Teach data)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            data.id_AspNetUsersCreate = db.AspNetUsers.First(d => d.Email == login).Id;
            data.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            data.datetimeCreate = DateTime.Now;
            data.datetimeUpdate = DateTime.Now;
            data.historyEdit = "";
            if (data.description == null)
                data.description = "";
            db.CMKO_Teach.Add(data);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeach(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Teach
                .AsNoTracking()
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idTeach = dataList.id,
                teacherTeach = dataList.id_AspNetUsersTeacher,
                studentTeach = dataList.id_AspNetUsersTeach,
                periodTeach = dataList.id_CMKO_PeriodResult,
                costTeach = dataList.cost,
                descriptionTeach = dataList.description
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTeach(CMKO_Teach postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Teach updateData = db.CMKO_Teach.First(d => d.id == postData.id);
            updateData.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            updateData.datetimeUpdate = DateTime.Now;
            if (updateData.description != postData.description && postData.description != null)
                updateData.description = postData.description;
            if (updateData.id_AspNetUsersTeach != postData.id_AspNetUsersTeach)
                updateData.id_AspNetUsersTeach = postData.id_AspNetUsersTeach;
            if (updateData.id_AspNetUsersTeacher != postData.id_AspNetUsersTeacher)
                updateData.id_AspNetUsersTeacher = postData.id_AspNetUsersTeacher;
            if (updateData.cost != postData.cost)
                updateData.cost = postData.cost;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveTeach(CMKO_Teach postData)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Teach updateData = db.CMKO_Teach.First(d => d.id == postData.id);
            db.Entry(updateData).State = EntityState.Deleted;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetUsersList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.AspNetUsers
                .AsNoTracking()
                .Where(d => d.LockoutEnabled == true)
                .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                .Include(d => d.CMKO_TaxCatigories)
                .Include(d => d.Devision1)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkUser(login, dataList.Id),
                ciliricName = dataList.CiliricalName,
                devisionName = dataList.Devision1.name,
                category = dataList.CMKO_TaxCatigories.catigoriesName,
                dateToCMKO = JsonConvert.SerializeObject(dataList.dateToCMKO, settings).Replace(@"""", ""),
                dataList.tax
            });
            return Json(new { data });
        }

        string GetEditLinkUser(string login, string id)
        {
            if (login == "myi@katek.by" || login == "Kuchynski@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetUser('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        public JsonResult GetUser(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.AspNetUsers
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Include(d => d.Devision1)
                .ToList();
            var data = query.Select(dataList => new
            {
                idUser = dataList.Id,
                ciliricNameUser = dataList.CiliricalName,
                devisionNameUser = dataList.Devision1.name,
                categoryUser = dataList.id_CMKO_TaxCatigories,
                dateToCMKO = JsonConvert.SerializeObject(dataList.dateToCMKO, settingsDNY).Replace(@"""", ""),
                taxUser = dataList.tax
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateUser(AspNetUsers postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            AspNetUsers updateData = db.AspNetUsers.First(d => d.Id == postData.Id);
            if (updateData.id_CMKO_TaxCatigories != postData.id_CMKO_TaxCatigories)
                updateData.id_CMKO_TaxCatigories = postData.id_CMKO_TaxCatigories;
            if (updateData.dateToCMKO != postData.dateToCMKO)
                updateData.dateToCMKO = postData.dateToCMKO;
            if (updateData.tax != postData.tax)
                updateData.tax = postData.tax;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCategoryList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_TaxCatigories
                .AsNoTracking()
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkCategory(login, dataList.id),
                nameCategory = dataList.catigoriesName,
                selaryCategory = dataList.salary
            });
            return Json(new { data });
        }

        string GetEditLinkCategory(string login, int id)
        {
            if (login == "myi@katek.by" || login == "Kuchynski@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetCategory('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        public JsonResult AddCategory(CMKO_TaxCatigories data)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            db.CMKO_TaxCatigories.Add(data);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategory(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_TaxCatigories
                .AsNoTracking()
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idCategory = dataList.id,
                nameCategory = dataList.catigoriesName,
                selaryCategory = dataList.salary
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCategory(CMKO_TaxCatigories postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_TaxCatigories updateData = db.CMKO_TaxCatigories.First(d => d.id == postData.id);
            if (updateData.catigoriesName != postData.catigoriesName)
                updateData.catigoriesName = postData.catigoriesName;
            if (updateData.salary != postData.salary)
                updateData.salary = postData.salary;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPeriodList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_PeriodResult
                .AsNoTracking()
                .ToList();
            var data = query.Select(dataList => new
            {
                dataList.period
            });
            return Json(new { data });
        }

        public JsonResult AddPeriod(CMKO_PeriodResult data)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            data.close = false;
            db.CMKO_PeriodResult.Add(data);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCalendList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.ProductionCalendar
                .AsNoTracking()
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkProductionCalend(login, dataList.id),
                dataList.period,
                dataList.timeToOnePerson
            });
            return Json(new { data });
        }

        string GetEditLinkProductionCalend(string login, int id)
        {
            if (login == "myi@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetCalend('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        public JsonResult AddCalend(ProductionCalendar data)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            db.ProductionCalendar.Add(data);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCalend(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.ProductionCalendar
                .AsNoTracking()
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idCalend = dataList.id,
                periodCalend = dataList.period,
                timeToOnePersonCalend = dataList.timeToOnePerson
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCalend(ProductionCalendar postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            ProductionCalendar updateData = db.ProductionCalendar.First(d => d.id == postData.id);
            if (updateData.timeToOnePerson != postData.timeToOnePerson)
                updateData.timeToOnePerson = postData.timeToOnePerson;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCurencyList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CurencyBYN
                .AsNoTracking()
                .OrderByDescending(d => d.date)
                .Take(120)
                .ToList();
            var data = query.Select(dataList => new
            {
                date = JsonConvert.SerializeObject(dataList.date, settings).Replace(@"""", ""),
                dataList.USD
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult GetSpeedUserList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.DashboardKO_UsersMonthPlan
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.ProductionCalendar)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkProductionCalend(login, dataList.id),
                dataList.ProductionCalendar.period,
                user = dataList.AspNetUsers.CiliricalName,
                coef = dataList.k
            });
            return Json(new { data });
        }

        string GetEditLinkSpeedUser(string login, int id)
        {
            if (login == "myi@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetSpeedUser('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        public JsonResult GetSpeedUser(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.DashboardKO_UsersMonthPlan
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.ProductionCalendar)
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idSpeedUser = dataList.id,
                userNameSpeedUser = dataList.AspNetUsers.CiliricalName,
                periodSpeedUser = dataList.ProductionCalendar.period,
                coefSpeedUser = dataList.k
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSpeedUser(DashboardKO_UsersMonthPlan postData)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            DashboardKO_UsersMonthPlan updateData = db.DashboardKO_UsersMonthPlan.First(d => d.id == postData.id);
            if (updateData.k != postData.k)
                updateData.k = postData.k;
            db.Entry(updateData).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSummaryWageFundWorker()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisWageFund.AsNoTracking().First();
                SummaryWageFund[] summaryWageFund = new SummaryWageFund[3];
                for (int i = 0; i < 3; i++)
                {
                    summaryWageFund[i] = new SummaryWageFund();
                    if (i == 0)
                    {
                        summaryWageFund[i].DevisionId = 0;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.wageFundOverflowsWorkerMPlan + fundData.wageFundOverflowsWorkerEPlan) - Convert.ToInt32(fundData.wageFundOverflowsWorkerMFact + fundData.wageFundOverflowsWorkerEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.wageFundOverflowsWorkerMFact + fundData.wageFundOverflowsWorkerEFact);
                    }
                    else if (i == 1)
                    {
                        summaryWageFund[i].DevisionId = 1;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.wageFundOverflowsWorkerMPlan) - Convert.ToInt32(fundData.wageFundOverflowsWorkerMFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.wageFundOverflowsWorkerMFact);
                    }
                    else
                    {
                        summaryWageFund[i].DevisionId = 2;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.wageFundOverflowsWorkerEPlan) - Convert.ToInt32(fundData.wageFundOverflowsWorkerEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.wageFundOverflowsWorkerEFact);
                    }
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSummaryWageFundManager()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisDeductionsBonusFund.AsNoTracking().First();
                SummaryWageFund[] summaryWageFund = new SummaryWageFund[3];
                for (int i = 0; i < 3; i++)
                {
                    summaryWageFund[i] = new SummaryWageFund();
                    if (i == 0)
                    {
                        summaryWageFund[i].DevisionId = 0;
                        summaryWageFund[i].Plan = (Convert.ToInt32(fundData.deductionsMKOMPlan) + Convert.ToInt32(fundData.deductionsMKOEPlan)) - (Convert.ToInt32(fundData.deductionsMKOMFact) + Convert.ToInt32(fundData.deductionsMKOEFact));
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsMKOMFact) + Convert.ToInt32(fundData.deductionsMKOEFact);
                    }
                    else if (i == 1)
                    {
                        summaryWageFund[i].DevisionId = 1;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.deductionsMMPlan) - Convert.ToInt32(fundData.deductionsMMFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsMMFact);
                    }
                    else
                    {
                        summaryWageFund[i].DevisionId = 2;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.deductionsMEPlan) - Convert.ToInt32(fundData.deductionsMEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsMEFact);
                    }
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSummaryWageFundG()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var fundData = db.CMKO_ThisDeductionsBonusFund.AsNoTracking().First();
                SummaryWageFund[] summaryWageFund = new SummaryWageFund[3];
                for (int i = 0; i < 3; i++)
                {
                    summaryWageFund[i] = new SummaryWageFund();
                    if (i == 0)
                    {
                        summaryWageFund[i].DevisionId = 0;
                        summaryWageFund[i].Plan = (Convert.ToInt32(fundData.deductionsGMPlan) + Convert.ToInt32(fundData.deductionsGEPlan)) - (Convert.ToInt32(fundData.deductionsGMFact) + Convert.ToInt32(fundData.deductionsGEFact));
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsGMFact) + Convert.ToInt32(fundData.deductionsGEFact);
                    }
                    else if (i == 1)
                    {
                        summaryWageFund[i].DevisionId = 1;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.deductionsGMPlan) - Convert.ToInt32(fundData.deductionsGMFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsGMFact);
                    }
                    else
                    {
                        summaryWageFund[i].DevisionId = 2;
                        summaryWageFund[i].Plan = Convert.ToInt32(fundData.deductionsGEPlan) - Convert.ToInt32(fundData.deductionsGEFact);
                        summaryWageFund[i].Fact = Convert.ToInt32(fundData.deductionsGEFact);
                    }
                }
                return Json(summaryWageFund, JsonRequestBehavior.AllowGet);
            }
        }
    }
}