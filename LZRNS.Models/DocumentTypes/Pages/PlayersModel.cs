using LZRNS.Common.Extensions;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.Extensions;
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

        new public BannerModel Banner => this.GetCachedValue(() =>
            Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
                .EmptyIfNull()
                .FirstOrDefault()?
                .AsType<BannerModel>());

        public IEnumerable<string> Leagues => this.GetPropertyValue<IEnumerable<string>>();
        public string CurrentShownLeague { get; set; }
    }
}