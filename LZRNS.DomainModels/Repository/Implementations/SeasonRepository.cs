using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;

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

		public void DeleteLeagueSeason(LeagueSeason leagueSeason)
		{
				_context.Set<LeagueSeason>().Remove(leagueSeason);
				_context.SaveChanges();
		}
	}
}
