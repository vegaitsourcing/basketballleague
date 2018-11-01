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
	public class GameManagementController : RenderMvcController
	{
		private ILeagueRepository _leagueRepo;
		private ISeasonRepository _seasonRepo;

		public GameManagementController(ILeagueRepository leagueRepo, ISeasonRepository seasonRepo)
		{
			_leagueRepo = leagueRepo;
			_seasonRepo = seasonRepo;
		}


		public ActionResult Index(GameManagementModel model)
		{
			model.Seasons = _seasonRepo.GetAll()
				.Select(x => new SelectListItem()
				{
					Text = x.Name,
					Value = x.Id.ToString()
				})
				.OrderByDescending(x => x.Text);

			return View(model);
		}
	}

	[MemberAuthorize]
	public class GameManagementSurfaceController : SurfaceController
	{
		private readonly ISeasonRepository _seasonRepo;
		private readonly IRoundRepository _roundRepo;
		private readonly ITeamRepository _teamRepo;
		private readonly IGameRepository _gameRepo;

		public GameManagementSurfaceController(ISeasonRepository seasonRepo, IRoundRepository roundRepo, ITeamRepository teamRepo, IGameRepository gameRepo)
		{
			_seasonRepo = seasonRepo;
			_roundRepo = roundRepo;
			_teamRepo = teamRepo;
			_gameRepo = gameRepo;
		}

		#region [Render Views Actions]
		[HttpGet]
		public ActionResult Add(Guid leagueSeasonId, Guid roundId)
		{
			var model = new Game();
			var leagueSeason = _seasonRepo.GetLeagueSeasonById(leagueSeasonId);
			model.SeasonId = leagueSeason.SeasonId;
			model.RoundId = roundId;

			model.Teams = _teamRepo.GetAll()
				.ToList()
				.Where(x => x.LeagueSeasonId.Equals(leagueSeasonId))
				.Select(x => new SelectListItem()
				{
					Text = x.TeamName,
					Value = x.Id.ToString()
				});

			return PartialView(model);
		}
		[HttpGet]
		public ActionResult Edit(Guid gameId)
		{
			var model = _gameRepo.GetById(gameId);

			model.Teams = _teamRepo.GetAll()
				.ToList()
				.Where(x => x.LeagueSeasonId.Equals(model.Round.LeagueSeasonId))
				.Select(x => new SelectListItem()
				{
					Text = x.TeamName,
					Value = x.Id.ToString()
				});

			model.TeamA.AvailablePlayers = GetPlayerList(model.TeamA.PlayersPerSeason, gameId);
			model.TeamB.AvailablePlayers = GetPlayerList(model.TeamB.PlayersPerSeason, gameId);

			return PartialView(model);
		}

		private List<SelectListItem> GetPlayerList(ICollection<PlayerPerTeam> players, Guid gameId)
		{
			var result = new List<SelectListItem>();

			foreach (var player in players.OrderBy(x => x.Player.GetFullName))
			{
				var stat = player.Player.Stats.FirstOrDefault(z => z.GameId.Equals(gameId));
				var selected = stat != null;

				result.Add(new SelectListItem()
				{
					Selected = selected,
					Text = player.Player.GetFullName,
					Value = selected ? stat.Id.ToString() : player.PlayerId.ToString()
				});
			}

			return result;
		}

		[HttpGet]
		public ActionResult SeasonSelector(Guid seasonId)
		{
			var leagueSeasons = _seasonRepo.GetAllLeagueSeasons()
				.Where(x => x.SeasonId.Equals(seasonId))
				.ToList()
				.Select(y => new SelectListItem()
				{
					Text = y.FullName,
					Value = y.Id.ToString()
				})
				.OrderBy(z => z.Text);

			return PartialView(leagueSeasons);
		}
		[HttpGet]
		public ActionResult RoundSelector(Guid leagueSeasonId)
		{
			var rounds = _roundRepo.GetAll()
							.Where(x => x.LeagueSeasonId.Equals(leagueSeasonId))
							.ToList()
							.Select(y => new SelectListItem()
							{
								Text = y.RoundName,
								Value = y.Id.ToString()
							})
							.OrderBy(z => z.Text);

			return PartialView(rounds);
		}

		[HttpGet]
		public ActionResult GamesTable(Guid roundId)
		{
			var games = _gameRepo.GetAll()
							.Where(x => x.RoundId.Equals(roundId))
							.ToList();

			return PartialView(games);
		}
		#endregion

		#region [Data Change Actions]

		[HttpPost]
		public ActionResult Add(Game model)
		{
			if (ModelState.IsValid)
			{
				_gameRepo.Add(model);
			}

			return null;
		}

		[HttpPost]
		public JsonResult EditPlayerStats(Stats model)
		{
			ModelState.Clear();
			if (TryValidateModel(model))
			{
				_gameRepo.UpdateStatsForPlayerInGame(model);
			}

			return Json(new { gameId = model.GameId }, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		public ActionResult Edit(Game model)
		{
			if (ModelState.IsValid)
			{
				_gameRepo.Update(model);
			}

			return null;
		}

		[HttpGet]
		public JsonResult Delete(Guid gameId)
		{
			var status = "success";
			var message = "";
			try
			{
				_gameRepo.Delete(_gameRepo.GetById(gameId));
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

		[HttpPost]
		public JsonResult AddPlayerStats(Stats model)
		{
			if (ModelState.IsValid)
			{
				_gameRepo.AddStatsForPlayerInGame(model);
			}

			return Json(new { gameId = model.GameId });
		}

		[HttpGet]
		public ActionResult AddPlayerStats(Guid gameId, Guid playerId)
		{
			var model = new Stats
			{
				GameId = gameId,
				PlayerId = playerId
			};

			return PartialView(model);
		}

		[HttpGet]
		public JsonResult DeletePlayerStats(Guid gameId, Guid playerId)
		{
				_gameRepo.DeleteStatsForPlayerInGame(playerId);

			return Json(new { gameId = gameId }, JsonRequestBehavior.AllowGet);
		}

		#endregion
	}
}