using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public IEnumerable<string> Name { get; set; }

        public int NamesPercolumn => NamesPerColumn(Name);

        public int NamesPerColumn(IEnumerable<string> Names)
        {
            int namesPerColumn = Names.Count() / 4;
            if (namesPerColumn == 0)
            {
                namesPerColumn = 1;
            }
            return namesPerColumn; 
        }
    }
}
