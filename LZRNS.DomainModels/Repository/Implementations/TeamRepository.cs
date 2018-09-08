using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.Models;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(BasketballDbContext context) : base(context)
        {
        }

        public PlayerPerTeam AddPlayerToTeam(Player player, Team team)
        {
            var playerInTeam = new PlayerPerTeam { Player = player, Team = team };
            var entity = _context.PlayersPerTeam.Add(playerInTeam);
            _context.SaveChanges();
            return entity;
        }
    }
}
