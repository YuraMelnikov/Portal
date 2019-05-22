using Wiki.Areas.Reclamation.Models;
using System.Web.Mvc;

namespace Wiki.Areas.Reclamation.Controllers
{
    public class TechnicalAdviceController : Controller
    {
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

        //public JsonResult GetProtocolData(int id)
        //{
        //    return Json(new { data });
        //}

        //public JsonResult GetTA(int id)
        //{
        //    var query = db.Reclamation.Where(d => d.id == id).ToList();
        //    var data = query.Select(dataList => new
        //    {
        //        dataList.id,
        //        dataList.fixedExpert,
        //        dataList.id_Reclamation_Type,
        //        dataList.id_DevisionReclamation,
        //        dataList.id_Reclamation_CountErrorFirst,
        //        dataList.id_Reclamation_CountErrorFinal,
        //        id_AspNetUsersCreate = dataList.AspNetUsers.CiliricalName,
        //        dataList.id_DevisionCreate,
        //        dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settings).Replace(@"""", ""),
        //        dataList.text,
        //        dataList.description,
        //        dataList.timeToSearch,
        //        dataList.timeToEliminate,
        //        dataList.close,
        //        dataList.gip,
        //        dataList.closeDevision,
        //        dataList.PCAM,
        //        dataList.editManufacturing,
        //        dataList.editManufacturingIdDevision,
        //        dataList.id_PF,
        //        dataList.technicalAdvice,
        //        dataList.id_AspNetUsersError,
        //        pZ_PlanZakaz = GetPlanZakazArray(dataList.Reclamation_PZ.ToList()),
        //        answerHistiryText = GetAnswerText(dataList.Reclamation_Answer.ToList())
        //    });
        //    return Json(data.First(), JsonRequestBehavior.AllowGet);
        //}
    }
}