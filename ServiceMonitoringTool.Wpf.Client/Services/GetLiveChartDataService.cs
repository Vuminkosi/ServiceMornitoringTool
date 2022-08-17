 

using ServiceMonitoringTool.Wpf.Client.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceMonitoringTool.Wpf.Client.Services
{
    public class LiveChartDataService
    {
        public LiveChartDataService()
        {
        }

        public async Task<IEnumerable<SeriesModel>> GetLiveChartDataAsync(string requestUrl, ServiceMonitorRequestModel requestModel, CancellationToken cancellationToken = default)
        {
            using (HttpClient httpClient = new HttpClient())
            {

                // Add accepted media type
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUrl)))
                {
                    var _content = Newtonsoft.Json.JsonConvert.SerializeObject(requestModel);
                    request.Content = new StringContent(_content, Encoding.UTF8, "application/json");
                    // get server response
                    using (var response = await httpClient.SendAsync(request, cancellationToken: cancellationToken))
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                response.EnsureSuccessStatusCode();
                                return await GetAPIStreamResponseAsync<IEnumerable<SeriesModel>>(response);
                            
                        
                        }
                         
                    };
                };
                
            }

            return default;
            
        }
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions();
        private async Task<TR> GetAPIStreamResponseAsync<TR>(HttpResponseMessage response)  
        {

            response.EnsureSuccessStatusCode();
            var contentStream = await response.Content.ReadAsStreamAsync(); 
            return await JsonSerializer.DeserializeAsync<TR>(contentStream, Options);
        }
    }
}
