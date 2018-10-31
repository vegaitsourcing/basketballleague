using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
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

		private ISeasonRepository _seasonRepo;

		public ManagementController(ISeasonRepository seasonRepo)
		{
			_seasonRepo = seasonRepo;
		}

		public ActionResult Index(RenderModel renderModel)
		{
			ManagementModel model = new ManagementModel(renderModel.Content);

			if (!Members.IsLoggedIn() || !Members.MemberHasAccess(model.Content.Path))
			{
				return View(LoginViewPath, model);
			}
			
			model.ResultsRoundGames = _seasonRepo.GetSeasonByYear(model.StatisticsSettings.SeasonYearStart)
				.LeagueSeasons.First( /*TODO: INSERT LEAGUE ID PLS*/)
				.Rounds.Where(x => x.RoundName.Equals(model.StatisticsSettings.ResultsRound))
				.SelectMany(y => y.Games)
				.ToList();

			model.ScheduleRoundGames = _seasonRepo.GetSeasonByYear(model.StatisticsSettings.SeasonYearStart)
				.LeagueSeasons.First( /*TODO: INSERT LEAGUE ID PLS*/)
				.Rounds.Where(x => x.RoundName.Equals(model.StatisticsSettings.ScheduleRound))
				.SelectMany(y => y.Games)
				.ToList();

			return View(IndexViewPath, model);
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