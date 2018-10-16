using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class ManagementController : RenderMvcController
	{
		public ActionResult Index(ManagementModel model)
		{
			return View(model);
		}
	}
}