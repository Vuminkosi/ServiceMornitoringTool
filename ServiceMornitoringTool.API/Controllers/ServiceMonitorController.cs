using Microservice.Framework.Common;
using Microservice.Framework.Domain.Commands;
using Microservice.Framework.Domain.Queries;

using Microsoft.AspNetCore.Mvc;

using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel;
using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.Commands;
using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.Entities;
using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.Queries;
using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.ValueObjects;
using ServiceMornitoringTool.API.Models;

namespace ServiceMornitoringTool.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceMonitorController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public ServiceMonitorController(
            ICommandBus commandBus,
            IQueryProcessor queryProcessor
            )
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        [HttpPost("addservice")]
        public async Task<IActionResult> AddService(CreateServiceMonitorRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var createServiceMonitorResult = await _commandBus
                    .PublishAsync(
                        new CreateServiceMonitorCommand(ServiceMonitorAggregateId.New, model.ServiceName),
                        CancellationToken.None);

                if (createServiceMonitorResult.IsSuccess)
                    return Ok(createServiceMonitorResult);
                else
                    return BadRequest(createServiceMonitorResult);
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }

        [HttpPost("queryservice")]
        public async Task<IActionResult> QueryService(GetServiceMonitorRequestModel model)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _queryProcessor
                    .ProcessAsync(new QueryServiceMonitor(
                        new ServiceMonitorAggregateId(model.Id)),
                        CancellationToken.None));
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }

        [HttpPost("addservicemethodlog")]
        public async Task<IActionResult> AddServiceMethodLog(AddServiceMethodLogRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var addServiceMethodLogResult = await _commandBus.PublishAsync(new AddServiceMethodEntryCommand(
                        new ServiceMonitorAggregateId(model.ServiceId),
                        new ServiceMethod
                        {
                            Id = ServiceMethodId.New,
                            Name = model.MethodName,
                            Request = model.RequestUrl,
                            Response = model.Response,
                            ExecutionTime = model.MethodExecutionTime,
                            TimeElapsed = model.ElapsedTime,
                            ExecutionsStatus = model.ExecutionStatus,
                            ExecutedBy = model.ExecutedBy
                        }), CancellationToken.None);

                if (addServiceMethodLogResult.IsSuccess)
                    return Ok(addServiceMethodLogResult);
                else
                    return BadRequest(addServiceMethodLogResult);
            }
            else
            {
                return BadRequest(ModelState.Values);
            }
        }
    }
}
