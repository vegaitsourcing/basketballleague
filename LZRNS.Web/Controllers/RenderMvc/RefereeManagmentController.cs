﻿using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class RefereeManagmentController : RenderMvcController
    {
        public ActionResult Index(RefereeManagmentModel model)
        {
            return CurrentTemplate(model);
        }
    }
}