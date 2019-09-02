using LZRNS.DomainModel.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LZRNS.DomainModels.Models
{
    public class PlayerPerTeam : AbstractModel
    {
        public virtual Player Player { get; set; }

        public Guid PlayerId { get; set; }

        public virtual Team Team { get; set; }

        public Guid TeamId { get; set; }

        [ForeignKey("LeagueSeason_Id")]
        public virtual LeagueSeason  LeagueSeason { get; set; }
        public Guid? LeagueSeason_Id { get; set; }
    }
}
