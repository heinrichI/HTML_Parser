using AngleSharp.Dom.Html;
using AngleSharp.XPath;
using System;
using System.Collections.Generic;

namespace HTML_Parser.Pages
{
    public class OffersInMyRegion
	{
		private IHtmlDocument _document = null;

		private string _Shop = ".n-snippet-card2__shop-name a";
		private string _ProductName = ".n-snippet-card2__title a";
		private string _Price = ".n-snippet-card2__price .price";
		private string _DeliveryPice = ".n-delivery__info .n-delivery__price";
		private string _DeliveryTime = ".n-delivery__info .n-delivery__text";
		private string _FeedbackQuantity = ".n-shop-rating__description span";
		private string _ShopRating = ".n-rating-stars";
		private string _AdditionalOfferQuantity = "//div[@class='snippet-card__menu-item']//a[contains(text(),'предложение')]";
		private string _ShopDirectLink = ".n-snippet-card2__shop-name a";
		private string _ProductDescription = ".n-snippet-card2__desc";
		private string _ProductCard = ".n-snippet-card2";//Карта продукта

		private string _CapchaForm = "form[action=\"/checkcaptcha\"]";
		private string _CaptchaImage = "form[action=\"/checkcaptcha\"] img";

		public OffersInMyRegion(IHtmlDocument document)
		{
			_document = document;
		}

		public List<FieldSet> GetElements()
		{
			var shops = _document.QuerySelectorAll(_ProductCard);
			int i = 0;
			List<FieldSet> list = null;
			var captcha = _document.QuerySelector(_CapchaForm);
			var image = captcha?.QuerySelector(_CaptchaImage).GetAttribute("src");

            if (captcha != null)
            {
                Console.WriteLine($"Получена капча");
            }

            string html = _document.Body.ToString();
            try
            {
                if (_document.Title == "Ой!" || _document == null)
                {
                    list = new List<FieldSet>();

                    list.Add(new FieldSet() { Blocked = true });

                    CapchaModel capchaModel = new CapchaModel(_document);
                    FieldSet fields = capchaModel.GetData();
                    Recaptcha recaptcha = new Recaptcha();
                    // string img =  await recaptcha.GetKeywordAsync(fields.ImgPath); на случай рабочей рекапчи
                }
                else
                {
                    list = new List<FieldSet>();

                    foreach (var item in shops)
                    {
                        string offer = item.SelectSingleNode(_AdditionalOfferQuantity)?.TextContent.Length > 0 ?
                            (item.SelectSingleNode(_AdditionalOfferQuantity)?.TextContent.Split(' ').Length > 0 ? item.SelectSingleNode(_AdditionalOfferQuantity)?.TextContent.Split(' ')[1] : "")
                            :
                            "";

                        FieldSet fieldSet = new FieldSet()
                        {
                            Shop = item?.QuerySelector(_Shop)?.InnerHtml,
                            ProductName = BeautifyProductName(item?.QuerySelector(_ProductName)?.InnerHtml),
                            Price = BeautifyPrice(item.QuerySelector(_Price)?.InnerHtml),
                            DeliveryPice = BeautifyPrice(item?.QuerySelector(_DeliveryPice)?.InnerHtml),
                            DeliveryTime = BeautifyDeliveryTime(item?.QuerySelector(_DeliveryTime)?.InnerHtml),
                            FeedbackQuantity = BeatifyFeedback(item.QuerySelector(_FeedbackQuantity)?.InnerHtml),
                            ShopRating = item?.QuerySelector(_ShopRating)?.GetAttribute("data-rate"),
                            AdditionalOfferQuantity = offer,
                            ShopDirectLink = item?.QuerySelector(_ShopDirectLink)?.GetAttribute("href"),
                            ProductDescription = item?.QuerySelector(_ProductDescription)?.InnerHtml
                        };

                        list.Add(fieldSet);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("OffersInMyRegion " + e.Message);
            }
			

			return list;

		}

        /// <summary>
        /// Очистка цен
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        private string BeautifyPrice(string price)
        {
            try
            {
                if (price != null)
                    if (price.Contains("&nbsp"))
                    {
                        int index = price.IndexOf("&nbsp");
                        price = price?.Remove(index, 5);
                    }
                price = GetNumber(price);
            }
            catch(Exception e)
            {
                Console.WriteLine("В методе BeautifyPrice {0}", e.Message);
            }
            
            return price;
        }

        /// <summary>
        /// Очистка отзывов
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public string BeatifyFeedback(string feedback)
        {
            try
            {
                if (feedback != null)
                    if (feedback.Contains("&nbsp;"))
                    {
                        int index = feedback.IndexOf("&nbsp;");
                        feedback = feedback?.Remove(index, 5);
                    }
                feedback = GetNumber(feedback);
            }
            catch(Exception e)
            {
                Console.WriteLine("В методе BeatifyFeedback {0}", e.Message);
            }
            
            return feedback;
        }


        /// <summary>
        /// Очищаем время доставки от лишних символов
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
		private string BeautifyDeliveryTime(string time)
        {
            try
            {
                if (time != null)
                {
                    if (time.Contains("span"))
                    {
                        int closingBr = time.IndexOf(">");
                        if (closingBr >= 0)
                        {
                            time = time.Remove(0, closingBr + 1);
                            int openBr = time.IndexOf("<");
                            if (openBr >= 0)
                            {
                                time = time.Remove(openBr, time.Length - openBr);
                            }
                        }
                    }
                    if (time.Contains("&nbsp"))
                    {
                        time = time.Remove(time.IndexOf("&nbsp"), 5);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("В методе BeautifyDeliveryTime {0}",e.Message );
            }
           

            return time;
        }

        /// <summary>
        /// Вытаскиваем числа
        /// </summary>
        /// <returns></returns>
        public string GetNumber(string price)
        {
            string answer = "";
            try
            {
                if (price != null)
                {
                    foreach (char c in price)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (c.ToString().Equals(i.ToString()))
                            {
                                answer += i;
                            }
                        }
                    }

                    if (answer.Length > 0)
                    {
                        price = answer;
                    }
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine("В методе GetNumber {0}",e.Message);
            }
            
            return price;
        }

        /// <summary>
        /// Очистить наименование продукта от тегов
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string BeautifyProductName(string name)
        {
            try
            {
                if (name != null)
                {
                    string answer = "";
                    int index = name.IndexOf("<");

                    while (index >= 0)
                    {
                        int closingBracets = name.IndexOf(">");

                        if (index > 0)
                        {
                            answer += name.Remove(index, name.Length - index);
                            answer += name.Remove(0, closingBracets + 1);
                            name = answer;
                            answer = "";
                        }
                        if (index == 0)
                        {
                            answer += name.Remove(0, closingBracets + 1);
                            name = answer;
                            answer = "";
                        }
                        index = name.IndexOf("<");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("В методе BeautifyProductName {0}",e.Message);
            }
            

            return name;
        }
    }
}
