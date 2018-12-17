using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser
{
    class Recaptcha
    {
        public string CaptchaKey { get; set; } = "06b0949b45f9a9ffe02b89304a5f8d87";

        public string GetCaptchaId(string image)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://rucaptcha.com/in.php");
            // request.

            WebResponse resp = request.GetResponse();

            return null;
        }

        /// <summary>
        /// Получить кодовое слово капчи
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public async Task<string> GetKeywordAsync(string imageUrl)
        {
            string uri = imageUrl.Trim('"');
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imageUrl.Trim('"'));
            WebClient webClient = new WebClient();
            WebResponse resp = null;
            string text = null;
            string image64;
            string answer = null;
            string keyword = null;
            try
            {
                resp = request.GetResponse();
                text = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                image64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));

                byte[] image = webClient.DownloadData(new Uri(imageUrl));
                string keyToken = await GetKeyTokenAsync(image);
                WebHeaderCollection collection = new WebHeaderCollection();
                request = (HttpWebRequest)WebRequest.Create("http://rucaptcha.com/res.php");
                collection.Add("action", "get");
                collection.Add("key",CaptchaKey);                
                collection.Add("id", keyToken);
                request.Headers.Add(collection);
                request.Method = "GET";

                
                resp = request.GetResponse();               
                answer = new StreamReader(resp.GetResponseStream(), Encoding.UTF8).ReadToEnd();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        /// <summary>
        /// Получить id капчи у сервиса
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private async Task<string> GetKeyTokenAsync(byte[] image)
        {
           // byte[] fileArray = Encoding.UTF8.GetBytes(image);
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new StringContent("post"), "method");
            form.Add(new StringContent(CaptchaKey), "key");
            form.Add(new ByteArrayContent(image), "file", "capcha.jpg");
            HttpResponseMessage response = await httpClient.PostAsync("http://rucaptcha.com/in.php", form);
            response.EnsureSuccessStatusCode(); httpClient.Dispose();
            string result = response.Content.ReadAsStringAsync().Result;
            string token_key = null;

            if (result.Contains("OK"))
            {
                token_key = result?.Split('|')[1];
            }            

            return token_key;
        }
        
    }
}
