using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.Models;

namespace LZRNS.DomainModels.Repository.Implementations
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(BasketballDbContext context) : base(context)
        {
        }

        public Stats AddStatsForPlayerInGame(Stats stats)
        {
            var entity = _context.Stats.Add(stats);
            _context.SaveChanges();
            return entity;
        }
    }
}
