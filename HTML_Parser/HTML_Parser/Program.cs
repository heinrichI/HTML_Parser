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
		static async Task Main(string[] args)
		{
			System.Management.ManagementObjectSearcher man =
			new System.Management.ManagementObjectSearcher("Select * from Win32_Processor");
			int procLoadPercentage;
			foreach (System.Management.ManagementObject obj in man.Get())
			{
				Console.WriteLine(obj["LoadPercentage"] + "%");
				procLoadPercentage = Convert.ToInt32(obj["LoadPercentage"]);
			}

			System.Diagnostics.Stopwatch sw = new Stopwatch();
			sw.Start();

			string appPath = Directory.GetCurrentDirectory();

			string path = Path.GetFullPath(appPath + "\\Data\\urls.txt");
			FileReader fileReader = new FileReader();
			List<string> urlLines = await fileReader.GetLinesAsync(Path.GetFullPath(appPath + "\\Data\\urls.txt"));
			List<string> proxys = await fileReader.GetLinesAsync(Path.GetFullPath(appPath + "\\Data\\proxy.txt"));
			
			DBComponent component = new DBComponent();
			TaskBrocker brocker = new TaskBrocker();
			CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

			brocker.Start(cancelTokenSource.Token, 30);

		/*	Parser parser = new Parser();
			int i = 0;
			Random random = null;
			HeaderList headerList = new HeaderList();
			Parallel.ForEach(urlLines, new ParallelOptions() { MaxDegreeOfParallelism = 30 }, async (item) =>
			{
				random = new Random();
				int index = random.Next(0, proxys.Count);
				string proxy;
				using (ParserContext context = new ParserContext())
				{
					proxy = (from c in context.Proxies where c.IsBanned == false select c).First().Url;
				}

				ProxyHandler handler = new ProxyHandler();
				string html = handler.GetContent(item, handler.GetProxyConnectionData(proxy), ref headerList);
				List<FieldSet> data = await parser.ParseDomAsync(html);
				List<ParsedData> parsedDatas = new List<ParsedData>();
				if (data!= null)
				{
					foreach (var it in data)
					{
						parsedDatas.Add(new ParsedData()
						{
							Shop = it.Shop,
							ProductName = it.ProductName,
							Price = it.Price,
							ProductDescription = it.ProductDescription,
							DeliveryPice = it.DeliveryPice,
							DeliveryTime = it.DeliveryTime,
							FeedbackQuantity = it.FeedbackQuantity,
							ShopRating = it.ShopRating,
							AdditionalOfferQuantity = it.AdditionalOfferQuantity,
							ShopDirectLink = it.ShopDirectLink
						});
					}
				}
				await component.AddParsedData(parsedDatas);

				Console.WriteLine($"№ {i++} Blocked data count {data?.Where(x => x.Blocked == true).Count()} Parsed {data?.Where(x => x.Blocked = false).Count()} by url {item}");
			});

			*/
			/*foreach (var item in urlLines)
			{
				random = new Random();
				int index = random.Next(0, proxys.Count);

				ProxyHandler handler = new ProxyHandler();
				string html = handler.GetContent(item, handler.GetProxyConnectionData(proxys[6]), ref headerList);//handler.GetContent(item,new ProxyData());//
				List<FieldSet> data = await parser.ParseDomAsync(html);

				Console.WriteLine($"№ {i++} Blocked data count {data?.Where(x => x.Blocked == true).Count()} Parsed {data?.Where(x => x.Blocked == false).Count()} by url {item}");
			}
			*/

			sw.Stop();
			TimeSpan time = TimeSpan.FromMilliseconds(sw.Elapsed.Milliseconds);
			Console.WriteLine($"Elapsed: {time.Hours}: {time.Minutes}:{time.Seconds}: {time.Milliseconds} ");
			Console.ReadKey();
		}
	}
}
