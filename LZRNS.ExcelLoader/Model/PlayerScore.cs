using System.Linq;

namespace LZRNS.ExcelLoader
{
    public class PlayerScore
    {
        public string AgainstTeam { get; set; }
        public int Assistance { get; set; }
        public int Block { get; set; }
        public int DefensiveReb { get; set; }
        public int FreeThrowsAttempt { get; set; }
        public int FreeThrowsMade { get; set; }
        public string LastName => string.IsNullOrEmpty(NameAndLastName) ? string.Empty : NameAndLastName.Split(' ').Last();
        public int Minutes { get; set; }
        public string NameAndLastName { get; set; } = "-";
        public int Number { get; set; }
        public int OffensiveReb { get; set; }
        public int PointAttempt2 { get; set; }
        public int PointAttempt3 { get; set; }
        public int PointMade2 { get; set; }
        public int PointMade3 { get; set; }
        public int Steal { get; set; }
        public int TurnOver { get; set; }
    }
}