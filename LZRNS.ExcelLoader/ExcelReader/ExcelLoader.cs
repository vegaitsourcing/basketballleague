using ClosedXML.Excel;
using LZRNS.ExcelLoader.ExcelReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class ExcelLoader:AbstractExcelLoader
    {
        #region Private Fields
     //   private XLWorkbook exApp;
      //  private IXLWorksheets sheets;

        // key represent team name (I will change that later)
        private Dictionary<string, TeamStatistic> teams;

        // key represent team name, list of player names
        //private Dictionary<string, HashSet<string>> teamAndPlayers;

        // key represent player full name
        private Dictionary<string, List<PlayerScore>> playerStores;

        //key represent team name (unique), the value dictionary should have the following form - season:value, league:value
        //private Dictionary<string, Dictionary<string, string>> teamsSeasonAndLeague;

     //   private string _seasonName;
     //   private string _leagueName;



       // private MapperModel mapper;
        //private int maxPlayerPerMatch = 12;

        #endregion Private Fields

        #region Properies 

        public Dictionary<string, TeamStatistic> Teams
        {
            get { return teams; }
        }

        public Dictionary<string, List<PlayerScore>> PlayerStores
        {
            get { return playerStores; }
        }
        /*
        public Dictionary<string, HashSet<string>> TeamAndPlayers
        {
            get { return teamAndPlayers; }
        }*/

      //  public string SeasonName { get => _seasonName; set => _seasonName = value; }
      //  public string LeagueName { get => _leagueName; set => _leagueName = value; }

        #endregion Properies


        #region Constructors
        public ExcelLoader(string configPath):base(configPath)
        {
            Loger.log.Debug("Start main proces");
            teams = new Dictionary<string, TeamStatistic>();
            playerStores = new Dictionary<string, List<PlayerScore>>();
            //this.teamAndPlayers = new Dictionary<string, HashSet<string>>();
            //mapper = new MapperModel(configPath);

        }
        #endregion Constructors

        #region Public Methods
        public void ProcessFile(String path, string fileName)
        {
            try
            {

                using (ExApp = new XLWorkbook(path))
                {
                    Load(ExApp, fileName);
                }
            }
            finally
            {
                Sheets = null;
                ExApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void ProcessFile(MemoryStream memoryStream, string fileName)
        {
            try
            {
                using (ExApp = new XLWorkbook(memoryStream))
                {
                    Load(ExApp, fileName);
                }
            }
            finally
            {
                Sheets = null;
                ExApp = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public List<PlayerScore> GetPlayerScoreList(string playerFullName)
        {
            List<PlayerScore> scores;
            PlayerStores.TryGetValue(playerFullName, out scores);

            return scores;
        }
        

        
    
        #endregion Public Methods
        //tested
        /*
        private void CheckMappingValidation(MapperModel mapper, IXLWorksheet sheet)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            IXLRows rows = sheet.RowsUsed();
            int currentRowIndex = -1;
            IXLRow row = null;

            foreach (FieldItem item in mapper.Fields)
            {
                if (item.CellName.Equals("")) continue;

                if (currentRowIndex != item.RowIndex)
                {
                    row = rows.ElementAt(item.RowIndex);
                    currentRowIndex = item.RowIndex;
                }

                object value = row.Cell(item.ColumnIndex).Value;

                if (value == null || !value.Equals(item.CellName))
                {

                    Loger.log.Error("Mapping is invalid for sheet: " + sheet.Name);
                    throw new Exception();
                }
            }
            stopwatch.Stop();
            //Loger.log.Debug("CheckMappingValidation Successfully completed for time: " + stopwatch.Elapsed);

        }
        */
        #region Private Methods
        private void ProcessSheet(IXLWorksheet sheet, ref TeamStatistic teamStatistic, out bool isEmptyPage)
        {
            //Loger.log.Debug("ProcessSheet started for table: " + sheet.Name);

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
                //tested
                PopulateModelField(pl, rows, otherFields, currentRowNo);
                /*contains list of all players score*/
                teamScore.AddPlayerScore(pl);

                AddPlayerScore(pl);
                AddPlayerInTeam(teamStatistic.TeamName, pl.NameAndLastName);
                //NOTE:only for debug
                //break;
            }

            Loger.log.Debug("ProcessSheet: ENDED for sheet: " + sheet.Name + ", timeElapsed: " + stopwatch.Elapsed);

            stopwatch.Stop();
            teamStatistic.AddTeamScore(teamScore);

        }
        /*
        private void PopulateModelField(Object modelObj, IXLRows exlRange, IEnumerable<FieldItem> fields, int rowIndex = -1)
        {
            foreach (FieldItem fieldItem in fields)
            {
                PopulateModelValue(modelObj, exlRange, fieldItem, rowIndex);
            }
        }

        private void PopulateModelValue(Object obj, IXLRows exlRange, FieldItem fieldItem, int rowIndex = -1)
        {
            var objProperty = obj.GetType().GetProperty(fieldItem.PropertyName, BindingFlags.Public | BindingFlags.Instance);
            //get the value of the property
            if (objProperty == null)
            {
                Loger.log.Error("PopulateModelValue: PropertyName: " + fieldItem.PropertyName + " not exist in model: " + obj);
                // We should log error (currently I will just throw exception to be aware that model and mapping are not matched)
                throw new Exception();
            }

            // If we do not explcitly send row index or data should be directly read from cell, then it should be used directly from configuration
            // Examlple: matchDate doesn not contain header
            if (fieldItem.DirectCellData == true || rowIndex == -1)
            {
                rowIndex = fieldItem.RowIndex;
            }

            Object value = GetCellValue(exlRange, rowIndex, fieldItem.ColumnIndex);
            if (value == null)
            {
                Loger.log.Error("PopulateModelValue - PropertyName: " + fieldItem.PropertyName + " in NULL");
                return;
            }

            objProperty.SetValue(obj, fieldItem.GetValueConverted(value));

        }

        private Object GetCellValue(IXLRows rows, int rowIndex, int columnIndex)
        {
            if (rows.Count() <= rowIndex)
            {
                return null;
            }
            IXLRow row = rows.ElementAt(rowIndex);
            Object obj = row.Cell(columnIndex).Value;

            return obj;
        }

        /* This method used to calculate number of rows that are populated for players statistic */
        /*
        private int CalculatePlayerCount(IXLRows exlRange, int currentRowNo, int columnIndex, int maxPlayerCount)
        {
            int playersCount = 0;
            for (int i = 0; i < maxPlayerCount; i++)
            {

                if (GetCellValue(exlRange, currentRowNo + i, columnIndex) == null)
                {
                    break;
                }

                playersCount++;
            }

            return playersCount;

        }

        private bool CheckIfPageIsEmty(IXLRows rows, int rowIndex, int colunmIndex)
        {
            bool isEmpty = false;

            if (GetCellValue(rows, rowIndex, colunmIndex) == null)
            {
                isEmpty = true;
            }

            return isEmpty;
        }
        */
        private void AddPlayerScore(PlayerScore ps)
        {
            List<PlayerScore> list;
            if (!ps.NameAndLastName.Equals(String.Empty))
            {
                if (playerStores.TryGetValue(ps.NameAndLastName, out list))
                {
                    list.Add(ps);
                }
                else
                {
                    playerStores[ps.NameAndLastName] = new List<PlayerScore>() { ps };
                }
            }
        }

        public override void Load(XLWorkbook exApp, string fileName)
        {
            /*
          //fileName should have the following format - teamName-seasonName-leagueName.xlsx (at least one file)
          int currentSheetNo = 0;

          //string nameWithExtension = fileName.Split(new string[] { "stats-teams-" }, StringSplitOptions.None).Last();

          string[] nameParts = fileName.Split('-');

          string teamName = nameParts[2];
          if (teamName.Contains(".xlsx"))
          {
              teamName = teamName.Substring(0, teamName.Length - 5);
          }
          //only one file will determine SeasonName and LeagueName - first with appropriate filename structure
          if (nameParts.Length == 5 && SeasonName == null && LeagueName == null)
          {
              SeasonName = nameParts[3];
              LeagueName = nameParts[4].Substring(0, nameParts[4].Length - 5);
          }

          Loger.log.Debug("Loading data for team: " + teamName);

          sheets = exApp.Worksheets;

          //Only for first sheet we want to check validation for mapping fields configuration
          foreach (IXLWorksheet sheet in sheets)
          {
              //check file format/mapping
              CheckMappingValidation(mapper, sheet);
              break;
          }
          */

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
