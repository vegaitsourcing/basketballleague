using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LZRNS.Models.DocumentTypes.Pages;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class SitemapController : RenderMvcController
	{
		public ActionResult Index(SitemapModel model)
		{
			return CurrentTemplate(model);
		}
	}
}
