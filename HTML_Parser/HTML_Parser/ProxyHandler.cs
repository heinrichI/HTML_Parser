using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading;

namespace HTML_Parser.Data
{
	public class ProxyHandler
	{

		private delegate void Add(HeaderList list);
		private event Add AddHeader;
		private string _appPath = Directory.GetCurrentDirectory();
		private string _cookieStore = "\\Data\\cookies.json";

		public ProxyHandler()
		{
			AddHeader = new Add(OnHeaderAdd);
		}

		/// <summary>
		/// Считываем данные с proxy
		/// </summary>
		/// <param name="url"></param>
		/// <param name="proxy"></param>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <returns>html</returns>
		public string GetContent(string url, ProxyData proxyData, ref HeaderList headerList)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				Uri uri = new UriBuilder("http", proxyData.Host, Convert.ToInt32(proxyData.Port)).Uri;
				WebProxy proxy = new WebProxy(uri, true);
				CredentialCache credentialCache = new CredentialCache();

				credentialCache.Add(uri, "Basic", new NetworkCredential(proxyData.Login, proxyData.Password));
				proxy.Credentials = credentialCache;

				request.Proxy = proxy;
				WebHeaderCollection collection = headerList[proxyData] ?? new WebHeaderCollection();

				request.ContentType = "text/html; charset=utf-8";
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36";
				collection["Content-Type"] = "application / json";
				request.CookieContainer = headerList.GetCookie(proxyData);
				HeaderList list = GetSavedCookies();
				headerList = list ?? headerList;

				HttpWebResponse response = null;
				try
				{
					response = (HttpWebResponse)request.GetResponse();
					var uriResponse = response.ResponseUri;
			//		Console.WriteLine(uriResponse);
					if (headerList?[proxyData] == null)
					{
						headerList.AddWebHeader(proxyData, response.Headers);
						headerList.AddCookie(proxyData, response.Cookies);
						AddHeader.Invoke(headerList);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				string html = null;

				Thread.Sleep(5000);
				if (response != null && response?.StatusCode == System.Net.HttpStatusCode.OK)
				{
					try
					{
						using (Stream stream = response.GetResponseStream())
						{
							using (StreamReader reader = new StreamReader(stream))
							{
								string str = null;
								while ((str = reader.ReadLine()) != null)
								{
									html += str;
								}
							}
						}
					}
					catch (Exception e)
					{
						Console.Write(e.Message);
					}

					var resUri = response.ResponseUri;
					response.Close();
				}
				else
				{
					Console.WriteLine($"Failed to get page with URL {url}");
				}
				return html;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return null;
		}

		/// <summary>
		/// Парсим строку подключения proxy
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public ProxyData GetProxyConnectionData(string line)
		{
			ProxyData proxyData = new ProxyData();
			string[] data = line.Split('@');

			proxyData.Host = data[0].Split(':')[0];
			proxyData.Port = data[0].Split(':')[1];
			proxyData.Login = data[1].Split(':')[0];
			proxyData.Password = data[1].Split(':')[1];

			return proxyData;
		}

		/// <summary>
		/// Сериализуем объект куки
		/// </summary>
		/// <param name="list"></param>
		private void OnHeaderAdd(HeaderList list)
		{
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(HeaderList));
			using (StreamWriter stream = new StreamWriter(Path.GetFullPath(_appPath + _cookieStore)))
			{
				var obj = JsonConvert.SerializeObject(list);

				stream.Write(obj);
			}
		}

		/// <summary>
		/// Получить сохраненные куки
		/// </summary>
		/// <returns></returns>
		public HeaderList GetSavedCookies()
		{
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(HeaderList));
			HeaderList list = null;

			using (StreamReader stream = new StreamReader(Path.GetFullPath(_appPath + _cookieStore)))
			{
				try
				{
					string json = "";
					string line = null;
					while ((line = stream.ReadLine()) != null)
					{
						json += line;
					}
					list = JsonConvert.DeserializeObject<HeaderList>(json);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
			return list;
		}
	}
}
