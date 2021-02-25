using HTML_Parser.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser
{
    class FakeComponent : IComponent
    {
        string _appPath;

        public FakeComponent()
        {
            _appPath = Directory.GetCurrentDirectory();
        }

        public Task AddParsedData(List<ParsedData> data)
        {
            throw new NotImplementedException();
        }

        public List<Proxy> GetActiveProxys()
        {
           return new List<Proxy>
           {
               new Proxy
               {
                   Url = "127.0.0.1;12624;wXsqbZEy;qCgivsBQ"
               }
           };
        }

        public Link GetLink()
        {
            string path = Path.GetFullPath(_appPath + "\\Data\\urls.txt");
            FileReader fileReader = new FileReader();
            List<string> urlLines = fileReader.GetLinesAsync(path).Result;//для считывания из фала 
            return urlLines.Select(u => new Link { Url = u }).First();
        }

        public Task SetBlockedStateAsync(Proxy proxy, Link link, int ProxyBanWait = 1)
        {
            throw new NotImplementedException();
        }

        public Task SetProxyCounterAsync(Proxy proxy, int count, int ProxyWaitSeconds = 4)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLinkAsync(Link link)
        {
            throw new NotImplementedException();
        }
    }
}
