using Microservice.Framework.Common;
using Microservice.Framework.Domain.Events;
using Microservice.Framework.Domain.Subscribers;

using Microsoft.AspNetCore.SignalR;

using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.Events;
using ServiceMornitoringTool.API.Core.Hubs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.Subscribers
{
    public class AddedServiceMethodEntryEventSubscriber :
       ISubscribeSynchronousTo<ServiceMonitorAggregate, ServiceMonitorAggregateId, AddedServiceMethodEntryEvent>
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IHubContext<SendMethodLogsHub> _hubContext;

        public AddedServiceMethodEntryEventSubscriber(
            IHubContext<SendMethodLogsHub> hubContext,
            IJsonSerializer jsonSerializer)
        {
            _hubContext = hubContext;
            _jsonSerializer = jsonSerializer;
        }

        public async Task HandleAsync(
            IDomainEvent<ServiceMonitorAggregate, ServiceMonitorAggregateId, AddedServiceMethodEntryEvent> domainEvent,
            CancellationToken cancellationToken)
        {

            var data = _jsonSerializer.Serialize(domainEvent.AggregateEvent.ServiceMethod);
            await _hubContext.Clients.All.SendAsync("ChartPointReceived", data);
        }
    }
}
