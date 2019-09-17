using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.DomainModels.Repository.Interfaces.Exceptions;

using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Data.Entity.Core;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
    public class LeagueManagementController : RenderMvcController
    {
        private readonly ILeagueRepository _leagueRepo;

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
        private readonly ISeasonRepository _seasonRepo;

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

        #endregion [Render Views Actions]

        #region [Data Change Actions]

        [HttpPost]
        public ActionResult Add(League model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            try
            {
                _leagueRepo.Add(model);
            }
            catch (DalException ex)
            {
                string message = ex.Message;
                Logger.Error(typeof(LeagueManagementController), "Neuspesno dodavanje lige.", ex);
                ViewBag.Hello = "Hello";
                TempData["League_Name_Error"] = message;
                ModelState.AddModelError("error_msg", message);
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
            string status = "success";
            string message = "";
            try
            {
                _leagueRepo.Delete(_leagueRepo.GetById(leagueId));
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(LeagueManagementController), "Unsuccessfull delete action", ex);
                status = "failed";
                message = ex.GetType() == typeof(UpdateException) ? ex.Message : "Nešto je pošlo naopako, molim vas kontaktirajte administratora ili pokušajte ponovo.";
            }

            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
        }

        #endregion [Data Change Actions]
    }
}