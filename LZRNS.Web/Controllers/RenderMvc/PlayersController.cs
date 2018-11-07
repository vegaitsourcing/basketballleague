using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using LZRNS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class PlayersController : RenderMvcController
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public ActionResult Index(PlayersModel model, string q, string fl, bool activeOnly = true)
        {
            //model.CurrentShownLeague = !String.IsNullOrWhiteSpace(ln) ? ln : model.Leagues.FirstOrDefault();

            var viewModel = new PlayersViewModel()
            {
                PlayersModel = model,
                Players = _playerRepository.FilterPlayers(q, fl, activeOnly),
                CurrentQuery = q,
                CurrentFL = fl,
                ActiveOnly = activeOnly
            };

            return CurrentTemplate(viewModel);
        }
    }
}