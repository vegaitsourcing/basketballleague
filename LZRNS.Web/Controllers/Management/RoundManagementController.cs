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

	public class RoundManagementSurfaceController : SurfaceController
	{
		private IRoundRepository _roundRepo;
		private ISeasonRepository _seasonRepo;

		public RoundManagementSurfaceController(IRoundRepository roundRepo, ISeasonRepository seasonRepo)
		{
			_roundRepo = roundRepo;
			_seasonRepo = seasonRepo;
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
				_roundRepo.Add(model);
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
	}
}