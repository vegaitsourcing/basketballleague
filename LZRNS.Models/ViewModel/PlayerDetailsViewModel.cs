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
    public class PlayerDetailsViewModel : PageModel
    {
        public PlayerDetailsModel PlayerDetailsModel { get; set; }
        public Player CurrentPlayer { get; set; }

    }
}
