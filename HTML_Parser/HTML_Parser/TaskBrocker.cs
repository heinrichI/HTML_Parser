using HTML_Parser.Data;
using HTML_Parser.DataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTML_Parser
{
    class TaskBrocker
    {
        private object _lock = new object();
        List<Proxy> proxys = null;

        /// <summary>
        /// Время ожидания на время отсутствия ссылок
        /// </summary>
        public int MinutesToWait { get; set; } = 2;

        /// <summary>
        /// Ожидание на случай достижение максимального числа запросов через прокси 
        /// </summary>
        public int ProxyWaitSeconds { get; set; } = 4;

        /// <summary>
        /// Ожидание на случай блокировки прокси в МИНУТАХ
        /// </summary>
        public int ProxyBanWait { get; set; } = 3;


        public void SetProxys(DBComponent component)
        {
            lock (_lock)
            {
                proxys = component.GetActiveProxys();
            }
        }

        public void Start(CancellationToken token, int parallelismDegree = 0)
        {
            DBComponent component = new DBComponent();

            int i = 0;
            HeaderList headerList = new HeaderList();
            SetProxys(component);

            do
            {
                if (component.GetLink() != null)
                {

                    var listProxy = (proxys.Count % 2 ==0) ?  proxys.Take(proxys.Count / 2) : proxys.Take(1);
			
                    Parallel.ForEach(listProxy, new ParallelOptions() { MaxDegreeOfParallelism = parallelismDegree }, async (item) =>
                    {
                        Parser parser = new Parser();
                        ProxyHandler handler = new ProxyHandler();
                        Link link = component.GetLink();

                        string html = handler.GetContent(link.Url, handler.GetProxyConnectionData(item.Url), ref headerList);
                        await component.SetProxyCounterAsync(item, item.RequestCount + 1 , 0);

                        List<FieldSet> data = new List<FieldSet>();
                        data = await parser.ParseDomAsync(html);

                        Console.WriteLine($"Link id {link?.Id}");

                        if (data != null)
                        {
                            if (data.Count > 0)
                            {
                                if (data.First().Blocked == false)
                                {
                                    await AddParsedDataAsync(data, link, component);
                                }
                                else
                                {
                                    await component.SetBlockedStateAsync(item, link);
                                }
                            }
                            else
                            {
                                await component.SetBlockedStateAsync(item, link);
                            }
                        }
                        else
                        {
                            //Задаем значение блокировки
                            // Блокируем прокси 
                            await component.SetBlockedStateAsync(item, link);
                        }

                        //Обнуляем счетчик proxy
                        if (item.RequestCount >= item.MaxRequests)
                        {
							item.IsBanned = false;
                            await component.SetProxyCounterAsync(item, 0, 5);
                        }

                        Console.WriteLine($"Parsed URL {link.Url}");

						
                    });


                }
                else
                {
                    Thread.Sleep(TimeSpan.FromMinutes(2).Milliseconds);
                }
                SetProxys(component);//ищем открытые прокси

            } while (!token.IsCancellationRequested);
        }

        /// <summary>
        /// Добаавляет полученные данные в базу
        /// </summary>
        /// <param name="data">Список данных</param>
        /// <param name="link">Ссылка ресурса</param>
        /// <param name="component">Компонент для работы с базой</param>
        /// <returns></returns>
        public async Task AddParsedDataAsync(List<FieldSet> data, Link link, DBComponent component)
        {
            List<ParsedData> parsedDatas = new List<ParsedData>();
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
                    ShopDirectLink = it.ShopDirectLink,
                    LinkId = link,
                    ParsedTime = DateTime.Now
                });
            }

            link.IsParsed = true;//обновили состояние ссылки
            await component.UpdateLinkAsync(link);
            await component.AddParsedData(parsedDatas);
        }

    }
}
