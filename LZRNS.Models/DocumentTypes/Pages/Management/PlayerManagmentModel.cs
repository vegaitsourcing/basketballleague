using LZRNS.DomainModel.Models;
using System.Collections.Generic;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class PlayerManagementModel : ManagementModel
    {
		public IEnumerable<Player> Players { get; set; }
	}
}
