using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
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
            var league = new League { Name = leagueName };
            Add(league);
            return league;
        }

        public void GenerateSchedule(LeagueSeason leagueSeason)
        {
            throw new NotImplementedException();
        }

        public IQueryable<League> GetLeaguesByName(string leagueName)
        {
            return Context.Leagues.Where(l => l.Name == leagueName);
        }
    }
}