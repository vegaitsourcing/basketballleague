using LZRNS.DomainModel.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class RefereeManagementModel : ManagementPageModel
	{
		public IEnumerable<Referee> Referees { get; set; }
	}
}
