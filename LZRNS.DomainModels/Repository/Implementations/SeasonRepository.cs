using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class SeasonRepository : RepositoryBase<Season>, ISeasonRepository
    {
        public SeasonRepository(BasketballDbContext context) : base(context)
        {
        }

        public LeagueSeason AddLeagueToSeason(LeagueSeason leagueSeason)
        {
            leagueSeason.Id = Guid.NewGuid();
            var entity = Context.LeagueSeasons.Add(leagueSeason);
            Context.SaveChanges();
            return entity;
        }

        public void DeleteLeagueSeason(LeagueSeason leagueSeason)
        {
            Context.Set<LeagueSeason>().Remove(leagueSeason);
            Context.SaveChanges();
        }

        public IEnumerable<LeagueSeason> GetAllLeagueSeasons()
        {
            return Context.Set<LeagueSeason>();
        }

        public LeagueSeason GetLeagueSeasonById(Guid id)
        {
            return Context.Set<LeagueSeason>().Find(id);
        }

        public LeagueSeason GetLeagueSeasonsBySeasonAndLeague(Guid seasonId, ICollection<Guid> leaguesIds)
        {
            return Context.LeagueSeasons.FirstOrDefault(ls => ls.SeasonId == seasonId && leaguesIds.Contains(ls.LeagueId));
        }

        public Season GetSeasonByName(string seasonName)
        {
            return Context.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));
        }

        public Season GetSeasonByYear(int seasonStartYear)
        {
            return Context.Seasons.FirstOrDefault(x => x.SeasonStartYear.Equals(seasonStartYear));
        }

        public bool UpdateLeagueSeason(LeagueSeason leagueSeason)
        {
            try
            {
                Context.Entry(leagueSeason).State = EntityState.Modified;
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}