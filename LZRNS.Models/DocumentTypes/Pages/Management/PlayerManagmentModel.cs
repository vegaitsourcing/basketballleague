using LZRNS.DomainModel.Models;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class PlayerManagementModel : ManagementModel
    {
	    public PlayerManagementModel(IPublishedContent content) : base(content)
	    {
	    }

		public IEnumerable<Player> Players { get; set; }
    }
}
