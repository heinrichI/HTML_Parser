using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser.DataModels
{
	public class Link
	{
		public long Id { get; set; }

		public string Url { get; set; }

		public bool IsParsed { get; set; }

		//public ParsedData ParsedData {get; set; }
	}
}
