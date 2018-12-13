using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser.DataModels
{
	public class ParsedData
	{
		/// <summary>
		/// Id записи
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Id запроса связанного
		/// </summary>
		public Link LinkId { get; set; }
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

        /// <summary>
        /// Время когда обработали
        /// </summary>
        public DateTime ParsedTime { get; set; }
	}
}
