using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser.DataModels
{
	public class Proxy
	{
		/// <summary>
		/// Id записи
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Адресс ссылки
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Свойство заблокированности
		/// </summary>
		public bool IsBanned { get; set; }

		/// <summary>
		/// Ждать использования до
		/// </summary>
		public DateTime WaitTo { get; set; }

        /// <summary>
        /// Максимальное количество запросов с данного прокси
        /// </summary>
        public int MaxRequests { get; set; } = 5;

        /// <summary>
        /// Количество проведенных запросов
        /// </summary>
        public int RequestCount { get; set; } = 0;
	}
}
