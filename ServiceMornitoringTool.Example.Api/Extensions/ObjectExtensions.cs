using Newtonsoft.Json;

namespace ServiceMornitoringTool.Example.Api.Extensions
{
    public static class ObjectExtensions
    {
        public static HttpContent SerializeToJson(this object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value","Cannot Serialize a null object");
            }

            var json = JsonConvert.SerializeObject(value);
            var responseStringContent = new StringContent(json);
            responseStringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return responseStringContent;

        }
    }
}
