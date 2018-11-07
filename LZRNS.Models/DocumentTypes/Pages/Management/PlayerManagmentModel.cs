using LZRNS.DomainModel.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class PlayerManagementModel : ManagementPageModel
    {

		public IEnumerable<Player> Players { get; set; }
    }
}
