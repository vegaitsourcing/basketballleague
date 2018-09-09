using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class PlayerManagmentController : RenderMvcController
    {
        private IPlayerRepository _playerRepo;
        public PlayerManagmentController(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }

        public ActionResult Index(PlayerManagmentModel model)
        {
            var viewModel = _playerRepo.GetAll();
            return CurrentTemplate(viewModel);
        }
    }
}