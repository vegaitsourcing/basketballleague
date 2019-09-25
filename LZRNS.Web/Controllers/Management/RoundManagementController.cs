using LZRNS.Core;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using LZRNS.Models.ViewModel;
using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
    [MemberAuthorize]
    public class RoundManagementController : RenderMvcController
    {
        private readonly IRoundRepository _roundRepo;

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
        private readonly IRoundGenerator _roundGenerator;

        public RoundManagementSurfaceController(IRoundRepository roundRepo, ISeasonRepository seasonRepo, ITeamRepository teamRepo, IRoundGenerator roundGenerator)
        {
            _roundRepo = roundRepo;
            _seasonRepo = seasonRepo;
            _teamRepo = teamRepo;
            _roundGenerator = roundGenerator;
        }

        #region [Render Views Actions]

        [HttpGet]
        public ActionResult Add()
        {
            var model = new RoundManagementSurfaceViewModel
            {
                LeagueSeasons = GetLeagueSeasonsAsListItems()
            };

            return PartialView(model);
        }

        private System.Collections.Generic.IEnumerable<SelectListItem> GetLeagueSeasonsAsListItems()
        {
            return _seasonRepo.GetAllLeagueSeasons().ToList().Select(x => new SelectListItem() { Text = x.FullName, Value = x.Id.ToString() });
        }

        [HttpGet]
        public ActionResult Edit(Guid roundId)
        {
            var model = _roundRepo.GetById(roundId);
            model.LeagueSeasons = GetLeagueSeasonsAsListItems();
            return PartialView(model);
        }

        #endregion [Render Views Actions]

        #region [Data Change Actions]

        [HttpPost]
        public ActionResult Add(RoundManagementSurfaceViewModel model)
        {
            if (!ModelState.IsValid) return null;

            var leagueSeason = _seasonRepo.GetLeagueSeasonById(model.LeagueSeasonId);
            var teamsForLeagueSeason = _teamRepo.GetTeamsByLeagueSeasonId(model.LeagueSeasonId).ToList();

            var roundsWithGames = _roundGenerator.GenerateRoundsWithGames(teamsForLeagueSeason, leagueSeason, model.RoundScheduleOptions);

            _roundRepo.AddRange(roundsWithGames);

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
            string status = "success";
            string message = "";
            try
            {
                _roundRepo.Delete(_roundRepo.GetById(roundId));
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