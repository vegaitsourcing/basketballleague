using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Cache
{
    public class TeamCache
    {
        public Dictionary<string, Team> TeamByTeamNameCache { get; set; } = new Dictionary<string, Team>();

        private readonly BasketballDbContext _db;

        public TeamCache(BasketballDbContext context)
        {
            _db = context;
        }

        public void LoadTeamByTeamNameCache(Guid leagueSeasonId)
        {
            var teams = _db.Teams.Where(t => t.LeagueSeasonId.Equals(leagueSeasonId));
            TeamByTeamNameCache = teams.ToDictionary(keySelector: team => FormatTeamName(team.TeamName));
        }

        private static string FormatTeamName(string teamName)
        {
            return teamName?.ToLower().Trim() ?? "";
        }
    }
}