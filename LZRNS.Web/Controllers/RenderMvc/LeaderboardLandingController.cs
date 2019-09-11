using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class LeaderboardLandingController : RenderMvcController
    {
        private readonly ISeasonRepository _seasonRepo;

        public LeaderboardLandingController(ISeasonRepository seasonRepo)
        {
            _seasonRepo = seasonRepo;
        }

        public ActionResult Index(LeaderboardLandingModel model, string ln, string r)
        {
            model.CurrentShownLeague = GetCurrentShownLeague(model.Leagues, ln);

            var roundNames = GetAllRoundsByLeagueNameAndYear(model.CurrentShownLeague, model.SeasonYearStart);
            model.AllRounds = SortRoundNames(roundNames);

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

        private static IEnumerable<string> GetRoundNames(LeagueSeason league)
        {
            return league?.Rounds?.Select(z => z.RoundName) ?? new List<string>();
        }

        private static List<string> SortRoundNames(IEnumerable<string> roundNames)
        {
            var names = roundNames.ToList();
            if (names.Count == 0) return new List<string>();
            int maxRoundNameLength = names.Max(x => x.Length);
            return names.OrderBy(k => k.PadLeft(maxRoundNameLength, '0')).ToList();
        }

        private IEnumerable<string> GetAllRoundsByLeagueNameAndYear(string leagueName, int year)
        {
            var league = GetLeagueByNameAndYear(leagueName, year);
            return GetRoundNames(league);
        }

        private LeagueSeason GetLeagueByNameAndYear(string leagueName, int year)
        {
            var startYearSeason = _seasonRepo.GetSeasonByYear(year);
            var leagueSeasons = startYearSeason?.LeagueSeasons ?? new List<LeagueSeason>();
            return leagueSeasons.FirstOrDefault(x => x.League.Name.Equals(leagueName));
        }
    }
}