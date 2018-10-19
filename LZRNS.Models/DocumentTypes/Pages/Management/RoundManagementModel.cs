using LZRNS.DomainModels.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class RoundManagementModel : ManagementPageModel
	{
		public IList<Round> Rounds { get; set; }
	}
}
