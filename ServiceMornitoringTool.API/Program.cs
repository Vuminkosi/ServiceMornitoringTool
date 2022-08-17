using Microservice.Framework.Domain;
using ServiceMornitoringTool.API.Relational;
using Microservice.Framework.Persistence.EFCore;
using ServiceMornitoringTool.API.Relational.Extensions;

using ServiceMornitoringTool.API.Core;
using Microservice.Framework.Domain.Extensions;
using Microservice.Framework.Persistence;
using ServiceMornitoringTool.API.Core.Hubs;

namespace ServiceMornitoringTool.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "enableCors",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddLogging(I => I.AddConsole());

            DomainContainer.New(builder.Services)
                .ConfigureServiceMonitoringDomain()
                .ConfigureEntityFramework(EntityFrameworkConfiguration.New)
                .AddDbContextProvider<ServiceMonitoringContext, ServiceMonitoringContextProvider>()
                .RegisterServices(sr =>
                {
                    sr.AddTransient<IPersistenceFactory, EntityFrameworkPersistenceFactory<ServiceMonitoringContext>>();
                    sr.AddSingleton(rctx => { return builder.Configuration; });
                });

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


            app.MapControllers();
            app.MapHub<SendMethodLogsHub>("/MethodLogServiceHub");
            app.Run();
        }
    }
}