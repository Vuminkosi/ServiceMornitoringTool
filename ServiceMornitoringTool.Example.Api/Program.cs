using Polly;
using Polly.Extensions.Http;

using ServiceMornitoringTool.Example.Api.ExternalService;
using ServiceMornitoringTool.Example.Api.Middlewares;

namespace ServiceMornitoringTool.Example.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. 
            builder.Services.AddHttpClient<IServiceMonitor, ServiceMonitor>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration["ServiceMonitor:Host"]);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.MapControllers();

            app.Run();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(1.5, retryAttempt) * 1000), (_, waitingTime) =>
            {
                Console.WriteLine("Retrying due to Polly Retry Policy");
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(3, TimeSpan.FromSeconds(3));
        }
    }
}