using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
    public class ManagementController : SurfaceController, IRenderMvcController
    {
        public const string IndexViewPath = "Management/Index";
        public const string LoginViewPath = "Management/Login";

        private readonly ISeasonRepository _seasonRepo;

        public ManagementController(ISeasonRepository seasonRepo)
        {
            _seasonRepo = seasonRepo;
        }

        public ActionResult Index(RenderModel renderModel)
        {
            var model = new ManagementModel(renderModel.Content);

            if (!Members.IsLoggedIn() || !Members.MemberHasAccess(model.Content.Path))
            {
                return View(LoginViewPath, model);
            }

            var firstLeagueSeason = GetFirstLeagueSeasonByYear(model.StatisticsSettings.SeasonYearStart);

            model.ResultsRoundGames = GetGamesByRoundName(firstLeagueSeason, model.StatisticsSettings.ResultsRound).ToList();
            model.ScheduleRoundGames = GetGamesByRoundName(firstLeagueSeason, model.StatisticsSettings.ScheduleRound).ToList();

            return View(IndexViewPath, model);
        }

        private static IEnumerable<Game> GetGamesByRoundName(LeagueSeason season, string roundName)
        {
            return season?.Rounds?.Where(x => x.RoundName.Equals(roundName)).SelectMany(y => y.Games) ?? new List<Game>();
        }

        private LeagueSeason GetFirstLeagueSeasonByYear(int year)
        {
            var startYearSeason = _seasonRepo.GetSeasonByYear(year);
            var leagueSeasons = startYearSeason?.LeagueSeasons ?? new List<LeagueSeason>();
            return leagueSeasons.FirstOrDefault();
        }

        [ChildActionOnly]
        public ActionResult LoginForm()
        {
            return PartialView(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleLogin(LoginModel model)
        {
            if (ModelState.IsValid && Members.Login(model.Username, model.Password))
            {
                return RedirectToCurrentUmbracoPage();
            }

            ModelState.AddModelError("", "Neispravno korisnicko ime ili lozinka");

            return CurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            if (Members.IsLoggedIn())
            {
                Members.Logout();
            }

            return RedirectToCurrentUmbracoPage();
        }
    }
}