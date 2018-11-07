using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class StatisticsController : RenderMvcController
	{
		public ActionResult Index(StatisticsModel model) => CurrentTemplate(model);
	}
}