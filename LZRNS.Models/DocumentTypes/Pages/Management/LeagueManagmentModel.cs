using LZRNS.DomainModel.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class LeagueManagementModel : ManagementPageModel
    {
		public IEnumerable<League> Leagues { get; set; }
	}
}
