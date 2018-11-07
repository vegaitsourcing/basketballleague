using System.Collections.Generic;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.Extensions;
using System.Globalization;
using System.Linq;
using System.Web;
using LZRNS.Common.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
    public class AboutUsModel : PageModel
    {
        public AboutUsModel()
        {
        }

        public AboutUsModel(IPublishedContent content) : base(content)
		{
        }

        public AboutUsModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
        }
	    
	    public string ContactFormTitle => this.GetPropertyValue<string>();
		public IHtmlString Text => this.GetPropertyValue<IHtmlString>();
        public string EmailAddress => this.GetPropertyValue<string>();
    }
}
