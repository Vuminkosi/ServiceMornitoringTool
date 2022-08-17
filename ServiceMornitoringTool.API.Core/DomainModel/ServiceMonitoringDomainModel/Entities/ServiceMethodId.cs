using Microservice.Framework.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.Entities
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ServiceMethodId : Identity<ServiceMethodId>
    {
        public ServiceMethodId(string value)
            : base(value)
        {

        }
    }
}
