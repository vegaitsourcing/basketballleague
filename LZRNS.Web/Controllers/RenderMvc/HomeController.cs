using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class HomeController: RenderMvcController
    {
        public ActionResult Index(HomeModel model)
        {
            return CurrentTemplate(model);
        }
    }
}