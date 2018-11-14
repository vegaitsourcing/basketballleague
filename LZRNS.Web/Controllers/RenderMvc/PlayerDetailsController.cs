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
    public class PlayerDetailsController : RenderMvcController
    {
        private readonly IPlayerRepository _repository;

        public PlayerDetailsController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index(PlayerDetailsModel model, Guid? id)
        {
            var player = id.HasValue ?  _repository.GetById(id.Value) : null;

            var viewModel = new PlayerDetailsViewModel()
            {
                PlayerDetailsModel = model,
                CurrentPlayer = player
            };

            return CurrentTemplate(viewModel);
        }
    }
}