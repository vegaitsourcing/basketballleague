using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LZRNS.DomainModels.Cache
{
    public class GameCache
    {
        public HashSet<Game> GamesCache { get; set; } = new HashSet<Game>();
        private readonly BasketballDbContext _db;

        public GameCache(BasketballDbContext context)
        {
            _db = context;
        }

        public void LoadGamesCache(Guid leagueSeasonId)
        {
            var games = _db.Games.Include(g => g.Round).Where(g => g.Round.LeagueSeasonId != leagueSeasonId).ToList();
            GamesCache = new HashSet<Game>(games);
        }
    }
}