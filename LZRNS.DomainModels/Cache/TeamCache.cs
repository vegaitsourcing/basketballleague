using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
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

        public Team CreateOrGetTeamByName(string teamName, LeagueSeason leagueSeason)
        {
            string formattedTeamName = FormatTeamName(teamName);
            if (TeamByTeamNameCache.TryGetValue(formattedTeamName, out var team))
            {
                return team;
            }
            team = CreateTeam(teamName, leagueSeason);
            TeamByTeamNameCache.Add(formattedTeamName, team);
            return team;
        }

        private Team CreateTeam(string teamName, LeagueSeason leagueSeason)
        {
            var team = new Team()
            {
                Id = Guid.NewGuid(),
                TeamName = teamName,
                LeagueSeason = leagueSeason
            };

            _db.Teams.Add(team);

            return team;
        }

        private static string FormatTeamName(string teamName)
        {
            return teamName?.ToLower().Trim() ?? "";
        }
    }
}