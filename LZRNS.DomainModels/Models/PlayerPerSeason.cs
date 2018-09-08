using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.Models
{
    public class PlayerPerSeason : AbstractModel
    {
        public virtual Player Player { get; set; }

        public Guid PlayerId { get; set; }

        public virtual Team Team { get; set; }

        public Guid TeamId { get; set; }
    }
}
