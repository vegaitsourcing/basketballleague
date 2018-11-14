using LZRNS.Common;

namespace LZRNS.Web.Extensions
{
	public static class IntExtensions
	{
		public static string ToSeasonQueryParameter(this int seasonYear)
		{
			return $"{Constants.RequestParameters.StatisticsSeason}={seasonYear}";
		}
	}
}