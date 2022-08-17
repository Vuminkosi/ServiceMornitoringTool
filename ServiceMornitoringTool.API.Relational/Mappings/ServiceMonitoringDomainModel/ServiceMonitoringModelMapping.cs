using Microsoft.EntityFrameworkCore;

using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel;
using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.Entities;
using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.ValueObjects;
using ServiceMornitoringTool.API.Relational.ValueObjectConvertors;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Relational.Mappings.ServiceMonitoringDomainModel
{
    public static class ServiceMonitoringModelMapping
    {
        public static ModelBuilder ServiceMonitoringModelMap( this ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<ServiceMethod>()
            .Property(o => o.Id)
            .HasConversion(new SingleValueObjectIdentityValueConverter<ServiceMethodId>());

            modelBuilder
            .Entity<ServiceMethod>()
            .Property<ExecutionsStatusType>(o => o.ExecutionsStatus).HasColumnType("int");

            modelBuilder
            .Entity<ServiceMonitorAggregate>()
            .Property(o => o.Id)
            .HasConversion(new SingleValueObjectIdentityValueConverter<ServiceMonitorAggregateId>());

            modelBuilder
                .Entity<ServiceMethod>()
                .HasOne<ServiceMonitorAggregate>()
                .WithMany(c => c.ServiceMethods);

            return modelBuilder;
        }
    }
}
