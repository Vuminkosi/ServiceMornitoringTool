using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Relational
{
    public class ServiceMonitoringContextFactory : IDesignTimeDbContextFactory<ServiceMonitoringContext>
    {
        public ServiceMonitoringContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{envName}.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ServiceMonitoringContext>();
            optionsBuilder.UseSqlServer(configuration["DataConnection:Database"]);

            return new ServiceMonitoringContext(optionsBuilder.Options);
        }
    }
}
