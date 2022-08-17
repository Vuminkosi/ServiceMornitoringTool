using ServiceMornitoringTool.Example.Api.ExternalService.Models;

namespace ServiceMornitoringTool.Example.Api.ExternalService
{
    public class ServiceMonitor: IServiceMonitor
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration configuration;

        public ServiceMonitor(HttpClient httpClient,IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task LogServiceMethod(ServiceMethodEntryExternalModel model, CancellationToken cancellationToken = default)
        {
            _ = await  _httpClient.PostAsJsonAsync<ServiceMethodEntryExternalModel>(configuration["ServiceMonitor:Url"], model, cancellationToken);
             
        }
    }
}
