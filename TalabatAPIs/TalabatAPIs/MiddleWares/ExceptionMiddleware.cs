using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TalabatAPIs.Errors;

namespace TalabatAPIs.MiddleWares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> loogre;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> loogre, IHostEnvironment env)
        {
            this.next = next;
            this.loogre = loogre;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext contex)
        {
            try
            {
                await next.Invoke(contex);
            }
            catch (Exception ex)
            {
                loogre.LogError(ex , ex.Message );
     
                contex.Response.ContentType = "application/json";
                contex.Response.StatusCode =(int) HttpStatusCode.InternalServerError;
                
                var exceptionErrorResponse = env.IsDevelopment() ?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    :
                    new ApiExceptionResponse((int) HttpStatusCode.InternalServerError,ex.Message, ex.StackTrace.ToString());

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


                var json = JsonSerializer.Serialize(exceptionErrorResponse, options);
                
                await contex.Response.WriteAsync(json);
            }
        }
    }
}
