using AngleSharp.Dom.Html;
using AngleSharp.XPath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser
{
	/// <summary>
	/// 
	/// </summary>
    class DomModel
    {
        private IHtmlDocument _document;

        private string _Shop = ".n-snippet-card a.link_type_shop-name";
        private string _ProductName = ".n-snippet-card a.snippet-card__header-link span";
        private string _Price = ".snippet-card__price .price";
        private string _DeliveryPice = ".n-delivery__info .n-delivery__price";
        private string _DeliveryTime = ".n-delivery__info .n-delivery__time";
        private string _FeedbackQuantity = ".n-shop-rating__description span";
        private string _ShopRating = ".n-rating-stars";
        private string _AdditionalOfferQuantity = "//div[@class='snippet-card__menu-item']//a[contains(text(),'предложение')]";
        private string _ShopDirectLink = ".snippet-card__action a";
        private string _ProductDescription = ".snippet-card__desc";
        private string _ProductCard = ".n-snippet-card";//Карта продукта

        private string _CapchaForm = "form[action=\"/checkcaptcha\"]";
        private string _CaptchaImage = "form[action=\"/checkcaptcha\"] img";

        public DomModel(IHtmlDocument document)
        {
            _document = document;
        }
        
        public async Task<List<FieldSet>> GetElementsAsync()
        {
            var shops = _document.QuerySelectorAll(_ProductCard);
            int i = 0;
            List<FieldSet> list = null;
            var captcha = _document.QuerySelector(_CapchaForm);
            var image = captcha?.QuerySelector(_CaptchaImage).GetAttribute("src");
            Console.WriteLine($"Капча {captcha}");
            string html = _document.Body.ToString();

            if(_document.Title == "Ой!" || _document == null)
            {
				list = new List<FieldSet>();

				list.Add(new FieldSet() { Blocked = true});

                CapchaModel capchaModel = new CapchaModel(_document);
                FieldSet fields = capchaModel.GetData();
                Recaptcha recaptcha = new Recaptcha();
               // string img =  await recaptcha.GetKeywordAsync(fields.ImgPath);
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


                    list.Add(new FieldSet()
                    {
                        Shop = item.QuerySelector(_Shop)?.InnerHtml,
                        ProductName = item.QuerySelector(_ProductName)?.InnerHtml,
                        Price = BeautifyPrice(item.QuerySelector(_Price)?.InnerHtml),
                        DeliveryPice = BeautifyPrice(item.QuerySelector(_DeliveryPice)?.InnerHtml),
                        DeliveryTime = BeautifyDeliveryTime(item.QuerySelector(_DeliveryTime)?.InnerHtml),
                        FeedbackQuantity = item.QuerySelector(_FeedbackQuantity)?.InnerHtml,
                        ShopRating = item.QuerySelector(_ShopRating)?.GetAttribute("data-rate"),
                        AdditionalOfferQuantity = offer,
                        ShopDirectLink = item.QuerySelector(_ShopDirectLink)?.GetAttribute("href"),
                        ProductDescription = item.QuerySelector(_ProductDescription)?.InnerHtml
                    });
                }
            }
            
            return list;
        }



		private string BeautifyPrice(string price)
		{
			if (price != null)
				if (price.Contains("&nspb"))
				{
					price = price?.Remove(price.IndexOf("&nspb"), 5);
				}
			return price;
		}

		private string BeautifyDeliveryTime(string time)
		{
			if (time != null)
			{
				if (time.Contains("span"))
				{
					int closingBr = time.IndexOf(">");
					time = time.Remove(0, closingBr + 1);
					int openBr = time.IndexOf("<");
					time = time.Remove(openBr, time.Length - openBr);
				}

				if (time.Contains("&nspb"))
				{
					time = time.Remove(time.IndexOf("&nspb"), 5);
				}
			}

			return time;
		}
	}
}
