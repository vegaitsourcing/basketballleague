using System.Web;
using System.Web.Mvc;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.DocumentTypes.Nodes;

namespace LZRNS.Web.Controllers.Surface
{
	public class SharedController : BaseSurfaceController
	{
		[ChildActionOnly]
		public ActionResult SeoMetaTags(PageModel model)
		{
			return RenderActionResult(model, () => PartialView(model));
		}

        [ChildActionOnly]
        public ActionResult Header(HeaderModel model)
        {
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Footer(FooterModel model)
        {
            return PartialView(model);
        }
    }
}