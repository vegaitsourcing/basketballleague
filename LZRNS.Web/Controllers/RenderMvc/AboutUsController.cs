using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class AboutUsController : RenderMvcController
    {
        public ActionResult Index(AboutUsModel model)
        {
            return CurrentTemplate(model);
        }
    }
}
