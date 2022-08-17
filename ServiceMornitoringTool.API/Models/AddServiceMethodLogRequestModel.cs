using ServiceMornitoringTool.API.Core.DomainModel.ServiceMonitoringDomainModel.ValueObjects;

namespace ServiceMornitoringTool.API.Models
{
    public class AddServiceMethodLogRequestModel
    {
        public string ServiceId { get; set; }
        public string MethodName { get; set; }
        public string RequestUrl { get; set; }
        public string Response { get; set; }
        public DateTime MethodExecutionTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public ExecutionsStatusType ExecutionStatus { get; set; }
        public string ExecutedBy { get; set; } 
    }
}
