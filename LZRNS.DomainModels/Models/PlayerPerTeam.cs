using LZRNS.DomainModel.Models;
using System;

namespace LZRNS.DomainModels.Models
{
    public class PlayerPerTeam : AbstractModel
    {
        public virtual Player Player { get; set; }

        public Guid PlayerId { get; set; }

        public virtual Team Team { get; set; }

        public Guid TeamId { get; set; }
    }
}
