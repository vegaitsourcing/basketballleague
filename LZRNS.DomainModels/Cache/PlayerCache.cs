using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.ExcelLoaderModels;
using LZRNS.DomainModels.Models;
using System;
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

        public Player CreateOrGetPlayer(PlayerInfo info)
        {
            if (PlayerByUIdCache.TryGetValue(info.UId, out var player))
            {
                return player;
            }

            player = new Player()
            {
                Id = Guid.NewGuid(),
                Name = DefaultIfNullOrWhiteSpace(info.FirstName),
                MiddleName = DefaultIfNullOrWhiteSpace(info.MiddleName),
                LastName = DefaultIfNullOrWhiteSpace(info.LastName),
                UId = info.UId,
                Stats = new List<Stats>(),
            };

            PlayerByUIdCache.Add(player.UId, player);

            _db.Players.Add(player);

            return player;
        }

        private static string DefaultIfNullOrWhiteSpace(string str, string @default = "?")
        {
            return string.IsNullOrWhiteSpace(str) ? @default : str;
        }
    }
}