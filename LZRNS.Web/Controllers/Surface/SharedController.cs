using System.Web;
using System.Web.Mvc;
using LZRNS.Models.DocumentTypes.Compositions;

namespace LZRNS.Web.Controllers.Surface
{
	public class SharedController : BaseSurfaceController
	{
		[ChildActionOnly]
		public ActionResult SeoMetaTags(PageModel model)
		{
			return RenderActionResult(model, () => PartialView(model));
		}
	}
}