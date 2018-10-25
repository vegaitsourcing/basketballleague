using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class HistoryController : RenderMvcController
	{
		public ActionResult Index(HistoryModel model)
		{
			return CurrentTemplate(model);
		}
	}
}