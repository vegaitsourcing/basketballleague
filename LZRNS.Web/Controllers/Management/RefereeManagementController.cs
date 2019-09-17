using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Data.Entity.Core;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
    [MemberAuthorize]
    public class RefereeManagementController : RenderMvcController
    {
        private readonly IRefereeRepository _refereeRepo;

        public RefereeManagementController(IRefereeRepository refereeRepo)
        {
            _refereeRepo = refereeRepo;
        }

        public ActionResult Index(RefereeManagementModel model)
        {
            model.Referees = _refereeRepo.GetAll();

            return View(model);
        }
    }

    [MemberAuthorize]
    public class RefereeManagementSurfaceController : SurfaceController
    {
        private readonly IRefereeRepository _refereeRepo;

        public RefereeManagementSurfaceController(IRefereeRepository refereeRepo)
        {
            _refereeRepo = refereeRepo;
        }

        #region [Render Views Actions]

        [HttpGet]
        public ActionResult Add()
        {
            return PartialView(new Referee());
        }

        [HttpGet]
        public ActionResult Edit(Guid refereeId)
        {
            return PartialView(_refereeRepo.GetById(refereeId));
        }

        #endregion [Render Views Actions]

        #region [Data Change Actions]

        [HttpPost]
        public ActionResult Add(Referee model)
        {
            if (ModelState.IsValid)
            {
                _refereeRepo.Add(model);
            }

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(Referee model)
        {
            if (ModelState.IsValid)
            {
                _refereeRepo.Update(model);
            }

            return PartialView(model);
        }

        [HttpGet]
        public JsonResult Delete(Guid refereeId)
        {
            string status = "success";
            string message = "";
            try
            {
                _refereeRepo.Delete(_refereeRepo.GetById(refereeId));
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