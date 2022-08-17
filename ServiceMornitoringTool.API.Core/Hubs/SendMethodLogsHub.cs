using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Core.Hubs
{
    public class SendMethodLogsHub : Hub<ISendMethodLogsHub>
    {

        private readonly IHubContext<SendMethodLogsHub> _context;

        public SendMethodLogsHub(IHubContext<SendMethodLogsHub> context)
        {
            _context = context;
        }

        public async Task ChartPointReceived(object data, CancellationToken cancellationToken)
        {
           await _context.Clients.All.SendAsync("ChartPointReceived", data);
        }
    }
}
