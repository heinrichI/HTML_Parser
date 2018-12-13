using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser.DataModels
{
	public class ParserContext:DbContext
	{
		public ParserContext() : base("DBConnection")
		{ }

		public DbSet<Link> Links { get; set; }
		public DbSet<ParsedData> ParsedData { get; set; }
		public DbSet<Proxy> Proxies { get; set; }
	}
}
