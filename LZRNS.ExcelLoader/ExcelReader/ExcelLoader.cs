using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class ExcelLoader : AbstractExcelLoader
    {
        #region Private Fields
        // key represent team name
        private Dictionary<string, TeamStatistic> teams;

        // key represent player full name
        private Dictionary<string, List<PlayerScore>> playerScores;
        #endregion Private Fields

        #region Properies 

        public Dictionary<string, TeamStatistic> Teams
        {
            get { return teams; }
        }

        public Dictionary<string, List<PlayerScore>> PlayerScores
        {
            get { return playerScores; }
        }

        #endregion Properies


        #region Constructors
        public ExcelLoader(string configPath) : base(configPath)
        {
            Loger.log.Debug("Start main proces");
            teams = new Dictionary<string, TeamStatistic>();
            playerScores = new Dictionary<string, List<PlayerScore>>();

        }
        #endregion Constructors

        #region Public Methods
        public List<PlayerScore> GetPlayerScoreList(string playerFullName)
        {
            List<PlayerScore> scores;
            PlayerScores.TryGetValue(playerFullName, out scores);

            return scores;
        }

        #endregion Public Methods

        #region Private Methods
        private void ProcessSheet(IXLWorksheet sheet, ref TeamStatistic teamStatistic, out bool isEmptyPage)
        {
            Loger.log.Debug("ProcessSheet started for table: " + sheet.Name);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            isEmptyPage = false;

            IXLRows rows = sheet.RowsUsed();

            TeamScore teamScore = new TeamScore();
            teamScore.RoundName = sheet.Name;
            int currentRowNo;

            IEnumerable<FieldItem> globalFields = Mapper.Fields.FindAll(i => i.GlobalField == true);
            currentRowNo = Mapper.Fields.First().RowIndex;

            // we are incrasing rowNo because currentRow reprenset row where is header placed!
            if (CheckIfPageIsEmty(rows, currentRowNo + 1, Mapper.Fields.First().ColumnIndex))
            {
                Loger.log.Debug("ProcessSheet: Sheet: " + sheet.Name + ", is empty for Team: " + teamStatistic.TeamName);
                isEmptyPage = true;
                return;
            }

            // we are incrasing rowNo because currentRow reprenset row where is header placed!
            PopulateModelField(teamScore, rows, globalFields, currentRowNo + 1);

            // When we start to load data for each player, we must take row number of headers and then increase it for 1
            currentRowNo = Mapper.Fields.First().RowIndex;
            IEnumerable<FieldItem> otherFields = Mapper.Fields.FindAll(i => i.GlobalField == false);
            int playerCount = CalculatePlayerCount(rows, currentRowNo, otherFields.First().ColumnIndex, MaxPlayerPerMatch);

            for (int i = 1; i < playerCount; i++)
            {
                currentRowNo++;
                PlayerScore pl = new PlayerScore();
                PopulateModelField(pl, rows, otherFields, currentRowNo);
                /*contains list of all players score*/
                teamScore.AddPlayerScore(pl);

                AddPlayerScore(pl);
                AddPlayerInTeam(teamStatistic.TeamName, pl.NameAndLastName);
            }

            Loger.log.Debug("ProcessSheet: ENDED for sheet: " + sheet.Name + ", timeElapsed: " + stopwatch.Elapsed);

            stopwatch.Stop();
            teamStatistic.AddTeamScore(teamScore);

        }

        private void AddPlayerScore(PlayerScore ps)
        {
            List<PlayerScore> list;
            if (!ps.NameAndLastName.Equals(String.Empty))
            {
                if (playerScores.TryGetValue(ps.NameAndLastName, out list))
                {
                    list.Add(ps);
                }
                else
                {
                    playerScores[ps.NameAndLastName] = new List<PlayerScore>() { ps };
                }
            }
        }

        public override void Load(XLWorkbook exApp, string fileName)
        {
            string teamName = CheckFileStructure(exApp, fileName);
            int currentSheetNo = 0;

            TeamStatistic teamStatistic = new TeamStatistic(teamName);
            bool isPageEmpty;

            foreach (IXLWorksheet sheet in Sheets)
            {
                // for odd sheet we want to skip loading
                if (currentSheetNo % 2 == 0)
                {
                    ///process sheet by sheet
                    ProcessSheet(sheet, ref teamStatistic, out isPageEmpty);

                }
                currentSheetNo++;
            }

            teams[teamStatistic.TeamName] = teamStatistic;
        }

        #endregion Private Methods




    }
}
