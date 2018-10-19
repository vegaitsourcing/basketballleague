using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Data.Entity.Core;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
	public class RefereeManagementController : RenderMvcController
	{
		private IRefereeRepository _refereeRepo;
		public RefereeManagementController(IRefereeRepository refereeRepo)
		{
			_refereeRepo = refereeRepo;
		}


		public ActionResult Index(RefereeManagementModel model)
		{
			model.Referees = _refereeRepo.GetAll();

			return View(model);
		}
	}

	public class RefereeManagementSurfaceController : SurfaceController
	{
		private IRefereeRepository _refereeRepo;

		public RefereeManagementSurfaceController(IRefereeRepository refereeRepo)
		{
			_refereeRepo = refereeRepo;
		}

		#region [Render Views Actions]
		[HttpGet]
		public ActionResult Add()
		{
			return PartialView(new Referee());
		}
		[HttpGet]
		public ActionResult Edit(Guid refereeId)
		{
			return PartialView(_refereeRepo.GetById(refereeId));
		}
		#endregion

		#region [Data Change Actions]

		[HttpPost]
		public ActionResult Add(Referee model)
		{
			if (ModelState.IsValid)
			{
				_refereeRepo.Add(model);
			}

			return PartialView(model);
		}

		[HttpPost]
		public ActionResult Edit(Referee model)
		{
			if (ModelState.IsValid)
			{
				_refereeRepo.Update(model);
			}

			return PartialView(model);
		}

		[HttpGet]
		public JsonResult Delete(Guid refereeId)
		{
			var status = "success";
			var message = "";
			try
			{
				_refereeRepo.Delete(_refereeRepo.GetById(refereeId));
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