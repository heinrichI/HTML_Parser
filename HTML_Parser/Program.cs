using HTML_Parser.Data;
using HTML_Parser.DataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace HTML_Parser
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string appPath = Directory.GetCurrentDirectory();

            string path = Path.GetFullPath(appPath + "\\Data\\urls.txt");
            FileReader fileReader = new FileReader();
          //  List<string> urlLines = await fileReader.GetLinesAsync(Path.GetFullPath(appPath + "\\Data\\urls.txt"));//для считывания из фала 
            List<string> proxys = await fileReader.GetLinesAsync(Path.GetFullPath(appPath + "\\Data\\proxy.txt"));// для считывания из файла

            DBComponent component = new DBComponent();
            TaskBrocker brocker = new TaskBrocker();
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();


            List<Link> linkScope = new List<Link>();

           /* foreach (var item in urlLines)
            {
                linkScope.Add(new Link()
                {
                    IsParsed = false,
                    Url = item,
                    ProductId = -1,
                    AddingDate = DateTime.Now
                });
            }*/
           // await component.AddLinksScopeAsync(linkScope);


            /*foreach (var item in proxys)
            {
                await component.AddProxyAsync(new Proxy()
                {
                    IsBanned = false,
                    Url = item,
                    WaitTo = DateTime.Now,
                });
            }*/


            brocker.Start(cancelTokenSource.Token, 50);

            Console.ReadKey();
        }

        /// <summary>
        /// Проверка загрузки процессора
        /// </summary>
        public void GetProcessDiagnostics()
        {
            System.Management.ManagementObjectSearcher man = new System.Management.ManagementObjectSearcher("Select * from Win32_Processor");
            int procLoadPercentage;
            foreach (System.Management.ManagementObject obj in man.Get())
            {
                Console.WriteLine(obj["LoadPercentage"] + "%");
                procLoadPercentage = Convert.ToInt32(obj["LoadPercentage"]);
            }
        }

    }
}
