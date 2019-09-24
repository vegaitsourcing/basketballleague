using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace LZRNS.DomainModels.Cache
{
    public class LeagueSeasonDataCache
    {
        public League CurrentLeagueCache { get; set; }
        public LeagueSeason CurrentLeagueSeasonCache { get; set; }
        public Season CurrentSeasonCache { get; set; }
        public GameCache GameCache { get; set; }
        public PlayerCache PlayerCache { get; set; }
        public PlayerPerTeamCache PlayerPerTeamCache { get; set; }
        public RoundCache RoundCache { get; set; }
        public StatsCache StatsCache { get; set; }
        public TeamCache TeamCache { get; set; }

        private readonly BasketballDbContext _db;

        public LeagueSeasonDataCache(BasketballDbContext context)
        {
            _db = context;
            GameCache = new GameCache(_db);
            PlayerCache = new PlayerCache(_db);
            PlayerPerTeamCache = new PlayerPerTeamCache(_db);
            RoundCache = new RoundCache(_db);
            StatsCache = new StatsCache(_db);
            TeamCache = new TeamCache(_db);
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public void LoadDataToCache(string seasonName, string leagueName)
        {
            CurrentSeasonCache = _db.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));
            CurrentLeagueCache = _db.Leagues.FirstOrDefault(l => l.Name.Equals(leagueName));

            if (CurrentSeasonCache == null || CurrentLeagueCache == null)
            {
                return;
            }

            LoadCurrentLeagueSeasonCache(CurrentSeasonCache.Id, CurrentLeagueCache.Id);

            if (CurrentLeagueSeasonCache == null)
            {
                return;
            }

            var leagueSeasonId = CurrentLeagueSeasonCache.Id;

            GameCache.LoadGamesCache(leagueSeasonId);
            LoadPlayerRelatedCache(leagueSeasonId);
            RoundCache.LoadRoundByRoundNameCache(leagueSeasonId);
            TeamCache.LoadTeamByTeamNameCache(leagueSeasonId);
        }

        private void LoadCurrentLeagueSeasonCache(Guid seasonId, Guid leagueId)
        {
            CurrentLeagueSeasonCache = _db.LeagueSeasons.Include(ls => ls.Season).Include(ls => ls.League)
                .FirstOrDefault(ls => seasonId.Equals(ls.SeasonId) && leagueId.Equals(ls.LeagueId));
        }

        private void LoadPlayerRelatedCache(Guid leagueSeasonId)
        {
            PlayerPerTeamCache.LoadPlayersPerTeamCache(leagueSeasonId);

            PlayerCache.LoadPlayerCache();

            var playerIds = PlayerCache.PlayerByUIdCache.Values.Select(player => player.Id).Distinct().ToArray();
            StatsCache.LoadPlayerStatsCache(playerIds);
        }
    }
}