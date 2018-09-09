using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class SeasonManagmentController : RenderMvcController
    {
        private ISeasonRepository _seasonRepo;
        public SeasonManagmentController(ISeasonRepository seasonRepo)
        {
            _seasonRepo = seasonRepo;
        }

        public ActionResult Index(SeasonManagmentModel model)
        {
            var viewModel = _seasonRepo.GetAll();
            return CurrentTemplate(viewModel);
        }
    }
}