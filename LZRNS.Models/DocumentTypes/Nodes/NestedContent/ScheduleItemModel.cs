using System;
using System.Collections.Generic;
using System.Linq;
using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent
{
	public class ScheduleItemModel : NestedContentBaseModel
	{
		public ScheduleItemModel(IPublishedContent content) : base(content)
		{
		}

		public DateTime FirstDate => this.GetPropertyValue<DateTime>();
		public DateTime SecondDate => this.GetPropertyValue<DateTime>();
		public string RoundName => this.GetPropertyValue<string>();
		public IEnumerable<DateTime> FirstDateRange => this.GetCachedValue(() => GetDateRange(FirstDate, true));
		public IEnumerable<DateTime> SecondDateRange => this.GetCachedValue(() => GetDateRange(SecondDate, false));

		private IEnumerable<DateTime> GetDateRange(DateTime date, bool dateLast)
		{
			return Enumerable.Range(0, 1 + (date.AddDays(4) - date).Days)
				.Select(offset => dateLast ? date.AddDays(offset - 4) : date.AddDays(offset));
		}
	}
}
