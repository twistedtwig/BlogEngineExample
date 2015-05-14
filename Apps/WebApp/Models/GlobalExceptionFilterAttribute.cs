using System;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class GlobalExceptionFilterAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;

            while (ex != null)
            {
                Write(ex);
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                else
                {
                    ex = null;
                }
            }
        }

        private void Write(Exception ex)
        {
            System.IO.File.AppendAllText(@"C:\temp\blogExample.log", ex.Message);
            System.IO.File.AppendAllText(@"C:\temp\blogExample.log", ex.StackTrace);
            System.IO.File.AppendAllText(@"C:\temp\blogExample.log", "");
        }
    }
}