using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface ITeamRepository : IRepositoryBase<Team>
    {
        PlayerPerTeam AddPlayerToTeam(Player player, Team team);

        Team FindTeam(string teamName, string seasonName);

    }
}
