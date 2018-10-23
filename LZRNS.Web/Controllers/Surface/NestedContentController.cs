using System.Web.Mvc;
using LZRNS.Models.DocumentTypes.Nodes.NestedContent;

namespace LZRNS.Web.Controllers.Surface
{
	public class NestedContentController : BaseSurfaceController
	{
		[ChildActionOnly]
		public ActionResult Render(NestedContentBaseModel model)
		{
			return RenderActionResultBasedOnName(model);
		}
	}
}