using System.Globalization;
using System.Web;
using Umbraco.Core.Models;
using LZRNS.Models.Extensions;
using LZRNS.Models.DocumentTypes.Compositions;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class Error404Model : PageModel
	{
		public Error404Model()
		{
		}

		public Error404Model(IPublishedContent content) : base(content)
		{
		}

		public Error404Model(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		public IHtmlString Text => this.GetPropertyValue<IHtmlString>();
	}
}
