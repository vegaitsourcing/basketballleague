using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Data.Entity.Core;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

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

	[MemberAuthorize]
	public class LeagueManagementSurfaceController : SurfaceController
	{
		private ILeagueRepository _leagueRepo;
		private ISeasonRepository _seasonRepo;

		public LeagueManagementSurfaceController(ILeagueRepository leagueRepo, ISeasonRepository seasonRepo)
		{
			_leagueRepo = leagueRepo;
			_seasonRepo = seasonRepo;
		}

		#region [Render Views Actions]
		[HttpGet]
		public ActionResult Add()
		{
			return PartialView(new League());
		}
		[HttpGet]
		public ActionResult Edit(Guid leagueId)
		{
			return PartialView(_leagueRepo.GetById(leagueId));
		}
		#endregion

		#region [Data Change Actions]

		[HttpPost]
		public ActionResult Add(League model)
		{
			if (ModelState.IsValid)
			{
				_leagueRepo.Add(model);
			}

			return PartialView(model);
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
		public JsonResult Delete(Guid leagueId)
		{
			var status = "success";
			var message = "";
			try
			{
				_leagueRepo.Delete(_leagueRepo.GetById(leagueId));
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