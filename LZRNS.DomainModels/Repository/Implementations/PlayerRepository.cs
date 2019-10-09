using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(BasketballDbContext context) : base(context)
        {
        }

        public IEnumerable<Player> FilterPlayers(string q, string fl, bool activeOnly)
        {
            string query = !string.IsNullOrEmpty(q) ? q : string.Empty;
            var searchResult = GetAll(activeOnly).Where(p => p.GetFullName.ToLower().Contains(query.ToLower()));

            if (!string.IsNullOrWhiteSpace(fl))
            {
                return searchResult.Where(p => p.Name.StartsWith(fl, StringComparison.OrdinalIgnoreCase));
            }

            return searchResult;
        }

        public Player GetPlayerByName(string firstName, string lastName, string middleName = "")
        {
            return GetAll().FirstOrDefault(p => p.Name.Equals(firstName) && p.LastName.Equals(lastName) && p.MiddleName.Equals(middleName));
        }

        public IEnumerable<Player> GetAll(bool active)
        {
            var all = GetAll().ToList().OrderBy(p => p.GetFullName);

            if (active)
            {
                return all.Where(p => p.PlayersPerSeason
                    .Any(ps => Context.Seasons.OrderByDescending(s => s.SeasonStartYear).First()
                        .LeagueSeasons.Any(ls => ls.Teams.Select(t => t.Id).Any(tid => tid.Equals(ps.Team.Id))))).OrderBy(p => p.GetFullName);
            }

            return all;
        }
    }
}