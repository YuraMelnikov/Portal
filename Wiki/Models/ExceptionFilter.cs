using NLog;
using System;
using System.Web.Mvc;
using Wiki.Controllers;

namespace Wiki.Models
{
    public class ExceptionFilter : IExceptionFilter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string className;
        private string methodName;
        private Exception exception;

        public ExceptionFilter(string className, string methodName)
        {
            this.className = className;
            this.methodName = methodName;
        }

        public ExceptionFilter(string className, string methodName, Exception exception)
        {
            this.className = className;
            this.methodName = methodName;
            this.exception = exception;
        }

        public void Info()
        {
            logger.Info("Class - " + className + " | Method - " + methodName);
        }

        public void Error()
        {
            logger.Error("Class - " + className + " | Method - " + methodName + " | Exception - " + exception.Message);
        }

        public void Fatal()
        {
            logger.Fatal("Class - " + className + " | Method - " + methodName + " | Exception - " + exception.Message);
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            if (!exceptionContext.ExceptionHandled && exceptionContext.Exception is IndexOutOfRangeException)
            {
                exceptionContext.Result = new RedirectResult("/Exception/Index");
                exceptionContext.ExceptionHandled = true;
            }
        }
    }
}