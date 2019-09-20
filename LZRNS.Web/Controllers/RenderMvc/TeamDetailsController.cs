using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.DomainModels.ViewModels;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class TeamDetailsController : RenderMvcController
    {
        private readonly ITeamRepository _teamRepo;

        public TeamDetailsController(ITeamRepository teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public ActionResult Index(TeamDetailsModel model, string id)
        {
            if (string.IsNullOrWhiteSpace(id)) Response.Redirect(model.Home.Url);
            Guid guid;
            if (!Guid.TryParse(id, out guid)) Response.Redirect(model.Home.Url);

            model.Team = new TeamDetails(_teamRepo.GetById(guid), model.RoundName);

            return CurrentTemplate(model);
        }
    }
}