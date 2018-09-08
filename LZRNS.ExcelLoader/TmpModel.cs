using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.ExcelLoader
{
    class TmpModel
    {
        public string Vs { get; set; }
        public string GameDate { get; set; }
        public int Number { get; set; }
        public int NameAndLastName { get; set; }
        public int Min { get; set; }
        public int PointAttempt2 { get; set; }
        public int PointMade2 { get; set; }
        public int PointAttempt3 { get; set; }
        public int PointMade3 { get; set; }
        public int FreeThrowsAttempt { get; set; }
        public int FreeThrowsMade { get; set; }
        public int OffensiveReb { get; set; }
        public int DefensiveReb { get; set; }
        public int Assistance { get; set; }
        public int TurnOver { get; set; }
        public int Steal { get; set; }
        public int Block { get; set; }

        public TmpModel() { }

    }
}
