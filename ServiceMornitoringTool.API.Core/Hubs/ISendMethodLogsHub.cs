using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Core.Hubs
{
    public interface ISendMethodLogsHub
    {
        Task SendMethodLog(CancellationToken cancellationToken);
    }
}
