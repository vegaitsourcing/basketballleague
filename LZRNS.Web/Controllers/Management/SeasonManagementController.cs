using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LZRNS.DomainModel.Models;
using LZRNS.Web.Controllers.Surface;
using System;
using System.Linq;
using LZRNS.DomainModels.Models;
using System.Collections.Generic;
using System.Data.Entity.Core;

namespace LZRNS.Web.Controllers.Management
{
	public class SeasonManagementController : RenderMvcController
	{
		private ISeasonRepository _seasonRepo;
		public SeasonManagementController(ISeasonRepository seasonRepo)
		{
			_seasonRepo = seasonRepo;
		}


		public ActionResult Index(SeasonManagementModel model)
		{
			model.Seasons = _seasonRepo.GetAll();

			return View(model);
		}
	}

	public class SeasonManagementSurfaceController : BaseSurfaceController
	{
		private ISeasonRepository _seasonRepo;
		private ILeagueRepository _leagueRepo;

		public SeasonManagementSurfaceController(ISeasonRepository seasonRepo, ILeagueRepository leagueRepo)
		{
			_seasonRepo = seasonRepo;
			_leagueRepo = leagueRepo;
		}

		#region [Season Actions]

		#region [Get Actions]

		[HttpGet]
		public ActionResult Add()
		{
			return PartialView(new Season());
		}

		[HttpGet]
		public ActionResult Edit(Guid seasonId)
		{
			var model = _seasonRepo.GetById(seasonId);
			var allLeagues = _leagueRepo.GetAll().ToList();
			model.LeagueList = new List<SelectListItem>();

			foreach(var league in allLeagues)
			{
				var existingLeagueSeason = model.LeagueSeasons
						.Where(y => y.LeagueId == league.Id)
						.FirstOrDefault();

				model.LeagueList.Add(new SelectListItem
				{
					Selected = existingLeagueSeason != null,
					Text = league.Name,
					Value = existingLeagueSeason != null ? existingLeagueSeason.Id.ToString() : league.Id.ToString()
				});
			}

			model.LeagueList = model.LeagueList
				.OrderBy(x => x.Text)
				.ToList();

			return PartialView(model);
		}

		[HttpGet]
		public JsonResult Delete(Guid seasonId)
		{
			var status = "success";
			var message = "";
			try
			{
				_seasonRepo.Delete(_seasonRepo.GetById(seasonId));
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

		#region [Post Actions]

		[HttpPost]
		public ActionResult Add(Season model)
		{
			if (ModelState.IsValid)
			{
				_seasonRepo.Add(model);
			}

			return PartialView(model);
		}

		[HttpPost]
		public ActionResult Edit(Season model)
		{
			if (ModelState.IsValid)
			{
				_seasonRepo.Update(model);
			}

			return PartialView(model);
		}

		#endregion

		#endregion


		[HttpGet]
		public ActionResult EditLeagueSeason(Guid id)
		{
			var model = _seasonRepo.GetLeagueSeasonById(id);
			return PartialView(_seasonRepo.GetLeagueSeasonById(id));
		}

		[HttpPost]
		public JsonResult EditLeagueSeason(LeagueSeason model)
		{
			if (ModelState.IsValid)
			{
				_seasonRepo.UpdateLeagueSeason(model);
			}

			return Json(new { seasonId = model.SeasonId });
		}

		[HttpGet]
		public ActionResult AddLeagueSeason(Guid seasonId, Guid leagueId)
		{
			var model = new LeagueSeason();

			model.SeasonId = seasonId;
			model.LeagueId = leagueId;

			return PartialView(model);
		}

		[HttpPost]
		public JsonResult AddLeagueSeason(LeagueSeason model)
		{
			if (ModelState.IsValid)
			{
				_seasonRepo.AddLeagueToSeason(model);
			}

			return Json(new { seasonId = model.SeasonId });
		}

		[HttpGet]
		public JsonResult DeleteLeagueSeason(Guid leagueSeasonId)
		{
			var leagueSeason = _seasonRepo.GetLeagueSeasonById(leagueSeasonId);

			_seasonRepo.DeleteLeagueSeason(leagueSeason);

			return Json(new { seasonId = leagueSeason.SeasonId}, JsonRequestBehavior.AllowGet);
		}
	}
}