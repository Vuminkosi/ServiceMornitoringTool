using ServiceMornitoringTool.Example.Api.ExternalService;
using ServiceMornitoringTool.Example.Api.ExternalService.Enums;
using ServiceMornitoringTool.Example.Api.ExternalService.Models;

using System.Diagnostics;

namespace ServiceMornitoringTool.Example.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var serviceMonitor = serviceProvider.GetService<IServiceMonitor>();
            var request = await FormatRequest(context.Request);
            var stopWatch = Stopwatch.StartNew();

            try
            {
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next.Invoke(context);
                    var elapsed = stopWatch.Elapsed;
                    var response = await FormatResponse(context.Response);

                    await serviceMonitor.LogServiceMethod(
                        new ServiceMethodEntryExternalModel
                        {
                            ServiceId = configuration["ServiceMonitor:ServiceId"],
                            MethodName = request.Key,
                            RequestUrl = request.Value,
                            Response = response.Key,
                            MethodExecutionTime = DateTime.Now,
                            ElapsedTime = elapsed,
                            ExecutionStatus = response.Value
                        },CancellationToken.None);
                    await responseBody.CopyToAsync(originalBodyStream); 
                }
            }
            catch (Exception ex)
            {
                await serviceMonitor.LogServiceMethod(
                                       new ServiceMethodEntryExternalModel
                                       {
                                           ServiceId = configuration["ServiceMonitor:ServiceId"],
                                           MethodName = request.Key,
                                           RequestUrl = request.Value,
                                           Response = ex.ToString(),
                                           MethodExecutionTime = DateTime.Now,
                                           ElapsedTime = stopWatch.Elapsed,
                                           ExecutionStatus = ExceutionStatus.Ex_Fail
                                       }, CancellationToken.None);

                throw;
            }
        }

        private async Task<KeyValuePair<string, ExceutionStatus>> FormatResponse(HttpResponse response)
        { 
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return new KeyValuePair<string, ExceutionStatus>($"{response.StatusCode}: {text}", (response.StatusCode>= 400 && response.StatusCode<=599)? ExceutionStatus.Ex_Fail: ExceutionStatus.Ex_Suc);
        }

        private async Task<KeyValuePair<string, string>> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0,SeekOrigin.Begin);
            return new KeyValuePair<string, string>(request.Path, $"{request.Scheme}://{request.Host}{request.Path} {request.QueryString} {body}");
        }

       
    }
}
