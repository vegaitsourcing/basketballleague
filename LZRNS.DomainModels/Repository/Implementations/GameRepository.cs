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

            var entity = Context.Stats.Add(stats);

            Context.SaveChanges();

            return entity;
        }

        public void UpdateStatsForPlayerInGame(Stats stats)
        {
            Context.Entry(stats).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void DeleteStatsForPlayerInGame(Guid gamePlayerId)
        {
            var entity = Context.Stats.Find(gamePlayerId);

            Context.Stats.Remove(entity);

            Context.SaveChanges();
        }
    }
}