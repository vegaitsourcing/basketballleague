using LZRNS.Common.Extensions;

namespace LZRNS.Models.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Returns name of the model class after stripping "Model" suffix from it.
		/// </summary>
		/// <param name="modelName">The name of the model class.</param>
		/// <returns>Name without "Model" suffix.</returns>
		public static string RemoveModelSuffix(this string modelName)
		{
			return modelName.RemoveSuffix("Model");
		}
	}
}
