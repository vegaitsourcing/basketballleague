using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Surface
{
    public class SeasonsController : SurfaceController
    {
        private ISeasonRepository _seasonRepo;

        public SeasonsController(ISeasonRepository seasonRepo)
        {
            _seasonRepo = seasonRepo;
        }

        [HttpPost]
        public ActionResult AddSeason(Season season)
        {
            var addedItem = _seasonRepo.Add(season);
            return RedirectToCurrentUmbracoPage("?id=" + addedItem.Id);
        }

        [HttpPost]
        public ActionResult EditSeason(Season season)
        {
            _seasonRepo.Update(season);
            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult DeleteSeason(Guid id)
        {
            var league = _seasonRepo.GetById(id);
            _seasonRepo.Delete(league);
            return RedirectToCurrentUmbracoPage();
        }
    }
}
