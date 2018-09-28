using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Implementations;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Surface
{
    public class RefereesController : SurfaceController
    {
        private IRefereeRepository _playerRepo;

        public RefereesController(IRefereeRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }
        [HttpPost]
        public ActionResult AddReferee(Referee player)
        {
            var addedItem = _playerRepo.Add(player);
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult EditReferee(Referee player)
        {
            _playerRepo.Update(player);
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult DeleteReferee(Guid id)
        {
            var player = _playerRepo.GetById(id);
            _playerRepo.Delete(player);
            return RedirectToCurrentUmbracoPage();
            //return Redirect("/");
        }
    }
}