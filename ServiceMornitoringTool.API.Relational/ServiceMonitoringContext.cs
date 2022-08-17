using Microsoft.EntityFrameworkCore;

using ServiceMornitoringTool.API.Relational.Mappings.ServiceMonitoringDomainModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Relational
{
    public class ServiceMonitoringContext : DbContext
    {
        public ServiceMonitoringContext(DbContextOptions<ServiceMonitoringContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ServiceMonitoringModelMap();
        }
    }
}
