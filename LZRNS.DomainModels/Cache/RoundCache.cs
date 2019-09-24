using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Cache
{
    public class RoundCache
    {
        public Dictionary<string, Round> RoundByRoundNameCache { get; set; } = new Dictionary<string, Round>();

        private readonly BasketballDbContext _db;

        public RoundCache(BasketballDbContext context)
        {
            _db = context;
        }

        public void LoadRoundByRoundNameCache(Guid leagueSeasonId)
        {
            var rounds = _db.Rounds.Where(r => r.LeagueSeasonId.Equals(leagueSeasonId));
            RoundByRoundNameCache = rounds.ToDictionary(keySelector: round => FormatRoundName(round.RoundName));
        }

        public Round CreateOrGetRound(Guid leagueSeasonId, string roundName)
        {
            string formattedRoundName = FormatRoundName(roundName);
            RoundByRoundNameCache.TryGetValue(formattedRoundName, out var round);

            if (round != null)
            {
                return round;
            }

            round = new Round()
            {
                Id = Guid.NewGuid(),
                RoundName = formattedRoundName,
                LeagueSeasonId = leagueSeasonId,
                Games = new List<Game>()
            };

            RoundByRoundNameCache.Add(formattedRoundName, round);

            _db.Rounds.Add(round);

            return round;
        }

        private static string FormatRoundName(string roundName)
        {
            return roundName.Contains("GAME") && roundName.Length >= 5 ? roundName.Substring(4) : roundName;
        }
    }
}