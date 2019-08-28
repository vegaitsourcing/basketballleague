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

		public Season GetSeasonByYear(int seasonStartYear)
		{
			return _context.Seasons.First(x => x.SeasonStartYear.Equals(seasonStartYear));
		}

		public LeagueSeason AddLeagueToSeason(LeagueSeason leagueSeason)
		{
			leagueSeason.Id = Guid.NewGuid();
			var entity = _context.LeagueSeasons.Add(leagueSeason);
			_context.SaveChanges();
			return entity;
		}

		public bool UpdateLeagueSeason(LeagueSeason leagueSeason)
		{
			try
			{
				_context.Entry(leagueSeason).State = EntityState.Modified;
				_context.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public IEnumerable<LeagueSeason> GetAllLeagueSeasons()
		{
			return _context.Set<LeagueSeason>();
		}

		public LeagueSeason GetLeagueSeasonById(Guid id)
		{
			return _context.Set<LeagueSeason>().Find(id);
		}

         
        public Season GetSeasonByName(string seasonName)
        {
            return _context.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));
        }

        public void DeleteLeagueSeason(LeagueSeason leagueSeason)
		{
				_context.Set<LeagueSeason>().Remove(leagueSeason);
				_context.SaveChanges();
		}

        public LeagueSeason GetLeagueSeasonsBySeasonAndLeague(Guid seasonId , ICollection<Guid> leaguesIds)
        {
            return _context.LeagueSeasons.Where(ls => ls.SeasonId == seasonId && leaguesIds.Contains(ls.LeagueId)).FirstOrDefault();
        }
    }
}
