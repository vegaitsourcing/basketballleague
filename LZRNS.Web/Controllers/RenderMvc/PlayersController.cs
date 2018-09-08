using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class PlayersController : RenderMvcController
    {
        public ActionResult Index(PlayersModel model)
        {
            return CurrentTemplate(model);
        }
    }
}