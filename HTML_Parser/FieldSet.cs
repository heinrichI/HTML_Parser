namespace HTML_Parser
{
    public class FieldSet
    {
        /// <summary>
        /// Магазин
        /// </summary>
        public string Shop { get; set; }
        /// <summary>
        /// Товар
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// Стоимость доставки
        /// </summary>
        public string DeliveryPice { get; set; }
        /// <summary>
        /// Время доставки
        /// </summary>
        public string DeliveryTime { get; set; }
        /// <summary>
        /// Количество отзывов
        /// </summary>
        public string FeedbackQuantity { get; set; }
        /// <summary>
        /// Рейтинг магазина
        /// </summary>
        public string ShopRating { get; set; }
        /// <summary>
        /// Количество дополнительных предложений
        /// </summary>
        public string AdditionalOfferQuantity { get; set; }
        /// <summary>
        /// Ссылка на магазин
        /// </summary>
        public string ShopDirectLink { get; set; }
        /// <summary>
        /// Описание продукта
        /// </summary>
        public string ProductDescription { get; set; }

        public bool Blocked { get; set; }
        
        /// <summary>
        /// Код формы
        /// </summary>
        public string FormKey { get; set; }

        /// <summary>
        /// Путь для возврата контента
        /// </summary>
        public string RetPath { get; set; }

        /// <summary>
        /// Путь для скачивания капчи
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        /// Строка ответа
        /// </summary>
        public string RepName { get; set; }


        public bool NoData { get; set; }
    }
}
