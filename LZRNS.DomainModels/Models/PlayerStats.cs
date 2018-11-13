using System;
using System.Linq;

namespace LZRNS.DomainModels.Models
{
	public abstract class PlayerStats
	{
		private readonly Func<int, double> _calculationFunc;
		private readonly Stats[] _stats;
		
		private double _points;
		private double _assists;
		private double _steals;
		private double _rebounds;
		private double _blocks;
		private double _min;
		private double _eff;
		private double _fg;
		private double _twoPts;
		private double _threePts;
		private double _ft;
		private double _offReb;
		private double _defReb;
		private double _to;

		protected PlayerStats(string playerName, Stats[] stats, Func<int, double> calculationFunc)
		{
			if (playerName == null) throw new ArgumentNullException(nameof(playerName));
			if (stats == null) throw new ArgumentNullException(nameof(stats));
			if (calculationFunc == null) throw new ArgumentNullException(nameof(calculationFunc));

			_calculationFunc = calculationFunc;
			_stats = stats;
			PlayerName = playerName;
		}

		public string PlayerName { get; }

		public double Points => _points > 0 ? _points : (_points = GetValue(s => s.Pts));
		public double Assists => _assists > 0 ? _assists : (_assists = GetValue(s => s.Ast));
		public double Steals => _steals > 0 ? _steals : (_steals = GetValue(s => s.Stl));
		public double Rebounds => _rebounds > 0 ? _rebounds : (_rebounds = GetValue(s => s.Reb));
		public double Blocks => _blocks > 0 ? _blocks : (_blocks = GetValue(s => s.Blk));
		public double MinutesPlayed => _min > 0 ? _min : (_min = GetValue(s => s.MinutesPlayed));
		public double Efficiency => _eff > 0 ? _eff : (_eff = GetValue(s => s.Eff));
		public double FieldGoals => _fg > 0 ? _fg : (_fg = GetValue(s => s.FgM));
		public double TwoPoints => _twoPts > 0 ? _twoPts : (_twoPts = GetValue(s => s.TwoPtMade));
		public double ThreePoints => _threePts > 0 ? _threePts : (_threePts = GetValue(s => s.ThreePtMade));
		public double FreeThrows => _ft > 0 ? _ft : (_ft = GetValue(s => s.FtMade));
		public double OffensiveRebounds => _offReb > 0 ? _offReb : (_offReb = GetValue(s => s.OReb));
		public double DefensiveRebounds => _defReb > 0 ? _defReb : (_defReb = GetValue(s => s.DReb));
		public double To => _to > 0 ? _to : (_to = GetValue(s => s.To));
		
		private double GetValue(Func<Stats, int> property)
		{
			int total = _stats.Sum(property);

			return _calculationFunc.Invoke(total);
		}
	}
}
