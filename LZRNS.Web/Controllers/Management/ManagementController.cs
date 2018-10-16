using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
	public class ManagementController : RenderMvcController
	{
		public ActionResult Index(ManagementModel model)
		{
			return View(model);
		}
	}
}