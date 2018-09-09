using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Surface
{
    public class PlayersController : SurfaceController
    {
        private IPlayerRepository _playerRepo;

        public PlayersController(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }
        [HttpPost]
        public ActionResult AddPlayer(Player player)
        {
            var addedItem = _playerRepo.Add(player);
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult EditPlayer(Player player)
        {
            _playerRepo.Update(player);
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult DeletePlayer(Guid id)
        {
            var player = _playerRepo.GetById(id);
            _playerRepo.Delete(player);
            return RedirectToCurrentUmbracoPage();
            //return Redirect("/");
        }
    }
}