using LZRNS.Common.Extensions;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LZRNS.DomainModels.Cache
{
    public class LeagueSeasonCache
    {
        private readonly BasketballDbContext _db;

        public LeagueSeasonCache(BasketballDbContext context)
        {
            _db = context;
        }

        public League CurrentLeagueCache { get; set; }
        public LeagueSeason CurrentLeagueSeasonCache { get; set; }
        public Season CurrentSeasonCache { get; set; }

        /// <summary>
        /// Creates new LeagueSeason with `seasonName` and `leagueName` if Cache is null otherwise it returns cache value.
        /// </summary>
        public LeagueSeason CreateOrGetCurrentLeagueSeason(string seasonName, string leagueName)
        {
            if (CurrentLeagueSeasonCache != null)
            {
                return CurrentLeagueSeasonCache;
            }

            CurrentSeasonCache = CreateOrGetSeason(seasonName);
            CurrentLeagueCache = CreateOrGetLeague(leagueName);
            CurrentLeagueSeasonCache = CreateOrGetLeagueSeason(CurrentSeasonCache.Id, CurrentLeagueCache.Id);

            return CurrentLeagueSeasonCache;
        }

        public void LoadCurrentLeagueSeasonCache(string seasonName, string leagueName)
        {
            CurrentSeasonCache = GetSeason(seasonName);
            CurrentLeagueCache = GetLeague(leagueName);

            if (CurrentSeasonCache == null || CurrentLeagueCache == null)
            {
                return;
            }

            LoadCurrentLeagueSeasonCache(CurrentSeasonCache.Id, CurrentLeagueCache.Id);
        }

        private League CreateLeague(string leagueName)
        {
            var league = new League()
            {
                Id = Guid.NewGuid(),
                Name = leagueName
            };

            _db.Leagues.Add(league);
            _db.SaveChanges();

            return league;
        }

        private LeagueSeason CreateLeagueSeason(Guid seasonId, Guid leagueId)
        {
            var leagueSeason = new LeagueSeason()
            {
                Id = Guid.NewGuid(),
                SeasonId = seasonId,
                LeagueId = leagueId,
                Rounds = new List<Round>()
            };

            _db.LeagueSeasons.Add(leagueSeason);
            _db.SaveChanges();

            return leagueSeason;
        }

        private League CreateOrGetLeague(string leagueName)
        {
            var league = GetLeague(leagueName);
            return league ?? CreateLeague(leagueName);
        }

        private LeagueSeason CreateOrGetLeagueSeason(Guid seasonId, Guid leagueId)
        {
            var leagueSeason = GetLeagueSeason(seasonId, leagueId);
            return leagueSeason ?? CreateLeagueSeason(seasonId, leagueId);
        }

        private Season CreateOrGetSeason(string seasonName)
        {
            var season = GetSeason(seasonName);
            return season ?? CreateSeason(seasonName);
        }

        private Season CreateSeason(string seasonName)
        {
            var season = new Season()
            {
                Id = Guid.NewGuid(),
                Name = seasonName,
                SeasonStartYear = seasonName.ExtractNumber()
            };

            _db.Seasons.Add(season);
            _db.SaveChanges();

            return season;
        }

        private League GetLeague(string leagueName)
        {
            return _db.Leagues.FirstOrDefault(l => l.Name.Equals(leagueName));
        }

        private LeagueSeason GetLeagueSeason(Guid seasonId, Guid leagueId)
        {
            return _db.LeagueSeasons.Include(ls => ls.Season).Include(ls => ls.League)
                            .FirstOrDefault(ls => seasonId.Equals(ls.SeasonId) && leagueId.Equals(ls.LeagueId));
        }

        private Season GetSeason(string seasonName)
        {
            return _db.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));
        }

        private void LoadCurrentLeagueSeasonCache(Guid seasonId, Guid leagueId)
        {
            CurrentLeagueSeasonCache = GetLeagueSeason(seasonId, leagueId);
        }
    }
}