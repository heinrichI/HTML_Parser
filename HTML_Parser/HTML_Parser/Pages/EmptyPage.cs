using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Parser.Pages
{
	public class EmptyPage
	{
		private IHtmlDocument _document = null;
		private string EmptyTytle = ".n-noresult__title";


		public EmptyPage(IHtmlDocument document)
		{
			_document = document;
		}

		public List<FieldSet> GetElements()
		{
			var title = _document.QuerySelector(EmptyTytle);
			List<FieldSet> elements = new List<FieldSet>();
			FieldSet field = new FieldSet();
			if (title!=null)
			{
				field.ProductName = title?.InnerHtml;
				elements.Add(field);
			}
			return elements;
		}
	}
}
