using System;
using System.Linq;

namespace LZRNS.ExcelLoader
{
    public class PlayerScore
    {
        # region Private Fields
        private string nameAndLastName = "-";
        #endregion Private Fields

        public int Number { get; set; }
        public String NameAndLastName
        {
            get
            {
                return nameAndLastName;
            }
            set
            {
                nameAndLastName = value;
            }
        }

        public int Minutes { get; set; }
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
        public string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(NameAndLastName))
                {
                    return String.Empty;
                }
                return NameAndLastName.Split(' ').First();
            }
        }
        public string LastName
        {
            get
            {
                if (string.IsNullOrEmpty(NameAndLastName))
                {
                    return String.Empty;
                }
                return NameAndLastName.Split(' ').Last();
            }
        }
        public string MiddleName
        {
            get
            {
                if (string.IsNullOrEmpty(NameAndLastName))
                {
                    return String.Empty;
                }

                string[] names = NameAndLastName.Split(' ');
                if (names.Count() == 3)
                {
                    return names[1];
                }

                return string.Empty;
            }
        }

        public PlayerScore() { }
    }
}
