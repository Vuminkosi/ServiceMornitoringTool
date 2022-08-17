using ServiceMornitoringTool.Example.Api.ExternalService.Models;

namespace ServiceMornitoringTool.Example.Api.ExternalService
{
    public interface IServiceMonitor
    {
        Task LogServiceMethod(ServiceMethodEntryExternalModel model, CancellationToken cancellationToken = default);
    }
}
