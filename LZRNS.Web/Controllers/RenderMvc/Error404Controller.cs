using System.Net;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LZRNS.Models.DocumentTypes.Pages;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class Error404Controller : RenderMvcController
	{
		public ActionResult Index(Error404Model model)
		{
			Response.StatusCode = (int)HttpStatusCode.NotFound;
			Response.Status = "404 not found";
			Response.TrySkipIisCustomErrors = true;

			return CurrentTemplate(model);
		}
	}
}
