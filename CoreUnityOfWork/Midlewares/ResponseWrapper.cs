using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUnityOfWork.Midlewares
{
    public class ResponseWrapper
    {
        private readonly RequestDelegate _next;

        public ResponseWrapper(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var currentBody = context.Response.Body;

                using (var memoryStream = new MemoryStream())
                {
                    //set the current response to the memorystream.
                    context.Response.Body = memoryStream;

                    await _next(context);

                    //reset the body 
                    context.Response.Body = currentBody;
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var readToEnd = new StreamReader(memoryStream).ReadToEnd();
                    var objResult = JsonConvert.DeserializeObject(readToEnd);
                    bool error = false;
                    string msgError = string.Empty;


                    JToken t = JToken.FromObject(objResult);

                    // Wrap Content
                    JObject main = new JObject();
                    JObject response = new JObject();
                    JObject esito = new JObject();
                    esito.Add("codice", (error ? 500 : 0));
                    esito.Add("descrizione", (error ? msgError : "OK"));
                    response.Add("esito", esito);

                    response.Add("Content", (error ? null : t));
                    main.Add("response", response);
                    // var result = CommonApiResponse.Create((HttpStatusCode)context.Response.StatusCode, objResult, );
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(main));
                }
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }

        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            // Wrap Content
            JObject main = new JObject();
            JObject response = new JObject();
            JObject esito = new JObject();
            esito.Add("codice", ("500 "));
            esito.Add("descrizione", ("Error"));
            response.Add("esito", esito);

            response.Add("Content", (ex.Message));
            main.Add("response", response);
            // var result = CommonApiResponse.Create((HttpStatusCode)context.Response.StatusCode, objResult, );
            await context.Response.WriteAsync(JsonConvert.SerializeObject(main));
        }

    }

    public static class ResponseWrapperExtensions
    {
        public static IApplicationBuilder UseResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseWrapper>();
        }
    }
}
