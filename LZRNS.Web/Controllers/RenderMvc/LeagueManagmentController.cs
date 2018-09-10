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
    public class LeagueManagmentController : RenderMvcController
    {
        private ILeagueRepository _leagueRepo;
        public LeagueManagmentController(ILeagueRepository leagueRepo)
        {
            _leagueRepo = leagueRepo;
        }
        

        public ActionResult Index(LeagueManagmentModel model)
        {
            var viewModel = _leagueRepo.GetAll();
            return CurrentTemplate(viewModel);
        }
    }
}