using LZRNS.DomainModel.Context;
using System;
using System.Linq;

namespace LZRNS.DomainModels.Cache
{
    public class LeagueSeasonDataCache
    {
        private readonly BasketballDbContext _db;

        public LeagueSeasonDataCache(BasketballDbContext context)
        {
            _db = context;
            LeagueSeasonCache = new LeagueSeasonCache(_db);
            GameCache = new GameCache(_db);
            PlayerCache = new PlayerCache(_db);
            PlayerPerTeamCache = new PlayerPerTeamCache(_db);
            RoundCache = new RoundCache(_db);
            StatsCache = new StatsCache(_db);
            TeamCache = new TeamCache(_db);
        }

        public GameCache GameCache { get; set; }
        public LeagueSeasonCache LeagueSeasonCache { get; set; }
        public PlayerCache PlayerCache { get; set; }
        public PlayerPerTeamCache PlayerPerTeamCache { get; set; }
        public RoundCache RoundCache { get; set; }
        public StatsCache StatsCache { get; set; }
        public TeamCache TeamCache { get; set; }

        public void LoadDataToCache(string seasonName, string leagueName)
        {
            LoadPlayerAndStatsCache();

            LeagueSeasonCache.LoadCurrentLeagueSeasonCache(seasonName, leagueName);

            if (LeagueSeasonCache.CurrentLeagueSeasonCache == null)
            {
                return;
            }

            var leagueSeasonId = LeagueSeasonCache.CurrentLeagueSeasonCache.Id;
            GameCache.LoadGamesCache(leagueSeasonId);
            RoundCache.LoadRoundByRoundNameCache(leagueSeasonId);
            TeamCache.LoadTeamByTeamNameCache(leagueSeasonId);
            PlayerPerTeamCache.LoadPlayersPerTeamCache(leagueSeasonId);
        }

        private void LoadPlayerAndStatsCache()
        {
            PlayerCache.LoadPlayerCache();
            var playerIds = PlayerCache.PlayerByUIdCache.Values.Select(player => player.Id).Distinct().ToArray();
            StatsCache.LoadPlayerStatsCache(playerIds);
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }
    }
}