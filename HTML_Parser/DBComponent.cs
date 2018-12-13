using HTML_Parser.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;
using System.Data.SqlClient;

namespace HTML_Parser
{
	public class DBComponent
	{
		object _locker = new object();
        ConcurrentQueue<Link> linksQueue = new ConcurrentQueue<Link>();

        /// <summary>
        /// Добавить новую ссылку
        /// </summary>
        /// <param name="link"></param>
		public void AddNewLink(Link link)
		{
			try
			{
				using (ParserContext context = new ParserContext())
				{
					context.Links.Add(link);
					context.SaveChangesAsync();
				}
			}
			catch(SqlException e)
			{
				Console.WriteLine(e.Message);
			}
		}
        /// <summary>
        /// Добавить массив ссылок 
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        public async Task AddLinksScopeAsync(List<Link> links)
        {
            using (ParserContext context = new ParserContext())
            {
                foreach (var item in links)
                {
                    context.Links.Add(item);
                }
               await context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// Получить прокси
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
		public async Task AddProxyAsync(Proxy proxy)
		{
			int number = 0;
			try
			{
				using (ParserContext context = new ParserContext())
				{
					context.Proxies.Add(proxy);
					number = await context.SaveChangesAsync();
				}
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.Message);
			}
		}

        /// <summary>
        /// Добавить данные парсинга
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public async Task AddParsedData(List<ParsedData> data)
		{
			int number = 0;
			try
			{
				using (ParserContext context = new ParserContext())
				{
					foreach (var item in data)
					{
						context.ParsedData.Add(item);
					}					
					number = await context.SaveChangesAsync();
				}
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.Message);
			}
		}

        /// <summary>
        /// Получить ссылку
        /// </summary>
        /// <returns></returns>
		public Link GetLink()
		{
			lock (_locker)
			{
                Link link = null;
                try
                {
                    using (ParserContext context = new ParserContext())
                    {
                        var links = (from c in context.Links where c.IsParsed == false select c).ToList();

                        if (linksQueue.IsEmpty)
                        {
                            foreach (var i in links)
                            {
                                linksQueue.Enqueue(i);
                            }
                        }

                        
                        linksQueue.TryDequeue(out link);

                        return link;
                    }
                }catch(SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
                return link;
            }            
        }

        /// <summary>
        /// Получить список активных прокси
        /// </summary>
        /// <returns></returns>
		public List<Proxy> GetActiveProxys()
		{
            List<Proxy> proxys = null;
            try
            {           
                using (ParserContext context = new ParserContext())
                {
                    var timeReq = (from c in context.Proxies select c).ToList();

                    foreach (var item in timeReq)
                    {
                        DateTime time2 = item.WaitTo;
                        time2= time2.AddHours(-3);
                        item.WaitTo = time2;
                    }
                    context.SaveChanges();

                    proxys = (from c in context.Proxies where (c.IsBanned == false && c.WaitTo < DateTime.Now) || (c.IsBanned == true && c.WaitTo < DateTime.Now) select c).ToList();
                }
               
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            return proxys;
        }


        /// <summary>
        /// Заблокировать прокси и ссылку
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task SetBlockedStateAsync(Proxy proxy, Link link, int ProxyBanWait = 4)
        {
            try
            {
                using (ParserContext context = new ParserContext())
                {
                    TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute + ProxyBanWait, DateTime.Now.Second);
                    DateTime date = DateTime.Now;
                    date = date.Date + timeSpan;

                    (from c in context.Links where c.Id == link.Id select c).First().IsParsed = false;
                    var newStateProxy = (from c in context.Proxies where c.Id == proxy.Id select c).First();
                    newStateProxy.IsBanned = true;
                    newStateProxy.WaitTo = date;
                    await context.SaveChangesAsync();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Увеличивает количество запросов на 1
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="count"></param>
        /// <param name="ProxyWaitSeconds"></param>
        /// <returns></returns>
        public async Task SetProxyCounterAsync(Proxy proxy, int count, int ProxyWaitSeconds = 4)
        {
            try
            {
                using (ParserContext context = new ParserContext())
                {
                    TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Second, DateTime.Now.Second + ProxyWaitSeconds);
                    DateTime date = DateTime.Now;
                    date = date.Date + timeSpan;

                    var newStateProxy = (from c in context.Proxies where c.Id == proxy.Id select c).First();
                    newStateProxy.RequestCount = count;
                    newStateProxy.WaitTo = date;

                    await context.SaveChangesAsync();
                }
            }catch(SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        /// <summary>
        /// Обновить ссылку
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task UpdateLinkAsync(Link link)
        {
            try
            {
                using (ParserContext context = new ParserContext())
                {
                    var newState = (from c in context.Links where c.Id == link.Id select c).First();
                    newState = link;
                    await context.SaveChangesAsync();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
