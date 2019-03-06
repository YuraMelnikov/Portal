using System.Web.Mvc;

namespace Wiki.Models
{
    interface IExceptionFilter
    {
        void OnException(ExceptionContext filterContext);
    }
}
