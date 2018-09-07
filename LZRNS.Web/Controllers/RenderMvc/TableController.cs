using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class TableController : RenderMvcController
    {
        public ActionResult Index(TableModel model)
        {
            return CurrentTemplate(model);
        }
    }
}