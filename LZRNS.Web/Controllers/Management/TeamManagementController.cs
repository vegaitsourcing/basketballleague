using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using LZRNS.Web.Helpers;
using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
    [MemberAuthorize]
    public class TeamManagementController : RenderMvcController
    {
        private readonly ITeamRepository _teamRepo;

        public TeamManagementController(ITeamRepository teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public ActionResult Index(TeamManagementModel model)
        {
            model.Teams = _teamRepo.GetAll().ToList();

            return View(model);
        }
    }

    [MemberAuthorize]
    public class TeamManagementSurfaceController : SurfaceController
    {
        private readonly ITeamRepository _teamRepo;
        private readonly ISeasonRepository _seasonRepo;
        private readonly IPlayerRepository _playerRepo;

        public TeamManagementSurfaceController(ITeamRepository teamRepo, ISeasonRepository seasonRepo, IPlayerRepository playerRepo)
        {
            _teamRepo = teamRepo;
            _seasonRepo = seasonRepo;
            _playerRepo = playerRepo;
        }

        #region [Render Views Actions]

        [HttpGet]
        public ActionResult Add()
        {
            var model = new Team
            {
                Teams = _teamRepo.GetAll()
                .ToList()
                .Select(x => new SelectListItem()
                {
                    Text = $"{x.TeamName} - {x.LeagueSeason.FullName}",
                    Value = x.Id.ToString()
                }),
                LeagueSeasons = _seasonRepo.GetAllLeagueSeasons()
                .ToList()
                .Select(x => new SelectListItem()
                {
                    Text = x.FullName,
                    Value = x.Id.ToString()
                })
                .OrderBy(x => x.Text)
            };

            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid teamId)
        {
            var model = _teamRepo.GetById(teamId);

            model.Teams = _teamRepo.GetAll()
                .ToList()
                .Select(x => new SelectListItem()
                {
                    Text = $"{x.TeamName} - {x.LeagueSeason.FullName}",
                    Value = x.Id.ToString()
                });

            model.LeagueSeasons = _seasonRepo.GetAllLeagueSeasons()
                .ToList()
                .Select(x => new SelectListItem()
                {
                    Text = x.FullName,
                    Value = x.Id.ToString()
                })
                .OrderBy(x => x.Text);

            model.AvailablePlayers = _playerRepo.GetAll()
                .ToList()
                .Where(x => !(x.PlayersPerSeason.Count > 0) || x.PlayersPerSeason.Any(y => y.TeamId == model.PreviousTeamGuid))
                .Where(s => model.PlayersPerSeason.All(d => d.PlayerId != s.Id))
                .Select(z => new SelectListItem { Text = z.GetFullName, Value = z.Id.ToString() })
                .OrderBy(o => o.Text);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult DeleteTeamMember(Guid teamMemberId)
        {
            _teamRepo.DeletePlayerFromTeam(teamMemberId);

            return null;
        }

        #endregion [Render Views Actions]

        #region [Data Change Actions]

        [HttpPost]
        public ActionResult Add(Team model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _teamRepo.Add(model).Id;
                model.Image = ImageHandler.SaveImage(model, ObjectType.TEAM);
                _teamRepo.Update(model);
            }

            return null;
        }

        [HttpPost]
        public ActionResult AddPlayerToTeam(Guid playerId, Guid teamId)
        {
            _teamRepo.AddPlayerToTeam(playerId, teamId);

            return null;
        }

        [HttpPost]
        public ActionResult Edit(Team model)
        {
            if (!ModelState.IsValid)
                return null;

            ImageHandler.RemoveImage(model.Image);
            string newImage = ImageHandler.SaveImage(model, ObjectType.TEAM);
            if (newImage != null)
            {
                model.Image = newImage;
            }

            _teamRepo.Update(model);

            return null;
        }

        [HttpGet]
        public JsonResult Delete(Guid teamId)
        {
            string status = "success";
            string message = "";
            var model = _teamRepo.GetById(teamId);

            try
            {
                _teamRepo.Delete(model);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(LeagueManagementController), "Unsuccessfull delete action", ex);
                status = "failed";
                message = ex.GetType() == typeof(UpdateException) ? ex.Message : "Nešto je pošlo naopako, molim vas kontaktirajte administratora ili pokušajte ponovo.";
            }

            if (status == "success")
            {
                ImageHandler.RemoveImage(model.Image);
            }
            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
        }

        #endregion [Data Change Actions]
    }
}