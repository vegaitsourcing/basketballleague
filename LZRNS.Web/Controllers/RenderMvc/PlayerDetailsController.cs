using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using LZRNS.Models.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;
using LZRNS.DomainModels.Models;
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
			var player = id.HasValue ? _repository.GetById(id.Value) : null;

			var perSeasons = player?.Stats.GroupBy(s => s.Game.SeasonId).ToArray();

			var totals = perSeasons?.Select(ps => new PlayerTotalStats(player.GetFullName, ps.ToArray())).ToList();
			var averages = perSeasons?.Select(ps => new PlayerAverageStats(player.GetFullName, ps.ToArray())).ToList();
			var perMinutes = perSeasons?.Select(ps => new PlayerPerMinuteStats(player.GetFullName, ps.ToArray())).ToList();
			
			var viewModel = new PlayerDetailsViewModel
			{
				PlayerDetailsModel = model,
				CurrentPlayer = player,
				TotalStats = totals,
				AverageStats = averages,
				PerMinuteStats = perMinutes
			};

			return CurrentTemplate(viewModel);
		}
	}
}