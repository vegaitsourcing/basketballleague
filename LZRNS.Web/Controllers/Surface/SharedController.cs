using System.Collections.Generic;
using System.Web.Mvc;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.DocumentTypes.Nodes;

namespace LZRNS.Web.Controllers.Surface
{
	public class SharedController : BaseSurfaceController
	{
		[ChildActionOnly]
		public ActionResult Header(HeaderModel model, IEnumerable<PageModel> navigationItems)
		{
			model.NavigationItems = navigationItems;

			return PartialView(model);
		}

		[ChildActionOnly]
		public ActionResult Footer(FooterModel model)
		{
			return PartialView(model);
		}

		[ChildActionOnly]
		public ActionResult Banner(BannerModel model)
		{
			return PartialView(model);
		}
	}
}