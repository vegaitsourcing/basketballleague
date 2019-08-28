using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System.Linq;
using LZRNS.DomainModel.Context;
using System.Collections.Generic;
using System;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(BasketballDbContext context) : base(context)
        {
        }

        public IEnumerable<Player> FilterPlayers(string q, string fl, bool activeOnly)
        {
            var query = !string.IsNullOrEmpty(q) ? q : string.Empty;
            var searchResult = GetAll(activeOnly).Where(p => p.GetFullName.ToLower().Contains(query.ToLower()));
            
            if(!string.IsNullOrWhiteSpace(fl))
            {
                return searchResult.Where(p => p.Name.ToLower().StartsWith(fl.ToLower()));
            }

            return searchResult;
        }

        public Player GetPlayerByName(string firstName, string lastName, string middleName = "")
        {
            Player player = GetAll().Where(p => p.Name.Equals(firstName) && p.LastName.Equals(lastName) && p.MiddleName.Equals(middleName)).FirstOrDefault();

            return player;
        }

        public IEnumerable<Player> GetAll(bool active)
        {
            var all = GetAll().ToList();

            if(active)
            {
                return all.Where(p => p.PlayersPerSeason
                    .Any(ps => _context.Seasons.OrderByDescending(s => s.SeasonStartYear).First()
                        .LeagueSeasons.Any(ls => ls.Teams.Select(t => t.Id).Any(tid => tid.Equals(ps.Team.Id)))));
            }

            return all;
        }
        /*
        public Player GetlayerByTeamIdAndName(Guid teamId, string firstName, string lastName, string middleName = "")
        {
            Player player  = GetAll().Where(pl => pl.)
            
        }
        */
    }
}
