using System.Linq;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class HomeController : RenderMvcController
	{
		public ActionResult Index(HomeModel model, string ln)
		{
			model.CurrentShownLeague = !string.IsNullOrWhiteSpace(ln) ? ln : model.Leagues.LastOrDefault();

			return CurrentTemplate(model);
		}
	}
}