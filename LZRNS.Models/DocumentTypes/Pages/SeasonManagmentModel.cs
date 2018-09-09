using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
    public class SeasonManagmentModel : PageModel
    {
        public IPublishedContent StatsManagmentPicker => this.GetCachedValue(() => Content.GetPropertyValue<IPublishedContent>());
      
        public IPublishedContent ScheduleManagmentPicker => this.GetCachedValue(() => Content.GetPropertyValue<IPublishedContent>());
    }


}
