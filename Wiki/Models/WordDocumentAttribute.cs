using System.Web.Mvc;

namespace Wiki.Models
{
    public class WordDocumentAttribute : ActionFilterAttribute
    {
        public string DefaultFilename { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResult;

            if (result != null)
                result.MasterName = "~/Views/Shared/_LayoutWord.cshtml";

            filterContext.Controller.ViewBag.WordDocumentMode = true;

            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var filename = filterContext.Controller.ViewBag.WordDocumentFilename;
            filename = filename ?? DefaultFilename ?? "Document";

            filterContext.HttpContext.Response.AppendHeader("Content-Disposition", string.Format("filename={0}.doc", filename));
            filterContext.HttpContext.Response.ContentType = "application/msword";

            base.OnResultExecuted(filterContext);
        }
    }
}