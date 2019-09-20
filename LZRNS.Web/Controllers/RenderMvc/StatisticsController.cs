using LZRNS.Common;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.AdditionalModels;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

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

            TopStatisticCategories category;
            Enum.TryParse(Request[Constants.RequestParameters.StatisticsCategory], true, out category);

            model.Category = category;

            return CurrentTemplate(model);
        }
    }
}