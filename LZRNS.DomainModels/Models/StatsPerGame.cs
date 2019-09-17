using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Models
{
    public class StatsPerGame
    {
        public StatsPerGame(Guid gameId, Team team)
        {
            Team = team;
            GameId = gameId;
        }

        public virtual Team Team { get; }
        public virtual Guid GameId { get; }

        #region Points

        public int Pts
        {
            get
            {
                return Team.PlayersPerSeason
                    .Where(x => x.Player.Stats.Any(y => y.GameId == GameId))
                    .Select(z => z.Player.Stats)
                    .Sum(k => k.First(s => s.GameId == GameId).Pts);
            }
        }

        private IEnumerable<Stats> PlayerStats => Team.PlayersPerSeason
            .Where(x => x.Player.Stats
                .Any(y => y.GameId == GameId))
            .SelectMany(z => z.Player.Stats)
            .Where(z => z.GameId == GameId);

        public int TwoPtA =>
            PlayerStats.Sum(k => k.TwoPtA);

        public int TwoPtM =>
            PlayerStats.Sum(k => k.TwoPtMade);

        public double TwoPtPercA => Math.Round((double)TwoPtM / TwoPtA * 100, 1);

        public int ThreePtA =>
            PlayerStats.Sum(k => k.ThreePtA);

        public int ThreePtM =>
            PlayerStats.Sum(k => k.ThreePtMade);

        public double ThreePtPer => Math.Round((double)ThreePtM / ThreePtA * 100, 1);

        public int FtA =>
            PlayerStats.Sum(k => k.FtA);

        public int FtM =>
            PlayerStats.Sum(k => k.FtMade);

        public double FtPerc =>
            Math.Round((double)FtM / FtA * 100, 1);

        public int FgA =>
            TwoPtA + ThreePtA;

        public int FgM =>
            TwoPtM + ThreePtM;

        #endregion Points

        #region Rebounds

        public int Reb =>
            PlayerStats.Sum(k => k.Reb);

        public int OReb =>
            PlayerStats.Sum(k => k.OReb);

        public int DReb =>
            PlayerStats.Sum(k => k.DReb);

        #endregion Rebounds

        // Assists
        public int Ast =>
            PlayerStats.Sum(k => k.Ast);

        public int To =>
            PlayerStats.Sum(k => k.To);

        // Steals
        public int Stl =>
            PlayerStats.Sum(k => k.Stl);

        // Blocks
        public int Blk =>
            PlayerStats.Sum(k => k.Blk);

        public int Min =>
            PlayerStats.Sum(k => k.MinutesPlayed);

        public int Eff =>
            PlayerStats.Sum(k => k.Eff);
    }
}