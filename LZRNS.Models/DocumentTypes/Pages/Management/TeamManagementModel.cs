using LZRNS.DomainModel.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class TeamManagementModel : ManagementPageModel
	{
		public IList<Team> Teams { get; set; }
	}
}
