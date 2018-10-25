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
			public static string Ft => umbraco.library.GetDictionaryItem("Stats.Ft");
			public static string Reb => umbraco.library.GetDictionaryItem("Stats.Reb");
			public static string Off => umbraco.library.GetDictionaryItem("Stats.Off");
			public static string Def => umbraco.library.GetDictionaryItem("Stats.Def");
			public static string Ast => umbraco.library.GetDictionaryItem("Stats.Ast");
			public static string To => umbraco.library.GetDictionaryItem("Stats.To");
			public static string Stl => umbraco.library.GetDictionaryItem("Stats.Stl");
			public static string Blk => umbraco.library.GetDictionaryItem("Stats.Blk");
			public static string Pts => umbraco.library.GetDictionaryItem("Stats.Pts");
		}

		public static class TopStats
		{
			public static string Points => umbraco.library.GetDictionaryItem("TopStats.Points");
			public static string Rebounds => umbraco.library.GetDictionaryItem("TopStats.Rebounds");
			public static string Assists => umbraco.library.GetDictionaryItem("TopStats.Assists");
			public static string Steals => umbraco.library.GetDictionaryItem("TopStats.Steals");
		}

		public static class Contact
		{
			public static string Success => umbraco.library.GetDictionaryItem("Contact.Success");
			public static string Error => umbraco.library.GetDictionaryItem("Contact.Error");
		}
	}
}