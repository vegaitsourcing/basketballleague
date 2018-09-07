using System;
using System.Collections.Generic;
using System.Text;

namespace LZRNS.DomainModel.Models
{
    public class League: AbstractModel
    {
        public string Name { get; set; }

        public List<Season> Seasons { get; set; }
    }
}
