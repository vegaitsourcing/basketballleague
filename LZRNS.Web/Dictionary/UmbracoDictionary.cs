using LZRNS.DomainModels.Models;
using System;

namespace LZRNS.Web.Dictionary
{
	public static class UmbracoDictionary
	{
		public static class Shared
		{
			public static string Round => umbraco.library.GetDictionaryItem("Shared.Round");
		}

		public static class Stats
		{
			public static string Name => umbraco.library.GetDictionaryItem("Stats.Name");
			public static string Min => umbraco.library.GetDictionaryItem("Stats.Min");
			public static string Eff => umbraco.library.GetDictionaryItem("Stats.Eff");
			public static string TwoPts => umbraco.library.GetDictionaryItem("Stats.TwoPts");
			public static string ThreePts => umbraco.library.GetDictionaryItem("Stats.ThreePts");
			public static string Fg => umbraco.library.GetDictionaryItem("Stats.Fg");
			public static string Ft => umbraco.library.GetDictionaryItem("Stats.Ft");
			public static string Reb => umbraco.library.GetDictionaryItem("Stats.Reb");
			public static string Off => umbraco.library.GetDictionaryItem("Stats.Off");
			public static string Def => umbraco.library.GetDictionaryItem("Stats.Def");
			public static string Ast => umbraco.library.GetDictionaryItem("Stats.Ast");
			public static string To => umbraco.library.GetDictionaryItem("Stats.To");
			public static string Stl => umbraco.library.GetDictionaryItem("Stats.Stl");
			public static string Blk => umbraco.library.GetDictionaryItem("Stats.Blk");
			public static string Pts => umbraco.library.GetDictionaryItem("Stats.Pts");

			public static class Filter
			{
				public static string Active => umbraco.library.GetDictionaryItem("Stats.Filter.Active");
				public static string All => umbraco.library.GetDictionaryItem("Stats.Filter.All");

				public static class Categories
				{
					public static string Totals => umbraco.library.GetDictionaryItem("Stats.Filter.Categories.Totals");
					public static string Averages => umbraco.library.GetDictionaryItem("Stats.Filter.Categories.Averages");
					public static string PerMinute => umbraco.library.GetDictionaryItem("Stats.Filter.Categories.PerMinute");
				}
			}
		}

		public static class TopStats
		{
			public static string Points => umbraco.library.GetDictionaryItem("TopStats.Points");
			public static string Rebounds => umbraco.library.GetDictionaryItem("TopStats.Rebounds");
			public static string Assists => umbraco.library.GetDictionaryItem("TopStats.Assists");
			public static string Steals => umbraco.library.GetDictionaryItem("TopStats.Steals");
			public static string Blocks = umbraco.library.GetDictionaryItem("TopStats.Blocks");
			public static string Minutes = umbraco.library.GetDictionaryItem("TopStats.Minutes");
			public static string Efficiency = umbraco.library.GetDictionaryItem("TopStats.Efficiency");
			public static string FieldGoals = umbraco.library.GetDictionaryItem("TopStats.FieldGoals");
			public static string TwoPoints = umbraco.library.GetDictionaryItem("TopStats.TwoPoints");
			public static string ThreePoints = umbraco.library.GetDictionaryItem("TopStats.ThreePoints");
			public static string FreeThrows = umbraco.library.GetDictionaryItem("TopStats.FreeThrows");
			public static string OffensiveRebounds = umbraco.library.GetDictionaryItem("TopStats.OffensiveRebounds");
			public static string DefensiveRebounds = umbraco.library.GetDictionaryItem("TopStats.DefensiveRebounds");
			public static string Turnovers = umbraco.library.GetDictionaryItem("TopStats.Turnovers");
		}

		public static class Contact
		{
			public static string Success => umbraco.library.GetDictionaryItem("Contact.Success");
			public static string Error => umbraco.library.GetDictionaryItem("Contact.Error");
		}

		public static string GetTableName(TableTypes tableType)
		{
			if (!Enum.IsDefined(typeof(TableTypes), tableType)) return string.Empty;

			switch (tableType)
			{
				case TableTypes.Points:
					return TopStats.Points;
				case TableTypes.FieldGoals:
					return TopStats.FieldGoals;
				case TableTypes.ThreePoints:
					return TopStats.ThreePoints;
				case TableTypes.TwoPoints:
					return TopStats.TwoPoints;
				case TableTypes.FreeThrows:
					return TopStats.FreeThrows;
				case TableTypes.Assists:
					return TopStats.Assists;
				case TableTypes.Blocks:
					return TopStats.Blocks;
				case TableTypes.Steals:
					return TopStats.Steals;
				case TableTypes.Rebounds:
					return TopStats.Rebounds;
				case TableTypes.DefensiveRebounds:
					return TopStats.DefensiveRebounds;
				case TableTypes.OffensiveRebounds:
					return TopStats.OffensiveRebounds;
				case TableTypes.Efficiency:
					return TopStats.Efficiency;
				case TableTypes.Minutes:
					return TopStats.Minutes;
				case TableTypes.Turnovers:
					return TopStats.Turnovers;
				default:
					return string.Empty;
			}
		}
	}
}