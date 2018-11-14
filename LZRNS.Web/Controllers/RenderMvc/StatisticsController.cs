using LZRNS.Common.Extensions;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using LZRNS.Common;
using LZRNS.Models.AdditionalModels;
using Umbraco.Web.Mvc;
using WebGrease.Css.Extensions;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class StatisticsController : RenderMvcController
	{
		private readonly ISeasonRepository _seasonRepository;

		public StatisticsController(ISeasonRepository seasonRepository)
		{
			if (seasonRepository == null) throw new ArgumentException(nameof(seasonRepository));

			_seasonRepository = seasonRepository;
		}

		public ActionResult Index(StatisticsModel model)
		{
			model.Seasons = _seasonRepository.GetAll().OrderByDescending(s => s.SeasonStartYear).ToList();

			int seasonYear;
			if (!int.TryParse(Request[Constants.RequestParameters.StatisticsSeason], NumberStyles.Integer,
				CultureInfo.CurrentCulture, out seasonYear))
			{
				seasonYear = model.Seasons.FirstOrDefault()?.SeasonStartYear ?? 0;
			}

			model.SelectedSeasonYear = seasonYear;

			TopStatisticCategories category = TopStatisticCategories.Total;
			Enum.TryParse(Request[Constants.RequestParameters.StatisticsCategory], true, out category);

			model.Category = category;

			return CurrentTemplate(model);
		}
	}
}