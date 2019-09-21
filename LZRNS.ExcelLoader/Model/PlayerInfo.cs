using System.Linq;

namespace LZRNS.ExcelLoader.Model
{
    public class PlayerInfo
    {
        private const string defaultName = "-";
        public string FirstName => string.IsNullOrEmpty(NameAndLastName) ? defaultName : NameAndLastName.Split(' ').First();
        public string LastName => string.IsNullOrEmpty(NameAndLastName) ? defaultName : NameAndLastName.Split(' ').Last();

        public string MiddleName
        {
            get
            {
                if (string.IsNullOrEmpty(NameAndLastName))
                {
                    return defaultName;
                }

                var names = NameAndLastName.Split(' ');
                return names.Length == 3 ? names[1] : defaultName;
            }
        }

        public string NameAndLastName { get; set; } = defaultName;
        public string NewLeagueSeasonName { get; set; }
        public string NewTeamName { get; set; }
        public int Number { get; set; }
        public bool OnLoan { get; set; }
        public string PreviousLeagueSeasonName { get; set; }
        public string PreviousTeamName { get; set; }
        public string UId { get; set; }
    }
}