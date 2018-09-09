using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Surface
{
    public class LeaguesController : SurfaceController
    {
        private ILeagueRepository _leagueRepo;

        public LeaguesController(ILeagueRepository leagueRepo)
        {
            _leagueRepo = leagueRepo;
        }

        [HttpPost]
        public ActionResult AddLeague(League league)
        {
            var addedItem = _leagueRepo.Add(league);
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult EditLeague(League league)
        {
            _leagueRepo.Update(league);
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult DeleteLeague(Guid id)
        {
            var league = _leagueRepo.GetById(id);
            _leagueRepo.Delete(league);
            return RedirectToCurrentUmbracoPage();
        }
    }
}
