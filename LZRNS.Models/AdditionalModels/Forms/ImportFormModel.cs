using System.ComponentModel.DataAnnotations;
using System.Web;

namespace LZRNS.Models.AdditionalModels.Forms
{
	public class ImportFormModel
	{
		[Required]
		public HttpPostedFileBase[] Files { get; set; }
	}
}
