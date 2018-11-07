using System.Globalization;
using LZRNS.Models.AdditionalModels.Forms;
using LZRNS.Models.DocumentTypes.Compositions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class ImportModel : ManagementPageModel
	{
		public ImportModel()
		{
		}

		public ImportModel(IPublishedContent content) : base(content)
		{
		}

		public ImportModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		public ImportFormModel FormModel => new ImportFormModel();
	}
}
