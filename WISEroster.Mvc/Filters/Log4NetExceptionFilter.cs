using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using log4net;

namespace WISEroster.Mvc.Filters
{
    public class Log4NetExceptionFilter : IExceptionFilter
    {
        private readonly ILog _log;

        public Log4NetExceptionFilter(ILog log)
        {
            _log = log;
        }
        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;

            _log.Error(ex.Message);
        }

        public bool AllowMultiple { get; }
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}