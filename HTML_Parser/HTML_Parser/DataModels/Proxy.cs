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
	}
}
