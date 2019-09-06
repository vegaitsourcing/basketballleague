using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LZRNS.Common;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Extensions;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.AdditionalModels;
using LZRNS.Models.Extensions;

namespace LZRNS.Web.Controllers.Surface
{
	public class ManagementHtmlController : BaseSurfaceController
	{
		private readonly ISeasonRepository _seasonRepo;

		public ManagementHtmlController(ISeasonRepository seasonRepo)
		{
			_seasonRepo = seasonRepo;
		}

		[ChildActionOnly]
		public ActionResult ScheduledGames(int seasonStartYear, string roundName, string leagueName, DateTime? gamesDate)
		{
			if (seasonStartYear.Equals(default(int)) ||
				string.IsNullOrWhiteSpace(roundName) ||
				!gamesDate.HasValue) return new EmptyResult();
            
            var model = _seasonRepo.GetSeasonByYear(seasonStartYear)
					.LeagueSeasons.First(k => k.League.Name.Equals(leagueName))?
					.Rounds.Where(x => x.RoundName.CompareTo(roundName) <= 0)
					.SelectMany(y => y.Games)
					.Where(x => x.DateTime.Date == gamesDate.Value.Date);
            
            return PartialView(model);
		}

		public ActionResult GameResults(int seasonStartYear, string roundName, string leagueName)
		{
			if (seasonStartYear.Equals(default(int)) ||
				string.IsNullOrWhiteSpace(roundName)) return new EmptyResult();

            
			var model = _seasonRepo.GetSeasonByYear(seasonStartYear)
				.LeagueSeasons.First(k => k.League.Name.Equals(leagueName))
				.Rounds.Where(x => x.RoundName.Equals(roundName))
				.SelectMany(y => y.Games);
            
            
			return PartialView(model);
		}

		public ActionResult TopStats(int seasonStartYear, string roundName, string leagueName)
		{
			if (seasonStartYear.Equals(default(int)) ||
			    string.IsNullOrWhiteSpace(roundName)) return new EmptyResult();

			var model = _seasonRepo.GetSeasonByYear(seasonStartYear)
				.LeagueSeasons.First(k => k.League.Name.Equals(leagueName))
				.Rounds.Where(x => x.RoundName.CompareTo(roundName) <= 0)
				.SelectMany(y => y.Games)
				.Select(x => x.TeamAPlayerStats
					.Concat(x.TeamBPlayerStats))
				.SelectMany(x => x)
				.GroupBy(y => y.PlayerId); ;

			return PartialView(model);
		}

		public ActionResult TopStatsLeagues(int seasonStartYear, TopStatisticCategories category)
		{
			if (seasonStartYear == default(int))
			{
				return new EmptyResult();
			}

			var leagues = _seasonRepo.GetSeasonByYear(seasonStartYear)
				?.LeagueSeasons
				.Where(l => l.Rounds != null && l.Rounds.Any())
				.ToArray();

			if(leagues == null || !leagues.Any()) return new EmptyResult();

			int topStatsCount = AppSettings.StatisticsTableTopStatsToShow;

			List<LeagueTopStatsModel> leaguesTopStats = leagues.GetTopStatsPerLeague(category, topStatsCount).ToList();
			var model = new SeasonLeagueTopStatsModel(leaguesTopStats);

			return PartialView(model);
		}

		[ChildActionOnly]
		public ActionResult TopStatsPerLeague(LeagueTopStatsModel model)
		{
			if (model == null)
			{
				return new EmptyResult();
			}
			
			return PartialView(model);
		}

		public ActionResult Leaderboard(int seasonStartYear, string roundName, string leagueName)
		{
			if (seasonStartYear.Equals(default(int)) ||
			    string.IsNullOrWhiteSpace(roundName)) return new EmptyResult();

            var model = _seasonRepo.GetSeasonByYear(seasonStartYear)
				.LeagueSeasons.First(k => k.League.Name.Equals(leagueName))
				.Teams.Select(x => x.GetLeaderBoardPlacing(roundName))
				.OrderByDescending(x => x.Pts)
				.ThenByDescending(x => x.Diff)
				.ThenBy(x => x.TeamName);

			return PartialView(model);
		}
	}
}