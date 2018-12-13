using AngleSharp.Dom.Html;
using AngleSharp.XPath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser
{
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
        
        public List<FieldSet> GetElements()
        {
            Dictionary<string, string> elements = new Dictionary<string, string>();
            var shops = _document.QuerySelectorAll(_ProductCard);
            int i = 0;
            List<FieldSet> list = new List<FieldSet>();
            var captcha = _document.QuerySelector(_CapchaForm);
            var image = captcha?.QuerySelector(_CaptchaImage).GetAttribute("src");
            Console.WriteLine($"Капча {captcha}");

            if(_document.Title == "Ой!" || _document == null)
            {
                list.Add(new FieldSet() { Blocked = true});
            }
            else
            {
                foreach (var item in shops)
                {
                    /*Console.WriteLine($"№ {++i}");
                    Console.WriteLine($"Магазин: {item.QuerySelector(_Shop).InnerHtml}");
                    Console.WriteLine($"Товар: {item.QuerySelector(_ProductName).InnerHtml}");
                    Console.WriteLine($"Цена: {item.QuerySelector(_Price).InnerHtml}");
                    Console.WriteLine($"Стоимость доставки: {item.QuerySelector(_DeliveryPice)?.InnerHtml}");
                    Console.WriteLine($"Время доставки {item.QuerySelector(_DeliveryTime)?.InnerHtml}");
                    Console.WriteLine($"Отзывы: {item.QuerySelector(_FeedbackQuantity)?.InnerHtml}");
                    Console.WriteLine($"Рейтинг: {item.QuerySelector(_ShopRating)?.GetAttribute("data-rate")}");
                    string offer = item.SelectSingleNode(_AdditionalOfferQuantity)?.TextContent.Length > 0 ? 
                        (item.SelectSingleNode(_AdditionalOfferQuantity)?.TextContent.Split(' ').Length>0 ? item.SelectSingleNode(_AdditionalOfferQuantity)?.TextContent.Split(' ')[1]: "" )
                        : 
                        "";

                    Console.WriteLine($"Доп. предложение: {offer}");
                    Console.WriteLine($"Сссылка на магазин: {item.QuerySelector(_ShopDirectLink)?.GetAttribute("href")}");
                    Console.WriteLine($"Описание продукта: {item.QuerySelector(_ProductDescription)?.InnerHtml}");*/
                    string offer = item.SelectSingleNode(_AdditionalOfferQuantity)?.TextContent.Length > 0 ?
                        (item.SelectSingleNode(_AdditionalOfferQuantity)?.TextContent.Split(' ').Length > 0 ? item.SelectSingleNode(_AdditionalOfferQuantity)?.TextContent.Split(' ')[1] : "")
                        :
                        "";

                    list.Add(new FieldSet()
                    {
                        Shop = item.QuerySelector(_Shop).InnerHtml,
                        ProductName = item.QuerySelector(_ProductName).InnerHtml,
                        Price = item.QuerySelector(_Price).InnerHtml,
                        DeliveryPice = item.QuerySelector(_DeliveryPice)?.InnerHtml,
                        DeliveryTime = item.QuerySelector(_DeliveryTime)?.InnerHtml,
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
    }
}
