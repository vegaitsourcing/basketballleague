using LZRNS.Common.Extensions;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

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
            model.CurrentShownLeague = GetCurrentShownLeague(model.Leagues, ln);
            model.AllRounds = GetAllRounds(model.CurrentShownLeague, model.SeasonYearStart);
            model.CurrentShownRound = GetCurrentShownRound(model.AllRounds, r);

            return CurrentTemplate(model);
        }

        private static string GetCurrentShownLeague(IEnumerable<string> leagues, string leagueName)
        {
            return !string.IsNullOrWhiteSpace(leagueName) ? leagueName : leagues.FirstOrDefault();
        }

        private static string GetCurrentShownRound(IEnumerable<string> rounds, string round)
        {
            return !string.IsNullOrWhiteSpace(round) ? round : rounds.FirstOrDefault();
        }

        private static List<string> SortRoundNames(IEnumerable<string> roundNames)
        {
            var roundsList = roundNames.ToList().ConvertAll(name => new { number = name.ExtractNumber(), name });
            return roundsList.OrderBy(item => item.number).ToList().ConvertAll(item => item.name);
        }

        private List<string> GetAllRounds(string leagueName, int year)
        {
            var league = GetLeagueByNameAndYear(leagueName, year);
            var roundNames = GetRoundNames(league);
            return SortRoundNames(roundNames);
        }

        private static IEnumerable<string> GetRoundNames(LeagueSeason league)
        {
            return league?.Rounds?.Select(z => z.RoundName) ?? new List<string>();
        }

        private LeagueSeason GetLeagueByNameAndYear(string leagueName, int year)
        {
            var startYearSeason = _seasonRepo.GetSeasonByYear(year);
            var leagueSeasons = startYearSeason?.LeagueSeasons ?? new List<LeagueSeason>();
            return leagueSeasons.FirstOrDefault(x => x.League.Name.Equals(leagueName));
        }
    }
}