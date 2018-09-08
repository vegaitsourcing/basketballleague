using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class MatchManagmentController : RenderMvcController
    {
        public ActionResult Index(MatchManagmentModel model)
        {
            return CurrentTemplate(model);
        }
        [HttpPost]
        public string Index()
        {
            return "you hit me";
        }
    }
}