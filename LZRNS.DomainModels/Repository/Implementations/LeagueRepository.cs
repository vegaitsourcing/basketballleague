using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModel.Context;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class LeagueRepository : RepositoryBase<League>, ILeagueRepository
    {
        public LeagueRepository(BasketballDbContext context) : base(context)
        {
        }

        public League CreateLeague(string leagueName)
        {
            League league = new League();
            league.Name = leagueName;
            Add(league);
            return league;
        }

        public void GenerateSchedule(LeagueSeason leagueSeason)
        {
            throw new NotImplementedException();
        }

        public IQueryable<League> GetLeaguesByName(string leagueName)
        {
           return _context.Leagues.Where(l => l.Name == leagueName);
        }
    }
}
