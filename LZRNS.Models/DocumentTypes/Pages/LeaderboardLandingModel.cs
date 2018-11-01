using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.DocumentTypes.Nodes;
using LZRNS.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class LeaderboardLandingModel : PageModel
	{
		public LeaderboardLandingModel()
		{
		}

		public LeaderboardLandingModel(IPublishedContent content) : base(content)
		{
		}

		public LeaderboardLandingModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}
		
		public string RoundName => this.GetCachedValue(() =>
			UmbracoHelper.GetSingleContentOfType<StatisticsSettingsModel>()?.ResultsRound) ?? "1";

		public int SeasonYearStart => this.GetCachedValue(() =>
			UmbracoHelper.GetSingleContentOfType<StatisticsSettingsModel>()?.SeasonYearStart) ?? DateTime.Now.Year;

		public IEnumerable<string> Leagues => this.GetPropertyValue<IEnumerable<string>>();

		public IList<string> AllRounds { get; set; }
		public string CurrentShownLeague { get; set; }
		public string CurrentShownRound { get; set; }
		public int CurrentShownRoundIndex => 
			this.AllRounds?.IndexOf(this.CurrentShownRound) ?? 0;

		public string NextRound =>
			HasNextRound ? this.AllRounds[CurrentShownRoundIndex + 1] : string.Empty;
		public string PreviousRound => 
			HasPreviousRound ? this.AllRounds[CurrentShownRoundIndex - 1] : string.Empty;

		public bool HasNextRound => 
			CurrentShownRoundIndex + 1 < (this.AllRounds?.Count ?? 0);
		public bool HasPreviousRound => 
			CurrentShownRoundIndex > 0;
	}
}
