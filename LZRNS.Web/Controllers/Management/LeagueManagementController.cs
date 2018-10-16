using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LZRNS.DomainModel.Models;
using LZRNS.Web.Controllers.Surface;
using System;

namespace LZRNS.Web.Controllers.Management
{
	public class LeagueManagementController : RenderMvcController
	{
		private ILeagueRepository _leagueRepo;
		public LeagueManagementController(ILeagueRepository leagueRepo)
		{
			_leagueRepo = leagueRepo;
		}


		public ActionResult Index(LeagueManagementModel model)
		{
			model.Leagues = _leagueRepo.GetAll();

			return View(model);
		}
	}

	public class LeagueManagementSurfaceController : BaseSurfaceController
	{
		private ILeagueRepository _leagueRepo;
		public LeagueManagementSurfaceController(ILeagueRepository leagueRepo)
		{
			_leagueRepo = leagueRepo;
		}

		[HttpGet]
		public ActionResult Add()
		{
			return PartialView(new League());
		}
		[HttpPost]
		public ActionResult Add(League model)
		{
			if (ModelState.IsValid)
			{
				_leagueRepo.Add(model);
			}

			return PartialView(model);
		}

		[HttpGet]
		public ActionResult Edit(Guid leagueId)
		{
			return PartialView(_leagueRepo.GetById(leagueId));
		}
		[HttpPost]
		public ActionResult Edit(League model)
		{
			if (ModelState.IsValid)
			{
				_leagueRepo.Update(model);
			}

			return PartialView(model);
		}

		[HttpGet]
		public ActionResult Delete(Guid leagueId)
		{
			_leagueRepo.Delete(_leagueRepo.GetById(leagueId));
			return null;
		}
	}
}