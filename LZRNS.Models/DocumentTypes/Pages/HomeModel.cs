using System.Globalization;
using Umbraco.Core.Models;
using LZRNS.Models.DocumentTypes.Compositions;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class HomeModel : PageModel
	{
		public HomeModel()
		{
		}

		public HomeModel(IPublishedContent content) : base(content)
		{
		}

		public HomeModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}
	}
}
