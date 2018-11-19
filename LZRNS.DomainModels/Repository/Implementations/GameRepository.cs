using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Data.Entity;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
	{
		public GameRepository(BasketballDbContext context) : base(context)
		{
		}

		public Stats AddStatsForPlayerInGame(Stats stats)
		{
			stats.Id = Guid.NewGuid();

			var entity = _context.Stats.Add(stats);

			_context.SaveChanges();

			return entity;
		}

		public void UpdateStatsForPlayerInGame(Stats stats)
		{
			_context.Entry(stats).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public void DeleteStatsForPlayerInGame(Guid gamePlayerId)
		{
			var entity = _context.Stats.Find(gamePlayerId);

			_context.Stats.Remove(entity);

			_context.SaveChanges();

		}
	}
}
