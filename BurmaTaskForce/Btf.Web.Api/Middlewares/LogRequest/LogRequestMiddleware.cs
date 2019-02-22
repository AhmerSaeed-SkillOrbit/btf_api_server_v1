using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;
using Btf.Services.LogService;
using Btf.Utilities.Exceptions;

namespace Btf.Web.Api.LogRequestMiddleware
{
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public LogRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogService logService)
        {
            //Ask Log Service to Start Logging
            logService.StartLog(context.Request.Path);
            try
            {
                //let other components to process the request
                await _next(context);
            }
            catch(BadRequestException ex)
            {
                //context.Response.Clear();
                context.Response.StatusCode = 400;
                await context.WriteResultAsync(new ObjectResult(ex.Message));
                // context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ex.Message;
                // context.Response.Headers.Add(new KeyValuePair<string,StringValues>("Access-Control-Allow-Origin", "*"));
                string trace;
                //turncate trac to 4000 characters
                //as sql server can't store more than 4000 characters in nvarchar field
                if (ex.StackTrace.Length > 4000)
                    trace = ex.StackTrace.Substring(0, 4000);
                else
                    trace = ex.StackTrace;
                //log error 
                logService.LogError(ex.Message, trace);
            }
            catch (System.Exception ex)
            {
                //context.Response.Clear();
                context.Response.StatusCode = 500;
                await context.WriteResultAsync(new ObjectResult("Something went wrong."));
               // context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ex.Message;
               // context.Response.Headers.Add(new KeyValuePair<string,StringValues>("Access-Control-Allow-Origin", "*"));
                string trace;
                //turncate trac to 4000 characters
                //as sql server can't store more than 4000 characters in nvarchar field
                if (ex.StackTrace.Length > 4000)
                    trace = ex.StackTrace.Substring(0, 4000);
                else
                    trace = ex.StackTrace;
                //log error 
                logService.LogError(ex.Message, trace);
            }

            //Ok processing has been done 
            //Ask Log Service to End Logging 
            //And save it in datatbase
            //only if it's not count APIs
            if (context.Request.Path == "/api/requests/messages/unread/count")
                return;
            try
            {
                logService.EndLog(context.Response.StatusCode);
            }
            catch (System.Exception)
            {
                //ToDo: Log to file or server event viewer
            }            
            
        }
    }
}
