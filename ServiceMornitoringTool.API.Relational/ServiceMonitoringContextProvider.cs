using Microservice.Framework.Persistence.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Relational
{
    public class ServiceMonitoringContextProvider:IDbContextProvider<ServiceMonitoringContext>,IDisposable
    {
        private readonly DbContextOptions<ServiceMonitoringContext> _options;
        public ServiceMonitoringContextProvider(IConfiguration configuration)
        {
            _options = new DbContextOptionsBuilder<ServiceMonitoringContext>().UseSqlServer(configuration["DataConnection:Database"]).Options;
        }

        public ServiceMonitoringContext CreateContext()
        {
            return new ServiceMonitoringContext(_options);
        }

        public void Dispose()
        {
           
        }
    }
}
