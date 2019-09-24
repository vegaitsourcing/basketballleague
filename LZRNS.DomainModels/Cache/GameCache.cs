using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
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

        public Game CreateOrGetGame(Season season, Round round, Team teamA, Team teamB, DateTime gameDate)
        {
            var formattedGameDate = FormatGameDate(gameDate);

            var game = GamesCache.FirstOrDefault(g => GetGameComparison(g, season, round, teamA, teamB, formattedGameDate));

            if (game != null)
            {
                return game;
            }

            game = new Game()
            {
                Id = Guid.NewGuid(),
                Season = season,
                Round = round,
                TeamA = teamA,
                TeamB = teamB,
                DateTime = formattedGameDate
            };

            GamesCache.Add(game);

            _db.Games.Add(game);

            return game;
        }

        public IEnumerable<Game> GetGamesForTeam(Team team)
        {
            return GamesCache.Where(g => AreEqualById(g.TeamA, team) || AreEqualById(g.TeamB, team));
        }

        private static DateTime FormatGameDate(DateTime gameDateTime)
        {
            //if date time format is bad and initial datetime value is set, it will produce SQL exception, so change it
            var defaultDate = new DateTime(1, 1, 1, 0, 0, 0);
            if (DateTime.Compare(gameDateTime, defaultDate) == 0)
            {
                gameDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            }

            return gameDateTime;
        }

        private static bool GetGameComparison(Game game, Season season, Round round, Team teamA, Team teamB, DateTime gameDateTime)
        {
            return AreEqualById(game.Season, season)
                   && AreEqualById(game.Round, round)
                   && AreInSameGame(teamA, teamB, game)
                   && DateTime.Compare(game.DateTime, gameDateTime) == 0;
        }

        private static bool AreEqualById(Round lhs, Round rhs)
        {
            return lhs?.Id.Equals(rhs?.Id) == true;
        }

        private static bool AreEqualById(Season lhs, Season rhs)
        {
            return lhs?.Id.Equals(rhs?.Id) == true;
        }

        private static bool AreEqualById(Team lhs, Team rhs)
        {
            return lhs?.Id.Equals(rhs?.Id) == true;
        }

        private static bool AreInSameGame(Team teamA, Team teamB, Game g)
        {
            return (AreEqualById(g.TeamA, teamA) && AreEqualById(g.TeamB, teamB))
                   || (AreEqualById(g.TeamA, teamB) && AreEqualById(g.TeamB, teamA));
        }
    }
}