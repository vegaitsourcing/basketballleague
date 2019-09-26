using ClosedXML.Excel;
using LZRNS.DomainModels.ExcelLoaderModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class ExcelLoader : AbstractExcelLoader
    {
        private readonly Dictionary<string, Dictionary<string, List<PlayerScore>>> _playerScoresByPlayerNameByTeamName;

        public ExcelLoader(string configPath) : base(configPath)
        {
            TeamStatisticByTeamName = new Dictionary<string, TeamStatistic>();
            _playerScoresByPlayerNameByTeamName = new Dictionary<string, Dictionary<string, List<PlayerScore>>>();
            PlayerScores = new List<PlayerScore>();
        }

        public List<PlayerScore> PlayerScores { get; }
        public Dictionary<string, TeamStatistic> TeamStatisticByTeamName { get; }

        public List<PlayerScore> GetPlayerScoreList(string teamName, string playerFullName)
        {
            var teamScores = GetTeamScores(teamName);
            string playerName = FormatPlayerName(playerFullName);
            teamScores.TryGetValue(playerName, out var playerScores);
            return playerScores ?? new List<PlayerScore>();
        }

        public Dictionary<string, List<PlayerScore>> GetTeamScores(string teamName)
        {
            _playerScoresByPlayerNameByTeamName.TryGetValue(teamName, out var teamScores);
            return teamScores ?? new Dictionary<string, List<PlayerScore>>();
        }

        public override void Load(XLWorkbook exApp, string fileName)
        {
            string teamName = CheckFileStructureAndExtractTeamName(exApp, fileName);
            int currentSheetNo = 0;

            var teamStatistic = new TeamStatistic(teamName);

            foreach (var sheet in Sheets)
            {
                if (currentSheetNo % 2 == 0)
                {
                    ProcessSheet(sheet, ref teamStatistic);
                }
                currentSheetNo++;
            }

            TeamStatisticByTeamName[teamStatistic.TeamName] = teamStatistic;
        }

        private static string FormatPlayerName(string playerName)
        {
            string lower = playerName.Trim().ToLower();
            return Regex.Replace(lower, @"\W+", "-");
        }

        private void AddPlayerScore(string teamName, PlayerScore ps)
        {
            if (teamName.Equals(string.Empty)) return;
            string playerName = FormatPlayerName(ps.NameAndLastName);

            PlayerScores.Add(ps);

            if (!_playerScoresByPlayerNameByTeamName.TryGetValue(teamName, out var teamScore))
            {
                var playerScoreList = new List<PlayerScore> { ps };
                var scoresDictionary = new Dictionary<string, List<PlayerScore>> { { playerName, playerScoreList } };
                _playerScoresByPlayerNameByTeamName.Add(teamName, scoresDictionary);
                return;
            }

            if (playerName.Equals(string.Empty)) return;

            if (teamScore.TryGetValue(playerName, out var list))
            {
                list.Add(ps);
            }
            else
            {
                _playerScoresByPlayerNameByTeamName[teamName].Add(playerName, new List<PlayerScore> { ps });
            }
        }

        private void ProcessSheet(IXLWorksheet sheet, ref TeamStatistic teamStatistic)
        {
            var rows = sheet.RowsUsed();

            var teamScore = new TeamScore
            {
                RoundName = sheet.Name
            };

            IEnumerable<FieldItem> globalFields = Mapper.Fields.FindAll(i => i?.GlobalField == true);
            int currentRowNo = Mapper.Fields[0].RowIndex;
            int headerRowNo = currentRowNo + 1;
            if (CheckIfPageIsEmpty(rows, headerRowNo, Mapper.Fields[0].ColumnIndex))
            {
                return;
            }

            PopulateModelField(teamScore, rows, globalFields, headerRowNo);

            var otherFields = Mapper.Fields.FindAll(i => i?.GlobalField == false);
            int playerCount = CalculatePlayerCount(rows, currentRowNo, otherFields[0].ColumnIndex, MaxPlayerPerMatch);

            for (int i = 1; i < playerCount; i++)
            {
                currentRowNo++;
                var pl = new PlayerScore();
                PopulateModelField(pl, rows, otherFields, currentRowNo);
                pl.AgainstTeam = teamScore.AgainstTeam;
                teamScore.AddPlayerScore(pl);

                AddPlayerScore(teamStatistic.TeamName, pl);
                AddPlayerInTeam(teamStatistic.TeamName, pl.NameAndLastName);
            }

            teamStatistic.AddTeamScore(teamScore);
        }
    }
}