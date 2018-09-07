using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LZRNS.Models.DocumentTypes.Compositions;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class XMLSitemapController : RenderMvcController
	{
		public ActionResult XMLSitemap(PageModel model)
		{
			return CurrentTemplate(model);
		}
	}
}
