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
    public class PlayerDetailsModel : PageModel
    {
        public PlayerDetailsModel()
        {
        }

        public PlayerDetailsModel(IPublishedContent content) : base(content)
        {
        }

        public PlayerDetailsModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
        {
        }

    }
}
