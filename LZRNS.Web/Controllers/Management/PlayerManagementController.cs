using LZRNS.Common;
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
        private IPlayerRepository _playerRepo;
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
        private IPlayerRepository _playerRepo;

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
        #endregion

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
            if (ModelState.IsValid)
            {

                ImageHandler.RemoveImage(model.Image);
                string newImage = ImageHandler.SaveImage(model, ObjectType.PLAYER);
                if (newImage != null)
                {
                    model.Image = newImage;
                }

                _playerRepo.Update(model);
            }

            return PartialView(model);
        }

        [HttpGet]
        public JsonResult Delete(Guid playerId)
        {
            var status = "success";
            var message = "";
            var model = _playerRepo.GetById(playerId);

            try
            {
                _playerRepo.Delete(model);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(LeagueManagementController), "Unsuccessfull delete action", ex);
                status = "failed";
                if (ex.GetType() == typeof(UpdateException))
                {
                    message = ex.Message;
                }
                else
                {
                    message = "Nešto je pošlo naopako, molim vas kontaktirajte administratora ili pokušajte ponovo.";
                }
            }

            if (status == "success")
            {
                ImageHandler.RemoveImage(model.Image);
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion



    }
}