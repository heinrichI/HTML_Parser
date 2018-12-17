using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using HTML_Parser.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser
{
	class Parser
	{
		private IHtmlDocument document;

		public async Task<List<FieldSet>> ParseDomAsync(string html)
		{
			document = await new HtmlParser().ParseAsync(html);
			List<FieldSet> data = new List<FieldSet>();

			try
			{
				if (html != null)
				{
					DomModel model = new DomModel(document);
					data = await model.GetElementsAsync();
				}

				if (data.Count == 0)
				{
					EmptyPage emptyPage = new EmptyPage(document);
					data = emptyPage.GetElements();
				}

				if (data.Count == 0)
				{
					OffersInMyRegion offersInMyRegion = new OffersInMyRegion(document);
					data = offersInMyRegion.GetElements();
				}
				else
				{

				}
			}catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			

			return data;
		}

	}
}
