﻿using HTML_Parser.Data;
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
        public TaskBrocker(IComponent component)
        {
            this._component = component;
        }

        private object _lock = new object();
        List<Proxy> proxys = null;
        private readonly IComponent _component;

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


        public void SetProxys(IComponent component)
        {
            lock (_lock)
            {
                proxys = component.GetActiveProxys();
            }
        }

        public void Start(CancellationToken token, int parallelismDegree = 0)
        {
            int i = 0;
            HeaderList headerList = new HeaderList();
            SetProxys(_component);

            do
            {
                if (_component.GetLink() != null)
                {
                    var listProxy = (proxys.Count > 10 ) ?  proxys.Take(proxys.Count / 2) : proxys.Take(proxys.Count);
                    Parallel.ForEach(listProxy, new ParallelOptions() { MaxDegreeOfParallelism = parallelismDegree }, async (item) =>
                    {
                        Parser parser = new Parser();
                        ProxyHandler handler = new ProxyHandler();
                        Link link = _component.GetLink();

                        string html = handler.GetContent(link.Url, handler.GetProxyConnectionData(item.Url), ref headerList);
                        await _component.SetProxyCounterAsync(item, item.RequestCount + 1 , 0);

                        List<FieldSet> data = new List<FieldSet>();
                        data = await parser.ParseDomAsync(html);

                        Console.WriteLine($"Link id {link?.Id}   Processed at  {DateTime.Now}");

                        if (data != null)
                        {
                            if (data.Count > 0)
                            {
                                if (data.First().Blocked == false)
                                {
                                    await AddParsedDataAsync(data, link);
                                }
                                else
                                {
                                    await _component.SetBlockedStateAsync(item, link,1);
                                }
                            }
                            else
                            {
                                await _component.SetBlockedStateAsync(item, link,1);
                            }
                        }
                        else
                        {
                            //Задаем значение блокировки
                            // Блокируем прокси 
                            await _component.SetBlockedStateAsync(item, link,1);
                        }

                        //Обнуляем счетчик proxy
                        if (item.RequestCount >= item.MaxRequests)
                        {
                            item.IsBanned = false;
                            await _component.SetProxyCounterAsync(item, 0, 3);
                        }

                       // Console.WriteLine($"Parsed URL {link.Url}");
                    });
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromMinutes(2).Milliseconds);
                }
                SetProxys(_component);//ищем открытые прокси

            } while (!token.IsCancellationRequested);
        }

        /// <summary>
        /// Добаавляет полученные данные в базу
        /// </summary>
        /// <param name="data">Список данных</param>
        /// <param name="link">Ссылка ресурса</param>
        /// <param name="component">Компонент для работы с базой</param>
        /// <returns></returns>
        public async Task AddParsedDataAsync(List<FieldSet> data, Link link)
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
                    LinkId = link.Id,
                    ParsedTime = DateTime.Now
                });
            }

            link.IsParsed = true;//обновили состояние ссылки
            await _component.UpdateLinkAsync(link);
            await _component.AddParsedData(parsedDatas);
        }

    }
}
