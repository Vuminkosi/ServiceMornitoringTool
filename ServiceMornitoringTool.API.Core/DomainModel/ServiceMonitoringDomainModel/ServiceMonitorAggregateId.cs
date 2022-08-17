using Microservice.Framework.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ServiceMonitorAggregateId : Identity<ServiceMonitorAggregateId>
    {
        public ServiceMonitorAggregateId(string value) : base(value)
        {
        }
    }
}
