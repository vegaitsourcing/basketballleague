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
    public class RefereeManagmentController : RenderMvcController
    {
        private IRefereeRepository _refereeRepo;
        public RefereeManagmentController(IRefereeRepository refereeRepo)
        {
            _refereeRepo = refereeRepo;
        }

        public ActionResult Index(RefereeManagmentModel model)
        {
            var viewModel = _refereeRepo.GetAll();
            return CurrentTemplate(viewModel);
        }
    }
}