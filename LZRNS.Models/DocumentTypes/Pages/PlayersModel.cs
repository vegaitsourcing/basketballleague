using LZRNS.Common.Extensions;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.Extensions;
using RJP.MultiUrlPicker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
    public class PlayersModel : PageModel
    {
        public PlayersModel()
        {
        }

        public PlayersModel(IPublishedContent content) : base(content)
        {
        }

        public PlayersModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }
		
        public IEnumerable<string> Leagues => this.GetPropertyValue<IEnumerable<string>>();
        public string CurrentShownLeague { get; set; }

        public Link PlayerDetailsPage => this.GetPropertyValue<Link>();

    }
}
