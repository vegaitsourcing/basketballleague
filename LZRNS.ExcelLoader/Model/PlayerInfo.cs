using System.Linq;

namespace LZRNS.ExcelLoader.Model
{
    public class PlayerInfo
    {
        public string FirstName => string.IsNullOrEmpty(NameAndLastName) ? string.Empty : NameAndLastName.Split(' ').First();
        public string LastName => string.IsNullOrEmpty(NameAndLastName) ? string.Empty : NameAndLastName.Split(' ').Last();

        public string MiddleName
        {
            get
            {
                if (string.IsNullOrEmpty(NameAndLastName))
                {
                    return string.Empty;
                }

                var names = NameAndLastName.Split(' ');
                return names.Length == 3 ? names[1] : string.Empty;
            }
        }

        public string NameAndLastName { get; set; } = "-";
        public string NewLeagueSeasonName { get; set; }
        public string NewTeamName { get; set; }
        public int Number { get; set; }
        public bool OnLoan { get; set; }
        public string PreviousLeagueSeasonName { get; set; }
        public string PreviousTeamName { get; set; }
        public string UId { get; set; }
    }
}