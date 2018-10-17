using LZRNS.DomainModel.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.Extensions;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class SeasonManagementModel : ManagementPageModel
	{
		public IPublishedContent StatsManagmentPicker => this.GetCachedValue(() => Content.GetPropertyValue<IPublishedContent>());

		public IPublishedContent ScheduleManagmentPicker => this.GetCachedValue(() => Content.GetPropertyValue<IPublishedContent>());

		public IEnumerable<Season> Seasons { get; set; }
	}


}
