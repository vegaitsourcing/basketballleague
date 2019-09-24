using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Cache
{
    public class PlayerCache
    {
        private readonly BasketballDbContext _db;

        public PlayerCache(BasketballDbContext context)
        {
            _db = context;
        }

        public Dictionary<string, Player> PlayerByUIdCache { get; set; } = new Dictionary<string, Player>();

        public void LoadPlayerCache()
        {
            PlayerByUIdCache = _db.Players.ToDictionary(keySelector: player => player.UId);
        }
    }
}