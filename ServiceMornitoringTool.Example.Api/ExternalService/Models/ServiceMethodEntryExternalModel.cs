using ServiceMornitoringTool.Example.Api.ExternalService.Enums;

namespace ServiceMornitoringTool.Example.Api.ExternalService.Models
{
    public class ServiceMethodEntryExternalModel
    {
        public string ServiceId { get; set; }
        public string MethodName { get; set; }
        public string RequestUrl { get; set; }
        public string Response { get; set; }
        public DateTime MethodExecutionTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public ExceutionStatus ExecutionStatus { get; set; }
        public string ExecutedBy { get; set; }
    }

   

}
