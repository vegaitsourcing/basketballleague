using LZRNS.Common.Extensions;
using System.Web.Mvc;

namespace LZRNS.Web.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Returns name of the controller class after stripping "controller" suffix from it.
		/// </summary>
		/// <param name="controllerName">The name of the controller class.</param>
		/// <returns>Name without "controller" suffix.</returns>
		public static string RemoveControllerSuffix(this string controllerName)
		{
			return controllerName.RemoveSuffix(nameof(Controller));
		}
	}
}
