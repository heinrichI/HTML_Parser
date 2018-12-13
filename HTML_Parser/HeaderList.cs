using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Reflection;

namespace HTML_Parser
{
    [DataContract]
    public class HeaderList
    {
        private object locker = new object();

        public HeaderList()
        {

        }

        public Dictionary<string, CookieCollection> _cookies = new Dictionary<string, CookieCollection>();
        [DataMember]
        public Dictionary<string, Dictionary<string, string>> _headerList = new Dictionary<string, Dictionary<string, string>>();

        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(DateTime) && property.GetValue(this).Equals(DateTime.MinValue))
                {
                    property.SetValue(this, DateTime.MinValue.ToUniversalTime());
                }
            }
        }


        public WebHeaderCollection this[ProxyData proxy]
        {
            get
            {
                lock (locker)
                {
                    if (_headerList.ContainsKey("header_" + proxy.Host + ":" + proxy.Port))
                    {
                        return GetWebHeader(proxy);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public CookieContainer GetCookie(ProxyData proxy)
        {
            CookieContainer container = new CookieContainer();
            lock (locker)
            {
                if (_cookies.Count > 0)
                {
                    // var cook = _cookies.Where(x => x.Key == "cookie_" + proxy.Host + ":" + proxy.Port).First();
                    CookieCollection cook = null;
                    if (_cookies.ContainsKey("cookie_" + proxy.Host + ":" + proxy.Port))
                    {
                        cook =_cookies["cookie_" + proxy.Host + ":" + proxy.Port] ; 
                    }

                    if (cook != null)
                    {
                        CookieCollection collection = new CookieCollection();
                        collection = cook;

                        foreach (var cookie in collection)
                        {
                            var c = (Cookie)cookie;
                            container.Add(c);
                        }

                        return container;
                    }
                    else
                    {
                        return new CookieContainer();
                    }
                }
                return new CookieContainer();
            }
        }

        public void AddCookie(ProxyData proxy, CookieCollection cookie)
        {
            lock (locker)
            {
                if (!_cookies.ContainsKey(("cookie_" + proxy.Host + ":" + proxy.Port)))
                {
                    _cookies.Add(("cookie_" + proxy.Host + ":" + proxy.Port), cookie);
                }
                
            }
        }

        public WebHeaderCollection GetWebHeader(ProxyData proxy)
        {
            lock (locker)
            {
                if(_headerList.ContainsKey("header_" + proxy.Host + ":" + proxy.Port))
                {
                    WebHeaderCollection collection = new WebHeaderCollection();
                    Dictionary<string, string> headers = _headerList["header_" + proxy.Host + ":" + proxy.Port];
                    var keys = headers.Keys;

                    foreach (var key in keys)
                    {
                        collection.Add(key,headers[key]);
                        
                    }


                    return collection;
                }
                else
                {
                    return null;
                }
            }
        }

        public void AddWebHeader(ProxyData proxy, WebHeaderCollection headers)
        {

            lock (locker)
            {
                var keys = headers.AllKeys;
                Dictionary<string, string> headerDict = new Dictionary<string, string>();
                foreach (var key in keys)
                {
                    headerDict.Add(key, headers.Get(key));
                }

                if (!_headerList.ContainsKey(("header_" + proxy.Host + ":" + proxy.Port)))
                {
                    _headerList.Add(("header_" + proxy.Host + ":" + proxy.Port), headerDict);
                }

            }
        }
    }

}

