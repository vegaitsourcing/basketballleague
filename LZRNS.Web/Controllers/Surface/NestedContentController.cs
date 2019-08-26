using System.Web.Mvc;
using LZRNS.Models.DocumentTypes.Nodes.NestedContent;
using LZRNS.Models.DocumentTypes.Nodes.NestedContent.Sections;
using LZRNS.ExcelLoader;
namespace LZRNS.Web.Controllers.Surface
{
	public class NestedContentController : BaseSurfaceController
	{
		[ChildActionOnly]
		public ActionResult Render(NestedContentBaseModel model)
		{
			return RenderActionResultBasedOnName(model);
		}

		[ChildActionOnly]
		public ActionResult RenderSection(SectionBaseModel model, string name)
		{
			model.LeagueName = name;
            return RenderActionResultBasedOnName(model);
		}
	}
}