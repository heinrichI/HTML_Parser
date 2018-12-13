using System;

namespace HTML_Parser.DataModels
{
	public class Link
	{
		public long Id { get; set; }

		public string Url { get; set; }

		public bool IsParsed { get; set; }

		public int ProductId { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime AddingDate { get; set; }
	}
}
