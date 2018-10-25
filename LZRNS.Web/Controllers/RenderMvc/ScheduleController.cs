using System.Linq;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class ScheduleController : RenderMvcController
    {
        public ActionResult Index(ScheduleModel model, string ln)
        {
	        model.CurrentShownLeague = !string.IsNullOrWhiteSpace(ln) ? ln : model.Leagues.FirstOrDefault();

			return CurrentTemplate(model);
        }
    }
}