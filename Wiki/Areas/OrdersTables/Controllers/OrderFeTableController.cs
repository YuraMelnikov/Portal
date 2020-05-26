using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Wiki.Areas.OrdersTables.Controllers
{
    public class OrderFeTableController : Controller
    {
        PortalKATEKEntities dbc = new PortalKATEKEntities();
        JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        public ActionResult Index()
        {
            ViewBag.Orders = new SelectList(dbc.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderByDescending(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
        }

        [HttpPost]
        public JsonResult ListActive()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.OrdersTables
                    .AsNoTracking()
                    .Where(d => d.dateClose == null)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = GetEditLink(dataList.id, login),
                    dataList.id,
                    orderNum = GetOrdersNum(dataList.id),
                    user = GetUserName(login),
                    dateCreate = JsonConvert.SerializeObject(dataList.dateCreate, settings).Replace(@"""", ""),
                    dateClose = JsonConvert.SerializeObject(dataList.dateClose, settings).Replace(@"""", ""),
                    dateRemove = JsonConvert.SerializeObject(dataList.dateRemove, settings).Replace(@"""", ""),
                    state = GetState(dataList),
                    removeLink = GetRemoveLink(dataList.id, login)
                });
                return Json(new { data });
            }
        }

        [HttpPost]
        public JsonResult ListClose()
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.OrdersTables
                    .AsNoTracking()
                    .Where(d => d.dateClose != null)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = GetEditLink(dataList.id, login),
                    dataList.id,
                    orderNum = GetOrdersNum(dataList.id),
                    user = GetUserName(login),
                    dateCreate = JsonConvert.SerializeObject(dataList.dateCreate, settings).Replace(@"""", ""),
                    dateClose = JsonConvert.SerializeObject(dataList.dateClose, settings).Replace(@"""", ""),
                    dateRemove = JsonConvert.SerializeObject(dataList.dateRemove, settings).Replace(@"""", ""),
                    state = GetState(dataList),
                    removeLink = GetRemoveLink(dataList.id, login)
                });
                return Json(new { data });
            }
        }

        private string GetRemoveLink(int id, string login)
        {
            if (login == "myi@katek.by")
            {
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return Remove('" + id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>";
            }
            else
            {
                return "";
            }
        }

        private string GetState(Wiki.OrdersTables order)
        {
            if (order.dateRemove != null)
                return "Отменен";
            else if (order.dateClose != null)
                return "Поступил";
            else
                return "Ожидание";
        }

        private string GetEditLink(int id, string login)
        {
            if(login == "myi@katek.by")
            {
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return Get('" + id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            }
            else
            {
                return "";
            }
        }

        private string GetOrdersNum(int id)
        {
            string order = "";
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var orderList = db.OrdersTablesPositions
                    .AsNoTracking()
                    .Include(a => a.PZ_PlanZakaz)
                    .Where(a => a.id_OrdersTables == id)
                    .ToList();
                foreach (var data in orderList)
                {
                    order += data.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
                }
                return order;
            }
        }

        private string GetUserName(string login)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                return db.AspNetUsers.AsNoTracking().First(a => a.Email == login).CiliricalName;
            }
        }

        public JsonResult Add(int[] ordersList)
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                Wiki.OrdersTables order = new Wiki.OrdersTables
                {
                    dateCreate = DateTime.Now,
                    dateClose = null,
                    dateRemove = null,
                    isActive = true,
                    id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id
                };
                db.OrdersTables.Add(order);
                db.SaveChanges();
                foreach (var data in ordersList)
                {
                    OrdersTablesPositions pos = new OrdersTablesPositions();
                    pos.id_PZ_PlanZakaz = data;
                    pos.id_OrdersTables = order.id;
                    pos.isActive = true;
                    db.OrdersTablesPositions.Add(pos);
                    db.SaveChanges();
                }
                string directory = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Таблички\" + order.id.ToString() + "\\";
                Directory.CreateDirectory(directory);
                DirectoryInfo dr = new DirectoryInfo(@"\\192.168.1.30\m$\_ЗАКАЗЫ\Таблички\BU\");
                foreach (FileInfo fi in dr.GetFiles("*.cdr"))
                {
                    fi.CopyTo(directory + fi.Name, true);
                }
                string[] body = GetFileBodyCRD(ordersList, directory);
                System.IO.File.WriteAllLines(directory + "RecordedMacros.bas", body, Encoding.Unicode);
                body = GetArrayFileBodyCRD(ordersList);
                System.IO.File.WriteAllLines(directory + "RecordedMacros.txt", body, Encoding.Unicode);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        private string[] GetArrayFileBodyCRD(int[] Id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                List<string> dataList = new List<string>();
                foreach (var data in Id)
                {
                    var order = db.PZ_PlanZakaz.AsNoTracking().Include(a => a.PZ_ProductType).First(a => a.Id == data);
                    if (order.massa < 1000)
                    {
                        dataList.Add("          кг.");
                    }
                    else
                    {
                        dataList.Add(order.massa.ToString());
                    }
                    dataList.Add(order.Name);
                    dataList.Add(order.nameTU);
                    dataList.Add(order.PlanZakaz.ToString());
                    dataList.Add(order.PZ_ProductType.tu);
                    dataList.Add(DateTime.Now.Year.ToString());
                }
                return dataList.ToArray();
            }
        }

        private string[] GetFileBodyCRD(int[] Id, string directory)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                List<string> dataList = new List<string>();
                dataList.Add("Sub Macro1()");
                dataList.Add("Dim counterStep As Integer");
                dataList.Add("Dim sizeArray As Integer");
                dataList.Add("sizeArray = " + Id.Count().ToString());
                dataList.Add("Dim cardArray(" + (Id.Count() - 1).ToString() + ") As CardClass");
                dataList.Add("counterStep = 0");
                int counterStep = 0;
                foreach (var data in Id)
                {
                    var order = db.PZ_PlanZakaz.AsNoTracking().Include(a => a.PZ_ProductType).First(a => a.Id == data);
                    dataList.Add("Set cardArray(" + counterStep.ToString() + ") = New CardClass");
                    if (order.massa < 1000)
                    {
                        dataList.Add("cardArray(" + counterStep.ToString() + ").Weight = " + @"""" + "          кг." + @"""");
                    }
                    else
                    {
                        dataList.Add("cardArray(" + counterStep.ToString() + ").Weight = " + @"""" + order.massa.ToString() + " кг." + @"""");
                    }
                    dataList.Add("cardArray(" + counterStep.ToString() + ").name = " + @"""" + order.Name.Replace(@"""", " + Chr (34) + ").Replace(@"«", " + Chr (34) + ").Replace(@"»", " + Chr (34) + ") + @"""");
                    dataList.Add("cardArray(" + counterStep.ToString() + ").NameT = " + @"""" + order.nameTU.Replace(@"""", " + Chr (34) + ").Replace(@"«", " + Chr (34) + ").Replace(@"»", " + Chr (34) + ") + @"""");
                    dataList.Add("cardArray(" + counterStep.ToString() + ").Num = " + @"""" + order.PlanZakaz.ToString() + @"""");
                    dataList.Add("cardArray(" + counterStep.ToString() + ").TU = " + @"""" + order.PZ_ProductType.tu + @"""");
                    dataList.Add("cardArray(" + counterStep.ToString() + ").Year = " + @"""" + DateTime.Now.Year.ToString() + @"""");
                    counterStep++;
                }
                dataList.Add("For fcounrer = 0 To sizeArray -1");
                dataList.Add("UpdateCard counterStep, cardArray(fcounrer), sizeArray");
                dataList.Add("UpdateCardName");
                dataList.Add("counterStep = counterStep + 1");
                dataList.Add("Next fcounrer");
                dataList.Add("Save");
                dataList.Add("End Sub");
                dataList.Add("Sub UpdateCard(counterStep1 As Integer, tcard As CardClass, sa As Integer)");
                dataList.Add("Dim strCardName As String");
                dataList.Add("Dim position As Integer");
                dataList.Add("If counterStep1<> 0 Then");
                dataList.Add("position = 3 * counterStep1 + 3");
                dataList.Add("End If");
                dataList.Add("If counterStep1 = 0 Then");
                dataList.Add("position = 3");
                dataList.Add("End If");
                dataList.Add("strCardName = counterStep1 + 1");
                dataList.Add("For Each card In ActiveDocument.ActivePage.Shapes");
                dataList.Add("If card.TreeNode.Name = Chr(34) + Card + Chr(34) + strCardName Then");
                dataList.Add("For Each child In card.TreeNode.Children");
                dataList.Add("If child.Name = Chr(34) + Name + Chr(34) Then");
                dataList.Add("child.Shape.Text.Frame.Range.Text = tcard.Name");
                dataList.Add("End If");
                dataList.Add("If child.Name = Chr(34) + NameT + Chr(34) Then");
                dataList.Add("child.Shape.Text.Frame.Range.Text = Chr(34) + Chr(34)");
                dataList.Add("child.Shape.Text.Frame.Range.Text = Replace(child.Shape.Text.Frame.Range.Text, Chr(34) + Chr(32) + Chr(34), Chr(34) + Chr(34))");
                dataList.Add("child.Shape.Text.Frame.Range.Text = tcard.NameT");
                dataList.Add("End If");
                dataList.Add("If child.Name = Chr(34) + Num + Chr(34) Then");
                dataList.Add("child.Shape.Text.Frame.Range.Text = tcard.Num");
                dataList.Add("End If");
                dataList.Add("If child.Name = Chr(34) + TU + Chr(34) Then");
                dataList.Add("child.Shape.Text.Frame.Range.Text = tcard.TU");
                dataList.Add("End If");
                dataList.Add("If child.Name = Chr(34) + Year + Chr(34) Then");
                dataList.Add("child.Shape.Text.Frame.Range.Text = tcard.Year");
                dataList.Add("End If");
                dataList.Add("If child.Name = Chr(34) + Weight + Chr(34) Then");
                dataList.Add("child.Shape.Text.Frame.Range.Text = tcard.Weight");
                dataList.Add("End If");
                dataList.Add("Next child");
                dataList.Add("If sa -1 <> counterStep1 Then");
                dataList.Add("card.Duplicate position, 0");
                dataList.Add("End If");
                dataList.Add("End If");
                dataList.Add("Next card");
                dataList.Add("End Sub");
                dataList.Add("Sub UpdateCardName()");
                dataList.Add("Dim step As Integer");
                dataList.Add("Dim Name As String");
                dataList.Add("step = 1");
                dataList.Add("For Each card In ActiveDocument.ActivePage.Shapes");
                dataList.Add("Name = Chr(34) + Card + Chr(34) + Str(step)");
                dataList.Add("card.Name = Replace(Name, Chr(34) + Chr(32) + Chr(34), Chr(34) + Chr(34))");
                dataList.Add("step = step + 1");
                dataList.Add("Next card");
                dataList.Add("End Sub");
                dataList.Add("Sub Save()");
                dataList.Add("Dim Path As String");
                dataList.Add("Dim Name As String");
                dataList.Add("Path = Chr(34) + " + directory + " + Chr(34)");
                dataList.Add("Name = Chr(34) + RecordedMacros.cdr + Chr(34)");
                dataList.Add("Dim SaveOptions As StructSaveAsOptions");
                dataList.Add("Set SaveOptions = CreateStructSaveAsOptions");
                dataList.Add("With SaveOptions");
                dataList.Add(".EmbedVBAProject = True");
                dataList.Add(".Filter = cdrCDR");
                dataList.Add(".IncludeCMXData = False");
                dataList.Add(".Range = cdrAllPages");
                dataList.Add(".EmbedICCProfile = False");
                dataList.Add(".Version = cdrVersion15");
                dataList.Add("End With");
                dataList.Add("ActiveDocument.SaveAs Path + Chr(34) + Chr(47) + Chr(34) + Name, SaveOptions");
                dataList.Add("End Sub");
                return dataList.ToArray();
            }
        }

        public JsonResult Get(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.OrdersTables
                    .Include(d => d.OrdersTablesPositions)
                    .Where(d => d.id == id)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    id_OrdersTables = dataList.id,
                    ordersList = GetOrdersArray(dataList.OrdersTablesPositions.ToList())
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        private string[] GetOrdersArray(List<OrdersTablesPositions> list)
        {
            string[] orders = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                orders[i] = list[i].id_PZ_PlanZakaz.ToString();
            }
            return orders;
        }

        public JsonResult Remove(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var order = db.OrdersTables.Find(id);
                order.isActive = false;
                order.dateClose = DateTime.Now;
                order.dateRemove = DateTime.Now;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Update(int id_OrdersTables, int[] ordersList)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var posList = db.OrdersTablesPositions.Where(d => d.id_OrdersTables == id_OrdersTables).ToList();
                foreach (var pos in posList)
                {
                    if (ordersList.Where(d => d == pos.id_PZ_PlanZakaz).Count() == 0)
                    {
                        db.OrdersTablesPositions.Remove(pos);
                        db.SaveChanges();
                    }
                }
                posList = db.OrdersTablesPositions.Where(d => d.id_OrdersTables == id_OrdersTables).ToList();
                foreach (var ord in ordersList)
                {
                    if (posList.Where(d => d.id_PZ_PlanZakaz == ord).Count() == 0)
                    {
                        OrdersTablesPositions order = new OrdersTablesPositions
                        {
                            isActive = true,
                            id_PZ_PlanZakaz = ord,
                            id_OrdersTables = id_OrdersTables
                        };
                        db.OrdersTablesPositions.Add(order);
                        db.SaveChanges();
                    }
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}