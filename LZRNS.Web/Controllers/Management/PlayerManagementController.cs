﻿using LZRNS.Common;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using LZRNS.Web.Helpers;
using System;
using System.Data.Entity.Core;
using System.IO;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
    [MemberAuthorize]
    public class PlayerManagementController : RenderMvcController
    {
        private readonly IPlayerRepository _playerRepo;

        public PlayerManagementController(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }

        public ActionResult Index(PlayerManagementModel model)
        {
            model.Players = _playerRepo.GetAll();

            return View(model);
        }
    }

    [MemberAuthorize]
    public class PlayerManagementSurfaceController : SurfaceController
    {
        private readonly IPlayerRepository _playerRepo;

        public PlayerManagementSurfaceController(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }

        #region [Render Views Actions]

        [HttpGet]
        public ActionResult Add()
        {
            return PartialView(new Player());
        }

        [HttpGet]
        public ActionResult Edit(Guid playerId)
        {
            return PartialView(_playerRepo.GetById(playerId));
        }

        #endregion [Render Views Actions]

        #region [Data Change Actions]

        [HttpPost]
        public ActionResult Add(Player model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _playerRepo.Add(model).Id;

                model.Image = ImageHandler.SaveImage(model, ObjectType.PLAYER);
                _playerRepo.Update(model);
            }

            return null;
        }

        [HttpPost]
        public ActionResult Edit(Player model)
        {
			Player player = _playerRepo.GetById(model.Id);
			player.ImageFile = model.ImageFile;
			if (ModelState.IsValid)
            {
                ImageHandler.RemoveImage(player, ObjectType.PLAYER);
                string newImage = ImageHandler.SaveImage(player, ObjectType.PLAYER);
                if (newImage != null)
                {
					player.Image = newImage;
                }
                _playerRepo.Update(player);
            }

            return PartialView(player);
        }

        [HttpGet]
        public JsonResult Delete(Guid playerId)
        {
            string status = "success";
            string message = "";
            Player model = _playerRepo.GetById(playerId);

            try
            {
                _playerRepo.Delete(model);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(LeagueManagementController), "Unsuccessfull delete action", ex);
                status = "failed";
                message = ex.GetType() == typeof(UpdateException) ? ex.Message : "Nešto je pošlo naopako, molim vas kontaktirajte administratora ili pokušajte ponovo.";
            }

            if (status == "success")
            {
                ImageHandler.RemoveImage(model, ObjectType.PLAYER);
            }

            return Json(new { status, message }, JsonRequestBehavior.AllowGet);
        }

        #endregion [Data Change Actions]
    }
}