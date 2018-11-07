using LZRNS.DomainModel.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.Models.ViewModel
{
    public class PlayersViewModel : PageModel
    {
        public PlayersModel PlayersModel { get; set; }
        public IEnumerable<Player> Players { get; set; }

        public string CurrentQuery { get; set; }
        public string CurrentFL { get; set; }
        public bool ActiveOnly { get; set; }
    }
}
