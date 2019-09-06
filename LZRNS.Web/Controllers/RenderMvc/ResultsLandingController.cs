using System.Linq;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LZRNS.DomainModels.Repository.Interfaces;
using System.Collections.Generic;
using System;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class ResultsLandingController : RenderMvcController
    {
        private readonly ISeasonRepository _seasonRepo;

        public ResultsLandingController(ISeasonRepository seasonRepo)
        {
            _seasonRepo = seasonRepo;
        }

        public ActionResult Index(ResultsLandingModel model, string ln, string r)
        {
            model.CurrentShownLeague = !string.IsNullOrWhiteSpace(ln) ? ln : model.Leagues.FirstOrDefault();
            //NOTE: bad implementation, patch for staging, refactor
            var allRounds = _seasonRepo.GetSeasonByYear(model.SeasonYearStart).LeagueSeasons
                .First(x => x.League.Name.Equals(model.CurrentShownLeague)).Rounds
                //.Where(y => y.RoundName.CompareTo(model.RoundName) <= 0)
                .Select(z => z.RoundName);

            int maxLen = allRounds.Max(x => x.Length);
            List<int> roundsList = allRounds.ToList().ConvertAll(int.Parse);
            

            model.AllRounds = roundsList.OrderBy(k => k).ToList().ConvertAll(x =>x.ToString()).ToList();
            /*
            model.AllRounds =_seasonRepo.GetSeasonByYear(model.SeasonYearStart).LeagueSeasons
				.First(x => x.League.Name.Equals(model.CurrentShownLeague)).Rounds
				.Where(y => y.RoundName.CompareTo(model.RoundName) <= 0)
				.Select(z => z.RoundName)
				.OrderBy(k => k)
				.ToList();
			*/
            model.CurrentShownRound = !string.IsNullOrWhiteSpace(r) ? r : model.AllRounds.First();

            return CurrentTemplate(model);
        }
    }
}