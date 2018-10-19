using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class GameManagementModel : ManagementPageModel
	{
		public IEnumerable<SelectListItem> Seasons { get; set; }
	}
}
