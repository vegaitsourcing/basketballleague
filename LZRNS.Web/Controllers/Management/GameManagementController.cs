﻿using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
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

	public class GameManagementSurfaceController : SurfaceController
	{
		private ISeasonRepository _seasonRepo;
		private IRoundRepository _roundRepo;
		private ITeamRepository _teamRepo;
		private IGameRepository _gameRepo;

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
		public ActionResult Edit(Guid roundId)
		{
			return PartialView(_roundRepo.GetById(roundId));
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
		public ActionResult Edit(Game model)
		{
			if (ModelState.IsValid)
			{
				_gameRepo.Update(model);
			}

			return null;
		}

		[HttpGet]
		public JsonResult Delete(Guid leagueId)
		{
			var status = "success";
			var message = "";
			try
			{
				_roundRepo.Delete(_roundRepo.GetById(leagueId));
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
	}
}