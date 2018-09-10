using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.ViewModels
{
    public class TeamHistory
    {
        ICollection<TeamHistory> TeamHistories { get; set; }

        Team Team { get; set; }
    }
}
