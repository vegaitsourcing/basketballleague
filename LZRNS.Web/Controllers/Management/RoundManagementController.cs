using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
	[MemberAuthorize]
	public class RoundManagementController : RenderMvcController
	{
		private IRoundRepository _roundRepo;
		public RoundManagementController(IRoundRepository roundRepo)
		{
			_roundRepo = roundRepo;
		}


		public ActionResult Index(RoundManagementModel model)
		{
			model.Rounds = _roundRepo.GetAll().ToList();

			return View(model);
		}
	}

	[MemberAuthorize]
	public class RoundManagementSurfaceController : SurfaceController
	{
		private readonly IRoundRepository _roundRepo;
		private readonly ISeasonRepository _seasonRepo;
        private readonly ITeamRepository _teamRepo;

        public RoundManagementSurfaceController(IRoundRepository roundRepo, ISeasonRepository seasonRepo, ITeamRepository teamRepo)
		{
			_roundRepo = roundRepo;
			_seasonRepo = seasonRepo;
            _teamRepo = teamRepo;
		}

		#region [Render Views Actions]
		[HttpGet]
		public ActionResult Add()
		{
			var model = new Round();
			model.LeagueSeasons = _seasonRepo.GetAllLeagueSeasons()
				.ToList()
				.Select(x => new SelectListItem()
				{
					Text = x.FullName,
					Value = x.Id.ToString()
				});

			return PartialView(model);
		}

		[HttpGet]
		public ActionResult Edit(Guid roundId)
		{
			var model = _roundRepo.GetById(roundId);
			model.LeagueSeasons = _seasonRepo.GetAllLeagueSeasons()
				.ToList()
				.Select(x => new SelectListItem()
				{
					Text = x.FullName,
					Value = x.Id.ToString()
				});

			return PartialView(model);
		}
		#endregion

		#region [Data Change Actions]

		[HttpPost]
		public ActionResult Add(Round model)
		{
			if (ModelState.IsValid)
			{
                var leagueSeason = _seasonRepo.GetLeagueSeasonById(model.LeagueSeasonId);
                var teamsForLeagueSeason = _teamRepo.GetTeamsByLeagueSeasonId(model.LeagueSeasonId).ToList();

                var roundsWithGames = GenerateRoundsAndGames(teamsForLeagueSeason, leagueSeason).ToList();

                _roundRepo.AddRange(roundsWithGames);
            }

			return null;
		}

		[HttpPost]
		public ActionResult Edit(Round model)
		{
			if (ModelState.IsValid)
			{
				_roundRepo.Update(model);
			}

			return null;
		}

		[HttpGet]
		public JsonResult Delete(Guid roundId)
		{
			var status = "success";
			var message = "";
			try
			{
				_roundRepo.Delete(_roundRepo.GetById(roundId));
			}
			catch (Exception ex)
			{
				Logger.Error(typeof(LeagueManagementController), "Unsuccessfull delete action", ex);
				status = "failed";
				if (ex.GetType() == typeof(UpdateException))
				{
					message = ex.Message;
				}
				else
				{
					message = "Nešto je pošlo naopako, molim vas kontaktirajte administratora ili pokušajte ponovo.";
				}
			}

			return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
		}
        #endregion

        #region [Helper methods]
        private IEnumerable<Round> GenerateRoundsAndGames(List<Team> teams, LeagueSeason leagueSeason)
        {
            // TODO: Optimize algorithm in order to support double Round Robin brackets 
            if (teams.Count % 2 != 0)
            {
                // TODO: Add support for odd number of teams if required. (Add empty bye team or use default empty team from the DB because of FK constraints)
                var byeTeam = new Team
                {
                    Id = Guid.NewGuid(),
                    LeagueSeasonId = leagueSeason.Id,
                    TeamName = "BYE team"
                };

                //teams.Add(byeTeam);
                yield break; // TODO: Remove when proper odd team number support is defined
            }

            var numberOfRounds = teams.Count - 1;
            var numberOfGamesPerRound = teams.Count / 2;

            var rotatedTeams = new List<Team>();
            rotatedTeams.AddRange(teams.Skip(numberOfGamesPerRound).Take(numberOfGamesPerRound));
            rotatedTeams.AddRange(teams.Skip(1).Take(numberOfGamesPerRound - 1).ToArray().Reverse());

            var numberOfTeams = rotatedTeams.Count;

            for (var roundNumber = 0; roundNumber < numberOfRounds; roundNumber++)
            {
                var games = new List<Game>();

                var round = new Round
                {
                    Id = Guid.NewGuid(),
                    Games = new List<Game>(),
                    LeagueSeasonId = leagueSeason.Id,
                    RoundName = string.Format("{0}", roundNumber + 1)
                };

                var teamIndex = roundNumber % numberOfTeams;

                games.Add(new Game
                {
                    Id = Guid.NewGuid(),
                    RoundId = round.Id,
                    SeasonId = leagueSeason.Season.Id,
                    TeamAId = teams[0].Id,
                    TeamBId = rotatedTeams[teamIndex].Id,
                    DateTime = DateTime.Now // TODO: DateTime is required at the moment, either remove it or set default time here
                });

                for (var index = 1; index < numberOfGamesPerRound; index++)
                {
                    var teamAIndex = (roundNumber + index) % numberOfTeams;
                    var teamBIndex = (roundNumber + numberOfTeams - index) % numberOfTeams;

                    games.Add(new Game
                    {
                        Id = Guid.NewGuid(),
                        RoundId = round.Id,
                        SeasonId = leagueSeason.Season.Id,
                        TeamAId = rotatedTeams[teamBIndex].Id,
                        TeamBId = rotatedTeams[teamAIndex].Id,
                        DateTime = DateTime.UtcNow // TODO: DateTime is required at the moment, either remove it or set default time here
                    });
                }
                round.Games = games;
                yield return round;
            }
        }
        #endregion
    }
}