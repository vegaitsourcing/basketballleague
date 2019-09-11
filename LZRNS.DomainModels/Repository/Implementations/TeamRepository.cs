using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(BasketballDbContext context) : base(context)
        {
        }

        public PlayerPerTeam AddPlayerToTeam(Guid playerId, Guid teamId)
        {
            var playerInTeam = new PlayerPerTeam
            {
                Id = Guid.NewGuid(),
                Player = _context.Players.Find(playerId),
                Team = _context.Teams.Find(teamId)
            };

            var entity = _context.PlayersPerTeam.Add(playerInTeam);
            _context.SaveChanges();

            return entity;
        }

        public void DeletePlayerFromTeam(Guid teamMemberId)
        {
            var playerInTeam = _context.PlayersPerTeam.Find(teamMemberId);

            if (playerInTeam == null)
            {
                Trace.TraceWarning($"Team member with id: {teamMemberId} not found!");
                return;
            }

            _context.PlayersPerTeam.Remove(playerInTeam);
            _context.SaveChanges();
        }

        public Team FindTeam(string teamName, string seasonName)
        {
            return GetAll().FirstOrDefault(t => t.TeamName.Equals(teamName) && t.LeagueSeason.Season.Name.Equals(seasonName));
        }

        public IEnumerable<Team> GetTeamsByLeagueSeasonId(Guid leagueSeasonId)
        {
            return GetAll().Where(t => t.LeagueSeasonId == leagueSeasonId);
        }
    }
}