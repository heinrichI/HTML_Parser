using HTML_Parser.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser
{
    interface IComponent
    {
        List<Proxy> GetActiveProxys();

        Link GetLink();

        Task SetProxyCounterAsync(Proxy proxy, int count, int ProxyWaitSeconds = 4);

        Task SetBlockedStateAsync(Proxy proxy, Link link, int ProxyBanWait = 1);

        Task AddParsedData(List<ParsedData> data);

        Task UpdateLinkAsync(Link link);
    }
}
