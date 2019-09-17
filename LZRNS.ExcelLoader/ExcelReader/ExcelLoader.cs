using ClosedXML.Excel;
using System.Collections.Generic;
using System.Diagnostics;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class ExcelLoader : AbstractExcelLoader
    {
        public Dictionary<string, TeamStatistic> Teams { get; }

        public Dictionary<string, Dictionary<string, List<PlayerScore>>> PlayerScores { get; }

        public ExcelLoader(string configPath) : base(configPath)
        {
            Loger.log.Debug("Start main process");
            Teams = new Dictionary<string, TeamStatistic>();
            PlayerScores = new Dictionary<string, Dictionary<string, List<PlayerScore>>>();
        }

        public Dictionary<string, List<PlayerScore>> GetTeamScores(string teamName)
        {
            PlayerScores.TryGetValue(teamName, out var teamScores);
            return teamScores ?? new Dictionary<string, List<PlayerScore>>();
        }

        public List<PlayerScore> GetPlayerScoreList(string teamName, string playerFullName)
        {
            var teamScores = GetTeamScores(teamName);
            teamScores.TryGetValue(playerFullName, out var playerScores);
            return playerScores ?? new List<PlayerScore>();
        }

        private void ProcessSheet(IXLWorksheet sheet, ref TeamStatistic teamStatistic)
        {
            Loger.log.Debug("ProcessSheet started for table: " + sheet.Name);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

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
                Loger.log.Debug("ProcessSheet: Sheet: " + sheet.Name + ", is empty for Team: " + teamStatistic.TeamName);
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

            stopwatch.Stop();
            Loger.log.Debug("ProcessSheet: ENDED for sheet: " + sheet.Name + ", timeElapsed: " + stopwatch.Elapsed);
        }

        private void AddPlayerScore(string teamName, PlayerScore ps)
        {
            if (teamName.Equals(string.Empty)) return;

            if (!PlayerScores.TryGetValue(teamName, out var teamScore))
            {
                var playerScoreList = new List<PlayerScore> { ps };
                var scoresDictionary = new Dictionary<string, List<PlayerScore>> { { ps.NameAndLastName, playerScoreList } };
                PlayerScores.Add(teamName, scoresDictionary);
                return;
            }

            if (ps.NameAndLastName.Equals(string.Empty)) return;

            if (teamScore.TryGetValue(ps.NameAndLastName, out var list))
            {
                list.Add(ps);
            }
            else
            {
                PlayerScores[teamName].Add(ps.NameAndLastName, new List<PlayerScore> { ps });
            }
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

            Teams[teamStatistic.TeamName] = teamStatistic;
        }
    }
}