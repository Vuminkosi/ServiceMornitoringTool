using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitoringTool.Wpf.Client.Models
{
    public class ServiceMonitorRequestModel
    {
        public string Id { get; set; }
    }

    public class SeriesModel
    {
        public string Name { get; set; }

        public IEnumerable<KeyValuePair<DateTime, double>> Data { get; set; }
    }

    public enum ExecutionsStatusType
    {
        FailedExecution,
        ExecutedSuccessfully
    }
}
